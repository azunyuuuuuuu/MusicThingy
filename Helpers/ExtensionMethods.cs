using System.IO;

namespace MusicThingy.Helpers
{
    public static class ExtensionMethods
    {
        public static string GetSafeFilename(this string filename)
        {
            return string.Join("_", filename.Split(Path.GetInvalidFileNameChars()));
        }

        public static string GetSafePath(this string filename)
        {
            return string.Join("_", filename.Split(Path.GetInvalidPathChars()));
        }
    }
}