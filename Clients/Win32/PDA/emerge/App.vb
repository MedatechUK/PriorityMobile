Imports System
Imports System.IO
Imports System.Threading

Public Class AppForm
    Inherits PDAOnBoardData.BaseForm 'System.Windows.Forms.Form '   '  '  '  '  '  '  ' '  '      ' ' 

    Public Shared WarningLock As Object = New Object()
    Dim ar As New MyCls.MyArray

    Dim _panelName As String = Nothing
    Dim _NoStatus As Boolean = False
    Dim _Sounds As String = ""
    Dim _ServiceCall As String = ""
    Dim _appexit As Boolean = False

    ' Hidden Tabs
    Dim _onsiteTabs As Boolean = False
    Dim _tabRepair As TabPage = Nothing
    Dim _tabParts As TabPage = Nothing

    ' Create User Controls
    Dim Details As ct_Details
    Dim Repair As ct_Repair
    Dim PartsUsed As ct_PartsUsed
    Dim dtSel As ct_DateSelect
    Dim Address As ct_Address
    Dim Signature As New ct_Sign
    Dim Survey As ct_Survey
    Dim Settings As ct_Settings

    Private Declare Auto Function PlaySound Lib "winmm.dll" _
        (ByVal lpszSoundName As String, ByVal hModule As Integer, _
        ByVal dwFlags As Integer) As Integer

    Private Const SND_FILENAME As Integer = &H20000

#Region "Delegate Subs"

    Private Delegate Sub delegateSetWarning()
    Private Delegate Sub delegateWHnewData()
    Private Delegate Sub delegatePanels()
    Private Delegate Sub delegateLockForm()
    Private Delegate Sub delegateUnLockForm()
    Private Delegate Sub initSettings()
    Private Delegate Sub rDraw()

    Sub SetWarning()
        Try
            Dim ct As delegateSetWarning
            ct = AddressOf SetWarningText
            Invoke(ct)
        Catch
        End Try
    End Sub

    Sub dorDraw()
        Dim ct As rDraw
        ct = AddressOf dtSel.rDraw
        Invoke(ct)
    End Sub

    Sub doinit()
        Dim ct As initSettings
        ct = AddressOf Settings.init
        Invoke(ct)
    End Sub

    Sub doPanels()
        Dim ct As delegatePanels
        ct = AddressOf EnablePanel
        Invoke(ct)
    End Sub

    Sub doLockForm()
        Dim ct As delegateLockForm
        ct = AddressOf Lockform
        Invoke(ct)
    End Sub

    Sub doUnlockForm()
        Dim ct As delegateLockForm
        ct = AddressOf UnLockform
        Invoke(ct)
    End Sub

    Sub dodelegateWHnewData()
        Dim ct As delegateWHnewData
        ct = AddressOf PartsUsed.RefreshWarehouse
        Invoke(ct)
    End Sub

#End Region

