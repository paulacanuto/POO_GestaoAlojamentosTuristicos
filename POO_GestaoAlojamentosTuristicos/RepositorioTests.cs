using System;
using System.Linq;
using System.IO;
using Xunit;
using POO_GestaoAlojamentosTuristicos.Data;
using POO_GestaoAlojamentosTuristicos.Models;
using POO_GestaoAlojamentosTuristicos.Exceptions;

namespace POO_GestaoAlojamentosTuristicos.Tests
{
    /// <summary>
    /// Testes unitários para os repositórios
    /// Testa persistência, CRUD e funcionalidades específicas
    /// </summary>
    public class RepositorioTests : IDisposable
    {
        private readonly string pastaTestesTemp;

        public RepositorioTests()
        {
            // Cria pasta temporária para testes
            pastaTestesTemp = Path.Combine(Path.GetTempPath(), $"TestesRepo_{Guid.NewGuid()}");
            Directory.CreateDirectory(pastaTestesTemp);
        }

        public void Dispose()
        {
            // Limpa após testes
            if (Directory.Exists(pastaTestesTemp))
                Directory.Delete(pastaTestesTemp, true);
        }

        #region Testes RepositorioHotel

        [Fact]
        public void RepositorioHotel_Adicionar_DeveIncrementarContagem()
        {
            // Arrange
            var repo = new RepositorioHotel();
            var hotel = new Hotel(1, "Hotel Teste", 100, 4);

            // Act
            var resultado = repo.Adicionar(hotel);

            // Assert
            Assert.True(resultado);
            Assert.Equal(1, repo.Contar());
        }

        [Fact]
        public void RepositorioHotel_ObterPorId_DeveRetornarHotelCorreto()
        {
            // Arrange
            var repo = new RepositorioHotel();
            var hotel = new Hotel(1, "Hotel Central", 120, 5);
            repo.Adicionar(hotel);

            // Act
            var resultado = repo.ObterPorId(1);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal("Hotel Central", resultado.Endereco);
            Assert.Equal(5, resultado.NumeroEstrelas);
        }

        [Fact]
        public void RepositorioHotel_ObterPorIdInexistente_DeveLancarExcecao()
        {
            // Arrange
            var repo = new RepositorioHotel();

            // Act & Assert
            Assert.Throws<EntidadeNaoEncontradaException>(() => repo.ObterPorId(999));
        }

        [Fact]
        public void RepositorioHotel_Remover_DeveRemoverHotel()
        {
            // Arrange
            var repo = new RepositorioHotel();
            var hotel = new Hotel(1, "Hotel Remover", 80, 3);
            repo.Adicionar(hotel);

            // Act
            var resultado = repo.Remover(1);

            // Assert
            Assert.True(resultado);
            Assert.Equal(0, repo.Contar());
        }

        [Fact]
        public void RepositorioHotel_BuscarPorEndereco_DeveEncontrar()
        {
            // Arrange
            var repo = new RepositorioHotel();
            repo.Adicionar(new Hotel(1, "Rua das Flores, 100", 90, 4));
            repo.Adicionar(new Hotel(2, "Av. Principal, 200", 110, 5));

            // Act
            var resultados = repo.BuscarPorEndereco("Flores");

            // Assert
            Assert.Single(resultados);
            Assert.Contains("Flores", resultados[0].Endereco);
        }

        [Fact]
        public void RepositorioHotel_ObterMaisCaros_DeveRetornarOrdenado()
        {
            // Arrange
            var repo = new RepositorioHotel();
            repo.Adicionar(new Hotel(1, "Hotel A", 50, 2));
            repo.Adicionar(new Hotel(2, "Hotel B", 150, 5));
            repo.Adicionar(new Hotel(3, "Hotel C", 100, 4));

            // Act
            var resultado = repo.ObterMaisCaros(2);

            // Assert
            Assert.Equal(2, resultado.Count);
            Assert.Equal(150, resultado[0].PrecoPorNoite);
            Assert.Equal(100, resultado[1].PrecoPorNoite);
        }

        [Fact]
        public void RepositorioHotel_GerarProximoId_DeveRetornarSequencial()
        {
            // Arrange
            var repo = new RepositorioHotel();

            // Act - Primeiro ID
            var id1 = repo.GerarProximoId();
            repo.Adicionar(new Hotel(id1, "Hotel 1", 100, 4));

            // Act - Segundo ID
            var id2 = repo.GerarProximoId();

            // Assert
            Assert.Equal(1, id1);
            Assert.Equal(2, id2);
        }

        #endregion

        #region Testes RepositorioApartamento

