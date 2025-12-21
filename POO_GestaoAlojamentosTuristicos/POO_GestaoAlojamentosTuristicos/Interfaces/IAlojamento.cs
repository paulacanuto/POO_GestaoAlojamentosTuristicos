using System;

namespace POO_GestaoAlojamentosTuristicos.Interfaces
{
    /// <summary>
    /// Interface que define o contrato para todos os tipos de alojamento
    /// Promove polimorfismo e desacoplamento
    /// </summary>
    public interface IAlojamento
    {
        // Propriedades de leitura
        int Id { get; }
        string Endereco { get; }
        double PrecoPorNoite { get; }

        // Métodos obrigatórios
        double CalcularTaxaServico();
        string ObterDetalhes();
    }
}