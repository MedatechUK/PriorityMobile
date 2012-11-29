<%@ Page Language="VB" %>
<%
    'Pay and Shop Limited (payandshop.com) - Licence Agreement.
    '© Copyright and zero Warranty Notice.
    '
    '
    'Merchants and their internet, call centre, and wireless application
    'developers (either in-house or externally appointed partners and
    'commercial organisations) may access payandshop.com technical
    'references, application programming interfaces (APIs) and other sample
    'code and software ("Programs") either free of charge from
    'www.payandshop.com or by emailing info@payandshop.com. 
    '
    'payandshop.com provides the programs "as is" without any warranty of
    'any kind, either expressed or implied, including, but not limited to,
    'the implied warranties of merchantability and fitness for a particular
    'purpose. The entire risk as to the quality and performance of the
    'programs is with the merchant and/or the application development
    'company involved. Should the programs prove defective, the merchant
    'and/or the application development company assumes the cost of all
    'necessary servicing, repair or correction.
    '
    'Copyright remains with payandshop.com, and as such any copyright
    'notices in the code are not to be removed. The software is provided as
    'sample code to assist internet, wireless and call center application
    'development companies integrate with the payandshop.com service.
    '
    'Any Programs licensed by Pay and Shop to merchants or developers are
    'licensed on a non-exclusive basis solely for the purpose of availing
    'of the Pay and Shop payment solution service in accordance with the
    'written instructions of an authorised representative of Pay and Shop
    'Limited. Any other use is strictly prohibited.
%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<script runat="server">
</script>

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Realex Payments Sample Transaction Page</title>
</head>

<body>

    <%        
        Dim tReq As RealexPayments.RealAuth.TransactionRequest
        Dim ccReq As RealexPayments.RealAuth.CreditCard
        Dim tResp As RealexPayments.RealAuth.TransactionResponse

        Dim backgroundColour As String
        Dim displayMessage As String
        
        Try
            Dim orderID As String
            orderID = Now().ToString("yyyyMMddhhmmss")
            
            tReq = New RealexPayments.RealAuth.TransactionRequest("yourmerchantidhere", "sharedsecret", "rebatepassword", "refundpassword")
            ccReq = New RealexPayments.RealAuth.CreditCard("visa", "4242424242424242", "0609", "Andrew Harcourt", "123", RealexPayments.RealAuth.CreditCard.CVN_PRESENT)
            
            tReq.TransAmount = 4242
            tReq.TransCurrency = "EUR"
            tReq.TransAccountName = "internet"
            tReq.TransCard = ccReq
            tReq.TransType = "auth"
            tReq.TransBillingAddressCountry = "au"
            tReq.TransShippingAddressCountry = "ie"
            tReq.TransAutoSettle = 1
            tReq.TransComments.Add("How many roads must a man walk down?")
            tReq.TransComments.Add("What do you get if you multiply six by nine?")
            tReq.TransCustomerNumber = "12345678"
            tReq.TransProductID = "marvin"
            tReq.TransVariableReference = "Arthur"
            
            tResp = tReq.SubmitTransaction()
            
            If (tResp.ResultCode = 0) Then
                displayMessage = "Transaction successful."
                backgroundColour = "green"
            Else
                displayMessage = "Transaction failed."
                backgroundColour = "red"
            End If
            
        Catch
            ' transaction failed
            displayMessage = "Transaction timed out or failed with unhandled exception."
            backgroundColour = "yellow"
        End Try
        
        Try
        %>
        <p style="background-color: <%=backgroundColour %>;">
            <%=displayMessage%>
        </p>
        
        <dl>
            <dt>Result Code</dt>
            <dd><%=tResp.ResultCode %></dd>
            
            <dt>Message</dt>
            <dd><%=tResp.ResultMessage %></dd>
        </dl>
        <%
        Catch
            ' null pointer exception. safe to ignore.            
        End Try
     %>
        
</body>
</html>
