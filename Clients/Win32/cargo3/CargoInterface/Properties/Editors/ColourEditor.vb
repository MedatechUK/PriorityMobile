Imports hookey
Imports prnscn.capture
Imports cargo3
Imports System.ComponentModel
Imports System.Drawing.Design

Public Class ColourEditor
    Inherits UITypeEditor

    Private _prop As System.ComponentModel.PropertyDescriptor
    Private _Instance As cargo3.prop_Condition
    Private _Colour As rgb

    Public Overrides Function EditValue(ByVal context As ITypeDescriptorContext, ByVal provider As IServiceProvider, ByVal value As Object) As Object

        _prop = context.PropertyDescriptor
        _Instance = context.Instance
        _Colour = value

        Dim selecting As New System.Timers.Timer
        With selecting
            .Interval = 250
            AddHandler .Elapsed, AddressOf hSelTimer
            .Start()
        End With

        KeyWait(VK_ESCAPE)

        selecting.Stop()
        selecting.Dispose()

        Return _Colour

    End Function

    Private Sub hSelTimer(ByVal sender As Object, ByVal e As System.EventArgs)
       SetColour
    End Sub

    Public Sub SetColour()

        With Cursor.Position
            Dim bmp As Bitmap = PrintScreen(.X + 1, .Y + 1)
            _Colour = New rgb(bmp.GetPixel(.X, .Y))
        End With
        _prop.SetValue(_Instance, _Colour)

    End Sub

    Public Overrides Function GetEditStyle(ByVal context As ITypeDescriptorContext) As UITypeEditorEditStyle
        Return UITypeEditorEditStyle.Modal
    End Function

End Class