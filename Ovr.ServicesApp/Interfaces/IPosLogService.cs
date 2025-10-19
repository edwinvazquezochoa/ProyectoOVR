namespace Ovr.ServicesApp.Interfaces
{
    public interface IPosLogService
    {
        void Add(Exception? ex = null, string? sLog = null);
    }
}
