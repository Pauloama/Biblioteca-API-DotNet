using AutoMapper;
using Biblioteca.API.Models;
using Biblioteca.API.Dtos;

namespace Biblioteca.API.Profiles;

public class LivroProfile : Profile
{
    public LivroProfile()
    {
        CreateMap<Livro, LivroRespostaDto>();

        CreateMap<LivroCriacaoDto, Livro>();
    }
}