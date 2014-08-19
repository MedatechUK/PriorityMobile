Imports PriorityMobile
Imports System.Xml

Public Class ctrl_CallDetail
    Inherits iView

    Public Overrides Sub Bind()
        With Gtext
            .thisHTML = thisForm.FormData.SelectSingleNode(thisForm.thisxPath).InnerText
            .PARSE()
        End With
    End Sub

End Class
