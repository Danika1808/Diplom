using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using WebVer.Domain.Documents;
using WebVer.Domain.Identity;

namespace WebVer.Models
{
    public class AppointSignerViewModel
    {
        public SelectList Documents { get; set; }
        public SelectList Users { get; set; }

        public Guid UserId { get; set; }
        public Guid DocumentId { get; set; }
    }
}
