using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace MoviesAdminAPI.DTOs
{
    public class Review
    {
        public int Id { get; set; }
        public int MovieId { get; set; }
        public string Description { get; set; } = string.Empty;
        public float Rating { get; set; }
        public Boolean IsPublished { get; set; } = false;
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public User User { get; set; } = null!;
    }
}
