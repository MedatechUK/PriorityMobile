Imports System.Xml
Imports CPCL
Imports PriorityMobile

Public Class ctrl_Credit
    Inherits iView

#Region "Initialisation and Finalisation"

    Public Overrides ReadOnly Property MyControls() As ControlCollection
        Get
            Return Me.Controls
        End Get
    End Property

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

        With Me
            With ListSort1
                .Sort = "name"
                .AddColumn("ordi", "ordi", 0, True)
                .AddColumn("name", "Part", 150)
                .AddColumn("des", "Description", 200)
                .AddColumn("qty", "Qty", 60)
                .AddColumn("unitprice", "Price", 130, , eColumnFormat.fmt_Money)
                .AddColumn("rcvdqty", "Received", 130)
                .AddColumn("reason", "Reason", 250)
                .AddColumn("ivnum", "Invoice No", 150)
            End With
        End With

    End Sub

#End Region

#Region "Overides Base Properties"

    'Public Overrides ReadOnly Property ButtomImage() As String
    '    Get
    '        Return "calendar.bmp"
    '    End Get
    'End Property

    Public Overrides ReadOnly Property Selected() As String
        Get
            Return ListSort1.Selected
        End Get
    End Property

#End Region

#Region "Overrides base Methods"

    Public Overrides Sub Bind() Handles ListSort1.Bind

        IsBinding = True
        Dim dr() As Data.DataRow = Nothing
        Dim query As String = String.Format( _
                "{0} <> '0'", _
                ListSort1.Keys(0) _
                )
        dr = thisForm.Datasource.Select(query, ListSort1.Sort)

        With ListSort1
            .Clear()
            For Each r As System.Data.DataRow In dr
                .AddRow(r)
                If .RowSelected(r, thisForm.CurrentRow) Then
                    .Items(.Items.Count - 1).Selected = True
                    .Items(.Items.Count - 1).Focused = True
                Else
                    .Items(.Items.Count - 1).Selected = False
                    .Items(.Items.Count - 1).Focused = False
                End If
            Next
            .Focus()
        End With

        thisForm.RefreshSubForms()
        IsBinding = False

    End Sub

    Public Overrides Sub SetFocus()
        Me.ListSort1.Focus()
    End Sub

    Public Overrides Sub ViewChanged()
        Bind()
    End Sub

    Public Overrides Function SubFormVisible(ByVal Name As String) As Boolean
        With Me
            If IsNothing(.Selected) Then Return False
            If IsNothing(.thisForm.TableData.Current) Then Return False
            If IsNothing(ListSort1.Selected) Then Return False
            Select Case Name.ToUpper
                Case Else
                    Return True
            End Select
        End With
    End Function

#End Region

#Region "Local control Handlers"

    Private Sub hSelectedindexChanged(ByVal row As Integer) Handles ListSort1.SelectedIndexChanged
        If IsBinding Then Exit Sub
        With thisForm
            Dim cur As String = ListSort1.Value(ListSort1.Keys(0), row)
            If Not String.Compare(Text, cur) = 0 Then
                .TableData.Position = .TableData.Find(ListSort1.Keys(0), cur)
                .RefreshSubForms()
                .RefreshDirectActivations()
            End If
        End With
    End Sub

    Private Sub hItemSelect() Handles ListSort1.ItemSelect
        With thisForm
            If Not IsNothing(.TableData.Current) Then
                .CurrentView += 1
                .RefreshForm()
            End If
        End With
    End Sub

#End Region

