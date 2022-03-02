using RodrigoQuestions.Hackerhank.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RodrigoQuestions.Hackerhank
{
    /// <summary>
    /// 2D Array-Ds
    /// </summary>
    /// <paramref name="https://www.hackerrank.com/challenges/2d-array/problem?isFullScreen=true&h_l=interview&playlist_slugs%5B%5D=interview-preparation-kit&playlist_slugs%5B%5D=arrays"/>
    public class ArrayDs : IArrayDs
    {
        public void ExecutaMetodo()
        {
            var list = new List<List<int>>();
            var rnd = new Random();
            for (var i = 0; i < 6; i++)
            {
                list.Add(new List<int>
                    {
                        rnd.Next(-9, 9),
                        rnd.Next(-9, 9),
                        rnd.Next(-9, 9),
                        rnd.Next(-9, 9),
                        rnd.Next(-9, 9),
                        rnd.Next(-9, 9)
                    }
                );
            }
            
            Console.WriteLine(HourglassSum(list));
        }

        public int HourglassSum(List<List<int>> arr)
        {
            Dictionary<int, int> dictionary;
            var biggestSum = int.MinValue;
            var possibleArr = arr.Count - 2;
            for (var i = 0; i < possibleArr; i++)
            {
                for (var j = 0; j < possibleArr; j++)
                {
                    var index = 0;
                    dictionary = arr[i].GetRange(j, 3)
                        .Concat(arr[i + 1].GetRange(j + 1, 1))
                        .Concat(arr[i + 2].GetRange(j, 3))
                        .ToDictionary(i1 => index++);

                    var somaHourgalss = dictionary.Sum(d => d.Value);
                    if (somaHourgalss > biggestSum)
                        biggestSum = somaHourgalss;
                }
            }
            return biggestSum;
        }
    }
}
