
using System;
using Xunit;
using POO_GestaoAlojamentosTuristicos.Models;
using POO_GestaoAlojamentosTuristicos.Exceptions;

namespace POO_GestaoAlojamentosTuristicos.Tests
{
    /// <summary>
    /// Testes unitários para as classes de modelo
    /// Framework: xUnit
    /// </summary>
    public class ModelTests
    {
        #region Testes Hotel

        [Fact]
        public void Hotel_CriacaoComDadosValidos_DeveFuncionar()
        {
            // Arrange & Act
            var hotel = new Hotel(1, "Rua Principal, 123", 75.50, 4);

            // Assert
            Assert.Equal(1, hotel.Id);
            Assert.Equal("Rua Principal, 123", hotel.Endereco);
            Assert.Equal(75.50, hotel.PrecoPorNoite);
            Assert.Equal(4, hotel.NumeroEstrelas);
        }

        [Fact]
        public void Hotel_PrecoNegativo_DeveLancarExcecao()
        {
            // Arrange & Act & Assert
            var exception = Assert.Throws<DadosInvalidosException>(() =>
                new Hotel(1, "Rua A", -10, 3)
            );

            Assert.Contains("preço", exception.Message.ToLower());
        }

        [Theory]
        [InlineData(0)]
        [InlineData(6)]
        [InlineData(10)]
        public void Hotel_EstrelasInvalidas_DeveLancarExcecao(int estrelas)
        {
            // Arrange & Act & Assert
            var exception = Assert.Throws<DadosInvalidosException>(() =>
                new Hotel(1, "Rua A", 50, estrelas)
            );

            Assert.Contains("estrelas", exception.Message.ToLower());
        }

        [Theory]
        [InlineData(5, "Luxo")]
        [InlineData(4, "Superior")]
        [InlineData(3, "Conforto")]
        [InlineData(2, "Standard")]
        [InlineData(1, "Standard")]
        public void Hotel_ClassificarHotel_DeveRetornarClassificacaoCorreta(int estrelas, string esperado)
        {
            // Arrange
            var hotel = new Hotel(1, "Rua A", 50, estrelas);

            // Act
            var resultado = hotel.ClassificarHotel();

            // Assert
            Assert.Equal(esperado, resultado);
        }

        [Fact]
        public void Hotel_CalcularTaxaServico_DeveRetornar10Porcento()
        {
            // Arrange
            var hotel = new Hotel(1, "Rua A", 100, 4);

            // Act
            var taxa = hotel.CalcularTaxaServico();

            // Assert
            Assert.Equal(10.0, taxa);
        }

        #endregion

        #region Testes Apartamento

