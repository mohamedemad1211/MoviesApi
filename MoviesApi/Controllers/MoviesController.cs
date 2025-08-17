using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MoviesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private new List<string> _allowedExtention = new List<string> { ".jpg", ".png" };
        private long _maxAllowedPosterSize = 1048576;
        public MoviesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var movies = await _context.Movies.Include(m => m.Genre).ToListAsync();
            return Ok(movies);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var movie = await _context.Movies.Include(m => m.Genre).SingleOrDefaultAsync(m => m.Id == id);
            if (movie == null)
                return NotFound();
            return Ok(movie);
        }

        [HttpGet("GetByGenreId")]
        public async Task<IActionResult> GetByGenreIdAsync(byte genreId)
        {
            var movies = await _context.Movies
                .Where(m => m.GenreId ==genreId)
                .Include(m => m.Genre).ToListAsync();
            return Ok(movies);
        }


        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromForm] MovieDto dto)
        {
            if (dto.Poster == null)
                return BadRequest("Poster is required");
            if (!_allowedExtention.Contains(Path.GetExtension(dto.Poster.FileName).ToLower()))
                return BadRequest("Only .png and .jpg images are allowed");
            if (dto.Poster.Length > _maxAllowedPosterSize)
                return BadRequest("Max allowed size for poster is 1MB");

            var isValidGenre = await _context.Genres.AnyAsync(g => g.Id == dto.GenreId);

            if (!isValidGenre)
                return BadRequest("Invalid genre ID");

            using var dataStream = new MemoryStream();
            await dto.Poster.CopyToAsync(dataStream);
            var movie = new Movie
            {
                GenreId = dto.GenreId,
                Title = dto.Title,
                Poster = dataStream.ToArray(),
                Rating = dto.Rating,
                Description = dto.Description,
                Year = dto.Year
            };

            await _context.AddAsync(movie);
            _context.SaveChanges();

            return Ok(movie);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromForm] MovieDto dto)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
                return NotFound($"No genere was found with Id : {id}");
            var isValidGenre = await _context.Genres.AnyAsync(g => g.Id == dto.GenreId);

            if (!isValidGenre)
                return BadRequest("Invalid genre ID");

            if(dto.Poster != null)
            {
                if (!_allowedExtention.Contains(Path.GetExtension(dto.Poster.FileName).ToLower()))
                    return BadRequest("Only .png and .jpg images are allowed");
                if (dto.Poster.Length > _maxAllowedPosterSize)
                    return BadRequest("Max allowed size for poster is 1MB");
                using var dataStream = new MemoryStream();
                await dto.Poster.CopyToAsync(dataStream);
                movie.Poster = dataStream.ToArray();
            }

            movie.Title = dto.Title;
            movie.GenreId = dto.GenreId;
            movie.Year = dto.Year;
            movie.Description = dto.Description;
            movie.Rating = dto.Rating;

            _context.SaveChanges();

            return Ok(movie);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var movie = await _context.Movies.SingleOrDefaultAsync(g => g.Id == id);
            if (movie == null)
                return NotFound($"No movie was found with Id : {id}");

            _context.Remove(movie);
            _context.SaveChanges();
            return Ok(movie);
        }
    }
}
