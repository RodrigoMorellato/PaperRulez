using Microsoft.VisualStudio.TestTools.UnitTesting;
using RodrigoQuestions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using RodrigoQuestions.Interfaces;

namespace RodrigoQuestionsTests
{
    [TestClass]
    public class PaperRulezTests
    {
        private string baseClientsFolder;
        private string client;
        private string filename;

        [TestInitialize]
        public void TestInitialize()
        {
            baseClientsFolder = Path.Combine(Path.GetTempPath(), "PaperRulezTests");
            client = "123456";
            filename = "764-OO3-3351_Meeting_minutes.text";

            var clientFolder = Path.Combine(baseClientsFolder, client);
            if (!Directory.Exists(clientFolder))
                Directory.CreateDirectory(clientFolder);
            else
            {
                foreach (var f in Directory.GetFiles(clientFolder))
                    File.Delete(f);
            }

            var filePath = Path.Combine(clientFolder, filename);
            File.WriteAllText(filePath, Properties.Resources.text_sample_file);
        }

        [TestMethod]
        public async Task ProcessClients_Test()
        {
            //Arrange
            var lookupStore = new PaperRulezLookupStoreMockup(client, "764-OO3-3351", "Cat", "box");
            var paperRulez = new PaperRulez(baseClientsFolder, lookupStore);

            //Act
            await paperRulez.ProcessClients(new List<string> { client });
        }

        [TestMethod()]
        public void ProcessText_WithoutParam_BaseClientsFolder_Test()
        {
            //Arrange
            var lookupStore = new PaperRulezLookupStoreMockup(client, "764-OO3-3351", "Cat", "box");

            //Act
            try
            {
                var paperRulez = new PaperRulez("", lookupStore);
                Assert.Fail();
            }
            catch (DirectoryNotFoundException)
            {
            }
            catch
            {
                Assert.Fail();
            }
        }

        [TestMethod()]
        public async Task ProcessClient_WithoutClient_Test()
        {
            try
            {
                //Arrange
                var lookupStore = new PaperRulezLookupStoreMockup(client, "764-OO3-3351", "Cat", "box");
                var paperRulez = new PaperRulez(baseClientsFolder, lookupStore);

                //Act
                await paperRulez.ProcessClient("");
                Assert.Fail();
            }
            catch (InvalidOperationException)
            {
            }
            catch
            {
                Assert.Fail();
            }
        }

        [TestMethod()]
        public async Task ProcessClient_ClientFolderNotExist_Test()
        {
            try
            {
                //Arrange
                var lookupStore = new PaperRulezLookupStoreMockup(client, "764-OO3-3351", "Cat", "box");
                var paperRulez = new PaperRulez(baseClientsFolder, lookupStore);

                //Act
                await paperRulez.ProcessClient("99999");
            }
            catch
            {
                Assert.Fail();
            }
        }
    }

    public class PaperRulezLookupStoreMockup : ILookupStore
    {
        private readonly string _expectedClient;
        private readonly string _expectedDocumentId;
        private readonly string[] _expectedKeywords;

        public PaperRulezLookupStoreMockup(string expectedClient, string expectedDocumentId, params string[] expectedKeywords)
        {
            _expectedClient = expectedClient;
            _expectedDocumentId = expectedDocumentId;
            _expectedKeywords = expectedKeywords;
        }

        public void Record(string client, string documentId, IEnumerable<string> keywords)
        {
            Assert.AreEqual(_expectedClient, client);
            Assert.AreEqual(_expectedDocumentId, documentId);
            CollectionAssert.AreEqual(_expectedKeywords, keywords.ToArray());
        }
    }
}
