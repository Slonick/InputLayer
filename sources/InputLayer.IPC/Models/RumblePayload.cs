using System.Xml.Serialization;

namespace InputLayer.IPC.Models
{
    public class RumbleMessage : IPCMessage
    {
        public RumbleMessage() { }

        public RumbleMessage(int durationMs, float intensity)
        {
            this.DurationMs = durationMs;
            this.Intensity = intensity;
        }

        [XmlAttribute]
        public int DurationMs { get; set; }

        [XmlAttribute]
        public float Intensity { get; set; }

        /// <inheritdoc/>
        public override string ToString()
            => $"Rumble: {this.DurationMs}ms, {this.Intensity}";
    }
}