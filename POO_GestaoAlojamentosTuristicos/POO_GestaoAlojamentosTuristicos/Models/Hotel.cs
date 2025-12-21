
using System;
using POO_GestaoAlojamentosTuristicos.Exceptions;

namespace POO_GestaoAlojamentosTuristicos.Models
{
    /// <summary>
    /// Classe Hotel herda de Alojamento
    /// Adiciona funcionalidade específica para hotéis
    /// </summary>
    public class Hotel : Alojamento
    {
        private int numeroEstrelas;

        public int NumeroEstrelas
        {
            get => numeroEstrelas;
            private set
            {
                if (value < 1 || value > 5)
                    throw new DadosInvalidosException("NumeroEstrelas",
                        "O número de estrelas deve estar entre 1 e 5.");
                numeroEstrelas = value;
            }
        }

        /// <summary>
        /// Construtor - chama o construtor da classe base
        /// </summary>
        public Hotel(int id, string endereco, double precoPorNoite, int numeroEstrelas)
            : base(id, endereco, precoPorNoite)
        {
            NumeroEstrelas = numeroEstrelas;
        }

        /// <summary>
        /// Classifica o hotel baseado no número de estrelas
        /// </summary>
        public string ClassificarHotel()
        {
            return NumeroEstrelas switch
            {
                5 => "Luxo",
                4 => "Superior",
                3 => "Conforto",
                _ => "Standard"
            };
        }

        /// <summary>
        /// Implementação do método abstrato da classe base
        /// </summary>
        public override string ObterDetalhes()
        {
            return $"Hotel {NumeroEstrelas}★ - {Endereco}\n" +
                   $"Preço: €{PrecoPorNoite:F2}/noite\n" +
                   $"Classificação: {ClassificarHotel()}\n" +
                   $"Taxa de Serviço: €{CalcularTaxaServico():F2}";
        }

        /// <summary>
        /// Override do método ToString
        /// </summary>
        public override string ToString()
        {
            return $"Hotel {NumeroEstrelas}★ - {base.ToString()}";
        }
    }
}
