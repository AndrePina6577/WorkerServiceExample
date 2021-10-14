using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace WorkerServiceExample
{
    public class WorkerDefault : IHostedService
    {
        private readonly ILogger<WorkerDefault> _logger;
        private Task task;
        private readonly CancellationTokenSource tokenSource = new CancellationTokenSource();

        public WorkerDefault(ILogger<WorkerDefault> logger)
        {
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            //this will keep running until cancelation is requested
            task = SomeJob(tokenSource.Token);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            if (task == null)
            {
                return null;
            }

            try
            {
                tokenSource.Cancel();
            }
            finally
            {
                Task.WhenAny(task, Task.Delay(Timeout.Infinite, cancellationToken));
            }

            return Task.CompletedTask;
        }

        protected async Task SomeJob(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(1000, token);
            }
        }

        //protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        //{
        //    while (!stoppingToken.IsCancellationRequested)
        //    {
        //        _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
        //        await Task.Delay(1000, stoppingToken);
        //    }
        //}
    }
}
