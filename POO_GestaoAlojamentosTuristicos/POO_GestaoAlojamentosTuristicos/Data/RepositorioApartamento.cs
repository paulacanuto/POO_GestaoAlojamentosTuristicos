using System;
using System.Collections.Generic;
using System.Linq;
using POO_GestaoAlojamentosTuristicos.Models;
using POO_GestaoAlojamentosTuristicos.Exceptions;

namespace POO_GestaoAlojamentosTuristicos.Data
{
    public class RepositorioApartamento : RepositorioBase<Apartamento>
    {
        public RepositorioApartamento() : base("apartamentos.json")
        {
        }

        public override Apartamento ObterPorId(int id)
        {
            var apt = entidades.FirstOrDefault(a => a.Id == id);
            if (apt == null)
                throw new EntidadeNaoEncontradaException("Apartamento", id);
            return apt;
        }

        public override bool Atualizar(Apartamento entidade)
        {
            if (entidade == null)
                throw new DadosInvalidosException("entidade", "A entidade não pode ser nula.");

            var indice = entidades.FindIndex(a => a.Id == entidade.Id);
            if (indice == -1)
                throw new EntidadeNaoEncontradaException("Apartamento", entidade.Id);

            entidades[indice] = entidade;
            return true;
        }

        public override bool Remover(int id)
        {
            var apt = entidades.FirstOrDefault(a => a.Id == id);
            if (apt == null)
                throw new EntidadeNaoEncontradaException("Apartamento", id);

            return entidades.Remove(apt);
        }

        public int GerarProximoId()
        {
            return entidades.Any() ? entidades.Max(a => a.Id) + 1 : 1;
        }

        public List<Apartamento> BuscarPorEndereco(string endereco)
        {
            if (string.IsNullOrWhiteSpace(endereco))
                return new List<Apartamento>();

            return entidades
                .Where(a => a.Endereco.Contains(endereco, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }
    }
}
