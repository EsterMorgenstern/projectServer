using DAL.Api;
using DAL.Models;

namespace DAL.Services
{
    public class DALPaymentService : IDALPayment
    {
        dbcontext dbcontext;

        public DALPaymentService(dbcontext data)
        {
            dbcontext = data;
        }

        public void Create(Payment payment)
        {
            dbcontext.Payments.Add(payment);
            dbcontext.SaveChanges();
        }

        public void Delete(int paymentId)
        {
            var payment = dbcontext.Payments.SingleOrDefault(x => x.PaymentId == paymentId);
            if (payment == null)
            {
                throw new KeyNotFoundException($"Payment with ID {paymentId} not found.");
            }
            dbcontext.Payments.Remove(payment);
            dbcontext.SaveChanges();
        }

        public List<Payment> Get()
        {
            try
            {
                if (dbcontext.Payments == null || !dbcontext.Payments.Any())
                {
                    throw new Exception("No Payment records found.");
                }
                return dbcontext.Payments.ToList();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while retrieving Payment records.", ex);
            }
        }

        public List<Payment> GetByStudentId(int studentId)
        {
            var payments = dbcontext.Payments.Where(x => x.StudentId == studentId).OrderByDescending(x => x.PaymentDate).ToList();
            if (payments == null)
            {
                throw new KeyNotFoundException($"Payments for student ID {studentId} not found.");
            }
            return payments;
        }

        public Payment GetById(int paymentId)
        {
            var payment = dbcontext.Payments.SingleOrDefault(x => x.PaymentId == paymentId);
            if (payment == null)
            {
                throw new KeyNotFoundException($"Payment with ID {paymentId} not found.");
            }
            return payment;
        }

        public void Update(Payment payment)
        {
            dbcontext.Payments.Update(payment);
            dbcontext.SaveChanges();
        }

        public List<Payment> GetByPaymentMethodId(int paymentMethodId)
        {
            var payments = dbcontext.Payments.Where(x => x.PaymentMethodId == paymentMethodId).ToList();
            return payments;
        }
    }
}