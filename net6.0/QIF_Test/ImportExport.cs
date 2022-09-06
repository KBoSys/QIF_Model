using QIF_Model.Helpers;
using QIF_Model.QIFApplications;
using System.Text.RegularExpressions;

namespace QIF_Test
{
    [TestClass]
    public class ImportExport
    {
        public TestContext TestContext { get; set; }

        private string? SkipEmptyRowsAndTrim(StreamReader reader, ref int row)
        {
            string? line = reader.ReadLine();
            string? result = null;

            while (line != null)
            {
                ++row;
                result = line.Trim();
                if (!String.IsNullOrEmpty(result))
                    break;

                line = reader.ReadLine();
                result = null;
            }
            return result;
        }

        // This method accepts two strings the represent two files to
        // compare. A return value of 0 indicates that the contents of the files
        // are the same. A return value of any other value indicates that the
        // files are not the same.
        private bool FileCompare(string file1, string file2)
        {
            // Determine if the same file was referenced two times.
            if (file1 == file2)
            {
                // Return true to indicate that the files are the same.
                return true;
            }

            StreamReader fs1;
            StreamReader fs2;

            // Open the two files.
            fs1 = new StreamReader(file1);
            fs2 = new StreamReader(file2);

            int row1 = 0;
            int row2 = 0;
            string? l1, l2;

            bool equal = true;
            do
            {
                l1 = SkipEmptyRowsAndTrim(fs1, ref row1);
                l2 = SkipEmptyRowsAndTrim(fs2, ref row2);

                if (l1 != null && l2 != null)
                {
                    if (l1 != l2) // string comparisson
                    {
                        // Try withoud withespaces
                        string normalized1 = Regex.Replace(l1, @"\s", "");
                        string normalized2 = Regex.Replace(l2, @"\s", "");
                        equal = String.Equals(normalized1, normalized2, StringComparison.Ordinal);
                    }
                }
                else
                {
                    break; // we reached the end of one or both files
                }
            }
            while (equal);

            equal = l1 == null && l2 == null; // The files are equal only if we reach the end of both of them

            int index = 0;
            if (!equal)
            {
                if (l1 != null && l2 != null)
                {
                    index = l1.Zip(l2, (c1, c2) => c1 == c2).TakeWhile(b => b).Count() + 1;
                    TestContext.WriteLine($"File {file1} is different from {file2}\nLine 1: {row1}, Line 2: {row2}, Pos: {index}");
                }
                else if (l1 == null)
                {
                    TestContext.WriteLine($"Input File {file1} is shorter then {file2}\nLine 1: {row1}, Line 2: {row2}");
                }
                else if (l2 == null)
                {
                    TestContext.WriteLine($"Input File {file1} is longer then {file2}\nLine 1: {row1}, Line 2: {row2}");
                }
            }

            // Close the files.
            fs1.Close();
            fs2.Close();

            // Return the success of the comparison. "file1byte" is
            // equal to "file2byte" at this point only if the files are
            // the same.
            return equal;
        }

        [TestMethod]
        public void TestMethod()
        {
            QIFSerializer qifImport = new QIFSerializer();

            string sourceDirectory = @"..\..\..\..\..\TestFiles";
            string outputFile = $"{sourceDirectory}\\Test\\Test.qif";
            try
            {
                var files = Directory.EnumerateFiles(sourceDirectory, "*.qif", SearchOption.TopDirectoryOnly);

                foreach (string currentFile in files)
                {
                    TestContext.WriteLine($"File {currentFile}");

                    string fileName = currentFile.Substring(sourceDirectory.Length + 1);

                    QIFDocumentType? document = qifImport.CreateQIFDocument(currentFile);
                    Assert.IsNotNull(document, "Could not create QIFDocument");

                    qifImport.Write(document, outputFile);
                    Assert.IsTrue(FileCompare(currentFile, outputFile), $"Problem with {currentFile}.");
                }
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }
    }
}