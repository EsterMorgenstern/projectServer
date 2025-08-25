using System.Text.Json;
using DAL.Api;
using DAL.Models;

public class DALGrowPaymentService : IDALGrowPayment
{
    private readonly HttpClient _httpClient;

    public DALGrowPaymentService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<GrowPaymentResponse> CreatePaymentProcessAsync(GrowPaymentRequest req)
    {
        var formData = new MultipartFormDataContent
    {
        { new StringContent(req.pageCode), "pageCode" },
        { new StringContent(req.userId), "userId" },
        { new StringContent(req.chargeType.ToString()), "chargeType" },
        { new StringContent(req.sum.ToString()), "sum" },
        { new StringContent(req.successUrl), "successUrl" },
        { new StringContent(req.cancelUrl), "cancelUrl" },
        { new StringContent(req.description), "description" },
        { new StringContent(req.pageField_fullName), "pageField[fullName]" },
        { new StringContent(req.pageField_phone), "pageField[phone]" }
    };

        if (!string.IsNullOrEmpty(req.pageField_email))
            formData.Add(new StringContent(req.pageField_email), "pageField[email]");

        if (!string.IsNullOrEmpty(req.cField1))
            formData.Add(new StringContent(req.cField1), "cField1");

        if (!string.IsNullOrEmpty(req.cField2))
            formData.Add(new StringContent(req.cField2), "cField2");

        // הוסף לוגים לנתונים שנשלחים
        Console.WriteLine("Request Data:");
        Console.WriteLine($"pageCode: {req.pageCode}");
        Console.WriteLine($"userId: {req.userId}");
        Console.WriteLine($"chargeType: {req.chargeType}");
        Console.WriteLine($"sum: {req.sum}");
        Console.WriteLine($"successUrl: {req.successUrl}");
        Console.WriteLine($"cancelUrl: {req.cancelUrl}");
        Console.WriteLine($"description: {req.description}");
        Console.WriteLine($"pageField_fullName: {req.pageField_fullName}");
        Console.WriteLine($"pageField_phone: {req.pageField_phone}");
        Console.WriteLine($"pageField_email: {req.pageField_email}");

        var response = await _httpClient.PostAsync("https://sandbox.meshulam.co.il/api/light/server/1.0/createPaymentProcess", formData);
        var json = await response.Content.ReadAsStringAsync();

        // הוסף לוגים לתגובה שמתקבלת
        Console.WriteLine("Response JSON: " + json);

        if (string.IsNullOrEmpty(json))
            throw new Exception("No response from the server.");

        return JsonSerializer.Deserialize<GrowPaymentResponse>(json);
    }



}
