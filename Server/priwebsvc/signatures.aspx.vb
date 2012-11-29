Imports System.IO

Partial Class signatures
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim path = System.AppDomain.CurrentDomain.BaseDirectory.ToString & "signatures\"
        Dim fileEntries As String() = Directory.GetFiles(path)
        ' Process the list of files found in the directory.
        Dim fileName As String
        Response.Write("<table>")        
        For Each fileName In fileEntries
            Dim p() As String = Split(fileName, "\")
            Dim fn As String = p(UBound(p))
            Response.Write(fn & "<br>")
            If Right(fileName, 3).ToLower = "sig" Then
                Dim F As New FileInfo(fileName)
                Response.Write("<tr><td>" & F.CreationTime & "</td><td>" & "<a href='signature.aspx?sig=" & fn & "'></a></td></tr>")
            End If
        Next fileName
        Response.Write("</table>")

    End Sub
End Class
