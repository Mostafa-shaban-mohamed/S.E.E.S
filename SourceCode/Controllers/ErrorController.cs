using Microsoft.AspNetCore.Mvc;

public class ErrorController : Controller
{
    [Route("/Error/{statusCode}")]
    public IActionResult HttpStatusCodeHandler(int statusCode)
    {
        switch (statusCode)
        {
            case 401:
                ViewBag.ErrorMessage = "User is not authorized to access.";
                break;

            case 403:
                ViewBag.ErrorMessage = "User is not forbidden to access.";
                break;

            case 404:
                ViewBag.ErrorMessage = "Resource could not be found.";
                break;

            case 500:
                ViewBag.ErrorMessage = "the server encountered an unexpected condition that prevented it from fulfilling the request.";
                break;

            case 501:
                ViewBag.ErrorMessage = "Server doesn't support functionality to fulfill the request.";
                break;

            case 502:
                ViewBag.ErrorMessage = "Server is Busy at the moment, try again later.";
                break;
        }
        return View("Error");
    }
}