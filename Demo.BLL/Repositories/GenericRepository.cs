using Demo.BLL.Interfaces;
using Demo.DAL.Data;
using Demo.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private protected readonly AppDbContext _context; // NULL
        public GenericRepository(AppDbContext context) // Ask CLR Create Object From AppDbContext
        {
            _context = context;
        }
        public async Task Add(T entity)
        {
           await _context.AddAsync(entity);
        }

        public void Delete(T entity)
        {
            _context.Remove(entity);
        }

        public async Task<T> Get(int id)
        {
           var result = await  _context.Set<T>().FindAsync(id);
            return result;
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            //var result = _context.Set<T>().ToList();

            if(typeof(T) == typeof(Employee))
            {
                return (IEnumerable<T>) await _context.Employees.Include(E => E.Department).ToListAsync();
            }

            return await _context.Set<T>().ToListAsync();
         
        }

        public void Update(T entity)
        {
            _context.Update(entity);
        }
    }
}
