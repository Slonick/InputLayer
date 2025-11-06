namespace InputLayer.IPC.Models
{
    public class RumbleMessage : IIPCMessage
    {
        public RumbleMessage() { }

        public RumbleMessage(int durationMs, float intensity)
        {
            this.DurationMs = durationMs;
            this.Intensity = intensity;
        }

        public int DurationMs { get; set; }

        public float Intensity { get; set; }

        /// <inheritdoc/>
        public override string ToString() => $"Rumble: {this.DurationMs}ms, {this.Intensity}";
    }

    public class PingMessage : IIPCMessage
    {
        /// <inheritdoc/>
        public override string ToString() => "Ping";
    }
}