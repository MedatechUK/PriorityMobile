Imports System.Windows.Forms.UserControl

' Protypes the subcontrols of the text control

Partial Public Class CtrlPartial

    Public Enum tCtrl
        ctrlNone = 0
        ctrlText = 1
        ctrlList = 2
        ctrlDate = 3
        ctrlKeyb = 4
        ctrlNumber = 5
    End Enum

    Public Event Complete(ByVal Ctrl As Object, ByVal ctrlType As tCtrl)

    Public Sub CtrlAccept(ByVal Ctrl As Object, ByVal ctrlType As tCtrl)
        RaiseEvent Complete(Ctrl, ctrlType)
    End Sub

    Public Overridable Property Data() As String
        Get

        End Get
        Set(ByVal value As String)

        End Set
    End Property

    Public Overridable Sub SetParent(ByVal Ctrl As Object)

    End Sub

    Public Overridable Sub AddItems(ByVal myArray As Array, ByVal DefaultText As String)

    End Sub

End Class

