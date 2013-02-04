Imports PriorityMobile

Public Class ctrl_Maintainance
    Inherits iView

    Public Overrides Sub Bind()
        With Gtext
            .thisHTML = thisForm.FormData.SelectSingleNode("pdadata/maintainance/text").InnerText
            .PARSE()
        End With
    End Sub

End Class
