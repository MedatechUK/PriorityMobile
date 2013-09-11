Imports System.IO

Partial Class signatures
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim path = System.AppDomain.CurrentDomain.BaseDirectory.ToString & "signatures\"
        Dim fileEntries As String() = Directory.GetFiles(path)
        Dim basedt As DateTime = New DateTime("1988", "1", "1")

        ' Process the list of files found in the directory.
        Dim fileName As String
        Dim ar(0) As String
        For Each fileName In fileEntries
            Dim p() As String = Split(fileName, "\")
            Dim fn As String = p(UBound(p))
            Dim F As New FileInfo(fileName)
            Dim m As Integer = DateDiff(DateInterval.Minute, basedt, F.CreationTime)
            If Right(fileName, 3).ToLower = "sig" Then
                If m > UBound(ar) Then
                    ReDim Preserve ar(m)
                End If
                ar(m) = "signature.aspx?sig=" & fn
            End If
        Next fileName

        Response.Write("<table>")
        For i As Integer = (DateDiff(DateInterval.Minute, basedt, Now) - (30 * 1440)) To UBound(ar)
            If Not IsNothing(ar(i)) Then
                Response.Write("<tr><td>" & basedt.AddMinutes(i) & "</td><td>" & "<a href='" & ar(i) & "'>" & Split(ar(i), "=")(1) & "</a></td></tr>")
            End If
        Next
        Response.Write("</table>")

    End Sub
End Class
