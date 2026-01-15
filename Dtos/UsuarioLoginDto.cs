using System.ComponentModel.DataAnnotations;

namespace Biblioteca.API.Dtos;

public class UsuarioUsuarioDto
{
    [Required(ErrorMessage = "O nome de usuário é obrigatório!")]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "A senha é obrigatória!")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "A senha deve ter no mínimo 6 caracteres")]
    public string Password { get; set; } = string.Empty;
}