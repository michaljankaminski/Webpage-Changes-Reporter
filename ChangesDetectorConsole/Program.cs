using System;
using ChangesDetector;

namespace ChangesDetectorConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Manager mn = new Manager();
            mn.Test();
            Console.WriteLine("Finished");
        }
    }
}
