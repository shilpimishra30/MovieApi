using System.ComponentModel.DataAnnotations;

namespace MovieApi.Models.DTO
{
    /// <summary>
    /// Users movie collection
    /// </summary>
    public class MovieCollection
    {
        /// <summary>
        /// Id of the user
        /// </summary>
        [Required]
        public string UserId { get; set; }

        /// <summary>
        /// Id of the movie
        /// </summary>
        [Required]
        public string MovieId { get; set; }
    }
}
