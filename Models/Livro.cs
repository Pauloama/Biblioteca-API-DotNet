using System.ComponentModel.DataAnnotations;

namespace Biblioteca.API.Models;

public class Livro
{
    public int Id { get; set; }

    [Required(ErrorMessage = "O título é obrigatório")]
    [StringLength(100, ErrorMessage = "O título não pode ter mais de 100 caracteres.")]
    public string Titulo { get; set; } = string.Empty;

    public int AutorId { get; set; }

    public Autor? Autor {get; set;} 

    [StringLength(50,  ErrorMessage = "O gênero não pode passar de 50 caracteres.")]
    public string Genero { get; set; } = string.Empty;

    [Range(0.01, 1000.00, ErrorMessage = "O preço deve ser entre 1 centavo e 1000 reais.")]
    public double Preco { get; set; }
}