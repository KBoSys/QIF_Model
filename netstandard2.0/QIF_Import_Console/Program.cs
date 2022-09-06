/*! \file Program.cs
    \brief Console test application

    \copyright Copyright © 2022 KBO Systems Inc. All rights reserved.    
*/
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;
using QIF_Model.Helpers;
using QIF_Model.QIFApplications;

namespace QIF_Import_Console
{
    public class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("No arguments. Expected: input_dir output_dir");
                return;
            }

            string sourceDirectory = args[0];
            string destDirectory = args[1];

            try
            {
                var files = Directory.EnumerateFiles(sourceDirectory, "*.qif", SearchOption.TopDirectoryOnly);

                foreach (string currentFile in files)
                {
                    Console.WriteLine("------------------------------------------------------");
                    // Validate the input against the XSD
                    //if (!Validate(input_file))
                    //	return;

                    Console.WriteLine($"Reading {currentFile}...");

                    // Create QIF document from the input file
                    QIFSerializer qifImport = new QIFSerializer();
                    QIFDocumentType document = qifImport.CreateQIFDocument(currentFile);
                    if (document == null)
                    {
                        Console.WriteLine("Could not create QIF Document.");
                        break;
                    }

                    // Export the document into test folder 
                    string filename = Path.GetFileNameWithoutExtension(currentFile);
                    //string output_file = @"..\..\..\TestFiles\Test\" + filename + ".conv.qif";
                    string output_file = Path.Combine(destDirectory, $"{filename}.conv.qif");

                    Console.WriteLine($"Exporting {output_file}...");
                    qifImport.Write(document, output_file);

                    // Validate the output file agains the XSD
                    if (!Validate(output_file))
                    {
                        //break;
                    }
                    Console.WriteLine("------------------------------------------------------\n");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        static bool Validate(string filename)
        {
            Console.WriteLine($"Validating {filename}");

            List<string> errorMessages = new List<string>();
            uint errors = QIFSerializer.Validate(
                filename,                                           // QIF file to validate
                @"..\..\..\..\xsd\QIFApplications\QIFDocument.xsd",    // Schema
                30,                                                 // Max Errors
                out errorMessages                                   // Output Error Message
                );

            if (errors == 0)
            {
                Console.WriteLine("XML validation succeeded");
            }
            else
            {
                Console.WriteLine("XML validation Failed");
            }

            foreach (var err in errorMessages)
            {
                Console.WriteLine(err);
            }
            return errors == 0;
        }
    }
}
