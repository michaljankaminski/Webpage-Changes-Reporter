using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ChangesDetectorConsole.service
{
    public abstract class BackgroundService : IHostedService, IDisposable
    {
        private Task _executingTask;

        private readonly CancellationTokenSource _stoppingCts =
                                                       new CancellationTokenSource();

        protected abstract Task ExecuteAsync(CancellationToken stoppingToken);

        public void Dispose()
        {
            _stoppingCts.Cancel();
        }

        public virtual Task StartAsync(CancellationToken cancellationToken)
        {
            _executingTask = ExecuteAsync(_stoppingCts.Token);

            if (_executingTask.IsCompleted)
                return _executingTask;
            
            return Task.CompletedTask;
        }

        public virtual async Task StopAsync(CancellationToken cancellationToken)
        {
            // Stop called without start
            if (_executingTask == null)
                return;
            
            try
            {
                _stoppingCts.Cancel();
            }
            finally
            {
                await Task.WhenAny(_executingTask, Task.Delay(Timeout.Infinite,
                                                              cancellationToken));
            }
        }
    }
}
