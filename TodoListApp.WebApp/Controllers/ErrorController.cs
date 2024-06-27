using Microsoft.AspNetCore.Mvc;

namespace TodoListApp.WebApp.Controllers;

[Route("error")]
public class ErrorController : Controller
{
    [Route("{statusCode}")]
    public IActionResult HttpStatusCodeHandler(int statusCode)
    {
        if (statusCode == 404)
        {
            return this.View("NotFound");
        }

        if (statusCode == 403)
        {
            return this.View("AccessDenied");
        }

        if (statusCode == 400)
        {
            return this.View("BadRequest");
        }

        if (statusCode == 500)
        {
            return this.View("InternalServerError");
        }

        return this.View("Error");
    }
}
