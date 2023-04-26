using System.ComponentModel.DataAnnotations;

namespace WebVer.Models
{
    public class AddDocumentViewModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }

        [Required]
        [DataType(DataType.Upload)]
        public IFormFile File { get; set; }
    }
}
