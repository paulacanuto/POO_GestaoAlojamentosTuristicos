using System;
using System.Text.RegularExpressions;
using POO_GestaoAlojamentosTuristicos.Exceptions;

namespace POO_GestaoAlojamentosTuristicos.Models
{
    /// <summary>
    /// Classe Cliente - representa um cliente do sistema
    /// </summary>
    public class Cliente
    {
        private int id;
        private string nome;
        private string email;
        private string telefone;

        public int Id
        {
            get => id;
            set
            {
                if (value <= 0)
                    throw new DadosInvalidosException("Id", "O ID deve ser maior que zero.");
                id = value;
            }
        }

        public string Nome
        {
            get => nome;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new DadosInvalidosException("Nome", "O nome não pode estar vazio.");
                if (value.Length < 3)
                    throw new DadosInvalidosException("Nome", "O nome deve ter pelo menos 3 caracteres.");
                nome = value;
            }
        }

        public string Email
        {
            get => email;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new DadosInvalidosException("Email", "O email não pode estar vazio.");
                if (!ValidarFormatoEmail(value))
                    throw new DadosInvalidosException("Email", "Formato de email inválido.");
                email = value;
            }
        }

        public string Telefone
        {
            get => telefone;
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    // Remove tudo que não for número
                    string apenasNumeros = new string(value.Where(char.IsDigit).ToArray());

                    if (apenasNumeros.Length < 9)
                        throw new DadosInvalidosException("Telefone", "Telefone deve ter pelo menos 9 dígitos.");

                    telefone = apenasNumeros;
                }
                else
                {
                    telefone = "";
                }
            }
        }

        /// <summary>
        /// Construtor
        /// </summary>
        public Cliente(int id, string nome, string email, string telefone = "")
        {
            Id = id;
            Nome = nome;
            Email = email;
            Telefone = telefone;
        }

        /// <summary>
        /// Valida formato do email usando regex simples
        /// </summary>
        private bool ValidarFormatoEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            // Regex simples para validação de email
            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, pattern);
        }

        /// <summary>
        /// Retorna informações do cliente
        /// </summary>
        public string ObterInformacoes()
        {
            return $"Cliente #{Id}: {Nome}\n" +
                   $"Email: {Email}\n" +
                   $"Telefone: {(string.IsNullOrWhiteSpace(Telefone) ? "Não informado" : Telefone)}";
        }

        public override string ToString()
        {
            return $"[{Id}] {Nome} - {Email}";
        }
    }
}
