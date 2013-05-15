Imports System.Xml

Public Class Menu

    Private doc As XmlDocument

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        doc = New XmlDocument

        Dim retry As Boolean = False
        Dim con As Boolean
        Do
            con = False
            Try
                doc.Load("http://10.10.10.150:8080/sfdc.ashx")
                con = True
            Catch ex As Exception
                retry = CBool(MsgBox(String.Format("Could not connect: {0}.", ex.Message), MsgBoxStyle.OkCancel) = MsgBoxResult.Ok)
            End Try
        Loop Until con Or (Not retry)

        Dim mnu As New sfdc3.ctrlMenu(doc.SelectSingleNode("sfdc"), AddressOf hMenuClick)
        Me.Menu = mnu

    End Sub

    Private Sub hMenuClick(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim si As MenuItem = sender
        Dim mi As MenuItem = si.Parent

        Try
            Dim int As New sfdc3.iForm(New sfdc3.cInterface(doc.SelectSingleNode(String.Format("sfdc/menu[@name='{0}']/interface[@name='{1}']", mi.Text, si.Text))))
            With int
                .Text = si.Text
                .Show()                
            End With

        Catch EX As Exception
            MsgBox(EX.Message)
        End Try

    End Sub

End Class
