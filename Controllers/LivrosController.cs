using Microsoft.AspNetCore.Mvc;
using Biblioteca.API.Models;
using Biblioteca.API.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Biblioteca.API.Dtos;

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

            var livrosDto = livros.Select(livro => new LivroRespostaDto
            {
                Id = livro.Id,
                Titulo = livro.Titulo,
                Autor = livro.Autor,
                Genero = livro.Genero,
                Preco = livro.Preco
            });
            return Ok(livrosDto);

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterPorId(int id)
        {

            var livro = await _context.Livros.FindAsync(id);

            if (livro == null) return NotFound();

            var livroDto = new LivroRespostaDto
            {
                Id = livro.Id,
                Titulo = livro.Titulo,
                Autor = livro.Autor,
                Genero = livro.Genero,
                Preco = livro.Preco
            };

            return Ok(livro);
        }

        [HttpPost]
        public async Task<IActionResult> Criar(LivroCriacaoDto livroDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var livroParaSalvar = new Livro
            {
                Titulo = livroDto.Titulo,
                Autor = livroDto.Autor,
                Genero = livroDto.Genero,
                Preco = livroDto.Preco
            };

            _context.Livros.Add(livroParaSalvar);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(ObterPorId), new { id = livroParaSalvar.Id }, livroParaSalvar);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(int id, LivroCriacaoDto livroDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            var livro = await _context.Livros.FindAsync(id);
            if (livro == null) return NotFound();

            livro.Titulo = livroDto.Titulo;
            livro.Autor = livroDto.Autor;
            livro.Genero = livroDto.Genero;
            livro.Preco = livroDto.Preco;

            await _context.SaveChangesAsync();

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