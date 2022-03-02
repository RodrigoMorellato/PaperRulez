using Microsoft.VisualStudio.TestTools.UnitTesting;
using RodrigoQuestions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RodrigoQuestionsTests.OneToManyMapperTests
{
    [TestClass]
    public class OneToManyMapperTests
    {
        [TestMethod]
        public void AddAndGet()
        {
            var mapper = new OneToManyMapper();

            mapper.Add(10, 100);
            mapper.Add(11, 110);
            mapper.Add(10, 101);

            var parent = mapper.GetParent(100);
            Assert.AreEqual(10, parent);
            parent = mapper.GetParent(110);
            Assert.AreEqual(11, parent);
            parent = mapper.GetParent(101);
            Assert.AreEqual(10, parent);
        }

        [TestMethod]
        public void AddDuplicated()
        {
            var mapper = new OneToManyMapper();

            mapper.Add(10, 100);
            mapper.Add(10, 101);

            try
            {
                mapper.Add(10, 100); // Add a child to the same parent a second time.
                Assert.Fail();
            }
            catch (ArgumentException)
            {
            }
            catch
            {
                Assert.Fail();
            }

            try
            {
                mapper.Add(11, 100); // Add a child to another parent.
                Assert.Fail();
            }
            catch (ArgumentException)
            {
            }
            catch
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void RemoveParent()
        {
            var mapper = new OneToManyMapper();

            mapper.Add(10, 100);
            mapper.Add(11, 110);
            mapper.Add(10, 101);

            mapper.RemoveParent(10);

            try
            {
                mapper.GetChildren(10);
                Assert.Fail();
            }
            catch (KeyNotFoundException)
            {
            }
            catch
            {
                Assert.Fail();
            }

            try
            {
                mapper.GetParent(100); // Get child of a removed parent.
                Assert.Fail();
            }
            catch (KeyNotFoundException)
            {
            }
            catch
            {
                Assert.Fail();
            }

            try
            {
                mapper.RemoveParent(10); // Removes parent a second time.
                Assert.Fail();
            }
            catch (KeyNotFoundException)
            {
            }
            catch
            {
                Assert.Fail();
            }

            mapper.GetChildren(11); // Get a non removed parent to check consistency.
        }

        [TestMethod]
        public void RemoveChildren()
        {
            var mapper = new OneToManyMapper();

            mapper.Add(10, 100);
            mapper.Add(11, 110);
            mapper.Add(10, 101);

            mapper.RemoveChild(100);
            Assert.IsFalse(mapper.GetChildren(10).Any(c => c == 100));
        }

        [TestMethod]
        public void UpdateChild()
        {
            var mapper = new OneToManyMapper();

            mapper.Add(10, 100);
            mapper.Add(11, 110);
            mapper.Add(10, 101);

            mapper.UpdateChild(100, 102);

            var parent = mapper.GetParent(102);
            Assert.AreEqual(10, parent);

            try
            {
                mapper.GetParent(100);
                Assert.Fail();
            }
            catch (KeyNotFoundException)
            {
            }
            catch
            {
                Assert.Fail();
            }

            try
            {
                mapper.UpdateChild(110, 102); // Update a child to a duplicated value.
                Assert.Fail();
            }
            catch (ArgumentException)
            {
            }
            catch
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void UpdateParent()
        {
            var mapper = new OneToManyMapper();

            mapper.Add(10, 100);
            mapper.Add(11, 110);
            mapper.Add(10, 101);

            var originalChildren = mapper.GetChildren(10);

            mapper.UpdateParent(10, 12);

            var currentChildren = mapper.GetChildren(12);
            Assert.AreSame(originalChildren, currentChildren);

            try
            {
                mapper.GetChildren(10);
                Assert.Fail();
            }
            catch (KeyNotFoundException)
            {
            }
            catch
            {
                Assert.Fail();
            }

            try
            {
                mapper.UpdateParent(11, 12); // Update a parent to a duplicated value.
                Assert.Fail();
            }
            catch (ArgumentException)
            {
            }
            catch
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void InvalidRangeValues()
        {
            var mapper = new OneToManyMapper();

            try
            {
                mapper.Add(Int32.MaxValue, 100);
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException)
            {
            }
            catch
            {
                Assert.Fail();
            }

            try
            {
                mapper.Add(10, Int32.MaxValue);
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException)
            {
            }
            catch
            {
                Assert.Fail();
            }

            try
            {
                mapper.Add(0, 100);
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException)
            {
            }
            catch
            {
                Assert.Fail();
            }

            try
            {
                mapper.Add(10, 0);
                Assert.Fail();
            }
            catch (ArgumentOutOfRangeException)
            {
            }
            catch
            {
                Assert.Fail();
            }
        }
    }
}
