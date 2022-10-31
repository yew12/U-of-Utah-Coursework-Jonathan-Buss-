using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileLogger
{
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
