using System.Text.RegularExpressions;

namespace Okai.Ofx;

public class OfxDocument
{
    private const string OfxStartTag = "<OFX>";
    private const string TransactionStartTag = "<STMTTRN>";
    private static readonly Regex TransactionRegex = new(
        @"<STMTTRN>.*?(?=<STMTTRN>|</BANKTRANLIST>|</CREDITCARDMSGSRSV1>|</OFX>|$)",
        RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Compiled);

    public string Headers { get; private set; } = string.Empty;
    public string BeforeBody { get; private set; } = string.Empty;
    public Transaction[] Transactions { get; private set; } = Array.Empty<Transaction>();

    public OfxDocument(string ofxContent)
    {
        if (string.IsNullOrWhiteSpace(ofxContent))
        {
            throw new ArgumentException("OFX content cannot be empty.", nameof(ofxContent));
        }

        ParseContent(ofxContent);
    }

    public void AddTransaction(Transaction transaction)
    {
        ArgumentNullException.ThrowIfNull(transaction);
        Transactions = Transactions.Concat(new[] { transaction }).ToArray();
    }

    public Transaction[] Result()
    {
        return Transactions;
    }

    private void ParseContent(string content)
    {
        var sgmlStart = content.IndexOf(OfxStartTag, StringComparison.OrdinalIgnoreCase);

        if (sgmlStart < 0)
        {
            throw new FormatException("Invalid OFX content: missing <OFX> tag.");
        }

        Headers = content[..sgmlStart];
        var ofxBody = content[sgmlStart..];

        var firstTransactionStart = ofxBody.IndexOf(TransactionStartTag, StringComparison.OrdinalIgnoreCase);
        if (firstTransactionStart < 0)
        {
            BeforeBody = ofxBody;
            Transactions = Array.Empty<Transaction>();
            return;
        }

        BeforeBody = ofxBody[..firstTransactionStart];
        var transactionBody = ofxBody[firstTransactionStart..];

        Transactions = TransactionRegex
            .Matches(transactionBody)
            .Select(match => new Transaction(match.Value))
            .ToArray();
    }
}
