using Microsoft.AspNetCore.Identity;

namespace AnswerCube.UI.MVC.Areas.Identity.Data;

public class UserRoleDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string LastName { get; set; }
    public bool IsPremium { get; set; }
    public List<string> Roles { get; set; }
    public List<IdentityRole> AllAvailableIdentityRoles { get; set; }
    public string SelectedRole { get; set; }
    public string SelectedRoleToRemove { get; set; }
    public string SelectedClaim { get; set; }
    public string loggedInId { get; set; }
}