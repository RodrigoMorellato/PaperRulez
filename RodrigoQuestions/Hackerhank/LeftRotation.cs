using System;
using System.Collections.Generic;
using System.Linq;

namespace RodrigoQuestions.Hackerhank
{
    /// <summary>
    /// LeftRotation
    /// </summary>
    /// <paramref name="https://www.hackerrank.com/challenges/ctci-array-left-rotation/problem?isFullScreen=true&h_l=interview&playlist_slugs%5B%5D=interview-preparation-kit&playlist_slugs%5B%5D=arrays&h_r=next-challenge&h_v=zen"/>
    public class LeftRotation
    {
        public void ExecutaMetodo()
        {
            var list = new List<int> { 1, 2, 3, 4, 5, 6 };
            Console.WriteLine(RotLeft(list, 4));
        }

        public List<int> RotLeft(List<int> a, int d)
        {
            var queue = new Queue<int>(a);
            while (d-- > 0)
            {
                var rotated = queue.Dequeue();
                queue.Enqueue(rotated);

                Console.WriteLine(rotated);
            }

            return queue.ToList();
        }
    }
}
