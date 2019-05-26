using TanCruzDentalInventorySystem.ViewModel;

namespace TanCruzDentalInventorySystem.BusinessService.BusinessServiceInterface
{
    public interface IAccountService
    {
        bool Login(LoginCredentialsViewModel loginInfo);
    }
}
