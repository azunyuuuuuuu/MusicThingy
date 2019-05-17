using System.IO;

namespace MusicThingy.Models
{
    public class Configuration
    {
        public string DatabasePath { get; set; }
        public string DataStoragePath { get; set; }
        public string DataPath { get; set; } = $"{System.Environment.GetFolderPath(System.Environment.SpecialFolder.UserProfile)}/.config/musicthingy/";
        public string SourcesPath { get { return Path.Combine(DataPath, "sources"); } }
    }
}
