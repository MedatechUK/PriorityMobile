Imports cargo3
Imports System.ComponentModel
Imports System.Drawing.Design
'''
<DefaultPropertyAttribute("Name")> _
Public Class prop_Condition

    Private _Cond As cargo3.Condition

    Public Event Refresh()

    Public Sub New()

    End Sub

    Public Sub New(ByRef Cond As cargo3.Condition)
        _Cond = Cond
        _Name = Cond.Name
        _Colour = Cond.Colour
        _Location = Cond.thisCoordinate
        _Tolerance = Cond.Tolerance
    End Sub

    Private _Name As String = ""
    '''
    <CategoryAttribute("General"), _
       Browsable(True), _
       [ReadOnly](False), _
       BindableAttribute(False), _
       DefaultValueAttribute(""), _
       DesignOnly(False), _
       DescriptionAttribute("The name of the selected state.")> _
       Public Property Name() As String
        Get
            Return _Name
        End Get
        Set(ByVal Value As String)
            _Name = Value
            RaiseEvent Refresh()
        End Set
    End Property

    Private _Tolerance As Integer
    '''
    <CategoryAttribute("General"), _
       Browsable(True), _
       [ReadOnly](False), _
       BindableAttribute(False), _
       DefaultValueAttribute(5), _
       DesignOnly(False), _
       DescriptionAttribute("The percentage colour tolerance.")> _
       Public Property TolerancePercent() As Integer
        Get
            Return _Cond.Tolerance
        End Get
        Set(ByVal Value As Integer)
            _Tolerance = Value
            _Cond.Tolerance = Value
            RaiseEvent Refresh()
        End Set
    End Property

    Private _Location As Point
    '''
    <CategoryAttribute("Trigger"), _
       Browsable(True), _
       [ReadOnly](False), _
       BindableAttribute(False), _
       DefaultValueAttribute(""), _
       DesignOnly(False), _
       Editor(GetType(ColCoordEditor), GetType(System.Drawing.Design.UITypeEditor)), _
       DescriptionAttribute("The pixel to check.")> _
       Public Property Location() As Point
        Get
            Return _Location
        End Get
        Set(ByVal Value As Point)
            _Location = Value
            _Cond.thisCoordinate = Value
            RaiseEvent Refresh()
        End Set
    End Property

    'Editor(GetType(ColourEditor), GetType(System.Drawing.Design.UITypeEditor)), _
    Private _Colour As rgb
    '''
    <CategoryAttribute("Trigger"), _
       Browsable(True), _
       [ReadOnly](False), _
       BindableAttribute(False), _
       DefaultValueAttribute(""), _
       DesignOnly(False), _
       Editor(GetType(ColourEditor), GetType(System.Drawing.Design.UITypeEditor)), _
       DescriptionAttribute("The pixel Colour to test.")> _
       Public Property Colour() As rgb
        Get
            Return _Colour
        End Get
        Set(ByVal Value As rgb)
            _Colour = Value
            _Cond.Colour = Value
            RaiseEvent Refresh()
        End Set
    End Property

End Class
