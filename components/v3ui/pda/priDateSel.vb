Imports System.Text.RegularExpressions
Imports System.xml

Public Class priDateSel

    Private ct As ctform = Nothing

    Public ReadOnly Property FromMin() As Integer
        Get
            Return DateDiff(DateInterval.Minute, #1/1/1988#, SelectedDate) - 1
        End Get
    End Property

    Public ReadOnly Property ToMin() As Integer
        Get
            Return DateDiff(DateInterval.Minute, #1/1/1988#, SelectedDate) + 1439
        End Get
    End Property

    Private Function SelectedDate() As Date
        Return New Date(DatePick.Value.Year, DatePick.Value.Month, DatePick.Value.Day)
    End Function

    Private Sub DatePick_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DatePick.ValueChanged
        If IsNothing(ct) Then
            ct = Parent
        End If
        ct.DrawForm()
    End Sub

End Class