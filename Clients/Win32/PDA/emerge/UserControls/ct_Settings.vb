Imports System
Imports System.IO
Imports System.Threading

Public Class ct_Settings

    Dim _App As PDAOnBoardData.BaseForm
    Private Tick As Integer = 0
    Dim ar As New MyCls.MyArray

    Public Event doSync()
    Public Event NoStatus()

    Public Sub New(Optional ByRef App As PDAOnBoardData.BaseForm = Nothing)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        If Not IsNothing(App) Then
            _App = App
        End If

    End Sub

    Public Sub init()

        Me.txt_ServiceURL.Text = My.Settings.Demo_App_priwebsvc_Service
        Me.txt_UserName.Text = My.Settings.Username
        Me.txt_Warehouse.Text = My.Settings.Warehouse
        Me.Chk_CheckOnStart.Checked = My.Settings.CheckOnStart
        Me.CheckFreq.Value = My.Settings.CheckFreq

        CheckFreq.Value = My.Settings.CheckFreq
        doFreqName()

        If Not IsNothing(_App.rss(o.Statuses).thisArray) Then
            ' Sync on Start
            If Me.Chk_CheckOnStart.Checked Then
                ' Create new q thread 
                Dim myThread As Thread
                myThread = New Thread(New ThreadStart(AddressOf wait5))
                myThread.Start()
            End If

            ' Scheduled Sync
            Timer1.Enabled = CBool(My.Settings.CheckFreq > 0)

            With Me.lst_Status
                .Clear()
                .Columns.Clear()
                .Columns.Add("Action")
                .Columns.Add("Status")
            End With

            With _App.rss(o.Actions)
                AddAction(txt_SetTimeEnRoute, .thisArray)
                AddAction(txt_SetTimeOnSite, .thisArray)
                AddAction(txt_SetTimeFinished, .thisArray)
                AddAction(txt_PostData, .thisArray)
                AddAction(txt_IsReIssue, .thisArray)
            End With

        Else
            RaiseEvent NoStatus()
        End If

    End Sub

    Private Sub doFreqName()

        Select Case CheckFreq.Value
            Case 0
                lbl_Freq.Text = "Don't Check"
            Case 1
                lbl_Freq.Text = "Check Every Two Hours"
            Case 2
                lbl_Freq.Text = "Check Every Hour"
            Case 3
                lbl_Freq.Text = "Check Every Half Hour"
            Case 4
                lbl_Freq.Text = "Check Every 15 Minutes"
        End Select

        lbl_Freq.Left = (Me.Width - lbl_Freq.Width) / 2
        Tick = 0

    End Sub

    Private Sub CheckFreq_Scroll(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckFreq.Scroll
        doFreqName()
    End Sub

    Private Sub ct_Settings_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize

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

        Dim y As Integer = 5
        With Me
            .txt_ServiceURL.Font = c
            .lbl_ServiceURL.Font = f
            .txt_UserName.Font = c
            .lbl_UserName.Font = f
            .txt_Warehouse.Font = c
            .lbl_WareHouse.Font = f
            .lst_Status.Font = c
            .CheckFreq.Font = c
            .lbl_Freq.Font = c
            Chk_CheckOnStart.Font = c

            .txt_ServiceURL.Top = y
            .lbl_ServiceURL.Top = y

            Dim r As Integer = os + .lbl_ServiceURL.Width
            Dim w As Integer = (Me.Width - (r + 5))

            .lbl_ServiceURL.Left = r - .lbl_ServiceURL.Width
            .txt_ServiceURL.Left = r
            .txt_ServiceURL.Width = w

            y = .txt_ServiceURL.Top + .txt_ServiceURL.Height + os

            .txt_UserName.Top = y
            .lbl_UserName.Top = y

            .lbl_UserName.Left = r - .lbl_UserName.Width
            .txt_UserName.Left = r
            .txt_UserName.Width = w

            y = .txt_UserName.Top + .txt_UserName.Height + os

            .txt_Warehouse.Top = y
            .lbl_WareHouse.Top = y

            .lbl_WareHouse.Left = r - .lbl_WareHouse.Width
            .txt_Warehouse.Left = r
            .txt_Warehouse.Width = w

            y = .txt_Warehouse.Top + .txt_Warehouse.Height + os

            Chk_CheckOnStart.Top = y
            Chk_CheckOnStart.Left = r

            y = .Chk_CheckOnStart.Top + .Chk_CheckOnStart.Height + os

            .CheckFreq.Left = 5
            .CheckFreq.Width = Me.Width - 10
            .CheckFreq.Top = y

            y = .CheckFreq.Top + (.CheckFreq.Height / 2)
            .lbl_Freq.Top = y
            doFreqName()

            y = .lbl_Freq.Top + .lbl_Freq.Height + 10

            .lst_Status.Top = y
            .lst_Status.Height = Me.Height - .lst_Status.Top

            With .lst_Status
                .Left = 5
                .Width = Me.Width - 10

                If .Items.Count > 0 Then
                    .Columns(0).AutoResize(ColumnHeaderAutoResizeStyle.HeaderSize)
                    .Columns(1).AutoResize(ColumnHeaderAutoResizeStyle.HeaderSize)
                Else
                    .Columns(0).AutoResize(ColumnHeaderAutoResizeStyle.HeaderSize)
                    .Columns(1).AutoResize(ColumnHeaderAutoResizeStyle.HeaderSize)
                End If

            End With
        End With
    End Sub

    Private Sub lst_Status_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles lst_Status.MouseUp

        Dim s As ListView.SelectedListViewItemCollection = Me.lst_Status.SelectedItems
        If s.Count = 0 Then Exit Sub

        If s.Count > 0 Then
            Select Case e.Button
                Case Windows.Forms.MouseButtons.Right
                    'RaiseEvent RightClicked(s(0).Text)
                    Dim menu As New ContextMenuStrip

                    With menu
                        .Items.Add("")
                        .Items(.Items.Count - 1).Font = Me.lst_Status.Font
                        .Items(.Items.Count - 1).Text = "_Blank"
                        .Items(.Items.Count - 1).Name = "_Blank"
                        AddHandler .Items(.Items.Count - 1).Click, AddressOf hChange_State

                        For y As Integer = 0 To UBound(_App.rss(o.Statuses).thisArray, 2)
                            _App.rss(o.Statuses).currentOrdinal = y
                            .Items.Add("")
                            .Items(.Items.Count - 1).Font = Me.lst_Status.Font
                            .Items(.Items.Count - 1).Text = _App.rss(o.Statuses).GetField("STATUS")
                            .Items(.Items.Count - 1).Name = _App.rss(o.Statuses).GetField("STATUS")
                            AddHandler .Items(.Items.Count - 1).Click, AddressOf hChange_State
                        Next
                    End With
                    Me.lst_Status.ContextMenuStrip = menu

            End Select
        End If
    End Sub

    Sub hChange_State(ByVal sender As Object, ByVal e As System.EventArgs)
        Select Case sender.text
            Case "_Blank"
                Me.lst_Status.Items(Me.lst_Status.SelectedIndices(0)).SubItems(1).Text = ""
            Case Else
                Me.lst_Status.Items(Me.lst_Status.SelectedIndices(0)).SubItems(1).Text = sender.text
        End Select

    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        Tick = Tick + 1
        Select Case My.Settings.CheckFreq
            Case 1
                If Tick >= 120 Then
                    RaiseEvent doSync()
                    Tick = 0
                End If
            Case 2
                If Tick >= 60 Then
                    RaiseEvent doSync()
                    Tick = 0
                End If
            Case 3
                If Tick >= 30 Then
                    RaiseEvent doSync()
                    Tick = 0
                End If
            Case 4
                If Tick >= 15 Then
                    RaiseEvent doSync()
                    Tick = 0
                End If
        End Select
    End Sub

    Public Sub AddAction(ByVal Name As String, ByRef thisarray(,) As String)

        If IsNothing(thisarray) Then
            With Me.lst_Status
                .Items.Add("")
                With .Items(.Items.Count - 1)
                    .Text = Name
                    .SubItems.Add("")
                End With
            End With
        Else

            Dim match(,) = Nothing
            match = ar.SubSet(thisarray, 1, Name)

            If IsNothing(match) Then
                With Me.lst_Status
                    .Items.Add("")
                    With .Items(.Items.Count - 1)
                        .Text = Name
                        .SubItems.Add("")
                    End With
                End With
            Else
                For i As Integer = 0 To UBound(match, 2)
                    With Me.lst_Status
                        .Items.Add("")
                        With .Items(.Items.Count - 1)
                            .Text = Name
                            .SubItems.Add(match(2, i))
                        End With
                    End With
                Next
            End If
        End If

    End Sub

    Public Function VerifySettings() As Boolean
        Dim ret As Boolean = True

        If Not RSSHasActions() Then
            ret = False
        End If

        If Not regex_WebService.IsMatch(My.Settings.Demo_App_priwebsvc_Service) Then
            ret = False
        End If

        If Not regex_String.IsMatch(My.Settings.Username) Then
            ret = False
        End If

        If Not regex_String.IsMatch(My.Settings.Warehouse) Then
            ret = False
        End If

        Return ret
    End Function

    Public Function SaveSettings() As Boolean

        Dim ret As Boolean = True

        If Not HasActions() Then
            ret = False
            MsgBox("Please associate a status with each action.", MsgBoxStyle.OkOnly)
        Else
            With Me.lst_Status
                For i As Integer = .Items.Count - 1 To 0 Step -1
                    If Len(.Items(i).SubItems(1).Text) = 0 Then
                        .Items.RemoveAt(i)
                    End If
                Next
            End With
            SaveActions()
        End If

        If regex_WebService.IsMatch(Me.txt_ServiceURL.Text) Then
            My.Settings.Demo_App_priwebsvc_Service = Me.txt_ServiceURL.Text
        Else
            MsgBox("Invalid Service URL.", MsgBoxStyle.OkOnly)
            ret = False
        End If

        If regex_String.IsMatch(Me.txt_UserName.Text) Then
            My.Settings.Username = Me.txt_UserName.Text
        Else
            MsgBox("Invalid Username.", MsgBoxStyle.OkOnly)
            ret = False
        End If

        If regex_String.IsMatch(Me.txt_Warehouse.Text) Then
            My.Settings.Warehouse = Me.txt_Warehouse.Text
        Else
            MsgBox("Invalid Warehouse.", MsgBoxStyle.OkOnly)
            ret = False
        End If

        My.Settings.CheckOnStart = Me.Chk_CheckOnStart.Checked
        My.Settings.CheckFreq = Me.CheckFreq.Value

        My.Settings.Save()

        If ret Then ' if saved
            ' Notify the datasets to refresh is they depend upon
            ' data in the settings
            For i As Integer = 0 To UBound(_App.rss)
                With _App.rss(i)
                    If Not IsNothing(_App.rss(i)) Then
                        .ChangeSettings()
                    End If
                End With
            Next
        End If

        Timer1.Enabled = CBool(Me.CheckFreq.Value > 0)
        Return ret

    End Function

    Public Function HasActions() As Boolean
        Dim c = 0
        With Me.lst_Status
            For i As Integer = 0 To .Items.Count - 1
                If Len(.Items(i).SubItems(1).Text) > 0 Then
                    Select Case .Items(i).Text
                        Case txt_SetTimeEnRoute
                            c = c + 1
                        Case txt_SetTimeOnSite
                            c = c + 1
                        Case txt_SetTimeFinished
                            c = c + 1
                        Case txt_PostData
                            c = c + 1
                        Case txt_IsReIssue
                            c = c + 1
                    End Select
                End If
            Next
        End With
        Return CBool(c >= num_Actions)
    End Function

    Private Function RSSHasActions() As Boolean
        Dim c = 0
        With _App.rss(o.Actions)
            If Not IsNothing(.thisArray) Then
                For i As Integer = 0 To UBound(.thisArray, 2)
                    If Len(.thisArray(2, i)) > 0 Then
                        Select Case .thisArray(1, i)
                            Case txt_SetTimeEnRoute
                                c = c + 1
                            Case txt_SetTimeOnSite
                                c = c + 1
                            Case txt_SetTimeFinished
                                c = c + 1
                            Case txt_PostData
                                c = c + 1
                            Case txt_IsReIssue
                                c = c + 1
                        End Select
                    End If
                Next
            End If
        End With
        Return CBool(c >= num_Actions)
    End Function

    Private Sub SaveActions()

        Dim rec As Integer = 0
        Dim st As String = ""
        _App.rss(o.Actions).thisArray = Nothing

        With Me.lst_Status
            For i As Integer = 0 To .Items.Count - 1
                If Len(.Items(i).SubItems(1).Text) > 0 Then
                    st = .Items(i).SubItems(1).Text
                    If Len(st) > 0 Then
                        With _App.rss(o.Actions)
                            rec = .NewRecord
                            .thisArray(0, rec) = CStr(i)
                            .thisArray(2, rec) = st
                            Select Case Me.lst_Status.Items(i).Text
                                Case txt_SetTimeEnRoute
                                    .thisArray(1, rec) = txt_SetTimeEnRoute
                                Case txt_SetTimeOnSite
                                    .thisArray(1, rec) = txt_SetTimeOnSite
                                Case txt_SetTimeFinished
                                    .thisArray(1, rec) = txt_SetTimeFinished
                                Case txt_PostData
                                    .thisArray(1, rec) = txt_PostData
                                Case txt_IsReIssue
                                    .thisArray(1, rec) = txt_IsReIssue
                            End Select

                        End With
                    End If
                End If
            Next
        End With
        _App.rss(o.Actions).Save()

    End Sub

    Private Sub wait5()
        Thread.Sleep(5000)
        RaiseEvent doSync()
    End Sub

End Class
