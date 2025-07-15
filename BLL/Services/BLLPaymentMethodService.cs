using BLL.Api;
using DAL.Api;
using DAL.Models;

namespace BLL.Services
{
    public class BLLPaymentMethodService : IBLLPaymentMethod
    {
        private readonly IDAL dal;

        public BLLPaymentMethodService(IDAL dal)
        {
            this.dal = dal;
        }

        public void Create(BLLPaymentMethod paymentMethod)
        {
            PaymentMethod pm = new PaymentMethod()
            {
                StudentId = paymentMethod.StudentId,
                MethodType = paymentMethod.MethodType ?? string.Empty,
                IsDefault = paymentMethod.IsDefault,
                IsActive = paymentMethod.IsActive,
                CreatedAt = paymentMethod.CreatedAt,
                UpdatedAt = paymentMethod.UpdatedAt,
                LastFourDigits = paymentMethod.LastFourDigits ?? string.Empty,
                CardType = paymentMethod.CardType ?? string.Empty,
                ExpiryMonth = paymentMethod.ExpiryMonth,
                ExpiryYear = paymentMethod.ExpiryYear,
                BankName = paymentMethod.BankName ?? string.Empty,
                AccountHolderName = paymentMethod.AccountHolderName ?? string.Empty
            };
            dal.PaymentMethods.Create(pm);
        }

        public void Delete(int paymentMethodId)
        {
            dal.PaymentMethods.Delete(paymentMethodId);
        }

        public List<BLLPaymentMethod> Get()
        {
            return dal.PaymentMethods.Get().Select(pm => new BLLPaymentMethod()
            {
                PaymentMethodId = pm.PaymentMethodId,
                StudentId = pm.StudentId,
                MethodType = pm.MethodType,
                IsDefault = pm.IsDefault,
                IsActive = pm.IsActive,
                CreatedAt = pm.CreatedAt,
                UpdatedAt = pm.UpdatedAt,
                LastFourDigits = pm.LastFourDigits,
                CardType = pm.CardType,
                ExpiryMonth = pm.ExpiryMonth,
                ExpiryYear = pm.ExpiryYear,
                BankName = pm.BankName,
                AccountHolderName = pm.AccountHolderName
            }).ToList();
        }

        public List<BLLPaymentMethod> GetByStudentId(int studentId)
        {
            List<PaymentMethod> pm = dal.PaymentMethods.GetByStudentId(studentId);
            List<BLLPaymentMethod> bllPm = new List<BLLPaymentMethod>();

            foreach (PaymentMethod p in pm)
            {
                BLLPaymentMethod bll = new BLLPaymentMethod()
                {
                    PaymentMethodId = p.PaymentMethodId,
                    StudentId = p.StudentId,
                    MethodType = p.MethodType,
                    IsDefault = p.IsDefault,
                    IsActive = p.IsActive,
                    CreatedAt = p.CreatedAt,
                    UpdatedAt = p.UpdatedAt,
                    LastFourDigits = p.LastFourDigits,
                    CardType = p.CardType,
                    ExpiryMonth = p.ExpiryMonth,
                    ExpiryYear = p.ExpiryYear,
                    BankName = p.BankName,
                    AccountHolderName = p.AccountHolderName
                };
                bllPm.Add(bll);
            }
            return bllPm;
        }

        public BLLPaymentMethod GetById(int paymentMethodId)
        {
            PaymentMethod pm = dal.PaymentMethods.GetById(paymentMethodId);
            return new BLLPaymentMethod()
            {
                PaymentMethodId = pm.PaymentMethodId,
                StudentId = pm.StudentId,
                MethodType = pm.MethodType,
                IsDefault = pm.IsDefault,
                IsActive = pm.IsActive,
                CreatedAt = pm.CreatedAt,
                UpdatedAt = pm.UpdatedAt,
                LastFourDigits = pm.LastFourDigits,
                CardType = pm.CardType,
                ExpiryMonth = pm.ExpiryMonth,
                ExpiryYear = pm.ExpiryYear,
                BankName = pm.BankName,
                AccountHolderName = pm.AccountHolderName
            };
        }

        public void Update(BLLPaymentMethod paymentMethod)
        {
            var pm = dal.PaymentMethods.GetById(paymentMethod.PaymentMethodId);
            pm.StudentId = paymentMethod.StudentId;
            pm.MethodType = paymentMethod.MethodType ?? string.Empty;
            pm.IsDefault = paymentMethod.IsDefault;
            pm.IsActive = paymentMethod.IsActive;
            pm.CreatedAt = paymentMethod.CreatedAt;
            pm.UpdatedAt = paymentMethod.UpdatedAt;
            pm.LastFourDigits = paymentMethod.LastFourDigits ?? string.Empty;
            pm.CardType = paymentMethod.CardType ?? string.Empty;
            pm.ExpiryMonth = paymentMethod.ExpiryMonth;
            pm.ExpiryYear = paymentMethod.ExpiryYear;
            pm.BankName = paymentMethod.BankName ?? string.Empty;
            pm.AccountHolderName = paymentMethod.AccountHolderName ?? string.Empty;

            dal.PaymentMethods.Update(pm);
        }

        public void SetAsDefault(int paymentMethodId, int studentId)
        {
            dal.PaymentMethods.SetAsDefault(paymentMethodId, studentId);
        }
    }
}