using System;
using System.Collections.Generic;
using TestGenerator;
using TestGenerator = TestGenerator.TestGenerator;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            int readingTaskCount = Convert.ToInt32(args[0]);
            int writeTaskCount = Convert.ToInt32(args[1]);
            string outpDirectory = args[2];
            List<string> paths = new List<string>();
            for (int i = 3; i < args.Length; i++)
            {
                paths.Add(args[i]);
            }
            TestGenerator.TestGenerator testGenerator = new TestGenerator(outpDirectory);
            testGenerator.
        }
    }
}