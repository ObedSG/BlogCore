using BlogCore.Models;
using BlogCore.Utilidades;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogCore.AccesoDatos.Data.Inicializado
{
    public class InicializadorBD : IInicializadorBD
    {

        private readonly ApplicationDbContext _bd;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _rolManager;

        public InicializadorBD(ApplicationDbContext bd, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> rolManager)
        {
            _bd = bd;
            _userManager = userManager;
            _rolManager = rolManager;


        }
        public void Inicializar()
        {
            try
            {
                if (_bd.Database.GetPendingMigrations().Count() > 0)
                {
                    _bd.Database.Migrate();
                }
            }
            catch (Exception)
            {

            }

            if (_bd.Roles.Any(ro => ro.Name == CNT.Administrador)) return;

            //crear los roles
            _rolManager.CreateAsync(new IdentityRole(CNT.Administrador)).GetAwaiter().GetResult();
            _rolManager.CreateAsync(new IdentityRole(CNT.Registrado)).GetAwaiter().GetResult();
            _rolManager.CreateAsync(new IdentityRole(CNT.Cliente)).GetAwaiter().GetResult();

            //crear el usuario inicial
            _userManager.CreateAsync(new ApplicationUser
            {
                UserName = "defaultAdmin@gmail.com",
                Email= "defaultAdmin@gmail.com",
                EmailConfirmed=true,
                Nombre = "DefaultAdmin"
            }, "Admin123.").GetAwaiter().GetResult();


            //asignar el rol al usuario
            ApplicationUser usuario = _bd.ApplicationUser.Where(us => us.Email == "defaultAdmin@gmail.com").FirstOrDefault();
            _userManager.AddToRoleAsync(usuario, CNT.Administrador).GetAwaiter().GetResult();
            


        }
    }
}
