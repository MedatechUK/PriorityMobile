Imports System.Xml
Imports System.IO

Public Class GetXMLData

#Region "private variables"

    Private Enum eRequestType
        Tabular = 1
        Scalar = 2
        NonQuery = 3
    End Enum

    Private _SQL As String
    Private _RequestType As eRequestType

#End Region

#Region "initialisation and finalisation"

    Public Sub New()

    End Sub

    Public Sub New(ByVal URL As String)
        Try
            _URL = URL
            If Not String.Compare(_URL.Split("/").Last.Split(".").Last, "ashx", True) = 0 Then
                If Not String.Compare(_URL.Last, "/") = 0 Then
                    _URL += "/"
                End If
                _URL += "getHandler.ashx"
            End If
        Catch
            Throw New Exception("Invalid URL specified.")
        End Try
    End Sub

#End Region

#Region "Public Properties"

    Private _URL As String
    Public Property URL() As String
        Get
            Return _URL
        End Get
        Set(ByVal value As String)
            Try
                _URL = value
                If Not String.Compare(_URL.Split("/").Last.Split(".").Last, "ashx", True) = 0 Then
                    If Not String.Compare(_URL.Last, "/") = 0 Then
                        _URL += "/"
                    End If
                    _URL += "getHandler.ashx"
                End If
            Catch
                Throw New Exception("Invalid URL specified.")
            End Try
        End Set
    End Property

    Private _Environment As String
    Public Property Environment() As String
        Get
            Return _Environment
        End Get
        Set(ByVal value As String)
            _Environment = value
        End Set
    End Property

#End Region

#Region "Public Methods"

    Public Function ExecuteReader(ByVal SQL As String, Optional ByVal Environment As String = Nothing) As Data.DataTable

        _Environment = Environment
        _SQL = SQL
        _RequestType = eRequestType.Tabular

        Dim ex As Exception
        ex = Nothing
        Dim response As XmlDocument = Post(ex)
        Dim result As Data.DataTable

        If Not IsNothing(ex) Then
            Throw New Exception(ex.Message)
        Else
            Try
                result = New Data.DataTable()
                For Each col As XmlNode In response.SelectNodes("//column")
                    result.Columns.Add( _
                        col.Attributes("name").InnerText, _
                        Type.GetType(col.Attributes("type").InnerText) _
                    )
                Next
                For Each row As XmlNode In response.SelectNodes("//row")
                    Dim values() As Object = Nothing
                    Dim i As Integer = 0
                    For Each col As System.Data.DataColumn In result.Columns
                        Try
                            ReDim Preserve values(UBound(values) + 1)
                        Catch
                            ReDim values(0)
                        Finally
                            values(UBound(values)) = row.Attributes(i).InnerText
                            i += 1
                        End Try
                    Next
                    result.Rows.Add(values)
                Next

            Catch exm As Exception
                Throw New Exception(exm.Message)
            End Try
            Return result
        End If
    End Function

    Public Function ExecuteScalar(ByVal SQL As String, Optional ByVal Environment As String = Nothing) As Object
        _Environment = Environment
        _SQL = SQL
        _RequestType = eRequestType.Scalar

        Dim ex As Exception
        ex = Nothing
        Dim response As XmlDocument = Post(ex)
        Dim result As Object

        If Not IsNothing(ex) Then
            Throw New Exception(ex.Message)
        Else
            Try
                result = response.SelectSingleNode("//result").InnerText

            Catch exm As Exception
                Throw New Exception(exm.Message)
            End Try
            Return result
        End If

    End Function

    Public Sub ExecuteNonQuery(ByVal SQL As String, Optional ByVal Environment As String = Nothing)
        _Environment = Environment
        _SQL = SQL
        _RequestType = eRequestType.NonQuery

        Dim ex As Exception
        ex = Nothing
        Dim response As XmlDocument = Post(ex)

        If Not IsNothing(ex) Then
            Throw New Exception(ex.Message)
        End If

    End Sub

#End Region

#Region "Post request"

    Private Function Post(ByRef Ex As Exception) As XmlDocument

        Dim uploadResponse As Net.HttpWebResponse
        Dim requestStream As Stream
        Dim posted As Boolean = False
        Ex = Nothing

        Try

            requestStream = Nothing
            uploadResponse = Nothing

            Dim ms As MemoryStream = New MemoryStream(toByte)
            Dim uploadRequest As Net.HttpWebRequest = CType(Net.HttpWebRequest.Create(_URL), Net.HttpWebRequest)

            uploadRequest.Method = "POST"
            uploadRequest.Proxy = Nothing
            uploadRequest.SendChunked = True
            requestStream = uploadRequest.GetRequestStream()

            ' Upload the XML
            Dim buffer(1024) As Byte
            Dim bytesRead As Integer
            While True
                bytesRead = ms.Read(buffer, 0, buffer.Length)
                If bytesRead = 0 Then
                    Exit While
                End If
                requestStream.Write(buffer, 0, bytesRead)
            End While

            ' The request stream must be closed before getting the response.
            requestStream.Close()
            uploadResponse = uploadRequest.GetResponse()

            Dim thisRequest As New XmlDocument
            Dim reader As New StreamReader(uploadResponse.GetResponseStream)
            thisRequest.LoadXml(reader.ReadToEnd)

            Dim status As XmlNode = thisRequest.SelectSingleNode("response/status")
            Select Case CInt(status.Attributes("code").Value)
                Case 200
                    Return thisRequest
                Case 400
                    Throw New Exception(String.Format("Request error: {0}", status.Attributes("message").Value))
                Case 500
                    Throw New Exception(String.Format("Response error: {0}", status.Attributes("message").Value))
            End Select

        Catch exep As UriFormatException
            Ex = New Exception(String.Format("Invalid URL: {0}", exep.Message))
        Catch exep As Net.WebException
            Ex = New Exception(String.Format("Connection Error: {0}", exep.Message))
        Catch exep As Exception
            Ex = New Exception(String.Format("Request failed: {0}", exep.Message))
        Finally
            ' Clean up the streams
            If Not IsNothing(uploadResponse) Then
                uploadResponse.Close()
            End If
            If Not IsNothing(requestStream) Then
                requestStream.Close()
            End If
        End Try

        Return Nothing

    End Function

    Private ReadOnly Property toByte() As Byte()
        Get
            Dim myEncoder As New System.Text.ASCIIEncoding
            Dim str As New System.Text.StringBuilder
            Dim xw As XmlWriter = XmlWriter.Create(str)
            With xw

                .WriteStartDocument()
                .WriteStartElement("GetRequest")

                If Not IsNothing(_Environment) Then
                    .WriteElementString("Environment", _Environment)
                End If

                Select Case _RequestType
                    Case eRequestType.Tabular
                        .WriteElementString("RequestType", "tabular")
                    Case eRequestType.Scalar
                        .WriteElementString("RequestType", "scalar")
                    Case eRequestType.NonQuery
                        .WriteElementString("RequestType", "nonquery")
                    Case Else

                End Select

                .WriteElementString("SQL", _SQL)

                .WriteEndDocument()
                .Flush()

            End With

            Return myEncoder.GetBytes(str.ToString)
        End Get
    End Property

#End Region

End Class