#Region "Initialisation and Finalisation"

    Private Sub AppForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        ' Set paths
        Dim MyBasePath As String = "C:\PDA" 'System.AppDomain.CurrentDomain.BaseDirectory()
        SavePath = MyBasePath & "\DATA"
        PostPath = MyBasePath & "\MAIL\OUTBOX"
        SentPath = MyBasePath & "\MAIL\SENT"
        _Sounds = MyBasePath & "\SOUNDS"

        ' Load the picture into a Bitmap.
        Dim bm As New Bitmap(MyBasePath & "\logo.BMP", True)

        ' Set the splash screen
        With splashlogo
            .Image = bm
            .Width = bm.Width
            .Height = bm.Height
        End With


        ' Set the background colour
        InnerArea.BackColor = bm.GetPixel(0, 0) 'Color.FromArgb(255, 8, 32, 82)

        ' And refresh the form
        Me.AppForm_Resize(sender, e)

        ' Create The data objects
        AddRSS(o.ServiceCall, New _ServiceCall(Me))
        AddRSS(o.Warehouse, New _Warehouse(Me))
        AddRSS(o.Statuses, New _Statuses(Me))
        AddRSS(o.Details, New _Details(Me))
        AddRSS(o.Malfunction, New _Malfunction(Me))
        AddRSS(o.Resolution, New _Resolution(Me))
        AddRSS(o.Survey, New _Survey(Me))
        AddRSS(o.Repair, New _Repair(Me))
        AddRSS(o.Time, New _Time(Me))
        AddRSS(o.Parts, New _Parts(Me))
        AddRSS(o.Signature, New _Signature(Me))
        AddRSS(o.Answers, New _Answers(Me))
        AddRSS(o.Flags, New _Flags(Me))
        AddRSS(o.Actions, New _Actions(Me))
        AddRSS(o.Cancel, New _Cancel(Me))
        AddRSS(o.DayEnd, New _DayEnd(Me))

        ' Create the User interface objects

        dtSel = New ct_DateSelect(Me)
        AddHandler dtSel.OpenCall, AddressOf hOpenCall
        AddHandler dtSel.CloseCall, AddressOf hCloseCall
        AddHandler dtSel.StatusChange, AddressOf hStatusChange
        With dtSel
            .Dock = DockStyle.Fill
            .DatePick.Value = Now
        End With

        Settings = New ct_Settings(Me)
        AddHandler Settings.NoStatus, AddressOf hNoStatus
        AddHandler Settings.doSync, AddressOf hdoSync
        With Settings
            .Dock = DockStyle.Fill
            .init()
        End With

        PartsUsed = New ct_PartsUsed(Me)
        With PartsUsed
            .Dock = DockStyle.Fill
        End With

        Address = New ct_Address(Me)
        With Address
            .Dock = DockStyle.Fill
        End With

        Repair = New ct_Repair(Me)
        With Repair
            .Dock = DockStyle.Fill
        End With

        Details = New ct_Details(Me)
        With Details
            .Dock = DockStyle.Fill
        End With

        Signature = New ct_Sign(Me)
        With Signature
            .Dock = DockStyle.Fill
        End With

        Survey = New ct_Survey(Me)
        With Survey
            .Dock = DockStyle.Fill
        End With

        ' Add controls to their placeholders on the form
        Me.pnl_DateSelect.Controls.Add(dtSel)
        Me.pnl_Parts.Controls.Add(PartsUsed)
        Me.pnl_Address.Controls.Add(Address)
        Me.pnl_Repair.Controls.Add(Repair)
        Me.pnl_Details.Controls.Add(Details)
        Me.pnl_Signature.Controls.Add(Signature)
        Me.pnl_Survey.Controls.Add(Survey)
        Me.pnl_Settings.Controls.Add(Settings)

        If _appexit Then
            Me.Close()
            Exit Sub
        End If


        ' Set the panels                
        _panelName = ""
        EnablePanel()
        dtSel.refreshCallList()

        With Me.CallTab
            _tabRepair = .TabPages(2)
            _tabParts = .TabPages(3)
            .TabPages.RemoveAt(3)
            .TabPages.RemoveAt(2)
        End With

        If Not (_NoStatus) Then
            If Not Settings.VerifySettings Then
                SyncToolStripMenuItem.Visible = False
                _panelName = Me.pnl_Settings.Name
                EnablePanel()
                Me.SettingsMenu.Visible = True
            End If
        End If

    End Sub

#End Region

