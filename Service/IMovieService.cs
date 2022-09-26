using MovieApi.Models.DTO;
using MovieApi.Models.Enums;
using MovieApi.Models.Enums.Validation;

namespace MovieApi.Service
{
    public interface IMovieService
    {
        /// <summary>
        /// Validates the given movieSearchCriteria
        /// </summary>
        /// <param name="movieSearchCriteria"></param>
        /// <returns></returns>
        MovieSearchValidationResults ValidateSearchCriteria(MovieSearchCriteria movieSearchCriteria);

        /// <summary>
        /// Searches movies using the given criteria
        /// </summary>
        /// <param name="movieSearchCriteria"></param>
        /// <returns></returns>
        List<Movie> SearchMovies(MovieSearchCriteria movieSearchCriteria);

        /// <summary>
        /// Top movies by average rating
        /// </summary>
        /// <param name="movieCount"></param>
        /// <returns></returns>
        List<Movie> TopMovies(byte movieCount);

        /// <summary>
        /// Top movies for the given user
        /// </summary>
        /// <param name="movieCount"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        List<Movie> TopMoviesByUser(byte movieCount, string userId);

        /// <summary>
        /// Checks if the user exists
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        bool UserExists(string userId);

        /// <summary>
        /// Checks if the given movie exists
        /// </summary>
        /// <param name="movieId"></param>
        /// <returns></returns>
        bool MovieExists(string movieId);

        /// <summary>
        /// Saves the given movie, if existing movie updates, if new inserts, returns true if save was successful
        /// </summary>
        /// <param name="movie"></param>
        /// <returns></returns>
        bool SaveMovie(Movie movie);

        ///// <summary>
        ///// Saves the rating.
        ///// If the rating already exists it will be updated to the new value
        ///// </summary>
        ///// <param name="movieRating"></param>
        ///// <returns>true if save successful, false if not</returns>
        bool SaveRating(MovieRating movieRating);

        ///// <summary>
        ///// Saves the rating.
        ///// If the rating already exists it will be updated to the new value
        ///// </summary>
        ///// <param name="movieRating"></param>
        ///// <returns>true if save successful, false if not</returns>
        bool SaveMovieToUsersCollection(MovieCollection movieCollection);

        /// <summary>
        /// Validates the movieRating for saving
        /// </summary>
        /// <param name="movieRating"></param>
        /// <returns></returns>
        MovieRatingSaveValidationResults ValidateMovieRating(MovieRating movieRating);

        /// <summary>
        /// Validates the movieCollection for saving
        /// </summary>
        /// <param name="movieCollection"></param>
        /// <returns></returns>
        MovieCollectionSaveValidationResults ValidateMovieCollection(MovieCollection movieCollection);

        /// <summary>
        /// Gets loggedIn User from the access token
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public string GetLoggedInUserId(HttpRequest request);
    }
}
