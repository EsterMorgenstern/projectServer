using DAL.Models;

namespace DAL.Api
{
    public interface IDALPayment
    {
        void Create(Payment payment);
        void Delete(int paymentId);
        List<Payment> Get();
        List<Payment> GetByStudentId(int studentId);
        Payment GetById(int paymentId);
        void Update(Payment payment);
        List<Payment> GetByPaymentMethodId(int paymentMethodId);
    }
}