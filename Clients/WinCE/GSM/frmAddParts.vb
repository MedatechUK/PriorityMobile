Imports System.Data
Imports System.Xml
Public Class frmAddParts
    Public pars As New DataTable
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.DialogResult = Windows.Forms.DialogResult.OK
    End Sub

    Private Sub frmAddParts_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.GotFocus
        TextBox1.Focus()
    End Sub


    Private Sub frmAddParts_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        TextBox1.Focus()

    End Sub

    Private Sub Button1_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        If DataGrid1.VisibleRowCount <> 0 Then
            MsgBox("Not all KIT checked off")
        Else
            Me.DialogResult = Windows.Forms.DialogResult.OK
        End If
    End Sub

    Private Sub TextBox1_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox1.KeyPress
        Select Case e.KeyChar
            Case Chr(13)

                Dim bcode As String = ""
                bcode = TextBox1.Text
                Dim v2 As String = ""
                Dim doc As New Xml.XmlDocument
                doc.LoadXml(TextBox1.Text)
                For Each nd As XmlNode In doc.SelectNodes("in/i")
                    Dim DataType As String = nd.Attributes("n").Value
                    Select Case DataType
                        Case "PART"
                            bcode = nd.Attributes("v").Value
                    End Select

                Next
                'as the scanner sends a return we can handle this to detect it as the end of teh barcode input

                'right now we have the barcode all we want to do is match it to the list and if it exists we remove it from the datagrid
                'to accomplish this we will itereate through the parts in the part datatable
                Dim pr As DataRow
                pr = pars.NewRow
                For Each pr In pars.Rows
                    If pr("Name") = bcode Then
                        'remove the row
                        pr.Delete()
                        'reset the barcode handler textbox
                        TextBox1.Text = ""
                        'restore focus to it for the next scan
                        TextBox1.Focus()
                        'refresh the datagrid
                        DataGrid1.DataSource = pars
                        'break out of the checking
                        Exit Sub
                    End If
                Next
                TextBox1.Text = ""
            Case Else
                Exit Sub
        End Select
    End Sub

    Private Sub TextBox1_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBox1.LostFocus
        If Button1.Focused = True Then

        Else

            TextBox1.Focus()
        End If
    End Sub

    Private Sub TextBox1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox1.TextChanged


    End Sub

    Private Sub DataGrid1_CurrentCellChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DataGrid1.CurrentCellChanged
        TextBox1.Focus()
    End Sub

    Private Sub DataGrid1_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles DataGrid1.GotFocus
        TextBox1.Focus()
    End Sub
End Class