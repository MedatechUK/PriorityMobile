Imports System.ComponentModel
Imports System.Configuration.Install

Public Class Installer1

    Public Sub New()
        MyBase.New()

        'This call is required by the Component Designer.
        InitializeComponent()

        'Add initialization code after the call to InitializeComponent

    End Sub

    Public Overrides Sub Install(ByVal stateSaver As System.Collections.IDictionary)
        MyBase.Install(stateSaver)
        Dim myInput As String = Me.Context.Parameters.Item("Message")
        If myInput Is Nothing Then
            myInput = "There was no message specified"
        End If
        MsgBox(myInput)
    End Sub

End Class
