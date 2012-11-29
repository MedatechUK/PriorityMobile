Public Class ct_Survey
    Dim ar As PRIORITY.MyArray

    Private thisAnswer As String = ""

    Dim _mode As Integer
    Dim _ServiceCall As String = ""
    Dim _SurveyName As String = ""
    Dim _SurveyNum As String = ""
    Dim _SurveyQuestions(,) As String
    Dim _QuestionSet() As String
    Dim _App As PDAOnBoardData.BaseForm

    Dim qar() As Object
    Dim aran() As ComboBox
    Dim surveyNum As Integer
    Dim SurveySign As PRIORITY.clsSign

#Region "Public Property Declarations"
    Public ReadOnly Property Mode()
        Get
            Return _mode
        End Get
    End Property

    Public Property ServiceCall() As String
        Get
            Return _ServiceCall
        End Get
        Set(ByVal value As String)
            _ServiceCall = value
        End Set
    End Property

    Public Property SurveyName() As String
        Get
            Return _SurveyName
        End Get
        Set(ByVal value As String)
            _SurveyName = value
            _SurveyQuestions = ar.SubSet(_App.rss(o.Survey).thisArray, 1, _SurveyName)
            _SurveyNum = _SurveyQuestions(0, 0)
            _QuestionSet = ar.Group(_SurveyQuestions, 2)
        End Set
    End Property

#End Region

#Region "Initialisation"

    Public Sub New(Optional ByRef App As PDAOnBoardData.BaseForm = Nothing) '

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        If Not IsNothing(App) Then
            _App = App
        End If

        ar = New PRIORITY.MyArray

        'Me.VerticalScroll.Visible = True
        Me.AutoSizeMode = Windows.Forms.AutoSizeMode.GrowAndShrink

    End Sub

#End Region

#Region "Survey Selection"

    Public Sub DrawMenu()

        Dim Surveys() As String = ar.Group(_App.rss(o.Survey).thisArray, 1)

        With Me.lst_Survey
            .Clear()
            .Columns.Clear()
            .Columns.Add("Survey Name")
            .Columns.Add("Has Data")
            For i As Integer = 0 To UBound(Surveys)
                .Items.Add("")
                .Items(.Items.Count - 1).Text = Surveys(i)
                If Has_Answers(_ServiceCall, Surveys(i)) Then
                    .Items(.Items.Count - 1).SubItems.Add("True")
                Else
                    .Items(.Items.Count - 1).SubItems.Add("False")
                End If
            Next
            If .Items.Count > 0 Then
                .Columns(0).AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent)
                .Columns(1).AutoResize(ColumnHeaderAutoResizeStyle.HeaderSize)
            Else
                .Columns(0).AutoResize(ColumnHeaderAutoResizeStyle.HeaderSize)
                .Columns(1).AutoResize(ColumnHeaderAutoResizeStyle.HeaderSize)
            End If
        End With

        ShowPanel(Me.pnl_SelectSurvey, True)
        ShowPanel(Me.pnl_Survey, False)
        ShowPanel(Me.pnl_TextAnswer, False)

        _mode = 0

    End Sub

    Private Sub lst_Survey_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lst_Survey.SelectedIndexChanged

        ShowPanel(Me.pnl_Survey, True)
        ShowPanel(Me.pnl_TextAnswer, False)
        ShowPanel(Me.pnl_SelectSurvey, False)

        _mode = 1

        Dim sel As Integer = lst_Survey.SelectedIndices(0)
        Me.SurveyName = lst_Survey.Items(sel).Text
        Me.NewData()

    End Sub

#End Region

