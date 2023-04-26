using Microsoft.AspNetCore.Identity;

namespace WebVer.Domain.Identity;

public class User : IdentityUser<Guid>
{
    public string FirstName { get; set; }

    public string LastName { get; set; }
    
    public string Patronymic { get; set; }

    public string Inn { get; set; }

    public virtual ICollection<UserRole> UserRoles { get; set; }
}