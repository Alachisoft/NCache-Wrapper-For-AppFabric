using Alachisoft.NCache.Data.Caching;
using System;

namespace NCacheAppFabricConsoleUI
{
    internal static class Logger
    {
        internal static void PrintTestStartInformation(string message)
        {
            var color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"******{message}******");
            Console.ForegroundColor = color;
            Console.Write("\n\n");

        }
        internal static void PrintDataCacheException(Exception e)
        {
            var ex = e as DataCacheException;
            var color = Console.ForegroundColor;
            if (ex != null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"EXCEPTION");
                Console.WriteLine($"DataCache exception encountered:");
                Console.WriteLine($"ErrorCode:{ex.ErrorCode}\tDescription:{ErrorCodeLookup.Description(ex.ErrorCode)}");
                Console.WriteLine($"Substatus:{ex.SubStatus}");
                Console.Write("\n\n");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine($"EXCEPTION");
                Console.WriteLine($"General exception encountered:");
                Console.WriteLine($"Exception Type:{e.GetType().Name}");
                Console.WriteLine($"ErrorMessage:{e.Message}");
                Console.Write("\n\n");
            }

            Console.ForegroundColor = color;
        }
        internal static void PrintSuccessfulOutcome(string message)
        {
            var color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"SUCCESS: {message}");
            Console.ForegroundColor = color;
            Console.Write("\n\n");
        }

        internal static void PrintFailureOutcome(string message)
        {
            var color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine($"FAILURE: {message}");
            Console.ForegroundColor = color;
            Console.Write("\n\n");
        }

        internal static void PrintBreakLine()
        {
            var color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"*************************");
            Console.ForegroundColor = color;
            Console.Write("\n\n");
        }

        internal static void WriteHeaderLine(string message)
        {
            var color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write($"++{message}++");
            Console.ForegroundColor = color;
            Console.Write("\n\n");
        }

        internal static void WriteFooterLine(string message)
        {
            var color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write($"--{message}--");
            Console.ForegroundColor = color;
            Console.Write("\n\n\n\n");
        }
    }
}
