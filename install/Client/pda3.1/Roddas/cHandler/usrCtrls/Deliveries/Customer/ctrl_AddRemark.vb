Imports PriorityMobile

Public Class ctrl_AddRemark
    Inherits iView

#Region "Overrides base class"

    Public Overrides Sub Bind()
        With Me
            .Remark.DataBindings.Add("Text", thisForm.TableData, "text")
        End With
    End Sub

#End Region

End Class
