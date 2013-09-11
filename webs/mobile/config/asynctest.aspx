<%@ Page Language="VB" MasterPageFile="SoapService.master" AutoEventWireup="false" CodeFile="asynctest.aspx.vb" Inherits="config_asynctest" title="Priority Mobile - Console" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">    

    <script type="text/javascript">function init(){CallServer('sysinfo:', null);}</script>
    <input type="button" value="Test Loading" 
    onclick="CallServer('selftest:', null)" id="Button1"/>    
           
    <input type="button" value="Refresh Environments" 
    onclick="CallServer('inetpub:', null)" id="Button2"/> 
    
    <input type="button" value="Clear Console" 
    onclick="document.getElementById('ctl00_ContentPlaceHolder1_txterror').value = ''; CallServer('sysinfo:', null);" id="Button3"/> 
        
    <br/>

    <asp:TextBox ID="txterror" runat="server" BackColor="Black" BorderColor="#E0E0E0"
        Font-Names="Lucida Console" Font-Size="Small" ForeColor="Lime" Height="326px"
        ReadOnly="True" TextMode="MultiLine" Width="600px"></asp:TextBox>
        
</asp:Content>

