using BlogCore.AccesoDatos.Data.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogCore.AccesoDatos.Data.Repository
{
    public class Repository<T> : IRepository<T> where T : class

    {
        protected readonly DbContext Context;   
        internal DbSet<T> dbSet;

        public Repository(DbContext context)
        {
            Context=context;
            this.dbSet=context.Set<T>();
        }


        public void Add(T entity)
        {
            dbSet.Add(entity);
        }

        public T Get(int id)
        {
            return dbSet.Find(id);
            
        }

        public IEnumerable<T> GetAll(System.Linq.Expressions.Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, string? includeProperties = null)
        {
            //se crea una una consulta IQueryable<T> a partr del Dbset del contexto
            IQueryable<T> query = dbSet; 

            if(filter != null)
            {
                query = query.Where(filter);
            }

            //Se incluyen propiedades de navegacion si se proporcionan
            if(includeProperties != null)
            {
                //Se divide la cadena de propiedades por coma y se itera sobre ellas
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries ))
                {
                    query = query.Include(includeProperty);
                }

            }

            // Se aplica el ordenamiento si se proporciona
            if(orderBy != null)
            {
                // Se ejecuta la funcion de ordenamiento y se convierte la consulta en una lista
                return orderBy(query).ToList();
            }

            // Si no se proporciona ordenamiento, simplemente se convierte la consulta en una lista
            return query.ToList();
        }



        public T GetFirstOrDefault(System.Linq.Expressions.Expression<Func<T, bool>>? filter = null, string? includeProperties = null)
        {
            //se crea una una consulta IQueryable<T> a partr del Dbset del contexto
            IQueryable<T> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            //Se incluyen propiedades de navegacion si se proporcionan
            if (includeProperties != null)
            {
                //Se divide la cadena de propiedades por coma y se itera sobre ellas
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }

            }

            return query.FirstOrDefault();


        }

        public void Remove(int id)
        {
            T entityRemove = dbSet.Find(id);
           

        }

        public void Remove(T entity)
        {
            dbSet.Remove(entity);
        }
    }
}
