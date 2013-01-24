Friend Class cp_TearArea : Inherits CPCommand

    Public Sub New(ByVal Sender As CPCL.Label, ByVal Location As Point, ByVal Height As Integer)
        Me.Location = Location
        changeHeight(Sender, Location.Y + Height)
    End Sub

    Public Overrides ReadOnly Property tostring() As String
        Get
            Return ""
        End Get
    End Property

End Class
