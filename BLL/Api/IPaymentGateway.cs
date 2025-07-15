namespace BLL.Services
{
    public interface IPaymentGateway
    {
        Task<string> ChargeAsync(decimal amount, string currency, string token);
    }
}
