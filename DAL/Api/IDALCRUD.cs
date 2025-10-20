namespace DAL.Api
{
    public interface IDALCRUD<T>
    {
        Task<List<T>> GetAll();
        Task Create(T item);
        Task Delete(int item);
        Task Update(T item);
    }
}
