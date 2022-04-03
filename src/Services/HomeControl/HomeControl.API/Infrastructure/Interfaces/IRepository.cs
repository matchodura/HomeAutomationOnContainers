namespace HomeControl.API.Infrastructure.Interfaces
{
    public interface IRepository
    {
        void Add<T>(T item) where T : class;
    }
}
