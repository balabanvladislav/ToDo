using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using ToDo.Domain;

namespace ToDo.BL
{
   
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }
        
        public DbSet<ToDoItem> ToDoItems { set; get; }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}