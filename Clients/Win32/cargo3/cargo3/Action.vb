Imports System.Xml

Public Class Action

    Public Sub New(ByVal Ord As Integer, ByVal Name As String, ByVal State As String, ByVal logic As XmlAttribute)
        _Name = Name
        _State = State
        _Ord = Ord
        If Not IsNothing(logic) Then _Logic = logic.Value
    End Sub

    Private _Ord As Integer
    Public Property Ord() As Integer
        Get
            Return _Ord
        End Get
        Set(ByVal value As Integer)
            _Ord = value
        End Set
    End Property

    Private _Name As String = ""
    Public ReadOnly Property Name() As String
        Get
            Return _Name
        End Get
    End Property

    Private _State As String = ""
    Public ReadOnly Property State() As String
        Get
            Return _State
        End Get
    End Property

    Private _Conditions As New Dictionary(Of String, Condition)
    Public Property Conditions() As Dictionary(Of String, Condition)
        Get
            Return _Conditions
        End Get
        Set(ByVal value As Dictionary(Of String, Condition))
            _Conditions = value
        End Set
    End Property

    Private _Results As New Dictionary(Of Boolean, result)
    Public Property Results() As Dictionary(Of Boolean, result)
        Get
            Return _Results
        End Get
        Set(ByVal value As Dictionary(Of Boolean, result))
            _Results = value
        End Set
    End Property

    Private _Logic As String = ""
    Public Property Logic() As String
        Get
            Return _Logic
        End Get
        Set(ByVal value As String)
            _Logic = value
        End Set
    End Property

    Private _myMethod As System.Reflection.MethodInfo
    Public Property myMethod() As System.Reflection.MethodInfo
        Get
            Return _myMethod
        End Get
        Set(ByVal value As System.Reflection.MethodInfo)
            _myMethod = value
        End Set
    End Property

End Class
