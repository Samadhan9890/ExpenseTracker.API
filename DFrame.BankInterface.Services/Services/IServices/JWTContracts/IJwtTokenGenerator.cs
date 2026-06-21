using ExpenseTracker.Services.Models.EntityModels;

namespace ExpenseTracker.Services.Contracts.IContracts.JWTContracts
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(TblUser user);
    }
}
