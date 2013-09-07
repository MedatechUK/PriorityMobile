Imports prnscn.capture

Public Class frm_State

    Private loaded As Boolean = False

    Public Sub New()
        InitializeComponent()
        Me.Size = My.Settings.fStateSize
        loaded = True
    End Sub

    Private Sub frm_State_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        For Each cond As cargo3.State In myStates.Values
            lst_States.Items.Add(cond.Name)
        Next
        With cargo3.Module2.ScriptForm
            .Top = Me.Top + 40
            .Left = Me.Left
        End With

        For Each c As Control In Me.Controls
            Try
                AddHandler c.MouseEnter, AddressOf hfocus
            Catch ex As Exception

            End Try
            RecurseControls(c)
        Next
    End Sub

    Private Sub RecurseControls(ByRef ct As Control)
        For Each c As Control In ct.Controls
            Try
                AddHandler c.MouseEnter, AddressOf hfocus
            Catch ex As Exception

            End Try
            RecurseControls(c)
        Next
    End Sub

    Private Sub hfocus(ByVal sender As Object, ByVal e As System.EventArgs)
        Me.Focus()
    End Sub

    Private Sub lst_States_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles lst_States.GotFocus
        lst_States_SelectedIndexChanged(Me, New System.EventArgs)
        SetNewBtnEnabled()
    End Sub

    Private Sub lst_States_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lst_States.SelectedIndexChanged
        With Me

            .lst_Actions.Items.Clear()
            .lst_Conditions.Items.Clear()

            If Not .lst_States.SelectedIndices.Count = 0 Then
                Dim p As New prop_State(TryCast(myStates(ListText(lst_States)), cargo3.State))
                With .PropertyGrid
                    .SelectedObject = p
                    .ExpandAllGridItems()
                End With

                Dim lstSort As New Dictionary(Of Integer, String)
                For Each act As cargo3.Action In myStates(.lst_States.Items(.lst_States.SelectedIndices(0)).Text).Actions.Values
                    lstSort.Add(act.Ord, act.Name)
                Next
                Dim lstSortKeys As List(Of Integer) = lstSort.Keys.ToList
                lstSortKeys.Sort()
                For Each i As Integer In lstSortKeys
                    .lst_Actions.Items.Add(myStates(ListText(lst_States)).Actions(lstSort(i)).Name)
                Next

            Else
                Dim p As New prop_None
                .PropertyGrid.SelectedObject = p
            End If
        End With
        SetNewBtnEnabled()

    End Sub

    Private Sub lst_Actions_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles lst_Actions.GotFocus
        lst_Actions_SelectedIndexChanged(Me, New System.EventArgs)
        SetNewBtnEnabled()
    End Sub

    Private Sub lst_Actions_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lst_Actions.SelectedIndexChanged
        With Me
            .lst_Conditions.Items.Clear()
            If Not .lst_Actions.SelectedIndices.Count = 0 Then
                Dim p As New prop_Action(TryCast(myStates(ListText(lst_States)).Actions(ListText(lst_Actions)), cargo3.Action), ListText(lst_States))
                With .PropertyGrid
                    .SelectedObject = p
                    .ExpandAllGridItems()
                End With
                For Each cond As cargo3.Condition In myStates(ListText(.lst_States)).Actions(ListText(.lst_Actions)).Conditions.Values
                    .lst_Conditions.Items.Add(cond.Name)
                Next
            Else
                Dim p As New prop_None
                .PropertyGrid.SelectedObject = p
            End If
        End With
        SetNewBtnEnabled()
    End Sub

    Private Sub lst_Conditions_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles lst_Conditions.GotFocus
        lst_Conditions_SelectedIndexChanged(Me, New System.EventArgs)
        SetNewBtnEnabled()
    End Sub

    Private Sub lst_Conditions_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lst_Conditions.SelectedIndexChanged
        With Me
            If Not .lst_Conditions.SelectedIndices.Count = 0 Then
                Dim p As New prop_Condition(TryCast(myStates(ListText(lst_States)).Actions(ListText(lst_Actions)).Conditions(ListText(lst_Conditions)), cargo3.Condition))
                With .PropertyGrid
                    .SelectedObject = p
                    .ExpandAllGridItems()
                End With
                AddHandler p.Refresh, AddressOf hRefresh
            Else
                Dim p As New prop_None
                .PropertyGrid.SelectedObject = p
            End If
        End With
        SetNewBtnEnabled()
    End Sub

    Private Sub hRefresh()
        If Me.InvokeRequired Then
            Me.Invoke(New MethodInvoker(AddressOf hRefresh))
        Else
            Me.PropertyGrid.Refresh()
        End If
    End Sub

    Private Function ListText(ByRef ListCtrl As ListView, Optional ByVal Ord As Integer = -1) As String
        Try
            If Ord = -1 Then
                Return ListCtrl.Items(ListCtrl.SelectedIndices(0)).Text
            Else
                Return ListCtrl.Items(Ord).Text
            End If
        Catch
            Return String.Empty
        End Try
    End Function

    Private Sub SetNewBtnEnabled()
        DeleteState.Enabled = CBool(ListText(lst_States).Length > 0)
        DeleteAction.Enabled = CBool(ListText(lst_Actions).Length > 0)
        DeleteCondition.Enabled = CBool(ListText(lst_Conditions).Length > 0)
        btn_TestCondition.Enabled = CBool(ListText(lst_Conditions).Length > 0)
        NewAction.Enabled = CBool(ListText(lst_States).Length > 0)
        NewCondition.Enabled = CBool(ListText(lst_Actions).Length > 0)
        SetMoveUpDown()
    End Sub

    Private Sub NewState_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewState.Click
        Dim strName As String = InputBox("New State Name:")
        If strName.Length > 0 Then
            If Not myStates.Keys.Contains(strName) Then
                myStates.Add(strName, New cargo3.State(strName))
                With lst_States.Items
                    .Add(strName)
                End With
            Else
                MsgBox(String.Format("A {0} called {1} already exists.", "state", strName))
            End If
        End If
    End Sub

    Private Sub NewAction_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewAction.Click
        Dim strName As String = InputBox("New Action Name:")
        If strName.Length > 0 Then
            If Not myStates(ListText(lst_States)).Actions.Keys.Contains(strName) Then
                myStates(ListText(lst_States)).Actions.Add(strName, New cargo3.Action(myStates(ListText(lst_States)).NextOrd, strName, ListText(lst_States), Nothing))
                With lst_Actions.Items
                    .Add(strName)
                End With
            Else
                MsgBox(String.Format("An {0} called {1} already exists.", "action", strName))
            End If
        End If
        SetMoveUpDown()
    End Sub

    Private Sub NewCondition_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewCondition.Click
        Dim strName As String = InputBox("New Condition Name:")
        If strName.Length > 0 Then
            If Not myStates(ListText(lst_States)).Actions(ListText(lst_Actions)).Conditions.Keys.Contains(strName) Then
                myStates(ListText(lst_States)).Actions(ListText(lst_Actions)).Conditions.Add(strName, New cargo3.Condition(strName, New Point(0, 0), New cargo3.rgb()))
                With lst_Conditions.Items
                    .Add(strName)
                End With
            Else
                MsgBox(String.Format("A {0} called {1} already exists.", "condition", strName))
            End If
        End If
    End Sub

    Private Sub SetMoveUpDown()
        With lst_Actions
            If Not .SelectedIndices.Count = 0 Then
                btn_Up.Enabled = Not (.SelectedIndices(0) = 0)
                btn_Down.Enabled = Not (.SelectedIndices(0) = lst_Actions.Items.Count - 1)
            Else
                btn_Up.Enabled = False
                btn_Down.Enabled = False
            End If
        End With
    End Sub

    Private Sub btn_Up_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Up.Click
        swap(lst_Actions.SelectedIndices(0), lst_Actions.SelectedIndices(0) - 1)
        lst_Actions.Items.Item(lst_Actions.SelectedIndices(0) - 1).Selected = True
        'lst_Actions_SelectedIndexChanged(Me, New System.EventArgs)
    End Sub

    Private Sub btn_Down_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Down.Click
        swap(lst_Actions.SelectedIndices(0), lst_Actions.SelectedIndices(0) + 1)
        lst_Actions.Items.Item(lst_Actions.SelectedIndices(0) + 1).Selected = True
        'lst_Actions_SelectedIndexChanged(Me, New System.EventArgs)
    End Sub

    Private Sub swap(ByVal item1 As Integer, ByVal item2 As Integer)

        Dim item1ord As Integer = myStates(ListText(lst_States)).Actions(ListText(lst_Actions, item1)).Ord
        Dim item2ord As Integer = myStates(ListText(lst_States)).Actions(ListText(lst_Actions, item2)).Ord

        Dim item1Key As String = ListText(lst_Actions, item1)
        Dim item2Key As String = ListText(lst_Actions, item2)

        myStates(ListText(lst_States)).Actions(item1Key).Ord = item2ord
        myStates(ListText(lst_States)).Actions(item2Key).Ord = item1ord

        lst_Actions.Items.Item(item1).Text = item2Key
        lst_Actions.Items.Item(item2).Text = item1Key

    End Sub

    Private Sub DeleteState_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DeleteState.Click
        If MsgBox(String.Format("Delete State {0}?", ListText(lst_States)), MsgBoxStyle.OkCancel) = MsgBoxResult.Ok Then
            myStates.Remove(ListText(lst_States))
        End If
        With lst_States
            .Items.RemoveAt(.SelectedIndices(0))
        End With
    End Sub

    Private Sub DeleteAction_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DeleteAction.Click
        If MsgBox(String.Format("Delete action {0}?", ListText(lst_Actions)), MsgBoxStyle.OkCancel) = MsgBoxResult.Ok Then
            myStates(ListText(lst_States)).Actions.Remove(ListText(lst_Actions))
        End If
        With lst_Actions
            .Items.RemoveAt(.SelectedIndices(0))
        End With
    End Sub

    Private Sub DeleteCondition_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DeleteCondition.Click
        If MsgBox(String.Format("Delete Condition {0}?", ListText(lst_Conditions)), MsgBoxStyle.OkCancel) = MsgBoxResult.Ok Then
            myStates(ListText(lst_States)).Actions(ListText(lst_Actions)).Conditions.Remove(ListText(lst_Conditions))
        End If
        With lst_Conditions
            .Items.RemoveAt(.SelectedIndices(0))
        End With
    End Sub

    Private Sub frm_State_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize
        If loaded Then
            My.Settings.fStateSize = Me.Size
            My.Settings.Save()
        End If
    End Sub

    Private Sub btn_TestCondition_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_TestCondition.Click
        Dim thisCondition As cargo3.Condition = myStates(ListText(lst_States)).Actions(ListText(lst_Actions)).Conditions(ListText(lst_Conditions))
        Dim thisColour As System.Drawing.Color
        With ThisCondition
            Select Case Test(PrintScreen(.thisCoordinate.X + 10, .thisCoordinate.Y + 10), thisColour)
                Case True
                    MsgBox("Returns True", MsgBoxStyle.Information)
                Case Else
                    MsgBox(String.Format("Returns False. Colour at location was rbg({0},{1},{2}).", thisColour.R, thisColour.G, thisColour.B), MsgBoxStyle.Information)
            End Select

        End With
    End Sub

    Public Function Test(ByRef bmp As System.Drawing.Bitmap, ByRef ColourAtPos As System.Drawing.Color) As Boolean
        Dim thisCondition As cargo3.Condition = myStates(ListText(lst_States)).Actions(ListText(lst_Actions)).Conditions(ListText(lst_Conditions))
        With thisCondition
            Dim this As System.Drawing.Color = bmp.GetPixel(.thisCoordinate.X, .thisCoordinate.Y)
            ColourAtPos = this
            If Not ((this.R > .Colour.Red - (.Tolerance * 2.5)) And (this.R < .Colour.Red + (.Tolerance * 2.5))) Then Return False
            If Not ((this.G > .Colour.Green - (.Tolerance * 2.5)) And (this.G < .Colour.Green + (.Tolerance * 2.5))) Then Return False
            If Not ((this.B > .Colour.Blue - (.Tolerance * 2.5)) And (this.B < .Colour.Blue + (.Tolerance * 2.5))) Then Return False
            Return True
        End With
    End Function

End Class