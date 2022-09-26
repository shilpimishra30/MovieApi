using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieApi.Models.DTO;
using MovieApi.Models.Enums;
using MovieApi.Models.Enums.Validation;
using MovieApi.Service;

namespace MovieApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController :  Controller
    {
        private static readonly byte movieCount = 5;
        private readonly IMovieService _movieService;

        public MovieController(IMovieService movieService)
        {
            _movieService = movieService;
        }

        //// GET: test api
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        /// <summary>
        /// Add new movie
        /// </summary>
        /// <param name="movie"></param>
        /// <returns></returns>
        [Authorize(Policy = "AddAccess")]
        //[Authorize(Roles = "MovieAdmin")]
        [Route("new")]
        [HttpPost]
        [ProducesResponseType(200, Type = typeof(Movie))]
        public IActionResult SaveMovie([FromBody] Movie movie)
        {
            if (_movieService.SaveMovie(movie))
            {
                return Ok();
            }
            else
            {
                return BadRequest("unable to save movie");
            }
        }

        /// <summary>
        /// Searches movies by given criteria
        /// Note: at least one field of the criteria object must be set
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        [Authorize(Policy = "ReadAccess")]
        //[Authorize(Roles = "MovieWatcher")]
        [HttpPost]
        [Route("search")]
        [ProducesResponseType(200, Type = typeof(List<Movie>))]
        public IActionResult SearchMovies(MovieSearchCriteria criteria)
        {
            var validationResult = _movieService.ValidateSearchCriteria(criteria);

            switch (validationResult)
            {
                case MovieSearchValidationResults.InvalidCriteria:
                case MovieSearchValidationResults.NoCriteria:
                    return BadRequest(validationResult.ToString());
            }

            var movies = _movieService.SearchMovies(criteria);

            if (movies == null || !movies.Any())
            {
                return NotFound("No movies found");
            }

            return Json(movies);
        }

        /// <summary>
        /// Get's top 5 movies across all users
        /// </summary>
        /// <returns></returns>
        [Authorize(Policy = "ReadAccess")]
        //[Authorize(Roles = "MovieWatcher")]
        [Route("top")]
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<Movie>))]
        public IActionResult TopMovies()
        {
            var movies = _movieService.TopMovies(movieCount);

            if (movies == null || !movies.Any())
            {
                return NotFound();
            }

            return Json(movies);
        }

        /// <summary>
        /// Gets top 5 movies for a specific user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [Authorize(Policy = "ReadAccess")]
        //[Authorize(Roles = "MovieWatcher")]
        [Route("top/{userId}")]
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<Movie>))]
        public IActionResult TopMoviesByUser(string userId)
        {
            if (String.IsNullOrEmpty(userId))
            {
                return BadRequest("userId is zero or negative");
            }

            if (!_movieService.UserExists(userId))
            {
                return BadRequest("userId is invalid");
            }

            var movies = _movieService.TopMoviesByUser(movieCount, userId);

            if (movies == null || !movies.Any())
            {
                return NotFound();
            }

            return Json(movies);
        }

        /// <summary>
        /// saves a movie rating
        /// </summary>
        /// <param name="movieRating"></param>
        /// <returns></returns>
        [Authorize(Policy = "ReadAccess")]
        //[Authorize(Roles = "MovieWatcher")]
        [Route("rating")]
        [HttpPost]
        public IActionResult SaveMovieRating([FromBody] MovieRating movieRating)
        {
            //string loggedInUserId = _movieService.GetLoggedInUserId(Request);

            //if (loggedInUserId == null || movieRating.UserId != loggedInUserId)
            //{
            //    return BadRequest("userId mismatch, user cannot submit the rating for any other user.");
            //}

            var validationResult = _movieService.ValidateMovieRating(movieRating);

            switch (validationResult)
            {
                case MovieRatingSaveValidationResults.OK:
                    if (_movieService.SaveRating(movieRating))
                    {
                        return Ok();
                    }
                    else
                    {
                        return BadRequest("couldn't save rating");
                    }
                case MovieRatingSaveValidationResults.NullRating:
                case MovieRatingSaveValidationResults.InvalidMovieId:
                case MovieRatingSaveValidationResults.InvalidUserId:
                    return BadRequest(validationResult.ToString());
                case MovieRatingSaveValidationResults.MovieNotfound:
                case MovieRatingSaveValidationResults.UserNotFound:
                    return NotFound(validationResult.ToString());
            }

            return BadRequest("unknown validation failure");
        }

        /// <summary>
        /// saves a movie collection for a user
        /// </summary>
        /// <param name="movieCollection"></param>
        /// <returns></returns>
        [Authorize(Policy = "ReadAccess")]
        //[Authorize(Roles = "MovieWatcher")]
        [Route("collection")]
        [HttpPost]
        public IActionResult SaveMovieToUsersCollection([FromBody] MovieCollection movieCollection)
        {
            //string loggedInUserId = _movieService.GetLoggedInUserId(Request);

            //if (loggedInUserId == null || movieCollection.UserId != loggedInUserId)
            //{
            //    return BadRequest("userId mismatch, user cannot change the collection for any other user.");
            //}

            var validationResult = _movieService.ValidateMovieCollection(movieCollection);

            switch (validationResult)
            {
                case MovieCollectionSaveValidationResults.OK:
                    if (_movieService.SaveMovieToUsersCollection(movieCollection))
                    {
                        return Ok();
                    }
                    else
                    {
                        return BadRequest("couldn't save collection");
                    }
                case MovieCollectionSaveValidationResults.NullCollection:
                case MovieCollectionSaveValidationResults.InvalidMovieId:
                case MovieCollectionSaveValidationResults.InvalidUserId:
                    return BadRequest(validationResult.ToString());
                case MovieCollectionSaveValidationResults.MovieNotfound:
                case MovieCollectionSaveValidationResults.UserNotFound:
                    return NotFound(validationResult.ToString());
            }

            return BadRequest("unknown validation failure");
        }


    }
}
