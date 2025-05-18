using BlogCore.AccesoDatos.Data.Repository.IRepository;
using BlogCore.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogCore.Areas.Admin.Controllers
{
    //[Authorize(Roles = "Administrador")]
    [Area("Admin")]
    public class ArticulosController : Controller
    {
        private readonly IContenedorTrabajo _contenedorTrabajo;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public ArticulosController(IContenedorTrabajo contenedorTrabajo, 
            IWebHostEnvironment hostingEnvironment)
        {
            _contenedorTrabajo = contenedorTrabajo;
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            ArticuloVM artiVM = new ArticuloVM()
            {
                //Articulo = new BlogCore.Models.Articulo(),
                Articulo = new BlogCore.Models.Articulo()
                {
                    Nombre = "", // opcional
                    Descripcion = "", // opcional
                    CategoriaId = 0, // aún no seleccionado
                    UrlImagen = "", // inicializado para evitar error en ModelState
                    FechaCreacion = DateTime.Now.ToString() // si es requerido
                },
                ListaCategorias = _contenedorTrabajo.Categoria.GetListaCategorias()
            };


            return View(artiVM);
        }


            [HttpPost]
            [ValidateAntiForgeryToken]
            public IActionResult Create(ArticuloVM artiVM)
            {
                // 1. Ver valores que llegaron antes del ModelState.IsValid
                Console.WriteLine("---- Valores recibidos ----");
                Console.WriteLine($"Nombre: {artiVM.Articulo.Nombre}");
                Console.WriteLine($"Descripcion: {artiVM.Articulo.Descripcion}");
                Console.WriteLine($"CategoriaId: {artiVM.Articulo.CategoriaId}");
                Console.WriteLine($"UrlImagen: {artiVM.Articulo.UrlImagen ?? "null"}");
                Console.WriteLine($"FechaCreacion: {artiVM.Articulo.FechaCreacion ?? "null"}");
                Console.WriteLine($"ListaCategorias: {(artiVM.ListaCategorias == null ? "null" : artiVM.ListaCategorias.Count().ToString())}");
            var archivos = HttpContext.Request.Form.Files;


            if (archivos == null || archivos.Count == 0)
            {
                ModelState.AddModelError("Articulo.UrlImagen", "Debes seleccionar una imagen.");
            }

            if (ModelState.IsValid)
            {
                string rutaPrincipal = _hostingEnvironment.WebRootPath;

                // Validación manual de imagen
               

                if (artiVM.Articulo.Id == 0 && archivos.Count() > 0)
                {
                    //Nuevo articulo
                    string nombreArchivo = Guid.NewGuid().ToString();
                    var subidas = Path.Combine(rutaPrincipal, @"imagenes\articulos");
                    var extension = Path.GetExtension(archivos[0].FileName);

                    using (var fileStreams = new FileStream(Path.Combine(subidas, nombreArchivo + extension), FileMode.Create))
                    {
                        archivos[0].CopyTo(fileStreams);
                    }

                    artiVM.Articulo.UrlImagen = @"\imagenes\articulos\" + nombreArchivo + extension;
                    artiVM.Articulo.FechaCreacion = DateTime.Now.ToString();

                    _contenedorTrabajo.Articulo.Add(artiVM.Articulo);
                    _contenedorTrabajo.Save();

                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("archivos", "Debes seleccionar una imagen");
                    artiVM.ListaCategorias = _contenedorTrabajo.Categoria.GetListaCategorias();
                    return View(artiVM);
                }

            }
            
            
                artiVM.ListaCategorias = _contenedorTrabajo.Categoria.GetListaCategorias();

            //foreach (var estado in ModelState)
            //{
            //    Console.WriteLine($"Campo: {estado.Key}");
            //    foreach (var error in estado.Value.Errors)
            //    {
            //        Console.WriteLine($"  - Error: {error.ErrorMessage}");
            //    }
            //}

            return View(artiVM);
            

               
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            ArticuloVM artiVM = new ArticuloVM()
            {
                Articulo = new BlogCore.Models.Articulo(),
                ListaCategorias = _contenedorTrabajo.Categoria.GetListaCategorias()
            };

            if (id != null)
            {
                artiVM.Articulo = _contenedorTrabajo.Articulo.Get(id.GetValueOrDefault());
            }

            return View(artiVM);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ArticuloVM artiVM)
        {
            if (ModelState.IsValid)
            {
                string rutaPrincipal = _hostingEnvironment.WebRootPath;
                var archivos = HttpContext.Request.Form.Files;

                var articuloDesdeBd = _contenedorTrabajo.Articulo.Get(artiVM.Articulo.Id);


                if (archivos.Count() > 0)
                {
                    //Nuevo imagen para el artículo
                    string nombreArchivo = Guid.NewGuid().ToString();
                    var subidas = Path.Combine(rutaPrincipal, @"imagenes\articulos");
                    var extension = Path.GetExtension(archivos[0].FileName);
                    var nuevaExtension = Path.GetExtension(archivos[0].FileName);

                    var rutaImagen = Path.Combine(rutaPrincipal, articuloDesdeBd.UrlImagen.TrimStart('\\'));

                    if (System.IO.File.Exists(rutaImagen))
                    {
                        System.IO.File.Delete(rutaImagen);
                    }

                    //Nuevamente subimos el archivo
                    using (var fileStreams = new FileStream(Path.Combine(subidas, nombreArchivo + extension), FileMode.Create))
                    {
                        archivos[0].CopyTo(fileStreams);
                    }

                    artiVM.Articulo.UrlImagen = @"\imagenes\articulos\" + nombreArchivo + extension;
                    artiVM.Articulo.FechaCreacion = DateTime.Now.ToString();

                    _contenedorTrabajo.Articulo.Update(artiVM.Articulo);
                    _contenedorTrabajo.Save();

                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    //Aquí sería cuando la imagen ya existe y se conserva
                    artiVM.Articulo.UrlImagen = articuloDesdeBd.UrlImagen;
                }

                _contenedorTrabajo.Articulo.Update(artiVM.Articulo);
                _contenedorTrabajo.Save();

                return RedirectToAction(nameof(Index));

            }

            artiVM.ListaCategorias = _contenedorTrabajo.Categoria.GetListaCategorias();
            return View(artiVM);
        }




        #region Llamadas a la API
        [HttpGet]
        public IActionResult GetAll()
        {
            return Json(new { data = _contenedorTrabajo.Articulo.GetAll(includeProperties: "Categoria") });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var articuloDesdeBd = _contenedorTrabajo.Articulo.Get(id);
            string rutaDirectorioPrincipal = _hostingEnvironment.WebRootPath;
            var rutaImagen = Path.Combine(rutaDirectorioPrincipal, articuloDesdeBd.UrlImagen.TrimStart('\\'));
            if (System.IO.File.Exists(rutaImagen))
            {
                System.IO.File.Delete(rutaImagen);
            }


            if (articuloDesdeBd == null)
            {
                return Json(new { success = false, message = "Error borrando artículo" });
            }

            _contenedorTrabajo.Articulo.Remove(articuloDesdeBd);
            _contenedorTrabajo.Save();
            return Json(new { success = true, message = "Artículo Borrado Correctamente" });
        }

        #endregion
    }
}