#Region "PDA Data Event Handlers"

    Overrides Function hSend(ByVal LoadData As String) As Boolean
        Dim ws As New priwebsvc.Service
        Try
            Return ws.LoadData(LoadData)
        Catch ex As Exception
            Return False
        End Try
    End Function

    Overrides Sub hNewData(ByVal EventIndex As Integer, ByVal o As PDAOnBoardData.PDAData)

        If NewEvent(EventIndex) Then
            Select Case o.Name
                Case "ServiceCall"
                    If Not IsNothing(rss(0).thisArray) Then
                        'Me.SetCallMax(UBound(rss(0).thisArray, 2))
                        'Me.LoadCall(CallSelect.Value)
                    End If
                    PlayWav(_Sounds & "\alarm.wav")
                Case "Warehouse"
                    dodelegateWHnewData()
                    MsgBox(o.Name & " was sucsessfully downloaded.")
                Case "Statuses"
                    If _NoStatus Then
                        _NoStatus = False
                        doinit()
                        If Not _NoStatus Then
                            doUnlockForm()
                            _panelName = Me.pnl_Settings.Name
                            doPanels()
                            SyncToolStripMenuItem.Visible = False
                            SettingsMenu.Visible = True
                        End If
                    Else
                        MsgBox("Statuses were sucsessfully downloaded.")
                    End If

                Case "Malfunction"
                    MsgBox(o.Name & " was sucsessfully downloaded.")
                Case "Resolution"
                    MsgBox(o.Name & " was sucsessfully downloaded.")
                Case "Survey"
                    MsgBox(o.Name & " was sucsessfully downloaded.")
            End Select
        End If

    End Sub

    Overrides Sub hWarning(ByVal EventIndex As Integer, ByVal o As PDAOnBoardData.PDAData, ByVal SubName As String, ByVal Message As String, ByVal infoOnly As Boolean)

        If NewEvent(EventIndex) Then
            If Not (infoOnly) Then
                MsgBox(o.Name & " : " & SubName & vbCrLf & Message, _
                MsgBoxStyle.Critical, _
                "Warning!")
            Else
                ' info only event
                Monitor.Enter(WarningLock)
                Try
                    ReDim Preserve _Warnings(2, UBound(_Warnings, 2) + 1)
                Catch ex As Exception
                    ReDim _Warnings(2, 0)
                End Try

                _Warnings(0, UBound(_Warnings, 2)) = CStr(EventIndex)
                _Warnings(1, UBound(_Warnings, 2)) = o.Name & ": " & Message
                _Warnings(2, UBound(_Warnings, 2)) = ""

                If Not _dispWarn Then
                    _dispWarn = True
                    ' Create new Warning message thread 
                    Dim myThread As Thread
                    myThread = New Thread(New ThreadStart(AddressOf doWarnings))
                    myThread.Start()
                Else
                    Thread.Sleep(0)
                End If
                Monitor.Exit(WarningLock)
            End If

        End If

    End Sub

    Overrides Sub hCancelWarning(ByVal EventIndex As Integer)

        If Not IsNothing(_Warnings) Then
            For i As Integer = 0 To UBound(_Warnings, 2)
                Try
                    If _Warnings(0, i) = CStr(EventIndex) Then
                        _Warnings(2, i) = "X"
                    End If
                Catch
                End Try
            Next
        End If
        'If Not IsNothing(_Warnings) Then 

    End Sub

    Overrides Sub hFormRedraw(ByVal EventIndex As Integer, ByVal o As PDAOnBoardData.PDAData)

        ' Redraw the form
        If NewEvent(EventIndex) Then
            dorDraw()
        End If

    End Sub

    Overrides Sub hSigSaved(ByVal EventIndex As Integer, ByVal o As PDAOnBoardData.PDAData, ByVal Ordinal As Integer)

        ' Signature was saved
        If NewEvent(EventIndex) Then
            With o
                .currentOrdinal = Ordinal
                If .Validate Then
                    If .doLoading(o) Then
                        .DeleteIndex()
                    End If
                End If
            End With
        End If

    End Sub

#End Region

