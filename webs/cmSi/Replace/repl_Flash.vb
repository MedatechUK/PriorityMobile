Imports System.Xml
Imports System.IO
Imports System.Web.UI.WebControls

Public Class repl_Flash : Inherits repl_Base

    Private lit As Literal

#Region "Control Definitions"

    Public Overrides ReadOnly Property ReplaceModule() As String
        Get
            Return "repl_Flash"
        End Get
    End Property

    Public Overrides ReadOnly Property Controls() As List(Of ReplaceControl)
        Get
            Dim ret As New List(Of ReplaceControl)
            With ret
                .Add(New ReplaceControl("System.Web.UI.WebControls.Literal", "flash", AddressOf hFlash))
            End With
            Return ret
        End Get
    End Property

#End Region

#Region "Delegate methods"

    Public Sub hFlash(ByVal sender As Object, ByVal e As repl_Argument)
        Dim swf As String = e.thisCMSPage.PageNode.Attributes("id").Value & ".swf"
        Dim fn As String = e.thisServer.MapPath("swf/") & swf
        If Not File.Exists(fn) Then swf = "header-sub.swf"
        lit.Text = String.Format("<script type={0}text/javascript{0}>{1}	 var so = new SWFObject({0}swf/{2}{0}, {0}flash-header-sub{0}, {0}946{0}, {0}210{0}, {0}8{0}, {0}#ffffff{0});{1}		 so.addParam({0}scale{0}, {0}showall{0});{1}		 so.addParam({0}wmode{0}, {0}opaque{0});{1}		 so.write({0}flash-sub{0});{1}</script>", Chr(34), vbCrLf, swf)
    End Sub

#End Region

End Class
