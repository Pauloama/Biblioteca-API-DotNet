using Biblioteca.API.Repositories.Interfaces;
using Biblioteca.API.Data;
using Biblioteca.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca.API.Repositories;

public class AutorRepository : IAutorRepository
{
    private readonly BibliotecaContext _context;

    public AutorRepository(BibliotecaContext context)
    {
        _context = context;
    }

    public async Task<List<Autor>> ObterTodos()
    {
        return await _context.Autores.ToListAsync();
    }

    public async Task<Autor?> ObterPorId(int id)
    {
        return await _context.Autores.FindAsync(id);
    }

    public async Task<Autor> Adicionar(Autor autor)
    {
        await _context.Autores.AddAsync(autor);
        await _context.SaveChangesAsync();
        return autor;
    }

    public async Task<Autor> Atualizar(Autor autor)
    {
        _context.Autores.Update(autor);
        await _context.SaveChangesAsync();
        return autor;
    }

    public async Task<bool> Deletar(int id)
    {
        var autor = await _context.Autores.FindAsync(id);
        if (autor == null) return false;
        _context.Autores.Remove(autor);
        await _context.SaveChangesAsync();
        return true;
    }
}