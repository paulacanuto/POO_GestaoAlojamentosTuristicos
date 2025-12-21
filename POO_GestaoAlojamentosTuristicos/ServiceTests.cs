using System;
using System.Linq;
using Xunit;
using POO_GestaoAlojamentosTuristicos.Business;
using POO_GestaoAlojamentosTuristicos.Exceptions;

namespace POO_GestaoAlojamentosTuristicos.Tests
{
    /// <summary>
    /// Testes unitários para os serviços
    /// </summary>
    public class ServiceTests
    {
        #region Testes AlojamentoService

        [Fact]
        public void AlojamentoService_AdicionarHotel_DeveAumentarContador()
        {
            // Arrange
            var service = new AlojamentoService();
            var contagemInicial = service.ContarTotal();

            // Act - Método atualizado com parâmetros corretos
            service.AdicionarHotel("Hotel Teste", "Rua Teste", 50, 3);
            var contagemFinal = service.ContarTotal();

            // Assert
            Assert.Equal(contagemInicial + 1, contagemFinal);
        }

        [Fact]
        public void AlojamentoService_AdicionarApartamento_DeveAumentarContador()
        {
            // Arrange
            var service = new AlojamentoService();
            var contagemInicial = service.ContarTotal();

            // Act - Método atualizado com parâmetros corretos
            service.AdicionarApartamento("Apartamento Teste", "Av. Teste", 40, 2, true);
            var contagemFinal = service.ContarTotal();

            // Assert
            Assert.Equal(contagemInicial + 1, contagemFinal);
        }

        [Fact]
        public void AlojamentoService_ListarHoteis_DeveRetornarApenasHoteis()
        {
            // Arrange
            var service = new AlojamentoService();
            service.AdicionarHotel("Hotel 1", "Rua A", 100, 4);
            service.AdicionarApartamento("Apt 1", "Rua B", 60, 2, false);
            service.AdicionarHotel("Hotel 2", "Rua C", 120, 5);

            // Act
            var hoteis = service.ListarHoteis();

            // Assert
            Assert.True(hoteis.Count >= 2);
            Assert.All(hoteis, h => Assert.IsType<POO_GestaoAlojamentosTuristicos.Models.Hotel>(h));
        }

        [Fact]
        public void AlojamentoService_ListarApartamentos_DeveRetornarApenasApartamentos()
        {
            // Arrange
            var service = new AlojamentoService();
            service.AdicionarHotel("Hotel 1", "Rua A", 100, 4);
            service.AdicionarApartamento("Apt 1", "Rua B", 60, 2, false);
            service.AdicionarApartamento("Apt 2", "Rua C", 70, 3, true);

            // Act
            var apartamentos = service.ListarApartamentos();

            // Assert
            Assert.True(apartamentos.Count >= 2);
            Assert.All(apartamentos, a => Assert.IsType<POO_GestaoAlojamentosTuristicos.Models.Apartamento>(a));
        }

        [Fact]
        public void AlojamentoService_BuscarPorEndereco_DeveEncontrarResultados()
        {
            // Arrange
            var service = new AlojamentoService();
            service.AdicionarHotel("Hotel Flores", "Rua das Flores, 100", 80, 3);
            service.AdicionarHotel("Hotel Centro", "Av. Principal, 200", 90, 4);

            // Act
            var resultados = service.BuscarPorEndereco("Flores");

            // Assert
            Assert.True(resultados.Any());
            Assert.All(resultados, r => Assert.Contains("Flores", r.Endereco));
        }

        [Fact]
        public void AlojamentoService_BuscarPorPreco_DeveRetornarDentroFaixa()
        {
            // Arrange
            var service = new AlojamentoService();
            service.AdicionarHotel("Hotel Barato", "Rua 1", 30, 2);
            service.AdicionarHotel("Hotel Médio", "Rua 2", 70, 3);
            service.AdicionarHotel("Hotel Caro", "Rua 3", 150, 5);

            // Act
            var resultados = service.BuscarPorPreco(50, 100);

            // Assert
            Assert.All(resultados, r =>
            {
                Assert.True(r.PrecoPorNoite >= 50);
                Assert.True(r.PrecoPorNoite <= 100);
            });
        }

