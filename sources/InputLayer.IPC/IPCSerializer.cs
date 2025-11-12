using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using InputLayer.Common.Logging;
using InputLayer.IPC.Models;

namespace InputLayer.IPC
{
    public static class IPCSerializer
    {
        private static readonly ILogger _logger = LogManager.Default.GetCurrentClassLogger();
        private static readonly XmlSerializerNamespaces _namespaces;
        private static readonly XmlSerializer _serializer = new XmlSerializer(typeof(IPCMessage));

        static IPCSerializer()
        {
            _namespaces = new XmlSerializerNamespaces();
            _namespaces.Add("", "clr-namespace:InputLayer.IPC");
            _namespaces.Add("xsi", "http://www.w3.org/2001/XMLSchema-instance");
        }

        public static IPCMessage Deserialize(string xml)
        {
            try
            {
                if (string.IsNullOrEmpty(xml))
                {
                    throw new ArgumentException("XML string cannot be null or empty", nameof(xml));
                }

                using (var stringReader = new StringReader(xml))
                {
                    using (var xmlReader = XmlReader.Create(stringReader))
                    {
                        return (IPCMessage)_serializer.Deserialize(xmlReader);
                    }
                }
            }
            catch (InvalidOperationException ex)
            {
                _logger.Error(ex, "Error deserializing IPC message");
                throw new InvalidOperationException("Failed to deserialize IPC message", ex);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Unexpected error deserializing IPC message");
                throw;
            }
        }

        public static string Serialize(IPCMessage message)
        {
            try
            {
                if (message == null)
                {
                    throw new ArgumentNullException(nameof(message));
                }

                using (var stringWriter = new StringWriter())
                {
                    using (var xmlWriter = XmlWriter.Create(stringWriter, new XmlWriterSettings
                           {
                               Indent = false,
                               OmitXmlDeclaration = true,
                               Encoding = Encoding.UTF8
                           }))
                    {
                        _serializer.Serialize(xmlWriter, message, _namespaces);
                        return stringWriter.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error serializing IPC message");
                throw;
            }
        }
    }
}