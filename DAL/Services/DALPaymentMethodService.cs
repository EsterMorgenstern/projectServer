using DAL.Api;
using DAL.Models;

namespace DAL.Services
{
    public class DALPaymentMethodService : IDALPaymentMethod
    {
        dbcontext dbcontext;

        public DALPaymentMethodService(dbcontext data)
        {
            dbcontext = data;
        }

        public void Create(PaymentMethod paymentMethod)
        {
            dbcontext.PaymentMethods.Add(paymentMethod);
            dbcontext.SaveChanges();
        }

        public void Delete(int paymentMethodId)
        {
            var paymentMethod = dbcontext.PaymentMethods.SingleOrDefault(x => x.PaymentMethodId == paymentMethodId);
            if (paymentMethod == null)
            {
                throw new KeyNotFoundException($"PaymentMethod with ID {paymentMethodId} not found.");
            }
            dbcontext.PaymentMethods.Remove(paymentMethod);
            dbcontext.SaveChanges();
        }

        public List<PaymentMethod> Get()
        {
            try
            {
                if (dbcontext.PaymentMethods == null || !dbcontext.PaymentMethods.Any())
                {
                    throw new Exception("No PaymentMethod records found.");
                }
                return dbcontext.PaymentMethods.ToList();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while retrieving PaymentMethod records.", ex);
            }
        }

        public List<PaymentMethod> GetByStudentId(int studentId)
        {
            var paymentMethods = dbcontext.PaymentMethods.Where(x => x.StudentId == studentId && x.IsActive).ToList();
            if (paymentMethods == null)
            {
                throw new KeyNotFoundException($"PaymentMethods for student ID {studentId} not found.");
            }
            return paymentMethods;
        }

        public PaymentMethod GetById(int paymentMethodId)
        {
            var paymentMethod = dbcontext.PaymentMethods.SingleOrDefault(x => x.PaymentMethodId == paymentMethodId);
            if (paymentMethod == null)
            {
                throw new KeyNotFoundException($"PaymentMethod with ID {paymentMethodId} not found.");
            }
            return paymentMethod;
        }

        public void Update(PaymentMethod paymentMethod)
        {
            dbcontext.PaymentMethods.Update(paymentMethod);
            dbcontext.SaveChanges();
        }

        public void SetAsDefault(int paymentMethodId, int studentId)
        {
            // הסר ברירת מחדל מכל האמצעים הקיימים
            var existingMethods = dbcontext.PaymentMethods.Where(x => x.StudentId == studentId).ToList();
            foreach (var method in existingMethods)
            {
                method.IsDefault = false;
            }

            // הגדר את האמצעי החדש כברירת מחדל
            var newDefaultMethod = dbcontext.PaymentMethods.SingleOrDefault(x => x.PaymentMethodId == paymentMethodId);
            if (newDefaultMethod != null)
            {
                newDefaultMethod.IsDefault = true;
            }

            dbcontext.SaveChanges();
        }
    }
}