using System.ComponentModel.DataAnnotations;

namespace MoviesAdminAPI.DTOs
{
    public class AudienceRating
    {
        public int Id { get; set; }

        public string Rating { get; set; } = string.Empty;
    }
}
