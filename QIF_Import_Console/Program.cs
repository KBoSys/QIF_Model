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
			if (args.Length < 1)
			{
				Console.WriteLine("No arguments!");
				return;
			}

			string input_file = args[0];

			// Validate the input against the XSD
			//if (!Validate(input_file))
			//	return;

			// Create QIF document from the input file
			QIFSerializer qifImport = new QIFSerializer();
            QIFDocumentType document = qifImport.CreateQIFDocument(input_file);

            if (document != null)
			{
				Console.WriteLine(document.QPId);
			}

			// Export the document into test folder 
			string filename = Path.GetFileNameWithoutExtension(input_file);
			string output_file = @"..\..\..\TestFiles\Test\" + filename + ".conv.qif";
            qifImport.Write(document, output_file);

			// Validate the output file agains the XSD
			Validate(output_file);
		}

		static bool Validate(string filename)
		{
			return QIFSerializer.Validate(filename, "http://qifstandards.org/xsd/qif3", @"..\..\..\xsd\QIFApplications\QIFDocument.xsd");
		}
	}
}
