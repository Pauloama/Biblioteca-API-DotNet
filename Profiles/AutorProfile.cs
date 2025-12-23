using AutoMapper;
using Biblioteca.API.Dtos;
using Biblioteca.API.Models;

namespace Biblioteca.API.Profiles;

public class AutorProfile : Profile
{
    public AutorProfile()
    {
        CreateMap<Autor, AutorRespostaDto>();
        CreateMap<AutorCriacaoDto, Autor>();
    }
}