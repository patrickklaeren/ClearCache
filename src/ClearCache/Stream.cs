using System;
using System.Collections.Generic;
using System.Text;

namespace ClearCache
{
    public class Stream
    {

        public static void Write(string message) => Console.WriteLine(message);
        public static void WaitOnKey() => Console.ReadKey();

        public static string Read(string message)
        {
            Write(message);
            return Console.ReadLine();
        }

        public static void Close() => Environment.Exit(0);
    }
}
