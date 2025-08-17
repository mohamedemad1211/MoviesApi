namespace MoviesApi.Dtos
{
    public class MovieDto
    {
        [Required]
        [MaxLength(250)]
        public string Title { get; set; }
        [Required]
        public int Year { get; set; }
        public double Rating { get; set; }
        [MaxLength(2500)]
        public string Description { get; set; }
        public IFormFile? Poster { get; set; }
        public byte GenreId { get; set; }
    }
}
