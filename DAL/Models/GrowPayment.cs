namespace DAL.Models
{

    public class GrowPaymentRequest
    {
        public int studentId { get; set; } // מזהה ייחודי לתלמיד
        public string? pageCode { get; set; } // מזהה ייחודי לשיטת התשלום
        public string? userId { get; set; } // מזהה ייחודי לעסק
        public int chargeType { get; set; } = 1; // סוג החיוב (ברירת מחדל: 1 - חיוב רגיל)
        public decimal sum { get; set; } // סכום התשלום
        public string? successUrl { get; set; } // כתובת URL לאחר תשלום מוצלח
        public string? cancelUrl { get; set; } // כתובת URL לאחר ביטול התשלום
        public string? description { get; set; } // תיאור המוצר או השירות
        public string? pageField_fullName { get; set; } // שם מלא של המשלם
        public string? pageField_phone { get; set; } // מספר טלפון של המשלם
        public string? pageField_email { get; set; } // כתובת אימייל של המשלם (אופציונלי)
        public string? cField1 { get; set; } // שדה מותאם אישית 1 (אופציונלי)
        public string? cField2 { get; set; } // שדה מותאם אישית 2 (אופציונלי)
        public string? creditCardNumber { get; set; } // מספר כרטיס אשראי לבדיקה
    }

}

public class GrowPaymentResponse
{
    public int status { get; set; }
    public string? message { get; set; }
    public GrowPaymentData? data { get; set; }
}

public class GrowPaymentData
{
    public string? paymentCode { get; set; }
    public string? processToken { get; set; }
    public string? redirectUrl { get; set; }
}

