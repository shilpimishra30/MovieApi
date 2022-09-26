using MovieApi.Models.DTO;

namespace MovieApi.Repository
{
    public interface IMovieRepository
    {
        void UpdateMovie(Movie movie);

        void DeleteMovie(string id);

        void DeleteAllMovies();

        /// <summary>
        /// Searches movies using the given criteria
        /// </summary>
        /// <param name="movieSearchCritera"></param>
        /// <returns></returns>
        List<Movie> SearchMovies(MovieSearchCriteria movieSearchCritera);

        /// <summary>
        /// Returns the top X movies based on average user rating
        /// </summary>
        /// <param name="movieCount"></param>
        /// <returns></returns>
        List<Movie> TopMovies(byte movieCount);

        /// <summary>
        /// Returns the top X movies for the given user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        List<Movie> TopMoviesByUser(byte movieCount, string userId);

        /// <summary>
        /// Saves the movie, inserts if the movie doesn't exist or updates if the movie does exist
        /// </summary>
        /// <param name="movie"></param>
        /// <returns></returns>
        bool SaveMovie(Movie movie);

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
        /// Saves the rating.
        /// If the rating already exists it will be updated to the new value
        /// </summary>
        /// <param name="movieRating"></param>
        /// <returns>true if save successful, false if not</returns>
        bool SaveRating(MovieRating movieRating);

        /// <summary>
        /// Saves the collection.
        /// If the collection already exists it will be updated to the new value
        /// </summary>
        /// <param name="movieCollection"></param>
        /// <returns>true if save successful, false if not</returns>
        public bool SaveMovieToUsersCollection(MovieCollection movieCollection);
    }
}
