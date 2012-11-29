
Partial Class Part
    Inherits DynamicMaster

    Public Overrides Sub hGetMPType() Handles MyBase.GetMPType
        ' Set the parameters of the page when the Pre-Init event fires in DynamicMaster.vb
        With Me
            ' Set basic parameters
            Select Case thisSession.Request("FAMILY")
                Case Else
                    .MasterPage = "default" ' Select which master page (in the folder /Masterpages) to use
            End Select
            .PageTitle = "Ecommerce testing" ' Set a title for this page
        End With
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If hasvar Then
            Dim cph As ContentPlaceHolder = Me.GetPH
            If Not IsNothing(cph) Then
                Dim c As Object = Nothing
                Try
                    Select Case thisSession.Request("FAMILY")
                        Case Else
                            c = LoadControl("~/controls/partTemplate.ascx")
                    End Select
                    With c
                        .ID = "thispart"
                        .PARTNAME = thisSession.Request("PARTNAME")
                    End With
                    cph.Controls.Add(c)
                Catch
                    Exit Sub
                End Try
            End If
        End If
    End Sub

    Private Function hasVar() As Boolean
        Return Not IsNothing(thisSession.Request("FAMILY")) And _
            Not IsNothing(thisSession.Request("PARTNAME"))
    End Function

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