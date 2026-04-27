using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoApi.Domain.Entities;

namespace ToDoApi.Infrastructure.Data;

    public class AppDbContext : DbContext
    {

     public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

      public DbSet<Todo> Todos { get; set; }
      public DbSet<User> Users { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
     {
        modelBuilder.Entity<Todo>(entity =>
        {
            entity.HasKey(t => t.Id);

            entity.Property(t => t.Title)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(t => t.Description)
                .HasMaxLength(500);

            entity.Property(t => t.CreatedAt)
                .IsRequired();
        });

        modelBuilder.Entity<User>(entity =>
        {

            entity.HasKey(u => u.Id);

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);

                entity.Property(u => u.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(u => u.Email)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(u => u.PasswordHash)
                    .IsRequired();

                entity.HasIndex(u => u.Email)
                    .IsUnique();
            
                entity.Property(u => u.Role)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasDefaultValue("User");




            });











        });
        
        
        
        
        
        
        
        
        
        
        
        
        
    }
}




