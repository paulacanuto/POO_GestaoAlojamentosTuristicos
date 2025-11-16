using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POO_GestaoAlojamentosTuristicos
{
    // Classe que herda de Alojamento
    public class Hotel : Alojamento
    {
        private int NumeroEstrelas { get; set; }

        public Hotel(int id, string endereco, double precoPorNoite, int numeroEstrelas)
            : base(id, endereco, precoPorNoite)
        {
            NumeroEstrelas = numeroEstrelas;
        }

        // Método público
        public string ClassificarHotel()
        {
            if (NumeroEstrelas >= 5)
                return "Luxo";
            if (NumeroEstrelas >= 3)
                return "Conforto";
            return "Standard";
        }


        // "Getter" método público para estrelas
        public int GetNumeroEstrelas() => NumeroEstrelas;
    }
}
