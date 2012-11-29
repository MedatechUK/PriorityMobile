<%@ Control Language="VB" AutoEventWireup="false" CodeFile="PriCart.ascx.vb" Inherits="controls_Basket" %>
    <asp:GridView ID="BasketGrid" runat="server" AutoGenerateColumns="False"
        CellPadding="4" ForeColor="#333333" GridLines="None" Width="586px" DataKeyNames="PARTNAME" ShowFooter="True">
        <Columns >
             <asp:TemplateField visible="False" >
                <ItemTemplate>
                    <%#Eval("PARTNAME")%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Part">
                <ItemTemplate>
                    <asp:HyperLink ID="PARTNAME" runat="server" NavigateUrl='<%#Eval("REFERER")%>' Text='<%#Eval("PARTNAME")%>'></asp:HyperLink>
                </ItemTemplate>
                <HeaderStyle HorizontalAlign="Left" />
                <ItemStyle HorizontalAlign="Left" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Description">
                <ItemTemplate>
            <%#Eval("PARTDES")%>
                </ItemTemplate>
                <HeaderStyle HorizontalAlign="Left" />
                <ItemStyle HorizontalAlign="Left" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Price">
                <ItemTemplate>
            <%#Eval("PARTPRICE")%>
                </ItemTemplate>                
                <HeaderStyle HorizontalAlign="Left" />
                <ItemStyle HorizontalAlign="Left" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="QTY">
                <ItemTemplate>
            <%#Eval("QTY")%>
                </ItemTemplate>                
                <HeaderStyle HorizontalAlign="Left" />
                <ItemStyle HorizontalAlign="Left" />
                <EditItemTemplate>
                    &nbsp;
                </EditItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Tax%">
                <ItemTemplate>
            <%#Eval("SALESTAX")%>
                </ItemTemplate>
                <HeaderStyle HorizontalAlign="Left" />
                <ItemStyle HorizontalAlign="Left" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Line Total">
                <ItemTemplate>
            <%#Eval("LINETOTAL")%>
                </ItemTemplate>                
                <HeaderStyle HorizontalAlign="Left" />
                <ItemStyle HorizontalAlign="Left" />
            </asp:TemplateField>
            <asp:CommandField ShowDeleteButton="True" ShowEditButton="True" ShowCancelButton="False" />
        </Columns>
        <RowStyle BackColor="#EFF3FB" />
        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
        <EditRowStyle BackColor="#2461BF" />
        <AlternatingRowStyle BackColor="White" />
        <EmptyDataTemplate>
            There are currently no items in your basket.
        </EmptyDataTemplate>
    </asp:GridView>