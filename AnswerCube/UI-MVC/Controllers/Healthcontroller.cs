using Microsoft.AspNetCore.Mvc;

namespace AnswerCube.UI.MVC.Controllers;

public class Healthcontroller : BaseController
{
    [HttpGet("/health")]
    public IActionResult Health()
    {
        return Ok();
    }
}