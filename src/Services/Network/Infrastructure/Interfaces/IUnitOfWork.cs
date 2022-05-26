namespace Network.API.Infrastructure.Interfaces
{
    public interface IUnitOfWork
    {
        IDeviceRepository DeviceRepository { get; }
        Task<bool> Complete();
        bool HasChanges();
    }
}
