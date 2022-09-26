﻿using System.ComponentModel.DataAnnotations;

namespace MovieApi.Models.DTO
{
    /// <summary>
    /// Users movie rating
    /// </summary>
    public class MovieRating
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

        /// <summary>
        /// User rating for the movie, must be zero or greater
        /// </summary>
        [Required]
        public decimal Rating { get; set; }
    }
}
