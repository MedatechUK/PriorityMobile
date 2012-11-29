'-b "c:\faxme.txt" -f "00448452306740" -r "CW" -s "subject" -n "note" -w
Imports System.Threading
Imports System.Reflection
Imports System.io
Imports ntSettings

Module fClient

    Enum myRunMode As Integer
        Normal = 0
        Config = 1
    End Enum

    Dim xS As New xmlSettings

    Private Body As String = "c:\Docs\Body.txt"
    Private FAXNumber As String = "12225550100"
    Private RecipientName As String = "Bud"
    Private Subject As String = "Today's fax"
    Private Note As String = "Here is the info you requested"
    Private Priority As FAXCOMEXLib.FAX_PRIORITY_TYPE_ENUM = FAXCOMEXLib.FAX_PRIORITY_TYPE_ENUM.fptNORMAL

    Private ReceiptType As FAXCOMEXLib.FAX_RECEIPT_TYPE_ENUM = FAXCOMEXLib.FAX_RECEIPT_TYPE_ENUM.frtMAIL
    Private CoverPageType As FAXCOMEXLib.FAX_COVERPAGE_TYPE_ENUM = FAXCOMEXLib.FAX_COVERPAGE_TYPE_ENUM.fcptLOCAL

    Dim WithEvents cApp As New ConsoleApp.CA

    Public Sub Main()

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
                        FAX()
                    Case myRunMode.Config
                        Dim fm As New Settings(xs)
                        fm.ShowDialog()
                        xS.SaveSettings()
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
                    Case "b", "body"
                        State = "body"
                    Case "f", "faxnumber"
                        State = "faxnumber"
                    Case "r", "recip", "recipientname"
                        State = "recipientname"
                    Case "s", "sub", "subject"
                        State = "subject"
                    Case "n", "note"
                        State = "note"
                    Case "p", "priority"
                        State = "priority"
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
                    Case "body"
                        If File.Exists(StrVal) Then
                            Body = StrVal
                        Else
                            .Quit = True
                            Throw New Exception(String.Format("The FAX body file [{0}] does not exist.", StrVal))
                        End If
                    Case "faxnumber"
                        FAXNumber = StrVal
                    Case "recipientname"
                        RecipientName = StrVal
                    Case "subject"
                        Subject = StrVal
                    Case "note"
                        Note = StrVal
                    Case "priority"
                        Select Case StrVal
                            Case "h", "hi", "high"
                                Priority = FAXCOMEXLib.FAX_PRIORITY_TYPE_ENUM.fptHIGH
                            Case "l", "lo", "low"
                                Priority = FAXCOMEXLib.FAX_PRIORITY_TYPE_ENUM.fptLOW
                            Case Else
                                Priority = FAXCOMEXLib.FAX_PRIORITY_TYPE_ENUM.fptNORMAL
                        End Select
                    Case Else
                        Valid = False
                End Select
            End With
        Catch ex As Exception
            Console.Write(ex.Message)
        End Try
    End Sub

    Sub FAX()

        'Console.WriteLine("")
        'Console.WriteLine("Press any key to continue.")
        'Dim strInput As String = Console.ReadKey(False).ToString
        'While (strInput = "")
        '    Thread.Sleep(100)
        'End While

        Dim objFaxDocument As New FAXCOMEXLib.FaxDocument
        Dim objFaxServer As New FAXCOMEXLib.FaxServer
        'Dim objSender As FAXCOMEXLib.FaxSender
        Dim JobID As Object

        Try

            'Connect to the fax server
            objFaxServer.Connect(xS.Setting("FaxServer"))

            'Set the fax body
            objFaxDocument.Body = Replace(Body, "\", "\\")

            'Name the document
            objFaxDocument.DocumentName = "Priority FAX"

            'Set the fax priority
            objFaxDocument.Priority = Priority

            'Add the recipient with the fax number 12225550100
            objFaxDocument.Recipients.Add(FAXNumber, RecipientName)

            'Choose to attach the fax to the fax receipt
            objFaxDocument.AttachFaxToReceipt = True

            'Set the cover page type and the path to the cover page
            If xS.Setting("CoverPage").Length > 0 Then
                objFaxDocument.CoverPageType = CoverPageType
                objFaxDocument.CoverPage = Replace(xS.Setting("CoverPage"), "\", "\\")
            End If

            'Provide the cover page note
            objFaxDocument.Note = Note

            If My.Settings.ReceiptAddress.Length > 0 Then
                'Set the receipt type to email
                objFaxDocument.ReceiptType = ReceiptType
                'Provide the address for the fax receipt
                objFaxDocument.ReceiptAddress = My.Settings.ReceiptAddress
            End If

            ''Specify that the fax is to be sent at a particular time
            'objFaxDocument.ScheduleType = FAXCOMEXLib.FAX_SCHEDULE_TYPE_ENUM.fstSPECIFIC_TIME
            ''CDate converts the time to the Date data type
            'objFaxDocument.ScheduleTime = CDate("4:35:47 PM")

            objFaxDocument.Subject = Subject

            'Set the sender properties.
            objFaxDocument.Sender.Title = xS.Setting("Title")
            objFaxDocument.Sender.Name = xS.Setting("Name")
            objFaxDocument.Sender.City = xS.Setting("City")
            objFaxDocument.Sender.State = xS.Setting("State")
            objFaxDocument.Sender.Company = xS.Setting("Company")
            objFaxDocument.Sender.Country = xS.Setting("Country")
            objFaxDocument.Sender.Email = xS.Setting("Email")
            objFaxDocument.Sender.FaxNumber = xS.Setting("FaxNumber")
            objFaxDocument.Sender.HomePhone = xS.Setting("HomePhone")
            objFaxDocument.Sender.OfficeLocation = xS.Setting("OfficeLocation")
            objFaxDocument.Sender.OfficePhone = xS.Setting("OfficePhone")
            objFaxDocument.Sender.StreetAddress = xS.Setting("StreetAddress")
            objFaxDocument.Sender.TSID = xS.Setting("TSID")
            objFaxDocument.Sender.ZipCode = xS.Setting("ZipCode")
            objFaxDocument.Sender.BillingCode = xS.Setting("BillingCode")
            objFaxDocument.Sender.Department = xS.Setting("Department")


            'Save sender information as default
            objFaxDocument.Sender.SaveDefaultSender()

            'Submit the document to the connected fax server
            'and get back the job ID.

            JobID = objFaxDocument.ConnectedSubmit(objFaxServer)

            Console.WriteLine("Fax was queued with jobID :" & JobID(0))

            objFaxServer.Disconnect()

        Catch ex As Exception
            Console.WriteLine(ex.Message)
        End Try

    End Sub

End Module
