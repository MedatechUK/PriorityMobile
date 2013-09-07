Imports cargo3
Imports System.ComponentModel
Imports System.Drawing.Design
'''
<DefaultPropertyAttribute("Name")> _
Public Class prop_Delay

    Private _thisstep As cargo3.ScriptStep

    Public Event Refresh()

    Public Sub New()

    End Sub

    Public Sub New(ByRef thisStep As cargo3.ScriptStep)
        _thisstep = thisStep
    End Sub

    '''
    <CategoryAttribute("Wait"), _
       Browsable(True), _
       [ReadOnly](False), _
       BindableAttribute(False), _
       DefaultValueAttribute(""), _
       DesignOnly(False), _
       DescriptionAttribute("The time in milliseconds to wait for.")> _
       Public Property Delay() As Integer
        Get
            Return _thisstep.Delay
        End Get
        Set(ByVal Value As Integer)
            _thisstep.Delay = Value
            RaiseEvent Refresh()
        End Set
    End Property

End Class
