using Microsoft.AspNetCore.Identity;

namespace WebApp.Data;

public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; }

    public override string UserName { get; set; }

    public string LastName { get; set; }

    public DateTime BirthDate { get; set; }

    public DateTime CreatedOnDate { get; set; }

    public bool IsDeleted { get; set; }
}