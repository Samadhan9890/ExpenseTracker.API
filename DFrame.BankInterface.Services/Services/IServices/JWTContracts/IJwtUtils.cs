using ExpenseTracker.Services.Models.EntityModels;

namespace ExpenseTracker.Services.Contracts.IContracts.JWTContracts
{
    public interface IJwtUtils
    {
        string GetJwtToken(TblUser user);
    }
}
