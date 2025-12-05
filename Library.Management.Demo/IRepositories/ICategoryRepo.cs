namespace Library.Management.Demo.IRepositories
{
    public interface ICategoryRepo
    {
        Task<bool> CheckExistence(int id);
    }
}
