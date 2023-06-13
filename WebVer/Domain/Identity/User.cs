using Microsoft.AspNetCore.Identity;
using WebVer.Domain.Documents;

namespace WebVer.Domain.Identity;

public class User : IdentityUser<Guid>
{
    public string FirstName { get; set; }

    public string LastName { get; set; }
    
    public string Patronymic { get; set; }

    public string Inn { get; set; }

    public ICollection<AppointSingerDocument> AppointDocuments { get; set; }

    public virtual ICollection<UserRole> UserRoles { get; set; }
}