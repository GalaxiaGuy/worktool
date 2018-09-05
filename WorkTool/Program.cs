using System;

namespace WorkTool
{
    class Program
    {
        static void Main(string[] args)
        {
            var gitTool = new GitTool(args?[0] ?? ".");

            var lines = gitTool.Execute();

            var dateCombiner = new DateCombiner();
            var result = dateCombiner.Process(lines);
        }
    }
}
