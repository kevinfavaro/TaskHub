using AutoMapper;
using TaskHub.Data;
using TaskHub.DTO;
using TaskHub.Models;
using TaskHub.Provider;
using TaskHub.Services.Interfaces;

namespace TaskHub.Services
{
    public class ProjetoService: IProjetoService
    {
        private readonly ApplicationDbContext _context;
        private readonly ITarefaService _tarefaService;
        private readonly IUsuarioProvider _usuarioProvider;
        private readonly IMapper _mapper;

        public ProjetoService(ApplicationDbContext context,
            ITarefaService tarefaService,
            IUsuarioProvider usuarioProvider,
            IMapper mapper)
        {
            _context = context;
            _tarefaService = tarefaService;
            _usuarioProvider = usuarioProvider;
            _mapper = mapper;
        }

        public Projeto Obter(int projetoId)
        {
            return _context.Projetos.FirstOrDefault(p => p.ProjetoId == projetoId);
        }
        public IEnumerable<Projeto> ObterTodos()
        {
            return _context.Projetos.ToList();
        }

        public Projeto Criar(ProjetoCreateDTO projetoCreate)
        {
            if (_usuarioProvider.UsuarioId == null)
            {
                throw new Exception("Necessário identificar o usuário para a operação.");
            }

            var projeto = _mapper.Map<Projeto>(projetoCreate);
            projeto.UsuarioId = (int)_usuarioProvider.UsuarioId;

            _context.Projetos.Add(projeto);
            _context.SaveChanges();
            return projeto;
        }

        public void Remover(int projetoId)
        {
            if (_tarefaService.TemPendentePorProjetoId(projetoId))
            {
                throw new Exception("Existe(m) tarefa(s) associada(s) a este projeto não concluída(s). Conclua a(s) tarefa(s) ou a(s) exclua");
            }

            var projetoExistente = _context.Projetos.FirstOrDefault(t => t.ProjetoId == projetoId && t.Ativo);
            if (projetoExistente != null)
            {
                projetoExistente.Ativo = false;
                _context.SaveChanges();
            }
        }

        public IEnumerable<Tarefa> ObterTarefas(int projetoId)
        {
            return _tarefaService.ObterPorProjetoId(projetoId).ToList();
        }
    }
}
