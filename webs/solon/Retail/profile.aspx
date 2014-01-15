<%@ Page Language="VB" MasterPageFile="~/membership.master" AutoEventWireup="false" CodeFile="profile.aspx.vb" Inherits="profile" title="Untitled Page" validateRequest="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" Runat="Server">
    <TABLE>
<tr><td valign="top" style="text-align: right">First Name</td><td align="left" valign="top">
                            <asp:TextBox ID="Name_First" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" 
                                ControlToValidate="Name_First" ErrorMessage="* Please enter your name"></asp:RequiredFieldValidator>
 </td></tr>
<tr><td valign="top" style="text-align: right">Last Name</td><td align="left" valign="top">
                            <asp:TextBox ID="Name_Last" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" 
                                ControlToValidate="Name_Last" ErrorMessage="* Please enter your surname"></asp:RequiredFieldValidator>
 </td></tr>
<tr><td valign="top" style="text-align: right">&nbsp;</td><td align="left" valign="top">
    &nbsp;</td></tr>
<tr><td valign="top" style="text-align: right">Address:</td><td align="left" valign="top">
 <asp:TextBox ID="Address_Address1" runat="server" Width="50%"></asp:TextBox>
                             <span style="color: #FF0000">* </span>
                             <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                                 ControlToValidate="Address_Address1" ErrorMessage="Missing"></asp:RequiredFieldValidator>
                             <br />
 </td></tr>
<tr><td valign="top" style="text-align: right">&nbsp;</td><td align="left" valign="top">
 <asp:TextBox ID="Address_Address2" runat="server" Width="50%"></asp:TextBox>
 </td></tr>
<tr><td valign="top" style="text-align: right">Town:</td><td align="left" valign="top">
 <asp:TextBox ID="Address_Address3" runat="server" Width="50%"></asp:TextBox>
                             <span style="color: #FF0000">* </span>
                             <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                                 ControlToValidate="Address_Address3" ErrorMessage="Missing"></asp:RequiredFieldValidator>
 </td></tr>
<tr><td valign="top" style="text-align: right">County:</td><td align="left" valign="top">
 <asp:TextBox ID="Address_Address4" runat="server" Width="50%"></asp:TextBox>
                             <span style="color: #FF0000">* </span>
                             <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
                                 ControlToValidate="Address_Address4" ErrorMessage="Missing"></asp:RequiredFieldValidator>
 </td></tr>
<tr><td valign="top" style="text-align: right">Post Code:</td><td align="left" valign="top">
 <asp:TextBox ID="Address_Postcode" runat="server"></asp:TextBox>
                             <span style="color: #FF0000">* </span>
                             <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
                                 ControlToValidate="Address_Postcode" ErrorMessage="Not a valid postcode." 
                                 ValidationExpression="^((([A-PR-UWYZ])([0-9][0-9A-HJKS-UW]?))|(([A-PR-UWYZ][A-HK-Y])([0-9][0-9ABEHMNPRV-Y]?))\s{0,2}(([0-9])([ABD-HJLNP-UW-Z])([ABD-HJLNP-UW-Z])))|(((GI)(R))\s{0,2}((0)(A)(A)))$"></asp:RegularExpressionValidator>
                             <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" 
                                 ControlToValidate="Address_Postcode" ErrorMessage="Missing"></asp:RequiredFieldValidator>
 </td></tr>
<tr><td valign="top" style="text-align: right">&nbsp;</td><td align="left" valign="top">
    &nbsp;</td></tr>
<tr><td valign="top" style="text-align: right">&nbsp;</td><td align="left" valign="top">
                             <asp:Button ID="btnSaveProfile" runat="server" Text="Save Changes"/>
 </td></tr>
<tr><td valign="top" style="text-align: right">&nbsp;</td><td align="left" valign="top">
    &nbsp;</td></tr>
<tr><td valign="top" style="text-align: right">&nbsp;</td><td align="left" valign="top">
    <span style="color: #FF0000">* Required field.</span></td></tr>
</table>
<BR/>
</asp:Content>
