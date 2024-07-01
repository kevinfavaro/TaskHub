using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Moq;
using TaskHub.Data;
using TaskHub.DTO;
using TaskHub.Mapper;
using TaskHub.Models;
using TaskHub.Provider;
using TaskHub.Services;
using TaskHub.Services.Interfaces;

namespace TaskHub.Tests
{
    public class TarefaServiceTests
    {
        private DbContextOptions<ApplicationDbContext> _options;
        private Mock<IHistoricoService> _historicoServiceMock;
        private Mock<IUsuarioProvider> _usuarioProviderMock;
        private IOptions<AppSettings> _appSettingsOptions;
        private IMapper _mapper;

        public TarefaServiceTests()
        {
            _options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            _historicoServiceMock = new Mock<IHistoricoService>();
            _usuarioProviderMock = new Mock<IUsuarioProvider>();

            AppSettings appSettings = new AppSettings {MaxTarefas = 10};
            _appSettingsOptions = Options.Create(appSettings);

            var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile(new AutoMapperProfile()));
            _mapper = mapperConfig.CreateMapper();
        }

        [Fact]
        public void Obter_DeveRetornarTarefaExistente()
        {
            // Arrange
            using (var context = new ApplicationDbContext(_options))
            {
                context.Database.EnsureDeleted();

                var tarefaService = new TarefaService(
                    context,
                    _historicoServiceMock.Object,
                    _usuarioProviderMock.Object,
                    _appSettingsOptions,
                    _mapper
                );

                var projetoId = 1;
                var tarefaId = 42;

                var tarefa = new Tarefa
                {
                    TarefaId = tarefaId,
                    ProjetoId = projetoId,
                    Titulo = "Minha Tarefa"
                };

                context.Tarefas.Add(tarefa);
                context.SaveChanges();

                // Act
                var tarefaObtida = tarefaService.Obter(projetoId, tarefaId);

                // Assert
                Assert.NotNull(tarefaObtida);
                Assert.Equal(tarefaId, tarefaObtida.TarefaId);
                Assert.Equal(projetoId, tarefaObtida.ProjetoId);
            }
        }

        [Fact]
        public void Existe_DeveRetornarTrueSeTarefaExistir()
        {
            // Arrange
            using (var context = new ApplicationDbContext(_options))
            {
                context.Database.EnsureDeleted();

                var tarefaService = new TarefaService(
                    context,
                    _historicoServiceMock.Object,
                    _usuarioProviderMock.Object,
                    _appSettingsOptions,
                    _mapper
                );

                var projetoId = 1;
                var tarefaId = 42;

                var tarefa = new Tarefa
                {
                    TarefaId = tarefaId,
                    ProjetoId = projetoId,
                    Titulo = "Minha Tarefa",
                    Ativa = true
                };

                context.Tarefas.Add(tarefa);
                context.SaveChanges();

                // Act
                var existeTarefa = tarefaService.Existe(projetoId, tarefaId);

                // Assert
                Assert.True(existeTarefa);
            }
        }

        [Fact]
        public void ObterPorProjetoId_DeveRetornarTarefasAtivasDoProjeto()
        {
            // Arrange
            using (var context = new ApplicationDbContext(_options))
            {
                context.Database.EnsureDeleted();

                var tarefaService = new TarefaService(
                    context,
                    _historicoServiceMock.Object,
                    _usuarioProviderMock.Object,
                    _appSettingsOptions,
                    _mapper
                );

                var projetoId = 1;

                var tarefa1 = new Tarefa { TarefaId = 1, ProjetoId = projetoId, Titulo = "Tarefa 1", Ativa = false };
                var tarefa2 = new Tarefa { TarefaId = 2, ProjetoId = projetoId, Titulo = "Tarefa 2", Ativa = true };

                context.Tarefas.AddRange(tarefa1, tarefa2);
                context.SaveChanges();

                // Act
                var tarefasObtidas = tarefaService.ObterPorProjetoId(projetoId);

                // Assert
                Assert.NotNull(tarefasObtidas);
                Assert.Single(tarefasObtidas);
                Assert.Equal(tarefa2.TarefaId, tarefasObtidas.First().TarefaId);
            }
        }

        [Theory]
        [InlineData(Enums.StatusTarefa.Pendente)]
        [InlineData(Enums.StatusTarefa.EmAndamento)]
        public void TemPendentePorProjetoId_DeveRetornarTrueSeTarefasPendentesExistirem(Enums.StatusTarefa status)
        {
            // Arrange
            using (var context = new ApplicationDbContext(_options))
            {
                context.Database.EnsureDeleted();

                var tarefaService = new TarefaService(
                    context,
                    _historicoServiceMock.Object,
                    _usuarioProviderMock.Object,
                    _appSettingsOptions,
                    _mapper
                );

                var projetoId = 1;

                var tarefaPendente = new Tarefa { TarefaId = 1, ProjetoId = projetoId, Titulo = "Tarefa Pendente", Ativa = true, Status = status };

                context.Tarefas.Add(tarefaPendente);
                context.SaveChanges();

                // Act
                var temPendente = tarefaService.TemPendentePorProjetoId(projetoId);

                // Assert
                Assert.True(temPendente);
            }
        }

