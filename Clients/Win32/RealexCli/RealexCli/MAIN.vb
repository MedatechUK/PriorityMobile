Imports System.Reflection
Imports System.Xml
Imports System.IO
Imports System.Text

Module MAIN

    Public Enum myRunMode As Integer
        Normal = 0
        Config = 1
        Test = 2
    End Enum

    Dim ConfigOpt As New Dictionary(Of String, String)

    Dim doc As New XmlDocument

    Public debug As Boolean = False
    Dim WithEvents cApp As New ConsoleApp.CA

    Dim PASRef As String = Nothing
    Dim TransactionID As String = Nothing
    Dim authcode As String = Nothing

#Region "Private Properties"

    Private ReadOnly Property Paths() As String() ' Returns the paths that should be in the XML settings file
        Get
            Dim ret() As String = { _
                            "merchant/name", _
                            "merchant/subaccount", _
                            "merchant/password/normal", _
                            "merchant/password/refund", _
                            "merchant/password/rebait" _
                        }
            Return ret
        End Get
    End Property

    Private ReadOnly Property XMLSettings() As String ' Returns the path to the setting file
        Get
            Dim fullPath As String = Replace(Replace(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase, "file:///", "", , , CompareMethod.Text), "/", "\")
            Return String.Format("{0}\merchant.xml", _
                fullPath.Substring(0, fullPath.LastIndexOf("\")), _
                "")
        End Get
    End Property

#End Region

#Region "Initialisation and Finalisation"

    Public Sub main()

        Dim StartTime As Date = Now
        With cApp
            .RunMode = myRunMode.Normal
            .doWelcome(Assembly.GetExecutingAssembly())

            If Not File.Exists(XMLSettings) Then
                Console.WriteLine("Created configuration file. Please /config.")
                .Quit = True
            Else
                doc.Load(XMLSettings)
            End If

            MakePaths(Paths)
            doc.Save(XMLSettings)

            If Not .Quit Then
                Try
                    .GetArgs(Command)
                Catch ex As Exception
                    Console.WriteLine("")
                    Console.WriteLine(ex.Message)
                    .Quit = True
                End Try
            End If

            If Not .Quit Then
                Select Case .RunMode
                    Case myRunMode.Normal
                        If IsNothing(PASRef) Or IsNothing(TransactionID) Or IsNothing(authcode) Then
                            Console.WriteLine("Missing Parameter(s). Please seek /help.")
                            .Quit = True
                        End If
                        If Not .Quit Then payload()
                    Case myRunMode.Config
                        If Not .Quit Then doConfig()
                    Case myRunMode.Test
                        Console.WriteLine("Running in test mode...")
                        If Not .Quit Then doTestPayment()
                End Select
            End If

            Dim RunLength As System.TimeSpan = Now.Subtract(StartTime)
            Console.WriteLine("")
            If debug Then Console.WriteLine(String.Format("Completed in {0} milliseconds.", RunLength.Milliseconds.ToString))

            cApp.Finalize()

        End With

        Application.Exit()

    End Sub

#Region "Switch Handlers"

    Private Sub cApp_Switch(ByVal StrVal As String, ByRef State As String, ByRef Valid As Boolean) Handles cApp.Switch
        Try
            With cApp
                Select Case StrVal.ToLower
                    Case "config"
                        .RunMode = myRunMode.Config
                    Case "test"
                        .RunMode = myRunMode.Test
                        debug = True
                    Case "a", "auth", "authcode"
                        State = "authcode"
                    Case "r", "ref", "pasref"
                        State = "pasref"
                    Case "i", "id", "transactionid"
                        State = "transactionid"
                    Case "v"
                        debug = True
                    Case Else
                        If Not IsNothing(doc.SelectSingleNode(StrVal)) Then
                            If doc.SelectSingleNode(StrVal).InnerText = doc.SelectSingleNode(StrVal).InnerXml Then
                                State = StrVal
                            Else
                                Valid = False
                            End If
                        Else
                            Valid = False
                        End If
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
                    Case "authcode"
                        authcode = StrVal
                    Case "pasref"
                        PASRef = StrVal
                    Case "transactionid"
                        TransactionID = StrVal
                    Case Else
                        If Not IsNothing(doc.SelectSingleNode(State)) Then
                            ConfigOpt.Add(State, StrVal)
                        Else
                            Valid = False
                        End If
                End Select
            End With
        Catch ex As Exception
            Console.Write(ex.Message)
        End Try
    End Sub

#End Region

#End Region

#Region "Private Methods"

    Private Sub payload() ' Send an XML request

        Dim tReq As RealexPayments.RealAuth.TransactionRequest
        With doc
            tReq = New RealexPayments.RealAuth.TransactionRequest( _
                .SelectSingleNode("merchant/name").InnerText, _
                .SelectSingleNode("merchant/password/normal").InnerText, _
                .SelectSingleNode("merchant/password/rebait").InnerText, _
                .SelectSingleNode("merchant/password/refund").InnerText _
            )
            Dim rResp As RealexPayments.RealAuth.TransactionResponse = _
                tReq.Void( _
                            .SelectSingleNode("merchant/subaccount").InnerText, _
                            TransactionID, _
                            PASRef, _
                            authcode _
                          )
            MsgBox(rResp.ResultMessage)
        End With

    End Sub

    Private Sub doTestPayment() ' Send a test payment and try to VOID it

        Dim tReq As RealexPayments.RealAuth.TransactionRequest
        Dim ccReq As RealexPayments.RealAuth.CreditCard
        Dim tResp As RealexPayments.RealAuth.TransactionResponse

        Console.WriteLine("")
        Console.WriteLine("Creating dummy payment...")

        tReq = New RealexPayments.RealAuth.TransactionRequest("trutextest", "secret", "secret", "secret")
        ccReq = New RealexPayments.RealAuth.CreditCard("visa", "4242424242424242", "0615", "Andrew Harcourt", "123", RealexPayments.RealAuth.CreditCard.CVN_PRESENT)
        tResp = tReq.Authorize("internet", Now().ToString("yyyyMMddhhmmss"), "EUR", 4133, ccReq)

        tReq = New RealexPayments.RealAuth.TransactionRequest("trutextest", "secret", "secret", "secret")
        With tResp

            Console.WriteLine("")
            Console.WriteLine("Attempting to void transaction...")
            Console.WriteLine("")

            Dim rVoid As RealexPayments.RealAuth.TransactionResponse = _
                tReq.Void( _
                            "internet", _
                            .ResultOrderID, _
                            .ResultPASRef, _
                            .ResultAuthCode _
                          )
        End With
    End Sub

    Private Sub doConfig() ' Make changes to, or display the config file

        Dim CH As Boolean = False
        For Each k As String In ConfigOpt.Keys
            doc.SelectSingleNode(k).InnerText = ConfigOpt(k)
            Console.WriteLine(String.Format("Setting {0} to: {1}", k, ConfigOpt(k)))
            CH = True
        Next
        If CH Then
            doc.Save(XMLSettings)
        Else
            Console.Write(IndentXMLString(doc))
        End If

    End Sub

#End Region

#Region "XML functions"

    Private Function IndentXMLString(ByVal doc As XmlDocument) As String ' Format XML for reading
        Dim ms As New MemoryStream
        Dim xtw As New XmlTextWriter(ms, Encoding.Unicode)
        xtw.Formatting = Formatting.Indented
        doc.WriteContentTo(xtw)
        xtw.Flush()
        ms.Seek(0, SeekOrigin.Begin)
        Using SR As New StreamReader(ms)
            Return SR.ReadToEnd
        End Using
    End Function

    Public Sub MakePaths(ByVal path() As String) ' Create missing paths in XML file
        For Each p As String In path
            Dim thisNode As XmlNode = doc.FirstChild
            For Each el As String In Split(p, "/")
                If IsNothing(thisNode) Then
                    With doc
                        .AppendChild(doc.CreateElement(el))
                        thisNode = .SelectSingleNode(el)
                        Console.WriteLine("Creating node {0}.", el)
                    End With
                Else
                    With thisNode
                        If Not String.Compare(el, thisNode.Name, True) = 0 Then
                            If IsNothing(.SelectSingleNode(el)) Then
                                .AppendChild(.OwnerDocument.CreateElement(el))
                                Console.WriteLine("Creating node {0} as child of {1}.", el, .Name)
                            End If
                            thisNode = .SelectSingleNode(el)
                        End If
                    End With
                End If
            Next
        Next
    End Sub

#End Region

End Module
