Public Enum ePriorityType As Integer
    INT_Type
    REAL_Type
    UNSIGNED_Type
    CHAR_Type
    RCHAR_Type
    DATE_Type
    TIME_Type
    DAY_Type
End Enum

Module PriorityTypes

    Public Function EvalType(ByVal TypeStr As String) As ePriorityType
        Select Case TypeStr.ToUpper
            Case "INT"
                Return ePriorityType.INT_Type
            Case "REAL"
                Return ePriorityType.REAL_Type
            Case "UNSIGNED"
                Return ePriorityType.UNSIGNED_Type
            Case "CHAR"
                Return ePriorityType.CHAR_Type
            Case "RCHAR"
                Return ePriorityType.RCHAR_Type
            Case "DATE"
                Return ePriorityType.DATE_Type
            Case "TIME"
                Return ePriorityType.TIME_Type
            Case "DAY"
                Return ePriorityType.DAY_Type
        End Select
    End Function

End Module
