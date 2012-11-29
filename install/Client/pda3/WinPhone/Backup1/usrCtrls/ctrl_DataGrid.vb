Imports PriorityMobile

Public Class ctrl_DataGrid
    Inherits iView

    Public Overrides Sub Bind()
        With DataGrid1
            .DataSource = thisForm.TableData
        End With
    End Sub

    Public Overrides Function SubFormVisible(ByVal Name As String) As Boolean
        Return MyBase.SubFormVisible(Name)
    End Function

End Class
