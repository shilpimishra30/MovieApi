using MovieApi.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace MovieApi.Models.DTO
{
    /// <summary>
    /// Movie search criteria, used in movie searches.
    /// At least one field must be completed to search
    /// </summary>
    public class MovieSearchCriteria
    {
        [MaxLength(255)]
        public string Title { get; set; }

        [Range(0, 2022)]
        public short Year { get; set; }
        
        public List<Genres> Genres { get; set; } = new List<Genres>();
    }
}
