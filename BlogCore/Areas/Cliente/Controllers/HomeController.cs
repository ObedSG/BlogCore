using BlogCore.AccesoDatos.Data.Repository.IRepository;
using BlogCore.Models;
using BlogCore.Models.ViewModels;
using Microsoft.AspNetCore.JsonPatch.Internal;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BlogCore.Areas.Cliente.Controllers
{
    [Area("cliente")]
    public class HomeController : Controller
    {
        //private readonly ILogger<HomeController> _logger;
        private readonly IContenedorTrabajo _contenedorTrabajo;
        public HomeController(IContenedorTrabajo contenedorTrabajo)
        {
            _contenedorTrabajo = contenedorTrabajo;
        }


        public IActionResult Index()
        {
            HomeVM homeVm = new HomeVM()
            {
                Sliders = _contenedorTrabajo.Slider.GetAll(),
                ListaArticulos = _contenedorTrabajo.Articulo.GetAll()
            };

            ViewBag.IsHome = true; // Para indicar que estamos en la pagina de inicio

            return View(homeVm);
        }

        //para buscador
        public IActionResult ResultadoBusqueda(string searchString, int page =1,int pageSize=6)
        {
            var articulos = _contenedorTrabajo.Articulo.AsQueryable();

            //Filtar por titulo si hay un termino de búsqueda
            if(!string.IsNullOrEmpty(searchString))
            {
                articulos = articulos.Where(e => e.Nombre.Contains(searchString) || e.Descripcion.Contains(searchString));

            }

            //paginar los resultados

            var paginatedEntries= articulos
                .Skip((page - 1) * pageSize)
                .Take(pageSize);

            //crear el modelo de vista
            var model = new ListaPaginada<Articulo>(paginatedEntries.ToList(),
                articulos.Count(),
                page,
                pageSize,
                searchString);
            return View(model);
            
        }

        [HttpGet]
        public IActionResult Detalle(int id)
        {
            var articuloDesdeDb = _contenedorTrabajo.Articulo.Get(id);
            return View(articuloDesdeDb);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
