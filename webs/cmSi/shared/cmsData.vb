Imports System.Web
Imports System.IO
Imports System.Xml

Public Class cmsData

    Public Shared rootpath As String
    Public Shared doc As New XmlDocument
    Public Shared cat As New XmlDocument
    Public Shared part As New XmlDocument
    Public Shared CatPath As String
    Public Shared DocPath As String
    Public Shared Settings As System.Collections.Specialized.NameValueCollection
    Public Shared rtSettings As New XmlDocument
    Public Shared rtSettingsPath As String
    Public Shared offersPath As String
    Public Shared offers As New XmlDocument

    Public Shared Function URLify(ByVal title As String) As String
        Dim ret As String = ""
        ret = RTrim(title)
        ret = LTrim(ret)
        ret = ret.ToLower
        ret = ret.Replace(" ", "-")
        ret = HttpContext.Current.Server.UrlEncode(ret)
        Return ret
    End Function

    Public Shared Function generateID(ByVal title As String) As String
        Dim id As String = URLify(title)
        Dim suffix As Integer = 1
        Dim tmpID As String = id
        While True
            If cat.SelectSingleNode(String.Format("//cat[@id={0}{1}{0}]", Chr(34), tmpID)) Is Nothing Then
                Return tmpID
            Else
                tmpID = String.Format("{0}_{1}", id, suffix)
                suffix += 1
            End If
        End While
    End Function

    Public Sub Load(ByRef thisServer As HttpServerUtility, ByRef AppSettings As System.Collections.Specialized.NameValueCollection)

        With thisServer

            Settings = AppSettings
            rootpath = .MapPath("")

            rtSettingsPath = .MapPath("settings.xml")
            Dim rtSettingsLoaded As Boolean = False
            While Not rtSettingsLoaded
                Try
                    Using sr As New StreamReader(rtSettingsPath)
                        rtSettings.LoadXml(sr.ReadToEnd)
                    End Using
                    rtSettingsLoaded = True
                Catch ex As Exception
                    Threading.Thread.Sleep(1000)
                End Try
            End While

            offersPath = .MapPath("offers.xml")
            Dim offersLoaded As Boolean = False
            While Not offersLoaded
                Try
                    Using sr As New StreamReader(offersPath)
                        offers.LoadXml(sr.ReadToEnd)
                    End Using
                    offersLoaded = True
                Catch ex As Exception
                    Threading.Thread.Sleep(1000)
                End Try
            End While

            CatPath = .MapPath("cat.xml")
            Dim CatLoaded As Boolean = False
            While Not CatLoaded
                Try

                    Using sr As New StreamReader(CatPath)
                        cat.LoadXml(sr.ReadToEnd)
                    End Using
                    CatLoaded = True
                Catch ex As Exception
                    ' Weirdness - give it a second
                    Threading.Thread.Sleep(1000)
                End Try
            End While

            DocPath = .MapPath("pages.xml")
            Dim PagesLoaded As Boolean = False
            While Not PagesLoaded
                Try
                    Using sr As New StreamReader(DocPath)
                        doc.LoadXml(sr.ReadToEnd)
                    End Using
                    PagesLoaded = True
                Catch ex As Exception
                    ' Weirdness - give it a second
                    Threading.Thread.Sleep(1000)
                End Try
            End While

            Dim PartsLoaded As Boolean = False
            While Not PartsLoaded
                Try
                    Using reader As XmlTextReader = New XmlTextReader(Settings.Get("PartFeedURL").ToString)
                        part.Load(reader)
                    End Using
                    PartsLoaded = True
                Catch
                    ' Network failiure, retry in 2 seconds
                    Threading.Thread.Sleep(2000)
                End Try
            End While

        End With

    End Sub

End Class
