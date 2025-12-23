using Microsoft.AspNetCore.Mvc;
using Biblioteca.API.Models;
using Biblioteca.API.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Biblioteca.API.Dtos;
using AutoMapper;
using Biblioteca.API.Repositories.Interfaces;

namespace Biblioteca.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LivrosController : ControllerBase
    {
        private readonly ILivroRepository _repository;
        private readonly IMapper _mapper;

        public LivrosController(ILivroRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> ObterTodos()
        {
            var livros = await _repository.ObterTodos();

            var livrosDto = _mapper.Map<List<LivroRespostaDto>>(livros);

            return Ok(livrosDto);

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterPorId(int id)
        {

            var livro = await _repository.ObterPorId(id);

            if (livro == null) return NotFound();

            var livroDto = _mapper.Map<LivroRespostaDto>(livro);

            return Ok(livroDto);
        }

        [HttpPost]
        public async Task<IActionResult> Criar(LivroCriacaoDto livroDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var livro = _mapper.Map<Livro>(livroDto);

            await _repository.Adicionar(livro);

            return CreatedAtAction(nameof(ObterPorId), new { id = livro.Id }, livro);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(int id, LivroCriacaoDto livroDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);


            var livro = await _repository.ObterPorId(id);
            if (livro == null) return NotFound();

            _mapper.Map(livroDto, livro);

            await _repository.Atualizar(livro);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Deletar(int id)
        {
            var sucesso = await _repository.Deletar(id);
            if (!sucesso) return NotFound();

            return NoContent();
        }
    }
}