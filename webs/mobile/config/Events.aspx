<%@ Page Title="" Language="VB" MasterPageFile="~/config/SoapService.master" AutoEventWireup="false"
    CodeFile="Events.aspx.vb" Inherits="config_Events" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="box">
        <h2 style="font-family: Verdana; font-size: large">
            Filters</h2>
        <table style="width: 97%">
            <tr>
                <th class="style1" style="text-align: right; font-family: Verdana; font-size: small;">
                    Date
                </th>
                <td>
                </td>
                <td colspan="2">
                    <asp:DropDownList runat="server" ID="DropDownList1" Style="font-family: Verdana;
                        font-size: small">
                        <asp:ListItem Value="0">Today</asp:ListItem>
                        <asp:ListItem Value="1">Past 24 Hrs</asp:ListItem>
                        <asp:ListItem Value="2">Past 3 Days</asp:ListItem>
                        <asp:ListItem Value="3">Past 5 Days</asp:ListItem>
                        <asp:ListItem Value="4">Past 7 Days</asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <th class="style1" style="text-align: right; font-family: Verdana; font-size: small;">
                    Level
                </th>
                <td>
                </td>
                <td>
                    <asp:CheckBoxList ID="CheckBoxList1" runat="server" RepeatDirection="Horizontal"
                        Width="248px" Font-Names="Verdana" Font-Size="Small">
                        <asp:ListItem Value="4">Success</asp:ListItem>
                        <asp:ListItem Value="5" Selected="True">Failure</asp:ListItem>
                        <asp:ListItem Selected="True" Value="1">Error</asp:ListItem>
                        <asp:ListItem Selected="True" Value="2">Warning</asp:ListItem>
                        <asp:ListItem Value="3">Information</asp:ListItem>
                    </asp:CheckBoxList>
                </td>
                <td class="style2">
                </td>
            </tr>
            <tr>
                <th style="text-align: right; font-family: Verdana; font-size: small;">
                    Find
                </th>
                <td>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtSearch" Width="345px" MaxLength="75" Style="font-family: Verdana;
                        font-size: small"></asp:TextBox>
                </td>
                <td class="style2">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <th colspan="2" style="text-align: right">
                </th>
                <td>
                    <asp:Button ID="Button1" runat="server" Style="text-align: center; font-family: Verdana;
                        font-size: small;" Text="GO" />
                </td>
                <td class="style2">
                </td>
            </tr>
        </table>
        <br>
    </div>
    <asp:Panel ID="pnlLoader" runat="server">
    </asp:Panel>
    <div id="dg">
        <asp:Panel ID="pnlGrid" runat="server">
            <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                DataKeyNames="Message" AllowSorting="True" PageSize="20" OnSorting="GridView1_Sorting">
                <RowStyle BorderStyle="Solid" />
                <EmptyDataRowStyle BorderStyle="Solid" />
                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <img alt="" style="cursor: pointer" src="Images/plus.png" />                            
                            <asp:Panel ID="pnlMSG" runat="server" Style="display: none">
                                <asp:TextBox ID="txtMessage" runat="server" Text="" TextMode="MultiLine" Width="350"
                                    Height="150" BorderStyle="Solid" BackColor="LightYellow"></asp:TextBox>
                            </asp:Panel>
                            </asp:Panel>
                        </ItemTemplate>
                        <HeaderStyle BackColor="#666666" ForeColor="White" />
                    </asp:TemplateField>
                    <asp:ImageField DataImageUrlField="Img">
                        <HeaderStyle HorizontalAlign="Left" BackColor="#666666" BorderStyle="None" ForeColor="White" />
                    </asp:ImageField>
                    <asp:BoundField HeaderText="Level" DataField="EntryType" SortExpression="EntryType">
                        <HeaderStyle HorizontalAlign="Left" BackColor="#666666" BorderStyle="None" ForeColor="White" />
                        <ItemStyle HorizontalAlign="Left" Width="175px" BorderStyle="None" />
                    </asp:BoundField>
                    <asp:BoundField HeaderText="Date and Time" DataField="TimeGenerated" SortExpression="TimeGenerated">
                        <HeaderStyle HorizontalAlign="Left" BackColor="#666666" ForeColor="White" />
                        <ItemStyle HorizontalAlign="Left" Width="175px" BorderStyle="Solid" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Message" HeaderText="Message" ItemStyle-Width="0px" ReadOnly="True"
                        Visible="False">
                        <ItemStyle Width="450px"></ItemStyle>
                    </asp:BoundField>
                </Columns>
                <PagerStyle BorderStyle="Solid" />
                <EditRowStyle BorderStyle="Solid" />
                <AlternatingRowStyle BorderStyle="Solid" />
            </asp:GridView>
        </asp:Panel>
        <asp:XmlDataSource ID="XmlDataSource1" runat="server"></asp:XmlDataSource>
        <br />
    </div>
</asp:Content>
