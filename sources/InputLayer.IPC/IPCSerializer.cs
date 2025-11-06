using InputLayer.Common.Utils;
using InputLayer.IPC.Models;

namespace InputLayer.IPC
{
    public static class IPCSerializer
    {
        public static IIPCMessage Deserialize(string message) => Serializer.Deserialize<IIPCMessage>(message);
        public static string Serialize(IIPCMessage message) => Serializer.Serialize(message);
    }
}