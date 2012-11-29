Imports PriorityMobile
Imports System.IO
Imports System.Runtime.InteropServices
Imports System.Xml

Public Class HostMainView

#Region "Private Properties"

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
                    Me.Focus()
            End Select
            _ShowTaskbar = value
        End Set
    End Property

#End Region

#Region "initialisation and finalisation"

    Private Sub init(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Try

            With MainView
                .xf = New xmlForms( _
                    New OfflineXML(.ue, "forms.xml", "forms.xml", ClearCache), _
                    New OfflineXML(.ue, "calls.xml", "calls.ashx", _
                        MsgBox("Syncronise Calls?", MsgBoxStyle.OkCancel) = MsgBoxResult.Ok, _
                        AddressOf .hSyncEvent, 60), _
                    New OfflineXML(.ue, "lookup.xml", "lookup.xml", ClearCache), _
                    New OfflineXML(.ue, "statusrules.xml", "statusrules.xml", ClearCache) _
                )
                .LoadViews()
            End With

        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.OkOnly + MsgBoxStyle.Critical, "Fatal Error.")
            Application.Exit()
            Exit Sub
        End Try

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

#End Region

#Region "Main View Event Handlers"

    Private Sub hLoadiView(ByVal ControlName As String, ByRef view As iView, ByRef thisForm As xForm) Handles MainView.LoadiView
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

    Private Sub hSyncEvent(ByVal NodeType As String, ByVal EventType As PriorityMobile.eSyncEventType, ByRef DataXML As OfflineXML) Handles MainView.SyncEvent

        With DataXML
            Select Case EventType
                Case eSyncEventType.BeginDownload
                    'myProgressBar.txt_File.Text = "Download: " & .FileURL
                    'myProgressBar.txt_Action.Text = ""

                Case eSyncEventType.EndDownload
                    'myProgressBar.txt_File.Text = ""
                    'myProgressBar.txt_Action.Text = ""

                Case eSyncEventType.NewNode
                    Select Case NodeType.ToLower
                        Case "servicecall"
                            'myProgressBar.txt_File.Text = "Sync: " & .FileURL
                            'myProgressBar.txt_Action.Text = "Adding call: " & .CurrentNode.SelectSingleNode("callnumber").InnerText
                            .MakePaths(Paths)

                        Case "part"
                            'myProgressBar.txt_File.Text = "Sync: " & .FileURL
                            'myProgressBar.txt_Action.Text = "Adding part: " & .CurrentNode.SelectSingleNode("name").InnerText

                    End Select

                Case eSyncEventType.EditNode
                    Select Case NodeType.ToLower
                        Case "servicecall"
                            'myProgressBar.txt_File.Text = "Sync: " & .FileURL
                            'myProgressBar.txt_Action.Text = "Edit call: " & .CurrentNode.SelectSingleNode("callnumber").InnerText
                            .MakePaths(Paths)

                        Case "part"
                            'myProgressBar.txt_File.Text = "Sync: " & .FileURL
                            'myProgressBar.txt_Action.Text = "Edit part: " & .CurrentNode.SelectSingleNode("name").InnerText

                    End Select

                Case eSyncEventType.DeleteNode
                    Select Case NodeType.ToLower
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
                    Me.ShowTaskbar = False
                    Me.Focus()

            End Select
        End With
    End Sub

    Private Sub hSetForm() Handles MainView.SetForm
        With Me
            .Menu = mainMenu1
            .ControlBox = True
            .FormBorderStyle = Windows.Forms.FormBorderStyle.None
            .WindowState = FormWindowState.Maximized
            .ShowTaskbar = False
        End With
    End Sub

    Private Sub hSetTaskbar(ByVal Visible As Boolean)
        ShowTaskbar = Visible
    End Sub

#End Region

    Private Sub HostMainView_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Resize
        'Me.MainView.Height = Screen.PrimaryScreen.WorkingArea.Height
    End Sub

End Class