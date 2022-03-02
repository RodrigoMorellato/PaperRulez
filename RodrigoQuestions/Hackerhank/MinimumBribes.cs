using System;
using System.Collections.Generic;
using System.Text;

namespace RodrigoQuestions.Hackerhank
{
    /// <summary>
    /// New Year Chaos - Minimum Bribes
    /// </summary>
    /// <paramref name="https://www.hackerrank.com/challenges/new-year-chaos/problem"/>
    internal class MinimumBribes
    {
        public void ExecutaMetodo()
        {
            for (var i = 0; i < 1; i++)
            {
                GetMinimumBribes(new List<int> { 5, 1, 2, 3, 7, 8, 6, 4 });
                GetMinimumBribes(new List<int> { 1, 2, 5, 3, 7, 8, 6, 4 }); 
                //GetMinimumBribes(new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8 });
            }
        }

        private static void GetMinimumBribes(List<int> q)
        {
            int bribes = 0;
            int qCount = q.Count;
            for (var i = 0; i < qCount; i++)
            {
                // If Value of q[i] bribed more than 2 times
                if (q[i] - (i + 1) > 2)
                {
                    Console.WriteLine("Too chaotic");
                    return;
                }

                // Check how many times the value q[i] was bribed.
                for (var b = Math.Max(0, q[i] - 2); b < i; b++)
                    if (q[b] > q[i])
                        bribes++;
            }
            Console.WriteLine(bribes);
        }
    }
}
