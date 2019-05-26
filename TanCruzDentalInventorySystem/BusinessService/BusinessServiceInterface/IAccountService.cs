using TanCruzDentalInventorySystem.ViewModel;

namespace TanCruzDentalInventorySystem.BusinessService.BusinessServiceInterface
{
    public interface IAccountService
    {
        UserProfileViewModel Login(LoginCredentialsViewModel loginInfo);
    }
}
