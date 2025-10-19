namespace Ovr.BlazorApp.Services.Intefaces
{
    public interface IAuthGuardService
    {
        Task<bool> RedirigirSiNoAutenticado(string? rutaRedireccion = "/");
        Task<bool> TieneRol(string rol);
    }
}
