Imports System.Xml
Imports prnscn.capture
Imports System.Drawing
Imports hookey
Imports System.Reflection

Public Class frm_Main

    Private fDebug As New frm_Debug

    Private _prn As Bitmap
    Private fScript As frm_Script = Nothing
    Private fState As frm_State = Nothing

    ' Form Dragging Variables
    Private IsFormBeingDragged As Boolean = False
    Private MouseDownX As Integer
    Private MouseDownY As Integer
    Private Moving As System.Timers.Timer

    Private RunTimer As System.Timers.Timer

    Private Sub ToolStrip1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ToolStrip1.Click
        If Not IsNothing(fScript) Then
            With fScript
                .BringToFront()
            End With
        End If
        If Not IsNothing(fState) Then
            With fState
                .BringToFront()
            End With
        End If
        If fDebug.Visible Then
            With fDebug
                .BringToFront()
            End With
        End If
        Me.BringToFront()
    End Sub

    Private Sub ToolStrip1_ItemClicked(ByVal sender As Object, ByVal e As System.Windows.Forms.ToolStripItemClickedEventArgs) Handles ToolStrip1.ItemClicked

        Select Case e.ClickedItem.Name

            Case "btn_Dbg"
                fDebug.Visible = Not fDebug.Visible
                If fDebug.Visible Then
                    With fDebug
                        .BringToFront()
                    End With
                End If
                Me.BringToFront()

            Case "btn_Properties"

                If IsNothing(fState) Then

                    If Not IsNothing(fScript) Then
                        ToolStrip1_ItemClicked(Me, New ToolStripItemClickedEventArgs(Me.btn_Scripts))
                    End If

                    fState = New frm_State
                    With fState
                        .Top = Me.Top + Me.ToolStrip1.Height + 2
                        .Left = Me.Left
                        '.Size = My.Settings.fStateSize
                    End With
                    Me.Timer1.Enabled = False

                    fState.Show()
                    Me.Focus()
                    Me.Timer1.Enabled = False
                Else
                    myStates.Save()
                    fState.Close()
                    fState = Nothing
                    Me.Timer1.Enabled = True
                End If

            Case "btn_Scripts"
                If IsNothing(fScript) Then
                    If Not IsNothing(fState) Then
                        ToolStrip1_ItemClicked(Me, New ToolStripItemClickedEventArgs(Me.btn_Properties))
                    End If

                    fScript = New frm_Script
                    fScript.ScriptName = myStates.myScripts.Values(0).Name
                    With fScript
                        .Top = Me.Top + Me.ToolStrip1.Height + 2
                        .Left = Me.Left
                        '.Size = My.Settings.fScriptSize
                    End With
                    fScript.Show()
                    Me.Focus()
                    Me.Timer1.Enabled = False
                Else
                    myStates.Save()
                    fScript.Close()
                    fScript = Nothing
                    Me.Timer1.Enabled = True
                End If

            Case "btn_Open"
                With Me.OpenFileDialog1
                    .InitialDirectory = Application.StartupPath
                    .FileName = ""
                    .ShowDialog()
                    If .FileName.Length > 0 Then
                        If System.IO.File.Exists(.FileName) Then

                            With myStates
                                If .Filename.Length > 0 Then
                                    .Save()
                                    .Clear()
                                    .myScripts.Clear()
                                End If
                            End With

                            myStates.Load(.FileName)
                            HasFile(True)
                        End If
                    End If
                End With

            Case "btn_New"
                With Me.SaveFileDialog1
                    .InitialDirectory = Application.StartupPath
                    .FileName = "NewTriggerFile.xml"
                    .ShowDialog()
                    If .FileName.Length > 0 Then
                        While (System.IO.File.Exists(.FileName))
                            System.IO.File.Delete(.FileName)
                            System.Threading.Thread.Sleep(100)
                        End While

                        With myStates
                            If .Filename.Length > 0 Then
                                .Save()
                                .Clear()
                                .myScripts.Clear()
                            End If
                            myStates = New cargo3.States(frm_Script)
                            myStates.Filename = SaveFileDialog1.FileName
                        End With

                        HasFile(True)

                    End If
                End With

            Case "btn_SaveAs"
                With Me.SaveFileDialog1
                    .FileName = myStates.Filename
                    .ShowDialog()
                    If .FileName.Length > 0 And .FileName <> myStates.Filename Then
                        While (System.IO.File.Exists(.FileName))
                            System.IO.File.Delete(.FileName)
                            System.Threading.Thread.Sleep(100)
                        End While

                        myStates.Filename = .FileName
                        myStates.Save()

                    End If
                End With

            Case "btn_Run"
                HasFile(False)
                btn_New.Enabled = False
                btn_Open.Enabled = False
                btn_Exit.Enabled = False
                btn_Stop.Enabled = True


            Case "btn_Stop"
                HasFile(True)
                btn_New.Enabled = True
                btn_Open.Enabled = True
                btn_Exit.Enabled = True
                btn_Stop.Enabled = False

            Case "btn_Exit"
                If MsgBox("Really quit?", MsgBoxStyle.OkCancel) = MsgBoxResult.Ok Then _
                    Me.Close()

        End Select
    End Sub

    Private Sub HasFile(ByVal DoesIt As Boolean)
        btn_Properties.Enabled = DoesIt
        btn_Scripts.Enabled = DoesIt
        btn_Run.Enabled = DoesIt
        btn_SaveAs.Enabled = DoesIt
    End Sub

