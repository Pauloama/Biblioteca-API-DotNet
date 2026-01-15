using System.Security.Cryptography;
using System.Text;
using Biblioteca.API.Data;
using Biblioteca.API.Models;
using Biblioteca.API.Repositories.Interfaces;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca.API.Repositories;
public class AuthRepository : IAuthRepository
{
    private readonly BibliotecaContext _context;

    public AuthRepository(BibliotecaContext context)
    {
        _context = context;
    }

    public async Task<int> Registrar(Usuario usuario, string password)
    {
        if (await UsuarioExiste(usuario.Username))
            throw new Exception("Usuário já existe!");

        CriarPasswordHash(password, out byte[] passwordHash, out byte[] passwrodSalt);
        usuario.PasswordHash = passwordHash;
        usuario.PasswordSalt = passwrodSalt;

        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();

        return usuario.Id;
    }

    public async Task<string> Login(string username, string password)
    {
        var usuario = await _context.Usuarios.FirstOrDefaultAsync(x => x.Username.ToLower() == username.ToLower());
        if (usuario == null)
            return "Usuário não encontrado..";

        if (!VerificarPasswordHash(password, usuario.PasswordHash, usuario.PasswordSalt))
            return "Senha incorreta";

        return "token_de_teste_temporário";
    }
    public async Task<bool> UsuarioExiste(string username)
    {
        if (await _context.Usuarios.AnyAsync(x => x.Username.ToLower() == username.ToLower()))
        {
            return true;
        }
        return false;
    }

    private void CriarPasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using (var hmac = new HMACSHA512())
        {
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }
    }

    private bool VerificarPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
    {
        using (var hmac = new HMACSHA512(passwordSalt))
        {
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != passwordHash[i]) return false;
            }
            return true;
        }
    }
}