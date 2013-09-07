Public Class cumulocityCredentials

#Region "Initialisation and finalisation"

    Public Sub New(ByVal Server As String, ByVal Tennant As String, ByVal UserName As String, ByVal Password As String)
        _URL = Server
        _Tennant = Tennant
        _UserName = UserName
        _Password = Password
    End Sub

    Public Sub New()

    End Sub

#End Region

#Region "Public Properties"

    Private _URL As String
    Public Property URL() As String
        Get
            Return _URL
        End Get
        Set(ByVal value As String)
            _URL = value
            If Not _URL.Substring(_URL.Length - 2, 1) = "/" Then
                _URL = _URL + "/"
            End If
        End Set
    End Property

    Private _Tennant As String = String.Empty
    Public Property Tennant() As String
        Get
            Return _Tennant
        End Get
        Set(ByVal value As String)
            _Tennant = value
        End Set
    End Property

    Private _UserName As String
    Public Property UserName() As String
        Get
            Return _UserName
        End Get
        Set(ByVal value As String)
            _UserName = value
        End Set
    End Property

    Private _Password As String
    Public Property Password() As String
        Get
            Return _Password
        End Get
        Set(ByVal value As String)
            _Password = value
        End Set
    End Property

    Public ReadOnly Property AuthHeader() As String            
        Get
            Dim encoding As New System.Text.UTF8Encoding()
            Return String.Format( _
                "Basic {0}", Convert.ToBase64String( _
                    encoding.GetBytes( _
                        String.Format("{0}/{1}:{2}", Tennant, UserName, Password) _
                    ) _
                ) _
            )
        End Get
    End Property

#End Region

End Class