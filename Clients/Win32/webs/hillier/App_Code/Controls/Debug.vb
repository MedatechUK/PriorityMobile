Imports Microsoft.VisualBasic

Public Class Debug

    Dim _caller As DynamicMaster

    Public Sub New(ByRef DM As DynamicMaster)
        _caller = DM
    End Sub

    Public Sub DisplayError(ByVal Message As String, ByVal iPage As Page, ByVal ph As PlaceHolder)
        With ph
            .Controls.Add(New LiteralControl("<font color='red'>*&nbsp;" & Message & "</font><br>"))
        End With
    End Sub

End Class
