using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace InventarioWeb.Models
{
    // Para agregar datos de perfil del usuario, agregue más propiedades a su clase ApplicationUser. Visite https://go.microsoft.com/fwlink/?LinkID=317594 para obtener más información.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Tenga en cuenta que authenticationType debe coincidir con el valor definido en CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Agregar reclamaciones de usuario personalizadas aquí
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public System.Data.Entity.DbSet<InventarioWeb.Models.Producto> Productos { get; set; }
        public System.Data.Entity.DbSet<InventarioWeb.Models.Almacen> Almacen { get; set; }
        public System.Data.Entity.DbSet<InventarioWeb.Models.Proveedor> Proveedores { get; set; }
        public System.Data.Entity.DbSet<InventarioWeb.Models.Inventario> Inventario { get; set; }
        public System.Data.Entity.DbSet<InventarioWeb.Models.Negocio> Negocio { get; set; }
    }
}