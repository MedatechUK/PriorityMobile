Imports System.Web

Public Class cmsHTTPPost

    Private _FormID As String
    Private ReadOnly Property Formid() As String
        Get
            Return _FormID
        End Get
    End Property

    Private _URL As String
    Public Sub New(ByVal URL As String, Optional ByVal FormID As String = "httpPost")
        _FormID = FormID
        _URL = URL
    End Sub

    Private _NameValues As New Dictionary(Of String, String)
    Public Property NameValues() As Dictionary(Of String, String)
        Get
            Return _NameValues
        End Get
        Set(ByVal value As Dictionary(Of String, String))
            _NameValues = value
        End Set
    End Property

    Public Sub Post()

        Dim htmlform As New Text.StringBuilder
        With htmlform
            .AppendLine("<html>")
            .AppendFormat("<body onload='document.forms[{0}{1}{0}].submit()'>", Chr(34), Formid).AppendLine() '
            .AppendFormat("<form id='{0}' method='POST' action='{1}'>", Formid, _URL).AppendLine()
            For Each k As String In _NameValues.Keys
                .AppendFormat("<input type='hidden' name='{0}' value='{1}' />", k, _NameValues(k)).AppendLine()
            Next
            .AppendLine("</form>")
            .AppendLine("</body>")
            .AppendLine("</html>")
        End With

        With HttpContext.Current.Response
            .Clear()
            .Write(htmlform.ToString())
            .End()
        End With

    End Sub

End Class
