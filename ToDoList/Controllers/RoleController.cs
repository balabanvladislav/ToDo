using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.EntityFramework;
using ToDo.BL;
using ToDo.Domain;
using ToDoList.Models;

namespace ToDoList.Controllers
{
    public class RoleController : Controller
    {
        private ApplicationDbContext context;
        // GET
        public RoleController()
        {
            context = new ApplicationDbContext();
        }

        /// <summary>
        /// Get All Roles
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "MainAdmin")]
        public ActionResult Index()
        {
            var role = context.Roles.ToList();
            return View(role);
        }

        /// <summary>
        /// Create a new role
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "MainAdmin")]
        public ActionResult Create()
        {
            var role = new IdentityRole();
            return View(role);
        }

        
        [HttpPost]
        [Authorize(Roles = "MainAdmin")]
        public ActionResult Create(IdentityRole role)
        {
            context.Roles.Add(role);
            context.SaveChanges();
            return RedirectToAction("Index");
        }


    }
}