namespace Library.Management.Demo.IRepositories
{
    public interface IAuthorRepo
    {
        Task<bool> CheckExistence(int id);
    }
}
