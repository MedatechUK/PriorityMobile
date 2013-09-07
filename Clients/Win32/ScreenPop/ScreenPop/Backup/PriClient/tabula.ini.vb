Imports System.IO
Imports ntEvtlog

Public Class tabulaini

    Private _ev As ntEvtlog.evt

#Region "initialisation"

    Sub New(Optional ByRef ev As ntEvtlog.evt = Nothing)

        If Not IsNothing(ev) Then
            _ev = ev
        End If

        If Not File.Exists(tabulaini) Then
            Me.SafeLog( _
                String.Format( _
                    "The tabula.ini file was not found on this system.{0}Please ensure that the Priority Client is properly installed.", _
                    vbCrLf _
                ), _
                 EventLogEntryType.Error, _
                 ntEvtlog.EvtLogVerbosity.Normal _
            )
        Else
            Try
                Dim k As String = ""
                Using sr As New StreamReader(tabulaini)
                    With sr
                        While Not sr.EndOfStream
                            Dim l As String = Trim(.ReadLine)
                            If l.Length > 0 Then
                                If Not Left(l, 1) = "#" Then
                                    If Left(l, 1) = "[" And Right(l, 1) = "]" Then
                                        k = Mid(l, 2, l.Length - 2)
                                    Else
                                        Dim eq = InStr(l, "=")
                                        If eq > 0 Then
                                            If Not _iniDictionary.ContainsKey(k) Then
                                                _iniDictionary.Add(k, New Dictionary(Of String, String))
                                            End If
                                            _iniDictionary(k).Add(Strings.Mid(l, 1, eq - 1), Strings.Mid(l, eq + 1, l.Length - eq))
                                        End If
                                    End If
                                End If
                            End If
                        End While
                        .Close()
                    End With
                End Using
                _loaded = True

            Catch ex As Exception
                Me.SafeLog( _
                    String.Format( _
                        "The tabula.ini file was not loaded.{0}{1}", _
                        vbCrLf, _
                        ex.Message _
                    ), _
                     EventLogEntryType.Error, _
                     ntEvtlog.EvtLogVerbosity.Normal _
                )
            End Try
        End If
    End Sub

#End Region

#Region "Public properties"

    Private ReadOnly Property tabulaini() As String
        Get
            Return Replace(Replace(Environment.GetEnvironmentVariable("SystemRoot") & "\tabula.ini", "\\", "\"), "/", "\")
        End Get
    End Property

    Private _loaded As Boolean
    Public ReadOnly Property Loaded() As Boolean
        Get
            Return _loaded
        End Get
    End Property

    Private _iniDictionary As New Dictionary(Of String, Dictionary(Of String, String))
    Public ReadOnly Property iniDictionary() As Dictionary(Of String, Dictionary(Of String, String))
        Get
            Return _iniDictionary
        End Get
    End Property

    Public ReadOnly Property iniValue(ByVal Key As String, ByVal Name As String) As String

        Get
            If _iniDictionary.ContainsKey(Key) Then
                If _iniDictionary(Key).ContainsKey(Name) Then
                    Return _iniDictionary(Key).Item(Name)
                Else
                    Throw New Exception("The tabula.ini did not contain the key pair [" & Key & "." & Name & "]")
                End If
            Else
                Throw New Exception("The tabula.ini did not contain the key pair [" & Key & "." & Name & "]")
            End If
        End Get
    End Property

#End Region

#Region "Private Functions"

    Private Sub SafeLog(ByVal Entry As String, ByVal EventType As LogEntryType, ByVal Verbosity As ntEvtlog.EvtLogVerbosity)
        Try
            If IsNothing(_ev) Then Throw New Exception("No event object specified.")
            _ev.Log( _
                Entry, _
                EventType, _
                Verbosity _
              )
        Catch exep As Exception
            Console.WriteLine( _
                "Failed to write to the [{0} {1}] log.{4}" & _
                "The error reported was: {4}{2}{4}" & _
                "The event could not be written to the log because: {4}{3}{4}", _
                _ev.LogName, _ev.AppName, Entry, exep.Message, vbCrLf _
            )
        End Try
    End Sub

#End Region

#Region "EXAMPLES"

    'Imports System.threading

    'Public Module main

    '        Public Sub Main()
    'Dim ini As New Priority.tabulaini
    '            For Each k As String In ini.iniDictionary.Keys
    '                For Each n As String In ini.iniDictionary(k).Keys
    '                    Console.WriteLine( _
    '                        String.Format( _
    '                            "{0}.{1}={2}", _
    '                            k, n, ini.iniDictionary(k)(n) _
    '                        ) _
    '                    )
    '                Next
    '            Next

    '            Console.WriteLine( _
    '                String.Format( _
    '                    "Priority Directory: {0}", _
    '                    ini.iniValue("Environment", "Priority Directory") _
    '                ) _
    '            )


    '            Dim keypress As String = ""
    '            Do
    '                keypress = Console.ReadKey(False).ToString
    '                Thread.Sleep(100)
    '            Loop Until keypress.Length > 0

    '        End Sub

    '    End Module

#End Region

End Class
