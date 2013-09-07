Imports System.Xml
Imports prnscn.capture
Imports System.Drawing
Imports hookey

Module Module1

    Private myStates As New States
    Private _ScreenW As Integer
    Private _ScreenH As Integer
    Private _prn As Bitmap


    Private _escape As Boolean = False
    Private i As Integer = 0

    Sub Main()

        Try
            hookey.HookKeyboard(AddressOf hHook)
            myStates.Load("triggers.xml")
            _prn = PrintScreen(myStates.ScreenW, myStates.ScreenH)

            Do Until _escape
                Threading.Thread.Sleep(100)
            Loop

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Sub

    Private Sub hHook(ByVal KeyData As KeyEventArgs)

        If (KeyData.vkCode = VK_ESCAPE) Then
            'i += 1
            'lblrpt.Text = i.ToString            
            HookedKey = True
            _escape = True
        End If

    End Sub

End Module

'

'Private Sub Form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
'    HookKeyboard(AddressOf hHook)
'End Sub

'Private Sub hHook(ByVal KeyData As KeyEventArgs)

'    If (KeyData.vkCode = VK_ESCAPE) Then
'        i += 1
'        lblrpt.Text = i.ToString
'        HookedKey = True
'    End If

'End Sub