using Microsoft.AspNetCore.Mvc;
using Biblioteca.API.Models;
using Biblioteca.API.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Biblioteca.API.Dtos;
using AutoMapper;
using Biblioteca.API.Repositories.Interfaces;

namespace Biblioteca.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AutorController : ControllerBase
{
    private readonly IAutorRepository _repository;
    private readonly IMapper _mapper;

    public AutorController(IAutorRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> ObterTodos()
    {
        var autores = await _repository.ObterTodos();

        var autoresDto = _mapper.Map<List<AutorRespostaDto>>(autores);

        return Ok(autoresDto);

    }

    [HttpGet("{id}")]
    public async Task<IActionResult> ObterPorId(int id)
    {

        var autor = await _repository.ObterPorId(id);

        if (autor == null) return NotFound();

        var autorDto = _mapper.Map<AutorRespostaDto>(autor);

        return Ok(autorDto);
    }

    [HttpPost]
    public async Task<IActionResult> Criar(AutorCriacaoDto autorDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var autor = _mapper.Map<Autor>(autorDto);

        await _repository.Adicionar(autor);

        return CreatedAtAction(nameof(ObterPorId), new { id = autor.Id }, autor);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Atualizar(int id, AutorCriacaoDto autorDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);


        var autor = await _repository.ObterPorId(id);
        if (autor == null) return NotFound();

        _mapper.Map(autorDto, autor);

        await _repository.Atualizar(autor);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<AutorRespostaDto>> Deletar(int id)
    {
        try
        {
            var autorDeletado = await _repository.Deletar(id);
            if (!autorDeletado)
            {
                return NotFound("Autor não encontrado.");
            }
            return NoContent();
        }
        catch (Exception)
        {
            return BadRequest("Não é possível apagar este autor pois ele poussui livros cadastrados");
        }
    }
}
