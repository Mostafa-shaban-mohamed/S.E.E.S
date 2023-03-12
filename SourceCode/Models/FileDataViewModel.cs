using System.ComponentModel.DataAnnotations;

namespace SourceCode.Models
{
    public class FileDataViewModel
    {
        public string id { get; set; }
        [Required]
        public IFormFile File { get; set; }
    }
}