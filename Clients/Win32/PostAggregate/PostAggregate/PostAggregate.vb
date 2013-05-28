Imports System.Xml
Imports System.Net
Imports System.Web
Imports Spring
Imports Spring.Social.Twitter


Module PostAggregate

    Sub Main()

        extractNewsItemsFromEmergeSite()
        extractBlogItemsfromEmergeBlog()
        Console.ReadLine()
    End Sub


    Function formulateTweet(ByRef tweet As String, ByVal bitly As String) As String
        If tweet.Length > 140 Then
            tweet = tweet.Remove(140 - bitly.Length - 4, tweet.Length + bitly.Length - 140 + 4)
        End If

        tweet += "... " & bitly

        Return tweet
    End Function

    Sub extractBlogItemsfromEmergeBlog()
        Dim blog As New XmlDocument()
        blog.Load("http://blog.emerge-it.co.uk/sitemap.axd")
        Dim xmlnsmgr As New XmlNamespaceManager(blog.NameTable)
        xmlnsmgr.AddNamespace("ns", "http://www.google.com/schemas/sitemap/0.84")
        Dim switch As String = "No new blog items found. No tweets were posted."

        Dim posts As XmlNodeList = blog.SelectNodes("//ns:loc", xmlnsmgr)

        For Each post As XmlNode In posts
            Dim bitly As String = generateABitlyURL(HttpUtility.UrlEncode(post.InnerText))

            If Not bitly = "False" Then
                switch = ""
                Dim tweet As String = post.InnerText.Split("/").Last.Split(".").First.Replace("-", " ")
                formulateTweet(tweet, bitly)

                Console.WriteLine("[TWEETING]:" & tweet)
                postTweet(tweet)
            End If
        Next

        If Not IsNothing(switch) Then
            Console.WriteLine(switch)
        End If
    End Sub

    Sub extractNewsItemsFromEmergeSite()
        Dim pages As New XmlDocument
        pages.Load("http://www.emerge-it.co.uk/pages.xml")
        Dim news As XmlNodeList = pages.SelectNodes("//page[@masterpage=""emerge.newsitem.master""]")
        Dim url As String = "http://www.emerge-it.co.uk"
        Dim switch As String = "No new news items found. No tweets were posted."

        For Each post As XmlNode In news
            Dim bitly As String = generateABitlyURL(HttpUtility.UrlEncode(url & "/" & post.SelectSingleNode("@id").InnerText))
            If Not bitly = "False" Then
                switch = ""
                Dim tweet As String = post.SelectSingleNode("@title").InnerText
                formulateTweet(tweet, bitly)

                Console.WriteLine("[TWEETING]:" & tweet)
                postTweet(tweet)
            End If
        Next

        If Not IsNothing(switch) Then
            Console.WriteLine(switch)
        End If
    End Sub

    Sub postTweet(ByVal tweet As String) 'change twitter details to live account
        Dim twitter As Api.ITwitter = New Api.Impl.TwitterTemplate("iHhlxp5je97oAIZCpGZoA", _
                                                                        "tcMMA50Ul4xMsKoAqxgE40pS7cShfy1POCenUbuDmQ", _
                                                                        "1454757097-TgbCbysfhF8vQrtPQIgfq2SfUPiu7BFAKEBNTHf", _
                                                                        "nIEALgCthQVC47kwfJe9IuKRMaPA6U9ib6HUvYOg")

        twitter.TimelineOperations.UpdateStatus(tweet)
    End Sub

    Function generateABitlyURL(ByVal longurl) As String
        Dim wc As New WebClient
        Dim login As String = "ttrmw"
        Dim key As String = "R_5e47b59cb740c22fab36ff1414b622e8"
        Dim request As String = String.Format("http://api.bit.ly/v3/shorten?login={0}&apiKey={1}&longUrl={2}&format=xml", login, key, longurl)
        Dim xmlResult As New XmlDocument
        xmlResult.Load(request)
        Dim isNew As Boolean = CBool(xmlResult.SelectSingleNode("//new_hash").InnerText)
        If isNew Then
            Return xmlResult.SelectSingleNode("//url").InnerText
        Else
            Return "False"
        End If
    End Function
End Module
