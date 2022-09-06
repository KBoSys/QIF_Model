## C# Class Library for Implementation of QIF 3.0 Standard
## Target framework: .NET Standard 2.0

Full implementation of QIF 3.0 Standard with test application.
Precise reading and writing of QIF files without loss of information.

## Copyright

**Existing XSD and XSLT checks in QIF 3.0 are:**
Copyright © 2018 by Digital Metrology Standards Consortium, Inc. (DMSC)

**The source code of QIF_Model is:**
Copyright © 2022 by KBO Systems Inc.

## License

See [License](LICENSE.md)

## Overview

Using QIF_Model library is extremely easy:

# Create QIF Document from qif file
using QIF_Model.Helpers;
using QIF_Model.QIFApplications;

QIFSerializer qifImport = new QIFSerializer();
QIFDocumentType document = qifImport.CreateQIFDocument(filename);

# Export QIF Document to qif file
qifImport.Write(document, filename);

#Validate QIF file against XSD
uint errors = QIFSerializer.Validate(
                filename,                    // QIF file to validate
                $"{path}QIFDocument.xsd",    // XSD
                10,                          // Max number of errors 
                out errorMessages            // Output List with Error Messages
                );


# Test Application
## QIF_Model_Use
QIF_Model_Use is a console application that receives a qif file as an input parameter.
1. Validates the input file against the XSD 
2. Creates QIF document from the input file - Deserialization
3. Exports the QIF Document to new file - Serialization
4. Validates the output file against the XSD

## MS unit test - test harness application
The test application reads files from the test dataset, exports the document to new file and compares the input and output files.
The test set contains 48 qif files for now.
