using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClearCache
{
    public static class Mediator
    {
        private const string FORCE = "-force";

        public static IEnumerable<string> Arguments { get; private set; }

        public static bool HasArguments => Arguments.Any();

        public static bool IsForcedExecution => Arguments.Any() && Arguments.Any(q => q.ToArgumentFormat() == FORCE);

        public static void MemoriseStartupArguments(string[] args) => Arguments = args;

        private static string ToArgumentFormat(this string source) => source.ToLower();
    }
}
