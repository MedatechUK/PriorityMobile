Imports system.io

Public Class cfSettings

    Private _priwebsvc_Service As String = ""
    Private _CheckFreq As Integer = 0
    Private _CheckOnStart As Boolean = False
    Private _Username As String = ""
    Private _Warehouse As String = ""

    Public Property priwebsvc_Service() As String
        Get
            Return _priwebsvc_Service
        End Get
        Set(ByVal value As String)
            _priwebsvc_Service = value
        End Set
    End Property

    Public Property CheckFreq() As Integer
        Get
            Return _CheckFreq
        End Get
        Set(ByVal value As Integer)
            _CheckFreq = value
        End Set
    End Property

    Public Property CheckOnStart() As Boolean
        Get
            Return _CheckOnStart
        End Get
        Set(ByVal value As Boolean)
            _CheckOnStart = value
        End Set
    End Property

    Public Property Username() As String
        Get
            Return _Username
        End Get
        Set(ByVal value As String)
            _Username = value
        End Set
    End Property

    Public Property Warehouse() As String
        Get
            Return _Warehouse
        End Get
        Set(ByVal value As String)
            _Warehouse = value
        End Set
    End Property

    Public Sub Load()

        If File.Exists(CStr(Get_app_path() & "\settings.txt")) Then
            Using sr As New StreamReader(CStr(Get_app_path() & "\settings.txt"))
                While Not sr.EndOfStream
                    Dim l As String = Trim(sr.ReadLine)
                    If Len(l) > 0 Then
                        If InStr(l, "=") > 0 And Left(l, 1) <> "#" Then
                            Dim name As String = LCase(Split(l, "=")(0))
                            Dim value As String = Split(l, "=")(1)
                            Select Case name
                                Case "priwebsvc_service"
                                    _priwebsvc_Service = value
                                Case "checkfreq"
                                    _CheckFreq = CInt(value)
                                Case "checkonstart"
                                    _CheckOnStart = CBool(value)
                                Case "username"
                                    _Username = value
                                Case "warehouse"
                                    _Warehouse = value
                            End Select
                        End If
                    End If
                End While
            End Using
        End If
    End Sub

    Public Sub Save()
        Dim fi As String = CStr(Get_app_path() & "\settings.txt")
        While File.Exists(fi)
            File.Delete(fi)
        End While

        Using sw As New StreamWriter(fi, False)
            sw.Write("")
            sw.WriteLine("priwebsvc_Service=" & _priwebsvc_Service)
            sw.WriteLine("CheckFreq=" & _CheckFreq)
            sw.WriteLine("CheckOnStart=" & _CheckOnStart)
            sw.WriteLine("Username=" & _Username)
            sw.WriteLine("Warehouse=" & _Warehouse)
            sw.Close()
        End Using

    End Sub

    Private Function Get_app_path() As String
        Dim fullPath As String = System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase
        Return fullPath.Substring(0, fullPath.LastIndexOf("\"))
    End Function

End Class