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
                .Sort = "name"
                .AddColumn("ordi", "ordi", 0, True)
                .AddColumn("name", "Part", 130)
                .AddColumn("des", "Description", 130)
                .AddColumn("barcode", "Barcode", 130)
                .AddColumn("qty", "Qty", 130)
                .AddColumn("unitprice", "Price", 130)
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

#Region "Direct Activations"

    Public Overrides Sub DirectActivations(ByRef ToolBar As daToolbar)
        ToolBar.Add(AddressOf hCredit, "delete.BMP", Not ListSort1.SelectedIndex = -1)
    End Sub

    Private Sub hCredit()
        Dim credit As New dlgAddCredit
        With credit
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
            .FocusContolName = "CreditQty"
        End With        
        thisForm.Dialog(credit)
    End Sub

    Public Overrides Sub CloseDialog(ByVal frmDialog As PriorityMobile.UserDialog)

        Dim CreditQty As NumericUpDown = frmDialog.FindControl("CreditQty")
        Dim rcvQty As NumericUpDown = frmDialog.FindControl("rcvQty")
        Dim PartName As Label = frmDialog.FindControl("PartName")

        With thisForm
            If frmDialog.Result = DialogResult.OK Then
                Dim CreditNote As XmlNode = .FormData.SelectSingleNode(.boundxPath).ParentNode.ParentNode.ParentNode.ParentNode.SelectSingleNode("creditnote")

                If IsNothing(CreditNote.SelectSingleNode(String.Format(".//part[name='{0}']", .CurrentRow("name")))) Then
                    Dim part As XmlNode = .CreateNode(CreditNote.SelectSingleNode("parts"), "part")
                    .CreateNode(part, "ivnum", .FormData.SelectSingleNode(.boundxPath).ParentNode.ParentNode.SelectSingleNode("ivnum").InnerText)
                    .CreateNode(part, "ordi", .CurrentRow("ordi"))
                    .CreateNode(part, "name", .CurrentRow("name"))
                    .CreateNode(part, "des", .CurrentRow("des"))
                    .CreateNode(part, "qty", CreditQty.Value.ToString)
                    .CreateNode(part, "unitprice", .CurrentRow("unitprice"))
                    .CreateNode(part, "rcvdqty", rcvQty.Value.ToString)
                Else
                    Dim part As XmlNode = CreditNote.SelectSingleNode(String.Format(".//part[name='{0}']", .CurrentRow("name")))
                    part.SelectSingleNode("qty").InnerText = (Integer.Parse(part.SelectSingleNode("qty").InnerText) + CreditQty.Value).ToString
                    part.SelectSingleNode("rcvdqty").InnerText = (Integer.Parse(part.SelectSingleNode("rcvdqty").InnerText) + rcvQty.Value).ToString
                End If

                .CurrentRow("qty") = .CurrentRow("qty") - CreditQty.Value.ToString
                .Save()
                .Bind()

            End If
            .RefreshForm()
        End With

    End Sub


#End Region

End Class
