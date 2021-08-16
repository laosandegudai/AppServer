using System;
using System.Threading;
using System.Threading.Tasks;

using ASC.Common;
using ASC.Web.Files.Utils;

using Microsoft.Extensions.Hosting;

namespace ASC.DeleteExpired
{
    [Singletone]
    public class DeleteExpiredServiceLauncher : IHostedService
    {
        private Timer ClearTimer;
        private DeleterExpired DeleterExpired { get; }

        public DeleteExpiredServiceLauncher(DeleterExpired deleterExpired)
        {
            DeleterExpired = deleterExpired;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            ClearTimer = new Timer(DeleteExpired);
            ClearTimer.Change(new TimeSpan(0), TimeSpan.FromDays(1));
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            if (ClearTimer != null)
            {
                ClearTimer.Dispose();
            }
            return Task.CompletedTask;
        }

        private void DeleteExpired(object state)
        {
            DeleterExpired.DeleteExpired();
        }
    }
}
