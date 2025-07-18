using BlogCore.AccesoDatos.Data.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BlogCore.Areas.Admin.Controllers
{
    [Authorize(Roles = "Administrador")]

    [Area("Admin")]

    public class UsuariosController : Controller
    {

        private IContenedorTrabajo _contenedorTrabajo;

        public UsuariosController(IContenedorTrabajo contenedorTrabajo)
        {
            _contenedorTrabajo = contenedorTrabajo;
        }


        [HttpGet]
        public IActionResult Index()
        {
            //Opcion 1 para obtener todos los usuarios
            //return View(_contenedorTrabajo.Usuario.GetAll());

            //Opcion 2 para obtener todos los usuarios menos el que esta logueado
            var claimsIdentity = (ClaimsIdentity)this.User.Identity;
            var usuarioActual = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            return View(_contenedorTrabajo.Usuario.GetAll(u => u.Id != usuarioActual.Value));

        }

        [HttpGet]
        public IActionResult Bloquear(string id)
        {
            if (id == null)
            {
                return NotFound();

            }
            _contenedorTrabajo.Usuario.BloquearUsuarios(id);
            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public IActionResult Desbloquear(string id)
        {
            if (id == null)
            {
                return NotFound();

            }
            _contenedorTrabajo.Usuario.DesbloquearUsuario(id);
            return RedirectToAction(nameof(Index));
        }

    }
}
