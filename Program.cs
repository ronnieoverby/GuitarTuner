using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace GuitarTuner
{
    public static class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Use array keys to change strings.\r\n");

            var guitar = new GuitarDisplay(Console.CursorLeft, Console.CursorTop)
            {
                HotString = 6
            };
            var frequencies = new[] { 84, 110, 147, 196, 247, 330 }; // approx :(

            while (true)
            {
                var freq = frequencies[6 - guitar.HotString];
                var thread = new Thread(() => Console.Beep(freq, int.MaxValue));
                thread.Start();

                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.UpArrow:
                    case ConsoleKey.LeftArrow:
                        guitar.HotString++;
                        break;
                    case ConsoleKey.RightArrow:
                    case ConsoleKey.DownArrow:
                    default:
                        guitar.HotString--;
                        break;
                }

                thread.Abort();
            }

        }

    }

    class GuitarDisplay
    {
        private readonly int _left;
        private readonly int _top;

        private int _hotString = 0;

        public int HotString
        {
            get { return _hotString; }
            set
            {
                if (value >= 1 && value <= 6)
                {
                    _hotString = value;
                    ReDraw();

                }
            }
        }

        public GuitarDisplay(int cursorLeft, int cursorTop)
        {
            _left = cursorLeft;
            _top = cursorTop;

            Draw();
        }

        private void ReDraw()
        {
            var top = Console.CursorTop;
            var left = Console.CursorLeft;
            Console.SetCursorPosition(_left, _top);
            Draw();
            Console.SetCursorPosition(left, top);
        }

        private void Draw()
        {
            const string guitar = @"  ╔═╤═╤═╤═╤═╤═╤═╗
  ║ │ │ │ │ │ │ ║
  ║─┼─┼─┼─┼─┼─┼─║
  ║ │ │ │ │ │ │ ║
  ║─┼─┼─┼─┼─┼─┼─║
  ║ │ │ │*│ │ │ ║
  ║─┼─┼─┼─┼─┼─┼─║
  ║ │ │ │ │ │ │ ║
  ║─┼─┼─┼─┼─┼─┼─║
  ║ │ │ │*│ │ │ ║
  ║─┼─┼─┼─┼─┼─┼─║
  ║ │ │ │ │ │ │ ║
  ║─┼─┼─┼─┼─┼─┼─║
  ║ │ │ │*│ │ │ ║
  ║─┼─┼─┼─┼─┼─┼─║
  ║ │ │ │ │ │ │ ║
  ║─┼─┼─┼─┼─┼─┼─║
  ║ │ │ │*│ │ │ ║
  ║─┼─┼─┼─┼─┼─┼─║
  ║ │ │ │ │ │ │ ║
  ║─┼─┼─┼─┼─┼─┼─║
  ║ │*│ │ │ │*│ ║
  ║─┼─┼─┼─┼─┼─┼─║
  ║ │ │ │ │ │ │ ║";

            var dict = new Dictionary<int, int>
            {
                [6] = 5,
                [5] = 7,
                [4] = 9,
                [3] = 11,
                [2] = 13,
                [1] = 15,
            };

            int h;
            if (!dict.TryGetValue(HotString, out h))
                h = -1;

            using (var reader = new StringReader(guitar))
            {
                foreach (var line in ReadLines(reader))
                {
                    var n = 0;
                    foreach (var c in line)
                    {
                        n++;

                        var color = Console.ForegroundColor;
                        if (n == h)
                            Console.ForegroundColor = ConsoleColor.Red;

                        Console.Write(c);
                        Console.ForegroundColor = color;

                    }

                    Console.WriteLine();
                }
            }
        }

        IEnumerable<string> ReadLines(TextReader reader)
        {
            string line;
            while ((line = reader.ReadLine()) != null)
                yield return line;
        }
    }
}
