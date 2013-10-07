Imports System.Windows.Forms

Public Class cTableItem
    Inherits Dictionary(Of String, String)

#Region "Initialisation and Finalisation"

    Public Sub New(ByRef DefaultItem As cTableItem, Optional ByRef EditItem As ListViewItem = Nothing)

        _EditItem = EditItem
        _Unique = DefaultItem.Unique

        For Each k As String In DefaultItem.Keys
            Me.Add(k, "")
        Next

    End Sub

    Public Sub New(ByRef Columns As cColumns, Optional ByRef Unique As List(Of List(Of String)) = Nothing)
        _Unique = Unique
        For Each col As cColumn In Columns.Values
            Me.Add(String.Format(":$.{0}", col.Name), "")
        Next
    End Sub

#End Region

#Region "Public Properties"

    Private _Unique As List(Of List(Of String))
    Public Property Unique() As List(Of List(Of String))
        Get
            Return _Unique
        End Get
        Set(ByVal value As List(Of List(Of String)))
            _Unique = value
        End Set
    End Property

    Private _EditItem As ListViewItem
    Public Property EditItem() As ListViewItem
        Get
            Return _EditItem
        End Get
        Set(ByVal value As ListViewItem)
            _EditItem = value
        End Set
    End Property

    Public Property Column(ByVal ColumnName As String) As String
        Get
            If Keys.Contains(ColumnName) Then
                Return Item(ColumnName)
            Else
                Return Nothing
            End If
        End Get
        Set(ByVal value As String)
            If Keys.Contains(ColumnName) Then
                Item(ColumnName) = value
            End If
        End Set
    End Property

    Public ReadOnly Property HasKeys() As List(Of String)
        Get
            If Unique.Count = 0 Then Return Nothing
            For Each keys As List(Of String) In _Unique
                Dim un As Boolean = True
                For Each key As String In keys
                    If Me(key).Length = 0 Then
                        un = False
                        Exit For
                    End If
                Next
                If un Then
                    Return keys
                End If
            Next
            Return Nothing
        End Get
    End Property

    Public Shadows ReadOnly Property Equals(ByVal TableItem As cTableItem, ByVal Keys As List(Of String)) As Boolean
        Get
            For Each k As String In Keys
                If Not (String.Compare(Me(k), TableItem(k)) = 0) Then
                    Return False
                End If
            Next
            Return True
        End Get
    End Property

#End Region

#Region "Public Methods"

    Public Function thisItem() As ListViewItem
        Dim ret As New ListViewItem
        Dim i As Integer = 0
        For Each k As String In Keys
            If i = 0 Then
                ret.Text = Item(k)
            Else
                Dim lvsi As New ListViewItem.ListViewSubItem
                lvsi.Text = Item(k)
                ret.SubItems.Add(lvsi)
            End If
            i += 1
        Next
        Return ret
    End Function

#End Region

End Class
