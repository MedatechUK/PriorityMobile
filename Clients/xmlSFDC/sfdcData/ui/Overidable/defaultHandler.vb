Imports PrioritySFDC

Public Class defaultHandler : Inherits iHandler

    Public Overrides Sub DisabledButtons(ByRef Add As Boolean, ByRef Edit As Boolean, ByRef Copy As Boolean, ByRef Delete As Boolean, ByRef FormPrint As Boolean, ByRef TablePrint As Boolean)
        Copy = True
        Delete = True
        FormPrint = True
        TablePrint = True
    End Sub

End Class
