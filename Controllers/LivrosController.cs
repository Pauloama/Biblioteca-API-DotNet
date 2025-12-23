using Microsoft.AspNetCore.Mvc;
using Biblioteca.API.Models;
using Biblioteca.API.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Biblioteca.API.Dtos;
using AutoMapper;

namespace Biblioteca.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LivrosController : ControllerBase
    {
        private readonly BibliotecaContext _context;
        private readonly IMapper _mapper;

        public LivrosController(BibliotecaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> ObterTodos()
        {
            var livros = await _context.Livros.ToListAsync();

            var livrosDto = _mapper.Map<List<LivroRespostaDto>>(livros);

            return Ok(livrosDto);

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterPorId(int id)
        {

            var livro = await _context.Livros.FindAsync(id);

            if (livro == null) return NotFound();

            var livroDto = _mapper.Map<LivroRespostaDto>(livro);

            return Ok(livro);
        }

        [HttpPost]
        public async Task<IActionResult> Criar(LivroCriacaoDto livroDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var livro = _mapper.Map<Livro>(livroDto);

            _context.Livros.Add(livro);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(ObterPorId), new { id = livro.Id }, livro);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(int id, LivroCriacaoDto livroDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);


            var livro = await _context.Livros.FindAsync(id);
            if (livro == null) return NotFound();

            _mapper.Map(livroDto, livro);

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