#Region "Interface event handlers"

    Private Sub hOpenCall(ByVal DOCNO As String)

        _ServiceCall = DOCNO

        ' If on-site
        If Me.dtSel.IsOnsite(_ServiceCall) Then
            ' Add repair and parts tabs 
            CallTab.TabPages.Insert(2, _tabParts)
            CallTab.TabPages.Insert(3, _tabRepair)
            Me.SurveysToolStripMenuItem1.Enabled = True
            _onsiteTabs = True
        Else
            Me.SurveysToolStripMenuItem1.Enabled = False
            _onsiteTabs = False
        End If

        Me.CallTab.SelectedIndex = 0
        With rss(o.ServiceCall)
            .currentIndex = _ServiceCall
        End With

        ' Load the controls
        Me.PartsUsed.ServiceCall = _ServiceCall
        Me.Address.ServiceCall = _ServiceCall
        Me.Repair.ServiceCall = _ServiceCall
        Me.Details.ServiceCall = _ServiceCall
        Me.Signature.ServiceCall = _ServiceCall
        Me.Survey.ServiceCall = _ServiceCall    

        ' Check if new and prompt for en-route
        Me.dtSel.FirstOpen(_ServiceCall)

        ' Set the panels
        _panelName = Me.pnl_CallTabs.Name
        EnablePanel()

        Me.Call_menu.Visible = True
        SyncToolStripMenuItem.Visible = False

    End Sub

    Private Sub hStatusChange(ByVal DOCNO As String)

        _ServiceCall = DOCNO

    End Sub

    Private Sub hCloseCall(ByVal DOCNO As String)

        Me.Call_menu.Visible = True
        Me.Signature.ServiceCall = DOCNO

        SyncToolStripMenuItem.Visible = False
        SurveysToolStripMenuItem1.Enabled = False

        ' Set the panels
        _panelName = Me.pnl_Signature.Name
        EnablePanel()

    End Sub

    Private Sub hdoSync()
        If Settings.VerifySettings Then
            rss(o.ServiceCall).BeginSeek(rss(o.ServiceCall))
            rss(o.Details).BeginSeek(rss(o.Details))
            rss(o.Cancel).BeginSeek(rss(o.Cancel))
        End If
    End Sub

    Private Sub hNoStatus()
        _NoStatus = True
        If MsgBox("No Status file was found." & vbCrLf & "Download now?", MsgBoxStyle.OkCancel, "Missing file.") = MsgBoxResult.Ok Then
            Lockform()
            rss(2).BeginSeek(rss(2))
        Else
            Me.Close()
            _appexit = True
        End If
    End Sub

#End Region

#Region "Toolbar Commands"

    Private Sub DayEndToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DayEndToolStripMenuItem.Click
        With rss(o.DayEnd)
            If MsgBox("Send end of working day message?", MsgBoxStyle.YesNo, "End Day") = MsgBoxResult.Yes Then
                .doLoading(rss(o.DayEnd))
                MsgBox("Message Sent.", MsgBoxStyle.OkOnly, "End Day.")
            End If
        End With
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub CallsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CallsToolStripMenuItem.Click
        hdoSync()
    End Sub

    Private Sub AllToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AllToolStripMenuItem.Click
        rss(1).BeginSeek(rss(1))
        rss(2).BeginSeek(rss(2))
        rss(4).BeginSeek(rss(4))
        rss(5).BeginSeek(rss(5))
        rss(6).BeginSeek(rss(6))
    End Sub

    Private Sub MalfunctionCodesToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MalfunctionCodesToolStripMenuItem.Click
        rss(4).BeginSeek(rss(4))
    End Sub

    Private Sub ResolutionCodesToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ResolutionCodesToolStripMenuItem.Click
        rss(5).BeginSeek(rss(5))
    End Sub

    Private Sub StatusesToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles StatusesToolStripMenuItem.Click
        rss(2).BeginSeek(rss(2))
    End Sub

    Private Sub WarehouseToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles WarehouseToolStripMenuItem.Click
        rss(1).BeginSeek(rss(1))
    End Sub

    Private Sub SurveysToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SurveysToolStripMenuItem.Click
        rss(6).BeginSeek(rss(6))
    End Sub

    Private Sub QuitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles QuitToolStripMenuItem.Click

        If Me.pnl_Signature.Visible Then
            SaveCall()

            _panelName = pnl_DateSelect.Name
            EnablePanel()
            dtSel.refreshCallList()

            SyncToolStripMenuItem.Visible = True
            SurveysToolStripMenuItem1.Enabled = True
        End If

        If Me.pnl_Survey.Visible Then
            Select Case Survey.Mode
                Case 0
                    ' Set the panels
                    _panelName = Me.pnl_CallTabs.Name
                    EnablePanel()
                    Me.SurveysToolStripMenuItem1.Enabled = True
                    Me.DrawingsToolStripMenuItem.Enabled = True

                Case 1
                    Survey.SaveSurvey()
                    Survey.DrawMenu()

                Case 2
                    Survey.CloseTextEdit()

            End Select

        Else

            If _onsiteTabs Then
                Try
                    With Me.CallTab
                        .TabPages.RemoveAt(3)
                        .TabPages.RemoveAt(2)
                    End With
                Catch
                    Beep()
                End Try
            End If

            dtSel.refreshCallList(Address.ServiceCall)
            Me.Call_menu.Visible = False

            ' Set the panels
            Me.SyncToolStripMenuItem.Visible = True
            _panelName = Me.pnl_DateSelect.Name
            EnablePanel()
            dtSel.Focus()

        End If

    End Sub

    Private Sub SettingsClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles SettingsToolStripMenuItem.Click
        SyncToolStripMenuItem.Visible = False
        _panelName = Me.pnl_Settings.Name
        EnablePanel()
        Me.SettingsMenu.Visible = True
    End Sub

    Private Sub SurveyClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SurveysToolStripMenuItem1.Click

        ' Set the panels
        _panelName = Me.pnl_Survey.Name
        EnablePanel()

        Me.SurveysToolStripMenuItem1.Enabled = False
        Me.DrawingsToolStripMenuItem.Enabled = False

        Survey.ServiceCall = _ServiceCall
        Survey.DrawMenu()

    End Sub

    Private Sub SaveSettingToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveSettingToolStripMenuItem.Click
        If Settings.SaveSettings Then
            ' Save and Quit
            SyncToolStripMenuItem.Visible = True
            Me.SettingsMenu.Visible = False
            _panelName = pnl_DateSelect.Name
            EnablePanel()
        End If
    End Sub

    Private Sub QuitNoSaveToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles QuitNoSaveToolStripMenuItem.Click

        ' Quit no save
        If MsgBox("Settings will not be saved." & vbCrLf & "Are you sure?", MsgBoxStyle.Exclamation + MsgBoxStyle.YesNo, "Save...") = MsgBoxResult.Yes Then
            SyncToolStripMenuItem.Visible = True
            Me.SettingsMenu.Visible = False
            _panelName = pnl_DateSelect.Name
            EnablePanel()
        End If

    End Sub

