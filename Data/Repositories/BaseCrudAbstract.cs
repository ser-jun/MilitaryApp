using Microsoft.EntityFrameworkCore;
using MilitaryApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MilitaryApp.DTO;

namespace MilitaryApp.Data.Repositories
{
    public class BaseCrudAbstract <T> : ICrudRepository<T> where T : class
    {
        protected readonly MilitaryDbContext _context;
        private readonly DbSet<T> _dbSet;

        public BaseCrudAbstract(MilitaryDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }
        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }
        public async Task<T> AddAsync(T entity)
        {
            try
            {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
            }
            catch (DbUpdateException ex )
            {
                MessageBox.Show(ex.InnerException?.Message);
                throw;   
            }
        }
        public async Task<T> UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
        public async Task DeleteAsync(T entity)
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }


    }
}
