Imports System.Windows.Forms

Public Class UserDialog

    Public Event CloseDialog(ByRef frmDialog As PrioritySFDC.UserDialog)

    Public Overridable ReadOnly Property MyControls() As ControlCollection
        Get
            Return Nothing
        End Get
    End Property

    Private _Result As DialogResult = Nothing
    Public Property Result() As DialogResult
        Get
            Return _Result
        End Get
        Set(ByVal value As DialogResult)
            _Result = value
        End Set
    End Property

    Private _FocusContolName As String = Nothing
    Public Property FocusContolName() As String
        Get
            Return _FocusContolName
        End Get
        Set(ByVal value As String)
            _FocusContolName = value
            Me.FocusTimer.Enabled = True
        End Set
    End Property

    Private _frmName As String = String.Empty
    Public Property frmName() As String
        Get
            Return _frmName
        End Get
        Set(ByVal value As String)
            _frmName = value
        End Set
    End Property

    Public Sub EndDialog()
        RaiseEvent CloseDialog(Me)
    End Sub

    Public Function FindControl(ByVal ControlName As String) As Control
        Dim ret As Control = Nothing
        For Each C As Control In Me.MyControls
            If String.Compare(C.Name, ControlName, True) = 0 Then
                ret = C
                Exit For
            End If
        Next
        Return ret
    End Function

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Private Sub FocusTimer_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles FocusTimer.Tick
        FocusTimer.Enabled = False
        Dim c As Control = Me.FindControl(FocusContolName)
        If Not IsNothing(c) Then c.Focus()
    End Sub

End Class
