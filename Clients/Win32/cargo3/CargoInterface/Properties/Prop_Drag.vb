Imports cargo3
Imports System.ComponentModel
Imports System.Drawing.Design
'''
<DefaultPropertyAttribute("Name")> _
Public Class prop_Drag

    Private _thisstep As cargo3.ScriptStep

    Public Event Refresh()

    Public Sub New()

    End Sub

    Public Sub New(ByRef thisStep As cargo3.ScriptStep)
        _thisstep = thisStep
    End Sub

    '''
    <CategoryAttribute("Drag"), _
       Browsable(True), _
       [ReadOnly](False), _
       BindableAttribute(False), _
       DefaultValueAttribute(""), _
       DesignOnly(False), _
       Editor(GetType(CoordinateEditor), GetType(System.Drawing.Design.UITypeEditor)), _
       DescriptionAttribute("The screen coordinates to begin dragging.")> _
       Public Property StartLocation() As Point
        Get
            Return _thisstep.StartLoc
        End Get
        Set(ByVal Value As Point)
            _thisstep.StartLoc = Value
            RaiseEvent Refresh()
        End Set
    End Property

    '''
    <CategoryAttribute("Drag"), _
       Browsable(True), _
       [ReadOnly](False), _
       BindableAttribute(False), _
       DefaultValueAttribute(""), _
       DesignOnly(False), _
       Editor(GetType(CoordinateEditor), GetType(System.Drawing.Design.UITypeEditor)), _
       DescriptionAttribute("The screen coordinates to end dragging.")> _
       Public Property EndLocation() As Point
        Get
            Return _thisstep.EndLoc
        End Get
        Set(ByVal Value As Point)
            _thisstep.EndLoc = Value
            RaiseEvent Refresh()
        End Set
    End Property

End Class
