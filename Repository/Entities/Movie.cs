namespace MovieApi.Repository.Entities
{
    public class Movie
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        //public string UserId { get; set; }

        public string Title { get; set; } = string.Empty;
        
        public short Year { get; set; }

        public short GenreId { get; set; }

        public string? RunTime { get; set; }

        public string? Poster { get; set; }

        public decimal AverageRating { get; set; } = 0;
    }
}
