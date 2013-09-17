<%@ Page Language="VB" debug="true" AutoEventWireup="false" CodeFile="Default.aspx.vb" Inherits="_Default" validateRequest="false" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="StyleSheet.css" rel="stylesheet" type="text/css" />
    
<script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
<script type="text/javascript">
    $("[src*=plus]").live("click", function() {
        $(this).closest("tr").after("<tr><td></td><td colspan = '999'>" + $(this).next().html() + "</td></tr>")
        $(this).attr("src", "images/minus.png");
    });
    $("[src*=minus]").live("click", function() {
        $(this).attr("src", "images/plus.png");
        $(this).closest("tr").next().remove();
    });
</script>


</head>
<body>
    <form id="form1" runat="server">

<div class="box">
<h2>Filters</h2>
<table style="width: 97%">
<tr><th class="style1">Level</th><td></td><td>
    <asp:CheckBoxList ID="CheckBoxList1" runat="server" 
        RepeatDirection="Horizontal" Width="248px">
        <asp:ListItem Value="4">Success</asp:ListItem>
        <asp:ListItem Value="5" Selected="True">Failure</asp:ListItem>
        <asp:ListItem Selected="True" Value="1">Error</asp:ListItem>
        <asp:ListItem Selected="True" Value="2">Warning</asp:ListItem>
        <asp:ListItem Value="3">Information</asp:ListItem>
       </asp:CheckBoxList>
    </td><td class="style2"></td></tr>
<tr><th >Search Message</th><td></td><td>
    <asp:TextBox runat="server" ID="txtSearch" 
        Width="345px" MaxLength="75"></asp:TextBox></td><td class="style2">
        &nbsp;</td></tr>
       <tr><th class="style1">Date</th><td></td><td colspan =2" >
           <asp:DropDownList runat="server" ID="DropDownList1">
                    <asp:ListItem>Today</asp:ListItem>
                    <asp:ListItem>Past 24 Hrs</asp:ListItem>
                    <asp:ListItem>Past 7 Days</asp:ListItem>
                    <asp:ListItem>Past 30 Days</asp:ListItem>
                    <asp:ListItem>Past 90 Days</asp:ListItem>
                    </asp:DropDownList>  </td></tr>
                    
<tr><th colspan="2"></th><td>
                        <asp:Button ID="Button1" runat="server" style="text-align: center" Text="GO" />
                        </td><td class="style2"></td></tr>
</table>

</div>
<asp:Panel ID="pnlLoader" runat="server">
</asp:Panel>
        <div id="dg">
    <asp:Panel ID="pnlGrid" runat ="server">

        <asp:GridView ID="GridView1" runat="server" 
                AllowPaging="True" AutoGenerateColumns="False" datakeynames="Message" 
                AllowSorting="True" PageSize="20"
             onsorting="GridView1_Sorting">
               
                
               
                <RowStyle BorderStyle="Solid" />
                <EmptyDataRowStyle BorderStyle="Solid" />
               
                
               
                <Columns>                   
         <asp:TemplateField>
            <ItemTemplate>
                <img alt = "" style="cursor: pointer" src="Images/plus.png" />
                <asp:Panel ID="pnlMSG" runat="server" Style="display: none">
                <asp:TextBox ID="txtMessage" runat="server" Text="" TextMode="MultiLine" Width="350" Height="150" BorderStyle="Solid" BackColor="LightYellow"></asp:TextBox>

                </asp:Panel>
            </ItemTemplate>
             <HeaderStyle BackColor="#666666" ForeColor="White" />
        </asp:TemplateField>
<asp:TemplateField>
            <ItemTemplate>
                <asp:image runat="server" ID="imgLevImg" Height="25" Width="25" />
                

            </ItemTemplate>
            <HeaderStyle BackColor="#666666" BorderStyle="None" ForeColor="White" />
            <ItemStyle BorderStyle="None" />
        </asp:TemplateField>
                    <asp:BoundField HeaderText="Level" DataField= "EntryType" 
                        SortExpression="EntryType" >
                        <HeaderStyle HorizontalAlign="Left" BackColor="#666666" BorderStyle="None" 
                        ForeColor="White" />
                    <ItemStyle HorizontalAlign="Left" Width="175px" BorderStyle="None" />
                    </asp:BoundField>
                        <asp:BoundField HeaderText="Date and Time" DataField= "TimeGenerated" 
                        SortExpression="TimeGenerated" >
                    
                    <HeaderStyle HorizontalAlign="Left" BackColor="#666666" ForeColor="White" />
                    <ItemStyle HorizontalAlign="Left" Width="175px" BorderStyle="Solid" />
                    </asp:BoundField>
                    
                    <asp:BoundField DataField="Message" HeaderText="Message" 
                        ItemStyle-Width="0px" ReadOnly="True" Visible="False">
<ItemStyle Width="450px"></ItemStyle>
                    </asp:BoundField>
                </Columns>
                <PagerStyle 
                    BorderStyle="Solid" />
<EditRowStyle 
                    BorderStyle="Solid" />
                <AlternatingRowStyle BorderStyle="Solid" />

            </asp:GridView>


</asp:Panel>
            </div>
        

    
    </form>
</body>
</html>
