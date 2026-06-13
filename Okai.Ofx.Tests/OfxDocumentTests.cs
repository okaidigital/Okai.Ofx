namespace Okai.Ofx.Tests;

[TestClass]
public sealed class OfxDocumentTests
{
    [TestMethod]
    public void Constructor_ParsesBankStatementTransactions()
    {
        var document = new OfxDocument(SampleOfx);

        Assert.HasCount(2, document.Transactions);
        Assert.Contains("OFXHEADER:100", document.Headers);
        Assert.Contains("<BANKTRANLIST>", document.BeforeBody);

        var firstTransaction = document.Transactions[0];
        Assert.AreEqual("DEBIT", firstTransaction.TransactionType);
        Assert.AreEqual(new DateTime(2024, 05, 27, 12, 30, 00), firstTransaction.PostedDate);
        Assert.AreEqual(-10.50m, firstTransaction.TransactionAmount);
        Assert.AreEqual(firstTransaction.TransactionAmount, firstTransaction.Amount);
        Assert.AreEqual("FIT-001", firstTransaction.FitId);
        Assert.AreEqual("123", firstTransaction.CheckNumber);
        Assert.AreEqual("Coffee shop", firstTransaction.Memo);
    }

    [TestMethod]
    public void Constructor_ReturnsEmptyTransactionsWhenStatementHasNoTransactions()
    {
        var document = new OfxDocument("""
            OFXHEADER:100

            <OFX>
            <BANKTRANLIST>
            </BANKTRANLIST>
            </OFX>
            """);

        Assert.IsEmpty(document.Transactions);
    }

    [TestMethod]
    public void Constructor_RejectsContentWithoutOfxTag()
    {
        var exception = Assert.ThrowsExactly<FormatException>(() => new OfxDocument("not an OFX document"));

        Assert.AreEqual("Invalid OFX content: missing <OFX> tag.", exception.Message);
    }

    [TestMethod]
    public void AddTransaction_AppendsTransaction()
    {
        var document = new OfxDocument(SampleOfx);
        var transaction = new Transaction("""
            <STMTTRN>
            <TRNTYPE>CREDIT
            <DTPOSTED>20240529110000
            <TRNAMT>42.00
            <FITID>FIT-003
            <MEMO>Manual adjustment
            """);

        document.AddTransaction(transaction);

        Assert.HasCount(3, document.Result());
        Assert.AreSame(transaction, document.Result()[2]);
    }

    private const string SampleOfx = """
        OFXHEADER:100
        DATA:OFXSGML

        <OFX>
        <BANKMSGSRSV1>
        <STMTTRNRS>
        <STMTRS>
        <BANKTRANLIST>
        <STMTTRN>
        <TRNTYPE>DEBIT
        <DTPOSTED>20240527123000[-3:BRT]
        <TRNAMT>-10.50
        <FITID>FIT-001
        <CHECKNUM>123
        <MEMO>Coffee shop
        <STMTTRN>
        <TRNTYPE>CREDIT
        <DTPOSTED>20240528100000
        <TRNAMT>2500.00
        <FITID>FIT-002
        <MEMO>Salary
        </BANKTRANLIST>
        </STMTRS>
        </STMTTRNRS>
        </BANKMSGSRSV1>
        </OFX>
        """;
}
