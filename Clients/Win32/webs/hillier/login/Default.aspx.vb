
Partial Class _Default
    Inherits DynamicMaster

#Region "Lookup Profile Controls"

    Private Property txtField(ByVal Name As String) As String
        Get
            Dim container As LoginView = Me.LoginView1
            Dim txt As TextBox = container.FindControl(Name)
            If IsNothing(txt) Then Return Nothing
            Return txt.Text
        End Get
        Set(ByVal value As String)
            Dim container As LoginView = Me.LoginView1
            Dim txt As TextBox = container.FindControl(Name)
            If Not IsNothing(txt) Then
                txt.Text = value
            End If
        End Set
    End Property

    Private Property btnField(ByVal Name As String) As Button
        Get
            Dim container As LoginView = Me.LoginView1
            Dim txt As Button = container.FindControl(Name)
            If IsNothing(txt) Then Return Nothing
            Return txt
        End Get
        Set(ByVal value As Button)
            Dim container As LoginView = Me.LoginView1
            Dim txt As Button = container.FindControl(Name)
            If Not IsNothing(txt) Then
                txt = value
            End If
        End Set
    End Property

    Private ReadOnly Property thisMultiView() As MultiView
        Get
            Dim container As LoginView = Me.LoginView1
            Dim txt As MultiView
            Select Case thisSession.User.Identity.IsAuthenticated
                Case True
                    txt = container.FindControl("LoggedInView")
                Case Else
                    txt = container.FindControl("UnLoggedView")
            End Select

            If IsNothing(txt) Then Return Nothing
            Return txt
        End Get

    End Property

#End Region

#Region "Initialisations"

    Public Overrides Sub hGetMPType() Handles MyBase.GetMPType
        ' Set the parameters of the page when the Pre-Init event fires in DynamicMaster.vb
        With thisSession
            Select Case .User.Identity.IsAuthenticated
                Case True ' Is logged in
                    Select Case .Request.Params("arg")
                        Case "bye"
                            ' Logging off                               
                            olu.CloseSession(.SessionID)
                            FormsAuthentication.SignOut()
                            Session.RemoveAll()
                            With Me
                                .MasterPage = "login" ' Select which master page (in the folder /Masterpages) to use
                                .PageTitle = "Logged off..." ' Set a title for this page
                            End With
                        Case Else
                            ' Edit Profile
                            With Me
                                .MasterPage = "default" ' Select which master page (in the folder /Masterpages) to use
                                .PageTitle = "User Profile" ' Set a title for this page
                            End With
                    End Select

                Case Else ' Not logged in

                    Select Case .Request.Params("arg")
                        Case "forgot"
                            ' Forgot password
                            With Me
                                .MasterPage = "login" ' Select which master page (in the folder /Masterpages) to use
                                .PageTitle = "Password Retreival." ' Set a title for this page
                            End With
                        Case "newuser"
                            ' New user
                            With Me
                                .MasterPage = "login" ' Select which master page (in the folder /Masterpages) to use
                                .PageTitle = "Create your account." ' Set a title for this page
                            End With
                        Case Else
                            ' Logging in
                            With Me
                                .MasterPage = "login" ' Select which master page (in the folder /Masterpages) to use
                                .PageTitle = "Welcome. Please log in." ' Set a title for this page
                            End With
                    End Select

            End Select

        End With
    End Sub

    Protected Sub Page_Load1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        With thisSession
            Select Case .User.Identity.IsAuthenticated
                Case True ' Is logged in
                    Select Case .Request.Params("arg")
                        Case "bye"
                            ' Logging off                               
                            thisMultiView.ActiveViewIndex = 1
                            FormsAuthentication.SignOut()

                        Case Else
                            ' Edit Profile
                            Try
                                thisMultiView.ActiveViewIndex = 0
                                AddHandler btnField("Button1").Click, AddressOf hButton_Click
                                If Not Page.IsPostBack Then
                                    With thisSession.Profile
                                        txtField("txtDelAddress1") = .Item("txtDelAddress1")
                                        txtField("txtDelAddress2") = .Item("txtDelAddress2")
                                        txtField("txtDelAddress3") = .Item("txtDelAddress3")
                                        txtField("txtDelAddress4") = .Item("txtDelAddress4")
                                        txtField("txtDelPostcode") = .Item("txtDelPostcode")
                                    End With
                                End If
                            Catch
                            End Try

                    End Select

                Case Else ' Not logged in

                    Select Case .Request.Params("arg")
                        Case "forgot"
                            ' Forgot password
                            thisMultiView.ActiveViewIndex = 2
                        Case "newuser"
                            ' New user
                            thisMultiView.ActiveViewIndex = 1
                        Case Else
                            ' Logging in
                            thisMultiView.ActiveViewIndex = 0
                    End Select

            End Select

        End With

    End Sub

#End Region

#Region "Event Handlers"

    Protected Sub hButton_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        With thisSession.Profile
            .Item("txtDelAddress1") = txtField("txtDelAddress1")
            .Item("txtDelAddress2") = txtField("txtDelAddress2")
            .Item("txtDelAddress3") = txtField("txtDelAddress3")
            .Item("txtDelAddress4") = txtField("txtDelAddress4")
            .Item("txtDelPostcode") = txtField("txtDelPostcode")
        End With
    End Sub

#End Region

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

