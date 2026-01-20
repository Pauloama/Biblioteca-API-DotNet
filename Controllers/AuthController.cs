using Biblioteca.API.Dtos;
using Biblioteca.API.Models;
using Biblioteca.API.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;

namespace Biblioteca.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthRepository _authRepo;
    private readonly IMapper _mapper;

    public AuthController(IAuthRepository authRepo, IMapper mapper)
    {
        _authRepo = authRepo;
        _mapper = mapper;
    }

    [HttpPost("registrar")]
    public async Task<ActionResult> Registrar(UsuarioRegistroDto usuarioDto)
    {
        var usuarioParaCriar = _mapper.Map<Usuario>(usuarioDto);
        try
        {
            var usuarioId = await _authRepo.Registrar(usuarioParaCriar, usuarioDto.Password);
            return Ok(new { message = $"Usuário criado com sucesso! ID: {usuarioId}" });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

    }

    [HttpPost("login")]
    public async Task<ActionResult> Login(UsuarioRegistroDto usuarioDto)
    {
        try
        {
            var token = await _authRepo.Login(usuarioDto.Username, usuarioDto.Password);

            if (token == "Usuário não encontrado." || token == "Senha incorreta.")
                return BadRequest(token);

            return Ok(new { token = token });
        }
        catch (Exception ex)
        {
            return BadRequest("Erro ao realizar login: " + ex.Message);
        }
    }
}