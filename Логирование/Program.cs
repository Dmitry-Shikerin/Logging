using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Логирование
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // подумать над названиями
            ILogger[] loggers = new ILogger[] 
            {
                new FileLogWritter(),
                new ConsoleLogWritter(),
                new DefiniteLogWriter(new ConsoleLogWritter()),
                new DefiniteLogWriter(new FileLogWritter()),
                new DefiniteConsoleLogWritter(new ConsoleLogWritter(), new DefiniteLogWriter(new FileLogWritter())),
            };

            foreach (ILogger logger in loggers)
            {
                new Pathfinder(logger).Find();
            }
        }
    }

    interface ILogger
    {
        void WriteError(string message);
    }

    class Pathfinder
    {
        private readonly ILogger _logger;

        public Pathfinder(ILogger logger)
        {
            _logger = logger;
        }

        public void Find()
        {
            _logger.WriteError("пишу в лог");
        }
    }

    class ConsoleLogWritter : ILogger
    {
        public void WriteError(string message)
        {
            Console.WriteLine(message);
        }
    }

    class FileLogWritter : ILogger         
    {
        public void WriteError(string message)
        {
            File.WriteAllText("log.txt", message);
        }
    }

    class DefiniteLogWriter : ILogger
    {
        private readonly ILogger _logger;

        public DefiniteLogWriter(ILogger logger)
        {
            _logger = logger;
        }

        public void WriteError(string message)
        {
            if (DateTime.Now.DayOfWeek == DayOfWeek.Friday)
            {
                _logger.WriteError(message);
            }
        }
    }

    class DefiniteConsoleLogWritter : ILogger
    {
        private readonly ILogger[] _loggers;

        public DefiniteConsoleLogWritter(params ILogger[] loggers)
        {
            _loggers = loggers;
        }

        public void WriteError(string message)
        {
            foreach (ILogger logger in _loggers) 
            { 
                logger.WriteError(message);
            }
        }
    }
    static class File
    {
        public static void WriteAllText(string file, string message)
        {
            Console.WriteLine($"{file}/{message}");
        }
    }
}

