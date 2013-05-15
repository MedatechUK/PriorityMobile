Public Class iForm

    Public Sub New(ByRef intf As sfdc3.cInterface)
        InitializeComponent()

        With Me
            With .FormView
                .Load(intf.Form)                
            End With
            With .TableView
                .Load(intf.Table)
            End With
        End With

    End Sub

End Class