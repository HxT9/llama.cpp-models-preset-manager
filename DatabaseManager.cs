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
        private static DatabaseManager? _instance;
        private static readonly object _padlock = new object();

        public static DatabaseManager Instance
        {
            get
            {
                lock (_padlock)
                {
                    if (_instance == null)
                    {
                        _instance = new DatabaseManager();
                    }
                    return _instance;
                }
            }
        }

        private DbContext _dbContext;
        public DbContext DbContext
        {
            get { return _dbContext; }
        }

        /*
         * Apply migrations to database
         */
        public DatabaseManager()
        {
            _dbContext = new DbContext();
        }

        public static void init()
        {
            Instance.DbContext.Database.Migrate();
        }

        /*
         * Insert or update an entity that has Id as key.
         */
        public static bool Upsert<T>(T entity) where T : class
        {
            var entry = Instance.DbContext.Entry(entity);
            
            var idProperty = entity.GetType().GetProperty("Id");
            if (idProperty != null && idProperty.PropertyType == typeof(int))
            {
                int id = (int?)idProperty.GetValue(entity) ?? 0;
                if (id == 0)
                {
                    Instance.DbContext.Set<T>().Add(entity);
                }
                else
                {
                    // Check if it's already tracked
                    var local = Instance.DbContext.Set<T>().Local.FirstOrDefault(e => 
                    {
                        var eId = (int?)e.GetType().GetProperty("Id")?.GetValue(e) ?? 0;
                        return eId == id;
                    });

                    if (local != null)
                    {
                        // If it's already tracked, we might need to copy values if 'entity' is a different instance
                        if (!ReferenceEquals(local, entity))
                        {
                            Instance.DbContext.Entry(local).CurrentValues.SetValues(entity);
                        }
                    }
                    else
                    {
                        Instance.DbContext.Set<T>().Update(entity);
                    }
                }
            }
            else
            {
                // Fallback or handle other key types 
                Instance.DbContext.Set<T>().Update(entity);
            }

            Instance.DbContext.SaveChanges();

            return true;
        }

        public static void Delete<T>(T entity) where T : class
        {
            if (Instance.DbContext.Entry(entity).State == EntityState.Detached)
            {
                Instance.DbContext.Set<T>().Attach(entity);
            }
            Instance.DbContext.Set<T>().Remove(entity);
            Instance.DbContext.SaveChanges();
        }

        //Undo pending changes
        public static void UndoChanges()
        {
            var changedEntries = Instance.DbContext.ChangeTracker.Entries().Where(e => e.State != EntityState.Unchanged).ToList();
            foreach (var entry in changedEntries)
            {
                switch (entry.State)
                {
                    case EntityState.Modified:
                        entry.CurrentValues.SetValues(entry.OriginalValues);
                        entry.State = EntityState.Unchanged;
                        break;
                    case EntityState.Added:
                        entry.State = EntityState.Detached;
                        break;
                    case EntityState.Deleted:
                        entry.State = EntityState.Unchanged;
                        break;
                }
            }
        }
    }
}
