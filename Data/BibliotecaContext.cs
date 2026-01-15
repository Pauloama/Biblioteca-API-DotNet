using Microsoft.EntityFrameworkCore;
using Biblioteca.API.Models;

namespace Biblioteca.API.Data;

public class BibliotecaContext : DbContext
{
    public BibliotecaContext(DbContextOptions<BibliotecaContext> options) : base(options)
    {
    }

    public DbSet<Livro> Livros { get; set; }
    public DbSet<Autor> Autores { get; set; }

    public DbSet<Usuario> Usuarios { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Livro>()
        .HasOne(l => l.Autor)
        .WithMany(a => a.Livros)
        .HasForeignKey(l => l.AutorId)
        .OnDelete(DeleteBehavior.Restrict);
    }
}
