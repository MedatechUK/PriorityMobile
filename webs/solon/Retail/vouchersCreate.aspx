<%@ Page Language="VB" AutoEventWireup="false" CodeFile="vouchersCreate.aspx.vb"
    Inherits="VouchersCreate" MasterPageFile="~/membership.master" Title="VouchersCreate" ValidateRequest="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Main" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    
    <div class="voucherMain">
        <asp:Label ID="lblVoucherMessage" runat="server" Visible="false"></asp:Label>
        
        <asp:UpdatePanel ID="uPnlVType" runat="server">            
            <contenttemplate>
                <span class="voucherType">
                    <h2>Voucher Type</h2>
                        Select the voucher type for this voucher. 
                    <br /> 
                    <br /> 
                    <span id="vouchertypeselect">
                        <asp:Listbox ID="voucherType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="voucherType_SelectedIndexChanged"></asp:Listbox>
                    </span>   
                    <span id="voucherlongdes">
                        <asp:Label ID="VoucherLongDes" runat="server" ></asp:Label>
                    </span>
                    <asp:RequiredFieldValidator ID="rfvVtype" runat="server" 
                    ControlToValidate="voucherType" ErrorMessage="Required Field"></asp:RequiredFieldValidator>
                    <br /> 
                    <br /> 
                    <asp:Button ID="submitType" runat="server" Text="Proceed" cssClass="submitType"/>
                </span>
            </contenttemplate>            
        </asp:UpdatePanel>
        
        <asp:Panel ID="pnlVoucherBasic" runat="server" Visible="False">
            <span class="voucherDetail1">
                
                <asp:Label ID="lblVoucherCode" runat="server">
                    <h2>Voucher Code</h2>
                    Enter the code for this voucher to be used with.
                    <br />  <br /> 
                </asp:Label>
                <asp:TextBox ID="txtVoucherCode" runat="server"></asp:TextBox>
                
                <asp:RequiredFieldValidator ID="rfvVcode" runat="server" 
                ControlToValidate="txtVoucherCode" ErrorMessage="Required Field"></asp:RequiredFieldValidator>
                
                <asp:Label ID="lblVoucherDes" runat="server">
                    <h2>Voucher Description</h2>
                    Enter a description for this voucher.
                    <br />  <br /> 
                </asp:Label>
                <asp:TextBox ID="txtVoucherDes" runat="server"></asp:TextBox>
                
                <asp:RequiredFieldValidator ID="rfvVoucherDes" runat="server" 
                ControlToValidate="txtVoucherDes" ErrorMessage="Required Field"></asp:RequiredFieldValidator>
                
                <asp:Label ID="lblVoucherExpiry" runat="server">
                    <h2>Voucher Expiry</h2>
                    Enter an expiry date for this voucher.
                    <br /><br />
                </asp:Label>
                <asp:Calendar ID="cdrVoucherExpiry" runat="server"></asp:Calendar>
            </span>
            <br />
        </asp:Panel>
        
        <asp:Panel ID="pnlSpendVoucher" runat="server" Visible="false"> 
            <asp:Label ID="lblVoucherSpend" runat="server">
                <h2>Optional Spend</h2>
                Enter a minimum spend amount to qualify for this voucher (optional).
                <br />  <br /> 
            </asp:Label>
            <asp:TextBox ID="txtVoucherSpend" runat="server"></asp:TextBox>
            
            <asp:Label ID="lblVoucherDiscount" runat="server">
                <h2>Discount Amount</h2>
                Enter a discount amount for this voucher.
                <br />  <br /> 
            </asp:Label>
            <asp:TextBox ID="txtVoucherDiscount" runat="server"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rfvVoucherDiscount" runat="server" 
                ControlToValidate="txtVoucherDiscount" ErrorMessage="Required Field"></asp:RequiredFieldValidator>
        </asp:Panel>
        
        <asp:Panel ID="pnlBuyParts" runat="server" Visible="false">
            <span class="buypartsLeft">
                <asp:Label ID="lblVoucherBuyParts" runat="server">
                    <h2>Buy Parts</h2>
                    Select parts to which the new voucher will apply.  <br /> 
                    ctrl+click should be used to select multiple items. 
                    <br />  <br />
                </asp:Label>
                <asp:RequiredFieldValidator ID="rfvBuyParts" runat="server" 
                ControlToValidate="buyParts" ErrorMessage="Required Field"></asp:RequiredFieldValidator>
                <asp:ListBox ID="buyParts" runat="server" Height="430px" SelectionMode="Multiple"
                                                    Width="477px"></asp:ListBox>
            </span>
            <span class="buypartsRight">
                <asp:Label ID="lblVoucherBuyQty" runat="server" Visible="False">
                    <br /><br /><br /><br /><br />
                    <h2> Buy Qty</h2>
                    Enter a quantity of purchased parts required to trigger voucher.
                    <br />  <br />
                </asp:Label>
                <asp:TextBox ID="txtVoucherBuyQty" runat="server" Width="43px" Visible="False"></asp:TextBox>
            </span>
            <asp:RequiredFieldValidator ID="rfvVoucherBuyQty" runat="server" 
                ControlToValidate="txtVoucherBuyQty" ErrorMessage="Required Field"></asp:RequiredFieldValidator>
            <br />
        </asp:Panel>
        
        <asp:Panel ID="pnlGetParts" runat="server" Visible ="False"> 
            <span class="buyPartsLeft"> 
                <asp:Label ID="lblVoucherGetParts" runat="server"> 
                <br /><br /><br /><br />
                    <h2>Get Parts</h2>
                    Enter a quantity of purchased parts that the discount may apply to.
                    <br /> <br /> 
                </asp:Label>
                <asp:Textbox ID="txtVoucherGetQty" runat="server" Width="43px" Visible="true"> </asp:Textbox>
            </span>
            
            <asp:RequiredFieldValidator ID="rfvGetQty" runat="server" 
                ControlToValidate="txtVoucherGetQty" ErrorMessage="Required Field"></asp:RequiredFieldValidator>
            
        </asp:Panel>
        <span class="endVoucher">
            <asp:Button ID="btnBack" runat="server" Text="Back" Style="" Visible="True" /> &nbsp;
            <asp:Button ID="addVoucher" runat="server" Text="Submit" Style="" Visible="False" />
            <asp:Button ID="LinkSaveVoucher" runat="server" Text="Submit" Style="" Visible="false" /> 
            <br />
        </span>
    </div>
</asp:Content>
