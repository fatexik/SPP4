using System;
using System.Collections.Generic;
using TestGenerator;

namespace Console
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            int readingTaskCount = Convert.ToInt32(args[0]);
            int writeTaskCount = Convert.ToInt32(args[1]);
            string outpDirectory = args[2];
            List<string> paths = new List<string>();
            for (int i = 3; i < args.Length; i++)
            {
                paths.Add(args[i]);
            }
            Reader reader = new Reader();
            Writer writer = new Writer(outpDirectory);
            TestGeneratorClass.TestGeneratorClass testGeneratorClass = new TestGeneratorClass.TestGeneratorClass(outpDirectory);

            testGeneratorClass.generate(readingTaskCount, writeTaskCount, paths).Wait();
            
            System.Console.WriteLine("Done !");
            System.Console.ReadLine();
        }        
    }
}