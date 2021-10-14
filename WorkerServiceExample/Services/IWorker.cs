using System;
using System.Threading;

namespace WorkerServiceExample.Services
{
    public interface IWorker : IDisposable
    {
        void SomeJob(CancellationToken token);
    }
}