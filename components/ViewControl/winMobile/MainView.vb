﻿Public Class MainView

#Region "Public Declarations"

    Public ue As UserEnv
    Public WithEvents xf As xmlForms

#End Region

#Region "Public Events"

    Public Event LoadiView(ByVal ControlName As String, ByRef view As iView, ByRef thisForm As xForm)
    Public Event SyncEvent(ByVal NodeType As String, ByVal EventType As PriorityMobile.eSyncEventType, ByRef DataXML As OfflineXML)
    Public Event SetForm()
    Public Event SetTaskbar(ByVal Visible As Boolean)
    Public Event BeginClose()

#End Region

#Region "Initialisation and finalisation"

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        ue = New UserEnv()

    End Sub

    Public Sub LoadViews()

        With Me
            .ToolStrip.IconFolder = .ue.AppPath & "\icons\"
            AddHandler .xf.AddUserControl, AddressOf xf_AddUserControl
            .xf.LoadViewControls()

            With .mnu_TopForms
                .Font = New Font("Microsoft Sans Serif", 10, FontStyle.Bold)
                .ForeColor = Color.FromArgb(0, 0, 255)
                .BackColor = Color.FromArgb(240, 240, 240)
                For Each t As TopLevelForm In xmlForms.TopForm.Values
                    .Add(t.TopForm.FormName)
                Next
                .Height = .Internalheight
                .MakeImage()
                .Selected(0) = True
                AddHandler .ItemClick, AddressOf hTopMenuClick
            End With
        End With

        With Active.CurrentForm
            .RefreshForm()
            .RefreshSubForms()
            .RefreshDirectActivations()
        End With

    End Sub

    Private Sub xf_AddUserControl(ByVal ControlName As String, ByRef view As iView, ByRef thisForm As xForm)
        RaiseEvent LoadiView(ControlName, view, thisForm)
    End Sub

#End Region

#Region "View Control References"

    Private ReadOnly Property Active() As TopLevelForm
        Get
            With xmlForms.TopForm
                Return .Item(.Keys(xf.ActiveForm))
            End With
        End Get
    End Property

    Private ReadOnly Property Current() As xForm
        Get
            Return Active.CurrentForm
        End Get
    End Property

#End Region

#Region "Draw Interface elements"

    Private Sub DrawForm() Handles xf.DrawForm
        With Me
            With .ContentPanel
                With .Controls
                    .Clear()
                    .Add(Current.Views(Current.CurrentView))
                End With
                .Controls(.Controls.Count - 1).Dock = DockStyle.Fill
                Current.Views(Current.CurrentView).SetFocus()
            End With
            RaiseEvent SetForm()
        End With
    End Sub

    Private Sub DrawSubMenu() Handles xf.DrawSubMenu
        With Me
            With .mnu_SubForms
                .Clear()
                .Font = New Font("Microsoft Sans Serif", 10, FontStyle.Bold)
                .ForeColor = Color.FromArgb(0, 0, 255)
                .BackColor = Color.FromArgb(240, 240, 240)
                For Each s As xForm In Current.VisibleSubForms.Values
                    .Add(s.FormName)
                Next
                .Height = .Internalheight
                .MakeImage()
                RemoveHandler .ItemClick, AddressOf hSubMenuClick
                AddHandler .ItemClick, AddressOf hSubMenuClick
            End With
        End With
    End Sub

    Private Sub DrawDirectActivations() Handles xf.DrawDirectActivations

        With ToolStrip
            .Clear()
            .Add(AddressOf btn_Up_Click, "UP1LVL.BMP", Not IsNothing(Current.Parent))
            .Add(AddressOf btn_View_Click, Active.CurrentForm.NextViewButton, CBool(Current.Views.Count > 1))            
            .Add()
            Active.CurrentForm.Views(Active.CurrentForm.CurrentView).DirectActivations(ToolStrip)
            .Add()
            .Add(AddressOf btn_Sync, "SYNC.BMP", IsNothing(Current.Parent))
            .Add()
            .Add(AddressOf btn_Close, "close.BMP", IsNothing(Current.Parent))
            .MakeImage()
        End With

    End Sub

    Public Sub StartCalc(ByVal Max As Integer) Handles xf.StartCalc
        With Me
            With .ContentPanel
                With .Controls
                    .Clear()
                    Dim C As New calc
                    C.Max = Max
                    AddHandler C.SetNumber, AddressOf Active.CurrentForm.Views(Active.CurrentForm.CurrentView).SetNumber
                    .Add(C)
                End With
                .Controls(.Controls.Count - 1).Dock = DockStyle.Fill

            End With
        End With
    End Sub

    Public Sub StartDialog(ByVal frmDialog As PriorityMobile.UserDialog) Handles xf.StartDialog
        With Me
            With .ContentPanel
                With .Controls
                    .Clear()
                    AddHandler frmDialog.CloseDialog, AddressOf Active.CurrentForm.Views(Active.CurrentForm.CurrentView).CloseDialog
                    .Add(frmDialog)
                End With
                .Controls(.Controls.Count - 1).Dock = DockStyle.Fill

            End With
        End With
    End Sub

#End Region

#Region "Interface Event Handlers"

    Private Sub hTopMenuClick(ByVal Button As Integer)
        If Not (xf.ActiveForm = Button) Then
            xf.ActiveForm = Button
            With Active.CurrentForm
                .RefreshForm()
                .RefreshSubForms()
                .RefreshDirectActivations()
            End With
        End If
        mnu_TopForms.Selected(Button) = True
    End Sub

    Private Sub hSubMenuClick(ByVal Button As Integer)
        Active.OpenForm(mnu_SubForms.itemText(Button))
        DrawSubMenu()
        DrawForm()
    End Sub

    Private Sub btn_Up_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Active.CloseForm()
        DrawSubMenu()
        DrawForm()
    End Sub

    Private Sub btn_View_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Current.CurrentView += 1
        DrawForm()
    End Sub

    Private Sub btn_Sync(ByVal sender As System.Object, ByVal e As System.EventArgs)
        xf.Sync()
        Current.Bind()
        For Each v As iView In Current.Views
            v.Bind()
        Next
        RaiseEvent SetTaskbar(False)

    End Sub

    Private Sub btn_Close(ByVal sender As System.Object, ByVal e As System.EventArgs)
        RaiseEvent BeginClose()
    End Sub

    Public Sub hSyncEvent(ByVal sender As System.Object, ByVal e As System.EventArgs)

        ProgressBar.Visible = True
        ToolStrip.Visible = False

        Dim offxml As OfflineXML = sender
        With offxml

            If .SyncItemCount > 0 Then
                ProgressBar.Value = CInt((.SyncCurrentItem / .SyncItemCount) * 100)
            Else
                ProgressBar.Value = 1
            End If

            RaiseEvent SyncEvent(offxml.CurrentType, .SyncEventType, offxml)
            Select Case .SyncEventType
                Case eSyncEventType.EndSync
                    ProgressBar.Visible = False
                    ToolStrip.Visible = True
            End Select

        End With

    End Sub

#End Region

End Class