#Region "Add Action to settings"

    Private Sub SetTimeEnRouteToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SetTimeEnRouteToolStripMenuItem.Click
        'Set Time En-Route
        Settings.AddAction(txt_SetTimeEnRoute, Nothing)
    End Sub

    Private Sub SetTimeOnSiteToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SetTimeOnSiteToolStripMenuItem.Click
        'Set Time On-Site
        Settings.AddAction(txt_SetTimeOnSite, Nothing)
    End Sub

    Private Sub SetTimeFinishedToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SetTimeFinishedToolStripMenuItem.Click
        'Set Time Finished
        Settings.AddAction(txt_SetTimeFinished, Nothing)
    End Sub

    Private Sub PostDataToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PostDataToolStripMenuItem.Click
        'Post Data
        Settings.AddAction(txt_PostData, Nothing)
    End Sub

#End Region

#End Region

#Region "Panel Functions"

    Public Sub Lockform()
        With Me
            .SyncToolStripMenuItem.Enabled = False
            With .dtSel
                .CallList.Enabled = False
                .DatePick.Enabled = False
            End With
        End With
    End Sub

    Public Sub UnLockform()
        With Me
            .SyncToolStripMenuItem.Enabled = True
            With .dtSel
                .CallList.Enabled = True
                .DatePick.Enabled = True
            End With
        End With
    End Sub

    Private Sub ShowPanel(ByRef pnl As Panel, ByVal Show As Boolean)
        With pnl
            Select Case Show
                Case False
                    .Dock = DockStyle.None
                    .Top = Me.Height
                    .Left = 0
                    .Width = 0
                    .Height = 0
                    .Visible = False
                Case True
                    .Dock = DockStyle.Fill
                    .Top = 0
                    .Left = 0
                    .Width = Me.Width
                    .Height = Me.Height - Status.Height
                    .Visible = True
            End Select
        End With

    End Sub

    Public Sub EnablePanel()
        ' Set the panels
        With Me

            If _panelName = "pnl_Signature" Then
                ShowPanel(.pnl_Signature, True)
            Else
                ShowPanel(.pnl_Signature, False)
            End If

            If _panelName = "pnl_CallTabs" Then
                ShowPanel(.pnl_CallTabs, True)
            Else
                ShowPanel(.pnl_CallTabs, False)
            End If

            If _panelName = "pnl_Survey" Then
                ShowPanel(.pnl_Survey, True)
            Else
                ShowPanel(.pnl_Survey, False)
            End If

            If _panelName = "pnl_DateSelect" Then
                ShowPanel(.pnl_DateSelect, True)
            Else
                ShowPanel(.pnl_DateSelect, False)
            End If

            If _panelName = "pnl_Settings" Then
                ShowPanel(.pnl_Settings, True)
            Else
                ShowPanel(.pnl_Settings, False)
            End If

            If _panelName = "pnl_Drawing" Then
                ShowPanel(.pnl_Drawing, True)
            Else
                ShowPanel(.pnl_Drawing, False)
            End If

        End With
    End Sub

