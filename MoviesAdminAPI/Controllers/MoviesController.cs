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
            var sql = "SELECT m.*,g.*" +
                "FROM Movie m " +
                "INNER JOIN Genres g ON m.GenreId = g.Id";


            //Retrieve all movies from the database or any data source
            var movies = await _db.QueryAsync<Movie, Genre, Movie>(sql, (movie, genre) =>
            {
                movie.Genre = genre;
                return movie;
            });
            return Ok(movies);
        }
    }
}
