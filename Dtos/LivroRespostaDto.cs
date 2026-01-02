namespace Biblioteca.API.Dtos;

public class LivroRespostaDto
{
    public int Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public AutorRespostaDto? Autor { get; set; }
    public string Genero { get; set; } = string.Empty;
    public double Preco { get; set; }
}