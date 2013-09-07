Imports hookey

Module WaitForKeys

    Private _WaitForKey As Integer = Nothing
    Private _WaitKey As Boolean = False

    Public Sub hHook(ByVal KeyData As KeyEventArgs)
        If (KeyData.vkCode = _WaitForKey) Then
            'i += 1
            'lblrpt.Text = i.ToString            
            HookedKey = True
            _WaitKey = False
            _WaitForKey = Nothing
        End If
    End Sub

    Public Sub KeyWait(ByVal Key As Integer)
        _WaitForKey = Key
        _WaitKey = True
        Do Until Not _WaitKey
            Application.DoEvents()
        Loop
    End Sub

End Module