#Region "Draw Suvey"

    Public Sub NewData()

        Me.pnl_Survey.Controls.Clear()

        Dim tit As New Label
        With tit
            .Name = "TIT:"
            .AutoSize = True
            .Text = _SurveyName
        End With
        Me.pnl_Survey.Controls.Add(tit)

        For i As Integer = 0 To UBound(_QuestionSet)
            Dim answers(,) As String = ar.SubSet(_SurveyQuestions, 2, _QuestionSet(i))

            Dim lbl As New Label
            With lbl
                .Name = "LBL:" & _QuestionSet(i)
                .AutoSize = True
                .Text = answers(3, 0)
            End With
            Me.pnl_Survey.Controls.Add(lbl)

            If Not IsYesNo(answers) Then
                Dim list As New ComboBox
                With list
                    .Name = "LST:" & _QuestionSet(i)
                    .Items.Add("")
                    For a As Integer = 0 To UBound(answers, 2)
                        .Items.Add(answers(5, a))
                        If Answer(_QuestionSet(i)) = answers(5, a) Then
                            list.SelectedText = answers(5, a)
                        End If
                    Next
                    AddHandler .SelectedValueChanged, AddressOf hSelectedValueChanged
                End With
                Me.pnl_Survey.Controls.Add(list)
            Else
                Dim tick As New CheckBox
                With tick
                    .Name = "TCK:" & _QuestionSet(i)
                    Select Case LCase(Answer(_QuestionSet(i)))
                        Case "y"
                            .Checked = True
                        Case "n"
                            .Checked = False
                        Case Else
                            .Checked = False
                    End Select
                    AddHandler .CheckedChanged, AddressOf hTick_CheckedChanged
                End With
                Me.pnl_Survey.Controls.Add(tick)
            End If

            Dim hyper As New LinkLabel
            With hyper
                .Name = "HYP:" & _QuestionSet(i)
                .Text = "More"
                AddHandler .LinkClicked, AddressOf hLinkClicked
            End With
            Me.pnl_Survey.Controls.Add(hyper)

        Next

        Dim label As New Label
        With label
            .Text = "Signature:"
            .Name = "LBL:SIG"
        End With
        Me.pnl_Survey.Controls.Add(label)

        SurveySign = New PRIORITY.clsSign
        With SurveySign
            .Name = "SIG:"
            Me.pnl_Survey.Controls.Add(.Sign)
        End With

        LoadSignature(SurveySign)

        Dim e As System.EventArgs = Nothing
        ct_Survey_Resize(Me, e)

    End Sub

    Sub CloseTextEdit()

        With _App.rss(o.Answers)
            .currentIndex = _ServiceCall & "/" & _SurveyNum & "/" & thisAnswer
            If Not .Validate Then
                Dim nr As Integer = .NewRecord
                .thisArray(0, nr) = .currentIndex
                .thisArray(1, nr) = _ServiceCall
                .thisArray(2, nr) = _SurveyNum
                .thisArray(3, nr) = thisAnswer
                .SetField("ANSTEXT", Me.txt_Answer.Text)
                .Save()
            Else
                .SetField("ANSTEXT", Me.txt_Answer.Text)
                .Save()
            End If
        End With

        ShowPanel(Me.pnl_TextAnswer, False)
        ShowPanel(Me.pnl_SelectSurvey, False)
        ShowPanel(Me.pnl_Survey, True)

        thisAnswer = ""
        _mode = 1

    End Sub

#End Region

#Region "Survey Control Event Handlers"

    Sub hSelectedValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)

        Dim ans = _QuestionSet(CInt(Split(sender.name, ":")(1)))

        With _App.rss(o.Answers)
            .currentIndex = _ServiceCall & "/" & _SurveyNum & "/" & ans
            If Not .Validate Then
                Dim nr As Integer = .NewRecord
                .thisArray(0, nr) = .currentIndex
                .thisArray(1, nr) = _ServiceCall
                .thisArray(2, nr) = _SurveyNum
                .thisArray(3, nr) = ans
                .SetField("ANSNUM", sender.text)
                .Save()
            Else
                .SetField("ANSNUM", sender.text)
                .Save()
            End If
        End With

    End Sub

    Sub hTick_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)

        Dim ans = _QuestionSet(CInt(Split(sender.name, ":")(1)))

        With _App.rss(o.Answers)
            .currentIndex = _ServiceCall & "/" & _SurveyNum & "/" & ans
            If Not .Validate Then
                Dim nr As Integer = .NewRecord
                .thisArray(0, nr) = .currentIndex
                .thisArray(1, nr) = _ServiceCall
                .thisArray(2, nr) = _SurveyNum
                .thisArray(3, nr) = ans
                If sender.checked Then
                    .SetField("ANSNUM", "Y")
                Else
                    .SetField("ANSNUM", "N")
                End If
                .Save()
            Else
                If sender.checked Then
                    .SetField("ANSNUM", "Y")
                Else
                    .SetField("ANSNUM", "Y")
                End If
                .Save()
            End If
        End With

    End Sub

    Sub hLinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs)

        ShowPanel(Me.pnl_TextAnswer, True)
        ShowPanel(Me.pnl_SelectSurvey, False)
        ShowPanel(Me.pnl_Survey, False)

        thisAnswer = Split(sender.name, ":")(1)

        Me.lbl_Question.Text = QuestionText(thisAnswer)
        Me.txt_Answer.Text = ExtendedAnswer(thisAnswer)

        _mode = 2

    End Sub

#End Region

