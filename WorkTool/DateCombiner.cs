using System;
using System.Collections.Generic;
using System.Linq;

namespace WorkTool
{
    public class DateCombiner
    {
        public ResultSet Process(IEnumerable<string> strings)
        {
            var dates = strings.Select(x => x.Trim()).Where(x => !string.IsNullOrWhiteSpace(x)).Select(DateTimeOffset.Parse).OrderBy(x => x);

            return new ResultSet
            {
                Results = new List<Result>
                {
                    Process(dates, TimeSpan.FromMinutes(1)),
                    Process(dates, TimeSpan.FromMinutes(15)),
                    Process(dates, TimeSpan.FromMinutes(60)),
                    Process(dates, TimeSpan.FromDays(1))
                }
            };
        }

        private Result Process(IEnumerable<DateTimeOffset> dates, TimeSpan timePerCommit)
        {
            var ranges = dates.Select(x => new DateRange { Start = x - timePerCommit, End = x }).ToList();

            for (var i = ranges.Count - 1; i >= 1; i--)
            {
                var latter = ranges[i];
                var earlier = ranges[i - 1];
                if (latter.Overlaps(earlier))
                {
                    latter.Combine(earlier);
                    ranges.Remove(earlier);
                }
            }

            return new Result { TimePerCommit = timePerCommit, Ranges = ranges };
        }
    }

    public class ResultSet
    {
        public List<Result> Results { get; set; }
    }

    public class Result
    {
        public TimeSpan TimePerCommit { get; set; }
        public List<DateRange> Ranges { get; set; }
        public TimeSpan Duration => Ranges.Aggregate(TimeSpan.Zero, (s, x) => s += x.Duration);
    }

    public class DateRange
    {
        public TimeSpan Duration => End - Start;
        public DateTimeOffset Start { get; set; }
        public DateTimeOffset End { get; set; }

        public bool Overlaps(DateRange other)
        {
            if (other.End > End)
            {
                throw new ArgumentException(nameof(other));
            }
            return other.End > Start;
        }

        public void Combine(DateRange other)
        {
            if (!Overlaps(other))
            {
                throw new ArgumentException(nameof(other));
            }
            Start = other.Start;
        }
    }
}