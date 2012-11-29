Imports System.Runtime.InteropServices
Imports System.Diagnostics

Module StartUp

    Public ClearCache As Boolean = False

#Region " API Declarations "

    Public Const SW_HIDE As Integer = &H0
    Public Const SW_SHOW As Integer = &H1

    <DllImport("coredll.dll", CharSet:=CharSet.Auto)> _
    Public Function FindWindow(ByVal lpClassName As String, ByVal lpWindowName As String) As IntPtr
    End Function

    <DllImport("coredll.dll", CharSet:=CharSet.Auto)> _
    Public Function ShowWindow(ByVal hwnd As IntPtr, ByVal nCmdShow As Integer) As Boolean
    End Function

    <DllImport("coredll.dll", CharSet:=CharSet.Auto)> _
    Public Function EnableWindow(ByVal hwnd As IntPtr, ByVal enabled As Boolean) As Boolean
    End Function

    <DllImport("coredll.dll", SetLastError:=True)> _
    Public Function GetCapture() As IntPtr
    End Function

    <DllImport("aygshell.dll")> _
    Public Function SHFullScreen(ByVal hwndRequester As IntPtr, ByVal dwState As Integer) As Integer
    End Function

    Public Enum SHFS As Integer
        SHOWTASKBAR = &H1
        HIDETASKBAR = &H2
        SHOWSIPBUTTON = &H4
        HIDESIPBUTTON = &H8
        SHOWSTARTICON = &H10
        HIDESTARTICON = &H20
    End Enum

#End Region

    Public Sub Main(ByVal args As String())

            For Each arg As String In args
                Select Case arg.ToLower
                    Case "clearcache"
                        ClearCache = True
                End Select
            Next
            Application.Run(New HostMainView) 'frmMain

    End Sub

End Module
