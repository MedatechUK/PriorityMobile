Imports cargo3
Imports System.ComponentModel
Imports System.Drawing.Design
'''
<DefaultPropertyAttribute("Name")> _
Public Class prop_KeyPress

    Private _thisstep As cargo3.ScriptStep

    Public Event Refresh()

    Public Sub New()

    End Sub

    Public Sub New(ByRef thisStep As cargo3.ScriptStep)
        _thisstep = thisStep
    End Sub

    '''
    <CategoryAttribute("Key Press"), _
       Browsable(True), _
       [ReadOnly](False), _
       BindableAttribute(False), _
       DefaultValueAttribute(""), _
       DesignOnly(False), _
       Editor(GetType(KeysStrEditor), GetType(System.Drawing.Design.UITypeEditor)), _
       DescriptionAttribute("The text to send to type.")> _
       Public Property Keys() As String
        Get
            Return _thisstep.KeyText
        End Get
        Set(ByVal Value As String)
            _thisstep.KeyText = Value
            RaiseEvent Refresh()
        End Set
    End Property

End Class
