Public Class frmMenu

#Region "Declarations"

    Dim ws As New PriWebSVC.Service
    Dim sd As New Priority.SerialData
    Dim ar As New Priority.MyArray

    Dim activeInterface As w32SFDCData.iForm
    Dim flogin As frmLogin
    Dim misClosing As Boolean = False

    Dim ip As String
    Dim port As String
    Dim myStatus As String = ""
    Dim frm As frmMenu

    Dim mModuleMenu As MenuItem = Nothing

    Public app_path As String = Get_app_path()

#End Region

#Region "Public Properties"

    Public Property IsClosing() As Boolean
        Get
            Return misClosing
        End Get
        Set(ByVal value As Boolean)
            misClosing = value
        End Set
    End Property

    Public Property ModuleMenu() As MenuItem
        Get
            Return mModuleMenu
        End Get
        Set(ByVal value As MenuItem)
            mModuleMenu = value
        End Set
    End Property

#End Region

#Region "Delegate subs called from within the main thread"

    Delegate Sub UpdateTextDelegate(ByVal message As String)
    Delegate Sub CloseMeDelegate()

    Sub UpdateText(ByVal message As String)
        Me.Status.Text = message
    End Sub

    Sub CloseMe()
        Me.Close()
    End Sub

    Sub setstatus(ByVal message As String)
        Dim del As New UpdateTextDelegate(AddressOf UpdateText)
        Dim params() As Object = {message}
        Status.Invoke(del, params)
    End Sub

    Sub CloseForm()
        Dim del As New CloseMeDelegate(AddressOf CloseMe)
        Me.Invoke(del)
    End Sub

#End Region

#Region "Initialisation and Finalisation"

    Public Sub AddRSS(ByVal i As Integer, ByVal otype As w32SFDCData.iForm)

        rss(i) = otype
        With rss(i)
            AddHandler .SendArray, AddressOf handles_SendArray
            AddHandler .Send, AddressOf handles_Send
            AddHandler .NewForm, AddressOf handles_NewForm
            AddHandler .RegisterModule, AddressOf handles_NewModule
            AddHandler .doTableScan, AddressOf handles_TableScan
            .Posted = True
        End With

    End Sub

    Private Sub frmMenu_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Width = 260 'Screen.PrimaryScreen.WorkingArea.Width
        Me.Height = 320 'Screen.PrimaryScreen.WorkingArea.Height

        Dim p As System.Drawing.Point
        p.X = (Screen.PrimaryScreen.WorkingArea.Width - Me.Width) / 2
        p.Y = (Screen.PrimaryScreen.WorkingArea.Height - Me.Height) / 2
        Me.Location = p

        IsClosing = False

        ' Load the picture into a Bitmap.
        Dim bm As New Bitmap(Get_app_path() & "\logo.BMP")

        ' Set the splash screen
        With SplashLogo
            .Image = bm
            .Width = bm.Width
            .Height = bm.Height
        End With

        ' Set the background colour
        Me.BackColor = Color.White

    End Sub

    Private Function ServiceName() As String
        Dim t As String = ""
        t = Replace(ws.Url, "http://", "", , , CompareMethod.Text)
        If InStr(t, ":") > 0 Then t = Split(t, ":")(0)
        Return t
    End Function

    Public Sub MainLoop()

        ' ******************************************
        ' *** Check the program is still connected

        Dim data As String = ""
        Dim resRetry As MsgBoxResult = MsgBoxResult.Retry

        With frmLogin
            .Text = ServiceName()
            Do
                .ShowDialog()
                If .Result = MsgBoxResult.Cancel Then
                    .Close()
                    Exit Sub
                End If

                resRetry = MsgBoxResult.Retry
                Do While resRetry = MsgBoxResult.Retry
                    Try
                        data = _
                        ws.GetData("select USERLOGIN, WARHSNAME from dbo.v_USERS where UPPER(USERLOGIN)='" & _
                        UCase(frmLogin.UserName) & _
                        "' and UPPER(PASSWORD)='" & _
                        UCase(frmLogin.Password) & _
                        "'")
                        Exit Do
                    Catch ex As Exception
                        resRetry = MsgBox("Connection Failed. " & ex.Message, MsgBoxStyle.RetryCancel)
                    End Try
                Loop

                If Strings.Left(data, 1) = "!" Then
                    MsgBox(Strings.Right(data, Len(data) - 1))
                    Exit Sub
                End If

                Dim ret(,) As String = sd.DeSerialiseData(data)
                If Not IsNothing(ret) Then

                    For i As Integer = 0 To UBound(rss)
                        If Not IsNothing(rss(i)) Then
                            With rss(i)
                                .UserName = ret(0, 0)
                                .Warehouse = ret(1, 0)
                                Me.Status.Text = LCase(.UserName & "@" & ServiceName())
                            End With
                        End If
                    Next

                    frmLogin.Password = ""
                    Show()
                    IsClosing = False
                    While Not IsClosing
                        Application.DoEvents()
                    End While

                Else
                    If MsgBox("Invalid username or password.", MsgBoxStyle.RetryCancel) = MsgBoxResult.Cancel Then Exit Sub
                End If
            Loop

        End With

    End Sub

    Private Sub frmMenu_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing

        Hide()
        e.Cancel = True
        IsClosing = True

    End Sub

