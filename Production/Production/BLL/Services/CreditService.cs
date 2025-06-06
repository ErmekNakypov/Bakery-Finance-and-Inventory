using Production.DAL.Repositories;
using Production.Models;

namespace Production.BLL.Services;

public class CreditService : ICreditService
{
    private readonly ICreditRepository _creditRepository;

    public CreditService(ICreditRepository creditRepository)
    {
        _creditRepository = creditRepository;
    }

    public async Task<List<Credit>> GetCredits()
    {
        return await _creditRepository.GetCredits();
    }

    public async Task CreateCredit(Credit credit)
    {
        await _creditRepository.CreateCredit(credit);
    }
}