using AutoMapper;
using Microsoft.EntityFrameworkCore;
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
    public class ProjetoServiceTests
    {
        private DbContextOptions<ApplicationDbContext> _options;
        private Mock<ITarefaService> _tarefaServiceMock;
        private Mock<IUsuarioProvider> _usuarioProviderMock;
        private IMapper _mapper;

        public ProjetoServiceTests()
        {
            _options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "ProjetoServiceDb")
                .Options;

            _tarefaServiceMock = new Mock<ITarefaService>();
            _usuarioProviderMock = new Mock<IUsuarioProvider>();

            var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile(new AutoMapperProfile()));
            _mapper = mapperConfig.CreateMapper();
        }

        [Fact]
        public void Obter_DeveRetornarProjetoExistente()
        {
            // Arrange
            using (var context = new ApplicationDbContext(_options))
            {
                context.Database.EnsureDeleted();

                var projetoService = new ProjetoService(
                    context,
                    _tarefaServiceMock.Object,
                    _usuarioProviderMock.Object,
                    _mapper
                );

                var projeto = new Projeto
                {
                    ProjetoId = 1,
                    Nome = "Projeto de Teste",
                    Descricao = "Descrição de Teste",
                    Ativo = true
                };

                context.Projetos.Add(projeto);
                context.SaveChanges();

                // Act
                var projetoObtido = projetoService.Obter(1);

                // Assert
                Assert.NotNull(projetoObtido);
                Assert.Equal(projeto.ProjetoId, projetoObtido.ProjetoId);
                Assert.True(projetoObtido.Ativo);
            }
        }

        [Fact]
        public void ObterTarefas_DeveRetornarTarefasDoProjeto()
        {
            // Arrange
            using (var context = new ApplicationDbContext(_options))
            {
                context.Database.EnsureDeleted();
                
                var projetoService = new ProjetoService(
                    context,
                    _tarefaServiceMock.Object,
                    _usuarioProviderMock.Object,
                    _mapper
                );

                var projetoId = 1;
                var tarefasDoProjeto = new List<Tarefa>
                {
                    new Tarefa { TarefaId = 1, Titulo = "Tarefa 1", Ativa = true },
                    new Tarefa { TarefaId = 2, Titulo = "Tarefa 2", Ativa = true }
                };

                _tarefaServiceMock.Setup(s => s.ObterPorProjetoId(projetoId)).Returns(tarefasDoProjeto);

                // Act
                var tarefasObtidas = projetoService.ObterTarefas(projetoId);

                // Assert
                Assert.NotNull(tarefasObtidas);
                Assert.Equal(tarefasDoProjeto.Count(), tarefasObtidas.Count());
            }
        }


        [Fact]
        public void Criar_DeveCriarNovoProjeto()
        {
            // Arrange
            using (var context = new ApplicationDbContext(_options))
            {
                context.Database.EnsureDeleted();

                _usuarioProviderMock.SetupGet(p => p.UsuarioId).Returns(1);

                var projetoService = new ProjetoService(
                    context,
                    _tarefaServiceMock.Object,
                    _usuarioProviderMock.Object,
                    _mapper
                );

                var projetoCreate = new ProjetoCreateDTO
                {
                    Nome = "Novo Projeto",
                    Descricao = "Descrição Teste"
                };

                // Act
                var projetoCriado = projetoService.Criar(projetoCreate);

                // Assert
                Assert.NotNull(projetoCriado);
                Assert.Equal(projetoCreate.Nome, projetoCriado.Nome);
            }
        }

        [Fact]
        public void Remover_DeveDesativarProjetoExistente()
        {
            // Arrange
            using (var context = new ApplicationDbContext(_options))
            {
                context.Database.EnsureDeleted();

                var projetoService = new ProjetoService(
                    context,
                    _tarefaServiceMock.Object,
                    _usuarioProviderMock.Object,
                    _mapper
                );

                var projeto = new Projeto
                {
                    ProjetoId = 2,
                    Nome = "Projeto Ativo",
                    Descricao = "Descrição de Teste",
                    Ativo = true
                };

                context.Projetos.Add(projeto);
                context.SaveChanges();

                // Act
                projetoService.Remover(2);

                // Assert
                var projetoDesativado = context.Projetos.FirstOrDefault(p => p.ProjetoId == 2);
                Assert.NotNull(projetoDesativado);
                Assert.False(projetoDesativado.Ativo);
            }
        }

        [Fact]
        public void Remover_DeveLancarExcecaoSeTarefasPendentesExistirem()
        {
            // Arrange
            using (var context = new ApplicationDbContext(_options))
            {
                context.Database.EnsureDeleted();

                var projetoService = new ProjetoService(
                    context,
                    _tarefaServiceMock.Object,
                    _usuarioProviderMock.Object,
                    _mapper
                );

                var projetoId = 2;

                // Configurar o mock do ITarefaService para retornar tarefas pendentes
                _tarefaServiceMock.Setup(s => s.TemPendentePorProjetoId(projetoId)).Returns(true);

                // Act e Assert
                Assert.Throws<Exception>(() => projetoService.Remover(projetoId));
            }
        }

    }
}