        [Fact]
        public void Apartamento_CriacaoComDadosValidos_DeveFuncionar()
        {
            // Arrange & Act
            var apartamento = new Apartamento(1, "Av. Central, 456", 60, 2, true);

            // Assert
            Assert.Equal(1, apartamento.Id);
            Assert.Equal("Av. Central, 456", apartamento.Endereco);
            Assert.Equal(60, apartamento.PrecoPorNoite);
            Assert.Equal(2, apartamento.NumeroQuartos);
            Assert.True(apartamento.TemGaragem);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Apartamento_QuartosInvalidos_DeveLancarExcecao(int quartos)
        {
            // Arrange & Act & Assert
            var exception = Assert.Throws<DadosInvalidosException>(() =>
                new Apartamento(1, "Rua A", 50, quartos, false)
            );

            Assert.Contains("quartos", exception.Message.ToLower());
        }

        [Fact]
        public void Apartamento_ComGaragem_TaxaDeveSerMaior()
        {
            // Arrange
            var semGaragem = new Apartamento(1, "Rua A", 100, 2, false);
            var comGaragem = new Apartamento(2, "Rua B", 100, 2, true);

            // Act
            var taxaSem = semGaragem.CalcularTaxaServico();
            var taxaCom = comGaragem.CalcularTaxaServico();

            // Assert
            Assert.True(taxaCom > taxaSem);
            Assert.Equal(5.0, taxaCom - taxaSem); // Diferença de €5
        }

        #endregion

        #region Testes Cliente

        [Fact]
        public void Cliente_CriacaoComDadosValidos_DeveFuncionar()
        {
            // Arrange & Act
            var cliente = new Cliente(1, "João Silva", "joao@email.com", "912345678");

            // Assert
            Assert.Equal(1, cliente.Id);
            Assert.Equal("João Silva", cliente.Nome);
            Assert.Equal("joao@email.com", cliente.Email);
            Assert.Equal("912345678", cliente.Telefone);
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData(null)]
        public void Cliente_NomeVazio_DeveLancarExcecao(string nome)
        {
            // Arrange & Act & Assert
            var exception = Assert.Throws<DadosInvalidosException>(() =>
                new Cliente(1, nome, "email@test.com", "")
            );

            Assert.Contains("nome", exception.Message.ToLower());
        }

        [Theory]
        [InlineData("ab")] // Muito curto
        public void Cliente_NomeMuitoCurto_DeveLancarExcecao(string nome)
        {
            // Arrange & Act & Assert
            var exception = Assert.Throws<DadosInvalidosException>(() =>
                new Cliente(1, nome, "email@test.com", "")
            );

            Assert.Contains("nome", exception.Message.ToLower());
        }

        [Theory]
        [InlineData("email.invalido")]
        [InlineData("@invalido.com")]
        [InlineData("invalido@")]
        [InlineData("invalido")]
        public void Cliente_EmailInvalido_DeveLancarExcecao(string email)
        {
            // Arrange & Act & Assert
            var exception = Assert.Throws<DadosInvalidosException>(() =>
                new Cliente(1, "Nome Válido", email, "")
            );

            Assert.Contains("email", exception.Message.ToLower());
        }

        [Theory]
        [InlineData("teste@email.com")]
        [InlineData("usuario.teste@dominio.com.br")]
        [InlineData("nome+sobrenome@empresa.pt")]
        public void Cliente_EmailValido_NaoDeveLancarExcecao(string email)
        {
            // Arrange & Act
            var cliente = new Cliente(1, "Nome Teste", email, "");

            // Assert
            Assert.Equal(email, cliente.Email);
        }

        [Fact]
        public void Cliente_TelefoneOpcional_DeveFuncionar()
        {
            // Arrange & Act
            var cliente = new Cliente(1, "Nome", "email@test.com");

            // Assert
            Assert.NotNull(cliente);
        }

        #endregion

        #region Testes Polimorfismo

        [Fact]
        public void Polimorfismo_ListaAlojamentos_DeveAceitarDiferentesTipos()
        {
            // Arrange
            var alojamentos = new System.Collections.Generic.List<Alojamento>
            {
                new Hotel(1, "Hotel A", 100, 5),
                new Apartamento(2, "Apt B", 60, 2, true),
                new Hotel(3, "Hotel C", 80, 3)
            };

            // Act & Assert
            Assert.Equal(3, alojamentos.Count);
            Assert.IsType<Hotel>(alojamentos[0]);
            Assert.IsType<Apartamento>(alojamentos[1]);
            Assert.IsType<Hotel>(alojamentos[2]);
        }

        [Fact]
        public void Polimorfismo_MetodoAbstrato_DeveSerImplementadoPorTodos()
        {
            // Arrange
            Alojamento hotel = new Hotel(1, "Hotel", 100, 4);
            Alojamento apartamento = new Apartamento(2, "Apt", 60, 2, false);

            // Act
            var detalhesHotel = hotel.ObterDetalhes();
            var detalhesApt = apartamento.ObterDetalhes();

            // Assert
            Assert.NotNull(detalhesHotel);
            Assert.NotNull(detalhesApt);
            Assert.NotEmpty(detalhesHotel);
            Assert.NotEmpty(detalhesApt);
        }

        #endregion
    }
}
