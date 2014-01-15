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
