<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Default.aspx.vb" Inherits="dashboard" %>

<!DOCTYPE html>

<html >
<head runat="server">
    <title><!-- Insert title here --></title>
    <link href='/main.css' rel='stylesheet' type='text/css' />
    <link rel="SHORTCUT ICON" href="" /> <!-- Insert shortcut / browser tab icon -->
</head>

<body>
    <form id="form1" runat="server">
    <div>
        
    <!-- Update panel refreshes the page  -->
    
    <asp:UpdatePanel ID="UpdatePanel" runat="server">
    
        <ContentTemplate>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        
        <!-- Set the interval in ms -->
        
        <asp:Timer ID="timer" runat="server" Interval="180000" OnTick="Timer_Tick"></asp:Timer>
     
        <div id="container">
        
        <div id="title">
        </div
        
        <!-- Insert charts and SQL data sources below -->
        
        </div> <!-- Container div end -->
            
        </ContentTemplate>
    </asp:UpdatePanel>
    
    </div>
        
    </form>
</body>
</html>