#Region "Drag Form"

    Private Sub Form1_MouseDown(ByVal sender As Object, ByVal e As MouseEventArgs) Handles ToolStrip1.MouseDown
        If e.Button = MouseButtons.Left Then
            IsFormBeingDragged = True
            MouseDownX = e.X
            MouseDownY = e.Y

            Moving = New System.Timers.Timer
            With Moving
                AddHandler .Elapsed, AddressOf hMouseMove
                .Interval = 100
                .Enabled = True
                .Start()
            End With
        End If
    End Sub

    Private Sub Form1_MouseUp(ByVal sender As Object, ByVal e As MouseEventArgs) Handles ToolStrip1.MouseUp
        If e.Button = MouseButtons.Left Then
            IsFormBeingDragged = False
            With Moving
                .Stop()
                .Dispose()
            End With
            FormLocation()


            My.Settings.fMainLocation = Me.Location
            My.Settings.Save()

        End If

    End Sub

    Private Sub hMouseMove(ByVal sender As Object, ByVal e As System.EventArgs)
        If IsFormBeingDragged Then
            FormLocation()
        End If
    End Sub

    Private Sub FormLocation()
        With Me
            If .InvokeRequired Then
                .Invoke(New MethodInvoker(AddressOf FormLocation))
            Else
                Me.Left = Cursor.Position.X - MouseDownX
                Me.Top = Cursor.Position.Y - MouseDownY
                If Not IsNothing(fScript) Then
                    With fScript
                        .Top = Me.Top + Me.ToolStrip1.Height + 2
                        .Left = Me.Left
                    End With
                End If
                If Not IsNothing(fState) Then
                    With fState
                        .Top = Me.Top + Me.ToolStrip1.Height + 2
                        .Left = Me.Left
                    End With
                End If
            End If
        End With
    End Sub

