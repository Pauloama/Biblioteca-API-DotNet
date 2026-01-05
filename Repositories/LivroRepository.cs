using Biblioteca.API.Data;
using Biblioteca.API.Models;
using Biblioteca.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca.API.Repositories;

public class LivroRepository : ILivroRepository
{
    private readonly BibliotecaContext _context;

    public LivroRepository(BibliotecaContext context)
    {
        _context = context;
    }

    public async Task<List<Livro>> ObterTodos()
    {
        return await _context.Livros
        .Include(x => x.Autor)
        .ToListAsync();
    }

    public async Task<Livro?> ObterPorId(int id)
    {
        return await _context.Livros
        .Include(x => x.Autor)
        .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Livro> Adicionar(Livro livro)
    {
        await _context.Livros.AddAsync(livro);
        await _context.SaveChangesAsync();
        return livro;
    }

    public async Task<Livro> Atualizar(Livro livro)
    {
        _context.Livros.Update(livro);
        await _context.SaveChangesAsync();
        return livro;
    }

    public async Task<bool> Deletar(int id)
    {
        var livro = await _context.Livros.FindAsync(id);
        if (livro == null)
        {
            return false;
        }

        _context.Livros.Remove(livro);
        await _context.SaveChangesAsync();
        return true;
    }
}