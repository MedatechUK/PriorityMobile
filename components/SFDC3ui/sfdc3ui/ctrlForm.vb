Imports System.Xml

Public Class ctrlForm

    Public Sub New(ByRef Sender As iForm, ByVal ParentNode As XmlNode)
        InitializeComponent()
        myiForm = Sender
        Load(myiForm, ParentNode)
    End Sub

End Class
