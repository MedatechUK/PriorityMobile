Imports cargo3
Imports hookey
Imports prnscn.capture
Imports System.ComponentModel
Imports System.Drawing.Design

Public Class ColCoordEditor
    Inherits UITypeEditor

    Private _prop As System.ComponentModel.PropertyDescriptor
    Private _Instance As cargo3.prop_Condition
    Private _Colour As rgb
    Private _P As Point

    Public Overrides Function EditValue(ByVal context As ITypeDescriptorContext, ByVal provider As IServiceProvider, ByVal value As Object) As Object

        _prop = context.PropertyDescriptor
        _Instance = context.Instance
        _Colour = _Instance.Colour
        _P = _Instance.Location

        Cursor.Position = _P

        Dim selecting As New System.Timers.Timer
        With selecting
            .Interval = 250
            AddHandler .Elapsed, AddressOf hSelTimer
            .Start()
        End With

        KeyWait(VK_ESCAPE)

        selecting.Stop()
        selecting.Dispose()

        Return _P

    End Function

    Private Sub hSelTimer(ByVal sender As Object, ByVal e As System.EventArgs)
        SetColour()
        SetCoord()
    End Sub

    Public Sub SetColour()

        With Cursor.Position
            Dim bmp As Bitmap = PrintScreen(.X + 1, .Y + 1)
            _Colour = New rgb(bmp.GetPixel(.X, .Y))
        End With
        _Instance.Colour = _Colour
        '_prop.SetValue(_Instance.Colour, _Colour)

    End Sub

    Public Sub SetCoord()
        With Cursor.Position
            _P = New Point(.X, .Y)
            '_prop.SetValue(_Instance.Location, _P)
            _Instance.Location = _P
        End With
    End Sub

    Public Overrides Function GetEditStyle(ByVal context As ITypeDescriptorContext) As UITypeEditorEditStyle
        Return UITypeEditorEditStyle.Modal
    End Function

End Class
