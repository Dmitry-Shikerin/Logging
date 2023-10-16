using System;

namespace Логирование
{
    internal class Program
    {
        static void Main(string[] args)
        {
            DayOfWeek dayOfWeek = DayOfWeek.Friday;

            ILogger fileLogWriter = new FileLogWritter();
            ILogger consoleLogWriter = new ConsoleLogWritter();
            ILogger dayOfWeekFileLogWriter = new DayOfWeekLogWriter(fileLogWriter, dayOfWeek);
            ILogger dayOfWeekConsoleLogWriter = new DayOfWeekLogWriter(consoleLogWriter, dayOfWeek);
            ILogger compositeLogWriter = new CompositeLogWritter(consoleLogWriter, dayOfWeekFileLogWriter);

            ILogger[] loggers = new ILogger[] 
            {
                fileLogWriter,
                consoleLogWriter,
                dayOfWeekConsoleLogWriter,
                dayOfWeekFileLogWriter,
                compositeLogWriter,
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

    class DayOfWeekLogWriter : ILogger
    {
        private readonly ILogger _logger;
        private readonly DayOfWeek _dayOfWeek;

        public DayOfWeekLogWriter(ILogger logger, DayOfWeek dayOfWeek)
        {
            _logger = logger;
            _dayOfWeek = dayOfWeek;
        }

        public void WriteError(string message)
        {
            if (DateTime.Now.DayOfWeek == _dayOfWeek)
            {
                _logger.WriteError(message);
            }
        }
    }

    class CompositeLogWritter : ILogger
    {
        private readonly ILogger[] _loggers;

        public CompositeLogWritter(params ILogger[] loggers)
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

