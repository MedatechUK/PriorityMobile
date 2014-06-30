Imports System.Web.UI.WebControls
Imports System.Xml
Imports System.Web

Friend Class Repl_Search
    Inherits repl_Base

#Region "Control Definitions"

    Public Overrides ReadOnly Property ReplaceModule() As String
        Get
            Return "Repl_Search"
        End Get
    End Property

    Public Overrides ReadOnly Property Controls() As System.Collections.Generic.List(Of ReplaceControl)
        Get
            Dim ret As New List(Of ReplaceControl)
            With ret
                .Add(New ReplaceControl("System.Web.UI.WebControls.Button", "SearchButton", AddressOf hSearchButton))
                .Add(New ReplaceControl("System.Web.UI.WebControls.ListView", "SearchResults", AddressOf hSearchResults))
                .Add(New ReplaceControl("System.Web.UI.WebControls.TextBox", "SearchTerms", AddressOf hSearchTerms))
            End With
            Return ret
        End Get
    End Property

#End Region

#Region "Delegate methods"

    Public Sub hSearchTerms(ByVal sender As Object, ByVal e As repl_Argument)
        Dim SearchTerms As System.Web.UI.WebControls.TextBox = sender
        If Not SearchTerms.Page.IsPostBack Then
            With HttpContext.Current
                If Not IsNothing(.Request("s")) Then
                    SearchTerms.Text = .Request("s")
                End If
            End With
        End If
    End Sub

    Public Sub hSearchButton(ByVal sender As Object, ByVal e As repl_Argument)
        Dim btn As Button = sender
        AddHandler btn.Click, AddressOf SearchButton_Click
    End Sub

    Public Sub hSearchResults(ByVal sender As Object, ByVal e As repl_Argument)
        Dim dl As System.Web.UI.WebControls.DataBoundControl = sender

        Dim Found As New Dictionary(Of String, sResult)
        Dim SearchTerm As String = HttpContext.Current.Request("s").ToLower

        Dim ds As New System.Web.UI.WebControls.XmlDataSource
        With ds
            .ID = "SearchDataSource"
            .EnableCaching = False
        End With

        SearchPageNode(Found, SearchTerm, "html")
        SearchPageNode(Found, SearchTerm, "title")
        SearchPageNode(Found, SearchTerm, "description")
        SearchPageNode(Found, SearchTerm, "keywords")

        SearchPartNode(Found, SearchTerm, "PARTNAME")
        SearchPartNode(Found, SearchTerm, "PARTDES")
        SearchPartNode(Found, SearchTerm, "PARTREMARK")
        SearchPartNode(Found, SearchTerm, "VALUE")

        Dim scores As New List(Of Integer)
        For Each Result As sResult In Found.Values
            If Not scores.Contains(Result.Score) Then
                scores.Add(Result.Score)
            End If
        Next

        Dim str As New Text.StringBuilder
        With str
            .AppendLine("<results>")

            Do
                For Each Result As sResult In Found.Values
                    If Result.Score = scores.Max Then

                        .Append("   <result ")
                        .AppendFormat("pagetitle='{0}' ", Result.PageTitle)
                        .AppendFormat("loc='{0}' ", Result.Loc)
                        .AppendFormat("description='{0}' ", Result.Description)
                        .AppendFormat("img='{0}' ", Result.Image.Replace("~", ""))
                        .AppendFormat("score='{0}' ", Result.Score)
                        .Append("/>")
                        .AppendLine()

                    End If
                Next
                If Found.Count > 0 Then
                    scores.Remove(scores.Max)
                Else

                End If
            Loop While scores.Count > 0

            .AppendLine("</results>")

            ds.Data = .ToString



        End With
        dl.Page.Controls.Add(ds)

        Try
            With dl
                .DataSourceID = ds.ID
                .DataBind()
            End With

        Catch EX As Exception
            Throw New Exception( _
                String.Format( _
                    "Binding to XMLDataSource '{0}' failed. {1}", _
                    ds.ID, _
                    EX.Message _
                ) _
            )
        End Try
    End Sub

#End Region

#Region "Event Handlers"

    Public Sub SearchButton_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim btn As System.Web.UI.WebControls.Button = sender
        With btn.Page
            Dim searchTextBox As TextBox = .Master.FindControl("SearchTerms")
            If Not IsNothing(searchTextBox) Then
                .Response.Redirect(String.Format("search.aspx?s={0}", searchTextBox.Text))
            End If
        End With
    End Sub

