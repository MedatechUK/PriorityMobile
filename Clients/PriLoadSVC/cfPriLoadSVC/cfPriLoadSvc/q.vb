Imports System.Threading
Imports ntDictionaryLib
Imports System.io

Public Class q
    Inherits dCls

    Private qLock As New Queue
    Private qing As Boolean = False

    Public Event hSend( _
        ByVal qi As qItem, _
        ByVal SerialData As String, _
        ByRef hResult As Boolean _
    )

    Private _ev As ntEvtlog.evt

    Sub New(Optional ByRef ev As ntEvtlog.evt = Nothing)


        MyBase.New("ntDictionaryLib.qItem")

        ' Initialise the object
        With Me
            .Name = "q"
            .Undelete = False
        End With

        StackMode = tStackMode.fifo
        Load(tSource.File)

        If Not IsNothing(ev) Then
            _ev = ev
        End If

        ' Create new q thread 
        Dim myThread As Thread
        myThread = New Thread(New ThreadStart(AddressOf StartQ))
        myThread.Start()

    End Sub

    Private Sub StartQ()
        Try

            Thread.Sleep(2000)

            Me.SafeLog( _
                String.Format( _
                    "Initialising the queue object...{0}{1} items the queue at start-up.", _
                    vbCrLf, _
                    Data.count _
                ), _
                EventLogEntryType.Information, _
                ntEvtlog.EvtLogVerbosity.VeryVerbose _
            )

            If Data.Count > 0 Then
                StartThread()
            End If

        Catch ex As Exception
            Me.SafeLog( _
                String.Format( _
                    "An error occured whilst creating the queue:{0}{1}", _
                    vbCrLf, _
                    ex.Message _
                ), _
                 EventLogEntryType.Information, _
                 ntEvtlog.EvtLogVerbosity.Normal _
            )
        End Try

    End Sub

    Private _quit As Boolean
    Public Property Quit() As Boolean
        Get
            Return _quit
        End Get
        Set(ByVal value As Boolean)
            _quit = value
        End Set
    End Property

    Public Overrides Function Index(ByVal RowData() As String) As String
        Return RowData(1)
    End Function

    Public Overrides Function Columns() As String()
        ' Set the columns
        Dim myCols() As String = { _
            "BubbleID", "Filename", "qDate", "Caller", "SOAPProc" _
        }
        Return myCols
    End Function

    Public Overrides Sub OnInsertComplete(ByVal key As Object, ByVal value As Object)
        Try
            MyBase.OnInsertComplete(key, value)
            If Not Me.Seeking Then
                Me.Save(tSource.File)
                StartThread()
            End If
        Catch ex As Exception
            Me.SafeLog( _
                String.Format( _
                    "An error occured whilst inserting an item into the queue:{0}{1}", _
                    vbCrLf, _
                    ex.Message _
                ), _
                 EventLogEntryType.Information, _
                 ntEvtlog.EvtLogVerbosity.Normal _
            )
        End Try
    End Sub

    Private Sub StartThread()

        Monitor.Enter(qLock)
        Try
            If Not qing Then
                qing = True

                Me.SafeLog( _
                    String.Format( _
                        "Starting Queue thread...", _
                        vbCrLf _
                    ), _
                    EventLogEntryType.Information, ntEvtlog.EvtLogVerbosity.VeryVerbose _
                )

                ' Create new q thread 
                Dim myThread As Thread
                myThread = New Thread(New ThreadStart(AddressOf doq))
                myThread.Start()

            End If
        Catch ex As Exception
            Me.SafeLog( _
                String.Format( _
                    "An error occured whilst starting the queue thread:{0}{1}", _
                    vbCrLf, _
                    ex.Message _
                ), _
                 EventLogEntryType.Information, _
                 ntEvtlog.EvtLogVerbosity.Normal _
            )
        Finally
            Monitor.Exit(qLock)
        End Try

    End Sub

    Public Overrides Sub OnRemoveComplete(ByVal key As Object, ByVal value As Object)
        MyBase.OnRemoveComplete(key, value)
        Me.SafeLog( _
            String.Format( _
                "The file was removed from the queue:{0}{1}.", _
                vbCrLf, _
                key _
            ), _
             EventLogEntryType.SuccessAudit, _
             ntEvtlog.EvtLogVerbosity.Verbose _
        )
        Me.Save(tSource.File)
    End Sub

    Private Sub doq()

        While Data.Count > 0 And Not (_quit)
            Try
                Dim key As String = PeekKey()
                Dim fn As String = Data.Item(key).filename
                Dim hResult As Boolean = False
                Dim p

                Select Case LCase(Data.item(key).SOAPProc)
                    Case "savesignature"
                        p = New Priority.SerialData
                    Case Else
                        p = New Priority.Loading
                End Select

                With p
                    If .FromFile(fn) Then
                        RaiseEvent hSend(Data.Item(key), .toserial, hResult)
                    Else
                        Data.Remove(key)
                        Throw New Exception( _
                            String.Format( _
                                "BubbleID [{2}]{0} for application [{3}].{0}The file was not found:{0}{1}{0}Removing bad Bubble from the queue.", _
                                vbCrLf, _
                                fn, _
                                Data.Item(key).BubbleID, _
                                Data.Item(key).Caller _
                            ) _
                        )
                    End If
                End With

                If hResult Then
                    Me.SafeLog( _
                        String.Format( _
                            "Moving file from MAIL/OUTBOX to MAIL/SENT{0}{1}", _
                            vbCrLf, _
                            fn _
                        ), _
                         EventLogEntryType.Information, _
                         ntEvtlog.EvtLogVerbosity.VeryVerbose _
                    )
                    File.Move(fn, Replace(fn, "OUTBOX", "SENT", , , CompareMethod.Text))
                    Data.Remove(key)
                Else
                    For i As Integer = 0 To 10
                        Thread.Sleep(1000)
                        If _quit Then Exit While
                    Next
                End If

            Catch ex As Exception
                Me.SafeLog( _
                    String.Format( _
                        "An error occured whilst processing a queued item.{0}{1}", _
                        vbCrLf, _
                        ex.Message _
                    ), _
                     EventLogEntryType.Error, _
                     ntEvtlog.EvtLogVerbosity.Normal _
                )
            End Try
        End While
        qing = False

    End Sub

    Private Sub SafeLog(ByVal Entry As String, ByVal EventType As EventLogEntryType, ByVal Verbosity As ntEvtlog.EvtLogVerbosity)
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

End Class