Imports Pop3

Public Class Form1

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Dim mm As RxMailMessage
        Dim numberOfMailsInMailbox As Integer
        Dim mailboxSize As Integer

        Dim DemoClient As New Pop3.Pop3MimeClient("mail.ntsa.org.uk", 110, False, "wonderland\administrator", "=Sund1al")
        With DemoClient
            .ReadTimeout = 60000
            .Connect()
            .GetMailboxStats(numberOfMailsInMailbox, mailboxSize)
            For i As Integer = 1 To numberOfMailsInMailbox
                .GetEmail(i, mm)
                If Not IsNothing(mm) Then
                    Console.WriteLine(mm.MailStructure())
                    If mm.Attachments.Count > 0 Then
                        For Each at As System.Net.Mail.Attachment In mm.Attachments
                            Dim buffer(at.ContentStream.Length) As Byte
                            at.ContentStream.Read(buffer, 0, at.ContentStream.Length)
                            Using fs As New IO.FileStream(at.ContentType.Name, IO.FileMode.Create)
                                fs.Write(buffer, 0, buffer.Length)
                            End Using
                        Next
                    End If
                    '.DeleteEmail(i)
                End If
            Next
            .Disconnect()
        End With        

    End Sub
End Class
