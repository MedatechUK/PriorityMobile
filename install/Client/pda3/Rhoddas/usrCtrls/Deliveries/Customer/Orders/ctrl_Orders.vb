Imports PriorityMobile
Imports System.Xml

Public Class ctrl_Orders
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
                .Sort = "deliverydate"
                .AddColumn("deliverydate", "Date", 130, True)
                .AddColumn("ponum", "PO", 130)
                .AddColumn("value", "Total", 130)
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
        ToolBar.Add(AddressOf hShowAddDialog, "add.BMP", True)
        ToolBar.Add(AddressOf hPrint, "print.BMP", Not ListSort1.SelectedIndex = -1)
    End Sub

    Private Sub hShowAddDialog()        
        thisForm.Dialog(New dlgAddOrder)
    End Sub

    Public Overrides Sub CloseDialog(ByVal frmDialog As PriorityMobile.UserDialog)

        Dim DeliveryDate As DateTimePicker = frmDialog.FindControl("DeliveryDate")
        Dim PONum As TextBox = frmDialog.FindControl("PONum")
        With thisForm
            If frmDialog.Result = DialogResult.OK Then
                Dim Order As XmlNode = .CreateNode(.FormData.SelectSingleNode(.boundxPath).ParentNode, "order")
                .CreateNode(Order, "deliverydate", DeliveryDate.Value.ToString)
                .CreateNode(Order, "ponum", PONum.Text)
                .CreateNode(Order, "value", "0.00")
                Dim Parts As XmlNode = .CreateNode(Order, "parts")
                Dim Part As XmlNode = .CreateNode(Parts, "part")
                .CreateNode(Part, "name", "0")
                .CreateNode(Part, "barcode", "0")
                .CreateNode(Part, "des", "0")
                .CreateNode(Part, "qty", "0")
                .CreateNode(Part, "unitprice", "0")
                .Save()                
                .Bind()
            End If
            .RefreshForm()
        End With

    End Sub

    Private Sub hPrint()
        MsgBox("Printing...", MsgBoxStyle.OkOnly)
    End Sub

#End Region

End Class
