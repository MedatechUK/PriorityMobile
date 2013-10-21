Imports System.Xml
Imports PriorityMobile

Public Class ctrl_InvoiceItems
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
                .FormLabel = "Invoice Items"
                .Sort = "name"
                .AddColumn("ordi", "ordi", 0, True)
                .AddColumn("name", "Part", 130)
                .AddColumn("des", "Description", 260)
                '.AddColumn("barcode", "Barcode", 130)
                .AddColumn("qty", "Qty", 65)
                .AddColumn("unitprice", "Price", 130, , eColumnFormat.fmt_Money)
            End With
        End With

    End Sub

    Public Overrides Sub FormClosing()
        Dim total As Double = 0
        Dim dr() As Data.DataRow = Nothing
        Dim query As String = String.Format( _
                "{0} <> '0'", _
                ListSort1.Keys(0) _
                )
        dr = thisForm.Datasource.Select(query, ListSort1.Sort)

        For Each r As System.Data.DataRow In dr
            total += CDbl(r.Item("qty")) * CDbl(r.Item("unitprice"))
        Next

        With thisForm
            Dim parts As XmlNode = .FormData.SelectSingleNode(.boundxPath).ParentNode
            parts.ParentNode.SelectSingleNode("total").InnerText = total.ToString
            .Save()
            With .Parent
                .Views(.CurrentView).RefreshData()
                .RefreshForm()
            End With

        End With
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

#Region "XML lookups"

    Private ReadOnly Property Customer() As XmlNode
        Get
            With thisForm
                Return .FormData.SelectSingleNode(.boundxPath).ParentNode.ParentNode.ParentNode.ParentNode
            End With
        End Get
    End Property

    Private ReadOnly Property HasOpenOrder() As Boolean
        Get
            Return Customer.SelectNodes("./orders/order[deliverydate != '0']").Count > 0
        End Get
    End Property

    Private ReadOnly Property OpenOrder() As XmlNode
        Get
            Return Customer.SelectSingleNode("./orders").LastChild
        End Get
    End Property

#End Region

#Region "Direct Activations"

    Public Overrides Sub DirectActivations(ByRef ToolBar As daToolbar)
        ToolBar.Add(AddressOf hReorder, "COPY.BMP", Not ListSort1.SelectedIndex = -1)
        ToolBar.Add(AddressOf hCredit, "delete.BMP", Not ListSort1.SelectedIndex = -1)
    End Sub

    Private Sub hReorder()
        If Not HasOpenOrder Then
            Dim dlg As New dlgAddOrder
            dlg.Name = "Order"
            Dim dt As DateTimePicker = dlg.FindControl("DeliveryDate")
            dt.Value = Now
            thisForm.Dialog(dlg)
        Else
            thisForm.Calc(New calcSetting(CInt(0), , , String.Format("{0}", thisForm.CurrentRow("des"))))
        End If
    End Sub

    Private Sub hCredit()
        Dim credit As New dlgAddCredit
        With credit
            .Name = "Credit"
            Dim CreditQty As NumericUpDown = .FindControl("CreditQty")
            With CreditQty
                .Maximum = Integer.Parse(thisForm.CurrentRow("qty"))
                .Minimum = 0
                .Value = 1
            End With
            Dim rcvQty As NumericUpDown = .FindControl("rcvQty")
            With rcvQty
                .Maximum = 1
                .Minimum = 0
                .Value = 0
            End With
            Dim PartName As Label = .FindControl("PartName")
            PartName.Text = thisForm.CurrentRow("name")
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
            .FocusContolName = "CreditQty"
        End With
        thisForm.Dialog(credit)
    End Sub

    Public Overrides Sub CloseDialog(ByVal frmDialog As PriorityMobile.UserDialog)
        Select Case frmDialog.Name
            Case "Credit"
                CloseCreditDialog(frmDialog)
            Case "Order"
                CloseOrderDialog(frmDialog)
        End Select
    End Sub

    Private Sub CloseCreditDialog(ByVal frmDialog As PriorityMobile.UserDialog)

        Dim CreditQty As NumericUpDown = frmDialog.FindControl("CreditQty")
        Dim rcvQty As NumericUpDown = frmDialog.FindControl("rcvQty")
        Dim PartName As Label = frmDialog.FindControl("PartName")
        Dim CreditReason As ComboBox = frmDialog.FindControl("CreditReason")

        With thisForm
            If frmDialog.Result = DialogResult.OK Then
                Dim CreditNote As XmlNode = .FormData.SelectSingleNode(.boundxPath).ParentNode.ParentNode.ParentNode.ParentNode.SelectSingleNode("creditnote")
                With thisForm
                    AddCreditNote( _
                        thisForm, _
                        CreditNote, _
                        .CurrentRow("ordi"), _
                        .FormData.SelectSingleNode(.boundxPath).ParentNode.ParentNode.SelectSingleNode("ivnum").InnerText, _
                        .CurrentRow("name"), _
                        .CurrentRow("des"), _
                        CreditQty.Value.ToString, _
                        .CurrentRow("unitprice"), _
                        rcvQty.Value.ToString, _
                        CreditReason.SelectedItem _
                     )
                    .CurrentRow("qty") = .CurrentRow("qty") - CreditQty.Value.ToString
                    .Bind()
                End With
            End If
            .RefreshForm()
        End With

    End Sub

    Private Sub CloseOrderDialog(ByVal frmDialog As PriorityMobile.UserDialog)

        Dim DeliveryDate As DateTimePicker = frmDialog.FindControl("DeliveryDate")
        Dim PONum As TextBox = frmDialog.FindControl("PONum")
        With thisForm
            If frmDialog.Result = DialogResult.OK Then
                Dim orders As XmlNode = Customer.SelectSingleNode("orders")
                AddOrder(thisForm, orders, .DateToInt8(DeliveryDate.Value), PONum.Text)
                .Bind()
            End If
            .RefreshForm()
            If frmDialog.Result = DialogResult.OK Then .Calc(New calcSetting(CInt(0), , , .CurrentRow("des").ToString))
        End With

    End Sub

    Public Overrides Sub SetNumber(ByRef cSetting As calcSetting)
        With thisForm
            If cSetting.Result = DialogResult.OK Then
                AddOrderItem(thisForm, OpenOrder, .CurrentRow("name"), .CurrentRow("barcode"), .CurrentRow("des"), cSetting.DNUM.ToString, "")
                .RefreshForm()
            Else
                .RefreshForm()
            End If
        End With
    End Sub

#End Region

End Class
