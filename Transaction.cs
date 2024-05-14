using System.Globalization;
using System.Text.RegularExpressions;

namespace Cofactory.OFX
{
    public class Transaction
    {
        public string TransactionType { get; set; }
        public DateTime PostedDate { get; set; }
        public decimal TransactionAmount { get; set; }
        public string FitId { get; set; }
        public string CheckNumber { get; set; }
        public string Memo { get; set; }
        public string RawContent { get; set; }

        public Transaction(string content)
        {
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
            var match = Regex.Match(RawContent, $"<{tagName}>(.*?)(<|\n)", RegexOptions.Singleline);
            if (match.Success)
            {
                return match.Groups[1].Value.Trim();
            }
            return null;
        }

        private DateTime ParsePostedDate(string rawDate)
        {
            if (rawDate != null && rawDate.Length >= 14)
            {
                return DateTime.ParseExact(rawDate.Substring(0, 14), "yyyyMMddHHmmss", CultureInfo.InvariantCulture);
            }
            else
            {
                return DateTime.MinValue;
            }
        }

        private decimal ParseTransactionAmount(string rawAmount)
        {
            if (!string.IsNullOrEmpty(rawAmount))
            {
                return decimal.Parse(rawAmount, CultureInfo.InvariantCulture);
            }
            else
            {
                return 0;
            }
        }
    }
}
