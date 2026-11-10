namespace MoviesAdminAPI.DTOs
{
    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;

        public string Summary { get; set; } = string.Empty;

        public double RunTime { get; set; }

        public Genre Genre { get; set; } = null!;

        public DateTime ReleaseDate { get; set; }

        public string? ImageUrl { get; set; } = string.Empty;
    }
}
