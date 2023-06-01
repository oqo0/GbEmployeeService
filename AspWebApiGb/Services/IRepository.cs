namespace AspWebApiGb.Services;

public interface IRepository<T, TId>
{
    TId Create(T data);
    
    IList<T> GetAll();
    
    T GetById(TId id);
        
    bool Update(T data);
    
    bool Delete(TId id);
}