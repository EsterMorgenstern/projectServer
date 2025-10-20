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
    /// Executes the background task to mark daily attendance at 17:15 every day.
    /// </summary>
    /// <param name="stoppingToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the background operation.</returns>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Console.WriteLine("DailyAttendanceMarker: Service started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            var now = DateTime.Now;
            var nextRun = now.Date.AddHours(18).AddMinutes(00);
            if (now > nextRun)
            {
                nextRun = nextRun.AddDays(1).AddHours(18).AddMinutes(00);
            }


            var delay = nextRun - now;

            Console.WriteLine($"DailyAttendanceMarker: Next run scheduled for {nextRun}");
            await Task.Delay(delay, stoppingToken);

            using (var scope = _serviceProvider.CreateScope())
            {
                try
                {
                    var attendanceService = scope.ServiceProvider.GetRequiredService<IBLLAttendance>();
                    Console.WriteLine("DailyAttendanceMarker: Invoking AutoMarkDailyAttendance...");
                    attendanceService.AutoMarkDailyAttendance();
                    Console.WriteLine("DailyAttendanceMarker: AutoMarkDailyAttendance completed successfully.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"DailyAttendanceMarker: Error during AutoMarkDailyAttendance execution: {ex.Message}");
                }
            }
        }
    }
}
