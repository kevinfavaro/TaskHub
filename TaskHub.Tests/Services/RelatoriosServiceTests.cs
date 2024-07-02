using Microsoft.EntityFrameworkCore;
using Moq;
using TaskHub.Data;
using TaskHub.Models;
using TaskHub.Services;

namespace TaskHub.Tests
{
    public class RelatoriosServiceTests
    {
        [Fact]
        public void MediaTarefasConcluidas_DeveRetornarOk()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            using (var context = new ApplicationDbContext(options))
            {
                context.Database.EnsureDeleted();

                var inicio = new DateTime(2024, 7, 1);
                var fim = new DateTime(2024, 7, 31);
                var usuarioId = new int[] { 1, 2 };

                context.Tarefas.Add(new Tarefa { TarefaId = 1, Titulo = "Teste 1", UsuarioId = 1, Ativa = true, Status = Enums.StatusTarefa.Concluída, DataConcluido = new DateTime(2024, 7, 5) });
                context.Tarefas.Add(new Tarefa { TarefaId = 2, Titulo = "Teste 2", UsuarioId = 1, Ativa = true, Status = Enums.StatusTarefa.Concluída, DataConcluido = new DateTime(2024, 7, 10) });
                context.Tarefas.Add(new Tarefa { TarefaId = 3, Titulo = "Teste 3", UsuarioId = 2, Ativa = true, Status = Enums.StatusTarefa.Concluída, DataConcluido = new DateTime(2024, 7, 15) });
                context.SaveChanges();

                var relatorioService = new RelatorioService(context);

                // Act
                var relatorio = relatorioService.MediaTarefasConcluidas(inicio, fim, usuarioId).ToList();

                // Assert
                Assert.NotNull(relatorio);
                Assert.Equal(2, relatorio.Count());
            }
        }

        [Fact]
        public void MediaTarefasConcluidas_Dias_DeveRetornarOk()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            using (var context = new ApplicationDbContext(options))
            {
                context.Database.EnsureDeleted();

                var dias = 7;
                var usuarioId = new int[] { 1, 2 };

                context.Tarefas.Add(new Tarefa { TarefaId = 1, Titulo = "Teste 1", UsuarioId = 1, Ativa = true, Status = Enums.StatusTarefa.Concluída, DataConcluido = DateTime.Now.AddDays(-3) });
                context.Tarefas.Add(new Tarefa { TarefaId = 2, Titulo = "Teste 2", UsuarioId = 1, Ativa = true, Status = Enums.StatusTarefa.Concluída, DataConcluido = DateTime.Now.AddDays(-2) });
                context.Tarefas.Add(new Tarefa { TarefaId = 3, Titulo = "Teste 3", UsuarioId = 2, Ativa = true, Status = Enums.StatusTarefa.Concluída, DataConcluido = DateTime.Now.AddDays(-6) });
                context.Tarefas.Add(new Tarefa { TarefaId = 4, Titulo = "Teste 4", UsuarioId = 3, Ativa = true, Status = Enums.StatusTarefa.Concluída, DataConcluido = DateTime.Now.AddDays(-10) });
                context.SaveChanges();

                var relatorioService = new RelatorioService(context);

                // Act
                var relatorio = relatorioService.MediaTarefasConcluidas(dias, usuarioId).ToList();

                // Assert
                Assert.NotNull(relatorio);
                Assert.Equal(2, relatorio.Count());
            }
        }

        [Fact]
        public void MediaTarefasConcluidasPorUsuario_DeveRetornarOk()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            using (var context = new ApplicationDbContext(options))
            {
                context.Database.EnsureDeleted();

                var usuarioId = 1;
                var inicio = new DateTime(2024, 7, 1);
                var fim = new DateTime(2024, 7, 31);

                context.Tarefas.Add(new Tarefa { TarefaId = 1, Titulo = "Teste 1", UsuarioId = 1, Ativa = true, Status = Enums.StatusTarefa.Concluída, DataConcluido = new DateTime(2024, 7, 5) });
                context.Tarefas.Add(new Tarefa { TarefaId = 2, Titulo = "Teste 2", UsuarioId = 1, Ativa = true, Status = Enums.StatusTarefa.Concluída, DataConcluido = new DateTime(2024, 7, 10) });
                context.Tarefas.Add(new Tarefa { TarefaId = 3, Titulo = "Teste 3", UsuarioId = 2, Ativa = true, Status = Enums.StatusTarefa.Concluída, DataConcluido = new DateTime(2024, 7, 15) });
                context.SaveChanges();

                var relatorioService = new RelatorioService(context);

                // Act
                var relatorio = relatorioService.MediaTarefasConcluidasPorUsuario(usuarioId, inicio, fim);

                // Assert
                Assert.NotNull(relatorio);
                Assert.Equal(2, relatorio.NumeroTarefas);
            }
        }

        [Fact]
        public void MediaTarefasConcluidasPorUsuario_Dias_DeveRetornarOk()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            using (var context = new ApplicationDbContext(options))
            {
                context.Database.EnsureDeleted();

                var usuarioId = 1;
                var dias = 7;

                context.Tarefas.Add(new Tarefa { TarefaId = 1, Titulo = "Teste 1", UsuarioId = 1, Ativa = true, Status = Enums.StatusTarefa.Concluída, DataConcluido = DateTime.Now.AddDays(-3) });
                context.Tarefas.Add(new Tarefa { TarefaId = 2, Titulo = "Teste 2", UsuarioId = 1, Ativa = true, Status = Enums.StatusTarefa.Concluída, DataConcluido = DateTime.Now.AddDays(-2) });
                context.Tarefas.Add(new Tarefa { TarefaId = 3, Titulo = "Teste 3", UsuarioId = 2, Ativa = true, Status = Enums.StatusTarefa.Concluída, DataConcluido = DateTime.Now.AddDays(-6) });
                context.Tarefas.Add(new Tarefa { TarefaId = 4, Titulo = "Teste 4", UsuarioId = 3, Ativa = true, Status = Enums.StatusTarefa.Concluída, DataConcluido = DateTime.Now.AddDays(-10) });
                context.SaveChanges();

                var relatorioService = new RelatorioService(context);

                // Act
                var relatorio = relatorioService.MediaTarefasConcluidasPorUsuario(usuarioId, dias);

                // Assert
                Assert.NotNull(relatorio);
                Assert.Equal(2, relatorio.NumeroTarefas);
            }
        }

    }
}
