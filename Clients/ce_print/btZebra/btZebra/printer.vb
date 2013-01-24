
Imports ZSDK_API.Comm
Imports ZSDK_API.Printer
Imports System.Threading
Imports ZSDK_API.ApiException

Public Delegate Sub PrinterConnectionHandler()

Public Class printer
    Public Event connectionEstablished As PrinterConnectionHandler
    Public Event connectionClosed As PrinterConnectionHandler
    Public Event status(ByVal Message As String)

    Private connection As ZebraPrinterConnection
    Private printer As ZebraPrinter

    Private macAddress As [String]

#Region "Connect to Printer"

    Public Sub Connect(ByVal macAddress As [String])
        Me.macAddress = macAddress.Trim()
        Dim t As New Thread(AddressOf doConnectBT)
        t.Start()
    End Sub

    Private Sub doConnectBT()
        If Me.macAddress.Length <> 12 Then
            updateGuiFromWorkerThread("Invalid MAC Address")
            disconnect()
        Else
            updateGuiFromWorkerThread("Connecting... Please wait...")
            Try
                connection = New BluetoothPrinterConnection(Me.macAddress)
                threadedConnect(Me.macAddress)
            Catch generatedExceptionName As ZebraException
                updateGuiFromWorkerThread("COMM Error! Disconnected")
            End Try
        End If
    End Sub

    Private Sub threadedConnect(ByVal addressName As [String])
        Try
            connection.Open()
            Thread.Sleep(1000)
        Catch generatedExceptionName As ZebraPrinterConnectionException
            updateGuiFromWorkerThread("Unable to connect with printer")
            disconnect()
        Catch e As ZebraGeneralException
            updateGuiFromWorkerThread(e.Message)
            disconnect()
        Catch generatedExceptionName As Exception
            updateGuiFromWorkerThread("Error communicating with printer")
            disconnect()
        End Try

        printer = Nothing
        If connection IsNot Nothing AndAlso connection.IsConnected() Then
            Try
                printer = ZebraPrinterFactory.GetInstance(connection)
                Dim pl As PrinterLanguage = printer.GetPrinterControlLanguage()
                updateGuiFromWorkerThread("Printer Language " & pl.ToString())
                Thread.Sleep(1000)
                updateGuiFromWorkerThread("Connected to " & addressName)
                Thread.Sleep(1000)
                RaiseEvent connectionEstablished()
            Catch generatedExceptionName As ZebraPrinterConnectionException
                updateGuiFromWorkerThread("Unknown Printer Language")
                printer = Nothing
                Thread.Sleep(1000)
                disconnect()
            Catch generatedExceptionName As ZebraPrinterLanguageUnknownException
                updateGuiFromWorkerThread("Unknown Printer Language")
                printer = Nothing
                Thread.Sleep(1000)
                disconnect()
            End Try
        End If
    End Sub

#End Region

#Region "Disconnect printer"

    Public Sub disconnect()
        Dim t As New Thread(AddressOf doDisconnect)
        t.Start()
    End Sub

    Private Sub doDisconnect()
        Try
            If connection IsNot Nothing AndAlso connection.IsConnected() Then
                updateGuiFromWorkerThread("Disconnecting")
                connection.Close()
            End If
        Catch generatedExceptionName As ZebraException
            updateGuiFromWorkerThread("COMM Error! Disconnected")
        End Try
        Thread.Sleep(1000)
        RaiseEvent connectionClosed()
        updateGuiFromWorkerThread("Not Connected")
        connection = Nothing
    End Sub

#End Region

    'Public Sub updateGuiFromWorkerThread(ByVal message As [String])
    '    RaiseEvent status(message)
    'End Sub

    'Public Function getConnection() As ZebraPrinterConnection
    '    Return connection
    'End Function

    'Public Function getPrinter() As ZebraPrinter
    '    Return printer.GetGraphicsUtil.StoreImage(
    'End Function

    Public Sub WriteFile(ByVal Filename As String)
        printer.GetFileUtil().SendFileContents(Filename)
    End Sub

    Public Sub Send(ByVal plBytes As Byte())        
        connection.Write(plBytes)
    End Sub

End Class
