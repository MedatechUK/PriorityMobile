Public Class ct_Sign
    Inherits System.Windows.Forms.UserControl

    Dim ar As cfMyCls.MyArray
    Dim _ServiceCall As String = ""
    Dim CallerApp As cfOnBoardData.BaseForm
    Dim thisSig As New cfMyCls.clsSign

    Public Property ServiceCall() As String
        Get
            Return _ServiceCall
        End Get
        Set(ByVal value As String)
            _ServiceCall = value
            NewData()
        End Set
    End Property

    Public Sub New(Optional ByRef App As cfOnBoardData.BaseForm = Nothing) '

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        If Not IsNothing(App) Then
            CallerApp = App
        End If

        thisSig.Sign.Dock = DockStyle.Bottom
        thisSig.Sign.Height = Me.Height / 4
        Me.Controls.Add(thisSig.Sign)

    End Sub

    Public Sub NewData()

        With CallerApp.rss(o.Signature)
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

        Dim ch1 As New ColumnHeader
        Dim ch2 As New ColumnHeader
        Dim ch3 As New ColumnHeader

        With UsedParts
            .Items.Clear()
            .Columns.Clear()
            ch1.Text = "Part Number"
            .Columns.Add(ch1)
            ch2.Text = "Part Description"
            .Columns.Add(ch2)
            ch3.Text = "QTY"
            .Columns.Add(ch3)

            CallerApp.rss(o.Parts).currentIndex = _ServiceCall
            Dim sel() As Integer = CallerApp.rss(o.Parts).Selection
            Dim lvi As New ListViewItem
            .Items.Add(lvi)
            .Items(.Items.Count - 1).Text = "Hours"
            .Items(.Items.Count - 1).SubItems.Add("Labour Time")
            .Items(.Items.Count - 1).SubItems.Add(ElapsedTime)

            For i As Integer = 0 To UBound(sel)
                Dim lvi2 As New ListViewItem
                .Items.Add(lvi2)
                .Items(.Items.Count - 1).Text = CallerApp.rss(o.Parts).thisArray(2, sel(i))
                CallerApp.rss(o.Warehouse).currentIndex = CallerApp.rss(o.Parts).thisArray(2, sel(i))
                .Items(.Items.Count - 1).SubItems.Add(CallerApp.rss(o.Warehouse).GetField("PARTDES"))
                .Items(.Items.Count - 1).SubItems.Add(CallerApp.rss(o.Parts).thisArray(3, sel(i)))
            Next
            AutoSizeListView(UsedParts, Me.Width, 1)
        End With

    End Sub

    Private Sub ct_Sign_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize

        Dim os As Integer = 2
        Dim l As Point
        Dim s As Size

        Dim fs As Single = GetFontSize(Me.Width)
        Dim f As New Font("Microsoft Sans Serif", fs, FontStyle.Bold)
        Dim c As New Font("Microsoft Sans Serif", fs - 1, FontStyle.Regular)

        UsedParts.Font = c
        thisSig.Sign.Height = 100
        thisSig.Sign.BackColor = Color.LightGray
        UsedParts.Height = Me.Height - (thisSig.Sign.Height + 5)
        AutoSizeListView(UsedParts, Me.Width - 25, 1)
        With UsedParts
            .Height = TextSize(.Text, .Font).Height * (.Items.Count + 3)
            If .Height < Me.Height - (thisSig.Sign.Height + 5) Then
                .Height = Me.Height - (thisSig.Sign.Height + 5)
            End If
        End With

    End Sub

    Public Sub SaveSig()
        With CallerApp.rss(o.Signature)
            .SetField("SigData", thisSig.CompressSignature)
            .SetField("LOAD", "L")
            .Save()
            .BeginSigSave(CallerApp.rss(o.Signature))
        End With
    End Sub

    Public Function ElapsedTime() As String

        Dim st As Date
        Dim et As Date

        Try
            With CallerApp.rss(o.Time)
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
