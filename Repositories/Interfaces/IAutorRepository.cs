using Biblioteca.API.Models;

namespace Biblioteca.API.Repositories.Interfaces;

public interface IAutorRepository
{
    Task<List<Autor>> ObterTodos();

    Task<Autor?> ObterPorId(int id);

    Task<Autor> Adicionar(Autor autor);

    Task<Autor> Atualizar(Autor autor);

    Task<bool> Deletar(int id);
}