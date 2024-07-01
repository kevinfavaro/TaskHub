using AutoMapper;
using TaskHub.Data;
using TaskHub.DTO;
using TaskHub.Models;
using TaskHub.Provider;
using TaskHub.Services.Interfaces;

namespace TaskHub.Services
{
    public class ComentarioService : IComentarioService
    {
        private readonly ApplicationDbContext _context;
        private readonly ITarefaService _tarefaService;
        private readonly IHistoricoService _historicoService;
        private readonly IUsuarioProvider _usuarioProvider;
        private readonly IMapper _mapper;

        public ComentarioService(ApplicationDbContext context,
            ITarefaService tarefaService,
            IHistoricoService historicoService,
            IUsuarioProvider usuarioProvider,
            IMapper mapper
            )
        {
            _context = context;
            _tarefaService = tarefaService;
            _historicoService = historicoService;
            _usuarioProvider = usuarioProvider;
            _mapper = mapper;
        }

        public IEnumerable<Comentario> ObterTodos()
        {
            return _context.Comentarios.ToList();
        }

        public Comentario Criar(int projetoId, int tarefaId, ComentarioCreateDTO comentarioCreate)
        {
            if (_usuarioProvider.UsuarioId == null)
            {
                throw new Exception("Necessário identificar o usuário para a operação.");
            }

            if (_tarefaService.Existe(projetoId, tarefaId))
            {
                var comentario = _mapper.Map<Comentario>(comentarioCreate);
                comentario.TarefaId = tarefaId;
                comentario.UsuarioId = _usuarioProvider.UsuarioId;
                comentario.DataComentario = DateTime.Now;

                _context.Comentarios.Add(comentario);
                _context.SaveChanges();

                _historicoService.Criar(tarefaId, "Novo comentário: " + comentario.Texto, (int)comentario.UsuarioId);

                return comentario;
            }
            else
            {
                throw new Exception("A tarefa especificada não existe ou não pode mais receber comentários.");
            }
        }
    }
}