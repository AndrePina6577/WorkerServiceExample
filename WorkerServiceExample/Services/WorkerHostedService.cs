using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace WorkerServiceExample.Services
{
    public class WorkerHostedService : IHostedService
    {
        private readonly IServiceProvider serviceProvider;
        private IWorker worker;

        public WorkerHostedService(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            worker = serviceProvider.GetRequiredService<IWorker>();

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            this.worker.Dispose();

            //this.worker.AwaitThreadsConclusion(); Here you can make a method to finish all threads, will help with concurrency and logging

            return Task.CompletedTask;
        }
    }
}
