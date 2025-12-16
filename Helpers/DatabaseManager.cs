using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace llama.cpp_models_preset_manager.Helpers
{
    public class DatabaseManager
    {
        public static void init()
        {
            var _context = new DbContext();
            _context.Database.Migrate();
        }

        /*
         * Insert or update an entity that has Id key.
         * Return true if inserted, false otherwise.
         */
        public static bool Upsert<T>(T entity) where T : class
        {
            bool inserted = false;

            var _context = new DbContext();

            var entry = _context.Entry(entity);
            
            var idProperty = entity.GetType().GetProperty("Id");
            if (idProperty != null && idProperty.PropertyType == typeof(int))
            {
                int id = (int?)idProperty.GetValue(entity) ?? 0;
                if (id == 0)
                {
                    _context.Set<T>().Add(entity);
                    inserted = true;
                }
                else
                {
                    // Check if it's already tracked
                    var local = _context.Set<T>().Local.FirstOrDefault(e => 
                    {
                        var eId = (int?)e.GetType().GetProperty("Id")?.GetValue(e) ?? 0;
                        return eId == id;
                    });

                    if (local != null)
                    {
                        // If it's already tracked, we might need to copy values if 'entity' is a different instance
                        if (!ReferenceEquals(local, entity))
                        {
                            _context.Entry(local).CurrentValues.SetValues(entity);
                        }
                    }
                    else
                    {
                        _context.Set<T>().Update(entity);
                    }
                }
            }
            else
            {
                // Fallback or handle other key types 
                _context.Set<T>().Update(entity);
            }

            _context.SaveChanges();
            return inserted;
        }

        public static void Delete<T>(T entity) where T : class
        {
            var _context = new DbContext();

            if (_context.Entry(entity).State == EntityState.Detached)
            {
                _context.Set<T>().Attach(entity);
            }
            _context.Set<T>().Remove(entity);
            _context.SaveChanges();
        }
    }
}
