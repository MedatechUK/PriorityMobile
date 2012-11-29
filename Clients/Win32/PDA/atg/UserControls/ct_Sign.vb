Public Class ct_Sign
    Inherits System.Windows.Forms.UserControl

    Dim ar As Priority.MyArray
    Dim _ServiceCall As String = ""
    Dim _App As PDAOnBoardData.BaseForm
    Dim thisSig As New Priority.clsSign

    Public Property ServiceCall() As String
        Get
            Return _ServiceCall
        End Get
        Set(ByVal value As String)
            _ServiceCall = value
            NewData()
        End Set
    End Property

    Public Sub New(Optional ByRef App As PDAOnBoardData.BaseForm = Nothing) '

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        If Not IsNothing(App) Then
            _App = App
        End If

        thisSig.Sign.Dock = DockStyle.Bottom
        thisSig.Sign.Height = Me.Height / 4
        Me.Controls.Add(thisSig.Sign)

    End Sub

    Public Sub NewData()

        With _App.rss(o.Signature)
            .currentIndex = _ServiceCall
            If .Validate Then
                thisSig.coord = thisSig.UnpackSignature(.GetField("SigData"))
                ' Set the upper bound
                thisSig.uc = UBound(thisSig.coord, 2)
                ' Redraw the signature
                thisSig.Sign.Invalidate()
            Else
                thisSig.Reset()
                Dim nr As Integer = .NewRecord
                .thisArray(0, nr) = _ServiceCall
                .thisArray(1, nr) = ""
                .thisArray(2, nr) = ""
                .thisArray(3, nr) = ""
                .Save()
            End If
        End With

        With UsedParts
            .Items.Clear()
            .Columns.Clear()
            .Columns.Add("Part Number")
            .Columns.Add("Part Description")
            .Columns.Add("QTY")

            _App.rss(o.Parts).currentIndex = _ServiceCall
            Dim sel() As Integer = _App.rss(o.Parts).Selection

            .Items.Add("")
            .Items(.Items.Count - 1).Text = "Hours"
            .Items(.Items.Count - 1).SubItems.Add("Labour Time")
            .Items(.Items.Count - 1).SubItems.Add(ElapsedTime)

            For i As Integer = 0 To UBound(sel)
                .Items.Add("")
                .Items(.Items.Count - 1).Text = _App.rss(o.Parts).thisArray(2, sel(i))
                _App.rss(o.Warehouse).currentIndex = _App.rss(o.Parts).thisArray(2, sel(i))
                .Items(.Items.Count - 1).SubItems.Add(_App.rss(o.Warehouse).GetField("PARTDES"))
                .Items(.Items.Count - 1).SubItems.Add(_App.rss(o.Parts).thisArray(3, sel(i)))
            Next

            If .Items.Count > 0 Then
                .Columns(0).AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent)
                .Columns(2).AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent)
                .Columns(1).Width = .Width - (.Columns(2).Width + .Columns(0).Width + 5)
            Else
                .Columns(0).AutoResize(ColumnHeaderAutoResizeStyle.HeaderSize)
                .Columns(2).AutoResize(ColumnHeaderAutoResizeStyle.HeaderSize)
                .Columns(1).Width = .Width - (.Columns(2).Width + .Columns(0).Width + 5)
            End If
        End With

    End Sub

    Private Sub ct_Sign_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize

        Dim os As Integer = 2
        Dim l As Point
        Dim s As Size

        Dim fs As Single

        If Me.Width > 319 Then
            fs = 14
        ElseIf Me.Width > 268 Then
            fs = 12
        ElseIf Me.Width > 258 Then
            fs = 11
        ElseIf Me.Width > 241 Then
            fs = 10
        ElseIf Me.Width > 214 Then
            fs = 9
        ElseIf Me.Width > 199 Then
            fs = 8
        Else
            fs = 8
        End If

        Dim f As New Font("Microsoft Sans Serif", fs, FontStyle.Bold)
        Dim c As New Font("Microsoft Sans Serif", fs - 1, FontStyle.Regular)

        UsedParts.Font = c
        thisSig.Sign.Height = Me.Height / 4
        UsedParts.Height = Me.Height - (thisSig.Sign.Height + 5)
        Try
            With UsedParts
                If .Items.Count > 0 Then
                    .Columns(0).AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent)
                    .Columns(2).AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent)
                    .Columns(1).Width = .Width - (.Columns(2).Width + .Columns(0).Width + 5)
                Else
                    .Columns(0).AutoResize(ColumnHeaderAutoResizeStyle.HeaderSize)
                    .Columns(2).AutoResize(ColumnHeaderAutoResizeStyle.HeaderSize)
                    .Columns(1).Width = .Width - (.Columns(2).Width + .Columns(0).Width + 5)
                End If
            End With
        Catch
        End Try

    End Sub

    Public Sub SaveSig()
        With _App.rss(o.Signature)
            .SetField("SigData", thisSig.CompressSignature)
            .SetField("LOAD", "L")
            .Save()
            .BeginSigSave(_App.rss(o.Signature))
        End With
    End Sub

    Public Function ElapsedTime() As String

        Dim st As Date
        Dim et As Date

        Try
            With _App.rss(o.Time)
                .currentIndex = _ServiceCall
                If Not .Validate Then
                    Return "00:00"
                    Exit Function
                Else
                    st = CDate(.GetField("ONSITE"))
                    et = CDate(.GetField("END"))
                End If
            End With

            Dim min As Integer = DateDiff(DateInterval.Minute, st, et)
            Dim hr As String = Split(CStr(min / 60), ".")(0)
            Dim mn As String = Strings.Right("00" & CStr(min - CInt(hr * 60)), 2)

            Return CStr(hr & ":" & mn)

        Catch

            Return "00:00"

        End Try

    End Function

End Class
