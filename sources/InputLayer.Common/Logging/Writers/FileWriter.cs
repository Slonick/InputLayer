using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace InputLayer.Common.Logging.Writers
{
    public class FileWriter : ILogWriter
    {
        private const int MaxBackupFiles = 3;
        private readonly long _maxFileSize;
        private readonly Mutex _writeMutex;

        public FileWriter(string filePath, long maxFileSize = 10 * 1024 * 1024)
        {
            this.FilePath = filePath;
            _maxFileSize = maxFileSize;

            using (var md5 = MD5.Create())
            {
                var fullPath = Path.GetFullPath(filePath);
                var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(fullPath.ToLowerInvariant()));
                var mutexName = "Global\\InputLayerLog_" + BitConverter.ToString(hash).Replace("-", "");

                _writeMutex = new Mutex(false, mutexName);
            }
        }

        public string FilePath { get; }

        // Метод для очистки ресурсов
        public void Dispose()
        {
            _writeMutex?.Dispose();
        }

        public void Write(string line)
        {
            _writeMutex.WaitOne();

            try
            {
                if (this.ShouldRotateFile())
                {
                    this.RotateFiles();
                }

                using (var file = File.AppendText(this.FilePath))
                {
                    file.WriteLine(line);
                }
            }
            finally
            {
                _writeMutex.ReleaseMutex();
            }
        }

        private void RotateFiles()
        {
            try
            {
                var directory = Path.GetDirectoryName(this.FilePath) ?? throw new DirectoryNotFoundException();
                var fileName = Path.GetFileNameWithoutExtension(this.FilePath);
                var fileExtension = Path.GetExtension(this.FilePath);

                var oldestBackup = Path.Combine(directory, $"{fileName}.{MaxBackupFiles}{fileExtension}");
                if (File.Exists(oldestBackup))
                {
                    File.Delete(oldestBackup);
                }

                for (var i = MaxBackupFiles - 1; i >= 1; i--)
                {
                    var currentFile = Path.Combine(directory, $"{fileName}.{i}{fileExtension}");
                    var nextFile = Path.Combine(directory, $"{fileName}.{i + 1}{fileExtension}");

                    if (File.Exists(currentFile))
                    {
                        File.Move(currentFile, nextFile);
                    }
                }

                var firstBackup = Path.Combine(directory, $"{fileName}.1{fileExtension}");
                if (File.Exists(this.FilePath))
                {
                    File.Move(this.FilePath, firstBackup);
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }

        private bool ShouldRotateFile()
        {
            if (!File.Exists(this.FilePath))
            {
                return false;
            }

            var fileInfo = new FileInfo(this.FilePath);
            return fileInfo.Length >= _maxFileSize;
        }
    }
}