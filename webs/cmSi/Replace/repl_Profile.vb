Imports System.Xml
Imports System.Web.UI.WebControls
Imports System.Web

Public Class repl_Profile : Inherits repl_Base

#Region "Control Definitions"

    Public Overrides ReadOnly Property ReplaceModule() As String
        Get
            Return "repl_Profile"
        End Get
    End Property

    Public Overrides ReadOnly Property Controls() As List(Of ReplaceControl)
        Get
            Dim ret As New List(Of ReplaceControl)
            With ret
                .Add(New ReplaceControl("System.Web.UI.WebControls.TextBox", "*", AddressOf hProfileTextbox))
                .Add(New ReplaceControl("System.Web.UI.WebControls.Button", "btnSaveProfile", AddressOf hbtnSaveProfile))
            End With
            Return ret
        End Get
    End Property

#End Region

#Region "Delegate methods"

    Public Sub hbtnSaveProfile(ByVal sender As Object, ByVal e As repl_Argument)
        Dim btn As Button = sender
        AddHandler btn.Click, AddressOf hSaveProfile
    End Sub

    Public Sub hProfileTextbox(ByVal sender As Object, ByVal e As repl_Argument)
        If Not e.thisPage.IsPostBack Then
            Try
                With e.thisContext.Profile
                    Dim tx As System.Web.UI.WebControls.TextBox = sender
                    If InStr(tx.ID, "_") > 0 Then
                        tx.Text = .GetProfileGroup(sender.ID.Split("_")(0)).Item(sender.ID.Split("_")(1))
                    Else
                        tx.Text = .Item(tx.ID)
                    End If
                End With
            Catch ex As Exception

            End Try
        End If
    End Sub

#End Region

#Region "Event Handlers"

    Public Sub hSaveProfile(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim btn As Button = sender
        Dim P As cmsPage = New cmsPage(btn.Page, HttpContext.Current, btn.Page.Server)
        With HttpContext.Current
            For Each k As String In .Request.Form.Keys
                Try
                    Dim KeyVal As String = k.Split("$").Last
                    If InStr(KeyVal, "_") > 0 Then
                        If KeyVal = "Address_Postcode" Then
                            .Profile.GetProfileGroup(KeyVal.Split("_")(0)).Item(KeyVal.Split("_")(1)) = P.FormDictionary(KeyVal).ToUpper
                        Else
                            .Profile.GetProfileGroup(KeyVal.Split("_")(0)).Item(KeyVal.Split("_")(1)) = P.FormDictionary(KeyVal)
                        End If
                    Else
                        .Profile(k) = P.FormDictionary(KeyVal)
                    End If
                Catch
                End Try
            Next
            .Response.Redirect("basket.aspx") '.Request.Url.AbsoluteUri)
        End With
    End Sub

#End Region

End Class
