# Okai.Ofx

Okai.Ofx is a lightweight .NET 8 library for parsing OFX (Open Financial Exchange) bank statement content and extracting transactions.

## Features

- Parses OFX content from strings.
- Extracts statement transactions from `<STMTTRN>` blocks.
- Reads transaction type, posted date, amount, FIT ID, check number, and memo.
- Handles OFX date values that include timezone suffixes, such as `20240527123000[-3:BRT]`.

## Installation

Reference the project directly or install the `Okai.Ofx` package from your internal NuGet feed when available.

## Usage

```csharp
using Okai.Ofx;

var filePath = "path/to/your/file.ofx";
var ofxContent = File.ReadAllText(filePath);

var ofxDocument = new OfxDocument(ofxContent);
var transactions = ofxDocument.Result();

foreach (var transaction in transactions.Where(transaction => transaction.PostedDate > DateTime.MinValue))
{
    Console.WriteLine(
        $"Date: {transaction.PostedDate}, " +
        $"Amount: {transaction.Amount}, " +
        $"Description: {transaction.Memo}");
}
```

## Transaction Fields

Each parsed `Transaction` exposes:

- `TransactionType`
- `PostedDate`
- `TransactionAmount`
- `Amount`
- `FitId`
- `CheckNumber`
- `Memo`
- `RawContent`

## Validation Behavior

- Empty input throws `ArgumentException`.
- Content without an `<OFX>` tag throws `FormatException`.
- OFX documents without transaction blocks return an empty transaction list.
- Missing transaction fields are exposed as empty strings or default values instead of nullable strings.

## Build and Test

```bash
dotnet restore
dotnet build
dotnet test
```

## License

This project is licensed under the MIT License. See [LICENSE](LICENSE) for details.
