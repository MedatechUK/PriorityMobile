Imports cargo3
Imports System.ComponentModel
Imports System.Drawing.Design
'''
<DefaultPropertyAttribute("Name")> _
Public Class prop_State

    Private _state As cargo3.State

    Public Event Refresh()

    Public Sub New()

    End Sub

    Public Sub New(ByRef state As cargo3.State)
        _state = state
        _Name = state.Name
        _DefaultState = state.DefaultState
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

    Private _DefaultState As Boolean = False
    '''
    <CategoryAttribute("General"), _
       Browsable(True), _
       [ReadOnly](False), _
       BindableAttribute(False), _
       DefaultValueAttribute(""), _
       DesignOnly(False), _
       DescriptionAttribute("Is the selected state the default.")> _
       Public Property DefaultState() As Boolean
        Get
            Return _DefaultState
        End Get
        Set(ByVal Value As Boolean)
            _DefaultState = Value
            _state.DefaultState = Value
            If Value Then
                For Each st As cargo3.State In myStates.Values
                    If Not st.Name = _state.Name Then
                        st.DefaultState = False
                    End If
                Next
            End If

            RaiseEvent Refresh()
        End Set
    End Property

End Class
