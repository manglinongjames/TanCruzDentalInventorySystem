namespace TanCruzDentalInventorySystem.Repository.DataServiceInterface
{
    public interface IAccountRepository
    {
        IUnitOfWork UnitOfWork { get; set; }
        bool Login(string userName, string passWord);
    }
}