#End Region

#Region "Private Functions"

    Private Sub SaveCall()

        Signature.SaveSig()

        With rss(o.Repair)
            .currentIndex = _ServiceCall
            If .Validate Then
                If .doLoading(rss(o.Repair)) Then
                    .DeleteIndex()
                End If
            End If
        End With

        With rss(o.Time)
            .currentIndex = _ServiceCall
            If .Validate Then
                If .doLoading(rss(o.Time)) Then
                    .DeleteIndex()
                End If
            End If
        End With

        With rss(o.Parts)
            .currentIndex = _ServiceCall
            If .Validate Then
                If .doLoading(rss(o.Parts)) Then
                    .DeleteIndex()
                End If
            End If
        End With

        With rss(o.Flags)
            .currentIndex = _ServiceCall
            If .Validate Then
                If .doLoading(rss(o.Flags)) Then
                    .DeleteIndex()
                End If
            End If
        End With

        With rss(o.Answers)
            .currentIndex = _ServiceCall
            If .Validate Then
                If .doLoading(rss(o.Answers)) Then
                    .DeleteIndex()
                End If
            End If
        End With

        With rss(o.ServiceCall)
            .currentIndex = _ServiceCall
            .DeleteIndex()
        End With

    End Sub

    Private Function PlayWav(ByVal fileFullPath As String) _
            As Boolean

        'return true if successful, false if otherwise
        Dim iRet As Integer = 0

        Try

            iRet = PlaySound(fileFullPath, 0, SND_FILENAME)

        Catch

        End Try

        Return iRet

    End Function

    Private Sub AppForm_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize

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

        Me.CallTab.Font = c
        'Me.MenuStrip1.Font = c

        With Me.InnerArea
            .Left = 1
            .Width = Me.Width - 10
            .Top = Me.MenuStrip1.Top + Me.MenuStrip1.Height
            .Height = Status.Top - (Me.MenuStrip1.Top + Me.MenuStrip1.Height)
        End With

        With splashlogo ' center the splash
            .Top = (InnerArea.Height - .Height) / 2
            .Left = (InnerArea.Width - .Width) / 2
        End With
    End Sub

    Private Sub InnerArea_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles InnerArea.Click, splashlogo.Click
        _panelName = pnl_DateSelect.Name
        EnablePanel()
        dtSel.refreshCallList()
    End Sub

#End Region

#Region "Status bar messages"

    Public Sub SetWarningText()
        If Not IsNothing(_Warnings) Then
            StatusLabel.Text = _Warnings(1, UBound(_Warnings, 2))
            If Not _Warnings(2, UBound(_Warnings, 2)) = "" Then
                ar.DeleteByCriteria(_Warnings, 0, _Warnings(0, UBound(_Warnings, 2)))
            End If
        Else
            StatusLabel.Text = ""
        End If
    End Sub

    Private Sub doWarnings()

        While Not IsNothing(_Warnings) And Not rss(o.ServiceCall).appExit
            SetWarning()
            Thread.Sleep(100)
        End While
        SetWarning()
        _dispWarn = False

    End Sub

#End Region

End Class