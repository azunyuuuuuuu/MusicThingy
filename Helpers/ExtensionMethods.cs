using System.IO;

namespace MusicThingy.Helpers
{
    public static class ExtensionMethods
    {
        public static string GetSafeFilename(this string filename)
        {
            string invalidChars = System.Text.RegularExpressions.Regex.Escape(new string(System.IO.Path.GetInvalidFileNameChars())+"~");
            string invalidRegStr = string.Format(@"([{0}]*\.+$)|([{0}]+)", invalidChars);

            return System.Text.RegularExpressions.Regex.Replace(filename, invalidRegStr, "_");
            // var invalidchars = Path.GetInvalidFileNameChars();
            // return string.Join("_", filename.Split(invalidchars));
        }

        public static string GetSafePath(this string filename)
        {
            return string.Join("_", filename.Split(Path.GetInvalidPathChars()));
        }
    }
}