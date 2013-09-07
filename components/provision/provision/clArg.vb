Public Class clArg
    Inherits Dictionary(Of String, String)

    Private Enum eMode
        Switch
        Param
    End Enum

    Sub New(ByVal Args As String())

        Dim i As Integer = 0
        Dim m As eMode = eMode.Switch
        Dim thisSwitch As String = ""

        Do
            Select Case Args(i).Substring(0, 1)
                Case "-", "/"
                    Add(Args(i).Substring(1), "")
                    thisSwitch = Args(i).Substring(1)
                    m = eMode.Param
                Case Else
                    Select Case m
                        Case eMode.Param                            
                            Me(thisSwitch) = Args(i)
                            thisSwitch = ""
                            m = eMode.Switch

                        Case eMode.Switch
                            Add(Args(i), "")

                    End Select

            End Select
            i += 1
        Loop Until i = Args.Count

    End Sub

End Class
