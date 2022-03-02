using Microsoft.VisualStudio.TestTools.UnitTesting;
using RodrigoQuestions.Hackerhank;
using System.Collections.Generic;
using Moq;
using RodrigoQuestions.Interfaces;

namespace RodrigoQuestionsTests.Hackerhank
{
    [TestClass()]
    public class ArrayDsTests
    {
        [TestMethod()]
        public void ProcessClient_Test()
        {
            //arrange
            var arrayDs = new ArrayDs();

            var list = new List<List<int>>
            {
                new() { 0, -4, -6, 0, -7, -6 },
                new() { -1, -2, -6, -8, -3, -1 },
                new() { -8, -4, -2, -8, -8, -6 },
                new() { -3, -1, -2, -5, -7, -4 },
                new() { -3, -5, -3, -6, -6, -6 },
                new() { -3, -6, 0, -8, -6, -7 }
            };

            //act
            var result = arrayDs.HourglassSum(list);

            //assert
            Assert.AreEqual(-19, result);
        }
    }
}