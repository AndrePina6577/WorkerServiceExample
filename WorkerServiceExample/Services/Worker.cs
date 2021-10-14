using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace WorkerServiceExample.Services
{
    public class Worker : IWorker
    {
        // Implement with retries if makes sense
        public readonly int retries;
        private readonly ILogger<Worker> logger;
        private bool isDisposed;
        private readonly CancellationTokenSource tokenSource = new CancellationTokenSource();

        /// <summary>
        /// Implement necessary services (i.e ClientService) or something.
        /// </summary>
        /// <param name="retries">Hard coded but can be given value with autofac parameter configuration</param>
        public Worker(ILogger<Worker> logger, int retries = 3)
        {
            this.logger = logger;
            this.retries = retries;
            StartJob();
        }

        public void SomeJob(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                try
                {
                    logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                    Task.Delay(1000, token).Wait();
                }
                catch (Exception e)
                {
                    logger.LogError(e, e.Message);

                    if (retries <= 0)
                    {
                        //Do something like updating current to error in database
                    }

                    continue;
                }
            }
        }

        private void StartJob()
        {
            new Thread(() => SomeJob(tokenSource.Token)).Start();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                if (disposing)
                {
                    tokenSource.Cancel();
                    tokenSource.Dispose();
                }

                isDisposed = true;
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
