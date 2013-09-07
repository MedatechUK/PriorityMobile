Imports System.Runtime.InteropServices
Module FullScreen
#Region " API Declarations "
    <DllImport("coredll.dll", SetLastError:=True)> _
    Public Function GetCapture() As IntPtr
    End Function
    <DllImport("aygshell.dll")> _
    Private Function SHFullScreen(ByVal hwndRequester As IntPtr, ByVal dwState As Integer) As Integer
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
    Public Sub SetFullScreen(ByVal hwnd As IntPtr, ByVal dwState As Integer)
        Dim rtn As Integer
        rtn = SHFullScreen(hwnd, dwState)
    End Sub
End Module
