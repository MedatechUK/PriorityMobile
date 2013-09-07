Imports System.Drawing
Imports System.ComponentModel
Imports System.Globalization

<TypeConverter(GetType(rgbConverter)), _
DescriptionAttribute("Expand to see the trigger colour.")> _
Public Class rgb

    Private _r As Integer = 0
    Private _g As Integer = 0
    Private _b As Integer = 0

    Public Sub New()
        _r = 0
        _g = 0
        _b = 0
    End Sub

    Sub New(ByVal Colour As Color)
        Dim this As Color = Color.FromArgb(Colour.ToArgb)
        _r = this.R
        _g = this.G
        _b = this.B
    End Sub

    Public Sub New(ByVal r As Integer, ByVal g As Integer, ByVal b As Integer)
        _r = r
        _g = g
        _b = b
    End Sub

    <DefaultValueAttribute(0)> _
    Public Property Red() As Integer
        Get
            Return _r
        End Get
        Set(ByVal value As Integer)
            _r = value
        End Set
    End Property

    <DefaultValueAttribute(0)> _
    Public Property Green() As Integer
        Get
            Return _g
        End Get
        Set(ByVal value As Integer)
            _g = value
        End Set
    End Property

    <DefaultValueAttribute(0)> _
    Public Property Blue() As Integer
        Get
            Return _b
        End Get
        Set(ByVal value As Integer)
            _b = value
        End Set
    End Property

    <DefaultValueAttribute(0)> _
    Public ReadOnly Property Colour() As Color
        Get
            Return Color.FromArgb(_r, _g, _b)
        End Get
    End Property

End Class

Public Class rgbConverter
    Inherits ExpandableObjectConverter

    Public Overrides Function CanConvertTo( _
                              ByVal context As ITypeDescriptorContext, _
                              ByVal destinationType As Type) As Boolean
        If (destinationType Is GetType(rgb)) Then
            Return True
        End If
        Return MyBase.CanConvertFrom(context, destinationType)
    End Function

    Public Overrides Function ConvertTo( _
                              ByVal context As ITypeDescriptorContext, _
                              ByVal culture As CultureInfo, _
                              ByVal value As Object, _
                              ByVal destinationType As System.Type) _
                     As Object
        If (destinationType Is GetType(System.String) _
            AndAlso TypeOf value Is rgb) Then

            Dim _this As rgb = CType(value, rgb)
            Return String.Format("{0},{1},{2}", _this.Red.ToString, _this.Green.ToString, _this.Blue.ToString)

        End If
        Return MyBase.ConvertTo(context, culture, value, destinationType)
    End Function

    Public Overrides Function ConvertFrom( _
                              ByVal context As ITypeDescriptorContext, _
                              ByVal culture As CultureInfo, _
                              ByVal value As Object) As Object

        If (TypeOf value Is String) Then
            Try
                Dim s As String = CStr(value)
                Dim _this As rgb = New rgb()

                With _this
                    .Red = CInt(s.Split(",")(0))
                    .Green = CInt(s.Split(",")(1))
                    .Blue = CInt(s.Split(",")(2))
                End With

                Return _this
            Catch
                Throw New ArgumentException( _
                    "Can not convert '" & CStr(value) & _
                                      "' to type RGB")

            End Try
        End If
        Return MyBase.ConvertFrom(context, culture, value)
    End Function

End Class