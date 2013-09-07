Imports CPCL
Imports System.Xml
Imports PriorityMobile

Public Class ctrl_Deliveries
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
                .FormLabel = "Delivieries"
                .Sort = "ordinal"
                .AddColumn("ordinal", "ordinal", 0, True)
                .AddColumn("custnumber", "Customer", 260)
                .AddColumn("postcode", "Post Code", 130)
                .AddColumn("sonum", "Sales Order", 200)
            End With
        End With

    End Sub

    Public Overrides Sub FormClosing()
        MyBase.FormClosing()
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
                "{0} <> '0' and Delivered = 'N'", _
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

        If xmlForms.SetNodeChanged("pdadata/home") Or _
            xmlForms.SetNodeChanged("pdadata/warehouse") Then thisForm.Save()

        ''Dim doSave As Boolean = False
        'With xmlForms.FormData.Document
        '    If IsNothing(.SelectSingleNode("pdadata/home").Attributes.GetNamedItem("changed")) Then
        '        .SelectSingleNode("pdadata/home").Attributes.Append(xmlForms.changedAttribute)
        '        'doSave = True
        '        thisForm.Save()
        '    End If
        '    If IsNothing(.SelectSingleNode("pdadata/warehouse").Attributes.GetNamedItem("changed")) Then
        '        .SelectSingleNode("pdadata/warehouse").Attributes.Append(xmlForms.changedAttribute)
        '        'doSave = True
        '        thisForm.Save()
        '    End If
        '    'If doSave Then thisForm.Save()
        'End With

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
        ToolBar.Add(AddressOf hPrint, "print.BMP", Not ListSort1.SelectedIndex = -1)
        ToolBar.Add(AddressOf hCloseDelivery, "delete.BMP", Not ListSort1.SelectedIndex = -1)
    End Sub

    Private Sub hPrint()
        With thisForm.Printer
            If Not .Connected Then
                .BeginConnect(thisForm.MACAddress)
                Do While .WaitConnect
                    Threading.Thread.Sleep(100)
                Loop
            End If
            If .Connected Then PrintForm()
        End With
    End Sub

    Public Overrides Sub PrintForm()
        Dim dv As XmlNode

        With thisForm
            dv = .FormData.SelectSingleNode(String.Format("{0}[ordinal='{1}']", .boundxPath, ListSort1.Selected))


            Dim headerFont As New PrinterFont(50, 5, 2) 'variable width. 
            Dim largeFont As New PrinterFont(30, 0, 3)
            Dim smallFont As New PrinterFont(35, 0, 2)

            Using lblDeliveryNote As New Label(thisForm.Printer, eLabelStyle.receipt)

                'first receipt formatter
                'taking margins into account the receipt is 556px wide. At size 2, char width is 8px. 

                '#####delivery header####

                Dim dvNum As String = String.Format("{0}-{1}-{2}", dv.ParentNode.ParentNode.SelectSingleNode("curdate").InnerText, _
                                    dv.ParentNode.ParentNode.SelectSingleNode("routenumber").InnerText, _
                                    dv.SelectSingleNode("ordinal").InnerText)
                Dim dvDate As String = Now.ToString("dd/MM/yy")
                Dim dvTime As String = Now.ToString("hh:mm")
                Dim van As String = dv.ParentNode.ParentNode.SelectSingleNode("vehiclereg").InnerText.ToUpper


                Dim docHead As New ReceiptFormatter(64, _
                                                    New FormattedColumn(16, 0, eAlignment.Center), _
                                                    New FormattedColumn(16, 16, eAlignment.Center), _
                                                    New FormattedColumn(16, 32, eAlignment.Center), _
                                                    New FormattedColumn(16, 48, eAlignment.Center))



                docHead.AddRow("Number", "Date", "Time", "Van")
                docHead.AddRow(dvNum, dvDate, dvTime, van)


                '####customer header####
                Dim dvCust As String = dv.SelectSingleNode("customer/custnumber").InnerText.ToUpper
                Dim dvCustName As String = dv.SelectSingleNode("customer/custname").InnerText.ToUpper
                Dim dvCustPostCode As String = dv.SelectSingleNode("customer/postcode").InnerText.ToUpper

                Dim custDetails As New ReceiptFormatter(64, _
                                                        New FormattedColumn(13, 0, eAlignment.Right), _
                                                        New FormattedColumn(48, 16, eAlignment.Left))

                custDetails.AddRow("Customer:", dvCust)
                custDetails.AddRow("", dvCustName)
                custDetails.AddRow("", dvCustPostCode)


                '#### parts list #### 
                Dim partsList As New ReceiptFormatter(64, _
                                                      New FormattedColumn(3, 0, eAlignment.Right), _
                                                      New FormattedColumn(49, 4, eAlignment.Left), _
                                                      New FormattedColumn(8, 54, eAlignment.Left))
                partsList.AddRow("No:", "Description:", "Code:")
                Dim lines As Integer
                Dim units As Integer

                For Each deliverypart As XmlNode In dv.SelectNodes("parts/part[cquant>0]")
                    Dim name As String = deliverypart.SelectSingleNode("name").InnerText
                    Dim des As String = deliverypart.SelectSingleNode("des").InnerText
                    Dim qty As String = deliverypart.SelectSingleNode("cquant").InnerText

                    partsList.AddRow(qty, des, name)

                    lines += 1
                    units += CInt(qty)

                Next


                With lblDeliveryNote
                    'logo
                    .AddImage("roddas.pcx", New Point(186, thisForm.Printer.Dimensions.Height + 10), 147)

                    'line
                    .AddLine(New Point(10, thisForm.Printer.Dimensions.Height + 10), _
                             New Point(thisForm.Printer.Dimensions.Width - 10, thisForm.Printer.Dimensions.Height + 10), 10, 15)

                    'header = 334px wide
                    .AddText("DELIVERY NOTE", New Point((thisForm.Printer.Dimensions.Width / 2) - 167, thisForm.Printer.Dimensions.Height + 10), _
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

                    'itemised parts list. 
                    For Each StrVal In partsList.FormattedText
                        .AddText(StrVal, New Point(22, thisForm.Printer.Dimensions.Height), smallFont)
                    Next

                    'vat waffle

                    'line
                    .AddLine(New Point(10, thisForm.Printer.Dimensions.Height + 10), _
                             New Point(thisForm.Printer.Dimensions.Width - 10, thisForm.Printer.Dimensions.Height + 10), 5)

                    'itemisation
                    Dim totals As String = String.Format(" ( {0} lines {1} units ) ", lines, units)  'this will, of course, be calculated.
                    .AddText(totals, New Point((thisForm.Printer.Dimensions.Width / 2 - (totals.Length / 2) * 16), _
                                               thisForm.Printer.Dimensions.Height + 10), largeFont)

                    'line
                    .AddLine(New Point(10, thisForm.Printer.Dimensions.Height + 20), _
                             New Point(thisForm.Printer.Dimensions.Width - 10, thisForm.Printer.Dimensions.Height + 20), 5)

                    'vat number 
                    Dim vat As String = "V.A.T. No.  131 7759 63"
                    .AddText(vat, New Point((thisForm.Printer.Dimensions.Width / 2 - (vat.Length / 2) * 16), _
                                               thisForm.Printer.Dimensions.Height + 10), largeFont)

                    .AddMultiLine("For any remittance queries please contact" & vbCrLf & "accounts@roddas.co.uk".PadLeft(32, " "), _
                                  New Point(thisForm.Printer.Dimensions.Width / 2 - 168, thisForm.Printer.Dimensions.Height + 10), smallFont, 30)
                    'line
                    .AddLine(New Point(10, thisForm.Printer.Dimensions.Height + 10), _
                             New Point(thisForm.Printer.Dimensions.Width - 10, thisForm.Printer.Dimensions.Height + 10), 10)
                    'please quote
                    .AddText("Please quote account number in all correspondence.", New Point(thisForm.Printer.Dimensions.Width / 2 - 200, _
                                                                                             thisForm.Printer.Dimensions.Height + 10), _
                             smallFont)

                    'tear 'n' print!
                    .AddTearArea(New Point(0, thisForm.Printer.Dimensions.Height))
                    thisForm.Printer.Print(.toByte)

                End With

            End Using
        End With
    End Sub

    Private Sub hCloseDelivery()

        Dim ex As New Exception
        Dim dlg As dlgCloseDelivery = preClose(thisForm, thisForm.FormData.SelectSingleNode(thisForm.boundxPath), ex)

        If IsNothing(ex) Then
            If Not dlg.CompleteDelivery Then
                thisForm.Dialog(dlg)
            Else
                dlg.Result = DialogResult.OK
                CloseDialog(dlg)
            End If
        Else
            MsgBox(ex.Message)
        End If

    End Sub

    Public Overrides Sub CloseDialog(ByVal frmDialog As PriorityMobile.UserDialog)

        Dim dlg As dlgCloseDelivery = frmDialog

        With thisForm
            If dlg.Result = DialogResult.Cancel Then
                .RefreshForm()
                Exit Sub

            Else
                With .FormData.SelectSingleNode(String.Format("{0}[ordinal='{1}']", .boundxPath, .CurrentRow("ordinal")))
                    .SelectSingleNode("delivered").InnerText = "Y"
                    If Not (dlg.CompleteDelivery) Then
                        Dim NDReason As ComboBox = dlg.FindControl("Reason")
                        .SelectSingleNode("nodeliveryreason").InnerText = NDReason.SelectedItem
                    End If
                End With

                If Not (dlg.NoDelivery) Then
                    If MsgBox("Print Delivery Note?", MsgBoxStyle.OkOnly) = MsgBoxResult.Ok Then
                        With thisForm.Printer
                            If Not .Connected Then
                                .BeginConnect(thisForm.MACAddress)
                                Do While .WaitConnect
                                    Threading.Thread.Sleep(100)
                                Loop
                            End If
                            If .Connected Then PrintForm()
                        End With
                    End If
                End If

                .Save()
                .Bind()

                .RefreshForm()

            End If

        End With
    End Sub

#End Region

End Class
