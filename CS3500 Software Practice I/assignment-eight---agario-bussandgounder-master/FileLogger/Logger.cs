using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileLogger
{
    /// <summary>
    /// Logger class to essentially set up our logger to be used in our GUI classes
    /// </summary>
    internal class CustomFileLogger : ILogger
    {
        private readonly string _categoryName;
        private readonly string _fileName;

        /// <summary>
        /// Constructor for our logger
        /// </summary>
        /// <param name="categoryName"></param>
        public CustomFileLogger(string categoryName)
        {
            _categoryName = categoryName;
            _fileName = "LOGFILECS3500-A8";
        }

        /// <summary>
        /// Not used
        /// </summary>
        /// <typeparam name="TState"></typeparam>
        /// <param name="state"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public IDisposable BeginScope<TState>(TState state)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Not used
        /// </summary>
        /// <param name="logLevel"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public bool IsEnabled(LogLevel logLevel)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Sets up our logger to display the date and time
        /// </summary>
        /// <typeparam name="TState"></typeparam>
        /// <param name="logLevel"></param>
        /// <param name="eventId"></param>
        /// <param name="state"></param>
        /// <param name="exception"></param>
        /// <param name="formatter"></param>
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            string result = DateTime.Now.ToString() + $": {logLevel}: " + formatter(state, exception) + Environment.NewLine;
            File.AppendAllText(_fileName, result);
        }
    }



    /// <summary>
    /// Class creates our CustomFileLogProvider
    /// </summary>
    public class CustomFileLogProvider : ILoggerProvider
    {
        /// <summary>
        /// Method we care about 
        /// </summary>
        public ILogger CreateLogger(string categoryName)
        {
            return new CustomFileLogger(categoryName);
        }

        /// <summary>
        /// not used
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
