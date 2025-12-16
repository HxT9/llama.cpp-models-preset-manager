using llama.cpp_models_preset_manager.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace llama.cpp_models_preset_manager
{
    public class DbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public DbSet<KVConfig> KVConfig { get; set; }
        public DbSet<AiModel> AIModel { get; set; }
        public DbSet<Flag> Flag { get; set; }
        public DbSet<AiModelFlag> AIModelFlag { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=llama.cpp models-preset manager.sqlite;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<KVConfig>()
                .HasIndex(c => c.Key)
                .IsUnique();

            modelBuilder.Entity<AiModel>()
                .HasMany(m => m.Flags)
                .WithOne(f => f.AIModel)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<AiModel>()
                .HasIndex(m => m.Name)
                .IsUnique();

            modelBuilder.Entity<Flag>()
                .HasIndex(f => f.Name)
                .IsUnique();

            base.OnModelCreating(modelBuilder);
        }
    }
}
