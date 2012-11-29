' FORMNAME,TABLE,FORMID,PHONECOL
Public Class phoneItem
    Inherits DatasetObjectBase
#Region "Private Variables"
    Private _FORMNAME As String
    Private _TABLE As String
    Private _FORMID As String
    Private _PHONECOL As String
#End Region
#Region "Initialisation"
    Public Sub New()
        MyBase.New()
    End Sub
    Public Sub New(ByVal FORMNAME As String, ByVal TABLE As String, ByVal FORMID As String, ByVal PHONECOL As String)
        MyBase.New()
        _FORMNAME = FORMNAME
        _TABLE = TABLE
        _FORMID = FORMID
        _PHONECOL = PHONECOL
    End Sub
#End Region
#Region "Public Properties"
    Public Property FORMNAME() As String
        Get
            Return _FORMNAME
        End Get
        Set(ByVal value As String)
            If _FORMNAME <> value Then
                _FORMNAME = value
                OnPropertyChanged("FORMNAME")
            End If
        End Set
    End Property
    Public Property TABLE() As String
        Get
            Return _TABLE
        End Get
        Set(ByVal value As String)
            If _TABLE <> value Then
                _TABLE = value
                OnPropertyChanged("TABLE")
            End If
        End Set
    End Property
    Public Property FORMID() As String
        Get
            Return _FORMID
        End Get
        Set(ByVal value As String)
            If _FORMID <> value Then
                _FORMID = value
                OnPropertyChanged("FORMID")
            End If
        End Set
    End Property
    Public Property PHONECOL() As String
        Get
            Return _PHONECOL
        End Get
        Set(ByVal value As String)
            If _PHONECOL <> value Then
                _PHONECOL = value
                OnPropertyChanged("PHONECOL")
            End If
        End Set
    End Property
#End Region
End Class
