using System.Globalization;
using System.Text.RegularExpressions;

namespace Okai.Ofx;

public class Transaction
{
    public string TransactionType { get; set; }
    public DateTime PostedDate { get; set; }
    public decimal TransactionAmount { get; set; }
    public decimal Amount => TransactionAmount;
    public string FitId { get; set; }
    public string CheckNumber { get; set; }
    public string Memo { get; set; }
    public string RawContent { get; set; }

    public Transaction(string content)
    {
        if (string.IsNullOrWhiteSpace(content))
        {
            throw new ArgumentException("Transaction content cannot be empty.", nameof(content));
        }

        RawContent = content;
        TransactionType = GetValueFromContent("TRNTYPE");
        PostedDate = ParsePostedDate(GetValueFromContent("DTPOSTED"));
        TransactionAmount = ParseTransactionAmount(GetValueFromContent("TRNAMT"));
        FitId = GetValueFromContent("FITID");
        CheckNumber = GetValueFromContent("CHECKNUM");
        Memo = GetValueFromContent("MEMO");
    }

    private string GetValueFromContent(string tagName)
    {
        var match = Regex.Match(
            RawContent,
            $"<{Regex.Escape(tagName)}>(.*?)(?=<|\\r?\\n|$)",
            RegexOptions.IgnoreCase | RegexOptions.Singleline);

        return match.Success ? match.Groups[1].Value.Trim() : string.Empty;
    }

    private static DateTime ParsePostedDate(string rawDate)
    {
        if (rawDate.Length < 14)
        {
            return DateTime.MinValue;
        }

        return DateTime.TryParseExact(
            rawDate[..14],
            "yyyyMMddHHmmss",
            CultureInfo.InvariantCulture,
            DateTimeStyles.None,
            out var postedDate)
            ? postedDate
            : DateTime.MinValue;
    }

    private static decimal ParseTransactionAmount(string rawAmount)
    {
        return decimal.TryParse(rawAmount, NumberStyles.Number, CultureInfo.InvariantCulture, out var amount)
            ? amount
            : 0m;
    }
}
