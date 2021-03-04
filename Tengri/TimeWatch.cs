using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Tengri
{
    public class TimeWatch
    {
        public Dictionary<string, long> TimeElapsedData = new Dictionary<string, long>();
        private Stopwatch _stopwatch = new Stopwatch();

        public TimeWatch()
        {
            _stopwatch.Start();
        }

        public void PrintStatistics()
        {
            Console.WriteLine();
            Console.WriteLine("Statistics:");
            foreach (var time in TimeElapsedData)
            {
                Console.WriteLine($"[{time.Key}]: {time.Value}ms");
            }
                
            Console.WriteLine();
            Console.WriteLine("----");
        }

        public void Elapsed(string key)
        {
            _stopwatch.Stop();
            if (TimeElapsedData.ContainsKey(key))
            {
                TimeElapsedData[key] += _stopwatch.ElapsedMilliseconds;
            }
            else
            {
                TimeElapsedData.Add(key, _stopwatch.ElapsedMilliseconds);
            }
            
            _stopwatch.Restart();
        }
    }
}