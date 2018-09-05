using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace WorkTool
{
    public class GitTool
    {
        public GitTool(string dir)
        {
            _dir = dir;
        }

        private static readonly string _command = "git";
        private static readonly string _args = "--no-pager log --decorate=short --pretty=oneline --branches=* --format=%cd --date=iso8601";

        private readonly string _dir;

        public List<string> Execute()
        {
            var process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = _command,
                    Arguments = _args,
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    WorkingDirectory = _dir
                }
            };
            process.Start();
            var result = process.StandardOutput.ReadToEnd();
            process.WaitForExit();

            return result.Split(Environment.NewLine).ToList();
        }
    }
}