        [Fact]
        public void Criar_DeveCriarNovaTarefa()
        {
            // Arrange
            using (var context = new ApplicationDbContext(_options))
            {
                context.Database.EnsureDeleted();

                var tarefaService = new TarefaService(
                    context,
                    _historicoServiceMock.Object,
                    _usuarioProviderMock.Object,
                    _appSettingsOptions,
                    _mapper
                );

                var projetoId = 1;
                var usuarioId = 42; // ID do usuário identificado

                _usuarioProviderMock.SetupGet(p => p.UsuarioId).Returns(usuarioId);

                var novaTarefa = new TarefaCreateDTO
                {
                    Titulo = "Minha Nova Tarefa"
                };

                // Act
                var tarefaCriada = tarefaService.Criar(projetoId, novaTarefa);

                // Assert
                Assert.NotNull(tarefaCriada);
                Assert.Equal(projetoId, tarefaCriada.ProjetoId);
                Assert.Equal(usuarioId, tarefaCriada.UsuarioId);
                Assert.Equal(Enums.StatusTarefa.Pendente, tarefaCriada.Status);
            }
        }

        [Fact]
        public void Nao_PodeCriarMaisDeXTarefasEmUmProjeto()
        {
            // Arrange
            using (var context = new ApplicationDbContext(_options))
            {
                context.Database.EnsureDeleted();

                var tarefaService = new TarefaService(
                    context,
                    _historicoServiceMock.Object,
                    _usuarioProviderMock.Object,
                    _appSettingsOptions,
                    _mapper
                );

                var projetoId = 1;
                var usuarioId = 42;

                _usuarioProviderMock.SetupGet(p => p.UsuarioId).Returns(usuarioId);

                for (int i = 0; i < _appSettingsOptions.Value.MaxTarefas; i++)
                {
                    var novaTarefa = new TarefaCreateDTO
                    {
                        Titulo = $"Tarefa {i + 1}"
                    };

                    tarefaService.Criar(projetoId, novaTarefa);
                }

                var tarefaAlémDoLimite = new TarefaCreateDTO
                {
                    Titulo = "Tarefa Além do Limite"
                };

                // Act
                var excecao = Assert.Throws<Exception>(() => tarefaService.Criar(projetoId, tarefaAlémDoLimite));

                // Assert
                Assert.Equal($"Não é possível adicionar mais de {_appSettingsOptions.Value.MaxTarefas} tarefas no projeto", excecao.Message);
            }
        }

        [Fact]
        public void DeveAtualizarTarefaExistente()
        {
            // Arrange
            using (var context = new ApplicationDbContext(_options))
            {
                context.Database.EnsureDeleted();

                var tarefaService = new TarefaService(
                    context,
                    _historicoServiceMock.Object,
                    _usuarioProviderMock.Object,
                    _appSettingsOptions,
                    _mapper
                );

                var projetoId = 1;
                var tarefaId = 42;
                var usuarioId = 33;

                _usuarioProviderMock.SetupGet(p => p.UsuarioId).Returns(usuarioId);

                var tarefaExistente = new Tarefa
                {
                    TarefaId = tarefaId,
                    ProjetoId = projetoId,
                    Titulo = "Tarefa Existente",
                    Descricao = "Descrição original",
                    DataVencimento = DateTime.Now.AddDays(7),
                    Status = Enums.StatusTarefa.Pendente
                };

                context.Tarefas.Add(tarefaExistente);
                context.SaveChanges();

                var tarefaAtualizada = new TarefaUpdateDTO
                {
                    Titulo = "Tarefa Atualizada",
                    Descricao = "Nova descrição",
                    DataVencimento = DateTime.Now.AddDays(14),
                    Status = Enums.StatusTarefa.Concluída
                };

                // Act
                tarefaService.Atualizar(projetoId, tarefaId, tarefaAtualizada);

                // Assert
                var tarefaAtualizadaNoBanco = context.Tarefas.FirstOrDefault(t => t.TarefaId == tarefaId && t.ProjetoId == projetoId);
                Assert.NotNull(tarefaAtualizadaNoBanco);
                Assert.Equal(tarefaAtualizada.Titulo, tarefaAtualizadaNoBanco.Titulo);
                Assert.Equal(tarefaAtualizada.Descricao, tarefaAtualizadaNoBanco.Descricao);
                Assert.Equal(tarefaAtualizada.DataVencimento, tarefaAtualizadaNoBanco.DataVencimento);
                Assert.Equal(tarefaAtualizada.Status, tarefaAtualizadaNoBanco.Status);
            }
        }

        [Fact]
        public void DeveRemoverTarefaExistente()
        {
            // Arrange
            using (var context = new ApplicationDbContext(_options))
            {
                context.Database.EnsureDeleted();

                var tarefaService = new TarefaService(
                    context,
                    _historicoServiceMock.Object,
                    _usuarioProviderMock.Object,
                    _appSettingsOptions,
                    _mapper
                );

                var projetoId = 1;
                var tarefaId = 42;
                var usuarioId = 84;

                _usuarioProviderMock.SetupGet(p => p.UsuarioId).Returns(usuarioId);

                var tarefaExistente = new Tarefa
                {
                    TarefaId = tarefaId,
                    ProjetoId = projetoId,
                    Titulo = "Tarefa Teste",
                    Ativa = true
                };

                context.Tarefas.Add(tarefaExistente);
                context.SaveChanges();

                // Act
                tarefaService.Remover(projetoId, tarefaId);

                // Assert
                var tarefaRemovida = context.Tarefas.FirstOrDefault(t => t.TarefaId == tarefaId && t.ProjetoId == projetoId);
                Assert.NotNull(tarefaRemovida);
                Assert.False(tarefaRemovida.Ativa);
            }
        }

    }
}
