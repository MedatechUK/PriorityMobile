
MustInherit Class CPCommand

    Public MustOverride Shadows ReadOnly Property tostring() As String

    Private _Location As Point
    Public Property Location() As Point
        Get
            Return _Location
        End Get
        Set(ByVal value As Point)
            _Location = value
        End Set
    End Property

    Public Sub changeHeight(ByRef sender As Label, ByVal ProposedHeight As Integer)
        Select Case sender.Style
            Case eLabelStyle.receipt
                If ProposedHeight > sender.Printer.Dimensions.Height Then
                    sender.Printer.Dimensions = New Size(sender.Printer.Dimensions.Width, ProposedHeight)
                End If
            Case Else

        End Select
    End Sub

End Class
