Imports System.Security.Cryptography
Imports System.Text

Public Class realAuth : Inherits BaseRedirect

    Public Overrides ReadOnly Property PostURL() As String
        Get
            Return "https://epage.payandshop.com/epage.cgi"
        End Get
    End Property

    Public Sub New(ByVal MerchantName As String, ByVal OrderID As String, ByVal Amount As String, ByVal Currency As String, ByVal Password As String)
        With PostValues
            .Add("MERCHANT_ID", MerchantName)
            .Add("ORDER_ID", OrderID)
            .Add("ACCOUNT", "internettest")
            .Add("CURRENCY", Currency)
            .Add("AMOUNT", Amount)
            .Add("TIMESTAMP", DateTime.Now.ToString("yyyyMMddHHmmss"))
            .Add("SHA1HASH", SHA1Hash(MerchantName, OrderID, Amount, Currency, Password))
            .Add("AUTO_SETTLE_FLAG", "1")
        End With
    End Sub

    Private Function hexEncode(ByVal data As Byte()) As [String]

        Dim result As [String] = ""
        For Each b As Byte In data
            result += b.ToString("X2")
        Next
        result = result.ToLower()

        Return (result)

    End Function

    Private Function SHA1Hash(ByVal MerchantName As String, ByVal OrderID As String, ByVal Amount As String, ByVal Currency As String, ByVal Password As String)

        Dim sha As SHA1 = New SHA1Managed()

        Dim hashInput As [String] = String.Format("{0}.{1}.{2}.{3}.{4}", _
            DateTime.Now.ToString("yyyyMMddHHmmss"), _
            MerchantName, _
            OrderID, _
            Amount, _
            Currency _
        )

        Dim hashStage1 As [String] = hexEncode(sha.ComputeHash(Encoding.UTF8.GetBytes(hashInput))) & "." & Password
        Dim hashStage2 As [String] = hexEncode(sha.ComputeHash(Encoding.UTF8.GetBytes(hashStage1)))

        Return hashStage2

    End Function

End Class
