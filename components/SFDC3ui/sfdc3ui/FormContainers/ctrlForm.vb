Imports System.Xml

Public Class ctrlForm : Inherits ctrlBase

    Public Overrides Function formContainer() As System.Windows.Forms.Control
        Return Me
    End Function

    Public Overrides Sub Draw()
        DrawForm()
    End Sub

End Class
