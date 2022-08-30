## C# Class Library for Implementation of QIF 3.0 Standard
## Target framework: .NET Standard 2.0

Full implementation of QIF 3.0 Standard with test application.
Precise reading and writing of QIF files without loss of information.

## Overview

Using QIF_Model library is extremely easy:

# Create QIF Document from qif file
using QIF_Model.Helpers;
using QIF_Model.QIFApplications;

QIFSerializer qifImport = new QIFSerializer();
QIFDocumentType document = qifImport.CreateQIFDocument(filename);

# Export QIF Document to qif file

qifImport.Write(document, filename);

# The test harness application
The test application reads files from the test dataset, exports the document to new file and compares the input and output files.
The test set contains 48 qif files for now.
