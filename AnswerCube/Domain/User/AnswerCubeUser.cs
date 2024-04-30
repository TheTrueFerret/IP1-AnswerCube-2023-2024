using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace AnswerCube.BL.Domain.User;

public class AnswerCubeUser : IdentityUser
{
    [Required]
    [StringLength(25, ErrorMessage = "First Name must be less than 25 characters")]
    [Display(Name = "Firstname")]
    public string FirstName { get; set; }

    [Required]
    [StringLength(50, ErrorMessage = "First Name must be less than 50 characters")]
    public string LastName { get; set; }

    public TypeUser TypeUser { get; set; }

    public ICollection<UserClaim> Claims { get; set; } = new List<UserClaim>();
    
    public ICollection<UserOrganization> UserOrganizations { get; set; } = new List<UserOrganization>();
}