<%@ Page Language="VB" AutoEventWireup="false" CodeFile="vouchers.aspx.vb"
    Inherits="Vouchers" MasterPageFile="~/membership.master" Title="Vouchers" ValidateRequest="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
Voucher creation and management system. Click below to create a new voucher, or use the table to manage existing vouchers. 

                                
                <br /> 
                <br /> 
                <table class="basket-summery">

                
        <tr align="right">
            <td style="vertical-align: top;  text-align: right" 
                align="right">
                <asp:GridView ID="VoucherGrid" runat="server" AutoGenerateColumns="False"
                        CellPadding="4" DataKeyNames="code" >
                    <Columns >
                         <asp:TemplateField HeaderText="Voucher" >
                            <ItemTemplate>
                                <%#Eval("code")%>
                                
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Description">
                            <ItemTemplate>
                                <%#Eval("des")%>
                            </ItemTemplate>                            
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Type">
                            <ItemTemplate>
                        <%#Eval("type")%>
                            </ItemTemplate>                                                        
                        </asp:TemplateField>
                         <asp:TemplateField HeaderText="Expiry">
                            <ItemTemplate>
                        <%#Eval("expiry")%>
                            </ItemTemplate>                                                        
                        </asp:TemplateField>
                        
                        <asp:CommandField ShowDeleteButton="True" ShowEditButton="True" ShowCancelButton="False" />
                    </Columns>
                    
                   
                    
                    <EmptyDataTemplate>
                        There are currently no vouchers to display
                    </EmptyDataTemplate>
                    <HeaderStyle CssClass="basket-header" />
                </asp:GridView>
                </tr> 
                </td>
                </table>
                
                <br />

                <asp:Button ID="CreateVoucher" runat="server" Text="Create Voucher" />
</asp:Content>
