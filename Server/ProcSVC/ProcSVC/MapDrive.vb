Imports System.Runtime.InteropServices
Imports System.Text


    ''' <summary>
    ''' Network Drive Mapping class / wrapper
    ''' </summary>
    ''' <remarks>Maps, ummaps and general functions for network drives</remarks>
    Public Class cNetworkDrive

#Region "      Public variables and functions"

        Private _saveCredentials As Boolean = False
        ''' <summary>
        ''' Option to save credentials on reconnection...
        ''' </summary>
        Public Property SaveCredentials() As Boolean
            Get
                Return _saveCredentials
            End Get
            Set(ByVal value As Boolean)
                _saveCredentials = value
            End Set
        End Property

        Private _persistent As Boolean = False
        ''' <summary>
        ''' Option to reconnect drive after log off / reboot...
        ''' </summary>
        Public Property Persistent() As Boolean
            Get
                Return _persistent
            End Get
            Set(ByVal value As Boolean)
                _persistent = value
            End Set
        End Property

        Private _force As Boolean = False
        ''' <summary>
        ''' Option to force connection if drive is already mapped...
        ''' or force disconnection if network path is not responding...
        ''' </summary>
        Public Property Force() As Boolean
            Get
                Return _force
            End Get
            Set(ByVal value As Boolean)
                _force = value
            End Set
        End Property

        Private _promptForCredentials As Boolean = False
        ''' <summary>
        ''' Option to prompt for user credintals when mapping a drive
        ''' </summary>
        Public Property PromptForCredentials() As Boolean
            Get
                Return _promptForCredentials
            End Get
            Set(ByVal value As Boolean)
                _promptForCredentials = value
            End Set
        End Property

        Private _findNextFreeDrive As Boolean = False
        ''' <summary>
        ''' Option to auto select the 'LocalDrive' property to next free driver letter when mapping a network drive
        ''' </summary>
        Public Property FindNextFreeDrive() As Boolean
            Get
                Return _findNextFreeDrive
            End Get
            Set(ByVal value As Boolean)
                _findNextFreeDrive = value
            End Set
        End Property

        Private _localDrive As String = Nothing
        ''' <summary>
        ''' Drive to be used in mapping / unmapping (eg. 's:')
        ''' </summary>
        Public Property LocalDrive() As String
            Get
                Return _localDrive
            End Get
            Set(ByVal value As String)
                If value = Nothing Or value.Length = 0 Then
                    _localDrive = Nothing
                Else
                    _localDrive = value.Substring(0, 1) + ":"
                End If
            End Set
        End Property

        Private _shareName As String = ""
        ''' <summary>
        ''' Share address to map drive to. (eg. '\\Computer\C$')
        ''' </summary>
        Public Property ShareName() As String
            Get
                Return _shareName
            End Get
            Set(ByVal value As String)
                _shareName = value
            End Set
        End Property

        ''' <summary>
        ''' Returns a string array of currently mapped network drives
        ''' </summary>
        Public ReadOnly Property MappedDrives() As String()
            Get
                Dim arrayRet As System.Collections.ArrayList = New System.Collections.ArrayList()
                For Each driveLetter As String In System.IO.Directory.GetLogicalDrives()
                    If (PathIsNetworkPath(driveLetter)) Then
                        arrayRet.Add(driveLetter)
                    End If
                Next
                Return CType(arrayRet.ToArray(String.Empty.GetType()), String())
            End Get
        End Property

#End Region

#Region "      Public functions"

        ''' <summary>
        ''' Map network drive
        ''' </summary>
        Public Sub MapDrive()
            _mapDrive(Nothing, Nothing)
        End Sub

        ''' <summary>
        ''' Map network drive (using supplied Username and Password)
        ''' </summary>
        ''' <param name="username">Username passed for permissions / credintals ('Username' may be passed as null, to map using only a password)</param>
        ''' <param name="password">Password passed for permissions / credintals</param>
        Public Sub MapDrive(ByVal username As String, ByVal password As String)
            _mapDrive(username, password)
        End Sub

        ''' <summary>
        ''' Set common propertys, then map the network drive
        ''' </summary>
        ''' <param name="localDrive">LocalDrive to use for connection</param>
        ''' <param name="shareName">Share name for the connection (eg. '\\Computer\Share')</param>
        ''' <param name="force">Option to force dis/connection</param>
        Public Sub MapDrive(ByVal localDrive As String, ByVal shareName As String, ByVal force As Boolean)
            _localDrive = localDrive
            _shareName = shareName
            _force = force
            _mapDrive(Nothing, Nothing)
        End Sub

        ''' <summary>
        ''' Set common propertys, then map the network drive
        ''' </summary>
        ''' <param name="localDrive">Password passed for permissions / credintals</param>
        ''' <param name="force">Option to force dis/connection</param>
        Public Sub MapDrive(ByVal localDrive As String, ByVal force As Boolean)
            _localDrive = localDrive
            _force = force
            _mapDrive(Nothing, Nothing)
        End Sub

        ''' <summary>
        ''' Unmap network drive
        ''' </summary>
        Public Sub UnMapDrive()
            _unMapDrive()
        End Sub

        ''' <summary>
        ''' Unmap network drive
        ''' </summary>
        Public Sub UnMapDrive(ByVal localDrive As String)
            _localDrive = localDrive
            _unMapDrive()
        End Sub

        ''' <summary>
        ''' Unmap network drive
        ''' </summary>
        Public Sub UnMapDrive(ByVal localDrive As String, ByVal force As Boolean)
            _localDrive = localDrive
            _force = force
            _unMapDrive()
        End Sub

        ''' <summary>
        ''' Check / restore persistent network drive
        ''' </summary>
        Public Sub RestoreDrives()
            _restoreDrive(Nothing)
        End Sub

        ''' <summary>
        ''' Check / restore persistent network drive
        ''' </summary>
        Public Sub RestoreDrive(ByVal localDrive As String)
            _restoreDrive(localDrive)
        End Sub

        ''' <summary>
        ''' Display windows dialog for mapping a network drive (using Desktop as parent form)
        ''' </summary>		
        Public Sub ShowConnectDialog()
            _displayDialog(System.IntPtr.Zero, 1)
        End Sub

        ''' <summary>
        ''' Display windows dialog for mapping a network drive
        ''' </summary>
        ''' <param name="parentFormHandle">Form used as a parent for the dialog</param>
        Public Sub ShowConnectDialog(ByVal parentFormHandle As System.IntPtr)
            _displayDialog(parentFormHandle, 1)
        End Sub

        ''' <summary>
        ''' Display windows dialog for disconnecting a network drive (using Desktop as parent form)
        ''' </summary>		
        Public Sub ShowDisconnectDialog()
            _displayDialog(System.IntPtr.Zero, 2)
        End Sub

        ''' <summary>
        ''' Display windows dialog for disconnecting a network drive
        ''' </summary>
        ''' <param name="parentFormHandle">Form used as a parent for the dialog</param>
        Public Sub ShowDisconnectDialog(ByVal parentFormHandle As System.IntPtr)
            _displayDialog(parentFormHandle, 2)
        End Sub

        ''' <summary>
        ''' Returns the share name of a connected network drive
        ''' </summary>
        ''' <param name="localDrive">Drive name (eg. 'X:')</param>
        ''' <returns>Share name (eg. \\computer\share)</returns>
        Public Function GetMappedShareName(ByVal localDrive As String) As String

            ' collect and clean the passed LocalDrive param
            If localDrive = Nothing Or localDrive.Trim().Length = 0 Then
                Throw New System.Exception("Invalid 'localDrive' passed, 'localDrive' parameter cannot be 'empty'")
            End If
            localDrive = localDrive.Substring(0, 1)

            ' call api to collect LocalDrive's share name 
            Dim i As Integer = 255
            Dim bSharename(i) As Byte
            Dim iCallStatus As Integer = WNetGetConnection(localDrive + ":", bSharename, i)
            Select Case iCallStatus
                Case 1201
                    Throw New System.Exception("Cannot collect 'ShareName', Passed 'DriveName' is valid but currently not connected (API: ERROR_CONNECTION_UNAVAIL)")
                Case 1208
                    Throw New System.Exception("API function 'WNetGetConnection' failed (API: ERROR_EXTENDED_ERROR:" + iCallStatus.ToString() + ")")
                Case 1203
                    Throw New System.Exception("Cannot collect 'ShareName', No network connection found (API: ERROR_NO_NETWORK / ERROR_NO_NET_OR_BAD_PATH)")
                Case 1222
                    Throw New System.Exception("Cannot collect 'ShareName', No network connection found (API: ERROR_NO_NETWORK / ERROR_NO_NET_OR_BAD_PATH)")
                Case 2250
                    Throw New System.Exception("Invalid 'DriveName' passed, Drive is not a network drive (API: ERROR_NOT_CONNECTED)")
                Case 1200
                    Throw New System.Exception("Invalid / Malfored 'Drive Name' passed to 'GetShareName' function (API: ERROR_BAD_DEVICE)")
                Case 234
                    Throw New System.Exception("Invalid 'Buffer' length, buffer is too small (API: ERROR_MORE_DATA)")
            End Select

            ' return collected share name
            Return System.Text.Encoding.GetEncoding(1252).GetString(bSharename, 0, i).TrimEnd(Convert.ToChar(0))

        End Function

        '''' <summary>
        '''' Returns true if passed drive is a network drive
        '''' </summary>
        '''' <param name="localDrive">Drive name (eg. 'X:')</param>
        '''' <returns>'True' if the passed drive is a mapped network drive</returns>
        'Public Function IsNetworkDrive(ByVal localDrive As String) As Boolean

        '    ' collect and clean the passed LocalDrive param
        '    If localDrive = Nothing Or localDrive.Trim().Length = 0 Then
        '        Throw New System.Exception("Invalid 'LocalDrive' passed, 'DriveName' parameter cannot be 'empty'")
        '    End If
        '    localDrive = localDrive.Substring(0, 1)

        '    ' return status of drive type
        '    Return PathIsNetworkDrive(localDrive + ":")

        'End Function

#End Region

#Region "      Private functions"

        ' map network drive
        Private Sub _mapDrive(ByVal username As String, ByVal password As String)

            ' if drive property is set to auto select, collect next free drive			
            If _findNextFreeDrive Then
                _localDrive = _nextFreeDrive()
                If _localDrive = Nothing Or _localDrive.Length = 0 Then
                    Throw New System.Exception("Could not find valid free drive name")
                End If
            End If

            ' create struct data to pass to the api function
            Dim stNetRes As structNetResource = New structNetResource()
            stNetRes.Scope = 2
            stNetRes.Type = RESOURCETYPE_DISK
            stNetRes.DisplayType = 3
            stNetRes.Usage = 1
            stNetRes.RemoteName = _shareName
            stNetRes.LocalDrive = _localDrive

            ' prepare flags for drive mapping options
            Dim iFlags As Integer = 0
            If _saveCredentials Then
                iFlags += CONNECT_CMD_SAVECRED
            End If
            If _persistent Then
                iFlags += CONNECT_UPDATE_PROFILE
            End If
            If _promptForCredentials Then
                iFlags += CONNECT_INTERACTIVE + CONNECT_PROMPT
            End If

            ' prepare username / password params
            If username <> Nothing And username.Length = 0 Then
                username = Nothing
            End If
            If password <> Nothing And password.Length = 0 Then
                password = Nothing
            End If

            ' if force, unmap ready for new connection
            If _force Then
                Try
                    _unMapDrive()
                Catch
                End Try
            End If

            ' call and return
            Dim i As Integer = WNetAddConnection(stNetRes, password, username, iFlags)
            If i > 0 Then
                Throw New System.ComponentModel.Win32Exception(i)
            End If

        End Sub

        ' unmap network drive	
        Private Sub _unMapDrive()

            ' prep vars and call unmap
            Dim iFlags As Integer = 0
            Dim iRet As Integer = 0

            ' if persistent, set flag
            If _persistent Then
                iFlags += CONNECT_UPDATE_PROFILE
            End If

            ' if local drive is nothing, unmap with use connection
            If _localDrive = Nothing Then
                ' unmap use connection, passing the share name, as local drive
                iRet = WNetCancelConnection(_shareName, iFlags, System.Convert.ToInt32(_force))
            Else
                ' unmap drive
                iRet = WNetCancelConnection(_localDrive, iFlags, System.Convert.ToInt32(_force))
            End If

            ' if errors, throw exception
            If (iRet > 0) Then
                Throw New System.ComponentModel.Win32Exception(iRet)
            End If

        End Sub

        ' check / restore a network drive
        Private Sub _restoreDrive(ByVal driveName As String)

            ' call restore and return
            Dim i As Integer = WNetRestoreConnection(0, driveName)

            ' if error returned, throw
            If i > 0 Then
                Throw New System.ComponentModel.Win32Exception(i)
            End If

        End Sub

        ' display windows dialog
        Private Sub _displayDialog(ByVal wndHandle As System.IntPtr, ByVal dialogToShow As Integer)

            ' prep variables
            Dim i As Integer = -1
            Dim iHandle As Integer = 0

            ' get parent handle
            If wndHandle.Equals(System.IntPtr.Zero) Then
                iHandle = wndHandle.ToInt32()
            End If

            ' choose dialog to show bassed on 
            If dialogToShow = 1 Then
                i = WNetConnectionDialog(iHandle, RESOURCETYPE_DISK)
            ElseIf dialogToShow = 2 Then
                i = WNetDisconnectDialog(iHandle, RESOURCETYPE_DISK)
            End If

            ' if error returned, throw
            If i > 0 Then
                Throw New System.ComponentModel.Win32Exception(i)
            End If

        End Sub

        ' returns the next viable drive name to use for mapping
        Private Function _nextFreeDrive() As String

            ' loop from c to z and check that drive is free
            Dim retValue As String = Nothing

            Dim i As Integer
            For i = 67 To 90
                Dim charI As Char = Convert.ToChar(i)

                If GetDriveType(charI.ToString() + ":") = 1 Then

                    retValue = charI.ToString() + ":"
                    Exit For

                End If

            Next

            ' return selected drive
            Return retValue

        End Function

#End Region

#Region "      API functions / calls"

        <DllImport("mpr.dll", EntryPoint:="WNetAddConnection2A", SetLastError:=True, CharSet:=CharSet.Ansi, ExactSpelling:=True, CallingConvention:=CallingConvention.StdCall)> _
        Private Shared Function WNetAddConnection(ByRef netResStruct As structNetResource, ByVal username As String, ByVal password As String, ByVal flags As Integer) As Integer
        End Function

        <DllImport("mpr.dll", EntryPoint:="WNetCancelConnection2A", SetLastError:=True, CharSet:=CharSet.Ansi, ExactSpelling:=True, CallingConvention:=CallingConvention.StdCall)> _
        Private Shared Function WNetCancelConnection(ByVal name As String, ByVal flags As Integer, ByVal force As Integer) As Integer
        End Function

        <DllImport("mpr.dll", EntryPoint:="WNetConnectionDialog", SetLastError:=True)> _
        Private Shared Function WNetConnectionDialog(ByVal hWnd As Integer, ByVal type As Integer) As Integer
        End Function

        <DllImport("mpr.dll", EntryPoint:="WNetDisconnectDialog", SetLastError:=True, CharSet:=CharSet.Auto, ExactSpelling:=True, CallingConvention:=CallingConvention.StdCall)> _
        Private Shared Function WNetDisconnectDialog(ByVal hWnd As Integer, ByVal type As Integer) As Integer
        End Function

        <DllImport("mpr.dll", EntryPoint:="WNetRestoreConnectionW", SetLastError:=True, CharSet:=CharSet.Unicode)> _
        Private Shared Function WNetRestoreConnection(ByVal hWnd As Integer, ByVal localDrive As String) As Integer
        End Function

        <DllImport("mpr.dll", EntryPoint:="WNetGetConnection", SetLastError:=True, CharSet:=CharSet.Auto, ExactSpelling:=True, CallingConvention:=CallingConvention.StdCall)> _
        Private Shared Function WNetGetConnection(ByVal localDrive As String, ByVal remoteName As Byte(), ByRef bufferLength As Integer) As Integer
        End Function

        <DllImport("shlwapi.dll", EntryPoint:="PathIsNetworkPath", SetLastError:=True)> _
        Private Shared Function PathIsNetworkPath(ByVal localDrive As String) As Boolean

        End Function

        <DllImport("kernel32.dll", EntryPoint:="GetDriveType", SetLastError:=True, CharSet:=CharSet.Auto, ExactSpelling:=True, CallingConvention:=CallingConvention.StdCall)> _
        Private Shared Function GetDriveType(ByVal localDrive As String) As Integer
        End Function


        <StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)> _
        Private Structure structNetResource
            Public Scope As Integer
            Public Type As Integer
            Public DisplayType As Integer
            Public Usage As Integer
            Public LocalDrive As String
            Public RemoteName As String
            Public Comment As String
            Public Provider As String
        End Structure

        ' standard
        Public Const RESOURCETYPE_DISK As Integer = &H1
        Public Const CONNECT_INTERACTIVE As Integer = &H8
        Public Const CONNECT_PROMPT As Integer = &H10
        Public Const CONNECT_UPDATE_PROFILE As Integer = &H1
        ' ie4+
        Public Const CONNECT_REDIRECT As Integer = &H80
        ' nt5+
        Public Const CONNECT_COMMANDLINE As Integer = &H800
        Public Const CONNECT_CMD_SAVECRED As Integer = &H1000

#End Region

    End Class




