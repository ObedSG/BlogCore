using BlogCore.AccesoDatos.Data.Repository.IRepository;
using BlogCore.Models;
using Microsoft.AspNetCore.Mvc;

namespace BlogCore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoriasController : Controller
    {
        private IContenedorTrabajo _contenedorTrabajo;

        public CategoriasController(IContenedorTrabajo contenedorTrabajo)
        {
            _contenedorTrabajo = contenedorTrabajo;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Categoria categoria)
        {
            if (ModelState.IsValid)
            {
                //logica para guardar en BD
                _contenedorTrabajo.Categoria.Add(categoria);
                _contenedorTrabajo.Save();

                return RedirectToAction(nameof(Index));
            }

            return View();
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            Categoria categoria = new Categoria();
            categoria = _contenedorTrabajo.Categoria.Get(id);
            
            if(categoria == null)
            {
                return NotFound();

            }

            return View(categoria);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit (Categoria categoria)
        {
            if (ModelState.IsValid)
            {
                //logica para actualizar en BD
                _contenedorTrabajo.Categoria.Update(categoria); //cambiamos a solo update
                _contenedorTrabajo.Save();

                return RedirectToAction(nameof(Index));
            }

            return View();
        }




        #region llamadas a la API

        [HttpGet]
        public IActionResult GetAll()
        {
            var categorias = _contenedorTrabajo.Categoria.GetAll().Select(c => new {
                Id = c.Id,
                Nombre = c.Nombre,
                Orden = c.Orden
            });
            return Json(new { data = categorias });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var ObjDeleteDB = _contenedorTrabajo.Categoria.Get(id);
            if (ObjDeleteDB == null)
            {
                return Json(new { success = false, message = "Error al eliminar Categoria" });
            }
            else
            _contenedorTrabajo.Categoria.Remove(ObjDeleteDB);
            _contenedorTrabajo.Save();
            return Json(new { success = true, message = "Categoria borrada correctamente" });
               
        }
        #endregion

    }
}
