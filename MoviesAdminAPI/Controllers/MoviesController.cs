using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Data;
using MoviesAdminAPI.DTOs;
using Dapper;

namespace MoviesAdminAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IDbConnection _db;

        //Constructor
        public MoviesController(IDbConnection db)
        {
            //Initialize any dependencies or services here
            _db = db;
        }

        //GET: api/Movies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Movie>>> GetAllMovies()
        {
            var sql = "SELECT m.*,g.*,r.* " +
                "FROM Movie m " +
                "INNER JOIN dbo.Genres G on G.Id = m.GenreId " +
                "INNER JOIN dbo.AudienceRatings r on r.Id = m.AudienceRatingId";


            //Retrieve all movies from the database or any data source
            var movies = await _db.QueryAsync<Movie, Genre, AudienceRating, Movie>(sql, (movie, genre, audienceRating) =>
            {
                movie.Genre = genre;
                movie.AudienceRating = audienceRating;
                return movie;
            },
            splitOn: "Id");

            return Ok(movies);
        }

        // GET /api/movies/{id}
        [HttpGet("{id:int}")]
        public async Task<ActionResult<IEnumerable<Movie>>> GetMovieById(int id)
        {
            var sql = @"SELECT m.*,g.*,r.*,cr.*,u.*
            FROM Movie m
            INNER JOIN dbo.Genres G on G.Id = m.GenreId
            INNER JOIN dbo.AudienceRatings r on r.Id = m.AudienceRatingId

            Left Outer Join dbo.CriticReview cr on m.Id = cr.MovieId
            Left Outer Join dbo.UserSnapshot u on cr.CreatedBy = u.UserId
            WHERE m.Id = @Id";

            // Create a null movie
            Movie? movie = null;

            // Fetch all movies from database. <Table 1, Table 2, Table 3, Table 4, Output>
            await _db.QueryAsync<Movie, Genre, AudienceRating, Review, User, Movie>(
                sql,
                (m, c, r, cr, u) => // function is called for each row in the result set.
                {
                    // populate movie object only for the first row in result set
                    if (movie == null)
                    {
                        movie = m;
                        movie.Genre = c;
                        movie.AudienceRating = r;
                        movie.Reviews = new List<Review>(); // initialize an empty list
                    }

                    if (cr != null)
                    {
                        cr.User = u;
                        movie.Reviews.Add(cr);
                    }

                    return movie;
                },
                new { Id = id },
                splitOn: "Id,Id,Id,UserId"
             ); // indicates where to split the result set into Movie and Category objects. The first column of the next table.

            return Ok(movie);

        }
    }
}