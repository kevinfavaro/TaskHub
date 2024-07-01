using TaskHub.DTO;
using TaskHub.Models;

namespace TaskHub.Services.Interfaces
{
    public interface ITarefaService
    {
        public Tarefa Obter(int projetoId, int tarefaId);
        public bool Existe(int projetoId, int tarefaId);
        public IEnumerable<Tarefa> ObterPorProjetoId(int projetoId);
        public bool TemPendentePorProjetoId(int projetoId);
        public Tarefa Criar(int projetoId, TarefaCreateDTO novaTarefa);
        public void Atualizar(int projetoId, int tarefaId, TarefaUpdateDTO tarefaAtualizada);
        public void Remover(int projetoId, int tarefaId);
    }
}