#Region "Direct Activations"

    Public Overrides Sub DirectActivations(ByRef ToolBar As daToolbar)
        ToolBar.Add(AddressOf hManualCredit, "ADD.BMP", True)
        ToolBar.Add(AddressOf hPrint, "print.BMP", ListSort1.Items.Count > 0)
        ToolBar.Add(AddressOf hDelCredit, "DELETE.BMP", Not ListSort1.SelectedIndex = -1)
    End Sub

    Private Sub hDelCredit()
        If MsgBox("Delete this credit?", MsgBoxStyle.OkCancel) = MsgBoxResult.Ok Then
            With thisForm
                Dim iv As XmlNode = .FormData.SelectSingleNode(.boundxPath).ParentNode.ParentNode.ParentNode.SelectSingleNode(String.Format("invoices/invoice[ivnum='{0}']", .CurrentRow("ivnum")))
                If Not IsNothing(iv) Then
                    Dim part As XmlNode = iv.SelectSingleNode(String.Format(".//part[ordi='{0}']", .CurrentRow("ordi")))
                    part.SelectSingleNode("qty").InnerText = CDbl(part.SelectSingleNode("qty").InnerText) + CDbl(.CurrentRow("qty"))
                    .FormData.SelectSingleNode(.boundxPath).ParentNode.RemoveChild(.FormData.SelectSingleNode(.boundxPath).ParentNode.SelectSingleNode(String.Format("part[ordi='{0}' and reason='{1}']", .CurrentRow("ordi"), .CurrentRow("reason"))))

                    Dim total As Double = 0
                    For Each ivpart As XmlNode In iv.SelectNodes(".//part")
                        total += CDbl(ivpart.SelectSingleNode("qty").InnerText) * CDbl(ivpart.SelectSingleNode("unitprice").InnerText)
                    Next

                    iv.SelectSingleNode("total").InnerText = total.ToString
                Else
                    .FormData.SelectSingleNode(.boundxPath).ParentNode.RemoveChild(.FormData.SelectSingleNode(.boundxPath).ParentNode.SelectSingleNode(String.Format("part[ordi='{0}' and reason='{1}']", .CurrentRow("ordi"), .CurrentRow("reason"))))
                End If

                .Save()
                .Bind()
                .RefreshForm()
            End With
        End If

    End Sub

    Private Sub hManualCredit()
        Dim ManualCredit As New dlgManualCredit
        With ManualCredit
            Dim CreditReason As ComboBox = .FindControl("CreditReason")
            With CreditReason
                With .Items
                    .Clear()
                    .Add("Please Select")
                    For Each reason As XmlNode In thisForm.FormData.SelectSingleNode("pdadata/reasons/credit").SelectNodes(".//reason")
                        .Add(reason.InnerText)
                    Next
                End With
                .SelectedIndex = 0
            End With
        End With
        thisForm.Dialog(ManualCredit)
    End Sub

    Public Overrides Sub CloseDialog(ByVal frmDialog As PriorityMobile.UserDialog)
        Dim unitprice As TextBox = frmDialog.FindControl("CreditAmount")
        Dim reason As ComboBox = frmDialog.FindControl("CreditReason")
        With thisForm
            AddCreditNote(thisForm, _
                .FormData.SelectSingleNode(.boundxPath).ParentNode.ParentNode, _
                "-1", _
                "000", _
                "000", _
                "Manual Credit", _
                "1", _
                unitprice.Text, _
                "0", _
                reason.SelectedItem _
             )
            .Bind()
            .RefreshForm()
        End With
    End Sub

    Private Sub hPrint()
        With thisForm.Printer
            If Not .Connected Then
                .BeginConnect(thisForm.MACAddress)
                Do While .WaitConnect
                    Threading.Thread.Sleep(100)
                Loop
            End If
            If .Connected Then
                PrintForm()
            End If
        End With
    End Sub

    Public Overrides Sub PrintForm()
        Dim headerFont As New PrinterFont(50, 5, 2) 'variable width. 
        Dim largeFont As New PrinterFont(30, 0, 3) '16 
        Dim smallFont As New PrinterFont(35, 0, 2) '8 

        With thisForm
            Using lblCreditNote As New Label(thisForm.Printer, eLabelStyle.receipt)

                Dim cn As XmlNode = .FormData.SelectSingleNode(.boundxPath).ParentNode

                'first receipt formatter
                'taking margins into account the receipt is 556px wide. 
                'font 0 - 8/16 for size 2/3 respectively, font 5 is variable width ~20-30.

                Dim cnDelivery As String = String.Format("{0}-{1}-{2}", cn.ParentNode.ParentNode.ParentNode.ParentNode.ParentNode.SelectSingleNode("curdate").InnerText, _
                                         cn.ParentNode.ParentNode.ParentNode.ParentNode.ParentNode.SelectSingleNode("routenumber").InnerText, _
                                         cn.ParentNode.ParentNode.ParentNode.SelectSingleNode("ordinal").InnerText)


                Dim cnDate As String = Now.ToString("dd/MM/yy")
                Dim cnTime As String = Now.ToString("HH:mm")
                Dim van As String = cn.ParentNode.ParentNode.ParentNode.ParentNode.ParentNode.SelectSingleNode("vehiclereg").InnerText

                Dim docHead As New ReceiptFormatter(64, _
                                                    New FormattedColumn(16, 0, eAlignment.Center), _
                                                    New FormattedColumn(16, 16, eAlignment.Center), _
                                                    New FormattedColumn(16, 32, eAlignment.Center), _
                                                    New FormattedColumn(16, 48, eAlignment.Center))
                docHead.AddRow("Invoice", "Date", "Time", "Van")
                docHead.AddRow(cnDelivery, cnDate, cnTime, van)


                '### customer header### 
                Dim cnCustNum As String = cn.ParentNode.ParentNode.SelectSingleNode("custnumber").InnerText.ToUpper
                Dim cnCustName As String = cn.ParentNode.ParentNode.SelectSingleNode("custname").InnerText
                Dim cnCustPostCode As String = cn.ParentNode.ParentNode.SelectSingleNode("postcode").InnerText.ToUpper

                Dim custDetails As New ReceiptFormatter(64, _
                                                        New FormattedColumn(13, 0, eAlignment.Right), _
                                                        New FormattedColumn(48, 16, eAlignment.Left))
                custDetails.AddRow("Customer:", cnCustNum)
                custDetails.AddRow("", cnCustName)
                custDetails.AddRow("", cnCustPostCode)



                Dim lines As Integer
                Dim units As Integer
                Dim cnPartsList As New ReceiptFormatter(64, _
                                                      New FormattedColumn(10, 0, eAlignment.Left), _
                                                      New FormattedColumn(33, 11, eAlignment.Left), _
                                                      New FormattedColumn(7, 44, eAlignment.Right), _
                                                      New FormattedColumn(13, 51, eAlignment.Right))
                cnPartsList.AddRow("Invoice:", "Description:", "Qty:", "Returned:")
                Dim cnTotal As Double
                For Each line As XmlNode In cn.SelectNodes("part[qty>0]")
                    Dim cnInv As String = line.SelectSingleNode("ivnum").InnerText.ToUpper
                    Dim cnPartName As String = line.SelectSingleNode("des").InnerText
                    Dim cnQty As String = line.SelectSingleNode("qty").InnerText
                    Dim cnRcvd As String = line.SelectSingleNode("rcvdqty").InnerText
                    Dim cnPrice As String = line.SelectSingleNode("unitprice").InnerText
                    Dim cnPartReason As String = line.SelectSingleNode("reason").InnerText
                    cnPartsList.AddRow(cnInv, cnPartName, cnQty, cnRcvd)
                    cnPartsList.AddRow("", cnPartReason, "", (CDbl(cnPrice) * CDbl(cnQty)).ToString("c").Replace("£", "#"))

                    cnTotal = cnTotal + CDbl(cnQty) * CDbl(cnPrice)
                    lines += 1
                    units += CInt(cnQty)
                Next




                Dim total As New ReceiptFormatter(64, _
                                                  New FormattedColumn(6, 10, eAlignment.Right), _
                                                  New FormattedColumn(47, 16, eAlignment.Right))


                total.AddRow("Total:", cnTotal.ToString("c").Replace("£", "#"))


                With lblCreditNote
                    'logo
                    .AddImage("roddas.pcx", New Point(186, thisForm.Printer.Dimensions.Height + 10), 147)

                    'line
                    .AddLine(New Point(10, thisForm.Printer.Dimensions.Height + 10), _
                             New Point(thisForm.Printer.Dimensions.Width - 10, thisForm.Printer.Dimensions.Height + 10), 10, 15)

                    'header = 174px wide
                    .AddText("CREDIT NOTE", New Point((thisForm.Printer.Dimensions.Width / 2) - 138, thisForm.Printer.Dimensions.Height + 10), _
                             headerFont)

                    'line
                    .AddLine(New Point(10, thisForm.Printer.Dimensions.Height + 10), _
                             New Point(thisForm.Printer.Dimensions.Width - 10, thisForm.Printer.Dimensions.Height + 10), 10)

                    'address
                    .AddMultiLine("A.E. Rodda & Son Ltd." & vbCrLf & "The Creamery" & vbCrLf & "Scorrier" _
                                    & vbCrLf & "Redruth" & vbCrLf & "Cornwall" & vbCrLf & "TR165BU", _
                                                 New Point(10, thisForm.Printer.Dimensions.Height + 10), largeFont, 30)
                    'line
                    .AddLine(New Point(10, thisForm.Printer.Dimensions.Height + 10), _
                             New Point(thisForm.Printer.Dimensions.Width - 10, thisForm.Printer.Dimensions.Height + 10), 2)

                    'document header 
                    For Each StrVal In docHead.FormattedText
                        .AddText(StrVal, New Point(22, thisForm.Printer.Dimensions.Height), smallFont)
                    Next

                    'line
                    .AddLine(New Point(10, thisForm.Printer.Dimensions.Height + 10), _
                             New Point(thisForm.Printer.Dimensions.Width - 10, thisForm.Printer.Dimensions.Height + 10), 2)

                    'customer details 
                    For Each StrVal In custDetails.FormattedText
                        .AddText(StrVal, New Point(22, thisForm.Printer.Dimensions.Height), smallFont)
                    Next

                    'line
                    .AddLine(New Point(10, thisForm.Printer.Dimensions.Height + 10), _
                             New Point(thisForm.Printer.Dimensions.Width - 10, thisForm.Printer.Dimensions.Height + 10), 2)

                    'itemised invoice box
                    For Each StrVal In cnPartsList.FormattedText
                        .AddText(StrVal, New Point(22, thisForm.Printer.Dimensions.Height), smallFont)
                    Next
                    ' .AddText(

                    'line
                    .AddLine(New Point(10, thisForm.Printer.Dimensions.Height + 10), _
                             New Point(thisForm.Printer.Dimensions.Width - 10, thisForm.Printer.Dimensions.Height + 10), 5)

                    'total 
                    For Each StrVal In total.FormattedText
                        .AddText(StrVal, New Point(22, thisForm.Printer.Dimensions.Height), smallFont)
                    Next

                    'line
                    .AddLine(New Point(10, thisForm.Printer.Dimensions.Height + 10), _
                             New Point(thisForm.Printer.Dimensions.Width - 10, thisForm.Printer.Dimensions.Height + 10), 5)

                    'itemisation
                    Dim totals As String = String.Format(" ( {0} lines {1} units ) ", lines, units)
                    .AddText(totals, New Point((thisForm.Printer.Dimensions.Width / 2 - (totals.Length / 2) * 16), _
                                               thisForm.Printer.Dimensions.Height + 10), largeFont)

                    'line
                    .AddLine(New Point(10, thisForm.Printer.Dimensions.Height + 20), _
                             New Point(thisForm.Printer.Dimensions.Width - 10, thisForm.Printer.Dimensions.Height + 20), 5)

                    'vat number 
                    Dim vat As String = "V.A.T. No.  131 7759 63"
                    .AddText(vat, New Point((thisForm.Printer.Dimensions.Width / 2 - (vat.Length / 2) * 16), _
                                               thisForm.Printer.Dimensions.Height + 10), largeFont)

                    'For any remittance.... 
                    .AddMultiLine("For any remittance queries please contact" & vbCrLf & "accounts@roddas.co.uk".PadLeft(32, " "), _
                                  New Point(thisForm.Printer.Dimensions.Width / 2 - 168, thisForm.Printer.Dimensions.Height + 10), smallFont, 30)


                    'line
                    .AddLine(New Point(10, thisForm.Printer.Dimensions.Height + 10), _
                             New Point(thisForm.Printer.Dimensions.Width - 10, thisForm.Printer.Dimensions.Height + 10), 10)

                    'please quote
                    .AddText("Please quote account number in all correspondence.", _
                             New Point(thisForm.Printer.Dimensions.Width / 2 - 200, _
                                       thisForm.Printer.Dimensions.Height + 10), _
                             smallFont)

                    'tear 'n' print!
                    .AddTearArea(New Point(0, thisForm.Printer.Dimensions.Height))
                    thisForm.Printer.Print(.toByte)


                End With
            End Using
        End With
    End Sub

#End Region

End Class
