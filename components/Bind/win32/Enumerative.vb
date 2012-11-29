#Region "Enumerations"

Public Enum tTrigger
    PREINSERT
    POSTINSERT
    PREUPDATE
    POSTUPDATE
    PREDELETE
    POSTDELETE
End Enum
Public Enum tSource
    File = 1
    SOAP = 2
    Serial = 3
    ADO = 4
End Enum

Public Enum tStackMode
    fifo = 0
    lifo = 1
End Enum

Public Enum tFilePath
    data = 0
    del = 1
    post = 2
End Enum

Public Enum o
    ServiceCall = 0
    Warehouse = 1
    Statuses = 2
    Details = 3
    Malfunction = 4
    Resolution = 5
    Survey = 6
    Repair = 7
    Time = 8
    Parts = 9
    Signature = 10
    Answers = 11
    Flags = 12
    Actions = 13
    Cancel = 14
    DayEnd = 15
End Enum

' Enum to hold filter operators. The chars 
' are converted to their integer values.
Public Enum FilterOperator
    EqualTo
    LessThan
    GreaterThan
    None

End Enum

#End Region

Public Structure hSyncResult
    ' Holds the result of a dataset syncronisation

    Private _redraw As Boolean
    Private _NewData As Boolean

    Public Property Redraw() As Boolean
        Get
            Return _redraw
        End Get
        Set(ByVal value As Boolean)
            _redraw = value
        End Set
    End Property

    Public Property NewData() As Boolean
        Get
            Return _NewData
        End Get
        Set(ByVal value As Boolean)
            _NewData = value
        End Set
    End Property

End Structure

Public Structure SingleFilterInfo
    Public PropName As String
    Public PropDesc As ComponentModel.PropertyDescriptor
    Public CompareValue As Object
    Public OperatorValue As FilterOperator
End Structure
