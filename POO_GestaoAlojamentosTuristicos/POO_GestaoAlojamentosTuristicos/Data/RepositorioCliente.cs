using System;
using System.Collections.Generic;
using System.Linq;
using POO_GestaoAlojamentosTuristicos.Models;
using POO_GestaoAlojamentosTuristicos.Exceptions;

namespace POO_GestaoAlojamentosTuristicos.Data
{
    /// <summary>
    /// Repositório específico para Clientes
    /// </summary>
    public class RepositorioCliente : RepositorioBase<Cliente>
    {
        public RepositorioCliente() : base("clientes.json")
        {
        }

        /// <summary>
        /// Obtém cliente por ID
        /// </summary>
        public override Cliente ObterPorId(int id)
        {
            var cliente = entidades.FirstOrDefault(c => c.Id == id);

            if (cliente == null)
                throw new EntidadeNaoEncontradaException("Cliente", id);

            return cliente;
        }

        /// <summary>
        /// Atualiza cliente existente
        /// </summary>
        public override bool Atualizar(Cliente entidade)
        {
            if (entidade == null)
                throw new DadosInvalidosException("entidade", "A entidade não pode ser nula.");

            var indice = entidades.FindIndex(c => c.Id == entidade.Id);

            if (indice == -1)
                throw new EntidadeNaoEncontradaException("Cliente", entidade.Id);

            entidades[indice] = entidade;
            return true;
        }

        /// <summary>
        /// Remove cliente por ID
        /// </summary>
        public override bool Remover(int id)
        {
            var cliente = entidades.FirstOrDefault(c => c.Id == id);

            if (cliente == null)
                throw new EntidadeNaoEncontradaException("Cliente", id);

            return entidades.Remove(cliente);
        }

        /// <summary>
        /// Verifica se email já existe (evitar duplicados)
        /// </summary>
        public bool EmailJaExiste(string email, int? idIgnorar = null)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            return entidades.Any(c =>
                c.Email.Equals(email, StringComparison.OrdinalIgnoreCase) &&
                (!idIgnorar.HasValue || c.Id != idIgnorar.Value));
        }

        /// <summary>
        /// Busca cliente por email
        /// </summary>
        public Cliente ObterPorEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return null;

            return entidades.FirstOrDefault(c =>
                c.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Busca clientes por nome (parcial)
        /// </summary>
        public List<Cliente> BuscarPorNome(string nome)
        {
            if (string.IsNullOrWhiteSpace(nome))
                return new List<Cliente>();

            return entidades
                .Where(c => c.Nome.Contains(nome, StringComparison.OrdinalIgnoreCase))
                .OrderBy(c => c.Nome)
                .ToList();
        }

        /// <summary>
        /// Gera próximo ID disponível
        /// </summary>
        public int GerarProximoId()
        {
            return entidades.Any() ? entidades.Max(c => c.Id) + 1 : 1;
        }

        /// <summary>
        /// Adiciona com verificação de email duplicado
        /// </summary>
        public override bool Adicionar(Cliente entidade)
        {
            if (entidade == null)
                throw new DadosInvalidosException("entidade", "A entidade não pode ser nula.");

            if (EmailJaExiste(entidade.Email))
                throw new DadosInvalidosException("Email", "Já existe um cliente com este email.");

            return base.Adicionar(entidade);
        }
    }
}
