<%@ Page Language="VB" MasterPageFile="SoapService.master" AutoEventWireup="false" CodeFile="SelfTest.aspx.vb" Inherits="config_SelfTest" title="Priority Mobile - Install Checklist" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <script type="text/javascript">
        function init()
        {            
        }
    </script>
    <div style="text-align: left">
        <table cellpadding="20" style="font-size: small; width: 600px; font-family: Verdana">
            <tr>
                <td style="width: 10%; height: 9px;">
                    <asp:Button ID="Button1" runat="server" Text="Test Configuration" /></td>
                <td colspan="4" style="height: 9px">
                    &nbsp;<asp:Label ID="txtErrDes" runat="server" ForeColor="Red" Height="19px" Width="337px"></asp:Label></td>
            </tr>
            <tr>
                <td colspan="5" rowspan="2">
                    <table border="0" cellpadding="10">
                        <tr>
                            <td style="width: 398px">
                                Application name exists in the config file.</td>
                            <td style="width: 100px">
                            <asp:CheckBox ID="er4" runat="server" />
                            </td>
                        </tr>                    
                        <tr>
                            <td style="width: 398px">
                                Windows event log has been registered for the apllication.</td>
                            <td style="width: 100px">
                                <asp:CheckBox ID="er1" runat="server" /></td>
                        </tr>
                        <tr>
                        </tr>

                        <tr>
                            <td style="width: 398px">
                                Priority Environment specified in the web.config.</td>
                            <td style="width: 100px">
                            <asp:CheckBox ID="er3" runat="server" />
                            </td>
                        </tr>    
                            <td style="width: 398px">
                                Environment has a valid connection string.</td>
                            <td style="width: 100px">
                            <asp:CheckBox ID="er2" runat="server" />
                            </td>
                        <tr>
                            <td style="width: 398px">
                                Attempt to open the connection string.</td>
                            <td style="width: 100px">
                            <asp:CheckBox ID="er5" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 398px">
                                Badmail path is writable.</td>
                            <td style="width: 100px">
                            <asp:CheckBox ID="er201" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 398px">
                                Signature path is writable.</td>
                            <td style="width: 100px">
                            <asp:CheckBox ID="er202" runat="server" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
            </tr>
        </table>
    </div>
</asp:Content>

