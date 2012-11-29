Imports System.Windows.Forms

Public Class frmNewColumn

    Public Sub New(ByVal Table As String, ByVal Ord As Integer)
        InitializeComponent()
        _table = Table
        _ord = Ord
    End Sub

    Private _table As String = ""
    Public ReadOnly Property table() As String
        Get
            Return _table
        End Get
    End Property

    Private _Ord As Integer = -1
    Public ReadOnly Property ord() As Integer
        Get
            Return _Ord
        End Get
    End Property

    Public ReadOnly Property strType() As String
        Get
            Return Me.txt_Type.Text
        End Get
    End Property

    Public ReadOnly Property ColumnName() As String
        Get
            Return Me.txt_ColumnName.Text
        End Get
    End Property

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub txt_Type_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_Type.SelectedIndexChanged

    End Sub
    Private Sub Label2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label2.Click

    End Sub

End Class
