using Microsoft.AspNetCore.Mvc;

namespace ToDoApi.API.Controllers
{
    public class UserController_ : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