        [Fact]
        public void AlojamentoService_ObterEstatisticas_DeveRetornarDadosCorretos()
        {
            // Arrange
            var service = new AlojamentoService();
            service.AdicionarHotel("Hotel 1", "Rua A", 100, 4);
            service.AdicionarApartamento("Apt 1", "Rua B", 50, 2, false);

            // Act
            var stats = service.ObterEstatisticas();

            // Assert
            Assert.True((int)stats["Total"] >= 2);
            Assert.Contains("PrecoMedio", stats.Keys);
            Assert.Contains("Hoteis", stats.Keys);
            Assert.Contains("Apartamentos", stats.Keys);
        }

        [Fact]
        public void AlojamentoService_ObterMaisCaros_DeveRetornarOrdenado()
        {
            // Arrange
            var service = new AlojamentoService();
            service.AdicionarHotel("Hotel 1", "Rua 1", 50, 2);
            service.AdicionarHotel("Hotel 2", "Rua 2", 150, 5);
            service.AdicionarHotel("Hotel 3", "Rua 3", 100, 4);

            // Act
            var maisCaros = service.ObterMaisCaros(2);

            // Assert
            Assert.Equal(2, maisCaros.Count);
            Assert.True(maisCaros[0].PrecoPorNoite >= maisCaros[1].PrecoPorNoite);
        }

        #endregion

        #region Testes ClienteService

        [Fact]
        public void ClienteService_Adicionar_DeveAumentarContador()
        {
            // Arrange
            var service = new ClienteService();
            var contagemInicial = service.ContarTotal();

            // Act
            service.Adicionar("João Silva", "joao@test.com", "912345678");
            var contagemFinal = service.ContarTotal();

            // Assert
            Assert.Equal(contagemInicial + 1, contagemFinal);
        }

        [Fact]
        public void ClienteService_AdicionarEmailDuplicado_DeveLancarExcecao()
        {
            // Arrange
            var service = new ClienteService();
            service.Adicionar("Cliente 1", "duplicado@test.com", "");

            // Act & Assert
            var exception = Assert.Throws<DadosInvalidosException>(() =>
                service.Adicionar("Cliente 2", "duplicado@test.com", "")
            );

            Assert.Contains("email", exception.Message.ToLower());
        }

        [Fact]
        public void ClienteService_BuscarPorNome_DeveEncontrar()
        {
            // Arrange
            var service = new ClienteService();
            service.Adicionar("Maria Santos", "maria@test.com", "");
            service.Adicionar("João Santos", "joao@test.com", "");

            // Act
            var resultados = service.BuscarPorNome("Santos");

            // Assert
            Assert.True(resultados.Count >= 2);
            Assert.All(resultados, r => Assert.Contains("Santos", r.Nome));
        }

        [Fact]
        public void ClienteService_ObterPorEmail_DeveRetornarCliente()
        {
            // Arrange
            var service = new ClienteService();
            service.Adicionar("Pedro Alves", "pedro@test.com", "");

            // Act
            var cliente = service.ObterPorEmail("pedro@test.com");

            // Assert
            Assert.NotNull(cliente);
            Assert.Equal("Pedro Alves", cliente.Nome);
        }

        [Fact]
        public void ClienteService_EmailDisponivel_DeveRetornarTrue()
        {
            // Arrange
            var service = new ClienteService();

            // Act
            var disponivel = service.EmailDisponivel("novo@test.com");

            // Assert
            Assert.True(disponivel);
        }

        [Fact]
        public void ClienteService_EmailJaUsado_DeveRetornarFalse()
        {
            // Arrange
            var service = new ClienteService();
            service.Adicionar("Cliente", "usado@test.com", "");

            // Act
            var disponivel = service.EmailDisponivel("usado@test.com");

            // Assert
            Assert.False(disponivel);
        }

        #endregion

        #region Testes LINQ

        [Fact]
        public void LINQ_FiltrarEOrdenar_DeveFuncionar()
        {
            // Arrange
            var service = new AlojamentoService();
            service.AdicionarHotel("Hotel Z", "Rua Z", 50, 2);
            service.AdicionarHotel("Hotel A", "Rua A", 150, 5);
            service.AdicionarHotel("Hotel M", "Rua M", 100, 4);

            // Act
            var ordenados = service.ObterOrdenadosPorPreco(crescente: true);

            // Assert
            Assert.True(ordenados.Count >= 3);
            for (int i = 0; i < ordenados.Count - 1; i++)
            {
                Assert.True(ordenados[i].PrecoPorNoite <= ordenados[i + 1].PrecoPorNoite);
            }
        }

        #endregion
    }
}
