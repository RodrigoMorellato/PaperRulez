using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using RodrigoQuestions.Interfaces;

namespace RodrigoQuestions
{
    /// <summary>
    /// PaperRulez processes files based on specific directives and parameters configured within the file 
    /// </summary>
    public sealed class PaperRulez : IPaperRulez
    {
        private readonly string _baseClientsFolder;
        private readonly ILookupStore _lookupStore;

        public PaperRulez(string baseClientsFolder, ILookupStore lookupStore)
        {
            if (!Directory.Exists(baseClientsFolder))
                throw new DirectoryNotFoundException(baseClientsFolder);

            _baseClientsFolder = baseClientsFolder;

            _lookupStore = lookupStore ?? throw new ArgumentNullException("lookupStore");
        }

        /*
            ProcessClients can be implemented as Multi-threaded application.
            However it might be expensive to mantain, test, difficult to code, 
            and it's also prone to deadlock. For example, we'd need to use the 
            "lock" control access in some accessible resources to avoid deadlocks.

            Although in this cenary it's possible, we must take careful to do the 
            implementation, checking the scenery, and resources conditions to do it. 
            Since it might be costly.
        */
        /// <summary>
        /// Method make control under de list clients in processement
        /// </summary>
        /// <param name="clients"></param>
        public async Task ProcessClients(IList<string> clients)
        {
            foreach (var c in clients)
                await ProcessClient(c);
        }

        /// <summary>
        /// Process all 'text' type files in the client directory param
        /// </summary>
        /// <param name="client"></param>
        public async Task ProcessClient(string client)
        {
            if (string.IsNullOrWhiteSpace(client))
                throw new InvalidOperationException("Client not informed.");

            var clientFolder = Path.Combine(_baseClientsFolder, client);
            if (!Directory.Exists(clientFolder))
                return; // Nothing to do.

            var filePaths = Directory.GetFiles(clientFolder, "?*_*.text"); // Limits for 'text' only in the current version.
            foreach (var f in filePaths)
                await ProcessFile(client, f);
        }

        #region Private Methods

        /// <summary>
        /// Process file checking whether extension is accepted
        /// </summary>
        /// <param name="client"></param>
        /// <param name="filePath"></param>
        private async Task ProcessFile(string client, string filePath)
        {
            var filename = Path.GetFileName(filePath);
            var documentId = filename.Split('_')[0];
            var fileType = Path.GetExtension(filename);

            // Ignores files other than 'text' for now but keeps the structure to support more in future.
            switch (fileType)
            {
                case ".text":
                    await ProcessTextFile(client, documentId, filePath);
                    break;
            }
        }

        /// <summary>
        /// Process files switching between implemented directives, at the moment only 'lookup'
        /// </summary>
        /// <param name="client"></param>
        /// <param name="documentId"></param>
        /// <param name="filePath"></param>
        private async Task ProcessTextFile(string client, string documentId, string filePath)
        {
            var done = false;

            using (var stream = new StreamReader(filePath))
            {
                var firstLine = await stream.ReadLineAsync();
                if (firstLine != null)
                {
                    var parts = firstLine.Split('|');
                    var directive = parts[0];
                    var parameters = parts[1];

                    // Ignores directives other than 'lookup' for the moment.
                    switch (directive)
                    {
                        case "lookup":
                            var keywordsFound = LookupTextFile(parameters, stream);
                            if (keywordsFound != null)
                            {
                                await Task.Run(() => _lookupStore.Record(client, documentId, keywordsFound));
                                done = true;
                            }

                            break;
                    }
                }
            }

            if (done) // Keeps not processed files for further inspection.
                File.Delete(filePath);
        }

        /// <summary>
        /// Check keywords which exists within a file. Pattern Regex to check matches
        /// StreamReader Readline is using to read line by line to avoid memory exception
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="stream"></param>
        /// <returns>
        /// IEnumerable<string> from founded keywords or NULL if anyone
        /// </returns>
        private static IEnumerable<string> LookupTextFile(string parameters, TextReader stream)
        {
            var keywords = parameters.Split(',', StringSplitOptions.RemoveEmptyEntries);
            if (keywords.Length == 0)
                return null;

            var keywordsFound = new List<string>();

            string line = stream.ReadLine();
            while (line != null)
            {
                keywordsFound.AddRange(
                    from k in keywords
                    let pattern = @"\b" + Regex.Escape(k) + @"\b"
                    where Regex.IsMatch(line, pattern)
                    select k);
                line = stream.ReadLine();
            }

            return keywordsFound.Count == 0 ? null : keywordsFound;
        }

        #endregion Private Methods
    }
}
