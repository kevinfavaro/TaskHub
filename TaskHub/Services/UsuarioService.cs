using TaskHub.Data;
using TaskHub.Models;
using TaskHub.Services.Interfaces;

namespace TaskHub.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly ApplicationDbContext _context;

        public UsuarioService(ApplicationDbContext context)
        {
            _context = context;
        }

        public Usuario Obter(int id)
        {
            return _context.Usuarios.Where(u => u.UsuarioId == id && u.Ativo).First();
        }

        public bool ehGerente(int id)
        {
            return Obter(id).Funcao == "Gerente";
        }
    }
}
