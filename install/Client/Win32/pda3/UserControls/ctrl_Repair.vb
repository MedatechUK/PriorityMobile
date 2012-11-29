Public Class ctrl_Repair
    Inherits ViewControl.iView

#Region "Overrides base class"

    Public Overrides Sub Bind()
        With Me
            .malfunction.DataBindings.Add("SelectedValue", thisForm.TableData, "malfunction")
            .resolution.DataBindings.Add("SelectedValue", thisForm.TableData, "resolution")
            .repair.DataBindings.Add("Text", thisForm.TableData, "repair")
        End With
    End Sub

    Public Overrides Sub CurrentChanged()
        With Me
            ListBind(.malfunction, "malfunction")
            ListBind(.resolution, "resolution")
        End With
    End Sub

#End Region

End Class
