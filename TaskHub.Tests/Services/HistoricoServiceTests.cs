using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Moq;
using TaskHub.Data;
using TaskHub.Models;
using TaskHub.Services;
using TaskHub.Services.Interfaces;
using Xunit;

namespace TaskHub.Tests
{
    public class HistoricoServiceTests
    {
        [Fact]
        public void CriarHistorico_DeveCriarRegistro()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            using (var context = new ApplicationDbContext(options))
            {
                // Crie um contexto de banco de dados em memória para testes

                var historicoService = new HistoricoService(context);

                // Crie um objeto de histórico para teste
                var historico = new Historico
                {
                    TarefaId = 1,
                    Detalhes = "Atualização de status",
                    UsuarioId = 42,
                    DataAtualizacao = DateTime.Now
                };

                // Act
                var historicoCriado = historicoService.Criar(historico);

                // Assert
                Assert.NotNull(historicoCriado);
                Assert.Equal(historico.TarefaId, historicoCriado.TarefaId);
                Assert.Equal(historico.Detalhes, historicoCriado.Detalhes);
                Assert.Equal(historico.UsuarioId, historicoCriado.UsuarioId);
                Assert.True(historicoCriado.DataAtualizacao > DateTime.MinValue);
            }
        }
    }
}