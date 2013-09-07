Imports PrioritySFDC

Public Class defaultHandler : Inherits iHandler

    Public Overrides Sub DisabledButtons(ByRef Add As Boolean, ByRef Edit As Boolean, ByRef Copy As Boolean, ByRef Delete As Boolean, ByRef Print As Boolean)
        Print = True
    End Sub

End Class
