
using TaskManagerApp.Data;
using TaskManagerApp.Services;

namespace TaskManagerApp.BackGroundService
{
    public class AutoSaveService : BackgroundService
    {
        private readonly IServiceProvider _serviceprovider;
        private readonly ILogger<AutoSaveService> _logger;

        public AutoSaveService(IServiceProvider serviceprovider, ILogger<AutoSaveService> logger)
        {
            this._serviceprovider = serviceprovider;
            _logger = logger;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    // create a scope to avoid lifetime issues
                    using var scope = _serviceprovider.CreateScope();
                    var _service = scope.ServiceProvider.GetRequiredService<ITaskService>();

                    await _service.ExportToJsonAsync();

                    await _service.ExportToXmlAsync();
                }
                catch(Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while exporting tasks.");
                }
                await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
            }
        }
    }
}
