using AppLogin.Models;
using Microsoft.EntityFrameworkCore;

namespace AppLogin.Data
{
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions<DBContext> options) : base(options)
        {

        }
        //Trae a los usuarios completos de la base de datos
        public DbSet<User> Users { get; set; }

        //Metodo para llamar e inicialize la base de datos
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base .OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>(td =>
            {
                td.HasKey(col => col.Id);
                td.Property(col => col.Id).UseIdentityColumn().ValueGeneratedOnAdd();

                td.Property(col => col.Name).HasMaxLength(50);
                td.Property(col => col.Email).HasMaxLength(50);
                td.Property(col => col.Password).HasMaxLength(250);
            }
            );

            //LLena la tabla
            modelBuilder.Entity<User>().ToTable("users");
        }
    }
}
