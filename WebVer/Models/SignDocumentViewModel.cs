using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebVer.Models
{
    public class SignDocumentViewModel
    {
        public SelectList AppointSingerDocuments { get; set; }

        public Guid DocumentId { get; set; }
        public string Password { get; set; }
    }
}
