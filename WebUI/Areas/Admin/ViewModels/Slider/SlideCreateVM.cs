
using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;
using RequiredAttribute = System.ComponentModel.DataAnnotations.RequiredAttribute;

namespace WebUI.Areas.Admin.ViewModels.Slider
{
    public class SlideCreateVM
    {
        [Required]
        public IFormFile? Photo { get; set; }
        [MaxLength(150)]
        public string? Offer { get; set; }
        [Required,MaxLength(150)]
        public string? Title { get; set; }
        [MaxLength(500)]
        public string? Description { get; set; }
    }
}
