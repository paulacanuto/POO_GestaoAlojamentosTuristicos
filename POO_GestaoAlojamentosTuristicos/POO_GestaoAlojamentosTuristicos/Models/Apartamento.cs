using System;
using POO_GestaoAlojamentosTuristicos.Exceptions;

namespace POO_GestaoAlojamentosTuristicos.Models
{
    /// <summary>
    /// Classe Apartamento - outro tipo de alojamento
    /// Demonstra polimorfismo
    /// </summary>
    public class Apartamento : Alojamento
    {
        private int numeroQuartos;
        private bool temGaragem;

        public int NumeroQuartos
        {
            get => numeroQuartos;
            private set
            {
                if (value < 1)
                    throw new DadosInvalidosException("NumeroQuartos",
                        "Deve ter pelo menos 1 quarto.");
                numeroQuartos = value;
            }
        }

        public bool TemGaragem { get => temGaragem; private set => temGaragem = value; }

        /// <summary>
        /// Construtor
        /// </summary>
        public Apartamento(int id, string endereco, double precoPorNoite,
                          int numeroQuartos, bool temGaragem)
            : base(id, endereco, precoPorNoite)
        {
            NumeroQuartos = numeroQuartos;
            TemGaragem = temGaragem;
        }

        /// <summary>
        /// Override - apartamentos têm taxa diferente
        /// </summary>
        public override double CalcularTaxaServico()
        {
            double taxaBase = base.CalcularTaxaServico();
            // Taxa adicional se tem garagem
            return TemGaragem ? taxaBase + 5.0 : taxaBase;
        }

        /// <summary>
        /// Implementação do método abstrato
        /// </summary>
        public override string ObterDetalhes()
        {
            return $"Apartamento T{NumeroQuartos} - {Endereco}\n" +
                   $"Preço: €{PrecoPorNoite:F2}/noite\n" +
                   $"Garagem: {(TemGaragem ? "Sim" : "Não")}\n" +
                   $"Taxa de Serviço: €{CalcularTaxaServico():F2}";
        }

        public override string ToString()
        {
            return $"Apartamento T{NumeroQuartos} - {base.ToString()}";
        }
    }
}