#End Region

    Private Sub Form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Location = New Point(0, 0) 'My.Settings.fMainLocation

        fDebug.Show()

        Try
            hookey.HookKeyboard(AddressOf WaitForKeys.hHook)

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        With Me
            '.TopMost = True
            .BringToFront()
        End With
    End Sub

    Private Sub ToolStrip1_MouseEnter(ByVal sender As Object, ByVal e As System.EventArgs) Handles ToolStrip1.MouseEnter
        Me.Focus()
    End Sub

    Private Sub btn_Run_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Run.Click

        If Not (IsNothing(fScript)) Then ToolStrip1_ItemClicked(Me, New ToolStripItemClickedEventArgs(btn_Scripts))
        If Not (IsNothing(fState)) Then ToolStrip1_ItemClicked(Me, New ToolStripItemClickedEventArgs(btn_Properties))

        fDebug.Clear("")
        fDebug.Log("Compiling...")

        Dim execp As Exception = Nothing
        cargo3.Compile(myStates, execp)

        If Not IsNothing(execp) Then
            fDebug.Log(execp.Message)
            ToolStrip1_ItemClicked(Me, New ToolStripItemClickedEventArgs(btn_Stop))
        Else
            fDebug.Log("ok.")
            fDebug.Running = True
            For Each st As cargo3.State In myStates.Values
                If st.DefaultState Then
                    myStates.CurrentState = st
                    fDebug.Log("Default state is {0}.", st.Name)
                End If
            Next

            Dim lstSort As New Dictionary(Of Integer, String)
            For Each act As cargo3.Action In myStates(myStates.CurrentState.Name).Actions.Values
                lstSort.Add(act.Ord, act.Name)
            Next
            Dim lstSortKeys As List(Of Integer) = lstSort.Keys.ToList
            lstSortKeys.Sort()
            myStates.CurrentAction = myStates(myStates.CurrentState.Name).Actions(lstSort(0))

            fDebug.Log("Current action is {0}.", myStates.CurrentAction.Name)

            RunTimer = New System.Timers.Timer
            With RunTimer
                .Interval = 5000
                .Enabled = True
                AddHandler .Elapsed, AddressOf Run
                fDebug.Log("Starting timer...")
                .Start()
            End With
        End If

    End Sub

    Public Sub Run()

        With RunTimer
            .Enabled = False
            .Stop()
        End With

        Dim lstSort As Dictionary(Of Integer, String) = Nothing
        Dim lstSortKeys As List(Of Integer) = Nothing

        With fDebug
            .Clear("")
            .Log("Current state is {0}.", myStates.CurrentState.Name)
            .Log("Current action is {0}.", myStates.CurrentAction.Name)
            If .Visible Then
                .BringToFore("")
            End If
            fDebug.Log("Capturing screen...")
            _prn = PrintScreen(myStates.ScreenW, myStates.ScreenH)
        End With

        Dim exitloop As Boolean = False

        Do
            With myStates

                fDebug.Log("Checking conditions...")
                Dim ob() As Object = Nothing
                For Each cond As cargo3.Condition In myStates(.CurrentState.Name).Actions(.CurrentAction.Name).Conditions.Values
                    Try
                        ReDim Preserve ob(UBound(ob) + 1)
                    Catch
                        ReDim ob(0)
                    Finally
                        Dim prn(0)
                        prn(0) = _prn
                        ob(UBound(ob)) = cond.myMethod.Invoke(cargo3.Compiled, prn)
                        fDebug.Log("Condition {0} returns {1}", cond.Name, ob(UBound(ob)))
                    End Try
                Next

                SortAction(.CurrentState.Name, lstSort, lstSortKeys)

                Dim res As Boolean = .CurrentAction.myMethod.Invoke(cargo3.Compiled, ob)
                fDebug.Log("Action {0} returns {1}", myStates(.CurrentState.Name).Actions(.CurrentAction.Name).Name, res)

                Dim r As cargo3.Result
                Try
                    r = myStates(.CurrentState.Name).Actions(.CurrentAction.Name).Results(res)
                Catch ex As Exception
                    r = New cargo3.Result
                End Try
                
                With r

                    If Not IsNothing(.Script) Then
                        If .Script.Steps.Count > 0 Then
                            exitloop = True
                            fDebug.Log("Running script {0}", .Script.Name)
                            .Script.Execute()
                            KeyWait(VK_ESCAPE)
                        End If
                    End If

                    With fDebug
                        If .Visible Then
                            .BringToFore("")
                        End If
                    End With

                    If Not IsNothing(.NextState) Then
                        myStates.CurrentState = .NextState
                        SortAction(.NextState.Name, lstSort, lstSortKeys)
                        myStates.CurrentAction = myStates(.NextState.Name).Actions(lstSort(0))
                    ElseIf Not IsNothing(.NextAction) Then
                        exitloop = True
                        myStates.CurrentAction = .NextAction
                    Else
                        If myStates.CurrentAction.Ord < lstSortKeys.Max Then
                            For Each i As Integer In lstSortKeys
                                If i > myStates.CurrentAction.Ord Then
                                    myStates.CurrentAction = myStates(myStates.CurrentState.Name).Actions(lstSort(i))
                                    Exit For
                                End If
                            Next
                        Else
                            exitloop = True
                            myStates.CurrentAction = myStates(myStates.CurrentState.Name).Actions(lstSort(0))
                        End If
                    End If

                End With

                With fDebug
                    .Log("Next state is {0}.", myStates.CurrentState.Name)
                    .Log("Next action is {0}.", myStates.CurrentAction.Name)
                    If .Visible Then
                        .BringToFore("")
                    End If
                End With

            End With

        Loop Until exitloop
        Try
            With RunTimer
                .Enabled = True
                .Start()
            End With
        Catch
        End Try

    End Sub

    Private Sub SortAction(ByVal State As String, ByRef lstSort As Dictionary(Of Integer, String), ByRef lstSortKeys As List(Of Integer))
        lstSort = New Dictionary(Of Integer, String)
        For Each act As cargo3.Action In myStates(State).Actions.Values
            lstSort.Add(act.Ord, act.Name)
        Next
        lstSortKeys = New List(Of Integer)()
        lstSortKeys = lstSort.Keys.ToList
        lstSortKeys.Sort()
    End Sub

    Private Sub btn_Stop_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_Stop.Click
        Try
            With RunTimer
                .Stop()
                .Dispose()
            End With
        Catch
        Finally
            fDebug.Log("Quit.")
            fDebug.Running = False
        End Try
    End Sub


End Class

