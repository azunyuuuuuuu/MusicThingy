using System;
using System.IO;

namespace MusicThingy
{
    public static class ExtensionMethods
    {
        public static string GetSafeFilename(this string filename)
        {
            return string.Join("_", filename.Split(Path.GetInvalidFileNameChars()));
        }
    }
}