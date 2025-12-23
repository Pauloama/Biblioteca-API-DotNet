using Biblioteca.API.Models;

namespace Biblioteca.API.Repositories.Interfaces;

public interface ILivroRepository
{
    Task<List<Livro>> ObterTodos();

    Task<Livro?> ObterPorId(int id);

    Task<Livro> Adicionar(Livro livro);

    Task<Livro> Atualizar(Livro livro);

    Task<bool> Deletar(int id);
}