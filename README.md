# Okai.OFX
## Okai.OFX Library is a versatile tool for parsing OFX files and retrieving intricate financial transactions. OFX (Open Financial Exchange) is a universally accepted format for exchanging financial data between financial institutions and finance-based applications.

## .NET version
.NET 8

### Functionalities
- Analyzes OFX files and extracts detailed financial transactions.

### Installation
You can add the `Okai.OFX` library to your .NET project. Be sure to add the reference to your project and include the required namespaces.

### Example of Use in a Project
`Okai.OFX` library can be integrated into a larger project to process OFX files, as shown in the example below.

### Simple Integration Example
Assuming you have an OFX file stored on your computer, here's how you can utilize the Okai.OFX library to process it.

```csharp
Copy code
using Okai.OFX; // Import the Okai.OFX namespace
using System;
using System.IO;
using System.Linq;

class Program
{
    static void Main(string[] args)
    {
        string filePath = "path/to/your/file.ofx"; // Path to the OFX file on your computer

        // Read the content of the OFX file as a string
        string ofxString = File.ReadAllText(filePath);

        // Create an OFX document from the string
        var ofxDocument = new OfxDocument(ofxString);
        
        // Extract transactions from the OFX document
        var transactions = ofxDocument.Result();

        // Process each transaction
        foreach (var transaction in transactions.Where(t => t.PostedDate > DateTime.MinValue))
        {
            // Add your logic to handle each extracted transaction
            // Example: create a financial transaction, store it in the database, etc.
            Console.WriteLine($"Date: {transaction.PostedDate}, Amount: {transaction.Amount}, Description: {transaction.Memo}");
        }
    }
}
```

## OFX Content Analysis
Okai.OFX library receives the content (`ofxString`) and extracts the transactions.

## Transaction Listing
The library lists transactions from a current account statement, including details such as:

- Type of transaction
- Release date
- Value
- Description

### Conclusion
Okai.OFX library offers an efficient solution for integrating financial data into larger systems. By extracting and processing financial transactions from OFX files, this library simplifies the import process and allows manipulation of each extracted transaction. With the flexibility to add custom logic, such as creating financial transactions or recording them in a database, Okai.OFX makes financial data integration more accessible and effective.
