using MovieApi.Models.DTO;
using MovieApi.Models.Enums;
using Newtonsoft.Json;
using Repo = MovieApi.Repository.Entities;

namespace MovieApi.Repository
{
    public class MovieRepository : IMovieRepository
    {
        List<Repo.Movie> movies;
        List<Repo.User> users;
        List<Repo.UsersRatings> userRatings;
        List<Repo.UsersMovies> usersMoviesCollection;

        public MovieRepository()
        {
            //loading data from json file

            //for movies
            using (StreamReader r = new("movies.json"))
            {
                string json = r.ReadToEnd();
                movies = JsonConvert.DeserializeObject<List<Repo.Movie>>(json);
            }

            //for users
            using (StreamReader r = new("users.json"))
            {
                string json = r.ReadToEnd();
                users = JsonConvert.DeserializeObject<List<Repo.User>>(json);
            }

            //for users ratings
            using (StreamReader r = new("usersRatings.json"))
            {
                string json = r.ReadToEnd();
                userRatings = JsonConvert.DeserializeObject<List<Repo.UsersRatings>>(json);
            }

            //for user collections
            using (StreamReader r = new("usersMoviesCollection.json"))
            {
                string json = r.ReadToEnd();
                usersMoviesCollection = JsonConvert.DeserializeObject<List<Repo.UsersMovies>>(json);
            }
        }

        public bool UserExists(string userId)
        {
            return users.Any(m => m.Id == userId);
        }

        public bool MovieExists(string movieId)
        {
            return movies.Any(m => m.Id == movieId);
        }

        public List<Models.DTO.Movie> SearchMovies(MovieSearchCriteria movieSearchCritera)
        {
            if (!string.IsNullOrWhiteSpace(movieSearchCritera.Title))
            {
                movies = movies.Where(m => m.Title.Contains(movieSearchCritera.Title)).ToList();
            }

            if (Convert.ToInt16(movieSearchCritera.Year) > 0)
            {
                movies = movies.Where(m => m.Year == movieSearchCritera.Year).ToList();
            }

            if (movieSearchCritera.Genres != null && movieSearchCritera.Genres.Any())
            {
                var genreList = movieSearchCritera.Genres.Cast<int>().ToList();
                if (!genreList.Contains(0))
                {
                    movies = movies.Where(m => genreList.Contains(m.GenreId)).ToList();
                }
            }

            return movies.Select(m => new Models.DTO.Movie
            {
                AverageRating = m.AverageRating,
                Genre = (Genres)m.GenreId,
                RunTime = m.RunTime,
                Title = m.Title,
                Year = m.Year
            }).ToList();

        }

        public List<Models.DTO.Movie> TopMovies(byte movieCount)
        {
            return movies
                .OrderByDescending(m => m.AverageRating)
                .ThenBy(m => m.Title)
                .Take(movieCount)
                .Select(m => new Models.DTO.Movie
                {
                    AverageRating = m.AverageRating,
                    Genre = (Genres)m.GenreId,
                    RunTime = m.RunTime,
                    Title = m.Title,
                    Year = m.Year
                }).ToList();
        }

        public List<Models.DTO.Movie> TopMoviesByUser(byte movieCount, string userId)
        {
            var usersRecords = usersMoviesCollection.Where(u => u.UserId == userId).ToList();
            var moviesRecords = new List<Repo.Movie>();

            foreach (var item in usersRecords)
            {
                moviesRecords.AddRange(movies.Where(m => m.Id == item.MovieId));
            }

            return moviesRecords
                .OrderByDescending(m => m.AverageRating)
                .ThenBy(m => m.Title)
                .Take(movieCount)
                .Select(m => new Models.DTO.Movie
                { 
                    AverageRating = m.AverageRating,
                    Genre = (Genres)m.GenreId,
                    RunTime = m.RunTime,
                    Title = m.Title,
                    Year = m.Year,
                    Poster = m.Poster
                }).ToList();
        }

        public bool SaveMovie(Models.DTO.Movie movie)
        {
            bool movieExists = movies.Any(m => m.Title == movie.Title && m.Year == movie.Year);
            if (!movieExists)
            {
                movies.Add(new Repo.Movie()
                {
                    Id = Guid.NewGuid().ToString(),
                    //UserId = loggedInUserId can be saved here
                    GenreId = (short)movie.Genre,
                    RunTime = movie.RunTime,
                    Title = movie.Title,
                    Year = movie.Year,
                    AverageRating = movie.AverageRating,
                    Poster = movie.Poster,
                });

                var json = JsonConvert.SerializeObject(movies);

                using (StreamWriter w = new("movies.json"))
                {
                    w.Write(json);
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        public bool SaveMovieToUsersCollection(MovieCollection movieCollection)
        {
            var userMovie = usersMoviesCollection.FirstOrDefault(mr => mr.MovieId == movieCollection.MovieId && mr.UserId == movieCollection.UserId);

            if (userMovie != null)
            {
                userMovie.MovieId = movieCollection.MovieId;
            }
            else
            {
                userMovie = new Repo.UsersMovies()
                {
                    MovieId = movieCollection.MovieId,
                    UserId = movieCollection.UserId
                };

                usersMoviesCollection.Add(userMovie);

                var json = JsonConvert.SerializeObject(usersMoviesCollection);

                using (StreamWriter w = new("usersMoviesCollection.json"))
                {
                    w.Write(json);
                }
            }

            return true;
        }

        public bool SaveRating(MovieRating movieRating)
        {
            var userRating = userRatings.FirstOrDefault(mr => mr.MovieId == movieRating.MovieId && mr.UserId == movieRating.UserId);

            if (userRating != null)
            {
                userRating.Rating = movieRating.Rating;
            }
            else
            {
                userRating = new Repo.UsersRatings()
                {
                    MovieId = movieRating.MovieId,
                    UserId = movieRating.UserId,
                    Rating = movieRating.Rating
                };

                userRatings.Add(userRating);
            }

            var json = JsonConvert.SerializeObject(userRatings);

            using (StreamWriter w = new("usersRatings.json"))
            {
                w.Write(json);
            }

            return true;
        }

        public void UpdateMovie(Models.DTO.Movie movie)
        {
            throw new NotImplementedException();
        }

        public void DeleteMovie(string id)
        {
            throw new NotImplementedException();
        }

        public void DeleteAllMovies()
        {
            throw new NotImplementedException();
        }
    }
}
