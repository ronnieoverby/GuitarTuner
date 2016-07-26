using System;
using System.Threading;
using System.Threading.Tasks;

namespace GuitarTuner
{
    public static class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Press any key to advance to the next string.\r\n");

            var guitar = new GuitarDisplay(Console.CursorLeft, Console.CursorTop);
            var frequencies = new[] { 84, 110, 147, 196, 247, 330 }; // approx :(

            for (guitar.HotString = 6; guitar.HotString >= 1; guitar.HotString--)
            {
                using (var stop = new ManualResetEventSlim())
                {
                    var freq = frequencies[6 - guitar.HotString];
                    Pluck(freq, stop);
                    Console.ReadKey();
                    stop.Set();
                }
            }

            Console.ReadKey();
        }

        private static Task Pluck(int freq, ManualResetEventSlim stop)
        {
            return Task.Run(() =>
            {
                while (!stop.Wait(0))
                {
                    Console.Beep(freq, 1000);
                }
            });
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
                _hotString = value;
                ReDraw();
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
            const string indent = "  ";
            const int strings = 6;
            Console.WriteLine(indent + "EADGBe");
            Console.WriteLine(indent + new string('=', strings));
            for (int fret = 0; fret < 5; fret++)
            {
                Console.Write(indent);
                for (int i = 6; i > 0; i--)
                {
                    var color = Console.ForegroundColor;

                    if (HotString == i)
                        Console.ForegroundColor = ConsoleColor.Red;

                    Console.Write("|");
                    Console.ForegroundColor = color;

                }
                Console.WriteLine();
            }
        }
    }
}
