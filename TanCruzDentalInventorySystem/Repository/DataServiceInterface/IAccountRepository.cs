using TanCruzDentalInventorySystem.Models;

namespace TanCruzDentalInventorySystem.Repository.DataServiceInterface
{
    public interface IAccountRepository
    {
        IUnitOfWork UnitOfWork { get; set; }
        UserProfile Login(string userName, string passWord);
    }
}
