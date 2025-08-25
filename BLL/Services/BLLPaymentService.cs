using BLL.Api;
using DAL.Api;
using DAL.Models;
using RestSharp;


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
            try
            {
                var payments = dal.Payments.Get();
                if (payments == null || !payments.Any())
                {
                    Console.WriteLine("No payments found.");
                    return new List<BLLPayment>(); // מחזיר מערך ריק
                }

                return payments.Select(p => new BLLPayment
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
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching payments: {ex.Message}");
                return new List<BLLPayment>(); // מחזיר מערך ריק במקרה של שגיאה
            }
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

        public async Task<string> CreateGrowWalletPaymentAsync(
     BLLPayment payment,
     decimal amount,
     string fullName,
     string phone,
     string description,
     int studentId,
     string creditCardNumber)
        {
            var student = dal.Students.GetById(studentId);
            if (student == null)
            {
                throw new Exception("Student not found.");
            }

            // קישור התשלום לתלמיד
            payment.StudentId = student.Id;

            var req = new GrowPaymentRequest
            {
                pageCode = "79fd16425870", // קוד ייחודי לארנק
                userId = "b03a08c792436c6d", // מזהה ייחודי לעסק
                chargeType = 1,
                sum = amount,
                successUrl = "https://coursenet.nethost.co.il",
                cancelUrl = "https://coursenet.nethost.co.il",
                description = description,
                pageField_fullName = fullName,
                pageField_phone = phone,
                pageField_email = "test@example.com",
                cField1 = "Custom Field 1",
                cField2 = "Custom Field 2",
                creditCardNumber = creditCardNumber
            };

            var result = await dal.PaymentGrow.CreatePaymentProcessAsync(req);

            if (result.status != 1)
            {
                throw new Exception($"Grow error: {result.message}");
            }

            payment.TransactionId = result.data.processToken;
            payment.Status = "PENDING";
            payment.CreatedAt = DateTime.Now;
            payment.PaymentDate = DateTime.Now;
            Create(payment);

            string redirectUrl = $"https://sandbox.meshulam.co.il/api/light/server/1.0/redirect?processToken={result.data.processToken}";
            return redirectUrl;
        }
        public BLLPayment GetByTransactionId(string transactionId)
        {
            var payment = dal.Payments.GetByTransactionId(transactionId);
            if (payment == null)
            {
                return null;
            }

            return new BLLPayment
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
        }
        public async Task ApproveTransactionAsync(string paymentCode)
        {
            var client = new RestClient("https://secure.meshulam.co.il/api/light/server/1.0/approveTransaction");
            var request = new RestRequest();
            request.AddParameter("paymentCode", paymentCode);
            request.AddParameter("status", "APPROVED");

            var response = await client.PostAsync(request);
            if (response == null || !response.IsSuccessful)
            {
                throw new Exception("Failed to approve transaction.");
            }
        }

    }
}

