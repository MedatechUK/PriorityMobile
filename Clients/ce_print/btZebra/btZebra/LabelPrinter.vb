Imports CPCL

Imports ZSDK_API.Comm
Imports ZSDK_API.Printer
Imports System.Threading
Imports ZSDK_API.ApiException

Public Class LabelPrinter : Inherits CPCL.LabelPrinter

    Private connection As ZebraPrinterConnection
    Private printer As ZebraPrinter

#Region "initialisation and finalisation"

    Public Sub New(ByVal dpi As System.Drawing.Point, ByVal Dimensions As System.Drawing.Size, Optional ByVal ImageFolder As String = "")
        With Me
            .ImageFolder = ImageFolder
            .dpi = dpi
            .Dimensions = Dimensions
        End With
    End Sub

#End Region

#Region "Overriden Methods"

    Public Overrides Sub Print(ByVal Bytes() As Byte)
        connection.Write(Bytes)
    End Sub

    Public Overrides Sub StoreImage(ByVal Filename As String, ByVal Image As System.Drawing.Bitmap)
        Try            
            printer.GetGraphicsUtil.StoreImage( _
                Filename, _
                Image, _
                Image.Width, _
                Image.Height _
            )
        Catch EX As Exception
            MsgBox(EX.Message)
        End Try
    End Sub

    Public Overrides Function fileNames() As String()
        Return printer.GetFileUtil().RetrieveFileNames()
    End Function

#End Region

#Region "Connect to Printer"

    Public Overrides Sub BeginConnect(ByVal macAddress As String, Optional ByVal PIN As String = Nothing, Optional ByVal RefreshImages As Boolean = False)

        With Me
            .macAddress = macAddress.Trim()
            .PIN = PIN
            .RefreshImages = RefreshImages
        End With

        Dim t As New Thread(AddressOf doConnectBT)
        t.Start()

    End Sub

    Private Sub doConnectBT()
        If Me.macAddress.Length <> 12 Then
            MsgBox("Invalid MAC Address")
            disconnect()
        Else
            Try
                connection = New BluetoothPrinterConnection(Me.macAddress)
                threadedConnect(Me.macAddress)
            Catch generatedExceptionName As ZebraException
                MsgBox("COMM Error! Disconnected")
                disconnect()
            End Try
        End If
    End Sub

    Private Sub threadedConnect(ByVal addressName As [String])
        Try
            connection.Open()
            Thread.Sleep(1000)
        Catch generatedExceptionName As ZebraPrinterConnectionException
            MsgBox("Unable to connect with printer")
            disconnect()
        Catch e As ZebraGeneralException
            MsgBox(e.Message)
            disconnect()
        Catch generatedExceptionName As Exception
            MsgBox("Error communicating with printer")
            disconnect()
        End Try

        printer = Nothing
        If connection IsNot Nothing AndAlso connection.IsConnected() Then
            Try
                printer = ZebraPrinterFactory.GetInstance(connection)
                Thread.Sleep(1000)

                ' Connected
                Me.LoadImages(Me.RefreshImages)
                Me.RaiseConnect()

            Catch EX As Exception
                MsgBox(EX.Message)
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
                connection.Close()
            End If
        Catch generatedExceptionName As ZebraException

        End Try
        Thread.Sleep(1000)
        RaiseDisconnect()
        connection = Nothing
    End Sub

#End Region

End Class
