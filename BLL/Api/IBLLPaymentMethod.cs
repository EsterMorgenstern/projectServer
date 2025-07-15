namespace BLL.Api
{
    public interface IBLLPaymentMethod
    {
        void Create(BLLPaymentMethod paymentMethod);
        void Delete(int paymentMethodId);
        List<BLLPaymentMethod> Get();
        List<BLLPaymentMethod> GetByStudentId(int studentId);
        BLLPaymentMethod GetById(int paymentMethodId);
        void Update(BLLPaymentMethod paymentMethod);
        void SetAsDefault(int paymentMethodId, int studentId);
    }
}