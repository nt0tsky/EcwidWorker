using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using EcwidIntegration.Common.Interfaces;

namespace EcwidIntegration.Common.Services
{
    internal class AssemblyListener : IStartable
    {
        #region Properties

        private string workingDirectory;

        private void InitWorkingDirectory()
        {
            workingDirectory = Directory.GetCurrentDirectory();
        }

        public string WorkingDirectory
        {
            get
            {
                if (string.IsNullOrEmpty(workingDirectory))
                {
                    InitWorkingDirectory();
                }

                return workingDirectory;
            }
        }

        /// <summary>
        /// Включить отслеживание рабочей директории
        /// </summary>
        public bool EnableWatcher { get; set; }

        /// <summary>
        /// Вызывается при изменении в директории
        /// Актуален в случае выставлении флага <see cref="EnableWatcher"/>
        /// </summary>
        public Action<string> OnChange { get; set; }

        /// <summary>
        /// Сервис работы с файлами системы
        /// </summary>
        public FileSystemWatcher FileSystemWatcher { get; private set; }

        #endregion

        private void OnChanged(object source, FileSystemEventArgs e)
        {
            OnChange?.Invoke(e.FullPath);
        }

        private void Initialize()
        {
            if (EnableWatcher)
            {
                FileSystemWatcher = new FileSystemWatcher(WorkingDirectory, "*.dll");
                FileSystemWatcher.NotifyFilter = NotifyFilters.LastAccess
                                                 | NotifyFilters.LastWrite
                                                 | NotifyFilters.FileName
                                                 | NotifyFilters.DirectoryName;

                if (OnChange != null)
                {
                    // Add event handlers.
                    FileSystemWatcher.Changed += OnChanged;
                    FileSystemWatcher.Created += OnChanged;
                    FileSystemWatcher.Deleted += OnChanged;
                    FileSystemWatcher.Renamed += OnChanged;
                }

                // Begin watching.
                FileSystemWatcher.EnableRaisingEvents = true;
            }
        }

        /// <summary>
        /// Получить список сборок из рабочей директории
        /// </summary>
        /// <returns>Список сборок</returns>
        public IEnumerable<Assembly> GetAssemblies()
        {
            return Directory.GetFiles(WorkingDirectory, "*.dll").Select(p => Assembly.LoadFrom(p));
        }

        public void Start()
        {
            this.Initialize();
        }
    }
}
