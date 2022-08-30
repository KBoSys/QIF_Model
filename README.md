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

# Test Application
## QIF_Model_Use
QIF_Model_Use is a console application that receives a qif file as an input parameter.
1. Validates the input file agains the XSD 
2. Creates QIF document from the input file - Deserialization
3. Exports the QIF Document to new file - Serialization
4. Validates the output file agains the XSD

## MS ubit test - test harness application
The test application reads files from the test dataset, exports the document to new file and compares the input and output files.
The test set contains 48 qif files for now.
