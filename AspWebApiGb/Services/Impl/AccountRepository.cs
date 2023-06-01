using EmployeeServiceData;

namespace AspWebApiGb.Services.Impl;

public class AccountRepository : IAccountRepository
{
    #region Services

    private readonly EmployeeServiceDbContext _context;

    #endregion

    #region Constructor

    public AccountRepository(EmployeeServiceDbContext context)
    {
        _context = context;
    }

    #endregion
    
    public int Create(Account data)
    {
        _context.Add(data);
        _context.SaveChanges();

        return data.AccountId;
    }

    public IList<Account> GetAll()
    {
        return _context.Accounts.ToList();
    }

    public Account GetById(int id)
    {
        return _context.Accounts.FirstOrDefault(acc => acc.AccountId == id);
    }

    public bool Update(Account data)
    {
        Account account = GetById(data.AccountId);

        bool accountExists = (account != null);
        
        if (!accountExists)
            return false;

        account.EMail = data.EMail;
        account.LastName = data.LastName;
        account.FirstName = data.FirstName;
        account.SecondName = data.SecondName;
        account.Blocked = data.Blocked;

        int amountOfChanges = _context.SaveChanges();
        bool changesWereDone = (amountOfChanges != 0);

        return changesWereDone;
    }

    public bool Delete(int id)
    {
        Account account = GetById(id);

        if (account == null)
        {
            return false;
        }

        _context.Remove(account);
        _context.SaveChanges();
        
        return true;
    }
}