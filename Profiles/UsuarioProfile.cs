using AutoMapper;
using Biblioteca.API.Dtos;
using Biblioteca.API.Models;

namespace Biblioteca.API.Profiles;

public class UsuarioProfile : Profile
{
    public UsuarioProfile()
    {
        CreateMap<UsuarioRegistroDto, Usuario>();
    }
}