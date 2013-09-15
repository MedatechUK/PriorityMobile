Imports System.Windows.Forms

Public Class TablePanel
    Inherits iFormPanel

#Region "Initialisation and finalisation"

    Public Sub New(ByRef Parent As System.Windows.Forms.UserControl, ByRef Columns As cColumns)

        _Parent = Parent
        _Columns = Columns
        _TableItem = New cTableItem(Columns)

        Dim CharWid As Integer = 15

        With Me.Controls
            lv = NewListView()            
            .Add(lv)
            For Each col As cColumn In Columns.Values
                Dim ch = New ColumnHeader
                With ch                                        
                    If Not col.Visible Then
                        .Width = 0
                    Else
                        .Text = col.Title
                        Select Case col.ColumnType
                            Case "INT", "REAL"
                                .Width = CharWid * 6
                            Case Else
                                .Width = CharWid * col.Width
                                If .Width > Screen.PrimaryScreen.WorkingArea.Width / 2 Then
                                    .Width = Screen.PrimaryScreen.WorkingArea.Width / 2
                                End If
                        End Select

                    End If
                End With
                lv.Columns.Add(ch)
            Next
        End With

        _ScanBuffer = New ScanBuffer(Me)
        AddHandler lv.KeyDown, AddressOf Table_KeyDown

    End Sub

    Private Function NewListView() As ListView
        Dim ret As New ListView
        With ret
            .View = View.Details
            .FullRowSelect = True
            .Dock = DockStyle.Fill
        End With
        Return ret
    End Function

#End Region

#Region "Inheritance"

    Public Overrides ReadOnly Property ParentForm() As iForm
        Get
            Return Parent.ParentForm
        End Get
    End Property

    Private _Parent As iFormChild
    Public Overloads ReadOnly Property Parent() As iFormChild
        Get
            Return _Parent
        End Get
    End Property

#End Region

#Region "Public Properties"

    Private _ScanBuffer As ScanBuffer
    Public Property ScanBuffer() As ScanBuffer
        Get
            Return _ScanBuffer
        End Get
        Set(ByVal value As ScanBuffer)
            _ScanBuffer = value
        End Set
    End Property

    Private _TableItem As cTableItem
    Public Property TableItem() As cTableItem
        Get
            Return _TableItem
        End Get
        Set(ByVal value As cTableItem)
            _TableItem = value
        End Set
    End Property

    Private _Columns As cColumns
    Public Property Columns() As cColumns
        Get
            Return _Columns
        End Get
        Set(ByVal value As cColumns)
            _Columns = value
        End Set
    End Property

    Private WithEvents lv As ListView
    Public Property thisTable() As ListView
        Get
            Return lv
        End Get
        Set(ByVal value As ListView)
            lv = value
        End Set
    End Property

    Public ReadOnly Property HasSelection() As Boolean
        Get
            For Each i As ListViewItem In lv.Items
                If i.Selected Then
                    Return True
                End If
            Next
            Return False
        End Get        
    End Property

    Public ReadOnly Property SelectedItem() As cTableItem
        Get
            For Each i As ListViewItem In lv.Items
                If i.Selected Then
                    Dim item As New cTableItem(_TableItem, i)
                    Dim c As Integer = 0
                    For Each col As cColumn In Columns.Values
                        item(String.Format(":$.{0}", col.Name)) = i.SubItems(c).Text
                        c += 1
                    Next
                    Return item
                End If
            Next
            Return New cTableItem(_TableItem)
        End Get
    End Property

    Public ReadOnly Property HasMandatory() As Boolean
        Get
            Dim MandatoryColumn As New List(Of Integer)
            Dim i As Integer = 0
            For Each col As cColumn In _Columns.Values
                If col.Mandatory Then
                    MandatoryColumn.Add(i)
                End If
                i += 1
            Next
            For Each lvi As ListViewItem In lv.Items
                For Each m As Integer In MandatoryColumn
                    If lvi.SubItems(m).Text.Length = 0 Then
                        Return False
                    End If
                Next
            Next
            Return True
        End Get
    End Property

#End Region

#Region "Table Event Handlers"

    Private Sub Table_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)

        Select Case e.KeyValue

            Case 8
                e.Handled = True

            Case 40, 39, 38, 37, 9
                e.Handled = True

            Case 32
                e.Handled = True

            Case 10
                e.Handled = True

            Case 13
                e.Handled = True
                If _ScanBuffer.Length > 0 Then
                    With _Parent.ParentForm.thisHandler
                        Select Case _ScanBuffer.is2d
                            Case True
                                .Scan2d(Me)
                            Case Else
                                .Scan1d(Me)
                        End Select
                        _ScanBuffer.Clear()
                    End With
                End If

            Case 63, 46
                e.Handled = True

        End Select

    End Sub

    Private Sub hGotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles lv.GotFocus
        Dim excep As New Exception
        excep = Nothing
        ParentForm.ViewMain.FormView.ViewForm.Defocus(excep)
        If Not IsNothing(excep) Then
            MsgBox(excep.Message, , ParentForm.ViewMain.FormView.ViewForm.FocusedControl.thisColumn.Name)
        Else
            thisTable.Focus()
            ParentForm.ViewMain.FormButtons.RefreshButtons()
        End If
    End Sub

    Private Sub hSelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lv.SelectedIndexChanged
        ParentForm.ViewMain.FormButtons.RefreshButtons()
    End Sub

    Private Sub hLostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles lv.LostFocus
        ParentForm.ViewMain.FormButtons.RefreshButtons()
    End Sub

#End Region

#Region "Public Methods"

    Public Sub ClearSelection()
        For Each i As ListViewItem In lv.Items
            i.Selected = False
            i.Focused = False
        Next
    End Sub

#End Region

End Class
