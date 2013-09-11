Imports System.Data.SqlClient
Imports Microsoft.VisualBasic
Imports System.Diagnostics
Imports system.threading

Public Class ConfigValidation
    Inherits iSettings

    '#Region "Error Code Functions"

    Public Function ErrorCode(Optional ByRef iPage As Page = Nothing) As Integer
        '        Try
        '            With Me

        '                If .AppName.Length > 0 Then

        '                    tick(iPage, "er4")
        '                    Dim EV As New ntEvtlog.evt
        '                    Try
        '                        EV.RegisterLog(.AppName)
        '                        tick(iPage, "er1")
        '                    Catch
        '                        Return 1
        '                    End Try

        '                    If .Environment.Length > 0 Then
        '                        tick(iPage, "er3")
        '                        If Not IsNothing(.ConnectionString) Then
        '                            tick(iPage, "er2")
        '                            Dim Connection As SqlConnection = Nothing
        '                            Try
        '                                Connection = New SqlConnection(.ConnectionString)
        '                                Connection.Open()
        '                                tick(iPage, "er5")                                
        '                            Catch ex As Exception
        '                                Return 5
        '                            Finally
        '                                If Not IsNothing(Connection) Then
        '                                    Connection.Close()
        '                                End If
        '                            End Try
        '                            If Not CanWrite(Me.Badmail) Then Return 201
        '                            tick(iPage, "er201")
        '                            If Not CanWrite(Me.SignaturePath) Then Return 202
        '                            tick(iPage, "er202")

        '                            ' All ok.
        '                            Return 0
        '                        Else
        '                            Return 2
        '                        End If

        '                    Else
        '                        Return 3
        '                    End If
        '                Else
        '                    Return 4
        '                End If

        '            End With

        '        Catch ex As Exception
        '            Return 500
        '        End Try
    End Function

    '    Private Sub tick(ByRef iPage As Page, ByVal er As String)
    '        If Not IsNothing(iPage) Then
    '            Dim ercheck As CheckBox = Nothing
    '            ercheck = iPage.Master.FindControl("ContentPlaceHolder1").FindControl(er)
    '            If Not IsNothing(ercheck) Then
    '                ercheck.Checked = True
    '            End If
    '        End If
    '    End Sub

    '    Private Function CanWrite(ByVal path As String) As Boolean
    '        Dim ret As Boolean = True
    '        Try
    '            If Right(path, 1) <> "\" Then path += "\"            
    '            Using sw As New System.IO.StreamWriter(path & "testout.txt")
    '                sw.WriteLine("test")
    '                sw.Close()
    '            End Using
    '            System.IO.File.Delete(path & "testout.txt")
    '        Catch ex As Exception
    '            ret = False
    '        End Try
    '        Return ret
    '    End Function

    Public Function ErrorDescription(ByVal ErrCode As Integer) As String

        '        If IsNothing(ErrCode) Then ErrCode = ErrorCode()
        '        Select Case ErrCode
        '            Case 0
        '                Return "Configuration ok."

        '            Case 1
        '                Return "Could not access the event log."

        '            Case 2
        '                Return String.Format( _
        '                    "Invalid Environment variable. Connection [{0}] not found.", _
        '                    Environment _
        '                )

        '            Case 3
        '                Return "No Environmnent variable has been set."

        '            Case 4
        '                Return "No AppName variable has been set."

        '            Case 5
        '                Return String.Format( _
        '                    "Failed to open connection string [{0}]", _
        '                    ConnectionString _
        '                )
        '            Case 201
        '                Return String.Format( _
        '                    "Cannot write to BadMail folder [{0}]", _
        '                    Me.Badmail _
        '                )
        '            Case 202
        '                Return String.Format( _
        '                    "Cannot write to Signature folder [{0}]", _
        '                    Me.SignaturePath _
        '                )
        '            Case Else
        '                Return "An unknown error occured."

        '        End Select

    End Function

    Public Function Fatal(ByVal ErCode) As Boolean
        Dim ret As Boolean = True
        '        Select Case ErCode
        '            Case 0, 201, 202
        ret = False
        '        End Select
        Return ret
    End Function

    '#End Region

End Class
