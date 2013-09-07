Public Class frm_Script
    Inherits cargo3.ScriptFrm ' System.Windows.Forms.Form ' 

    Private loaded As Boolean = False

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        Me.Size = My.Settings.fScriptSize
        loaded = True
        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Private Sub frm_Script_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize
        If loaded Then
            My.Settings.fScriptSize = Me.Size
            My.Settings.Save()
        End If
    End Sub

    Private Sub frm_Script_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Me.ResultScript = lst_Scripts.SelectedItem
    End Sub

    Private Sub frm_Script_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        With lst_Scripts
            .Items.Clear()
            For Each ks As cargo3.KeyScript In sharedFunctions.myStates.myScripts.Values
                .Items.Add(ks.Name)
            Next
            If Not IsNothing(ScriptName) Then
                If Not (myStates.myScripts.Keys.Contains(ScriptName)) Then
                    myStates.myScripts.Add(ScriptName, New cargo3.KeyScript(ScriptName, Nothing))
                    With lst_Scripts.Items
                        .Add(ScriptName)
                    End With
                End If
                .SelectedItem = ScriptName
                lst_Scripts_SelectedIndexChanged(Me, New System.EventArgs)

            Else
                Select Case MsgBox("Create new script?", MsgBoxStyle.YesNoCancel)
                    Case MsgBoxResult.Yes
                        Dim strName As String = InputBox("New Script Name:")
                        If strName.Length > 0 Then
                            If Not myStates.myScripts.Keys.Contains(strName) Then
                                myStates.myScripts.Add(strName, New cargo3.KeyScript(strName, Nothing))
                                With lst_Scripts.Items
                                    .Add(strName)
                                End With                                

                            Else
                                MsgBox(String.Format("A {0} called {1} already exists.", "script", strName))                                
                            End If

                            lst_Scripts.SelectedItem = strName
                            lst_Scripts_SelectedIndexChanged(Me, New System.EventArgs)

                        End If

                    Case MsgBoxResult.No
                        lst_Scripts.SelectedIndex = 0

                    Case Else
                        Me.Close()

                End Select
            End If
        End With

        Dim p As New prop_None
        PropertyGrid.SelectedObject = p

        For Each c As Control In Me.Controls
            Try
                AddHandler c.KeyDown, AddressOf hKeyDown
                AddHandler c.MouseEnter, AddressOf hfocus
            Catch ex As Exception

            End Try
            RecurseControls(c)
        Next
    End Sub

    Private Sub RecurseControls(ByRef ct As Control)
        For Each c As Control In ct.Controls
            Try
                If IsNothing(TryCast(c, PropertyGrid)) Then
                    AddHandler c.KeyDown, AddressOf hKeyDown
                    'AddHandler c.MouseEnter, AddressOf hfocus
                End If
            Catch ex As Exception

            End Try
            RecurseControls(c)
        Next
    End Sub

    Private Sub hfocus(ByVal sender As Object, ByVal e As System.EventArgs)
        If Not Me.PropertyGrid.Focused Then Me.Focus()
    End Sub

    Private Sub hKeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        If Me.CloseOnEscape Then
            If e.KeyValue = Keys.Escape Then Me.Close()
        End If
    End Sub

    Private Sub lst_Scripts_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lst_Scripts.SelectedIndexChanged
        With lst_Step
            With .Items
                .Clear()
                myStates.myScripts(lst_Scripts.SelectedItem).Steps.Sort(AddressOf Sortstep)
                For Each st As cargo3.ScriptStep In myStates.myScripts(lst_Scripts.SelectedItem).Steps
                    .Add(st.ToString)
                Next
            End With
        End With
    End Sub

    Private Function Sortstep(ByVal step1 As cargo3.ScriptStep, ByVal step2 As cargo3.ScriptStep) As Integer
        Return step1.Ord.CompareTo(step2.Ord)
    End Function

    Private Sub lst_Step_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles lst_Step.GotFocus
        btn_Click.Enabled = Not IsNothing(lst_Scripts.SelectedItem)
        btn_Drag.Enabled = Not IsNothing(lst_Scripts.SelectedItem)
        btn_Timer.Enabled = Not IsNothing(lst_Scripts.SelectedItem)
        btn_Keypress.Enabled = Not IsNothing(lst_Scripts.SelectedItem)
        btn_Delete.Enabled = lst_Step.SelectedIndices.Count > 0
        SetMoveUpDown()
    End Sub

    Private Sub SetMoveUpDown()
        With lst_Step
            If Not .SelectedIndices.Count = 0 Then
                btn_Up.Enabled = Not (.SelectedIndices(0) = 0)
                btn_Down.Enabled = Not (.SelectedIndices(0) = lst_Step.Items.Count - 1)
            Else
                btn_Up.Enabled = False
                btn_Down.Enabled = False
            End If
        End With
    End Sub

    Private Sub lst_Step_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles lst_Step.LostFocus
        With lst_Step
            btn_Click.Enabled = False
            btn_Drag.Enabled = False
            btn_Timer.Enabled = False
            btn_Keypress.Enabled = False
            btn_Up.Enabled = False
            btn_Down.Enabled = False
            btn_Delete.Enabled = False
        End With
    End Sub

    Private Sub lst_Step_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lst_Step.SelectedIndexChanged
        With lst_Step
            If .SelectedIndices.Count > 0 Then

                btn_Up.Enabled = Not (.SelectedIndices(0) = 0)
                btn_Down.Enabled = Not (.SelectedIndices(0) = lst_Step.Items.Count - 1)

                Select Case myStates.myScripts(lst_Scripts.SelectedItem).Steps.Item(.SelectedIndices(0)).StepType
                    Case cargo3.ScriptStep.eStepType.step_Click
                        Dim p As New prop_Click(TryCast(myStates.myScripts(lst_Scripts.SelectedItem).Steps.Item(.SelectedIndices(0)), cargo3.ScriptStep))
                        With PropertyGrid
                            .SelectedObject = p
                            .ExpandAllGridItems()
                        End With
                        AddHandler p.Refresh, AddressOf hRefresh

                    Case cargo3.ScriptStep.eStepType.step_Drag
                        Dim p As New prop_Drag(TryCast(myStates.myScripts(lst_Scripts.SelectedItem).Steps.Item(.SelectedIndices(0)), cargo3.ScriptStep))
                        With PropertyGrid
                            .SelectedObject = p
                            .ExpandAllGridItems()
                        End With
                        AddHandler p.Refresh, AddressOf hRefresh

                    Case cargo3.ScriptStep.eStepType.step_KeyPress
                        Dim p As New prop_KeyPress(TryCast(myStates.myScripts(lst_Scripts.SelectedItem).Steps.Item(.SelectedIndices(0)), cargo3.ScriptStep))
                        With PropertyGrid
                            .SelectedObject = p
                            .ExpandAllGridItems()
                        End With
                        AddHandler p.Refresh, AddressOf hRefresh

                    Case cargo3.ScriptStep.eStepType.step_Wait
                        Dim p As New prop_Delay(TryCast(myStates.myScripts(lst_Scripts.SelectedItem).Steps.Item(.SelectedIndices(0)), cargo3.ScriptStep))
                        With PropertyGrid
                            .SelectedObject = p
                            .ExpandAllGridItems()
                        End With
                        AddHandler p.Refresh, AddressOf hRefresh

                    Case Else
                        Dim p As New prop_None
                        PropertyGrid.SelectedObject = p
                End Select
            Else
                Dim p As New prop_None
                PropertyGrid.SelectedObject = p
                btn_Up.Enabled = False
                btn_Down.Enabled = False
            End If
        End With
        btn_Delete.Enabled = lst_Step.SelectedIndices.Count > 0
    End Sub

    Private Sub hRefresh()
        With Me
            If .InvokeRequired Then
                .Invoke(New MethodInvoker(AddressOf hRefresh))
            Else
                .PropertyGrid.Refresh()
                lst_Step.Items(lst_Step.SelectedIndices(0)).Text = myStates.myScripts(lst_Scripts.SelectedItem).Steps.Item(lst_Step.SelectedIndices(0)).ToString
            End If
        End With
    End Sub

    Private Sub btn_Click_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Click.Click
        If lst_Scripts.SelectedItem.length > 0 Then
            With myStates.myScripts(lst_Scripts.SelectedItem).Steps
                Dim st As New cargo3.ScriptStep(myStates.myScripts(lst_Scripts.SelectedItem).NextOrd, New Point(0, 0), cargo3.ScriptStep.eScriptButtons.btn_Left)
                .Add(st)
                lst_Step.Items.Add(st.ToString)
                SetMoveUpDown()
            End With
        End If
    End Sub

    Private Sub btn_Drag_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Drag.Click
        If lst_Scripts.SelectedItem.length > 0 Then
            With myStates.myScripts(lst_Scripts.SelectedItem).Steps
                Dim st As New cargo3.ScriptStep(myStates.myScripts(lst_Scripts.SelectedItem).NextOrd, New Point(0, 0), New Point(0, 0))
                .Add(st)
                lst_Step.Items.Add(st.ToString)
                SetMoveUpDown()
            End With
        End If
    End Sub

    Private Sub btn_Keypress_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Keypress.Click
        If lst_Scripts.SelectedItem.length > 0 Then
            With myStates.myScripts(lst_Scripts.SelectedItem).Steps
                Dim st As New cargo3.ScriptStep(myStates.myScripts(lst_Scripts.SelectedItem).NextOrd, "")
                .Add(st)
                lst_Step.Items.Add(st.ToString)
                SetMoveUpDown()
            End With
        End If
    End Sub

    Private Sub btn_Timer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Timer.Click
        If lst_Scripts.SelectedItem.length > 0 Then
            With myStates.myScripts(lst_Scripts.SelectedItem).Steps
                Dim st As New cargo3.ScriptStep(myStates.myScripts(lst_Scripts.SelectedItem).NextOrd, 0)
                .Add(st)
                lst_Step.Items.Add(st.ToString)
                SetMoveUpDown()
            End With
        End If
    End Sub

    Private Sub btn_Up_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Up.Click
        swap(lst_Step.SelectedIndices(0), lst_Step.SelectedIndices(0) - 1)
        lst_Step.Items.Item(lst_Step.SelectedIndices(0) - 1).Selected = True
    End Sub

    Private Sub btn_Down_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Down.Click
        swap(lst_Step.SelectedIndices(0), lst_Step.SelectedIndices(0) + 1)
        lst_Step.Items.Item(lst_Step.SelectedIndices(0) + 1).Selected = True
    End Sub

    Private Sub swap(ByVal item1 As Integer, ByVal item2 As Integer)
        With myStates.myScripts(lst_Scripts.SelectedItem).Steps
            Dim item1ord As Integer = .Item(item1).Ord
            Dim item2ord As Integer = .Item(item2).Ord
            .Item(item1).Ord = item2ord
            .Item(item2).Ord = item1ord
            .Sort(AddressOf Sortstep)
        End With

        lst_Step.Items.Item(item1).Text = myStates.myScripts(lst_Scripts.SelectedItem).Steps.Item(item1).ToString
        lst_Step.Items.Item(item2).Text = myStates.myScripts(lst_Scripts.SelectedItem).Steps.Item(item2).ToString
        
    End Sub

    Private Sub btn_Delete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Delete.Click
        Dim del As Integer = lst_Step.SelectedIndices(0)
        myStates.myScripts(lst_Scripts.SelectedItem).Steps.RemoveAt(del)
        For Each st As cargo3.ScriptStep In myStates.myScripts(lst_Scripts.SelectedItem).Steps
            If st.Ord > del Then
                st.Ord -= 1
            End If
        Next
        lst_Step.Items.RemoveAt(del)
    End Sub

    Private Sub btn_New_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_New.Click
        Dim strName As String = InputBox("New Script Name:")
        If strName.Length > 0 Then
            If Not myStates.myScripts.Keys.Contains(strName) Then
                myStates.myScripts.Add(strName, New cargo3.KeyScript(strName, Nothing))
                With lst_Scripts.Items
                    .Add(strName)
                End With

            Else
                MsgBox(String.Format("A {0} called {1} already exists.", "script", strName))
            End If

            lst_Scripts.SelectedItem = strName
            lst_Scripts_SelectedIndexChanged(Me, New System.EventArgs)
        End If
    End Sub

    Private Sub ToolStripButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton1.Click
        With myStates.myScripts(lst_Scripts.SelectedItem)
            .Execute()
        End With
    End Sub

End Class