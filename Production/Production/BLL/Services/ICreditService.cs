using Production.Models;

namespace Production.BLL.Services;

public interface ICreditService
{
    public Task<List<Credit>> GetCredits();
    public Task CreateCredit(Credit credit);
}