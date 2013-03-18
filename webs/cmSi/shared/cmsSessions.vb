Imports Microsoft.VisualBasic
Imports System.Collections.Generic
Imports System.Xml
Imports System.IO
Imports System.Web
Imports System.Web.UI.WebControls

Public Class cmsSessions

#Region "Public Shared variables"

    Public Shared Sessions As Dictionary(Of String, Session)

#End Region

#Region "Public Shared Methods"

    Public Shared Function CurrentSession(ByVal CurrentContext As HttpContext) As Session
        If IsNothing(Sessions) Then
            Sessions = New Dictionary(Of String, Session)
        End If
        If Not Sessions.ContainsKey(CurrentContext.Session.SessionID) Then
            Sessions.Add(CurrentContext.Session.SessionID, New Session(CurrentContext))
        Else
            Sessions.Item(CurrentContext.Session.SessionID).Update(CurrentContext)
        End If
        Return Sessions.Item(CurrentContext.Session.SessionID)
    End Function

    Public Shared Sub CloseSession(ByVal SessionID As String)
        If Not IsNothing(Sessions) Then
            Sessions.Remove(SessionID)
        End If
    End Sub

#End Region

End Class
