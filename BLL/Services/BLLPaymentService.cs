using BLL.Api;
using DAL.Api;
using DAL.Models;

namespace BLL.Services
{
    public class BLLPaymentService : IBLLPayment
    {
        private readonly IDAL dal;
        private readonly IPaymentGateway paymentGateway;

        public BLLPaymentService(IDAL dal, IPaymentGateway paymentGateway) // ✅ תיקון הקונסטרקטור
        {
            this.dal = dal;
            this.paymentGateway = paymentGateway;
        }
        public BLLPaymentService(IDAL dal)
        {
            this.dal = dal;
        }

        // ✅ הוספת מתודה חדשה לחיוב
        public async Task<string> ProcessCreditCardPaymentAsync(BLLPayment payment, string stripeToken)
        {
            try
            {
                // ביצוע החיוב דרך Stripe
                string transactionId = await paymentGateway.ChargeAsync(
                    payment.Amount,
                    "ILS",
                    stripeToken
                );

                // יצירת רשומת תשלום מוצלח
                payment.TransactionId = transactionId;
                payment.Status = "COMPLETED";
                payment.PaymentDate = DateTime.Now;
                payment.CreatedAt = DateTime.Now;

                Create(payment);

                return transactionId;
            }
            catch (Exception ex)
            {
                // יצירת רשומת תשלום נכשל
                payment.Status = "FAILED";
                payment.Notes = $"שגיאה בתשלום: {ex.Message}";
                payment.PaymentDate = DateTime.Now;
                payment.CreatedAt = DateTime.Now;

                Create(payment);

                throw; // זריקת השגיאה הלאה
            }
        }
        public void Create(BLLPayment payment)
        {
            Payment p = new Payment()
            {
                StudentId = payment.StudentId,
                Amount = payment.Amount,
                PaymentDate = payment.PaymentDate,
                PaymentMethod = payment.PaymentMethod ?? string.Empty,
                Notes = payment.Notes ?? string.Empty,
                PaymentMethodId = payment.PaymentMethodId,
                Status = payment.Status ?? "PENDING",
                TransactionId = payment.TransactionId ?? string.Empty,
                GroupId = payment.GroupId,
                CreatedAt = payment.CreatedAt
            };
            dal.Payments.Create(p);
        }
        public async Task CreateAndChargeAsync(BLLPayment payment, string token)
        {
            string transactionId = await paymentGateway.ChargeAsync(payment.Amount, "ILS", token);

            Payment paymentRecord = new Payment
            {
                StudentId = payment.StudentId,
                Amount = payment.Amount,
                PaymentDate = DateTime.Now,
                TransactionId = transactionId,
                Status = "COMPLETED",
                CreatedAt = DateTime.Now,
                PaymentMethodId = payment.PaymentMethodId
            };

            dal.Payments.Create(paymentRecord);
        }

        public void Delete(int paymentId)
        {
            dal.Payments.Delete(paymentId);
        }

        public List<BLLPayment> Get()
        {
            return dal.Payments.Get().Select(p => new BLLPayment()
            {
                PaymentId = p.PaymentId,
                StudentId = p.StudentId,
                Amount = p.Amount,
                PaymentDate = p.PaymentDate,
                PaymentMethod = p.PaymentMethod,
                Notes = p.Notes,
                PaymentMethodId = p.PaymentMethodId,
                Status = p.Status,
                TransactionId = p.TransactionId,
                GroupId = p.GroupId,
                CreatedAt = p.CreatedAt
            }).ToList();
        }

        public List<BLLPayment> GetByStudentId(int studentId)
        {
            List<Payment> p = dal.Payments.GetByStudentId(studentId);
            List<BLLPayment> bllP = new List<BLLPayment>();

            foreach (Payment payment in p)
            {
                BLLPayment bll = new BLLPayment()
                {
                    PaymentId = payment.PaymentId,
                    StudentId = payment.StudentId,
                    Amount = payment.Amount,
                    PaymentDate = payment.PaymentDate,
                    PaymentMethod = payment.PaymentMethod,
                    Notes = payment.Notes,
                    PaymentMethodId = payment.PaymentMethodId,
                    Status = payment.Status,
                    TransactionId = payment.TransactionId,
                    GroupId = payment.GroupId,
                    CreatedAt = payment.CreatedAt
                };
                bllP.Add(bll);
            }
            return bllP;
        }

        public BLLPayment GetById(int paymentId)
        {
            Payment p = dal.Payments.GetById(paymentId);
            return new BLLPayment()
            {
                PaymentId = p.PaymentId,
                StudentId = p.StudentId,
                Amount = p.Amount,
                PaymentDate = p.PaymentDate,
                PaymentMethod = p.PaymentMethod,
                Notes = p.Notes,
                PaymentMethodId = p.PaymentMethodId,
                Status = p.Status,
                TransactionId = p.TransactionId,
                GroupId = p.GroupId,
                CreatedAt = p.CreatedAt
            };
        }

        public void Update(BLLPayment payment)
        {
            var p = dal.Payments.GetById(payment.PaymentId);
            p.StudentId = payment.StudentId;
            p.Amount = payment.Amount;
            p.PaymentDate = payment.PaymentDate;
            p.PaymentMethod = payment.PaymentMethod ?? string.Empty;
            p.Notes = payment.Notes ?? string.Empty;
            p.PaymentMethodId = payment.PaymentMethodId;
            p.Status = payment.Status ?? "PENDING";
            p.TransactionId = payment.TransactionId;
            p.GroupId = payment.GroupId;
            p.CreatedAt = payment.CreatedAt;

            dal.Payments.Update(p);
        }

        public List<BLLPayment> GetByPaymentMethodId(int paymentMethodId)
        {
            List<Payment> p = dal.Payments.GetByPaymentMethodId(paymentMethodId);
            List<BLLPayment> bllP = new List<BLLPayment>();

            foreach (Payment payment in p)
            {
                BLLPayment bll = new BLLPayment()
                {
                    PaymentId = payment.PaymentId,
                    StudentId = payment.StudentId,
                    Amount = payment.Amount,
                    PaymentDate = payment.PaymentDate,
                    PaymentMethod = payment.PaymentMethod,
                    Notes = payment.Notes,
                    PaymentMethodId = payment.PaymentMethodId,
                    Status = payment.Status,
                    TransactionId = payment.TransactionId,
                    GroupId = payment.GroupId,
                    CreatedAt = payment.CreatedAt
                };
                bllP.Add(bll);
            }
            return bllP;
        }
    }
}

