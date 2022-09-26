using System.ComponentModel.DataAnnotations;

namespace MovieApi.Models.DTO
{
    /// <summary>
    /// User
    /// </summary>
    public class User
    {
        /// <summary>
        /// Users id
        /// </summary>
        [Required]
        public string Id { get; set; }

        /// <summary>
        /// Users Name
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        /// <summary>
        /// Users Email
        /// </summary>
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        /// <summary>
        /// Users Nickname
        /// </summary>
        [MaxLength(50)]
        public string Nickname { get; set; }


    }
}
