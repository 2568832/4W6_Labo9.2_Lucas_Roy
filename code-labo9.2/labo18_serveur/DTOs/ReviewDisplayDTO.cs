namespace serveur16.Models.DTOs
{
    public class ReviewDisplayDTO
    {
        public int Id { get; set; }
        public string Text { get; set; } = null!;
        public string Game { get; set; } = null!;
        public string Author { get; set; } = null!; 
        public int Upvotes { get; set; }            
    }
}