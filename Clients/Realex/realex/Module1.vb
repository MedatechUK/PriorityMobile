Imports System.Reflection
Imports System.io
Imports ConsoleApp

Module RealexCli

    Private CUST As Integer
    Private AMT As Double
    Private CUR As String

    Enum myRunMode As Integer
        Normal = 0
        Config = 1
    End Enum

    Dim WithEvents cApp As New ConsoleApp.CA

    Sub Main()
        With cApp
            .RunMode = myRunMode.Normal
            .doWelcome(Assembly.GetExecutingAssembly())
            Try
                .GetArgs(Command)
            Catch ex As Exception
                Console.WriteLine(ex.Message)
            End Try

            If Not .Quit Then
                Select Case .RunMode
                    Case myRunMode.Normal
                        PAYLOAD()
                    Case myRunMode.Config
                        Dim fm As New Settings
                        fm.ShowDialog()
                End Select
            End If

            cApp.Finalize()

        End With
    End Sub

    Private Sub cApp_Switch(ByVal StrVal As String, ByRef State As String, ByRef Valid As Boolean) Handles cApp.Switch
        Try
            With cApp
                Select Case StrVal
                    Case "config"
                        .RunMode = myRunMode.Config
                        State = Nothing
                    Case "c", "cust"
                        State = "c"
                    Case "a", "amt", "amount"
                        State = "a"
                    Case "x", "cur", "currency"
                        State = "x"
                    Case Else
                        Valid = False
                End Select
            End With
        Catch ex As Exception
            Console.Write(ex.Message)
        End Try
    End Sub

    Private Sub cApp_SwitchVar(ByVal State As String, ByVal StrVal As String, ByRef Valid As Boolean) Handles cApp.SwitchVar
        Try
            With cApp
                Select Case State
                    Case "c"
                        CUST = CInt(StrVal)
                    Case "a"
                        AMT = CDbl(StrVal)
                    Case "x"
                        CUR = StrVal
                    Case Else
                        Valid = False
                End Select
            End With
        Catch ex As Exception
            Console.Write(ex.Message)
        End Try
    End Sub

    Sub PAYLOAD()

        Dim ws As New PriWebSVC.Service
        Dim sd As New Priority.SerialData
        '
        Try

            Dim data(,) As String = sd.DeSerialiseData(ws.GetData("SELECT ZATP_CARDHOLDER, ZATP_CARDTYPE, ZATP_CARDNUM, ZATP_EXPMONTH, ZATP_EXPYEAR, ZATP_CVN FROM CUSTOMERS WHERE CUST = " & CUST))

            If IsNothing(data) Then _
                Throw New Exception( _
                    String.Format( _
                        "Customer ID {0} was not found in the database.", CUST))

            If Left(data(0, 0), 1) = "!" Then _
                Throw New Exception( _
                    String.Format( _
                        "An error occured proccesing the request: {0}", _
                        Right(data(0, 0), Len(data(0, 0) - 1))))

            For I As Integer = 0 To UBound(data, 1)
                If data(I, 0).Length = 0 Then _
                        Throw New Exception( _
                                "Incomplete card data in the customer record.")
            Next

            Dim tReq As RealexPayments.RealAuth.TransactionRequest
            Dim ccReq As RealexPayments.RealAuth.CreditCard
            Dim tResp As RealexPayments.RealAuth.TransactionResponse

            Dim orderID As String
            orderID = Now().ToString("yyyyMMddhhmmss")

            With My.Settings
                tReq = New RealexPayments.RealAuth.TransactionRequest( _
                    .MerchantName, _
                    .normalPassword, _
                    .rebatePassword, _
                    .refundPassword _
                )

                ccReq = New RealexPayments.RealAuth.CreditCard( _
                    data(1, 0), _
                    data(2, 0), _
                    Right("00" & data(3, 0), 2) & Right("00" & data(4, 0), 2), _
                    data(0, 0), _
                    data(5, 0), _
                    RealexPayments.RealAuth.CreditCard.CVN_PRESENT _
                )
                tResp = tReq.Authorize(.transAccount, _
                    orderID, _
                    CUR, _
                    AMT, _
                    ccReq _
                )
            End With

            If (tResp.ResultCode = 0 Or tResp.ResultCode = 200) Then

                Dim p As New Priority.Load

                '** TODO: Set the appropriate values for the loading below
                Try
                    With p
                        ''.DebugFlag = True
                        .Procedure = "ZSFDC_TEST"
                        .Table = "ZATP_BATCHCCREC"
                        .RecordType1 = "CUST, QPRICE, CONFNUM, ZATP_PASREF"
                        .RecordTypes = ",,TEXT,TEXT"
                    End With

                    ' Type 1 records
                    Dim t1() As String = { _
                                        CUST, _
                                        AMT, _
                                        tResp.ResultAuthCode, _
                                        tResp.ResultPASRef _
                                        }
                    p.AddRecord(1) = t1
                    ws.LoadData(sd.SerialiseDataArray(p.Data))

                Catch ex As Exception
                    ' transaction failed
                    Throw New Exception(ex.Message)
                End Try
            Else
                Throw New Exception(String.Format("Transaction Fails with code: {0}.", tResp.ResultCode))
            End If

        Catch ex As Exception
            ' transaction failed
            Dim Ep As New Priority.Load

            '** TODO: Set the appropriate values for the loading below
            Try
                With Ep
                    ''.DebugFlag = True
                    .Procedure = "ZSFDC_TEST"
                    .Table = "ZATP_BATCHCCREC"
                    .RecordType1 = "CUST, QPRICE, ERR, ERROR"
                    .RecordTypes = ",,TEXT,TEXT"
                End With

                ' Type 1 records
                Dim t1() As String = { _
                                        CUST, _
                                        AMT, _
                                        "Y", _
                                        ex.Message _
                                    }
                Ep.AddRecord(1) = t1
                ws.LoadData(sd.SerialiseDataArray(Ep.Data))

            Catch exp As Exception
                ' transaction failed
                Console.Write(ex.Message)
            End Try
        End Try

    End Sub

End Module
