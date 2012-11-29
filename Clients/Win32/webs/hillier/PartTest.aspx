<%@ Page Language="VB" MasterPageFile="~/Masterpages/default.master" AutoEventWireup="false" CodeFile="PartTest.aspx.vb" Inherits="PartTest" title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <strong>
    
    <hr/>Currency Select is placed on the page with the HTML:</strong><br />&lt;PRIORITY:CURRENCY runat=&quot;server&quot; ID=&quot;currency&quot; /&gt;
    <hr/>
    <strong></strong>
    <br />
    <br />
    <br />
    <PRIORITY:CURRENCY runat="server" ID="currency" />    &nbsp;<br />
    <br />
    <strong>
        <br /><hr/>
        Part is placed on the page with HTML:</strong><br/>&lt;PRIORITY:PART runat=&quot;server&quot; ID=&quot;MyPart&quot; PARTNAME=&quot;111-001&quot;  /&gt;
    <hr/>
    <strong></strong>
    <br />
    <br />
    <br />
    <PRIORITY:PART runat="server" ID="MyPart" PARTNAME="111-001"  />
    <br />
    <br />
    <strong>
        <br /><hr/>
        &nbsp;Part is placed on the page with HTML:</strong><br/>&lt;PRIORITY:PART runat=&quot;server&quot; ID=&quot;PART1&quot; PARTNAME=&quot;111-002&quot; /&gt;
    <hr/>
    <strong></strong>
    <br />
    <br />
    <br />
    <PRIORITY:PART runat="server" ID="PART1" PARTNAME="111-002" />
    <br />
    <br />
    <strong>
        <br /><hr/>
        &nbsp;<br />
    This part was added programatically with the code:</strong>
    <br />
    &#39; Load a part control programatically
    <br />
        Dim cph As ContentPlaceHolder = Me.GetPH
    <br />
        If Not IsNothing(cph) Then
    <br />
    &nbsp;&nbsp;
            Dim c As Object = LoadControl(&quot;~/controls/partTemplate.ascx&quot;)
    <br />
    &nbsp;&nbsp; With c
    <br />
    &nbsp; &nbsp; &nbsp;
                .ID = &quot;newpart&quot;
    <br />
    &nbsp; &nbsp; &nbsp;
                .PARTNAME = &quot;111-003&quot;
    <br />
    &nbsp;&nbsp; End With
    <br />
    &nbsp;&nbsp;
            cph.Controls.Add(c)
    <br />
    End If
    <hr/>
    <strong></strong>
    <br />
    <br />
</asp:Content>

