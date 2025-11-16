using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POO_GestaoAlojamentosTuristicos
{
    // Classe base Alojamento
    public class Alojamento
    {
        private int Id { get; set; }
        private string Endereco { get; set; }
        private double PrecoPorNoite { get; set; }

        // Contrutor protegido ( mas visível para herança)
        protected Alojamento(int id, string endereco, double precoPorNoite)
        {
            Id = id;
            Endereco = endereco;
            PrecoPorNoite = precoPorNoite;
        }

        // Método publico para calcular taxas
        public double CalcularTaxaServico()
        {
            return PrecoPorNoite * 0.10; // 10%
        }

        // "Getters", método público para leitura segura
        public int GetId() => Id;
        public string GetEndereco() => Endereco;
        public double GetPrecoPorNoite() => PrecoPorNoite;
    }

}
