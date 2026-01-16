using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Biblioteca.API.Data;
using Biblioteca.API.Models;
using Biblioteca.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Biblioteca.API.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly BibliotecaContext _context;
        private readonly IConfiguration _configuration;

        public AuthRepository(BibliotecaContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<int> Registrar(Usuario usuario, string password)
        {
            if (await UsuarioExiste(usuario.Username))
                throw new Exception("Usuário já existe!");

            CriarPasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

            usuario.PasswordHash = passwordHash;
            usuario.PasswordSalt = passwordSalt;

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return usuario.Id;
        }

        public async Task<string> Login(string username, string password)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(x => x.Username.ToLower() == username.ToLower());

            if (usuario == null)
                return "Usuário não encontrado.";

            if (!VerificarPasswordHash(password, usuario.PasswordHash, usuario.PasswordSalt))
                return "Senha incorreta.";

            return CriarToken(usuario);
        }

        public async Task<bool> UsuarioExiste(string username)
        {
            if (await _context.Usuarios.AnyAsync(x => x.Username.ToLower() == username.ToLower()))
            {
                return true;
            }
            return false;
        }


        private string CriarToken(Usuario usuario)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Name, usuario.Username)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
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
}