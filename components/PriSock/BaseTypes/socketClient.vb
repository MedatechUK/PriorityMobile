Public MustInherit Class socketClient : Inherits Protocol

    Public MustOverride Overloads Function Send(ByVal data() As Byte) As Byte()
    Public MustOverride ReadOnly Property ConnectionError() As Exception

    Public Overloads Function Send(ByVal strdata As String) As Byte()
        Return Send(Text.Encoding.ASCII.GetBytes(strdata))
    End Function

End Class
