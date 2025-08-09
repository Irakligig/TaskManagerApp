
using TaskManagerApp.Data;
using TaskManagerApp.Services;

namespace TaskManagerApp.BackGroundService
{
    public class AutoSaveService : BackgroundService
    {
        private readonly ITaskService _service;
        private readonly ILogger<AutoSaveService> _logger;

        public AutoSaveService(ITaskService service, ILogger<AutoSaveService> logger)
        {
            this._service = service;
            _logger = logger;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
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
