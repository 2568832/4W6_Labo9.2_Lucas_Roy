using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using serveur16.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace serveur16.Data
{
    public class serveur16Context : IdentityDbContext<User>
    {
        public serveur16Context (DbContextOptions<serveur16Context> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            User u1 = new User
            {
                Id = "11111111-1111-1111-1111-111111111111",
                UserName = "bob",
                Email = "b@b.b",
                NormalizedUserName = "BOB",
                NormalizedEmail = "B@B.B",
                ConcurrencyStamp = "8ad80a27-73dd-4785-9ff9-118c05cf9719",
                SecurityStamp = "56825025-1e7a-4309-afc0-7edf7ac0fea0",
                PasswordHash = "AQAAAAIAAYagAAAAENvyCAm+H9eSlX+/4uupNrY9Kl4EoVbqTPztcz/zjyfTLpWIABVIgkry4P8qsTcn3g=="
            };

            builder.Entity<User>().HasData(u1);

            builder.Entity<Review>().HasData(
                new { Id = 1, Game = "Cyberpunk 2077", Text = "C'parce que lé bonhommes font des T poses pis y rentrer dans le plancher des fois xd", AuthorId = u1.Id },
                new { Id = 2, Game = "Mario Kart World", Text = "Je préfère de loin Garfield Kart !", AuthorId = u1.Id }
            );
        }

        public DbSet<Review> Review { get; set; } = default!;
    }
}
