Imports System.Xml
Imports System.Web.UI.WebControls

Public Class repl_Blog : Inherits repl_Base

    Private lit As Literal

#Region "Control Definitions"

    Public Overrides ReadOnly Property ReplaceModule() As String
        Get
            Return "repl_Blog"
        End Get
    End Property

    Public Overrides ReadOnly Property Controls() As List(Of ReplaceControl)
        Get
            Dim ret As New List(Of ReplaceControl)
            With ret
                .Add(New ReplaceControl("System.Web.UI.WebControls.Literal", "FirstChildHtml", AddressOf hFirstChildHTML))
                .Add(New ReplaceControl("System.Web.UI.WebControls.Literal", "FirstChildTitle", AddressOf hFirstChildTitle))
                .Add(New ReplaceControl("System.Web.UI.WebControls.Literal", "SecondChildLink", AddressOf hSecondChildLink))
                .Add(New ReplaceControl("System.Web.UI.WebControls.Literal", "NextSibling", AddressOf hNextSibling))
                .Add(New ReplaceControl("System.Web.UI.WebControls.Literal", "LastSibling", AddressOf hLastSibling))
            End With
            Return ret
        End Get
    End Property

#End Region

#Region "Delegate methods"

    Public Sub hFirstChildHTML(ByVal sender As Object, ByVal e As repl_Argument)
        lit = sender
        If Not IsNothing(e.thisCMSPage.thisCat.FirstChild) Then
            Dim fchp As XmlNode = cmsData.doc.SelectSingleNode(String.Format("//page[@id='{0}']", e.thisCMSPage.thisCat.FirstChild.Attributes("id").Value))
            If Not IsNothing(fchp) Then
                Dim sec As XmlNode = fchp.SelectSingleNode(String.Format("section[@placeholder={0}main{0}]", Chr(34)))
                If Not IsNothing(sec) Then
                    lit.Text = sec.Attributes("html").Value
                End If
            End If
        End If
    End Sub

    Public Sub hFirstChildTitle(ByVal sender As Object, ByVal e As repl_Argument)
        lit = sender
        If Not IsNothing(e.thisCMSPage.thisCat.FirstChild) Then
            Dim fchp As XmlNode = cmsData.doc.SelectSingleNode(String.Format("//page[@id='{0}']", e.thisCMSPage.thisCat.FirstChild.Attributes("id").Value))
            If Not IsNothing(fchp) Then
                lit.Text = fchp.Attributes("title").Value
            End If
        End If
    End Sub

    Public Sub hSecondChildLink(ByVal sender As Object, ByVal e As repl_Argument)
        lit = sender
        If Not IsNothing(e.thisCMSPage.thisCat.FirstChild.NextSibling) Then
            Dim fchp As XmlNode = cmsData.doc.SelectSingleNode(String.Format("//page[@id='{0}']", e.thisCMSPage.thisCat.FirstChild.NextSibling.Attributes("id").Value))
            If Not IsNothing(fchp) Then
                lit.Text = "<a href='" & fchp.Attributes("id").Value & "'>Next item &gt;&gt;</a>"
            End If
        End If
    End Sub

    Public Sub hNextSibling(ByVal sender As Object, ByVal e As repl_Argument)
        lit = sender
        If Not IsNothing(e.thisCMSPage.thisCat.NextSibling) Then
            Dim fchp As XmlNode = cmsData.doc.SelectSingleNode(String.Format("//page[@id='{0}']", e.thisCMSPage.thisCat.NextSibling.Attributes("id").Value))
            If Not IsNothing(fchp) Then
                lit.Text = "<a href='" & fchp.Attributes("id").Value & "'>Next item &gt;&gt;</a>"
            End If
        End If
    End Sub

    Public Sub hLastSibling(ByVal sender As Object, ByVal e As repl_Argument)
        lit = sender
        If Not IsNothing(e.thisCMSPage.thisCat.PreviousSibling) Then
            If e.thisCMSPage.thisCat.Attributes("id").Value = e.thisCMSPage.thisCat.ParentNode.FirstChild.Attributes("id").Value Then
                lit.Text = "<a href='" & e.thisCMSPage.thisCat.ParentNode.Attributes("id").Value & "'>&lt;&lt; Last item</a>"
            Else
                lit.Text = "<a href='" & e.thisCMSPage.thisCat.PreviousSibling.Attributes("id").Value & "'>&lt;&lt; Last item</a>"
            End If
        End If
    End Sub

#End Region

End Class
