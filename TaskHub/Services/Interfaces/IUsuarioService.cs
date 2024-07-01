using TaskHub.Data;
using TaskHub.Models;

namespace TaskHub.Services.Interfaces
{
    public interface IUsuarioService
    {
        public Usuario Obter(int id);
        public bool ehGerente(int id);
    }
}
