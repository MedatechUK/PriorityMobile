Imports System.Web
Imports System.Web.UI
Imports System.Xml

Public Class cmsReplace

    Private _ReplaceTypes As New Dictionary(Of String, ControlList)
    Private _thispage As cmsPage

    Public Sub New(ByVal ParamArray Repl() As repl_Base)

        Add( _
            New repl_Basic, _
            New Repl_Search _
        )

        If Not IsNothing(Repl) Then
            For Each r As repl_Base In Repl
                For Each ctrl As ReplaceControl In r.Controls
                    With ctrl
                        If Not _ReplaceTypes.Keys.Contains(.ControlType) Then
                            _ReplaceTypes.Add(.ControlType, New ControlList)
                        End If
                        If Not _ReplaceTypes(.ControlType).Controls.Keys.Contains(.ControlID) Then
                            _ReplaceTypes(.ControlType).Controls.Add(.ControlID, New rControl(r.ReplaceModule, .ControlHandler))
                        End If
                    End With
                Next
            Next
        End If

    End Sub

    Public Sub Add(ByVal ParamArray Repl() As repl_Base)
        For Each r As repl_Base In Repl
            For Each ctrl As ReplaceControl In r.Controls
                With ctrl
                    If Not _ReplaceTypes.Keys.Contains(.ControlType) Then
                        _ReplaceTypes.Add(.ControlType, New ControlList)
                    End If
                    If Not _ReplaceTypes(.ControlType).Controls.Keys.Contains(.ControlID) Then
                        _ReplaceTypes(.ControlType).Controls.Add(.ControlID, New rControl(r.ReplaceModule, .ControlHandler))
                    End If
                End With
            Next
        Next
    End Sub

    Public Sub LoadControls(ByRef thisCMSPage As cmsPage, ByRef thisPage As Page, ByRef thisContext As HttpContext, ByRef thisServer As HttpServerUtility)
        For Each c As Control In thisPage.Master.Controls
            recurseLoadControls(c, thisCMSPage, thisPage, thisContext, thisServer)
        Next
    End Sub

    Private Sub recurseLoadControls(ByRef c As Control, ByRef thisCMSPage As cmsPage, ByRef thisPage As Page, ByRef thisContext As HttpContext, ByRef thisServer As HttpServerUtility)
        SearchLit(c, thisCMSPage, thisPage, thisContext, thisServer)
        If c.Controls.Count > 0 Then
            For Each s As Control In c.Controls
                recurseLoadControls(s, thisCMSPage, thisPage, thisContext, thisServer)
            Next
        End If
    End Sub

    Public Sub SearchLit(ByRef c As System.Web.UI.Control, ByRef thisCMSPage As cmsPage, ByRef thisPage As Page, ByRef thisContext As HttpContext, ByRef thisServer As HttpServerUtility)
        If Not IsNothing(c.ID) Then

            Dim thisType As String = c.ToString.ToLower
            Dim thisID As String = c.ID.ToLower

            If _ReplaceTypes.Keys.Contains(thisType) Then
                If _ReplaceTypes(thisType).Controls.Keys.Contains("*") Then
                    With _ReplaceTypes(thisType).Controls("*").hdler
                        Try
                            .Invoke(c, New repl_Argument(thisCMSPage, thisPage, thisContext, thisServer))
                        Catch ex As Exception
                            Throw New Exception( _
                                String.Format( _
                                    "Error Loading [{1}] control named [{0}] in module [{2}]. {3}", _
                                    c.ID, _
                                    c.ToString, _
                                    _ReplaceTypes(thisType).Controls("*").ReplaceModule, _
                                    ex.Message _
                                ) _
                            )
                        End Try
                    End With
                Else
                    If _ReplaceTypes(thisType).Controls.Keys.Contains(thisID) Then
                        With _ReplaceTypes(thisType).Controls(thisID).hdler
                            Try
                                .Invoke(c, New repl_Argument(thisCMSPage, thisPage, thisContext, thisServer))
                            Catch ex As Exception
                                Throw New Exception( _
                                    String.Format( _
                                        "Error Loading [{1}] control named [{0}] in module [{2}]. {3}", _
                                        c.ID, _
                                        c.ToString, _
                                        _ReplaceTypes(thisType).Controls(thisID).ReplaceModule, _
                                        ex.Message _
                                    ) _
                                )
                            End Try
                        End With
                    End If
                End If
            End If

        End If
    End Sub

End Class

Public Class ControlList
    Public Controls As New Dictionary(Of String, rControl)
End Class

Public Class rControl

    Private _ReplaceModule As String
    Public Property ReplaceModule() As String
        Get
            Return _ReplaceModule
        End Get
        Set(ByVal value As String)
            _ReplaceModule = value
        End Set
    End Property

    Private _hdler As System.EventHandler(Of repl_Argument)
    Public Property hdler() As System.EventHandler(Of repl_Argument)
        Get
            Return _hdler
        End Get
        Set(ByVal value As System.EventHandler(Of repl_Argument))
            _hdler = value
        End Set
    End Property

    Public Sub New(ByVal ReplaceModule As String, ByVal hdler As System.EventHandler(Of repl_Argument))
        _ReplaceModule = ReplaceModule
        _hdler = hdler
    End Sub

End Class