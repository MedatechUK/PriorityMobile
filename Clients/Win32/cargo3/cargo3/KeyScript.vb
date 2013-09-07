Imports System.Xml
Imports System.Drawing

Public Class KeyScript

    Private _nextord As Integer = -1
    Public ReadOnly Property NextOrd() As Integer
        Get
            _nextord += 1
            Return _nextord
        End Get
    End Property

    Private _Name As String = ""
    Public ReadOnly Property Name() As String
        Get
            Return _Name
        End Get
    End Property

    Private _Steps As New List(Of ScriptStep)
    Public Property Steps() As List(Of ScriptStep)
        Get
            Return _Steps
        End Get
        Set(ByVal value As List(Of ScriptStep))
            _Steps = value
        End Set
    End Property

    Public Sub New(ByVal Name As String, ByVal Script As XmlNode)
        _Name = Name

        If Not IsNothing(Script) Then
            With _Steps
                For Each stp As XmlNode In Script.SelectNodes("step")
                    If Not IsNothing(stp.Attributes("text")) Then
                        .Add(New ScriptStep(NextOrd, stp.Attributes("text").Value))

                    ElseIf Not IsNothing(stp.Attributes("delay")) Then
                        .Add(New ScriptStep(NextOrd, CInt(stp.Attributes("delay").Value)))

                    ElseIf Not IsNothing(stp.Attributes("x1")) And Not IsNothing(stp.Attributes("y1")) And Not IsNothing(stp.Attributes("x2")) And Not IsNothing(stp.Attributes("y2")) Then
                        Dim startloc As Point = New Point((stp.Attributes("x1").Value), (stp.Attributes("y1").Value))
                        Dim endloc As Point = New Point((stp.Attributes("x2").Value), (stp.Attributes("y2").Value))
                        .Add(New ScriptStep(NextOrd, startloc, endloc))

                    ElseIf Not IsNothing(stp.Attributes("x")) And Not IsNothing(stp.Attributes("y")) Then
                        Dim clickloc As New Point((stp.Attributes("x").Value), (stp.Attributes("y").Value))
                        If Not IsNothing(stp.Attributes("button")) Then
                            .Add( _
                                New ScriptStep( _
                                    NextOrd, _
                                    clickloc, _
                                    ScriptButton(stp.Attributes("button").Value) _
                                ) _
                            )
                        Else
                            .Add( _
                                New ScriptStep( _
                                    NextOrd, _
                                    clickloc _
                                ) _
                            )
                        End If

                    Else
                        Throw New Exception( _
                             String.Format( _
                                 "Invalid parameters for [{0}] step type in key script {1}.", _
                                 stp.Attributes("type").Value, _
                                 _Name _
                             ) _
                         )
                    End If
                Next
            End With
        End If

    End Sub

    Public Sub Execute()
        Me.Steps.Sort(AddressOf Sortstep)
        Dim sc As New System.Text.StringBuilder
        For Each st As ScriptStep In Me.Steps
            sc.Append(st.StepScript)
        Next
        sc.Append("KB_CLK(27)").AppendLine()

        Dim fn As String = My.Computer.FileSystem.CombinePath(My.Computer.FileSystem.CurrentDirectory, "script.scp")
        Using sw As New System.IO.StreamWriter(fn)
            sw.Write(sc.ToString)
        End Using

        Dim myProcess As Process = New Process()
        With myProcess
            With .StartInfo
                .FileName = "auto.exe"
                .Arguments = fn
                .WorkingDirectory = My.Computer.FileSystem.CurrentDirectory
                .UseShellExecute = True
                .CreateNoWindow = False
            End With
            .Start()
        End With

    End Sub

    Private Function Sortstep(ByVal step1 As cargo3.ScriptStep, ByVal step2 As cargo3.ScriptStep) As Integer
        Return step1.Ord.CompareTo(step2.Ord)
    End Function

    Private Function ScriptButton(ByVal Name As String) As eScriptButtons
        Select Case Name.ToLower
            Case "right"
                Return eScriptButtons.btn_right
            Case "double"
                Return eScriptButtons.btn_double
            Case Else
                Return eScriptButtons.btn_left
        End Select
    End Function

End Class
