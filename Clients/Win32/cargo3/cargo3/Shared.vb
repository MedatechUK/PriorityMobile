Public Module Module2

    Public Enum eScriptButtons
        btn_Left = 0
        btn_Right = 1
        btn_Double = 2
    End Enum

    Public myStates As States
    Public CurrentState As String

    Public KeyScripts As New Dictionary(Of String, KeyScript)
    Public ScriptForm As cargo3.ScriptFrm

End Module
