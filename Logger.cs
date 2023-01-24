using System;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

using Discord;

namespace Crusader
{
    public enum LoggerType
    {
        Default,
        Info,
        Warning,
        Error,
        Debug,
    }

    public static class Logger
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static ConsoleColor GetColor(LoggerType type) => type switch 
        { 
            LoggerType.Info => ConsoleColor.Gray, 
            LoggerType.Warning => ConsoleColor.Yellow, 
            LoggerType.Error => ConsoleColor.Red,
            LoggerType.Debug => ConsoleColor.DarkGray,
            _ => ConsoleColor.Gray
        };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static string GetPrefix(LoggerType type) => type switch
        {
            LoggerType.Info => "INFO: ",
            LoggerType.Warning => "WARN: ",
            LoggerType.Error => "ERROR: ",
            LoggerType.Debug => "DEBUG:",
            _ => string.Empty
        };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static LoggerType SeverityToPriority(LogSeverity severity) => severity switch
        {
            LogSeverity.Info => LoggerType.Info,
            LogSeverity.Warning => LoggerType.Warning,
            LogSeverity.Error => LoggerType.Error,
            LogSeverity.Debug => LoggerType.Debug,
            LogSeverity.Critical => LoggerType.Error,
            LogSeverity.Verbose => LoggerType.Debug,
            _ => LoggerType.Info
        };

        /// <summary>
        /// Logs a message to the console.
        /// </summary>
        /// <param name="msg">The string to print.</param>
        /// <param name="type">The message priority.</param>
        /// <returns>The task result.</returns>
        public static Task Log(string msg, LoggerType type)
        {
#if !DEBUG
            if (type == LoggerType.Debug)
                return Task.CompletedTask;
#endif

            Console.ForegroundColor = GetColor(type);
            Console.Write(GetPrefix(type));
            Console.WriteLine(msg);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Logs an info message to the console.
        /// </summary>
        /// <param name="msg">The message to print.</param>
        /// <returns>The task result.</returns>
        public static Task Info(string msg) => Log(msg, LoggerType.Info);

        /// <summary>
        /// Logs an warning message to the console.
        /// </summary>
        /// <param name="msg">The message to print.</param>
        /// <returns>The task result.</returns>
        public static Task Warn(string msg) => Log(msg, LoggerType.Error);

        /// <summary>
        /// Logs an error message to the console.
        /// </summary>
        /// <param name="msg">The message to print.</param>
        /// <returns>The task result.</returns>
        public static Task Error(string msg) => Log(msg, LoggerType.Error);

        /// <summary>
        /// Logs an debug message to the console.
        /// </summary>
        /// <param name="msg">The message to print.</param>
        /// <returns>The task result.</returns>
        public static Task Debug(string msg) => Log(msg, LoggerType.Debug);

        /// <summary>
        /// Logs a message to the console.
        /// </summary>
        /// <param name="msg">The message to print.</param>
        /// <returns>The task result.</returns>
        public static Task Log(LogMessage msg) => Log(msg.ToString(), SeverityToPriority(msg.Severity));
    }
}
