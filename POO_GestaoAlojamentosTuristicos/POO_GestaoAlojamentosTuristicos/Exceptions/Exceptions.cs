using System;

namespace POO_GestaoAlojamentosTuristicos.Exceptions
{
    /// <summary>
    /// Exceção base para o sistema de alojamentos
    /// </summary>
    public class AlojamentoException : Exception
    {
        public AlojamentoException(string mensagem) : base(mensagem)
        {
        }

        public AlojamentoException(string mensagem, Exception innerException)
            : base(mensagem, innerException)
        {
        }
    }

    /// <summary>
    /// Exceção lançada quando dados inválidos são fornecidos
    /// </summary>
    public class DadosInvalidosException : AlojamentoException
    {
        public string Campo { get; }

        public DadosInvalidosException(string campo, string mensagem)
            : base($"Dados inválidos no campo '{campo}': {mensagem}")
        {
            Campo = campo;
        }

        public DadosInvalidosException(string mensagem) : base(mensagem)
        {
        }
    }

    /// <summary>
    /// Exceção lançada quando uma entidade não é encontrada
    /// </summary>
    public class EntidadeNaoEncontradaException : AlojamentoException
    {
        public int Id { get; }

        public EntidadeNaoEncontradaException(string tipo, int id)
            : base($"{tipo} com ID {id} não foi encontrado.")
        {
            Id = id;
        }
    }

    /// <summary>
    /// Exceção lançada quando há problemas de persistência
    /// </summary>
    public class PersistenciaException : AlojamentoException
    {
        public PersistenciaException(string mensagem) : base(mensagem)
        {
        }

        public PersistenciaException(string mensagem, Exception innerException)
            : base($"Erro de persistência: {mensagem}", innerException)
        {
        }
    }
}
