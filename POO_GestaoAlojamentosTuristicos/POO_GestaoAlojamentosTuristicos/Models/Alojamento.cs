using System;
using POO_GestaoAlojamentosTuristicos.Interfaces;
using POO_GestaoAlojamentosTuristicos.Exceptions;

namespace POO_GestaoAlojamentosTuristicos.Models
{
    /// <summary>
    /// Classe base abstrata para todos os tipos de alojamento
    /// Implementa IAlojamento e define comportamento comum
    /// </summary>
    public abstract class Alojamento : IAlojamento
    {
        // Campos privados (encapsulamento)
        private int id;
        private string endereco;
        private double precoPorNoite;

        // Propriedades públicas (expõem dados de forma controlada)
        public int Id
        {
            get => id;
            protected set
            {
                if (value <= 0)
                    throw new DadosInvalidosException("Id", "O ID deve ser maior que zero.");
                id = value;
            }
        }

        public string Endereco
        {
            get => endereco;
            protected set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new DadosInvalidosException("Endereco", "O endereço não pode estar vazio.");
                endereco = value;
            }
        }

        public double PrecoPorNoite
        {
            get => precoPorNoite;
            protected set
            {
                if (value < 0)
                    throw new DadosInvalidosException("PrecoPorNoite", "O preço não pode ser negativo.");
                precoPorNoite = value;
            }
        }

        /// <summary>
        /// Construtor protegido (só acessível por classes derivadas)
        /// </summary>
        protected Alojamento(int id, string endereco, double precoPorNoite)
        {
            // Usa as propriedades para acionar validações
            Id = id;
            Endereco = endereco;
            PrecoPorNoite = precoPorNoite;
        }

        /// <summary>
        /// Calcula taxa de serviço (10% do preço)
        /// Pode ser sobrescrito por classes derivadas (virtual)
        /// </summary>
        public virtual double CalcularTaxaServico()
        {
            return PrecoPorNoite * 0.10;
        }

        /// <summary>
        /// Método abstrato - cada tipo de alojamento deve implementar
        /// </summary>
        public abstract string ObterDetalhes();

        /// <summary>
        /// Override do ToString para representação textual
        /// </summary>
        public override string ToString()
        {
            return $"[{Id}] {Endereco} - €{PrecoPorNoite:F2}/noite";
        }
    }
}
