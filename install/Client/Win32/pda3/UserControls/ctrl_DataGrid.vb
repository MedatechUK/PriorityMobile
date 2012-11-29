Public Class ctrl_DataGrid
    Inherits ViewControl.iView

    Public Overrides Sub Bind()
        With DataGridView1
            .DataSource = thisForm.TableData
        End With
    End Sub

    Public Overrides Function SubFormVisible(ByVal Name As String) As Boolean
        Return MyBase.SubFormVisible(Name)
    End Function

End Class
