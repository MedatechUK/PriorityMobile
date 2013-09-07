Imports System.Drawing

Public Class ScriptStep

    Public Enum eScriptButtons
        btn_Left = 0
        btn_Right = 1
        btn_Double = 2
    End Enum

    Public Enum eStepType
        step_Click = 0
        step_Drag = 1
        step_Wait = 2
        step_KeyPress = 3
    End Enum

    Public Overrides Function ToString() As String
        Select Case _StepType
            Case eStepType.step_Click
                Select Case _Button
                    Case eScriptButtons.btn_Double
                        Return String.Format("Double Click ({0},{1})", _ClickLoc.X, _ClickLoc.Y)
                    Case eScriptButtons.btn_Right
                        Return String.Format("Right Click ({0},{1})", _ClickLoc.X, _ClickLoc.Y)
                    Case Else
                        Return String.Format("Left Click ({0},{1})", _ClickLoc.X, _ClickLoc.Y)
                End Select

            Case eStepType.step_Drag
                Return String.Format("Drag from ({0},{1}) to ({2},{3})", _StartLoc.X, _StartLoc.Y, _EndLoc.X, _EndLoc.Y)

            Case eStepType.step_KeyPress
                If String.Format("Keypress '{0}'", _KeyText).Length > 20 Then
                    Return Microsoft.VisualBasic.Left(String.Format("Keypress '{0}'", _KeyText), 20) & "..."
                Else
                    Return String.Format("Keypress '{0}'", _KeyText)
                End If

            Case eStepType.step_Wait
                Return String.Format("Delay {0}ms", _Delay)

            Case Else
                Return String.Empty

        End Select
    End Function

    Public ReadOnly Property StepScript() As String
        Get
            Dim scr As New System.Text.StringBuilder
            With scr
                Select Case _StepType
                    Case eStepType.step_Click
                        Select Case _Button
                            Case eScriptButtons.btn_Double
                                .AppendFormat("M_MV({0},{1})", _ClickLoc.X, _ClickLoc.Y).AppendLine()
                                .AppendFormat("delay({0})", 200).AppendLine()
                                .AppendFormat("M_LD({0},{1})", _ClickLoc.X, _ClickLoc.Y).AppendLine()
                                .AppendFormat("delay({0})", 200).AppendLine()
                                .AppendFormat("M_LU({0},{1})", _ClickLoc.X, _ClickLoc.Y).AppendLine()
                                .AppendFormat("delay({0})", 200).AppendLine()
                                .AppendFormat("M_LD({0},{1})", _ClickLoc.X, _ClickLoc.Y).AppendLine()
                                .AppendFormat("delay({0})", 200).AppendLine()
                                .AppendFormat("M_LU({0},{1})", _ClickLoc.X, _ClickLoc.Y).AppendLine()
                                .AppendFormat("delay({0})", 500).AppendLine()

                            Case eScriptButtons.btn_Right
                                .AppendFormat("M_MV({0},{1})", _ClickLoc.X, _ClickLoc.Y).AppendLine()
                                .AppendFormat("delay({0})", 200).AppendLine()
                                .AppendFormat("M_RCLK({0},{1})", _ClickLoc.X, _ClickLoc.Y).AppendLine()
                                .AppendFormat("delay({0})", 500).AppendLine()

                            Case Else
                                .AppendFormat("M_MV({0},{1})", _ClickLoc.X, _ClickLoc.Y).AppendLine()
                                .AppendFormat("delay({0})", 200).AppendLine()
                                .AppendFormat("M_LCLK({0},{1})", _ClickLoc.X, _ClickLoc.Y).AppendLine()
                                .AppendFormat("delay({0})", 500).AppendLine()

                        End Select

                    Case eStepType.step_Drag
                        .AppendFormat("M_MV({0}, {1})", _StartLoc.X, _StartLoc.Y).AppendLine()
                        .AppendFormat("delay(500)").AppendLine()
                        .AppendFormat("M_LD({0}, {1})", _StartLoc.X, _StartLoc.Y).AppendLine()
                        .AppendFormat("delay(1000)").AppendLine()
                        .AppendFormat("M_MV({0}, {1})", _EndLoc.X, _EndLoc.Y).AppendLine()
                        .AppendFormat("delay(1000)").AppendLine()
                        .AppendFormat("M_LU({0}, {1})", _EndLoc.X, _EndLoc.Y).AppendLine()
                        .AppendFormat("delay({0})", 500).AppendLine()                        

                    Case eStepType.step_KeyPress
                        For Each K As String In _KeyText.Split(",")
                            .AppendFormat("{0}", K).AppendLine()
                            .AppendFormat("delay({0})", 200).AppendLine()
                        Next
                        .AppendFormat("delay({0})", 500).AppendLine()

                    Case eStepType.step_Wait
                        .AppendFormat("delay({0})", _Delay.ToString).AppendLine()                        

                End Select
            End With
            Return scr.ToString
        End Get
    End Property

    Private _Ord As Integer
    Public Property Ord() As Integer
        Get
            Return _Ord
        End Get
        Set(ByVal value As Integer)
            _Ord = value
        End Set
    End Property

    Private _StepType As eStepType
    Public ReadOnly Property StepType() As eStepType
        Get
            Return _StepType
        End Get
    End Property

#Region "Step Key Press"

    Private _KeyText As String
    Public Property KeyText() As String
        Get
            Return _KeyText
        End Get
        Set(ByVal value As String)
            _KeyText = value
        End Set
    End Property

    Public Sub New(ByVal ord As Integer, ByVal KeyText As String)
        _Ord = ord
        _StepType = eStepType.step_KeyPress
        _KeyText = KeyText
    End Sub

#End Region

#Region "Step Drag"

    Private _StartLoc As point
    Public Property StartLoc() As Point
        Get
            Return _StartLoc
        End Get
        Set(ByVal value As Point)
            _StartLoc = value
        End Set
    End Property

    Private _EndLoc As point
    Public Property EndLoc() As Point
        Get
            Return _EndLoc
        End Get
        Set(ByVal value As Point)
            _EndLoc = value
        End Set
    End Property

    Public Sub New(ByVal ord As Integer, ByVal StartLoc As Point, ByVal EndLoc As Point)
        _Ord = ord
        _StepType = eStepType.step_Drag
        _StartLoc = StartLoc
        _EndLoc = EndLoc
    End Sub

#End Region

#Region "Step Click"

    Private _ClickLoc As point
    Public Property ClickLoc() As Point
        Get
            Return _ClickLoc
        End Get
        Set(ByVal value As Point)
            _ClickLoc = value
        End Set
    End Property

    Private _Button As eScriptButtons
    Public Property Button() As eScriptButtons
        Get
            Return _Button
        End Get
        Set(ByVal value As eScriptButtons)
            _Button = value
        End Set
    End Property

    Public Sub New(ByVal ord As Integer, ByVal ClickLoc As Point, Optional ByVal Button As eScriptButtons = eScriptButtons.btn_Left)
        _Ord = ord
        _StepType = eStepType.step_Click
        _ClickLoc = ClickLoc
        _Button = Button
    End Sub

#End Region

#Region "Step Wait"

    Private _Delay As Integer
    Public Property Delay() As Integer
        Get
            Return _Delay
        End Get
        Set(ByVal value As Integer)
            _Delay = value
        End Set
    End Property

    Public Sub New(ByVal ord As Integer, ByVal Delay As Integer)
        _Ord = ord
        _StepType = eStepType.step_Wait
        _Delay = Delay
    End Sub

#End Region

End Class
