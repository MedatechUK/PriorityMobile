Public Class Form1

    Private getData As New GetData.GetXMLData("http://redknave:8080")

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim reader As System.Data.DataTable = getData.ExecuteReader("Select * From PART")
        Dim result As Integer = getData.ExecuteScalar("Select 1+1")

        getData.ExecuteNonQuery("delete from GENERALLOAD where LINE > 0")
    End Sub

End Class
