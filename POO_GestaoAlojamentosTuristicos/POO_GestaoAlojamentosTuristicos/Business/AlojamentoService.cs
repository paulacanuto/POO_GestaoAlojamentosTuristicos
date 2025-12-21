using System;
using System.Collections.Generic;
using System.Linq;
using POO_GestaoAlojamentosTuristicos.Models;
using POO_GestaoAlojamentosTuristicos.Data;
using POO_GestaoAlojamentosTuristicos.Exceptions;

namespace POO_GestaoAlojamentosTuristicos.Business
{
    /// <summary>
    /// Serviço de gestão de alojamentos
    /// Camada de negócio - contém lógica e regras de negócio
    /// </summary>
    public class AlojamentoService
    {
        private readonly RepositorioHotel repositorioHotel;
        private readonly RepositorioApartamento repositorioApartamento;

        public AlojamentoService()
        {
            repositorioHotel = new RepositorioHotel();
            repositorioApartamento = new RepositorioApartamento();

            try { repositorioHotel.Carregar(); } catch { }
            try { repositorioApartamento.Carregar(); } catch { }
        }

        #region Adicionar

        public bool AdicionarHotel(string endereco, string v, double preco, int estrelas)
        {
            int novoId = repositorioHotel.GerarProximoId();
            var hotel = new Hotel(novoId, endereco, preco, estrelas);

            repositorioHotel.Adicionar(hotel);
            repositorioHotel.Guardar();
            return true;
        }

        public bool AdicionarApartamento(string endereco, string v, double preco, int quartos, bool garagem)
        {
            int novoId = repositorioApartamento.GerarProximoId();
            var apartamento = new Apartamento(novoId, endereco, preco, quartos, garagem);

            repositorioApartamento.Adicionar(apartamento);
            repositorioApartamento.Guardar();
            return true;
        }

        #endregion

        #region Listar

        public List<Hotel> ListarHoteis() => repositorioHotel.ObterTodos();

        public List<Apartamento> ListarApartamentos() => repositorioApartamento.ObterTodos();

        public List<Alojamento> ListarTodos()
        {
            var todos = new List<Alojamento>();
            todos.AddRange(ListarHoteis());
            todos.AddRange(ListarApartamentos());
            return todos;
        }

        #endregion

        #region Buscar / Obter

        public Alojamento ObterPorId(int id)
        {
            var hotel = repositorioHotel.ObterTodos().FirstOrDefault(h => h.Id == id);
            if (hotel != null) return hotel;

            var apartamento = repositorioApartamento.ObterTodos().FirstOrDefault(a => a.Id == id);
            if (apartamento != null) return apartamento;

            throw new EntidadeNaoEncontradaException("Alojamento", id);
        }

        public List<Alojamento> BuscarPorEndereco(string endereco)
        {
            var resultados = new List<Alojamento>();
            resultados.AddRange(repositorioHotel.ObterTodos().Where(h => h.Endereco.Contains(endereco, StringComparison.OrdinalIgnoreCase)));
            resultados.AddRange(repositorioApartamento.ObterTodos().Where(a => a.Endereco.Contains(endereco, StringComparison.OrdinalIgnoreCase)));
            return resultados;
        }

        public List<Alojamento> BuscarPorPreco(double min, double max)
        {
            var resultados = new List<Alojamento>();
            resultados.AddRange(repositorioHotel.ObterTodos().Where(h => h.PrecoPorNoite >= min && h.PrecoPorNoite <= max));
            resultados.AddRange(repositorioApartamento.ObterTodos().Where(a => a.PrecoPorNoite >= min && a.PrecoPorNoite <= max));
            return resultados.OrderBy(a => a.PrecoPorNoite).ToList();
        }

        #endregion

        #region Remover

        public void Remover(int id)
        {
            var hotel = repositorioHotel.ObterTodos().FirstOrDefault(h => h.Id == id);
            if (hotel != null)
            {
                repositorioHotel.Remover(id);
                repositorioHotel.Guardar();
                return;
            }

            var apartamento = repositorioApartamento.ObterTodos().FirstOrDefault(a => a.Id == id);
            if (apartamento != null)
            {
                repositorioApartamento.Remover(id);
                repositorioApartamento.Guardar();
                return;
            }

            throw new EntidadeNaoEncontradaException("Alojamento", id);
        }

        #endregion

        #region Estatísticas / Ordenação

        public Dictionary<string, object> ObterEstatisticas()
        {
            var todos = ListarTodos();
            if (!todos.Any())
                return new Dictionary<string, object>
                {
                    { "Total", 0 },
                    { "Hoteis", 0 },
                    { "Apartamentos", 0 },
                    { "PrecoMedio", 0 },
                    { "PrecoMinimo", 0 },
                    { "PrecoMaximo", 0 }
                };

            return new Dictionary<string, object>
            {
                { "Total", todos.Count },
                { "Hoteis", repositorioHotel.Contar() },
                { "Apartamentos", repositorioApartamento.Contar() },
                { "PrecoMedio", todos.Average(a => a.PrecoPorNoite) },
                { "PrecoMinimo", todos.Min(a => a.PrecoPorNoite) },
                { "PrecoMaximo", todos.Max(a => a.PrecoPorNoite) }
            };
        }

        public List<Alojamento> ObterMaisCaros(int quantidade = 5)
        {
            return ListarTodos()
                .OrderByDescending(a => a.PrecoPorNoite)
                .Take(quantidade)
                .ToList();
        }

        public List<Alojamento> ObterOrdenadosPorPreco(bool crescente = true)
        {
            var todos = ListarTodos();
            return crescente
                ? todos.OrderBy(a => a.PrecoPorNoite).ToList()
                : todos.OrderByDescending(a => a.PrecoPorNoite).ToList();
        }

        public int ContarTotal() => ListarTodos().Count;

        public void AdicionarHotel(string v1, int v2, int v3)
        {
            throw new NotImplementedException();
        }

        public void AdicionarApartamento(string v1, int v2, int v3, bool v4)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}