using TaskHub.Data;
using TaskHub.Models;

namespace TaskHub.Services
{
    public class UsuarioService
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
