Imports System.Xml
Imports System.Drawing

Public Class States
    Inherits Dictionary(Of String, State)

    ' Runtime Objects
    Public CurrentState As cargo3.State
    Public CurrentAction As cargo3.Action

    Private Doc As New XmlDocument

    Public Sub New(ByRef frmScript As cargo3.ScriptFrm)
        ScriptForm = frmScript
        ScriptForm.CloseOnEscape = True
    End Sub

    Private _FileName As String = String.Empty
    Public Property Filename() As String
        Get
            Return _FileName
        End Get
        Set(ByVal value As String)
            _FileName = value
        End Set
    End Property

    Private _ScreenW As Integer
    Public ReadOnly Property ScreenW() As Integer
        Get
            Return _ScreenW + 10
        End Get
    End Property

    Private _ScreenH As Integer
    Public ReadOnly Property ScreenH() As Integer
        Get
            Return _ScreenH + 10
        End Get
    End Property

    Public Property myScripts() As Dictionary(Of String, KeyScript)
        Get
            Return KeyScripts
        End Get
        Set(ByVal value As Dictionary(Of String, KeyScript))
            KeyScripts = value
        End Set
    End Property

    Public Sub Load(ByVal FileName As String)

        _FileName = FileName
        Doc.Load(FileName)
        Dim cargo As XmlNode = Doc.SelectSingleNode("cargo")

        For Each ks As XmlNode In cargo.SelectNodes("scripts/script")
            If Not KeyScripts.Keys.Contains(ks.Attributes("name").Value) Then
                KeyScripts.Add(ks.Attributes("name").Value, New KeyScript(ks.Attributes("name").Value, ks))
            Else
                Throw New Exception( _
                    String.Format( _
                      "Duplicate key script {0}", _
                      ks.Attributes("name").Value _
                    ) _
                )
            End If
        Next

        Dim defCnt As Integer = 0
        For Each st As XmlNode In cargo.SelectNodes("state")
            If Not Me.Keys.Contains(st.Attributes("name").Value) Then
                If Not IsNothing(st.Attributes("default")) Then defCnt += 1
                Me.Add(st.Attributes("name").Value, New State(st.Attributes("name").Value, CBool(Not IsNothing(st.Attributes("default")))))
                With Me(st.Attributes("name").Value)
                    For Each act As XmlNode In st.SelectNodes("action")
                        If Not .Actions.Keys.Contains(act.Attributes("name").Value) Then
                            .Actions.Add(act.Attributes("name").Value, New Action(Me(st.Attributes("name").Value).NextOrd, act.Attributes("name").Value, st.Name, act.Attributes("logic")))
                        Else
                            Throw New Exception( _
                                String.Format( _
                                  "Duplicate Action {0} in state {1}.", _
                                  act.Attributes("name").Value, _
                                  st.Attributes("name").Value _
                                ) _
                            )
                        End If
                    Next
                End With
            Else
                Throw New Exception( _
                    String.Format( _
                      "Duplicate state {0}", _
                      st.Attributes("name").Value _
                    ) _
                )
            End If
        Next

        Select Case defCnt
            Case 0
                Me.Values(0).DefaultState = True
            Case 1

            Case Else
                Dim fst As Boolean = False
                For Each st As State In Me.Values
                    If st.DefaultState Then
                        If Not fst Then
                            fst = True
                        Else
                            st.DefaultState = False
                        End If
                    End If
                Next

        End Select

        For Each st As State In Me.Values
            For Each act As Action In st.Actions.Values
                Dim thisaction As XmlNode = cargo.SelectSingleNode(String.Format("state[@name='{0}']/action[@name='{1}']", st.Name, act.Name))
                With thisaction
                    For Each cond As XmlNode In .SelectNodes("condition")
                        If Not act.Conditions.Keys.Contains(cond.Attributes("name").Value) Then
                            If IsNothing(cond.Attributes("t")) Then
                                act.Conditions.Add( _
                                    cond.Attributes("name").Value, _
                                        New Condition( _
                                            cond.Attributes("name").Value, _
                                            New Point(CInt(cond.Attributes("x").Value), CInt(cond.Attributes("y").Value)), _
                                            New rgb(CInt(cond.Attributes("r").Value), CInt(cond.Attributes("g").Value), CInt(cond.Attributes("b").Value)) _
                                        ) _
                                )
                            Else
                                act.Conditions.Add( _
                                    cond.Attributes("name").Value, _
                                        New Condition( _
                                            cond.Attributes("name").Value, _
                                            New Point(CInt(cond.Attributes("x").Value), CInt(cond.Attributes("y").Value)), _
                                            New rgb(CInt(cond.Attributes("r").Value), CInt(cond.Attributes("g").Value), CInt(cond.Attributes("b").Value)), CInt(cond.Attributes("t").Value) _
                                        ) _
                                )
                            End If
                            If CInt(cond.Attributes("x").Value) > _ScreenW Then _ScreenW = CInt(cond.Attributes("x").Value)
                            If CInt(cond.Attributes("y").Value) > _ScreenH Then _ScreenH = CInt(cond.Attributes("y").Value)
                        Else
                            Throw New Exception( _
                             String.Format( _
                                "Duplicate Condition {0} State {1} Action: {2}", _
                                cond.Attributes("name").Value, _
                                st.Name, _
                                act.Name _
                             ) _
                         )
                        End If
                    Next

                    For Each res As XmlNode In .SelectNodes("result")
                        If Not (act.Results.Keys.Contains(CBool(res.Attributes("if").Value))) Then
                            act.Results.Add(CBool(res.Attributes("if").Value), New Result)
                            With act.Results(CBool(res.Attributes("if").Value))

                                If Not IsNothing(res.Attributes("nextaction")) Then
                                    If Me(st.Name).Actions.Keys.Contains(res.Attributes("nextaction").Value) Then
                                        .NextAction = Me(st.Name).Actions(res.Attributes("nextaction").Value)
                                    Else
                                        Throw New Exception( _
                                         String.Format( _
                                            "Invalid nextaction {0} State {1} Action: {2}", _
                                            res.Attributes("nextaction").Value, _
                                            st.Name, _
                                            act.Name _
                                         ) _
                                     )
                                    End If
                                End If

                                If Not IsNothing(res.Attributes("nextstate")) Then
                                    If Me.Keys.Contains(res.Attributes("nextstate").Value) Then
                                        .NextState = Me(res.Attributes("nextstate").Value)
                                    Else
                                        Throw New Exception( _
                                         String.Format( _
                                            "Invalid nextstate {0} State {1} Action: {2}", _
                                            res.Attributes("nextaction").Value, _
                                            st.Name, _
                                            act.Name _
                                         ) _
                                     )
                                    End If
                                End If

                                If Not IsNothing(res.Attributes("script")) Then
                                    If KeyScripts.Keys.Contains(res.Attributes("script").Value) Then
                                        .Script = KeyScripts(res.Attributes("script").Value)
                                    Else
                                        Throw New Exception( _
                                            String.Format( _
                                                "Invalid key script {0} State {1} Action: {2}", _
                                                res.Attributes("script").Value, _
                                                st.Name, _
                                                act.Name _
                                            ) _
                                        )
                                    End If
                                End If

                            End With
                        Else
                            Throw New Exception( _
                                String.Format( _
                                  "Duplicate result: State {0} Action: {1}", _
                                  st.Name, _
                                  act.Name _
                                ) _
                            )
                        End If
                    Next
                End With
            Next
        Next

    End Sub

    Public Sub Save(Optional ByVal FileName As String = Nothing)

        If Not IsNothing(FileName) Then
            _FileName = FileName
        End If

        ' Create XmlWriterSettings.
        Dim settings As XmlWriterSettings = New XmlWriterSettings()
        settings.Indent = True

        ' Create XmlWriter.
        Using writer As XmlWriter = XmlWriter.Create(_FileName, settings)
            With writer
                ' Begin writing.
                .WriteStartDocument()
                .WriteStartElement("cargo") ' Root.

                For Each St As cargo3.State In Me.Values
                    .WriteStartElement("state") ' Root.
                    .WriteAttributeString("name", St.Name)
                    If St.DefaultState Then _
                        .WriteAttributeString("default", "")

                    Dim lstSort As New Dictionary(Of Integer, String)
                    For Each act As cargo3.Action In Me(St.Name).Actions.Values
                        lstSort.Add(act.Ord, act.Name)
                    Next
                    Dim lstSortKeys As List(Of Integer) = lstSort.Keys.ToList
                    lstSortKeys.Sort()
                    For Each i As Integer In lstSortKeys
                        Dim act As cargo3.Action = Me(St.Name).Actions(lstSort(i))
                        .WriteStartElement("action") ' Root.
                        .WriteAttributeString("name", act.Name)

                        If Not IsNothing(act.Logic) Then
                            If act.Logic.Length > 0 Then
                                .WriteAttributeString("logic", act.Logic)
                            End If
                        End If

                        For Each cond As cargo3.Condition In Me(St.Name).Actions(act.Name).Conditions.Values
                            .WriteStartElement("condition") ' Root.
                            .WriteAttributeString("name", cond.Name)
                            .WriteAttributeString("x", cond.thisCoordinate.X)
                            .WriteAttributeString("y", cond.thisCoordinate.Y)
                            .WriteAttributeString("r", cond.Colour.Red)
                            .WriteAttributeString("g", cond.Colour.Green)
                            .WriteAttributeString("b", cond.Colour.Blue)
                            .WriteAttributeString("t", cond.Tolerance)
                            .WriteEndElement()
                            If cond.thisCoordinate.X > _ScreenW Then _ScreenW = cond.thisCoordinate.X
                            If cond.thisCoordinate.Y > _ScreenH Then _ScreenH = cond.thisCoordinate.Y
                        Next
                        If Me(St.Name).Actions(act.Name).Results.Keys.Contains(True) Then
                            With Me(St.Name).Actions(act.Name).Results(True)
                                writer.WriteStartElement("result")
                                writer.WriteAttributeString("if", "true")
                                If Not IsNothing(.NextAction) Then _
                                    writer.WriteAttributeString("nextaction", .NextAction.Name)
                                If Not IsNothing(.NextState) Then _
                                    writer.WriteAttributeString("nextstate", .NextState.Name)
                                If Not IsNothing(.Script) Then _
                                    writer.WriteAttributeString("script", .Script.Name)
                                writer.WriteEndElement()
                            End With
                        End If
                        If Me(St.Name).Actions(act.Name).Results.Keys.Contains(False) Then
                            With Me(St.Name).Actions(act.Name).Results(False)
                                writer.WriteStartElement("result")
                                writer.WriteAttributeString("if", "false")
                                If Not IsNothing(.NextAction) Then _
                                    writer.WriteAttributeString("nextaction", .NextAction.Name)
                                If Not IsNothing(.NextState) Then _
                                    writer.WriteAttributeString("nextstate", .NextState.Name)
                                If Not IsNothing(.Script) Then _
                                    writer.WriteAttributeString("script", .Script.Name)
                                writer.WriteEndElement()
                            End With
                        End If
                        .WriteEndElement()
                    Next
                    .WriteEndElement()
                Next

                .WriteStartElement("scripts") ' Root.
                For Each ks As cargo3.KeyScript In Me.myScripts.Values
                    .WriteStartElement("script") ' Root.
                    .WriteAttributeString("name", ks.Name)
                    For Each sst As cargo3.ScriptStep In ks.Steps
                        Select Case sst.StepType
                            Case ScriptStep.eStepType.step_Click
                                .WriteStartElement("step") ' Root.
                                .WriteAttributeString("type", "click")
                                .WriteAttributeString("x", sst.ClickLoc.X)
                                .WriteAttributeString("y", sst.ClickLoc.Y)
                                Select Case sst.Button
                                    Case ScriptStep.eScriptButtons.btn_Right
                                        .WriteAttributeString("button", "right")
                                    Case ScriptStep.eScriptButtons.btn_Double
                                        .WriteAttributeString("button", "double")
                                    Case Else
                                        .WriteAttributeString("button", "left")
                                End Select
                                .WriteEndElement()

                            Case ScriptStep.eStepType.step_Drag
                                .WriteStartElement("step") ' Root.
                                .WriteAttributeString("type", "drag")
                                .WriteAttributeString("x1", sst.StartLoc.X)
                                .WriteAttributeString("y1", sst.StartLoc.Y)
                                .WriteAttributeString("x2", sst.EndLoc.X)
                                .WriteAttributeString("y2", sst.EndLoc.Y)
                                .WriteEndElement()

                            Case ScriptStep.eStepType.step_KeyPress
                                .WriteStartElement("step") ' Root.
                                .WriteAttributeString("type", "keypress")
                                .WriteAttributeString("text", sst.KeyText)
                                .WriteEndElement()

                            Case ScriptStep.eStepType.step_Wait
                                .WriteStartElement("step") ' Root.
                                .WriteAttributeString("type", "wait")
                                .WriteAttributeString("delay", sst.Delay)
                                .WriteEndElement()

                        End Select
                    Next
                    .WriteEndElement()
                Next
                .WriteEndElement()

            End With
        End Using

    End Sub

End Class
