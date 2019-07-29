using SharpCompress.Archives.Zip;
using System;

namespace Rhythmic.Tools
{
    public static class ZipUtils
    {
        public static bool IsZipArchive(string path)
        {
            try
            {
                using (ZipArchive arc = ZipArchive.Open(path))
                {
                    foreach (ZipArchiveEntry entry in arc.Entries)
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
