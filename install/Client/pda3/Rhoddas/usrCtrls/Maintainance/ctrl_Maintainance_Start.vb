Imports PriorityMobile

Public Class ctrl_Maintainance_Start
    Inherits iView

    Public Overrides Sub Bind()
        With Gtext
            .thisHTML = thisForm.FormData.SelectSingleNode("pdadata/maintainance/daystart/text").InnerText
            .PARSE()
        End With
    End Sub

End Class
