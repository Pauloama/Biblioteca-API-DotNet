using System.ComponentModel.DataAnnotations;

namespace Biblioteca.API.Dtos;
public class AutorCriacaoDto
{
    [Required(ErrorMessage = "É preciso incluir o nome do autor.")]
    [StringLength(100, ErrorMessage = "O nome não pode ser maior que 100 caracteres.")]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "O autor deve possuir uma nacionalidade")]
    public string Nacionalidade { get; set; } = string.Empty;
}
