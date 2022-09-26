using MovieApi.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace MovieApi.Models.DTO
{
    public class Movie
    {
        [Required]
        [MaxLength(255)]
        public string Title { get; set; }

        [Required]
        [Range(1900, 2022)]
        public short Year { get; set; }

        [Range(0,5)]
        public Genres Genre { get; set; }

        public string? RunTime { get; set; }

        public string? Poster { get; set; }

        [Range(0,10)]
        public decimal AverageRating { get; set; }
    }
}
