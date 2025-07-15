namespace BLL.Api
{
    public interface IBLLPayment
    {
        void Create(BLLPayment payment);
        void Delete(int paymentId);
        List<BLLPayment> Get();
        List<BLLPayment> GetByStudentId(int studentId);
        BLLPayment GetById(int paymentId);
        void Update(BLLPayment payment);
        List<BLLPayment> GetByPaymentMethodId(int paymentMethodId);
    }
}