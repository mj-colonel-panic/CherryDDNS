namespace CherryDDNS
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }
        private bool IsRunning = false;
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                if (IsRunning) { return; }
                IsRunning = true;
                LogManager.Startup();
                while (!stoppingToken.IsCancellationRequested)
                {
                    DDNSService.Instance.Update();
                    await Task.Delay(1000, stoppingToken);
                }
            }
            catch (TaskCanceledException)
            {
                LogManager.Cancelled();
            }
            catch (Exception ex)
            {
                LogManager.Error(ex);
            }
            finally
            {
                IsRunning = false;
            }
        }
    }
}