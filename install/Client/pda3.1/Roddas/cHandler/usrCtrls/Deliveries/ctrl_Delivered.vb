Imports CPCL
Imports System.Xml
Imports PriorityMobile

Public Class ctrl_Delivered
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
                .FormLabel = "Delivered / Nearby"
                .Sort = "custnumber"
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
                "{0} <> '0' and Delivered = 'Y'", _
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
                Case "DELIVERY"
                    Return False
                Case "SIGN"
                    Return False
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
        With ToolBar
            .Add(AddressOf hClearSort, "SORTASC.BMP")
            .Add(AddressOf hPrint, "print.BMP", Not ListSort1.SelectedIndex = -1)
        End With
    End Sub

    Private Sub hClearSort()
        ListSort1.Sort = "ordinal"
        Bind()
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
        Dim dv As XmlNode
        Do
            With thisForm
                dv = .FormData.SelectSingleNode(String.Format("{0}[ordinal='{1}']", .boundxPath, ListSort1.Selected))
            End With

            If dv.SelectSingleNode("showprices").InnerText = "Y" Then
                printDelivery(dv)
            Else
                PrintInvoice(dv)
            End If
        Loop Until Not MsgBox("Print Duplicate?", MsgBoxStyle.OkCancel) = MsgBoxResult.Ok

    End Sub

    Public Sub printDelivery(ByVal dv As XmlNode)

        Dim headerFont As New PrinterFont(50, 5, 2) 'variable width. 
        Dim largeFont As New PrinterFont(30, 0, 3)
        Dim smallFont As New PrinterFont(35, 0, 2)

        Using lblDeliveryNote As New Label(thisForm.Printer, eLabelStyle.receipt)

            'first receipt formatter
            'taking margins into account the receipt is 556px wide. At size 2, char width is 8px. 

            '#####delivery header####

            Dim dvNum As String = dv.SelectSingleNode("sonum").InnerText
            'String.Format("{0}-{1}-{2}", dv.ParentNode.ParentNode.SelectSingleNode("curdate").InnerText, _
            '                    dv.ParentNode.ParentNode.SelectSingleNode("routenumber").InnerText, _
            '                    dv.SelectSingleNode("ordinal").InnerText)
            Dim dvDate As String = Now.ToString("dd/MM/yy")
            Dim dvTime As String = Now.ToString("hh:mm")
            Dim van As String = dv.ParentNode.ParentNode.SelectSingleNode("vehiclereg").InnerText.ToUpper


            Dim docHead As New ReceiptFormatter(64, _
                                                New FormattedColumn(16, 0, eAlignment.Center), _
                                                New FormattedColumn(16, 16, eAlignment.Center), _
                                                New FormattedColumn(16, 32, eAlignment.Center), _
                                                New FormattedColumn(16, 48, eAlignment.Center))



            docHead.AddRow("SO Num", "Date", "Time", "Van")
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

            If Not IsNothing(dv.SelectSingleNode("ponum")) Then
                custDetails.AddRow("Your PO:", dv.SelectSingleNode("ponum").InnerText)
            End If

            Dim partsList As New ReceiptFormatter(64, _
                                                  New FormattedColumn(8, 0, eAlignment.Right), _
                                                  New FormattedColumn(42, 10, eAlignment.Left), _
                                                  New FormattedColumn(7, 54, eAlignment.Left))
            partsList.AddRow("Code:", "Description:", "Qty:")
            Dim lines As Integer
            Dim units As Integer

            For Each deliverypart As XmlNode In dv.SelectNodes("parts/part[cquant>0]")
                Dim name As String = deliverypart.SelectSingleNode("name").InnerText
                Dim des As String = deliverypart.SelectSingleNode("des").InnerText
                Dim qty As String
                If deliverypart.SelectSingleNode("cheese").InnerText = "Y" Then
                    qty = deliverypart.SelectSingleNode("weight").InnerText & "kg"
                    units += 1
                Else
                    qty = deliverypart.SelectSingleNode("cquant").InnerText
                    units += CInt(qty)
                End If

                partsList.AddRow(name, des, qty)

                lines += 1


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
    End Sub

    Public Sub PrintInvoice(ByVal dv As XmlNode)
        Dim headerFont As New PrinterFont(50, 5, 2) 'variable width. 
        Dim largeFont As New PrinterFont(30, 0, 3)
        Dim smallFont As New PrinterFont(35, 0, 2)

        Using lblInvoice As New Label(thisForm.Printer, eLabelStyle.receipt)

            'first receipt formatter
            'taking margins into account the receipt is 556px wide. 
            'font 0 - 8/16 for size 2/3 respectively, font 5 is variable width ~20-30.

            '####invoice header'####
            Dim ivnum As String = dv.SelectSingleNode("sonum").InnerText
            'String.Format("{0}-{1}-{2}", dv.ParentNode.ParentNode.SelectSingleNode("curdate").InnerText, _
            '                    dv.ParentNode.ParentNode.SelectSingleNode("routenumber").InnerText, _
            '                    dv.SelectSingleNode("ordinal").InnerText)
            Dim ivdate As String = Now.ToString("dd/MM/yy")
            Dim ivtime As String = Now.ToString("HH:mm")
            Dim van As String = dv.ParentNode.ParentNode.SelectSingleNode("vehiclereg").InnerText.ToUpper

            Dim docHead As New ReceiptFormatter(64, _
                                                New FormattedColumn(16, 0, eAlignment.Center), _
                                                New FormattedColumn(16, 16, eAlignment.Center), _
                                                New FormattedColumn(16, 32, eAlignment.Center), _
                                                New FormattedColumn(16, 48, eAlignment.Center))
            docHead.AddRow("SO Num:", "Date:", "Time:", "Van:")
            docHead.AddRow(ivnum.ToUpper(), ivdate, ivtime, van)

            '####customer details####
            Dim custnumber As String = dv.SelectSingleNode("customer/custnumber").InnerText.ToUpper
            Dim custname As String = dv.SelectSingleNode("customer/custname").InnerText.ToUpper
            Dim postcode As String = dv.SelectSingleNode("customer/postcode").InnerText.ToUpper

            Dim custDetails As New ReceiptFormatter(64, _
                                                    New FormattedColumn(13, 0, eAlignment.Right), _
                                                    New FormattedColumn(48, 16, eAlignment.Left))
            custDetails.AddRow("Customer:", custnumber)
            custDetails.AddRow("", custname)
            custDetails.AddRow("", postcode)

            If Not IsNothing(dv.SelectSingleNode("ponum")) Then
                custDetails.AddRow("Your PO:", dv.SelectSingleNode("ponum").InnerText)
            End If

            '####parts list#####


            Dim invoicePartsList As New ReceiptFormatter(64, _
                                                  New FormattedColumn(7, 0, eAlignment.Right), _
                                                  New FormattedColumn(34, 8, eAlignment.Left), _
                                                  New FormattedColumn(7, 43, eAlignment.Right), _
                                                  New FormattedColumn(7, 55, eAlignment.Right), _
                                                  New FormattedColumn(1, 63, eAlignment.Right) _
                                                  )
            invoicePartsList.AddRow("No:", "Description:", "Price:", "Total:", "")
            Dim lines As Integer = 0
            Dim units As Integer = 0
            Dim invoicetotal As Double = 0
            Dim partsDict As New Dictionary(Of String, List(Of String))
            Dim vatTotal As Double = 0.0 'vat

            For Each OrderPart As XmlNode In dv.SelectNodes("parts/part[cquant>0]")

                Dim name As String = OrderPart.SelectSingleNode("name").InnerText
                Dim des As String = OrderPart.SelectSingleNode("des").InnerText

                Dim qty As String

                If OrderPart.SelectSingleNode("cheese").InnerText = "Y" Then
                    qty = OrderPart.SelectSingleNode("weight").InnerText
                Else
                    qty = OrderPart.SelectSingleNode("cquant").InnerText
                End If

                Dim price As Double = CDbl(OrderPart.SelectSingleNode("price").InnerText)
                'vat addition 
                Dim vat As Double = CDbl(OrderPart.SelectSingleNode("vat").InnerText)
                Dim vatMark As String = ""
                If vat > 0.0 Then
                    vatTotal += vat
                    vatMark = "+"
                End If

                Dim rcList As New List(Of String)

                If OrderPart.SelectSingleNode("cheese").InnerText = "Y" Then
                    invoicePartsList.AddRow(qty & "kg", _
                                            des, _
                                            price.ToString("c").Replace("£", "#"), _
                                            (CDbl(price) * CDbl(qty)).ToString("c").Replace("£", "#"), _
                                            vatMark _
                                            )

                    units += 1
                    lines += 1

                ElseIf Not partsDict.Keys.Contains(name) Then
                    rcList.Add(qty)
                    rcList.Add(des)
                    rcList.Add(price)
                    rcList.Add(CDbl(price) * CDbl(qty))
                    rcList.Add(vatMark)
                    partsDict.Add(name, rcList)
                    units += CDbl(qty)
                    lines += 1

                Else
                    partsDict(name)(0) = CStr(CDbl(partsDict(name)(0) + CDbl(qty)))
                    partsDict(name)(3) = CDbl(partsDict(name)(3)) + (CDbl(qty) * CDbl(price))
                    units += CDbl(qty)

                End If
                invoicetotal += (CDbl(price) * CDbl(qty))
            Next

            For Each k As String In partsDict.Keys
                invoicePartsList.AddRow(partsDict(k)(0), _
                                        partsDict(k)(1), _
                                        CDbl(partsDict(k)(2)).ToString("c").Replace("£", "#"), _
                                        CDbl(partsDict(k)(3)).ToString("c").Replace("£", "#"), _
                                        partsDict(k)(4) _
                                        )
            Next


            Dim RcptTotal As New ReceiptFormatter(64, _
                                              New FormattedColumn(6, 10, eAlignment.Right), _
                                              New FormattedColumn(47, 16, eAlignment.Right))
            RcptTotal.AddRow("Total:", CDbl(invoicetotal).ToString("c").Replace("£", "#"))
            RcptTotal.AddRow("", "")
            RcptTotal.AddRow("", "VAT on lines marked")
            RcptTotal.AddRow("VAT:", vatTotal.ToString("c").Replace("£", "#"))
            RcptTotal.AddRow("", "")
            RcptTotal.AddRow("Total", "")
            RcptTotal.AddRow("& VAT:", (invoicetotal + vatTotal).ToString("c").Replace("£", "#"))

            With lblInvoice

                .CharSet(eCountry.UK)

                'logo
                .AddImage("roddas.pcx", New Point(186, thisForm.Printer.Dimensions.Height + 10), 147)

                'line
                .AddLine(New Point(10, thisForm.Printer.Dimensions.Height + 10), _
                         New Point(thisForm.Printer.Dimensions.Width - 10, thisForm.Printer.Dimensions.Height + 10), 10, 15)

                'header = 174px wide
                .AddText("INVOICE", New Point((thisForm.Printer.Dimensions.Width / 2) - 87, thisForm.Printer.Dimensions.Height + 10), _
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
                For Each StrVal In invoicePartsList.FormattedText
                    .AddText(StrVal, New Point(22, thisForm.Printer.Dimensions.Height), smallFont)
                Next

                'line
                .AddLine(New Point(10, thisForm.Printer.Dimensions.Height + 10), _
                         New Point(thisForm.Printer.Dimensions.Width - 10, thisForm.Printer.Dimensions.Height + 10), 5)

                'total 
                For Each StrVal In RcptTotal.FormattedText
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

                '    'tear 'n' print!
                .AddTearArea(New Point(0, thisForm.Printer.Dimensions.Height))
                thisForm.Printer.Print(.toByte)


            End With
        End Using
    End Sub


#End Region

End Class
