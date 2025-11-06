using System.IO.Pipes;
using System.Threading;
using System.Threading.Tasks;
using InputLayer.Common.Logging;

namespace InputLayer.IPC
{
    public class IPCServer : IPCConnect<NamedPipeServerStream>
    {
        /// <inheritdoc/>
        protected override ILogger Logger { get; } = LogManager.Default.GetCurrentClassLogger();

        /// <inheritdoc/>
        protected override void WaitForConnection()
        {
            Connection = new NamedPipeServerStream(Constants.Name, PipeDirection.InOut, 1, PipeTransmissionMode.Message, PipeOptions.Asynchronous);
            Connection.WaitForConnection();
        }

        /// <inheritdoc/>
        protected override async Task WaitForConnectionAsync(CancellationToken cancellationToken)
        {
            Connection = new NamedPipeServerStream(Constants.Name, PipeDirection.InOut, 1, PipeTransmissionMode.Message, PipeOptions.Asynchronous);
            await Connection.WaitForConnectionAsync(cancellationToken);
        }
    }
}