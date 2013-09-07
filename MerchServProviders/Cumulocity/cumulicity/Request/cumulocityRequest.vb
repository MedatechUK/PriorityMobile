Imports System
Imports System.IO
Imports System.Net
Imports System.Text

Public MustInherit Class cumulocityRequest

#Region "Private variables"

    Private thisRequest As HttpWebRequest = Nothing
    Private thisResponse As HttpWebResponse = Nothing

    Private ReadOnly Property MyParams() As String
        Get
            Dim str As New System.Text.StringBuilder
            If Parameters.Count > 0 Then
                str.Append("?")
            End If
            For Each name As String In Parameters.Keys
                str.AppendFormat("{0}={1}&", name, Parameters(name))
            Next
            Return str.ToString.Substring(0, str.ToString.Length - 2)
        End Get
    End Property

#End Region

#Region "Must Override"

    Public MustOverride Function Result(ByRef excep As Exception) As CumulocityResponse

#End Region

#Region "Public Properties"

    Private _URL As String
    Public Property URL() As String
        Get
            Return _URL
        End Get
        Set(ByVal value As String)
            _URL = value
        End Set
    End Property

    Private _Tennant As cumulocityCredentials
    Public Property Tennant() As cumulocityCredentials
        Get
            Return _Tennant
        End Get
        Set(ByVal value As cumulocityCredentials)
            _Tennant = value
        End Set
    End Property

    Private _Parameters As New Dictionary(Of String, String)
    Public Property Parameters() As Dictionary(Of String, String)
        Get
            Return _Parameters
        End Get
        Set(ByVal value As Dictionary(Of String, String))
            _Parameters = value
        End Set
    End Property

#End Region

#Region "Initialisation and finalisation"

    Sub New(ByRef Tennant As cumulocityCredentials, ByVal URL As String)

        _URL = URL
        _Tennant = Tennant

    End Sub

#End Region

#Region "Public methods"

    Friend Function Response(ByRef excep As Exception) As System.IO.StreamReader
        excep = Nothing
        Try
            thisRequest = DirectCast(WebRequest.Create(String.Format("{0}{1}{2}", Tennant.URL, URL, MyParams)), HttpWebRequest)
            With thisRequest
                .Accept = "application/json"
                .Headers.Add("X-Cumulocity-Application-Key", "vendme")
                .Headers(HttpRequestHeader.Authorization) = Tennant.AuthHeader
                thisResponse = DirectCast(.GetResponse(), HttpWebResponse)
                ' Get the response stream into a reader  
                Return New StreamReader(thisResponse.GetResponseStream())
            End With
        Catch ex As Exception
            excep = ex
            Return Nothing
        End Try
    End Function

#End Region

End Class
