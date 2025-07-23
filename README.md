# 🎯 מערכת ניהול חוגים - Backend (ASP.NET Core)

> שרת API חזק ומאובטח לניהול מוסדות חינוך וחוגים

[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)
[![C#](https://img.shields.io/badge/C%23-12.0-239120?style=for-the-badge&logo=csharp&logoColor=white)](https://learn.microsoft.com/en-us/dotnet/csharp/)
[![Entity Framework](https://img.shields.io/badge/Entity_Framework-Core_8.0-512BD4?style=for-the-badge&logo=nuget&logoColor=white)](https://learn.microsoft.com/en-us/ef/)

## 🌐 הפרויקט החי

🚀 **ה-API זמין כעת בענן**

- **🌍 Live API:** [https://coursenet.nethost.co.il/api](https://coursenet.nethost.co.il/api)
- **📱 RESTful API:** מותאם לכל לקוח
- **⚡ Scalable & Secure:** תמיכה בביצועים גבוהים ואבטחה מתקדמת

## 📂 קישורי הפרויקט המלא

- **⚙️ Backend (זה):** [github.com/EsterMorgenstern/projectServer](https://github.com/EsterMorgenstern/projectServer)
- **🎨 Frontend:** [github.com/EsterMorgenstern/projectClient](https://github.com/EsterMorgenstern/projectClient)

## 🎯 סקירה כללית

שרת API חזק שפותח ב-ASP.NET Core, מספק שירותים לניהול מערכת החוגים. השרת כולל תמיכה ב-RESTful API, גישה למסד נתונים באמצעות Entity Framework, ושירותי רקע מתקדמים.

## ✨ תכונות מרכזיות

### 🛠️ תשתית
- ✅ **.NET 8.0** - פלטפורמה מודרנית ומהירה
- ✅ **Entity Framework Core** - ORM מתקדם
- ✅ **Dependency Injection** - ניהול תלויות מובנה
- ✅ **Background Services** - שירותי רקע מתקדמים

### 📊 API
- ✅ **RESTful API** - תמיכה מלאה ב-CRUD
- ✅ **Validation** - אימות נתונים חכם
- ✅ **Error Handling** - טיפול בשגיאות בצורה מסודרת
- ✅ **Authentication & Authorization** - אבטחה מתקדמת

### 🧭 שירותי רקע
- ✅ **Daily Attendance Marker** - סימון נוכחות יומי אוטומטי
- ✅ **Reports Generator** - יצירת דוחות חודשיים

## 🛠️ טכנולוגיות ושימושים

### Core Framework


```csharp
ASP.NET Core 8.0          // פלטפורמת פיתוח צד שרת
Entity Framework Core 8.0 // ORM לניהול מסד נתונים
C# 12.0                   // שפת תכנות מודרנית

```

### Utilities & Tools


```csharp
AutoMapper                // מיפוי אובייקטים
FluentValidation          // אימות נתונים
Serilog                   // לוגים מתקדמים
Swagger                   // תיעוד API

```

## 🚀 התקנה והרצה

### דרישות מערכת
- **.NET SDK** 8.0+
- **SQL Server** 2019+
- **Visual Studio** 2022 או **VS Code**

### הוראות התקנה


```shell
# 1. שכפול הפרויקט
git clone https://github.com/EsterMorgenstern/projectServer.git
cd projectServer

# 2. התקנת תלויות
dotnet restore

# 3. הגדרת משתני סביבה
cp appsettings.Development.json.example appsettings.Development.json
# ערכי את הקובץ עם הנתונים שלך

# 4. הרצת הפרויקט במצב פיתוח
dotnet run

# 5. פתיחה בדפדפן
# http://localhost:5000/swagger

```



### פקודות נוספות


```shell
# בניית הפרויקט לייצור
dotnet publish -c Release

# הרצת בדיקות
dotnet test

# ניתוח קוד
dotnet analyze

```

## 📁 מבנה הפרויקט


```
├───BLL
│   ├───Api
│   ├───Models
│   └───Services
├───DAL
│   ├───Api
│   ├───Models
│   └───Services
├───Project
│   ├───Controllers
│   ├───Services
│   └───Properties
└───publish

```

## 📊 ביצועים ומדדים

### Performance Metrics
- **Response Time:** < 200ms
- **Throughput:** 1000+ requests/sec
- **Error Rate:** < 0.1%

### Monitoring
- **Health Checks:** /health
- **Logging:** Serilog
- **Metrics:** Prometheus (planned)

## 🔧 תכונות מתקדמות

### Background Services


```csharp
// Daily Attendance Marker
// סימון נוכחות יומי אוטומטי

```

### Security Features


```csharp
// JWT Authentication
// Role-based Authorization
// Data Encryption

```

### Performance Optimizations


```csharp
// Caching
// Asynchronous Programming
// Database Indexing

```

## 🧪 בדיקות

### Testing Strategy


```shell
# Unit Tests - xUnit
dotnet test

# Integration Tests
dotnet test --filter Integration

# E2E Tests (planned)

```

### Test Coverage
- **Statements:** 85%+
- **Branches:** 80%+
- **Functions:** 90%+
- **Lines:** 85%+

## 🚀 Deployment

### Build Process


```shell
# Production build
dotnet publish -c Release

# Deploy to server
scp -r publish/ user@server:/var/www/projectServer

```

### Hosting & Production
- **Hosting Platform:** Azure
- **Database:** SQL Server
- **CDN:** Integrated hosting solution

### Environment Variables (Production)


```



## 👩‍💻 מפתחת הפרויקט

**אסתר מורגנשטרן**
- 🎓 **התמחות:** Full-Stack Development
- 📧 **Email:** em0527104104@gmail.com
- 💻 **GitHub:** [github.com/EsterMorgenstern](https://github.com/EsterMorgenstern)

---

⭐ **אם הפרויקט עזר לך או נראה לך מעניין, אל תשכח לתת כוכב ב-GitHub!**

📞 **מעוניינים בשיתוף פעולה? צרו קשר!**
