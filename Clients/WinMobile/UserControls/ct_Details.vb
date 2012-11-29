Public Class ct_Details
    Dim ar As cfMyCls.MyArray

    Dim _ServiceCall As String = ""
    Dim CallerApp As cfOnBoardData.BaseForm

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

    End Sub

    Public Sub NewData()

        If Not IsNothing(CallerApp.rss(o.ServiceCall).thisArray) Then
            With CallerApp.rss(o.ServiceCall)
                .currentIndex = _ServiceCall
                If Not .Validate Then
                    Exit Sub
                Else
                    Me.lbl_ReqDate.Text = CStr(Format(DateAdd(DateInterval.Minute, CDbl(.GetField("DATEOPENED")), #1/1/1988#), "dd/MM/yy"))
                    Me.lbl_ServTerms.Text = CStr(.GetField("SERVDES"))
                End If
            End With
        End If

        Dim tmp As String = ""
        If Not IsNothing(CallerApp.rss(o.Details).thisArray) Then
            With CallerApp.rss(o.Details)

                Dim i As Integer = 1
                .currentIndex = _ServiceCall & "-" & CStr(i)
                Do While .Validate
                    tmp = tmp & .GetField("TEXT")
                    i = i + 1
                    .currentIndex = _ServiceCall & "-" & i
                Loop
                tmp = Strings.Right(tmp, Len(tmp) - (InStr(tmp, "<p>", CompareMethod.Text) - 1))
                Me.txt_CallDetail.DocumentText = tmp
            End With
        End If

    End Sub

    Private Sub ct_Details_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize

        Dim os As Integer = 2
        Dim l As Point
        Dim s As Size

        Dim fs As Single = GetFontSize(Me.Width)

        Dim f As New Font("Microsoft Sans Serif", fs, FontStyle.Bold)
        Dim c As New Font("Microsoft Sans Serif", fs - 1, FontStyle.Regular)

        With Me

            With .lbl_Terms
                .Font = f
                .Width = TextSize(.Text, .Font).Width
                .Height = TextSize(.Text, .Font).Height                
                .Left = os
            End With

            With .lbl_Requested
                .Top = 0
                .Font = f
                .Width = TextSize(.Text, .Font).Width
                .Height = TextSize(.Text, .Font).Height
            End With

            With lbl_Terms
                .Left = Me.lbl_Requested.Left + (lbl_Requested.Width - .Width)
                .Top = Me.lbl_Requested.Top + Me.lbl_Requested.Height + os
            End With

            With .lbl_ReqDate
                .Font = c
                .Width = TextSize(.Text, .Font).Width
                .Height = TextSize(.Text, .Font).Height
                .Top = Me.lbl_Requested.Top
                .Left = Me.lbl_Requested.Left + Me.lbl_Requested.Width + os
            End With

            With .lbl_ServTerms
                .Font = c
                .Width = TextSize(.Text, .Font).Width
                .Height = TextSize(.Text, .Font).Height
                .Top = Me.lbl_Terms.Top
                .Left = Me.lbl_Terms.Left + Me.lbl_Terms.Width + os
            End With

            .txt_CallDetail.Height = .Height - (.lbl_ServTerms.Top + .lbl_ServTerms.Height + (os * 2))

        End With

    End Sub

End Class
