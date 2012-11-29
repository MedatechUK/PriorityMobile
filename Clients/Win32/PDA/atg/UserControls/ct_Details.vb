Public Class ct_Details
    Dim ar As Priority.MyArray

    Dim _ServiceCall As String = ""
    Dim _App As PDAOnBoardData.BaseForm

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

    End Sub

    Public Sub NewData()

        If Not IsNothing(_App.rss(o.ServiceCall).thisArray) Then
            With _App.rss(o.ServiceCall)
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
        If Not IsNothing(_App.rss(o.Details).thisArray) Then
            With _App.rss(o.Details)

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

        With Me
            .lbl_Requested.Font = f
            .lbl_ReqDate.Font = c
            .lbl_ReqDate.Left = .lbl_Requested.Width + os

            .lbl_ServTerms.Left = .lbl_ReqDate.Left

            With .lbl_Terms
                .Font = f
                .Top = Me.lbl_Requested.Top + Me.lbl_Requested.Height + (os * 2)
            End With

            With .lbl_ServTerms
                .Font = c
                .Top = Me.lbl_Terms.Top
            End With

            .txt_CallDetail.Height = .Height - (.lbl_ServTerms.Top + .lbl_ServTerms.Height + (os * 2))

        End With

    End Sub

End Class
