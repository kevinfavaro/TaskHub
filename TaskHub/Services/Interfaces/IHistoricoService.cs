using TaskHub.Models;

namespace TaskHub.Services.Interfaces
{
    public interface IHistoricoService
    {
        public IEnumerable<Historico> ObterTodos();
        public Historico Criar(Historico historico);
        public Historico Criar(int tarefaId, string detalhes, int usuarioId);
    }
}
