using Microsoft.AspNetCore.Identity;

namespace MotoLocadora.BuildingBlocks.Entities;

public class ApplicationUser : IdentityUser
{
    // Se quiser personalizar, exemplo:
    public string? FullName { get; set; }
}
