using BlogCore.AccesoDatos.Data.Repository.IRepository;
using BlogCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogCore.AccesoDatos.Data.Repository
{

    
    /// <summary>
    /// Clase que implementa el patrón Unit of Work para gestionar las operaciones de base de datos
    /// y mantener la consistencia entre múltiples repositorios.
    /// </summary>
    public class ContenedorTrabajo: IContenedorTrabajo
    {
        private readonly ApplicationDbContext _db;
       
        /// <summary>
        /// Constructor que inicializa el contenedor de trabajo con el contexto de base de datos
        /// y los repositorios necesarios.
        /// </summary>
        /// <param name="db">Contexto de la base de datos</param>
        public ContenedorTrabajo(ApplicationDbContext db)
        {
            _db = db;
            Categoria = new CategoriaRepository(_db);  //Inicialización del repositorio de categorías
            Articulo = new ArticuloRepository(_db);
        }

        /// <summary>
        /// Propiedad que expone el repositorio de categorías
        /// </summary>
        public ICategoriaRepository Categoria { get; private set; }
        public IArticuloRepository Articulo { get; private set; }

        /// <summary>
        /// Libera los recursos utilizados por el contexto de base de datos
        /// </summary>
        public void Dispose()
        {
            _db.Dispose();
            
        }

        /// <summary>
        /// Guarda todos los cambios realizados en el contexto de la base de datos
        /// </summary>
        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
