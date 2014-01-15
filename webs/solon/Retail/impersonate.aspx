<%@ Page Language="VB" AutoEventWireup="false" CodeFile="impersonate.aspx.vb" Inherits="members" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Membership</title>
    </head>
<body>
    <form id="form1" runat="server">
            <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
                ConnectionString="<%$ ConnectionStrings:LocalSqlServer %>" 
                
                
            SelectCommand="SELECT     dbo.aspnet_Users.UserName, dbo.v_UserPostcode.Postcode
FROM         dbo.v_UserPostcode RIGHT OUTER JOIN
                      dbo.aspnet_Users ON dbo.v_UserPostcode.UserId = dbo.aspnet_Users.UserId LEFT OUTER JOIN
                      dbo.aspnet_UsersInRoles ON dbo.aspnet_Users.UserId = dbo.aspnet_UsersInRoles.UserId
GROUP BY dbo.aspnet_Users.UserName, dbo.v_UserPostcode.Postcode
HAVING      (COUNT(dbo.aspnet_UsersInRoles.RoleId) = 0)">
            </asp:SqlDataSource>
            <br />
            Click the user you wish to impersonate.<br />
            <br />
            <asp:LoginView ID="LoginView1" runat="server">
                <RoleGroups>
                    <asp:RoleGroup Roles="impersonate">
                        <ContentTemplate>
                            <asp:GridView ID="GridView1" runat="server" AllowPaging="True" 
                                AllowSorting="True" AutoGenerateColumns="False" CellPadding="4" 
                                DataSourceID="SqlDataSource1" ForeColor="#333333" GridLines="None" 
                                style="font-family: Verdana; font-size: small">
                                <RowStyle BackColor="#EFF3FB" />
                                <Columns>
                                    <asp:HyperLinkField DataNavigateUrlFields="UserName" 
                                        DataNavigateUrlFormatString="?User={0}" DataTextField="UserName" 
                                        HeaderText="User" NavigateUrl="~/members.aspx" />
                                    <asp:BoundField DataField="Postcode" HeaderText="Post Code" />
                                </Columns>
                                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                <EditRowStyle BackColor="#2461BF" />
                                <AlternatingRowStyle BackColor="White" />
                            </asp:GridView>
                        </ContentTemplate>
                    </asp:RoleGroup>
                </RoleGroups>
                <AnonymousTemplate>
                    <div>
                    </div>
                </AnonymousTemplate>
            </asp:LoginView>
            </FORM>
            </body>
</html>
