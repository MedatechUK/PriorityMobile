
Partial Class Login_UserManager
    Inherits DynamicMaster

    Public Overrides Sub hGetMPType() Handles MyBase.GetMPType
        ' Set the parameters of the page when the Pre-Init event fires in DynamicMaster.vb
        With Me
            ' Set basic parameters
            .MasterPage = "default" ' Select which master page (in the folder /Masterpages) to use
            .PageTitle = "User Manger" ' Set a title for this page
        End With
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "DO NOT EDIT"
    Public Overrides Function GetPH(Optional ByVal ph_Name As String = Nothing) As Object
        ' returns a reference to the specified object in the content placeholder
        ' or a reference to the content placeholder itself if ph_Name is ommitted
        ' DO NOT EDIT!
        Dim cph As ContentPlaceHolder = Me.Form.FindControl("ContentPlaceHolder1")
        If Not IsNothing(cph) Then
            If IsNothing(ph_Name) Then
                Return cph
            Else
                Return cph.FindControl(ph_Name)
            End If
        Else
            Return Nothing
        End If
    End Function

#End Region

End Class
