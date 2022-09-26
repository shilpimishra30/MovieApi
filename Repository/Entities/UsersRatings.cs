namespace MovieApi.Repository.Entities
{
    public class UsersRatings
    {
        public string UserId { get; set; } = String.Empty;

        public string MovieId { get; set; } = string.Empty;

        public decimal Rating { get; set; } 
    }
}
