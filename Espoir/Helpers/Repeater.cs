namespace Espoir.Helpers
{
    internal class Repeater
    {
        public Func<CancellationToken, Task> Action { get; set; }

        public CancellationTokenSource CancelTokenSource { get; }
        
        public Task? Task { get; private set; }

        public Repeater(Func<CancellationToken, Task> action)
        {
            this.Action = action;
            this.CancelTokenSource = new CancellationTokenSource();
        }

        public void Start()
        {
            var token = this.CancelTokenSource.Token;
            this.Task = Task.Run(async () =>
            {
                await this.Action(token);
            }, token);
        }

        public void Stop()
        {
            this.CancelTokenSource.Cancel();
            this.Task?.Wait();
            this.CancelTokenSource?.Dispose();
        }
    }
}
