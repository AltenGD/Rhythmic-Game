using System;
using SharpCompress.Archives.Zip;

namespace Rhythmic.Tools
{
    public static class ZipUtils
    {
        public static bool IsZipArchive(string path)
        {
            try
            {
                using (var arc = ZipArchive.Open(path))
                {
                    foreach (var entry in arc.Entries)
                    {
                        using (entry.OpenEntryStream())
                        {
                        }
                    }
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
