using System;
using System.IO;
using System.Linq;

namespace ClearCache
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Mediator.MemoriseStartupArguments(args);
            Stream.Write(ConsoleDump.CONSOLE_START);

            if(Mediator.HasArguments)
                Stream.Write("Started with arguments: " + string.Join(" ", Mediator.Arguments));

            var workingDirectory = Directory.GetCurrentDirectory().GoUpDirectory();
            var filesInWorkingDirectory = Directory.GetFiles(workingDirectory);

            if (filesInWorkingDirectory.Any(x => x.EndsWith(Constants.SOLUTION_EXTENSION)) == false)
            {
                WarnOfInvalidWorkingDirectory(workingDirectory);
            }
            else
            {
                StartNuke(workingDirectory);
            }

            Stream.Write(ConsoleDump.SPLITTER);
            Stream.Write("Press any key to exit");
            Stream.WaitOnKey();
        }

        private static void WarnOfInvalidWorkingDirectory(string workingDirectory)
        {
            // The user has not launched in a working directory where a solution
            // file can be found
            Stream.Write("ERROR The working directory for the app (" + workingDirectory + ") does not contain a " +
                         "valid Visual Studio .SLN file, cannot proceed. Launch this app from the root of your solution directory. " +
                         "For detailed instructions, visit the GitHub project's page.");
        }

        private static void StartNuke(string directory)
        {
            if (Mediator.IsForcedExecution)
            {
                // TODO Implement forced execution to force close Visual Studio instances that match SLN
            }

            // Get the root folders in the solution's root directory
            try
            {
                var foldersInDirectory = Directory.GetDirectories(directory);

                foreach (var folder in foldersInDirectory)
                {
                    // Get the sub directories of the project we're presumably
                    // in

                    var projectDirectories = Directory.GetDirectories(folder);

                    var binPath = Path.Combine(folder, Constants.BIN_FOLDER);

                    if (projectDirectories.Any(path => path == binPath) == false)
                        continue;

                    Stream.Write("Starting clean of " + folder);

                    Directory.Delete(binPath, true);

                    Stream.Write("Removed bin " + binPath);
                }
            }
            catch (Exception e)
            {
                Stream.Write("ERROR Something went wrong while trying to remove all bin directories for your solution! " +
                             e.Message);

                if (Mediator.IsForcedExecution)
                {
                    Stream.Write("Visual Studio was not properly closed, if you rerun, ClearCache will attempt to re-force execution");
                }
                else
                {
                    Stream.Write("Ensure you have closed Visual Studio before rerunning!");
                }

                if (ContinueAfterError() == false)
                    return;

                StartNuke(directory);
            }
        }

        private static bool ContinueAfterError()
        {
            const string CONTINUE = "C";
            const string EXIT = "E";

            var input = Stream.Read("Type 'C' to continue or 'E' to exit, alternatively press enter to step out").ToUpper();

            if (input == EXIT)
                Stream.Close();

            return input == CONTINUE;
        }
    }
}
