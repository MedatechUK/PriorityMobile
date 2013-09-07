Imports System.Drawing.Design
Imports System.ComponentModel
Imports hookey

Public Class CoordinateEditor
    Inherits UITypeEditor

    Private _prop As System.ComponentModel.PropertyDescriptor
    Private _Instance As Object ' CargoInterface.prop_Condition
    Private _P As Point

    Public Overrides Function EditValue(ByVal context As ITypeDescriptorContext, ByVal provider As IServiceProvider, ByVal value As Object) As Object

        _prop = context.PropertyDescriptor
        _Instance = context.Instance
        _P = value

        Cursor.Position = _P

        Dim selecting As New System.Timers.Timer
        With selecting
            .Interval = 100
            AddHandler .Elapsed, AddressOf hSelTimer
            .Start()
        End With

        KeyWait(VK_ESCAPE)

        selecting.Stop()
        selecting.Dispose()

        Return _P

    End Function

    Private Sub hSelTimer(ByVal sender As Object, ByVal e As System.EventArgs)
        SetCoord()
    End Sub

    Public Sub SetCoord()        
        With Cursor.Position
            _P = New Point(.X, .Y)
            _prop.SetValue(_Instance, _P)
        End With
    End Sub

    Public Overrides Function GetEditStyle(ByVal context As ITypeDescriptorContext) As UITypeEditorEditStyle
        Return UITypeEditorEditStyle.Modal
    End Function

End Class