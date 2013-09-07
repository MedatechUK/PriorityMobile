Imports PriorityMobile

Public Class ctrl_Remarks
    Inherits iView

    Public Overrides Sub Bind()

    End Sub

    Public Overrides Sub CurrentChanged()
        With Gtext
            .thisHTML = thisForm.CurrentRow("text")
            .PARSE()
        End With
    End Sub

End Class
