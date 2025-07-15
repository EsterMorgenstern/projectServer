using DAL.Models;

namespace DAL.Api
{
    public interface IDALPaymentMethod
    {

        void Create(PaymentMethod paymentMethod);
        void Delete(int paymentMethodId);
        List<PaymentMethod> Get();
        List<PaymentMethod> GetByStudentId(int studentId);
        PaymentMethod GetById(int paymentMethodId);
        void Update(PaymentMethod paymentMethod);
        void SetAsDefault(int paymentMethodId, int studentId);


    }
}
