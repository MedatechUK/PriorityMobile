Imports System.Drawing
Imports System.Xml

Public Class Condition

    Public Sub New(ByVal Name As String, ByVal Coordinate As Point, ByVal Colour As rgb, Optional ByVal Tolerance As Integer = 5)
        _coordinate = Coordinate
        _Colour = Colour
        _Name = Name
        _Tolerance = Tolerance
    End Sub

    Private _Name As String = ""
    Public ReadOnly Property Name() As String
        Get
            Return _Name
        End Get
    End Property

    Private _Colour As rgb
    Public Property Colour() As rgb
        Get
            Return _Colour
        End Get
        Set(ByVal value As rgb)
            _Colour = value
        End Set
    End Property

    Private _coordinate As Point
    Public Property thisCoordinate() As point
        Get
            Return _coordinate
        End Get
        Set(ByVal value As point)
            _coordinate = value
        End Set
    End Property

    Private _Tolerance As Integer
    Public Property Tolerance() As Integer
        Get
            Return _Tolerance
        End Get
        Set(ByVal value As Integer)
            _Tolerance = value
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
