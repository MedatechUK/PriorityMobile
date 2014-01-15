<%@ Page Language="VB" MasterPageFile="~/membership.master" AutoEventWireup="false" CodeFile="basket.aspx.vb" Inherits="basket" title="Shopping Basket" ValidateRequest="false"%>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">

    <table class="basket-summery">
                
        <tr align="right">
            <td style="vertical-align: top;  text-align: right" 
                align="right">
                
                <asp:GridView ID="BasketGrid" runat="server" AutoGenerateColumns="False"
                        CellPadding="4" DataKeyNames="PARTNAME" ShowFooter="True">
                    <Columns >
                         <asp:TemplateField visible="False" >
                            <ItemTemplate>
                                <%#Eval("PARTNAME")%>
                                item
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Part">
                            <ItemTemplate>
                                <asp:HyperLink ID="PARTNAME" runat="server" NavigateUrl='<%#Eval("REFERER")%>' Text='<%#Eval("PARTNAME")%>'></asp:HyperLink>
                            </ItemTemplate>                            
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Description">
                            <ItemTemplate>
                        <%#Eval("PARTDES")%>
                            </ItemTemplate>                                                        
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Price">
                            <ItemTemplate>
                        <%#Eval("PARTPRICE")%>
                            </ItemTemplate>                                            
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="QTY">
                            <ItemTemplate>
                        <%#Eval("QTY")%>
                            </ItemTemplate>                                            
                            <EditItemTemplate>
                                &nbsp;
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Line Total">
                            <ItemTemplate>
                        <%#Eval("LINETOTAL")%>
                            </ItemTemplate>                                            
                        </asp:TemplateField>
                        <asp:CommandField ShowDeleteButton="True" ShowEditButton="True" ShowCancelButton="False" />
                    </Columns>
                    
                    <FooterStyle CssClass="basket-footer" />
                    
                    <EmptyDataTemplate>
                        There are currently no items in your basket.
                    </EmptyDataTemplate>
                    <HeaderStyle CssClass="basket-header" />
                </asp:GridView>
                
             
                
            </td>
            <td style="vertical-align: top;  text-align: right" 
                align="right">
                
                <asp:Label ID="lbllstCurrency" runat="server" Text="Select Currency" Width="100px"/>
                <br />
                <asp:DropDownList ID="lstCurrency" runat="server" AutoPostBack="True"/>            

            </td>
        </tr>    
        <tr align="right">
            <td style="vertical-align: top; text-align: right" 
                align="right" class="buy-buttons">
                <asp:LoginView ID="LoginView" runat="server">
                    <LoggedInTemplate>
                        <asp:LinkButton ID="btn_invoice" runat="server" PostBackUrl="~/checkout.aspx?method=invoice" >Send Order</asp:LinkButton>
                        &nbsp;&nbsp;
                        <asp:LinkButton ID="btn_card" runat="server">Pay by Card</asp:LinkButton>                         
                    </LoggedInTemplate>
                    <AnonymousTemplate>
                        Please <a href="login.aspx?redirect=basket.aspx">Log in</a> or <a href="register.aspx">Sign up</a> to place an order.
                    </AnonymousTemplate>
                </asp:LoginView>                                                    
            </td>
            <td style="vertical-align: top; text-align: right" 
                align="right">
                &nbsp;</td>
        </tr>    
    </table>
    <br />
</asp:Content>

