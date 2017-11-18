using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ClearCache
{
    public static class Extensions
    {
        public static string GoUpDirectory(this string directorySource)
            => Path.GetFullPath(Path.Combine(directorySource, @"..\"));
    }
}
