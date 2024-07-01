using TaskHub.Data;
using TaskHub.Models;
using TaskHub.Services.Interfaces;

namespace TaskHub.Services
{
    public class HistoricoService: IHistoricoService
    {
        private readonly ApplicationDbContext _context;

        public HistoricoService(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Historico> ObterTodos()
        {
            return _context.Historico.ToList();
        }

        public Historico Criar(Historico historico)
        {
            _context.Historico.Add(historico);
            _context.SaveChanges();
            return historico;
        }

        public Historico Criar(int tarefaId, string detalhes, int usuarioId)
        {
            return Criar(new Historico
            {
                TarefaId = tarefaId,
                Detalhes = detalhes,
                UsuarioId = usuarioId,
                DataAtualizacao = DateTime.Now
            });
        }
    }
}
