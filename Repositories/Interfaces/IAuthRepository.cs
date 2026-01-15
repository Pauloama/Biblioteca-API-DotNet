using Biblioteca.API.Models;

namespace Biblioteca.API.Repositories.Interfaces;

public interface IAuthRepository
{
    Task<int> Registrar(Usuario usuario, string password);
    Task<string> Login(string username, string password);
    Task<bool> UsuarioExiste(string username);
}