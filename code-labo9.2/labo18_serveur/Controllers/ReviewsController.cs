using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using serveur16.Data;
using serveur16.DTOs;
using serveur16.Models;
using serveur16.Models.DTOs;

namespace serveur16.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly serveur16Context _context;
        private readonly UserManager<User> _userManager;

        public ReviewsController(serveur16Context context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: api/reviews
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReviewDisplayDTO>>> GetReview()
        {
            var reviews = await _context.Review

                .Include(r => r.Author)

                .Include(r => r.Upvoters).ToListAsync();

            var resultat = reviews.Select(r => new ReviewDisplayDTO
            {
                Id = r.Id,
                Text = r.Text,
                Game = r.Game,
                Author = r.Author.UserName,
                Upvotes = r.Upvoters.Count
            });
                

            return Ok(resultat);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> EditReview(int id, ReviewDTO reviewDTO)
        {
            // 1. Récupérer l'ID de l'utilisateur connecté via le token JWT
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { Message = "Utilisateur non authentifié." });
            }

            // 2. Trouver la critique en incluant l'auteur pour vérifier la propriété
            var review = await _context.Review
                .Include(r => r.Author)
                .FirstOrDefaultAsync(r => r.Id == id);

            // 3. Vérifier si la critique existe
            if (review == null)
            {
                return NotFound(new { Message = "Critique introuvable." });
            }

            // 4. Vérifier si l'utilisateur connecté est bien l'auteur de la critique
            if (review.Author == null || review.Author.Id != userId)
            {
                return StatusCode(StatusCodes.Status403Forbidden,
                    new { Message = "Vous n'êtes pas autorisé à modifier cette critique." });
            }

            // 5. Mettre à jour les propriétés avec les données du DTO
            review.Text = reviewDTO.Text;
            review.Game = reviewDTO.Game;
            // Ajoutez d'autres champs ici si votre ReviewDTO en contient d'autres (ex: Note, etc.)

            // 6. Sauvegarder les modifications dans la base de données
            _context.Review.Update(review);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Review.Any(e => e.Id == id))
                {
                    return NotFound(new { Message = "Critique introuvable." });
                }
                throw;
            }

            return Ok(new { Message = "Critique modifiée !", Review = review });
        }


        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpvoteReview(int id)
        {
            // 1. Récupérer l'ID de l'utilisateur connecté via le token JWT

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { Message = "Utilisateur non authentifié." });
            }
            


            // 2. Récupérer l'utilisateur pour pouvoir l'ajouter/le retirer de la liste
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Unauthorized(new { Message = "Utilisateur introuvable." });
            }

           


            // 3. Trouver la critique en incluant la liste des upvoters
            var review = await _context.Review
                .Include(r => r.Upvoters)
                .FirstOrDefaultAsync(r => r.Id == id);

            // 4. Vérifier si la critique existe
            if (review == null)
            {
                return NotFound(new { Message = "Critique introuvable." });
            }


            if (user.UserName == review.Author.UserName) return Unauthorized();

            // 5. Gérer la bascule (Toggle) de l'upvote
            bool isUpvoted;
            if (review.Upvoters.Any(u => u.Id == userId))
            {
                // L'utilisateur a déjà voté -> on retire le vote
                review.Upvoters.Remove(user);
                isUpvoted = false;
            }
            else
            {
                // L'utilisateur n'a pas encore voté -> on ajoute le vote
                review.Upvoters.Add(user);
                isUpvoted = true;
            }

            // 6. Sauvegarder les changements dans la base de données
            await _context.SaveChangesAsync();

            // 7. Retourner le message et le nouveau total de votes
            return Ok(new
            {
                Message = isUpvoted ? "Vote ajouté !" : "Vote retiré !",
                UpvotesCount = review.Upvoters.Count,
                IsUpvoted = isUpvoted
            });
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> PostReview(ReviewDTO reviewDTO)
        {
            
            User? user = await _userManager.FindByIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            if (user == null) return Unauthorized();

            var review = new Review
            {
                Text = reviewDTO.Text,
                Game = reviewDTO.Game,
                Author = user
            };
            
            _context.Review.Add(review);
            int result = await _context.SaveChangesAsync();

            if (result <= 0)
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { Message = "Veuillez réessayer plus tard." });

            return Ok(review);
            
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteReview(int id)
        {
            User? user = await _userManager.FindByIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            
            Review? review = await _context.Review.Include(r => r.Author).FirstOrDefaultAsync(r => r.Id == id);

            if (review == null) return NotFound();

            
            if (user == null || !user.Reviews.Contains(review)) return Unauthorized(new { Message = "Hey touche pas, c'est pas à toi !" });

            
            _context.Review.Remove(review);
            int result = await _context.SaveChangesAsync();

           

            return Ok(new { Message = "La critique a bien été supprimée" });
        }
    }
}
