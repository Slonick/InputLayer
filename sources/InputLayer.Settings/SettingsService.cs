using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using InputLayer.Common.Logging;
using InputLayer.Settings.Mappers;

namespace InputLayer.Settings
{
    public class SettingsService<TData, TViewModel>
        where TData : class
        where TViewModel : class
    {
        private readonly ILogger _logger = LogManager.Default.GetCurrentClassLogger();
        private readonly IViewModelMapper<TData, TViewModel> _mapper;
        private readonly XmlSerializerNamespaces _namespaces;
        private readonly SettingsServiceOptions _options;
        private readonly XmlSerializer _serializer;

        public SettingsService(IViewModelMapper<TData, TViewModel> mapper, SettingsServiceOptions options = null)
        {
            _options = options ?? new SettingsServiceOptions();
            _mapper = mapper;
            _serializer = new XmlSerializer(typeof(TData));

            _namespaces = new XmlSerializerNamespaces();
            _namespaces.Add("", "clr-namespace:InputLayer.Settings");
            _namespaces.Add("xsi", "http://www.w3.org/2001/XMLSchema-instance");

            foreach (var prefixEntry in _options.NamespacePrefixMap)
            {
                _namespaces.Add(prefixEntry.Key, prefixEntry.Value);
            }
        }

        public TViewModel Clone(TViewModel viewModel)
        {
            try
            {
                var data = _mapper.FromViewModel(viewModel);

                using (var memoryStream = new MemoryStream())
                {
                    using (var xmlWriter = XmlWriter.Create(memoryStream))
                    {
                        _serializer.Serialize(xmlWriter, data, _namespaces);
                    }

                    memoryStream.Position = 0;

                    using (var xmlReader = XmlReader.Create(memoryStream))
                    {
                        var clonedData = (TData)_serializer.Deserialize(xmlReader);
                        return _mapper.ToViewModel(clonedData);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error cloning settings");
                return null;
            }
        }

        public TViewModel Load(string path)
        {
            try
            {
                if (!File.Exists(path))
                {
                    _logger.Info($"Settings file not found: {path}");
                    return null;
                }

                _logger.Info($"Loading settings from: {path}");

                TData data;
                using (var reader = XmlReader.Create(path))
                {
                    data = (TData)_serializer.Deserialize(reader);
                }

                var viewModel = _mapper.ToViewModel(data);

                _logger.Info("Settings loaded successfully");
                return viewModel;
            }
            catch (InvalidOperationException ex)
            {
                _logger.Error(ex, "Serialization error");

                if (_options.CreateBackup)
                {
                    var backupPath = path + ".backup";
                    if (File.Exists(backupPath))
                    {
                        _logger.Info("Attempting to restore from backup...");
                        try
                        {
                            TData data;
                            using (var reader = XmlReader.Create(backupPath))
                            {
                                data = (TData)_serializer.Deserialize(reader);
                            }

                            var viewModel = _mapper.ToViewModel(data);

                            _logger.Info("Settings restored from backup");
                            return viewModel;
                        }
                        catch (Exception backupEx)
                        {
                            _logger.Error(backupEx, "Error loading backup");
                        }
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error loading settings");
                return null;
            }
        }

        public void Save(TViewModel viewModel, string path)
        {
            try
            {
                _logger.Info($"Saving settings to: {path}");

                var data = _mapper.FromViewModel(viewModel);

                var directory = Path.GetDirectoryName(path);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                if (_options.CreateBackup && File.Exists(path))
                {
                    var backupPath = path + ".backup";
                    if (File.Exists(backupPath))
                    {
                        File.Delete(backupPath);
                    }

                    File.Copy(path, backupPath);
                    _logger.Info($"Backup created: {backupPath}");
                }

                var xmlSettings = new XmlWriterSettings
                {
                    Indent = _options.Indent,
                    IndentChars = _options.IndentChars,
                    NewLineOnAttributes = false,
                    Encoding = Encoding.UTF8
                };

                using (var fileStream = File.Create(path))
                {
                    using (var xmlWriter = XmlWriter.Create(fileStream, xmlSettings))
                    {
                        _serializer.Serialize(xmlWriter, data, _namespaces);
                    }
                }

                _logger.Info("Settings saved successfully");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"Error saving settings: {ex.Message}");
                throw;
            }
        }
    }
}