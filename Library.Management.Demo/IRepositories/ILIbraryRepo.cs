namespace Library.Management.Demo.IRepositories
{
    public interface ILIbraryRepo
    {
        Task<bool> CheckExistence(int id);
    }
}
