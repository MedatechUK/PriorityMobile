Imports System.IO
Imports System.Management
Imports System.Reflection

Module startup

    Public Sub Main(ByVal args As String())

        For Each arg As String In args
            If File.Exists(arg) Then
                Application.Run(New frmView(arg))
            Else
                MsgBox(String.Format("File [{0} does not exist.]", arg))
            End If
        Next

    End Sub

End Module
