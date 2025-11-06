using System.IO.Pipes;
using System.Threading;
using System.Threading.Tasks;
using InputLayer.Common.Logging;

namespace InputLayer.IPC
{
    public class IPCClient : IPCConnect<NamedPipeClientStream>
    {
        /// <inheritdoc/>
        protected override ILogger Logger { get; } = LogManager.Default.GetCurrentClassLogger();

        /// <inheritdoc/>
        protected override void WaitForConnection()
        {
            Connection = new NamedPipeClientStream(".", Constants.Name, PipeDirection.InOut, PipeOptions.Asynchronous);
            Connection.Connect();
        }

        /// <inheritdoc/>
        protected override async Task WaitForConnectionAsync(CancellationToken cancellationToken)
        {
            Connection = new NamedPipeClientStream(".", Constants.Name, PipeDirection.InOut, PipeOptions.Asynchronous);
            await Connection.ConnectAsync(cancellationToken);
        }
    }
}