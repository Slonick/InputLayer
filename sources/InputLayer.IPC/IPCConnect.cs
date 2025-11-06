using System;
using System.IO;
using System.IO.Pipes;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using InputLayer.Common.Logging;
using InputLayer.IPC.Models;

namespace InputLayer.IPC
{
    public abstract class IPCConnect<TPipe> : IDisposable where TPipe : PipeStream
    {
        protected TPipe Connection;
        private volatile bool _isDisposed;
        private Thread _listenThread;
        private StreamWriter _writer;
        public event Action Connected;
        public event Action Disconnected;
        public event Action<IIPCMessage> MessageReceived;

        protected abstract ILogger Logger { get; }

        public void Connect()
        {
            this.Logger.Trace("Wait for connection...");

            this.WaitForConnection();
            this.SetupClient();

            this.Connected?.Invoke();
            this.Logger.Trace("Connected");
        }

        public async Task ConnectAsync(CancellationToken cancellationToken = default)
        {
            this.Logger.Trace("Wait for connection...");

            await this.WaitForConnectionAsync(cancellationToken);
            this.SetupClient();

            this.Connected?.Invoke();
            this.Logger.Trace("Connected");
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            if (_isDisposed)
            {
                return;
            }

            _isDisposed = true;
            this.Cleanup();

            _listenThread?.Join(300);
        }

        public void Send(IIPCMessage message)
        {
            var payload = IPCSerializer.Serialize(message);

            this.Logger.Trace($"Sending message: {payload}");

            _writer.WriteLine(payload);
            _writer.Flush();
        }

        protected abstract void WaitForConnection();
        protected abstract Task WaitForConnectionAsync(CancellationToken cancellationToken);

        private void Cleanup()
        {
            try
            {
                _writer?.Dispose();
            }
            catch (IOException)
            {
                // Pipe already broken, ignore
            }
            finally
            {
                _writer = null;
            }

            Connection?.Dispose();
            Connection = null;
        }

        private void EventLoop()
        {
            using (var reader = new StreamReader(Connection, Encoding.UTF8, true, 1024, true))
            {
                while (Connection?.IsConnected == true)
                {
                    this.Logger.Trace("Reading message...");

                    var messageData = reader.ReadLine();
                    if (!string.IsNullOrWhiteSpace(messageData))
                    {
                        this.Logger.Trace($"Received message: {messageData}");
                        this.MessageReceived?.Invoke(IPCSerializer.Deserialize(messageData));
                    }

                    Thread.Sleep(16);
                }
            }

            if (!_isDisposed)
            {
                this.Disconnected?.Invoke();
                this.Cleanup();
                this.Connect();
            }
        }

        private void SetupClient()
        {
            _writer = new StreamWriter(Connection, Encoding.UTF8, 1024, true);

            _listenThread?.Join();
            _listenThread = new Thread(this.EventLoop) { IsBackground = true };
            _listenThread.Start();
        }
    }
}