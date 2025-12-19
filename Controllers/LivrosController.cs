using Microsoft.AspNetCore.Mvc;
using Biblioteca.API.Models;
using Biblioteca.API.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LivrosController : ControllerBase
    {
        private readonly BibliotecaContext _context;

        public LivrosController(BibliotecaContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> ObterTodos()
        {
            var livros = await _context.Livros.ToListAsync();
            return Ok(livros);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterPorId(int id)
        {

            var livro = _context.Livros.Find(id);

            if (livro == null) return NotFound();

            return Ok(livro);
        }

        [HttpPost]
        public async Task<IActionResult> Criar(Livro livro)
        {
            _context.Livros.Add(livro);

            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(ObterPorId), new {id = livro.Id}, livro);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(int id, Livro livroAtualizado)
        {
            var livro = _context.Livros.Find(id);
            if (livro == null) return NotFound();

            livro.Titulo = livroAtualizado.Titulo;
            livro.Autor = livroAtualizado.Autor;
            livro.Genero = livroAtualizado.Genero;
            livro.Preco = livroAtualizado.Preco;

            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Deletar(int id)
        {
            var livro = _context.Livros.Find(id);
            if (livro == null) return NotFound();

            _context.Livros.Remove(livro);
            _context.SaveChanges();

            return NoContent();
        }
    }
}