Imports cmSi

Partial Class FormEmail
    Inherits cmSi.cmsInherit

    Public Overrides Function AdminContext() As Boolean
        Return User.IsInRole("webmaster")
    End Function

    Public Overrides Sub PageLoaded(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim str As New Text.StringBuilder
        str.AppendFormat("Email from {0}.", cmSi.cmsData.Settings.Get("WebName")).AppendLine()
        For Each k As String In Request.Params.Keys
            If Not (k.Substring(0, 2) = "__") _
                And Not InStr(k, "$") > 0 Then
                str.AppendFormat("{0}: {1}", k, Request.Form(k)).AppendLine()
            End If
        Next
        str.AppendFormat("Sent: {0}", Now.ToString("dd/MM/yyy @hh:mm")).AppendLine()

        Dim smtp As New Net.Mail.SmtpClient
        For Each recipient As String In cmSi.cmsData.Settings.Get("RcptTO").Split(";")
            Dim mm As New Net.Mail.MailMessage("no-reply@solonsecurity.co.uk", recipient)
            With mm
                .Subject = String.Format("Email from {0}.", cmSi.cmsData.Settings.Get("WebName"))
                .Body = str.ToString
                .IsBodyHtml = False
            End With
            smtp.Send(mm)
        Next

    End Sub
End Class

