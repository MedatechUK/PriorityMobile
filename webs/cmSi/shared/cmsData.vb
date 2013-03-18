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

    Public Sub Load(ByRef thisServer As HttpServerUtility, ByRef AppSettings As System.Collections.Specialized.NameValueCollection)

        With thisServer

            Settings = AppSettings
            rootpath = .MapPath("")

            CatPath = .MapPath("cat.xml")
            Using sr As New StreamReader(CatPath)
                cat.LoadXml(sr.ReadToEnd)
            End Using

            DocPath = .MapPath("pages.xml")
            Using sr As New StreamReader(DocPath)
                doc.LoadXml(sr.ReadToEnd)
            End Using

            Try
                Using reader As XmlTextReader = New XmlTextReader(Settings.Get("PartFeedURL").ToString)
                    part.Load(reader)
                End Using
            Catch
            End Try

        End With

    End Sub

End Class
