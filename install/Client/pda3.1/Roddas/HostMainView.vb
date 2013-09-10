Imports PrioritySFDC
Imports PriorityMobile
Imports System.IO
Imports System.Runtime.InteropServices
Imports System.Xml
Imports System.Reflection

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
            Me.ControlBox = False
        End Set
    End Property

#End Region

#Region "initialisation and finalisation"

    Private Sub init(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Try
            With MainView
                .LoadUE(ue)
                .xf = New xmlForms(.ue, _
                    ue("forms"), _
                    ue("delivery"), _
                    Nothing, _
                    Nothing _
                )
                .xf.Printer = New btZebra.LabelPrinter( _
                    New Point(300, 300), _
                    New Size(576, 0), _
                    .ue.AppPath & "\prnimg\" _
                )
                .LoadViews()

            End With

        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.OkOnly + MsgBoxStyle.Critical, "Fatal Error.")
            ue.Log(ex.Message)
            Application.Exit()
            Exit Sub
        End Try

        Cursor.Current = Cursors.Default
        Me.ShowTaskbar = False

    End Sub

    Private Sub frmMain_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing
        ShowTaskbar = True
        Application.Exit()
    End Sub

    Private Sub frmMain_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Activated
        ShowTaskbar = False
        'Me.WindowState = FormWindowState.Maximized
    End Sub

    Private Sub frmMain_Deactivate(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Deactivate, MyBase.Disposed
        ShowTaskbar = True
        'Me.WindowState = FormWindowState.Normal
    End Sub

#End Region

#Region "Main View Event Handlers"

    Private Sub hLoadiView(ByVal ControlName As String, ByRef view As iView, ByRef thisForm As xForm) Handles MainView.LoadiView
        Dim f As Boolean = False
        With ue
            If Not IsNothing(.CustomHandler) Then
                Try
                    Dim oType As System.Type = .CustomHandler.GetType(String.Format("pdaHandler.{0}", ControlName.Replace(".vb", "")))
                    view = Activator.CreateInstance(oType)
                Catch EX As Exception
                    view = New ctrl_DataGrid
                End Try
            End If
        End With
        view.SetForm(thisForm)
    End Sub

    Public Sub hSyncEvent(ByVal NodeType As String, ByVal EventType As eSyncEventType, ByRef DataXML As OfflineXML) Handles MainView.SyncEvent

        With DataXML
            Select Case EventType
                Case eSyncEventType.BeginDownload
                    If Not DataXML.UrlGetParams.Keys.Contains("VANNUM") Then
                        Dim frmReg As New scan_Reg
                        frmReg.ShowDialog()
                        Cursor.Current = Cursors.Default
                        DataXML.UrlGetParams.Add("VANNUM", frmReg.txtReg.Text)
                        Cursor.Current = Cursors.WaitCursor
                    End If

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
                    Cursor.Current = Cursors.Default
                    Me.ShowTaskbar = False
                    Me.Focus()

            End Select
        End With
    End Sub

    Private Sub hSetForm() Handles MainView.SetForm
        With Me
            .Menu = mainMenu1
            .ControlBox = False
            .FormBorderStyle = Windows.Forms.FormBorderStyle.None
            .WindowState = FormWindowState.Maximized
            .ShowTaskbar = False
        End With
    End Sub

    Private Sub hCloseForm() Handles MainView.BeginClose
        Me.ControlBox = Not (Me.ControlBox)
    End Sub

    Private Sub hSetTaskbar(ByVal Visible As Boolean)
        ShowTaskbar = Visible
    End Sub

#End Region

End Class