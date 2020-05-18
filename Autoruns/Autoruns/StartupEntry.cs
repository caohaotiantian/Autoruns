using System;

namespace Autoruns
{
    internal class StartupEntry
    {
        public StartupEntry(bool _isMainEntry,
            string _entryName,
            string _desctiption,
            string _publisher,
            string _imagePath,
            DateTime _time)
        {
            IsMainEntry = _isMainEntry;
            EntryName = _entryName;
            Desctiption = _desctiption;
            Publisher = _publisher;
            ImagePath = _imagePath;
            Time = _time;
            IsEmpty = true;
        }

        public bool IsMainEntry { get; set; }

        public string EntryName { get; set; }

        public string Desctiption { get; set; }

        public string Publisher { get; set; }

        public string ImagePath { get; set; }

        public DateTime Time { get; set; }

        public bool IsEmpty { get; set; }
    }
}