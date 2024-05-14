using System.Globalization;
using System.Text.RegularExpressions;

namespace Cofactory.OFX
{
    public class OfxDocument
    {
        public string Headers { get; private set; }
        public string BeforeBody { get; private set; }
        public Transaction[] Transactions { get; private set; }

        public OfxDocument(string ofxContent)
        {
            ParseContent(ofxContent);
        }

        private void ParseContent(string content)
        {
            int sgmlStart = content.IndexOf("<OFX>");

            if (sgmlStart < 0)
            {
                throw new Exception("Arquivo OFX inválido. Verifique se o mesmo esta sendo enviado no formato correto.");
            }

            Headers = content.Substring(0, sgmlStart);
            content = content.Substring(sgmlStart);

            int startBody = content.IndexOf("<STMTTRN>");
            string body = content.Substring(startBody);
            BeforeBody = content.Substring(0, startBody);

            string[] transactions = Regex.Split(body, @"(?=<STMTTRN>)");
            Transactions = transactions.Select(t => new Transaction(t)).ToArray();
        }

        public void AddTransaction(Transaction transaction)
        {
            Transactions = Transactions.Concat(new[] { transaction }).ToArray();
        }
        public Transaction[] Result()
        {
            return Transactions;
        }
    }
}