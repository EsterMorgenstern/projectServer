using BLL.Api;

/// <summary>
/// A background service that automatically marks daily attendance at a specific time.
/// </summary>
public class DailyAttendanceMarker : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="DailyAttendanceMarker"/> class.
    /// </summary>
    /// <param name="serviceProvider">The service provider used to resolve dependencies.</param>
    public DailyAttendanceMarker(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    /// Executes the background task to mark daily attendance at 18:00 every day.
    /// </summary>
    /// <param name="stoppingToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the background operation.</returns>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Console.WriteLine("DailyAttendanceMarker: Service started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var now = DateTime.Now;
                var nextRun = now.Date.AddHours(18).AddMinutes(0);

                // אם כבר עברנו את השעה היום, קבע למחר
                if (now > nextRun)
                {
                    nextRun = nextRun.AddDays(1);
                }

                var delay = nextRun - now;
                Console.WriteLine($"DailyAttendanceMarker: Next run scheduled for {nextRun}");

                // המתן עד השעה הקבועה
                await Task.Delay(delay, stoppingToken);

                // אם נבקש לעצור במהלך ההמתנה
                if (stoppingToken.IsCancellationRequested)
                    break;

                // הפעל את סימון הנוכחות
                await ExecuteDailyAttendanceMarking();
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("DailyAttendanceMarker: Service stopped gracefully.");
                break;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"DailyAttendanceMarker: Unexpected error in main loop: {ex.Message}");
                // המתן 5 דקות לפני ניסיון נוסף במקרה של שגיאה
                await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
            }
        }
    }

    /// <summary>
    /// מבצע את פעולת סימון הנוכחות היומית
    /// </summary>
    private async Task ExecuteDailyAttendanceMarking()
    {
        using var scope = _serviceProvider.CreateScope();

        try
        {
            var attendanceService = scope.ServiceProvider.GetRequiredService<IBLLAttendance>();

            Console.WriteLine($"DailyAttendanceMarker: Starting daily attendance marking at {DateTime.Now}");

            // ?? כאן התיקון החשוב - הוסף await!
            await attendanceService.AutoMarkDailyAttendance();

            Console.WriteLine($"DailyAttendanceMarker: Daily attendance marking completed successfully at {DateTime.Now}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"DailyAttendanceMarker: Error during AutoMarkDailyAttendance execution:");
            Console.WriteLine($"  Message: {ex.Message}");
            Console.WriteLine($"  StackTrace: {ex.StackTrace}");

            // אפשר לשלוח התראה או לנסות שוב
            Console.WriteLine("DailyAttendanceMarker: Will retry tomorrow at scheduled time.");
        }
    }
}
