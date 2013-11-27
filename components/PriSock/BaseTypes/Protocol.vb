Public Enum eProtocolType
    tcp
    udp
End Enum

Public MustInherit Class Protocol : Implements IDisposable

    Public MustOverride ReadOnly Property ProtocolType() As eProtocolType
    Public MustOverride Sub disposeMe()

    Public Function EndTrans(ByRef sb As Text.StringBuilder) As Boolean
        Dim content As String = sb.ToString.ToLower
        With content
            Return .IndexOf("</request>") > -1 _
            Or content.IndexOf("</response>") > -1 _
            Or content.IndexOf("<#EOF#>") > -1
        End With
    End Function

    Private disposedValue As Boolean = False        ' To detect redundant calls

    ' IDisposable
    Protected Overridable Sub Dispose(ByVal disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                disposeMe()
            End If

            ' TODO: free your own state (unmanaged objects).
            ' TODO: set large fields to null.
        End If
        Me.disposedValue = True
    End Sub

#Region " IDisposable Support "
    ' This code added by Visual Basic to correctly implement the disposable pattern.
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region

End Class
