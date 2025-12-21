using System;
using System.Collections.Generic;
using System.Linq;
using POO_GestaoAlojamentosTuristicos.Models;
using POO_GestaoAlojamentosTuristicos.Exceptions;

namespace POO_GestaoAlojamentosTuristicos.Data
{
    public class RepositorioHotel : RepositorioBase<Hotel>
    {
        public RepositorioHotel() : base("hoteis.json")
        {
        }

        public override Hotel ObterPorId(int id)
        {
            var hotel = entidades.FirstOrDefault(h => h.Id == id);
            if (hotel == null)
                throw new EntidadeNaoEncontradaException("Hotel", id);
            return hotel;
        }

        public override bool Atualizar(Hotel entidade)
        {
            if (entidade == null)
                throw new DadosInvalidosException("entidade", "A entidade não pode ser nula.");

            var indice = entidades.FindIndex(h => h.Id == entidade.Id);
            if (indice == -1)
                throw new EntidadeNaoEncontradaException("Hotel", entidade.Id);

            entidades[indice] = entidade;
            return true;
        }

        public override bool Remover(int id)
        {
            var hotel = entidades.FirstOrDefault(h => h.Id == id);
            if (hotel == null)
                throw new EntidadeNaoEncontradaException("Hotel", id);

            return entidades.Remove(hotel);
        }

        public int GerarProximoId()
        {
            return entidades.Any() ? entidades.Max(h => h.Id) + 1 : 1;
        }

        public List<Hotel> BuscarPorEndereco(string endereco)
        {
            if (string.IsNullOrWhiteSpace(endereco))
                return new List<Hotel>();

            return entidades
                .Where(h => h.Endereco.Contains(endereco, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        public List<Hotel> ObterMaisCaros(int quantidade = 5)
        {
            return entidades.OrderByDescending(h => h.PrecoPorNoite).Take(quantidade).ToList();
        }
    }
}
