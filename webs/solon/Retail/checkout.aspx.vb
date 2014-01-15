Imports cmSi

Partial Class checkout
    Inherits cmsInherit

    Public Overrides Sub LoadReplaceModules()
        addReplaceModule(New cmSi.repl_Profile)
    End Sub

    Public Overrides Function AdminContext() As Boolean
        Return User.IsInRole("webmaster")
    End Function

    Public Overrides Sub PageLoaded(ByVal sender As Object, ByVal e As System.EventArgs)
       
    End Sub

    Private Function ValidAddress() As Boolean
        With Profile.Address
            If .Address1.Length = 0 Then Return False
            If .Address3.Length = 0 Then Return False
            If .Address4.Length = 0 Then Return False
            If .Postcode.Length = 0 Then Return False
        End With
        With Profile.Name
            If .First.Length = 0 Then Return False
            If .Last.Length = 0 Then Return False
        End With
        Return True
    End Function

    Private ReadOnly Property prf() As ProfileBase
        Get
            Return HttpContext.Current.Profile
        End Get
    End Property
    Private Sub Redirect()

        Dim realAuth As MerchantRedirect.realAuth
        With ts.cart
            .CustomerID = prf("CUSTNAME")
            With .DeliveryAddress
                .First = prf.GetProfileGroup("Name").Item("First")
                .Last = prf.GetProfileGroup("Name").Item("Last")
                If IsNothing(.First) Or IsNothing(.Last) Then
                    Response.Write("Error: please visit profile")
                End If
                .Address1 = prf.GetProfileGroup("Address").Item("Address1")
                .Address2 = prf.GetProfileGroup("Address").Item("Address2")
                .Address3 = prf.GetProfileGroup("Address").Item("Address3")
                .Address4 = prf.GetProfileGroup("Address").Item("Address4")
                .Address5 = prf.GetProfileGroup("Address").Item("Address5")
                .Address_Postcode = prf.GetProfileGroup("Address").Item("Postcode")
                .Phone = prf.GetProfileGroup("Address").Item("Phone")
                .eMail = User.Identity.Name
            End With

            realAuth = New MerchantRedirect.realAuth( _
                cmSi.cmsData.Settings.Get("MerchantName"), _
                cmSi.cmsData.Settings.Get("transAccount"), _
                .SaveCart, _
                .Total.ToString.Replace(".", ""), _
                .CURRENCY.ToString, _
                cmSi.cmsData.Settings.Get("normalPassword") _
            )
        End With

        Dim post As New cmsHTTPPost(realAuth.PostURL)
        For Each k As String In realAuth.PostValues.Keys
            post.NameValues.Add(k, realAuth.PostValues(k))
        Next
        Try
            cmSi.cmsSessions.CurrentSession(HttpContext.Current).cart.CartItems.Clear()
            post.Post()
        Catch
        End Try
    End Sub

    Protected Sub btnSaveProfile_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveProfile.Click
        If ValidAddress() Then
            Redirect()
        End If
    End Sub
End Class
