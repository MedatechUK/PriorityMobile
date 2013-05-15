Imports System.Xml

Public Class Survey

    Private SurveryQuestions As New List(Of QuestionBase)
    Private FormTitle As Label

    Private _FormLabel As String = ""
    Public Property FormLabel() As String
        Get
            Return _FormLabel
        End Get
        Set(ByVal value As String)
            _FormLabel = value
        End Set
    End Property

#Region "Label Resizing declarations"

    Private Structure RECT
        Public Left As Integer
        Public Top As Integer
        Public Right As Integer
        Public Bottom As Integer
    End Structure

    Private Const DT_CALCRECT As Integer = &H400
    Private Const DT_CENTER As Integer = &H1
    Private Const DT_LEFT As Integer = &H0
    Private Const DT_RIGHT As Integer = &H2
    Private Const DT_TOP As Integer = &H0
    Private Const DT_WORDBREAK As Integer = &H10

    Private Declare Function DeleteObject Lib "coredll.dll" (ByVal hObject As IntPtr) As Integer
    Private Declare Function DrawText Lib "coredll.dll" (ByVal hdc As IntPtr, ByVal lpStr As String, ByVal nCount As Integer, ByRef lpRect As RECT, ByVal wFormat As Integer) As Integer
    Private Declare Function SelectObject Lib "coredll.dll" (ByVal hdc As IntPtr, ByVal hObject As IntPtr) As IntPtr

#End Region

#Region "Response Event"

    Public Event NewResponse(ByVal QuestionNumber As Integer, ByVal Value As String, ByVal Text As String)

    Public Sub hNewResponse(ByVal QuestionNumber As Integer, ByVal Value As String, ByVal Text As String)
        RaiseEvent NewResponse(QuestionNumber, Value, Text)
    End Sub

#End Region

#Region "Draw Survey"

    Public Sub LoadSurvey(ByRef SurveyCategory As XmlNode)

        With Me

            FormTitle = New Label
            With FormTitle
                .ForeColor = Color.Blue
                .Font = New Font("Tahoma", 9, FontStyle.Bold)
                '.BackColor = Color.FromArgb(201, 220, 233)
                .Height = 30
                .Dock = DockStyle.Top
            End With

            .AutoScroll = False
            SurveryQuestions.Clear()
            .Controls.Clear()
            .AutoScrollPosition = New Point(0, 0)

            If _FormLabel.Length > 0 Then
                FormTitle.Text = _FormLabel
                .Controls.Add(FormTitle)
            End If

        End With

        Dim responseValue As String
        Dim responseText As String

        For Each question As XmlNode In SurveyCategory.SelectNodes("question")

            Dim questionNumber As Integer = CInt(question.SelectSingleNode("number").InnerText)
            Dim questionText As String = question.SelectSingleNode("text").InnerText
            Dim questionMandatory As Boolean = CBool(question.SelectSingleNode("mandatory").InnerText.Trim.ToLower = "y")
            Dim response As XmlNode = question.SelectSingleNode("response")

            With response
                responseText = .SelectSingleNode("text").InnerText
                responseValue = .SelectSingleNode("value").InnerText
            End With

            Dim Options As XmlNodeList = question.SelectNodes("option")

            With SurveryQuestions
                Select Case Options.Count
                    Case 0
                        If responseText.Length = 0 Then
                            .Add(New TextQuestion(questionNumber, questionText, , questionMandatory))
                        Else
                            .Add(New TextQuestion(questionNumber, questionText, responseText, questionMandatory))
                        End If

                    Case Else
                        Dim OptVal As New Dictionary(Of Integer, String)
                        Dim bool As Boolean = True
                        For Each opt As XmlNode In Options
                            Dim optText As String = opt.SelectSingleNode("text").InnerText
                            OptVal.Add(CInt(opt.SelectSingleNode("number").InnerText), optText)
                            Select Case optText.ToLower
                                Case "yes", "no", "y", "n", "true", "false"
                                Case Else
                                    bool = False
                            End Select
                        Next

                        Select Case bool And Options.Count = 2
                            Case True
                                If responseValue.Length = 0 Then
                                    .Add(New BoolQuestion(questionNumber, questionText, OptVal, , questionMandatory))
                                Else
                                    .Add(New BoolQuestion(questionNumber, questionText, OptVal, responseValue, questionMandatory))
                                End If
                            Case Else
                                If responseValue.Length = 0 Then
                                    .Add(New ChooseQuestion(questionNumber, questionText, OptVal, , questionMandatory))
                                Else
                                    .Add(New ChooseQuestion(questionNumber, questionText, OptVal, responseValue, questionMandatory))
                                End If
                        End Select

                End Select
            End With
            SurveryQuestions(SurveryQuestions.Count - 1).setResponseHandler(AddressOf hNewResponse)
            Me.Controls.Add(SurveryQuestions(SurveryQuestions.Count - 1))

        Next
        Me.AutoScroll = True
        Survey_Resize(Me, New System.EventArgs)

    End Sub

    Private Sub Survey_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Resize

        If SurveryQuestions.Count = 0 Then Exit Sub
        Try
            Dim y As Integer
            With Me
                If _FormLabel.Length > 0 Then
                    y = FormTitle.Height                    
                Else
                    y = 0
                End If
            End With

            Dim uRECT As RECT
            Dim objGraphics As Graphics = Me.CreateGraphics
            Dim hDc As IntPtr = objGraphics.GetHdc
            Dim hFont As IntPtr = SurveryQuestions(0).QuestionText.Font.ToHfont
            Dim hFontOld As IntPtr = SelectObject(hDc, hFont)
            Dim lFormat As Integer = DT_CALCRECT Or DT_WORDBREAK Or DT_TOP

            For Each Question As QuestionBase In SurveryQuestions
                With Question
                    .Top = y - Me.AutoScrollPosition.Y
                    .Left = 1
                    .Width = Me.Width - 30

                    With .QuestionText
                        .Top = 1
                        .Left = 1
                        .Width = Me.Width - 2
                        uRECT.Right = .Width
                        uRECT.Bottom = .Height
                        Try
                            If DrawText(hDc, .Text, -1, uRECT, lFormat) <> 0 Then ' Success
                                .Height = uRECT.Bottom
                            End If
                        Catch
                        End Try
                    End With

                    With .ResponsePanel
                        .Left = 1
                        .Width = Me.Width - 30
                        .Top = Question.QuestionText.Top + Question.QuestionText.Height
                    End With

                    .Height = .ResponsePanel.Top + .ResponsePanel.Height + 2
                    y += .Height + 5

                End With
            Next
            Try
                SelectObject(hDc, hFontOld)
                DeleteObject(hFont)
                objGraphics.Dispose()
            Catch
            End Try
        Catch
        End Try
    End Sub

#End Region

End Class
