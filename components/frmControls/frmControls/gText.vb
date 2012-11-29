Public Class gtext

    Private np As New Dictionary(Of String, String)

    Private _thishtml As String = ""
    Public Property thisHTML() As String
        Get
            Return Replace(Replace(_thishtml, ":[", "<"), "]:", ">")
        End Get
        Set(ByVal value As String)
            _thishtml = value
        End Set
    End Property

    Public Sub AddNamePair(ByVal Name As String, ByVal Value As String)
        If np.ContainsKey(Name) Then
            np(Name) = Value
        Else
            np.Add(Name, Value)
        End If
    End Sub

    Public Sub PARSE()

        Dim bhtml As String = ""
        For Each key As String In np.Keys
            bhtml += String.Format("<b>{0}</b>:&nbsp;<br/>{1}<br/>", key, Replace(np(key), vbCrLf, "<br/>"))
        Next        

        Dim Browse As New System.Windows.Forms.WebBrowser
        Me.Controls.Add(Browse)
        With Browse
            .DocumentText = String.Format("<html><body>{0}{1}</body></html>", bhtml, Me.thisHTML)
            .BringToFront()
            .Refresh()
            .Dock = DockStyle.Fill
        End With

    End Sub

End Class
