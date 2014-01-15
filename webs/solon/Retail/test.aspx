<%@ Page Language="VB" AutoEventWireup="false" CodeFile="test.aspx.vb" Inherits="test" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <asp:LoginName ID="LoginName1" FormatString="Hi {0}!" runat="server" />
    <% Eval("Hi " & User.Identity.Name.Split("@")(0) & "!")%>
    <% Response.Write(Profile.Name.First)%>
    </div>
    </form>
</body>
</html>
