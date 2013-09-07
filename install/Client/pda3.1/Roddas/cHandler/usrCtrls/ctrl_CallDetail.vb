Imports PriorityMobile
Imports System.Xml

Public Class ctrl_CallDetail
    Inherits iView

    Public Overrides Sub Bind()

    End Sub

    Public Overrides Sub CurrentChanged()
        With Gtext
            .thisHTML = thisForm.CurrentRow("detail")
            .PARSE()
        End With
    End Sub

End Class
