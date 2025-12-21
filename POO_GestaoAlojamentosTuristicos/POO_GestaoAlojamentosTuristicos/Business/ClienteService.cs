using System;
using System.Collections.Generic;
using System.Linq;
using POO_GestaoAlojamentosTuristicos.Models;
using POO_GestaoAlojamentosTuristicos.Data;
using POO_GestaoAlojamentosTuristicos.Exceptions;

namespace POO_GestaoAlojamentosTuristicos.Business
{
    /// <summary>
    /// Serviço de gestão de clientes
    /// </summary>
    public class ClienteService
    {
        private readonly RepositorioCliente repositorio;

        public ClienteService()
        {
            repositorio = new RepositorioCliente();

            // Carrega dados existentes
            try
            {
                repositorio.Carregar();
            }
            catch (PersistenciaException)
            {
                // Se falhar, inicia com repositório vazio
            }
        }

        /// <summary>
        /// Adiciona novo cliente
        /// </summary>
        public bool Adicionar(string nome, string email, string telefone = "")
        {
            try
            {
                int novoId = repositorio.GerarProximoId();
                var cliente = new Cliente(novoId, nome, email, telefone);

                repositorio.Adicionar(cliente);
                repositorio.Guardar();

                return true;
            }
            catch (Exception ex)
            {
                throw new AlojamentoException("Erro ao adicionar cliente.", ex);
            }
        }

        /// <summary>
        /// Atualiza dados do cliente
        /// </summary>
        public bool Atualizar(int id, string nome, string email, string telefone)
        {
            try
            {
                if (repositorio.EmailJaExiste(email, id))
                    throw new DadosInvalidosException("Email", "Este email já está em uso por outro cliente.");

                var clienteExistente = repositorio.ObterPorId(id);
                if (clienteExistente == null)
                    throw new DadosInvalidosException("Cliente não encontrado.");

                clienteExistente.Nome = nome;
                clienteExistente.Email = email;
                clienteExistente.Telefone = telefone;

                repositorio.Guardar(); // persiste no arquivo JSON

                return true;
            }
            catch (Exception ex)
            {
                throw new AlojamentoException("Erro ao atualizar cliente.", ex);
            }
        }

        /// <summary>
        /// Remove cliente
        /// </summary>
        public bool Remover(int id)
        {
            try
            {
                repositorio.Remover(id);
                repositorio.Guardar();
                return true;
            }
            catch (Exception ex)
            {
                throw new AlojamentoException("Erro ao remover cliente.", ex);
            }
        }

        /// <summary>
        /// Lista todos os clientes
        /// </summary>
        public List<Cliente> ListarTodos()
        {
            return repositorio.ObterTodos();
        }

        /// <summary>
        /// Busca cliente por ID
        /// </summary>
        public Cliente ObterPorId(int id)
        {
            return repositorio.ObterPorId(id);
        }

        /// <summary>
        /// Busca cliente por email
        /// </summary>
        public Cliente ObterPorEmail(string email)
        {
            return repositorio.ObterPorEmail(email);
        }

        /// <summary>
        /// Busca clientes por nome
        /// </summary>
        public List<Cliente> BuscarPorNome(string nome)
        {
            return repositorio.BuscarPorNome(nome);
        }

        /// <summary>
        /// Conta total de clientes
        /// </summary>
        public int ContarTotal()
        {
            return repositorio.Contar();
        }

        /// <summary>
        /// Verifica se email está disponível
        /// </summary>
        public bool EmailDisponivel(string email)
        {
            return !repositorio.EmailJaExiste(email);
        }

        /// <summary>
        /// Obtém clientes ordenados por nome
        /// </summary>
        public List<Cliente> ObterOrdenadosPorNome()
        {
            return repositorio.ObterTodos()
                .OrderBy(c => c.Nome)
                .ToList();
        }
    }
}