        [Fact]
        public void RepositorioApartamento_Adicionar_DeveFuncionar()
        {
            // Arrange
            var repo = new RepositorioApartamento();
            var apt = new Apartamento(1, "Av. Central", 60, 2, true);

            // Act
            var resultado = repo.Adicionar(apt);

            // Assert
            Assert.True(resultado);
            Assert.Equal(1, repo.Contar());
        }

        [Fact]
        public void RepositorioApartamento_ObterPorId_DeveFuncionar()
        {
            // Arrange
            var repo = new RepositorioApartamento();
            var apt = new Apartamento(1, "Rua Teste", 70, 3, false);
            repo.Adicionar(apt);

            // Act
            var resultado = repo.ObterPorId(1);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(3, resultado.NumeroQuartos);
            Assert.False(resultado.TemGaragem);
        }

        [Fact]
        public void RepositorioApartamento_BuscarPorEndereco_DeveFuncionar()
        {
            // Arrange
            var repo = new RepositorioApartamento();
            repo.Adicionar(new Apartamento(1, "Rua das Palmeiras", 80, 2, true));
            repo.Adicionar(new Apartamento(2, "Av. Central", 90, 3, false));

            // Act
            var resultados = repo.BuscarPorEndereco("Palmeiras");

            // Assert
            Assert.Single(resultados);
            Assert.Contains("Palmeiras", resultados[0].Endereco);
        }

        #endregion

        #region Testes RepositorioCliente

        [Fact]
        public void RepositorioCliente_EmailJaExiste_DeveDetectarDuplicado()
        {
            // Arrange
            var repo = new RepositorioCliente();
            var cliente = new Cliente(1, "João Silva", "joao@test.com", "912345678");
            repo.Adicionar(cliente);

            // Act
            var existe = repo.EmailJaExiste("joao@test.com");

            // Assert
            Assert.True(existe);
        }

        [Fact]
        public void RepositorioCliente_EmailJaExiste_DeveIgnorarProprio()
        {
            // Arrange
            var repo = new RepositorioCliente();
            var cliente = new Cliente(1, "Maria Santos", "maria@test.com", "");
            repo.Adicionar(cliente);

            // Act - Verifica se email existe, mas ignora o próprio ID
            var existe = repo.EmailJaExiste("maria@test.com", idIgnorar: 1);

            // Assert - Não deve considerar como duplicado
            Assert.False(existe);
        }

        [Fact]
        public void RepositorioCliente_ObterPorEmail_DeveEncontrar()
        {
            // Arrange
            var repo = new RepositorioCliente();
            var cliente = new Cliente(1, "Pedro Alves", "pedro@test.com", "");
            repo.Adicionar(cliente);

            // Act
            var resultado = repo.ObterPorEmail("pedro@test.com");

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal("Pedro Alves", resultado.Nome);
        }

        [Fact]
        public void RepositorioCliente_BuscarPorNome_DeveEncontrarMultiplos()
        {
            // Arrange
            var repo = new RepositorioCliente();
            repo.Adicionar(new Cliente(1, "Ana Santos", "ana@test.com", ""));
            repo.Adicionar(new Cliente(2, "João Santos", "joao@test.com", ""));
            repo.Adicionar(new Cliente(3, "Maria Silva", "maria@test.com", ""));

            // Act
            var resultados = repo.BuscarPorNome("Santos");

            // Assert
            Assert.Equal(2, resultados.Count);
            Assert.All(resultados, c => Assert.Contains("Santos", c.Nome));
        }

        [Fact]
        public void RepositorioCliente_AdicionarEmailDuplicado_DeveLancarExcecao()
        {
            // Arrange
            var repo = new RepositorioCliente();
            repo.Adicionar(new Cliente(1, "Cliente 1", "duplicado@test.com", ""));

            // Act & Assert
            var exception = Assert.Throws<DadosInvalidosException>(() =>
                repo.Adicionar(new Cliente(2, "Cliente 2", "duplicado@test.com", ""))
            );

            Assert.Contains("email", exception.Message.ToLower());
        }

        #endregion

        #region Testes Genéricos (RepositorioBase)

        [Fact]
        public void RepositorioBase_Contar_DeveRetornarQuantidadeCorreta()
        {
            // Arrange
            var repo = new RepositorioHotel();
            repo.Adicionar(new Hotel(1, "Hotel 1", 100, 4));
            repo.Adicionar(new Hotel(2, "Hotel 2", 120, 5));

            // Act
            var count = repo.Contar();

            // Assert
            Assert.Equal(2, count);
        }

