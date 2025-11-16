using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POO_GestaoAlojamentosTuristicos
{
    // Classe Cliente
    public class Cliente
    {
        private int Id { get; set; }
        private string Nome { get; set; }
        private string Email { get; set; }

        public Cliente(int id, string nome, string email)
        {
            Id = id;
            Nome = nome;
            Email = email;
        }

        // Exemplo de método público
        public bool ValidarEmail()
        {
            return Email.Contains("@");
        }
    }

}
