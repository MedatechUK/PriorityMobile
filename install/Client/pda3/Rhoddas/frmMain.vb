Imports PriorityMobile
Imports System.IO
Imports System.Runtime.InteropServices
Imports System.Xml

Public Class frmMain

    Private ue As UserEnv
    Private WithEvents xf As xmlForms
    Private myProgressBar As ProgressBar

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

    Private _ShowTaskbar As Boolean = True
    Private Property ShowTaskbar() As Boolean
        Get
            Return _ShowTaskbar
        End Get
        Set(ByVal value As Boolean)
            Dim h As IntPtr = FindWindow("HHTaskBar", "")
            Select Case value
                Case True
                    ShowWindow(h, &H1)
                    EnableWindow(h, True)
                Case False                    
                    ShowWindow(h, &H0)
                    EnableWindow(h, False)
            End Select
            _ShowTaskbar = value
        End Set
    End Property

    Private ReadOnly Property Paths() As String()
        Get
            Dim ret() As String = { _
                            "report/detail/malfunction", _
                            "report/detail/resolution", _
                            "report/detail/repair", _
                            "report/signature/image", _
                            "report/signature/print", _
                            "report/times" _
                        }
            Return ret
        End Get
    End Property

#End Region

#Region "initialisation and finalisation"

    Private Sub frmMain_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load

        'File.Delete(ue.LocalFolder & "\forms.xml")
        'File.Delete(ue.LocalFolder & "\calls.xml")
        'File.Delete(ue.LocalFolder & "\lookup.xml")

        'Try
        myProgressBar = New ProgressBar
        myProgressBar.Visible = False

        ue = New UserEnv()
        xf = New xmlForms( _
            New OfflineXML(ue, "forms.xml", "forms.xml", ClearCache), _
            New OfflineXML(ue, "calls.xml", "calls.ashx", _
                MsgBox("Syncronise Calls?", MsgBoxStyle.OkCancel) = MsgBoxResult.Ok, _
                AddressOf hSyncEvent), _
            New OfflineXML(ue, "lookup.xml", "lookup.xml", ClearCache), _
            New OfflineXML(ue, "statusrules.xml", "statusrules.xml", ClearCache) _
        )

        'Catch ex As Exception
        '    MsgBox(ex.Message, MsgBoxStyle.OkOnly + MsgBoxStyle.Critical, "Fatal Error.")
        '    Application.Exit()
        '    Exit Sub
        'End Try

        ToolStrip.IconFolder = ue.AppPath & "\icons\"
        AddHandler xf.AddUserControl, AddressOf xf_AddUserControl
        xf.LoadViewControls()

        With Me
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

        Cursor.Current = Cursors.Default
        Me.ShowTaskbar = False

    End Sub

    Private Sub frmMain_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
        If Not MsgBox("Close the application?", MsgBoxStyle.OkCancel, "Quit?") = MsgBoxResult.Ok Then
            e.Cancel = True
        Else
            ShowTaskbar = True
        End If
    End Sub

    Private Sub frmMain_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Activated
        ShowTaskbar = False
    End Sub

    Private Sub frmMain_Deactivate(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Deactivate, MyBase.Disposed
        ShowTaskbar = True
    End Sub

    Private Sub xf_AddUserControl(ByVal ControlName As String, ByRef view As iView, ByRef thisForm As xForm)
        Select Case ControlName
            Case "SUMMARY"
                view = New ctrl_Summary()
            Case "STATUSPANE"
                view = New ctrl_StatusPane
            Case "REPAIR"
                view = New ctrl_Repair
            Case "SIGN"
                view = New ctrl_Sign
            Case "ADDRESS"
                view = New ctrl_Address
            Case "CALLDETAIL"
                view = New ctrl_CallDetail
            Case "PARTSPLANNED"
                view = New ctrl_Parts_Planned
            Case "PARTSACTUAL"
                view = New ctrl_Parts_Actual
            Case "WAREHOUSE"
                view = New ctrl_Warehouse
            Case Else
                view = New ctrl_DataGrid
        End Select
        view.SetForm(thisForm)
    End Sub

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

            .Menu = MainMenu1
            .ControlBox = True
            .FormBorderStyle = Windows.Forms.FormBorderStyle.None
            .WindowState = FormWindowState.Maximized
            .ShowTaskbar = False

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
            .Add(AddressOf btn_Sync, "SYNC.BMP", IsNothing(Current.Parent))
            .Add()
            Active.CurrentForm.Views(Active.CurrentForm.CurrentView).DirectActivations(ToolStrip)
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
        Me.ShowTaskbar = False
        Me.Focus()
    End Sub

    Private Sub hSyncEvent(ByVal sender As System.Object, ByVal e As System.EventArgs)

        With myProgressBar
            If Not .Visible Then
                .Visible = True
                .Focus()
            End If
        End With

        Dim offxml As OfflineXML = sender
        With offxml

            If .SyncItemCount > 0 Then
                myProgressBar.Progress.Value = CInt((.SyncCurrentItem / .SyncItemCount) * 100)
            Else
                myProgressBar.Progress.Value = 1
            End If

            Select Case .SyncEventType
                Case eSyncEventType.BeginDownload
                    'myProgressBar.txt_File.Text = "Download: " & .FileURL
                    'myProgressBar.txt_Action.Text = ""

                Case eSyncEventType.EndDownload
                    'myProgressBar.txt_File.Text = ""
                    'myProgressBar.txt_Action.Text = ""

                Case eSyncEventType.NewNode
                    Select Case .CurrentType.ToLower
                        Case "servicecall"
                            'myProgressBar.txt_File.Text = "Sync: " & .FileURL
                            'myProgressBar.txt_Action.Text = "Adding call: " & .CurrentNode.SelectSingleNode("callnumber").InnerText
                            .MakePaths(Paths)

                        Case "part"
                            'myProgressBar.txt_File.Text = "Sync: " & .FileURL
                            'myProgressBar.txt_Action.Text = "Adding part: " & .CurrentNode.SelectSingleNode("name").InnerText

                    End Select

                Case eSyncEventType.EditNode
                    Select Case .CurrentType.ToLower
                        Case "servicecall"
                            'myProgressBar.txt_File.Text = "Sync: " & .FileURL
                            'myProgressBar.txt_Action.Text = "Edit call: " & .CurrentNode.SelectSingleNode("callnumber").InnerText
                            .MakePaths(Paths)

                        Case "part"
                            'myProgressBar.txt_File.Text = "Sync: " & .FileURL
                            'myProgressBar.txt_Action.Text = "Edit part: " & .CurrentNode.SelectSingleNode("name").InnerText

                    End Select

                Case eSyncEventType.DeleteNode
                    Select Case .CurrentType.ToLower
                        Case "servicecall"
                            'myProgressBar.txt_File.Text = "Sync: " & .FileURL
                            'myProgressBar.txt_Action.Text = "Remove call: " & .CurrentNode.SelectSingleNode("callnumber").InnerText
                        Case "part"
                            'myProgressBar.txt_File.Text = "Sync: " & .FileURL
                            'myProgressBar.txt_Action.Text = "Remove part: " & .CurrentNode.SelectSingleNode("name").InnerText
                    End Select

                Case eSyncEventType.EndSync
                    'myProgressBar.txt_File.Text = ""
                    'myProgressBar.txt_Action.Text = ""
                    myProgressBar.Progress.Value = 0
                    myProgressBar.Visible = False
                    Me.ShowTaskbar = False
                    Me.Focus()

            End Select
        End With
    End Sub

#End Region

    Private Sub frmMain_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        If (e.KeyCode = System.Windows.Forms.Keys.Up) Then
            'Up
        End If
        If (e.KeyCode = System.Windows.Forms.Keys.Down) Then
            'Down
        End If
        If (e.KeyCode = System.Windows.Forms.Keys.Left) Then
            'Left
        End If
        If (e.KeyCode = System.Windows.Forms.Keys.Right) Then
            'Right
        End If
        If (e.KeyCode = System.Windows.Forms.Keys.Enter) Then
            'Enter
        End If

    End Sub
End Class