#End Region

#Region "Private Methods"

    Private Function isVisible(ByVal pageID As String) As Boolean
        'TODO: SORT THIS ARGGHGHH
        Try

            If CBool(cmsData.cat.SelectSingleNode( _
                    String.Format("//*[@id={0}{1}{0}]", _
                    Chr(34), _
                    pageID)).Attributes("showonmenu").Value) Then
                Return True
            Else
                Return False
            End If

        Catch ex As NullReferenceException
            Return False
        End Try
    End Function

    Private Sub SearchPageNode(ByRef Found As Dictionary(Of String, sResult), ByVal SearchTerm As String, ByVal NodeName As String)

        For Each n As XmlNode In cmsData.doc.SelectNodes( _
            String.Format( _
                "//section[contains(translate(@{0}, 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'), '{1}')]", _
                NodeName, _
                SearchTerm _
            ) _
        )

            Dim id As String = n.ParentNode.Attributes("id").Value

            If Not Found.Keys.Contains(id) Then
                If isVisible(id) Then
                    Found.Add( _
                        id, _
                        New sResult(n.ParentNode) _
                    )
                End If
            Else
                Found(id).IncrementScore()
            End If



        Next
    End Sub

    Private Sub SearchPartNode(ByRef Found As Dictionary(Of String, sResult), ByVal SearchTerm As String, ByVal NodeName As String)
        For Each n As XmlNode In cmsData.part.SelectNodes( _
            String.Format( _
                "//PART[contains(translate({0}, 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'), '{1}')]", _
                NodeName, _
                SearchTerm _
            ) _
        )

            For Each p As XmlNode In cmsData.doc.SelectNodes(String.Format("//page[@part='{0}']", n.SelectSingleNode("PARTNAME").InnerText))
                Dim id As String = p.Attributes("id").Value
                If Not Found.Keys.Contains(id) Then
                    If isVisible(id) Then
                        With p
                            Found.Add(id, _
                                New sResult( _
                                    cmsCleanHTML.htmlEncode(.Attributes("title").Value), _
                                    cmsCleanHTML.htmlEncode(.Attributes("description").Value), _
                                    id, _
                                    String.Format("priImage.aspx?image={0}", n.SelectSingleNode("PRIIMG").InnerText) _
                                ) _
                            )
                        End With
                    End If
                Else
                    Found(id).IncrementScore()
                    Found(id).Image = String.Format("~/priImage.aspx?image={0}", n.SelectSingleNode("PRIIMG").InnerText)
                End If
            Next

        Next
    End Sub

#End Region

#Region "Result Class"

    Private Class sResult

        Private _pagetitle As String
        Public ReadOnly Property PageTitle() As String
            Get
                Return _pagetitle
            End Get
        End Property

        Private _description As String
        Public ReadOnly Property Description() As String
            Get
                Return _description
            End Get
        End Property

        Private _loc As String
        Public ReadOnly Property Loc() As String
            Get
                Return _loc
            End Get
        End Property

        Private _Image As String = String.Empty
        Public Property Image() As String
            Get
                If _Image.Length > 0 Then
                    Return _Image
                Else
                    Return "images/noimage.jpg"
                End If
            End Get
            Set(ByVal value As String)
                _Image = value
            End Set
        End Property

        Private _Score As Integer = 1
        Public ReadOnly Property Score() As Integer
            Get
                Return _Score
            End Get
        End Property

        Public Sub New(ByVal PageTitle As String, ByVal Description As String, ByVal Loc As String, ByVal Image As String)
            _pagetitle = PageTitle
            _description = Description
            _loc = Loc
            _Image = Image
        End Sub

        Public Sub New(ByVal pageNode As XmlNode)
            With pageNode
                _pagetitle = cmsCleanHTML.htmlEncode(.Attributes("title").Value)
                _description = cmsCleanHTML.htmlEncode(.Attributes("description").Value)
                _loc = .Attributes("id").Value
                If Not IsNothing(.Attributes("img")) Then _Image = .Attributes("img").Value
            End With
        End Sub

        Public Sub IncrementScore()
            _Score += 1
        End Sub

    End Class

#End Region

End Class

