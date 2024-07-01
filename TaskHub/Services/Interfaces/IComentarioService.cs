using TaskHub.DTO;
using TaskHub.Models;

namespace TaskHub.Services.Interfaces
{
    public interface IComentarioService
    {
        public IEnumerable<Comentario> ObterTodos();
        public Comentario Criar(int projetoId, int tarefaId, ComentarioCreateDTO comentarioCreate);
    }

}