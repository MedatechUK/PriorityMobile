
Partial Class PaymentResponse
    Inherits cmSi.cmsInherit

    Public Overrides Function AdminContext() As Boolean
        Return User.IsInRole("webmaster")
    End Function

    Private ReadOnly Property ph_fail() As PlaceHolder
        Get
            Return Master.FindControl("pay_fail")
        End Get
    End Property

    Private ReadOnly Property ph_ok() As PlaceHolder
        Get
            Return Master.FindControl("pay_ok")
        End Get
    End Property

    Public Overrides Sub PageLoaded(ByVal sender As Object, ByVal e As System.EventArgs)
        If Not AdminContext() Or (AdminContext() And Not IsNothing(Request("RESULT"))) Then
            Select Case Request("RESULT")
                Case "00"
                    ph_fail.Visible = False
                    ph_ok.Visible = True
                    Try
                        With cmSi.cmsSessions.CurrentSession(HttpContext.Current).cart
                            .LoadCart(Request("ORDER_ID"))
                            With .Payment
                                .trans = Request("PASREF")
                                .authcode = Request("AUTHCODE")
                                .amount = CDbl(Request("AMOUNT")) / 100
                            End With
                            .SaveCart(.PostGuid)
                            .PostCart(.PostGuid)
                            cmSi.cmsSessions.CurrentSession(HttpContext.Current).cart.CartItems.Clear()
                            cmSi.cmsSessions.CurrentSession(HttpContext.Current).Basket.Clear()
                            
                        End With
                    Catch ex As Exception
                        ph_fail.Visible = True
                        ph_ok.Visible = False
                        Dim lit As New Literal
                        lit.Text = String.Format("<br><p color='#ff0000'>{0}</p>", ex.Message)
                        ph_fail.Controls.Add(lit)
                    End Try
                Case Else
                    ph_fail.Visible = True
                    ph_ok.Visible = False
            End Select
        Else
            ph_fail.Visible = True
            ph_ok.Visible = True
        End If
    End Sub

End Class
