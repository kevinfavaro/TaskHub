using TaskHub.DTO;
using TaskHub.Models;

namespace TaskHub.Services.Interfaces
{
    public interface IProjetoService
    {
        public Projeto Obter(int projetoId);
        public IEnumerable<Projeto> ObterTodos();
        public Projeto Criar(ProjetoCreateDTO projetoCreate);
        public void Remover(int projetoId);
        public IEnumerable<Tarefa> ObterTarefas(int projetoId);
    }
}
