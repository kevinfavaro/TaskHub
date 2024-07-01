using TaskHub.Provider;

namespace TaskHub.Middleware
{
    public class UsuarioMiddleware
    {
        private readonly RequestDelegate _next;

        public UsuarioMiddleware(IHttpContextAccessor httpContextAccessor,
            RequestDelegate next)
        {
            _next = next;
        }

        public virtual async Task InvokeAsync(HttpContext context, UsuarioProvider usuarioIdProvider)
        {
            var userId = context.Request.Headers["UsuarioId"];

            if (!string.IsNullOrEmpty(userId) && int.TryParse(userId, out int id))
            {
                usuarioIdProvider.UsuarioId = id;
            }

            await _next(context);
        }
    }
}