#End Region

#Region "Form Interface Handlers"

    Private Sub Start_Interface(ByVal sender As System.Object, ByVal e As System.EventArgs)

        Dim btn As MenuItem = sender
        For i As Integer = 0 To UBound(rss)
            If Not IsNothing(rss(i)) Then
                With rss(i)
                    If .ModuleName = btn.Text Then
                        .Posted = False
                        activeInterface = rss(i)
                        .Loading()
                        Exit For
                    End If
                End With
            End If
        Next

    End Sub

    Private Sub handles_NewModule(ByRef Sender As w32SFDCData.iForm, ByVal mItem As MenuItem, ByVal subMenu As String)

        If Sender.ShowOnMenu Then
            AddHandler mItem.Click, AddressOf Start_Interface

            If Len(subMenu) = 0 Then
                mModuleMenu.MenuItems.Add(mItem)
            Else
                For i As Integer = 0 To MenuItem1.MenuItems.Count - 1
                    If mModuleMenu.MenuItems(i).Text = subMenu Then
                        mModuleMenu.MenuItems(i).MenuItems.Add(mItem)
                        Exit Sub
                    End If
                Next
                Dim mi As New MenuItem
                mi.Text = subMenu
                Me.mModuleMenu.MenuItems.Add(mi)
                For i As Integer = 0 To mModuleMenu.MenuItems.Count - 1
                    If mModuleMenu.MenuItems(i).Text = subMenu Then
                        mModuleMenu.MenuItems(i).MenuItems.Add(mItem)
                        Exit Sub
                    End If
                Next
            End If
        End If

    End Sub

    Private Sub handles_SendArray(ByRef Sender As w32SFDCData.iForm, ByVal Ar As System.Array)

        Dim resRetry As MsgBoxResult = MsgBoxResult.Retry
        Do While resRetry = MsgBoxResult.Retry
            Try
                ws.LoadData(sd.SerialiseDataArray(Ar))
                Exit Do
            Catch ex As Exception
                resRetry = MsgBox("Connection Failed. " & ex.Message, MsgBoxStyle.RetryCancel)
            End Try
        Loop

    End Sub

    Private Sub handles_Send(ByRef Sender As w32SFDCData.iForm, ByVal Command As String)

        Dim resRetry As MsgBoxResult = MsgBoxResult.Retry
        Do While resRetry = MsgBoxResult.Retry
            Try
                Dim data As String = ws.GetData(Command)
                Sender.c_ReceivedArray(sd.DeSerialiseData(data))
                Exit Do
            Catch ex As Exception
                resRetry = MsgBox("Connection Failed. " & ex.Message, MsgBoxStyle.RetryCancel)
            End Try
        Loop

    End Sub

    Private Sub handles_NewForm(ByRef RSSi As Integer, ByVal Param(,) As String)
        If Not IsNothing(rss(RSSi)) Then
            activeInterface = rss(RSSi)
            With rss(RSSi)
                For i As Integer = 0 To UBound(Param, 2)
                    .Argument(Param(0, i)) = Param(1, i)
                Next
                .Loading()
            End With
        End If
    End Sub

    Private Sub handles_TableScan(ByRef sender As w32SFDCData.iForm, ByVal Value As String)
        sender.TableScan(Value)
    End Sub

    Private Sub MenuItem2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem2.Click
        About.ShowDialog()
    End Sub

#End Region

#Region "Application Settings"

    Public Function LoadSettings() As Boolean

        Dim setting(,) As String

        If ar.ArrayFromFile(setting, app_path & "\settings.txt") Then
            ip = setting(1, ar.InArray(setting, 0, "ip"))
            port = setting(1, ar.InArray(setting, 0, "port"))
            Return True
        Else
            Return False
        End If

    End Function

    Public Sub SaveSettings()

        Dim setting(1, 1) As String

        setting(0, 0) = "ip"
        setting(0, 1) = "port"

        setting(1, 0) = ip
        setting(1, 1) = port

        If Not (ar.ArrayToFile(app_path & "\settings.txt", setting)) Then
            MsgBox("Could not save settings to file.")
        End If

    End Sub

    Public Function Get_app_path() As String
        Return System.AppDomain.CurrentDomain.BaseDirectory()
    End Function

#End Region

End Class