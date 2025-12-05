namespace Library.Management.Demo.IRepositories
{
    public interface IPublisherRepo
    {
        Task<bool> CheckExistence(int id);
    }
}
