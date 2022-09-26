using Microsoft.Net.Http.Headers;
using MovieApi.Models.DTO;
using MovieApi.Models.Enums;
using MovieApi.Models.Enums.Validation;
using MovieApi.Repository;
using System.IdentityModel.Tokens.Jwt;

namespace MovieApi.Service
{
    public class MovieService : IMovieService
    {
        private readonly IMovieRepository _movieRepository;

        public MovieService(IMovieRepository movieRepository)//, IUserService userService)
        {
            _movieRepository = movieRepository;
        }

        public bool UserExists(string userId)
        {
            if (String.IsNullOrEmpty(userId))
            {
                throw new ArgumentException("userId must be greater than zero", "userId");
            }

            return _movieRepository.UserExists(userId);
        }

        public bool MovieExists(string movieId)
        {
            if (String.IsNullOrEmpty(movieId))
            {
                throw new ArgumentException("moveId must be greater than zero", "movieId");
            }

            return _movieRepository.MovieExists(movieId); //await
        }

        public List<Movie> SearchMovies(MovieSearchCriteria movieSearchCriteria)
        {
            var validationResult = ValidateSearchCriteria(movieSearchCriteria);
            if (validationResult == MovieSearchValidationResults.NoCriteria)
            {
                throw new ArgumentException("movieSearchCriteria must not be null", "movieSearchCriteria");
            }

            if (validationResult == MovieSearchValidationResults.InvalidCriteria)
            {
                throw new ArgumentException("movieSearchCriteria must have at least one search criteria", "movieSearchCriteria");
            }

            return _movieRepository.SearchMovies(movieSearchCriteria); //await
        }

        public List<Movie> TopMovies(byte movieCount)
        {
            if (movieCount == 0)
            {
                throw new ArgumentException("movieCount must be greater than zero", "movieCount");
            }

            return _movieRepository.TopMovies(movieCount); //await
        }

        public List<Movie> TopMoviesByUser(byte movieCount, string userId)
        {
            if (movieCount == 0)
            {
                throw new ArgumentException("movieCount must be greater than zero", "movieCount");
            }

            if (String.IsNullOrEmpty(userId))
            {
                throw new ArgumentException("userId must be greater than zero", "userId");
            }

            if (!_movieRepository.UserExists(userId))
            {
                throw new ArgumentException($"User not found for userId {userId}", "userId");
            }

            return _movieRepository.TopMoviesByUser(movieCount, userId); //await
        }

        public MovieSearchValidationResults ValidateSearchCriteria(MovieSearchCriteria movieSearchCriteria)
        {
            if (movieSearchCriteria == null)
            {
                return MovieSearchValidationResults.NoCriteria;
            }

            if (string.IsNullOrEmpty(movieSearchCriteria.Title)
                && Convert.ToInt16(movieSearchCriteria.Year) == 0)
                //&& (movieSearchCriteria.Genres == null || !movieSearchCriteria.Genres.Any()))
            {
                return MovieSearchValidationResults.InvalidCriteria;
            }

            return MovieSearchValidationResults.OK;
        }

        public bool SaveMovie(Movie movie)
        {
            return _movieRepository.SaveMovie(movie);
        }

        public bool SaveRating(MovieRating movieRating)
        {
            var validationResult = ValidateMovieRating(movieRating);

            switch (validationResult)
            {
                case MovieRatingSaveValidationResults.NullRating:
                    throw new ArgumentException("movieRating must not be null", "movieRating");
                case MovieRatingSaveValidationResults.InvalidMovieId:
                    throw new ArgumentException("movieRating.MovieId must be greater than zero", "movieRating");
                case MovieRatingSaveValidationResults.MovieNotfound:
                    throw new ArgumentException($"Move not found for movieRating.MovieId {movieRating.MovieId} not found", "movieRating");
                case MovieRatingSaveValidationResults.InvalidUserId:
                    throw new ArgumentException("movieRating.UserId must be greater than zero", "movieRating");
                case MovieRatingSaveValidationResults.UserNotFound:
                    throw new ArgumentException($"User note found for movieRating.UserId {movieRating.UserId} must not be zero", "movieRating");
            }

            return _movieRepository.SaveRating(movieRating);
        }

        public bool SaveMovieToUsersCollection(MovieCollection movieCollection)
        {
            var validationResult = ValidateMovieCollection(movieCollection);

            switch (validationResult)
            {
                case MovieCollectionSaveValidationResults.NullCollection:
                    throw new ArgumentException("movieCollection must not be null", "movieCollection");
                case MovieCollectionSaveValidationResults.InvalidMovieId:
                    throw new ArgumentException("movieCollection.MovieId must be greater than zero", "movieCollection");
                case MovieCollectionSaveValidationResults.MovieNotfound:
                    throw new ArgumentException($"Move not found for movieCollection.MovieId {movieCollection.MovieId} not found", "movieCollection");
                case MovieCollectionSaveValidationResults.InvalidUserId:
                    throw new ArgumentException("movieCollection.UserId must be greater than zero", "movieRating");
                case MovieCollectionSaveValidationResults.UserNotFound:
                    throw new ArgumentException($"User note found for movieCollection.UserId {movieCollection.UserId} must not be zero", "movieCollection");
            }

            return _movieRepository.SaveMovieToUsersCollection(movieCollection);
        }

        public MovieRatingSaveValidationResults ValidateMovieRating(MovieRating movieRating)
        {
            if (movieRating == null)
            {
                return MovieRatingSaveValidationResults.NullRating;
            }

            if (String.IsNullOrEmpty(movieRating.MovieId))
            {
                return MovieRatingSaveValidationResults.InvalidMovieId;
            }

            if (!_movieRepository.MovieExists(movieRating.MovieId))
            {
                return MovieRatingSaveValidationResults.MovieNotfound;
            }

            if (String.IsNullOrEmpty(movieRating.UserId))
            {
                return MovieRatingSaveValidationResults.InvalidUserId;
            }

            if (!_movieRepository.UserExists(movieRating.UserId))
            {
                return MovieRatingSaveValidationResults.UserNotFound;
            }

            return MovieRatingSaveValidationResults.OK;

        }

        public MovieCollectionSaveValidationResults ValidateMovieCollection(MovieCollection movieCollection)
        {
            if (movieCollection == null)
            {
                return MovieCollectionSaveValidationResults.NullCollection;
            }

            if (String.IsNullOrEmpty(movieCollection.MovieId))
            {
                return MovieCollectionSaveValidationResults.InvalidMovieId;
            }

            if (!_movieRepository.MovieExists(movieCollection.MovieId))
            {
                return MovieCollectionSaveValidationResults.MovieNotfound;
            }

            if (String.IsNullOrEmpty(movieCollection.UserId))
            {
                return MovieCollectionSaveValidationResults.InvalidUserId;
            }

            if (!_movieRepository.UserExists(movieCollection.UserId))
            {
                return MovieCollectionSaveValidationResults.UserNotFound;
            }

            return MovieCollectionSaveValidationResults.OK;
        }

        public string GetLoggedInUserId(HttpRequest request)
        {
            var accessToken = request.Headers[HeaderNames.Authorization].ToString().Split(' ')[1];
            var handler = new JwtSecurityTokenHandler();
            var decodedToken = handler.ReadJwtToken(accessToken);
            var loggedInUserId = decodedToken.Subject.Split('|')[1];
            return loggedInUserId;
        }
    }
}
