Imports System
Imports System.Text
Imports System.IO
Imports System.Net
Imports System.Security.Cryptography

Namespace apitest
#Region "The test code"
    Class Program
        Shared Sub Main(ByVal args() As String)
            Dim msg As String = "<request><atomic>false</atomic><testing>false</testing><getquote><settlementdate>2010-08-19</settlementdate><buycurr>EUR</buycurr><sellcurr>GBP</sellcurr><side>B</side><amount>10000</amount></getquote></request>"

            Console.WriteLine(msg)

            Dim wfApi As WorldFirstApi = New WorldFirstApi()

            Dim ret As String = wfApi.doRequest(msg)

            Console.WriteLine(ret)
            Console.ReadLine()
        End Sub
    End Class
#End Region
#Region "The Application Program interface"
    Class WorldFirstApi
        Protected m_enc As Encoding = Encoding.UTF8
        Protected m_url As String = "https://trading.worldfirst.com/api/demo/"
        Protected m_user As String = "demo"
        Protected m_pass As String = "d3m0u5r"
        Protected m_key As String = "abcdefghihjklmnopqrstuvwxyz0123456789"

        Public Function doRequest(ByVal message As String) As String
            Dim hash As String = getHash(message, m_key)
            Console.WriteLine("HASH: " + hash)
            Dim url As String = m_url + "?hash=" + hash

            Return post(url, message)
        End Function 'doRequest

        Protected Function parseHash(ByVal hash As Byte()) As Byte()
            Dim ret As String = ""
            For Each a As Byte In hash
                ret += a.ToString("x2")
            Next
            Return m_enc.GetBytes(ret)

        End Function 'parseHash

        Protected Function getHash(ByVal message As String, ByVal key As String) As String
            Dim bKey() As Byte = m_enc.GetBytes(key)
            Dim bMessage() As Byte = m_enc.GetBytes(message)

            Dim hmacmd5 As HMACMD5 = New HMACMD5(bKey)

            Dim md5 As MD5CryptoServiceProvider = New MD5CryptoServiceProvider()
            Dim md5Hash() As Byte = parseHash(md5.ComputeHash(bMessage))

            Dim smd5Hash As String = m_enc.GetString(md5Hash)
            Console.WriteLine("MD5: " + smd5Hash)

            Dim bMsgHash() As Byte = parseHash(hmacmd5.ComputeHash(md5Hash))

            Return m_enc.GetString(bMsgHash)
        End Function 'getHash

        Protected Function post(ByVal uri As String, ByVal postData As String) As String
            Dim webRequest As WebRequest = Net.WebRequest.Create(uri)

            webRequest.Credentials = New NetworkCredential(m_user, m_pass)
            webRequest.ContentType = "application/x-www-form-urlencoded"
            webRequest.Method = "POST"

            Dim bytes() As Byte = m_enc.GetBytes(postData)
            Dim os As Stream = Nothing
            Try
                webRequest.ContentLength = bytes.Length
                os = webRequest.GetRequestStream()
                os.Write(bytes, 0, bytes.Length)
            Catch ex As WebException
                Console.WriteLine("HttpPost: Request error - " + ex.Message)
            Finally
                If Not os Is Nothing Then
                    os.Close()
                End If
            End Try

            Try
                Dim webResponse As WebResponse = webRequest.GetResponse()
                If webResponse Is Nothing Then
                    Return Nothing
                End If
                Dim sr As StreamReader = New StreamReader(webResponse.GetResponseStream())
                Return sr.ReadToEnd().Trim()
            Catch ex As WebException
                Console.WriteLine("HttpPost: Response error - " + ex.Message)
            End Try
            Return Nothing
        End Function 'post
    End Class
#End Region
End Namespace