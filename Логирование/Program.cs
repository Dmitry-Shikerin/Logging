using System;

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
                new DayOfWeekLogWriter(new ConsoleLogWritter(), DayOfWeek.Friday),
                new DayOfWeekLogWriter(new FileLogWritter(), DayOfWeek.Friday),
                new CompositeLogWritter(new ConsoleLogWritter(), new DayOfWeekLogWriter(new FileLogWritter(), DayOfWeek.Friday)),
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

