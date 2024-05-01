using Microsoft.AspNetCore.Authorization;

namespace AnswerCube.UI.MVC.Controllers;

[Authorize(Roles = "Admin,DeelplatformBeheerder")]
public class DeelplatformBeheerderController : BaseController
{
    
    
}