#Region "Resizing Code"

    Private Sub ct_Survey_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize

        Static winHeight As Integer = 0

        winHeight = Me.pnl_Survey.Height

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

        If Me.pnl_Survey.Visible Then
            Me.lst_Survey.Font = c
            Me.pnl_Survey.Height = 0

            Dim y As Integer = os
            For i As Integer = 0 To Me.pnl_Survey.Controls.Count - 1
                With Me.pnl_Survey.Controls.Item(i)
                    .Font = c
                    Select Case Split(.Name, ":")(0)
                        Case "TIT"
                            .Font = f
                            s.Width = Me.Width - (os * 2) - 20
                            s.Height = 0
                            .MaximumSize = s
                        Case "LST"
                            .Width = Me.Width - (os * 2) - 20
                        Case "LBL"
                            s.Width = Me.Width - (os * 2) - 20
                            s.Height = 0
                            .MaximumSize = s
                        Case "SIG"
                            .Width = Me.Width - (os * 2) - 20
                            .Height = Me.Height / 4
                    End Select


                    .Top = y
                    .Left = os

                    'If Split(.Name, ":")(0) = "LBL" Then .Height = CalculateHeight(Me.Controls.Item(i))
                    y = y + .Height + os

                End With
            Next
        End If

        If Me.pnl_SelectSurvey.Visible Then
            With Me.lst_Survey
                If .Items.Count > 0 Then
                    .Columns(0).AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent)
                    .Columns(1).AutoResize(ColumnHeaderAutoResizeStyle.HeaderSize)
                Else
                    .Columns(0).AutoResize(ColumnHeaderAutoResizeStyle.HeaderSize)
                    .Columns(1).AutoResize(ColumnHeaderAutoResizeStyle.HeaderSize)

                End If
                .Font = c
            End With
        End If

        If Me.pnl_TextAnswer.Visible Then

            Me.txt_Answer.Font = c
            Me.lbl_Question.Font = f

            s.Width = Me.Width - (os * 2) - 20
            s.Height = 0

            Me.lbl_Question.MaximumSize = s
            'lbl_Question.PerformLayout()
            'txt_Answer.Height = Me.Height - (lbl_Question.Top + lbl_Question.Size.Height + os)

        End If

        Me.Refresh()

    End Sub

    Private Sub ShowPanel(ByRef pnl As Panel, ByVal Show As Boolean)

        With pnl
            Select Case Show
                Case False
                    .Visible = False
                    .Dock = DockStyle.None
                    .Top = Me.Height
                    .Left = 0
                    .Width = 0
                    .Height = 0
                Case True
                    .Visible = True
                    .Dock = DockStyle.Fill
                    .Top = 0
                    .Left = 0
                    .Width = Me.Width
                    .Height = Me.Height
            End Select
        End With

    End Sub

#End Region

#Region "Signature Code"

    Sub LoadSignature(ByVal SignCtrl As PRIORITY.clsSign)

        With _App.rss(o.Signature)
            .currentIndex = _ServiceCall & "/" & _SurveyNum
            If .Validate Then
                SignCtrl.coord = SignCtrl.UnpackSignature(.GetField("SigData"))
                ' Set the upper bound
                SignCtrl.uc = UBound(SurveySign.coord, 2)
                ' Redraw the signature
                SignCtrl.Sign.Invalidate()
            Else
                SignCtrl.Reset()
                Dim nr As Integer = .NewRecord
                .thisArray(0, nr) = .currentIndex
                .thisArray(1, nr) = ""
                .thisArray(2, nr) = ""
                .thisArray(3, nr) = ""
                .Save()
            End If
        End With

    End Sub

    Public Sub SaveSurvey()

        With _App.rss(o.Answers)

            For i As Integer = 0 To pnl_Survey.Controls.Count - 1

                Select Case Split(pnl_Survey.Controls.Item(i).Name, ":")(0)
                    Case "TCK"
                        Dim tck As CheckBox = pnl_Survey.Controls.Item(i)

                        .currentIndex = _ServiceCall & "/" & _SurveyNum & "/" & Split(pnl_Survey.Controls.Item(i).Name, ":")(1)
                        If Not .Validate Then
                            Dim nr As Integer = .NewRecord
                            .thisArray(0, nr) = .currentIndex
                            .thisArray(1, nr) = _ServiceCall
                            .thisArray(2, nr) = _SurveyNum
                            .thisArray(3, nr) = Split(pnl_Survey.Controls.Item(i).Name, ":")(1)
                            If tck.Checked Then
                                .SetField("ANSNUM", "Y")
                            Else
                                .SetField("ANSNUM", "N")
                            End If
                            .Save()
                        Else
                            If tck.Checked Then
                                .SetField("ANSNUM", "Y")
                            Else
                                .SetField("ANSNUM", "N")
                            End If
                            .Save()
                        End If

                    Case "LST"
                        Dim lst As ComboBox = pnl_Survey.Controls.Item(i)

                        .currentIndex = _ServiceCall & "/" & _SurveyNum & "/" & Split(pnl_Survey.Controls.Item(i).Name, ":")(1)
                        If Not .Validate Then
                            Dim nr As Integer = .NewRecord
                            .thisArray(0, nr) = .currentIndex
                            .thisArray(1, nr) = _ServiceCall
                            .thisArray(2, nr) = _SurveyNum
                            .thisArray(3, nr) = Split(pnl_Survey.Controls.Item(i).Name, ":")(1)
                            .SetField("ANSNUM", lst.Text)
                            .Save()
                        Else
                            .SetField("ANSNUM", lst.Text)
                            .Save()
                        End If

                End Select

            Next

            .Save()

        End With

        _App.rss(o.Signature).currentIndex = _ServiceCall & "/" & _SurveyNum
        If _App.rss(o.Signature).Validate Then
            With _App.rss(o.Signature)
                .SetField("SigData", SurveySign.CompressSignature)
                .SetField("LOAD", "L")
                .Save()
                .BeginSigSave(_App.rss(o.Signature))
            End With
        End If

    End Sub

