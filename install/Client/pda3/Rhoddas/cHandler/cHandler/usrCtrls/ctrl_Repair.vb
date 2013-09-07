Imports PriorityMobile

Public Class ctrl_Repair
    Inherits iView

#Region "Overrides base class"

    Public Overrides Sub Bind()
        With Me
            .Malfunction.DataBindings.Add("SelectedValue", thisForm.TableData, "malfunction")
            .Resolution.DataBindings.Add("SelectedValue", thisForm.TableData, "resolution")
            .Repair.DataBindings.Add("Text", thisForm.TableData, "repair")
        End With
    End Sub

    Public Overrides Sub CurrentChanged()
        With Me
            ListBind(.Malfunction, "malfunction")
            ListBind(.Resolution, "resolution")
        End With
    End Sub

#End Region

End Class
