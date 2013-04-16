Imports System.Xml
Imports System.Web
Imports System.Web.UI
Imports System.Text

Public Class cmsMenu

#Region "Private Variables"

    Private _thisContext As HttpContext
    Private _MenuItems As New List(Of cmsMenuItem)

#End Region

#Region "Initialisation and finalisation"

    Public Sub New(ByRef thisContext As HttpContext)
        _thisContext = thisContext
    End Sub

#End Region

#Region "Public Methods"

#Region "Overloaded Add Method"

    Public Overloads Sub Add()
        Dim home As XmlNode = cmsData.cat.SelectSingleNode("/*[position()=1]")
        With home
            _MenuItems.Add( _
                New cmsMenuItem( _
                       String.Format("~/{0}", .Attributes("id").Value), _
                       .Attributes("name").Value, _
                       .Attributes("img").Value, _
                       CBool(.Attributes("showonmenu").Value) _
                   ) _
               )
        End With
    End Sub

    Public Overloads Sub Add(ByVal Item As cmsMenuItem)
        _MenuItems.Add(Item)
    End Sub

    Public Overloads Sub Add(ByVal CatXpath As String)
        Try
            For Each MenuItem As XmlNode In cmsData.cat.SelectNodes(CatXpath)
                With MenuItem
                    _MenuItems.Add( _
                        New cmsMenuItem( _
                               String.Format("~/{0}", .Attributes("id").Value), _
                               .Attributes("name").Value, _
                               .Attributes("img").Value, _
                               CBool(.Attributes("showonmenu").Value) _
                           ) _
                       )
                End With
            Next
        Catch
        End Try
    End Sub

#End Region

#Region "Process Request"

    Public Sub ProcessRequest()

        With _thisContext

            Dim objX As New XmlTextWriter(.Response.OutputStream, Nothing)
            With .Response
                .Clear()
                .ContentType = "text/xml"
                .ContentEncoding = Encoding.UTF8

                With objX
                    .WriteStartDocument()

                    .WriteStartElement("cat")
                    .WriteAttributeString("id", "menu")
                    .WriteAttributeString("name", "menu")
                    .WriteAttributeString("img", "")
                    .WriteAttributeString("showonmenu", True)

                    For Each item As cmsMenuItem In _MenuItems                        
                        .WriteStartElement("cat")
                        .WriteAttributeString("id", item.Cat)
                        .WriteAttributeString("name", item.Name)
                        .WriteAttributeString("img", item.Img)
                        .WriteAttributeString("showonmenu", item.ShowOnMenu.ToString)
                        .WriteEndElement()                        
                    Next
                    .WriteEndElement()

                    .WriteEndDocument()
                    .Flush()
                    .Close()
                End With

                .End()

            End With
        End With
    End Sub

#End Region

#End Region

End Class

Public Class cmsMenuItem    

#Region "Initialisation and Finalisation"
    Public Sub New(ByVal Cat As String, ByVal Name As String, ByVal Img As String, Optional ByVal showOnMenu As Boolean = True)
        _Cat = Cat
        _Name = Name
        _Img = Img
        _ShowOnMenu = showOnMenu
    End Sub
#End Region

#Region "Public Properties"
    Private _Cat As String
    Public Property Cat() As String
        Get
            Return _Cat
        End Get
        Set(ByVal value As String)
            _Cat = value
        End Set
    End Property
    Private _Name As String
    Public Property Name() As String
        Get
            Return _Name
        End Get
        Set(ByVal value As String)
            _Name = value
        End Set
    End Property
    Private _Img As String
    Public Property Img() As String
        Get
            Return _Img
        End Get
        Set(ByVal value As String)
            _Img = value
        End Set
    End Property
    Private _ShowOnMenu As Boolean = True
    Public Property ShowOnMenu() As Boolean
        Get
            Return _ShowOnMenu
        End Get
        Set(ByVal value As Boolean)
            _ShowOnMenu = value
        End Set
    End Property
#End Region

End Class