#End Region

#Region "Private Functions"

    Private Function IsYesNo(ByVal answers(,) As String) As Boolean
        For a As Integer = 0 To UBound(answers, 2)
            Select Case LCase(Trim(answers(5, a)))
                Case "y", "n"
                Case Else
                    Return False
            End Select
        Next
        Return True
    End Function

    Private Function Answer(ByVal QuestNum As String) As String

        Dim cond(1, 2) As String
        cond(0, 0) = "1"
        cond(1, 0) = _ServiceCall
        cond(0, 1) = "2"
        cond(1, 1) = _SurveyNum
        cond(0, 2) = "3"
        cond(1, 2) = QuestNum

        If IsNothing(_App.rss(o.Answers).thisArray) Then
            Return Nothing
        Else
            Dim tmp() As String = ar.IntToStr1d(ar.Matching(_App.rss(o.Answers).thisArray, cond))
            If UBound(tmp) = -1 Then
                Return Nothing
            Else
                Return _App.rss(o.Answers).thisArray(4, tmp(0))
            End If
        End If

    End Function

    Private Function ExtendedAnswer(ByVal QuestNum As String) As String

        Dim cond(1, 2) As String
        cond(0, 0) = "1"
        cond(1, 0) = _ServiceCall
        cond(0, 1) = "2"
        cond(1, 1) = _SurveyNum
        cond(0, 2) = "3"
        cond(1, 2) = QuestNum

        If IsNothing(_App.rss(o.Answers).thisArray) Then
            Return Nothing
        Else
            Dim tmp() As String = ar.IntToStr1d(ar.Matching(_App.rss(o.Answers).thisArray, cond))
            If UBound(tmp) = -1 Then
                Return Nothing
            Else
                Return _App.rss(o.Answers).thisArray(5, tmp(0))
            End If
        End If

    End Function

    Private Function Has_Answers(ByVal ServiceCall As String, ByVal SurveyName As String) As Boolean

        Dim SurveyQuestions = ar.SubSet(_App.rss(o.Survey).thisArray, 1, SurveyName)
        Dim surveyNum As String = SurveyQuestions(0, 0)

        If IsNothing(_App.rss(o.Answers).thisArray) Then
            Return False
        Else
            Dim cond(1, 1) As String
            cond(0, 0) = "1"
            cond(1, 0) = ServiceCall
            cond(0, 1) = "2"
            cond(1, 1) = surveyNum

            Dim tmp() As Integer = ar.Matching(_App.rss(o.Answers).thisArray, cond)
            If UBound(tmp) = -1 Then
                Return False
            Else
                Return True
            End If
        End If
    End Function

    Private Function CalculateHeight(ByVal l As Label) As Single

        Dim g As Graphics = Me.CreateGraphics
        Dim h As Integer = g.MeasureString(l.Text, l.Font).Height
        Dim w As Integer = g.MeasureString(l.Text, l.Font).Width
        Return h + (h * w / Me.Width)

    End Function

    Private Function QuestionText(ByVal Id As String) As String

        Dim cond(1, 1) As String
        cond(0, 0) = "0"
        cond(1, 0) = _SurveyNum
        cond(0, 1) = "2"
        cond(1, 1) = Id
        Dim tmp() As String = ar.IntToStr1d(ar.Matching(_App.rss(o.Survey).thisArray, cond))
        If Not IsNothing(tmp) Then
            Return _App.rss(o.Survey).thisArray(3, tmp(0))
        Else
            Return Nothing
        End If

    End Function

#End Region

End Class
