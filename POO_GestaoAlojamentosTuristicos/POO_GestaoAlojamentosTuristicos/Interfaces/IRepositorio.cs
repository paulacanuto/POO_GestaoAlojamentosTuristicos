using System.Collections.Generic;

namespace POO_GestaoAlojamentosTuristicos.Interfaces
{
    /// <summary>
    /// Interface genérica para operações CRUD (Create, Read, Update, Delete)
    /// Pattern Repository - abstrai a camada de dados
    /// </summary>
    /// <typeparam name="T">Tipo de entidade</typeparam>
    public interface IRepositorio<T> where T : class
    {
        // Create
        bool Adicionar(T entidade);

        // Read
        T ObterPorId(int id);
        List<T> ObterTodos();

        // Update
        bool Atualizar(T entidade);

        // Delete
        bool Remover(int id);

        // Persistência
        bool Guardar();
        bool Carregar();
    }
}