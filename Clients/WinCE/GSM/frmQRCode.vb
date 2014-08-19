Imports System.Xml
Public Class frmQRCode

    Private Sub frmQRCode_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Width = Screen.PrimaryScreen.WorkingArea.Width
        Me.Height = Screen.PrimaryScreen.WorkingArea.Height

        Dim p As System.Drawing.Point
        p.X = (Screen.PrimaryScreen.WorkingArea.Width - Me.Width) / 2
        p.Y = (Screen.PrimaryScreen.WorkingArea.Height - Me.Height) / 2
        Me.Location = p
        TextBox2.Focus()

    End Sub

    Private Sub TextBox2_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox2.KeyPress
        Select Case e.KeyChar
            Case Chr(13)
                Panel1.Controls.Clear()
                Dim ly As Integer = 22
                Dim ty As Integer = 21

                Dim bcode As String = ""
                bcode = TextBox2.Text
                If bcode.StartsWith("<") = True Then
                    Try
                        Dim v2 As String = ""
                        Dim doc As New Xml.XmlDocument
                        doc.LoadXml(TextBox2.Text)
                        For Each nd As XmlNode In doc.SelectNodes("in/i")
                            Dim l As New Label
                            Dim lp As New Point(18, ly)
                            Dim tp As New Point(123, ty)
                            l.Location = lp


                            l.Text = nd.Attributes("n").Value
                            Panel1.Controls.Add(l)
                            l.SendToBack()
                            Dim t As New TextBox
                            t.Multiline = True
                            t.WordWrap = True

                            t.Location = tp

                            t.BringToFront()
                            Select Case l.Text
                                Case "PART"
                                    l.Text = "BARCODE"
                                    t.Text = nd.Attributes("v").Value
                                Case "PASS"
                                    t.PasswordChar = "*"
                                    t.Text = "N/A"
                                Case Else
                                    t.Text = nd.Attributes("v").Value
                            End Select


                            Panel1.Controls.Add(t)
                            ty += t.Height + 5
                            ly += t.Height + 5

                        Next
                        TextBox2.Text = ""
                    Catch ex As Exception
                        OverControl.msgboxa("There is an error in the xml")
                        TextBox2.Text = ""
                    End Try

                Else
                    Dim l As New Label
                    Dim lp As New Point(18, ly)
                    Dim tp As New Point(123, ty)
                    l.Location = lp


                    l.Text = "Data"
                    Panel1.Controls.Add(l)
                    l.SendToBack()
                    Dim t As New TextBox
                    t.Location = tp
                    t.Text = bcode
                    t.BringToFront()
                    Panel1.Controls.Add(t)
                    TextBox2.Text = ""
                    ty += t.Height + 5
                    ly += t.Height + 5
                End If

        End Select
    End Sub

    Private Sub TextBox2_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox2.TextChanged

    End Sub
End Class