Imports cargo3
Imports System.ComponentModel
Imports System.Drawing.Design
'''
<DefaultPropertyAttribute("Name")> _
Public Class prop_Click

    Private _thisstep As cargo3.ScriptStep

    Public Event Refresh()

    Public Sub New()

    End Sub

    Public Sub New(ByRef thisStep As cargo3.ScriptStep)
        _thisstep = thisStep
    End Sub

    '''
    <CategoryAttribute("Click"), _
       Browsable(True), _
       [ReadOnly](False), _
       BindableAttribute(False), _
       DefaultValueAttribute(""), _
       DesignOnly(False), _
       Editor(GetType(CoordinateEditor), GetType(System.Drawing.Design.UITypeEditor)), _
       DescriptionAttribute("The screen coordinates to click.")> _
       Public Property ClickLocation() As Point
        Get
            Return _thisstep.ClickLoc
        End Get
        Set(ByVal Value As Point)
            _thisstep.ClickLoc = Value
            RaiseEvent Refresh()
        End Set
    End Property

    '''
    <CategoryAttribute("Click"), _
    Browsable(True), _
    [ReadOnly](False), _
    BindableAttribute(False), _
    DefaultValueAttribute(""), _
    DesignOnly(False), _
    Editor(GetType(CoordinateEditor), GetType(System.Drawing.Design.UITypeEditor)), _
    DescriptionAttribute("The screen coordinates to click.")> _
    Public Property Button() As cargo3.ScriptStep.eScriptButtons
        Get
            Return _thisstep.Button
        End Get
        Set(ByVal Value As cargo3.ScriptStep.eScriptButtons)
            _thisstep.Button = Value
            RaiseEvent Refresh()
        End Set
    End Property

End Class
