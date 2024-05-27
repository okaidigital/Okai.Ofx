# Okai.OFX
## The Okai.OFX Library is used to analyze OFX files and extract financial transactions. OFX (Open Financial Exchange) is a common format for exchanging financial information between financial institutions and financial applications.

## .NET version
.NET 8

### Functionalities
- Analyzes OFX files and extracts detailed financial transactions.

### Installation
You can add the `Okai.OFX` library to your .NET project. Be sure to add the reference to your project and include the required namespaces.

### Example of Use in a Project
The `Okai.OFX` library can be integrated into a larger project to process OFX files, as shown in the example below.

#### Simple Integration Example

#### File Receipt

A Power Apps field receives an OFX file in bytes. This file is obtained through a process that involves requests to Dataverse, as shown below:

```csharp
public async Task Consume(ConsumeContext<ProcessarImportacaoEvent> context)
{
     var request = context.Message;
     var dataverseClient = _dataverseFactory.CreateDataverseClient(request.OrganizacaoId);

     var requestFile = new InitializeFileBlocksDownloadRequest
     {
         Target = new EntityReference("logicalname_table", request.ImportacaoId),
         FileAttributeName = "logicalname_field"
     };
     var response = (InitializeFileBlocksDownloadResponse)dataverseClient.Execute(requestFile);

     DownloadBlockRequest downloadBlockRequest = new DownloadBlockRequest
     {
         FileContinuationToken = response.FileContinuationToken
     };

     var downloadBlockResponse = (DownloadBlockResponse)dataverseClient.Execute(downloadBlockRequest);

     byte[] fileByte = downloadBlockResponse.Data;
}
```
### Conversion to String

The OFX file in bytes is converted to a string using the "Windows-1252" encoding.

```csharp

         public async Task ProcessorFileOFX(byte[] fileByte, ServiceClient dataverseClient, EntityReference financial account, EntityReference organization)
         {
             // Converts the OFX file in bytes to a string using "Windows-1252" encoding.
             string ofxString = Encoding.GetEncoding("Windows-1252").GetString(fileByte);
            
             // Create an OFX document from the string.
             var ofxDocument = new OfxDocument(ofxString);
         }
```         

### OFX Content Analysis

The OFX library receives the converted content (`ofxString`) and extracts the transactions.

### Transaction Listing

The library lists transactions from a current account statement, including details such as:

- Type of transaction
- Release date
- Value
- Description

```csharp
             // Extracts transactions from the OFX document.
             var transactions = ofxDocument.Result();

             // Process each transaction.
             foreach (var transaction in transactions.Where(t => t.PostedDate > DateTime.MinValue))
             {
                 // Here you can add logic to handle each extracted transaction.
                 // Example: create a financial transaction, register it in the database, etc.
             }
 ```


### Conclusion

The Okai.OFX library offers an efficient solution for integrating financial data into larger systems. By extracting and processing financial transactions from OFX files, this library simplifies the import process and allows manipulation of each extracted transaction. With the flexibility to add custom logic, such as creating financial transactions or recording them in a database, Okai.OFX makes financial data integration more accessible and effective.
