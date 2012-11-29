Imports ViewControl
Imports System.Xml

Public Class ctrl_CallDetail
    Inherits ViewControl.iView

    Public Overrides Sub Bind()
        Dim bstr As String = ""
        For Each n As XmlNode In thisForm.FormData.SelectNodes(thisForm.thisxPath & "/line")
            bstr += n.InnerText
        Next
        With Gtext
            .HTML = bstr
            .PARSE()
        End With
    End Sub

End Class