        [Fact]
        public void RepositorioBase_ObterTodos_DeveRetornarCopia()
        {
            // Arrange
            var repo = new RepositorioHotel();
            repo.Adicionar(new Hotel(1, "Hotel 1", 100, 4));

            // Act
            var lista1 = repo.ObterTodos();
            var lista2 = repo.ObterTodos();

            // Assert - Deve retornar cópias diferentes
            Assert.NotSame(lista1, lista2);
            Assert.Equal(lista1.Count, lista2.Count);
        }

        [Fact]
        public void RepositorioBase_AdicionarNull_DeveLancarExcecao()
        {
            // Arrange
            var repo = new RepositorioHotel();

            // Act & Assert
            Assert.Throws<DadosInvalidosException>(() => repo.Adicionar(null));
        }

        #endregion

        #region Testes LINQ nos Repositórios

        [Fact]
        public void Repositorio_UsaFirstOrDefault_Corretamente()
        {
            // Arrange
            var repo = new RepositorioHotel();
            repo.Adicionar(new Hotel(1, "Hotel A", 100, 4));
            repo.Adicionar(new Hotel(2, "Hotel B", 120, 5));

            // Act
            var resultado = repo.ObterPorId(2);

            // Assert - FirstOrDefault é usado internamente
            Assert.NotNull(resultado);
            Assert.Equal("Hotel B", resultado.Endereco);
        }

        [Fact]
        public void Repositorio_UsaWhere_ParaFiltrar()
        {
            // Arrange
            var repo = new RepositorioHotel();
            repo.Adicionar(new Hotel(1, "Rua das Flores, 100", 80, 3));
            repo.Adicionar(new Hotel(2, "Av. Principal, 200", 90, 4));
            repo.Adicionar(new Hotel(3, "Rua das Flores, 300", 100, 5));

            // Act - Where é usado internamente no BuscarPorEndereco
            var resultados = repo.BuscarPorEndereco("Flores");

            // Assert
            Assert.Equal(2, resultados.Count);
        }

        [Fact]
        public void Repositorio_UsaOrderByDescending_E_Take()
        {
            // Arrange
            var repo = new RepositorioHotel();
            repo.Adicionar(new Hotel(1, "Hotel A", 50, 2));
            repo.Adicionar(new Hotel(2, "Hotel B", 150, 5));
            repo.Adicionar(new Hotel(3, "Hotel C", 100, 4));
            repo.Adicionar(new Hotel(4, "Hotel D", 200, 5));

            // Act - OrderByDescending + Take
            var top3 = repo.ObterMaisCaros(3);

            // Assert
            Assert.Equal(3, top3.Count);
            Assert.Equal(200, top3[0].PrecoPorNoite);
            Assert.Equal(150, top3[1].PrecoPorNoite);
            Assert.Equal(100, top3[2].PrecoPorNoite);
        }

        [Fact]
        public void Repositorio_UsaAny_ParaVerificar()
        {
            // Arrange
            var repo = new RepositorioCliente();
            repo.Adicionar(new Cliente(1, "Cliente 1", "email1@test.com", ""));

            // Act - Any é usado no EmailJaExiste
            var existe = repo.EmailJaExiste("email1@test.com");
            var naoExiste = repo.EmailJaExiste("email2@test.com");

            // Assert
            Assert.True(existe);
            Assert.False(naoExiste);
        }

        [Fact]
        public void Repositorio_UsaMax_ParaGerarId()
        {
            // Arrange
            var repo = new RepositorioHotel();
            repo.Adicionar(new Hotel(5, "Hotel A", 100, 4));
            repo.Adicionar(new Hotel(3, "Hotel B", 120, 5));
            repo.Adicionar(new Hotel(8, "Hotel C", 140, 3));

            // Act - Max é usado no GerarProximoId
            var proximoId = repo.GerarProximoId();

            // Assert - Deve ser o maior ID + 1
            Assert.Equal(9, proximoId);
        }

        [Fact]
        public void Repositorio_UsaOrderBy_NaBusca()
        {
            // Arrange
            var repo = new RepositorioCliente();
            repo.Adicionar(new Cliente(1, "Zeca Santos", "zeca@test.com", ""));
            repo.Adicionar(new Cliente(2, "Ana Santos", "ana@test.com", ""));
            repo.Adicionar(new Cliente(3, "Maria Santos", "maria@test.com", ""));

            // Act - OrderBy é usado no BuscarPorNome
            var resultados = repo.BuscarPorNome("Santos");

            // Assert - Deve vir ordenado por nome
            Assert.Equal("Ana Santos", resultados[0].Nome);
            Assert.Equal("Maria Santos", resultados[1].Nome);
            Assert.Equal("Zeca Santos", resultados[2].Nome);
        }

        #endregion
    }
}
