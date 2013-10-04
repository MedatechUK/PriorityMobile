Imports WatiN.Core
Imports System.IO

Module Scraper
    Sub Main()
        Console.WriteLine("This utility will scrape CreditSafe data by SIC code.")
        Console.WriteLine("A full scrape may take a long time! Consider running overnight")
        Console.WriteLine("if the anticipated return is over 1000 companies.")
        Console.WriteLine("Specify username:")
        Dim user As String = Console.ReadLine()
        Console.WriteLine("Specify password:")
        Dim pass As String = Console.ReadLine()
        Console.WriteLine("")
        Console.WriteLine("Specify SIC03 code:")
        Dim sic As String = Console.ReadLine()
        'Console.WriteLine("Specify SIC07 code:")
        'Dim sic2 As String = Console.ReadLine()
        Console.WriteLine(String.Format("Output will be C:\{0}.txt", sic))
        Console.WriteLine("Specify max pages to return, -1 for all:")
        Dim p As Integer = CInt(Console.ReadLine())

        Console.WriteLine("Specify minimum turnover:")
        Dim turnover As Integer = CInt(Console.ReadLine())
        Dim ie As New IE("http://app.creditsafeuk.com/CSUKLive/webpages/CreditsafeDotCom.aspx")

        Using wr As New StreamWriter(String.Format("C:\{0}.txt", sic))
            ie.TextField("txtUserName").TypeText(user)
            ie.TextField("txtPassword").TypeText(pass)
            ie.Link(Find.ByClass("large_button")).Click()

            ie.Link("ctl00_lnkMapSearch").Click()
            ie.Link("ctl00_MainContent_CompanySearch").Click()
            ie.Link("ctl00_MainContent_ActivityBtn").Click()
            ie.TextField("ctl00_MainContent_txtCode").TypeText(sic)
            ie.Element("ctl00_MainContent_btnSicSearch").Click()
            System.Threading.Thread.Sleep(1000)
            ie.SelectList("ctl00_MainContent_lstAvailableCodes").SelectByValue(sic)

            ie.Element("ctl00_MainContent_btnArrowRight").Click()
            System.Threading.Thread.Sleep(1000)
            ie.WaitForComplete()
            ie.Element("ctl00_MainContent_btnCompanySearch").Click()
            Console.WriteLine("pause")
            ie.WaitUntilContainsText("results")
            

            Dim x As Integer = 0
            Dim ex As New Exception
            Dim errorc As Integer = 0
            wr.WriteLine("Company Name" & vbTab & "Company Number" & vbTab & "Address1" & vbTab & "Address2" & vbTab & "Address3" & vbTab & "Postcode" & vbTab & "Telephone" & vbTab & "Website" & vbTab & "Employees" & vbTab & "Directors" & vbTab & "Net Worth" & vbTab & "Turnover" & vbTab & "Operating Profit" & vbCrLf)

            '207.

            'run from here on monday sucker!
            For k As Integer = 20 To 207 Step 9
                ie.Link(String.Format("ctl00_MainContent_lbLimited_{0}", k)).Click()
            Next

            Dim page As Integer = 0
            While True

                wr.Flush()
                Try
                    ie.WaitUntilContainsText("Search Results")
                    Dim t As Table = ie.Tables(0)
                    Dim rows As WatiN.Core.TableRowCollection = t.TableRows
                    For i As Integer = 1 To rows.Count - 1
                        If ie.Tables(0).TableRows(i).TableCells(6).Text.Contains("Active") And Not ie.Tables(0).TableRows(i).TableCells(6).Text.Contains("Newly") Then
                            ie.Tables(0).TableRows(i).Links(0).Click()
                            System.Threading.Thread.Sleep(1500)
                            ie.WaitUntilContainsText("Commentary")

                            Try
                                ie.WaitForComplete()
                                'at page
                                With wr
                                    'front
                                    'If Not ie.Span("ctl00_MainContent_CompanySummary1__lblSic07Code").Text.Contains(sic2) Then
                                    '    wait("Search Results", ie)
                                    '    Continue For
                                    'End If
                                    ie.Link("ctl00_MainContent_hlKeyFinancials").Click()
                                    ie.WaitUntilContainsText("Wages & Salaries")
                                    If ie.Span("ctl00_MainContent_KeyFinancials1_lblPLTurnover1").Text.Contains("-") Then
                                        wait("Search Results", ie)
                                        Continue For
                                    ElseIf CLng(ie.Span("ctl00_MainContent_KeyFinancials1_lblPLTurnover1").Text.TrimStart("£")) < turnOver Then
                                        wait("Search Results", ie)
                                        Continue For
                                    End If

                                    ie.Back()

                                    .Write(ie.Span("ctl00_MainContent_CompanySummary1__lblCompanyName").Text)
                                    .Write(vbTab)
                                    .Write(ie.Span("ctl00_MainContent_CompanySummary1__lblCompanyNumber").Text)
                                    .Write(vbTab)
                                    .Write(ie.Span("ctl00_MainContent_CompanySummary1__lblRegisteredAddressLine1").Text)
                                    .Write(vbTab)
                                    .Write(ie.Span("ctl00_MainContent_CompanySummary1__lblRegisteredAddressLine2").Text)
                                    .Write(vbTab)
                                    .Write(ie.Span("ctl00_MainContent_CompanySummary1__lblRegisteredAddressLine3").Text)
                                    .Write(vbTab)
                                    .Write(ie.Span("ctl00_MainContent_CompanySummary1__lblRegisteredPostCode").Text)
                                    .Write(vbTab)
                                    If Not ie.Span("ctl00_MainContent_CompanySummary1__lblPhone").Text = "-" Then
                                        .Write(CLng(ie.Span("ctl00_MainContent_CompanySummary1__lblPhone").Text).ToString("00000 ### ###"))
                                    Else
                                        .Write("-")
                                    End If
                                    .Write(vbTab)
                                    .Write(ie.Link("ctl00_MainContent_CompanySummary1_hlWebsiteAddress").Url)
                                    .Write(vbTab)
                                    'employees
                                    .Write(ie.Table("ctl00_MainContent_CompanySummaryAdditional1_keyFinalcials").TableRows(1).TableCells(4).Text)
                                    .Write(vbTab)
                                    'directorship
                                    ie.Link("ctl00_MainContent_hlDirector").Click()
                                    ie.WaitUntilContainsText("Present Appointments")
                                    Dim di As Integer = CInt(ie.Span("ctl00_MainContent_Directors1__lblTotalDirectors").Text)

                                    For j As Integer = 0 To di - 1
                                        .Write(ie.Link(String.Format("ctl00_MainContent_Directors1_reCurrentDirectors_ctl0{0}_hlDirectorName", j)).Text)
                                        .Write(", ")
                                    Next
                                    .Write(vbTab)
                                    'financials
                                    ie.Link("ctl00_MainContent_hlKeyFinancials").Click()
                                    ie.WaitUntilContainsText("Wages & Salaries")
                                    .Write(ie.Span("ctl00_MainContent_KeyFinancials1_lblOFINetWorth1").Text)
                                    .Write(vbTab)
                                    .Write(ie.Span("ctl00_MainContent_KeyFinancials1_lblPLTurnover1").Text)
                                    .Write(vbTab)
                                    .Write(ie.Span("ctl00_MainContent_KeyFinancials1_lblPLOperatingProfit1").Text)
                                    .Write(vbCrLf)
                                    wait("Search Result", ie)
                                End With

                            Catch exc As Exception
                                errorc += 1
                                If errorc < 3 Then
                                    wait("Search Results", ie)
                                End If
                            End Try


                        End If

                    Next
                    Try
                        If ie.ContainsText("Search Results") Then
                            ie.Link("ctl00_MainContent_lbNextLimited").Click()
                            x += 1
                        End If
                        If x = p And Not p = -1 Then
                            Throw (ex)
                        End If
                    Catch ex
                        Exit While
                    End Try
                Catch
                    wait("Search Results", ie)
                End Try

            End While

            'For Each row As TableRow In rows
            '    If row.TableCells.Count < 1 Then
            '    Else
            '        Console.WriteLine(row.TableCells(5).Text)
            '        If CDate(row.TableCells(5).Text.Trim(" ")) > New Date(2012, 1, 1) Then
            '            row.TableCells(0).Link(Find.First).Click()
            '            Console.WriteLine(ie.Span("ctl00_MainContent_CompanySummary1__lblCompanyName").Text)
            '            Console.WriteLine()
            '            Console.WriteLine(ie.Span("ctl00_MainContent_CompanySummary1__lblRegisteredAddressLine1").Text)
            '            Console.WriteLine(ie.Span("ctl00_MainContent_CompanySummary1__lblRegisteredAddressLine2").Text)
            '            Console.WriteLine(ie.Span("ctl00_MainContent_CompanySummary1__lblRegisteredAddressLine3").Text)
            '            Console.WriteLine(ie.Span("ctl00_MainContent_CompanySummary1__lblRegisteredPostCode").Text)
            '            ie.Back()
            '        End If
            '    End If

            'iterative approach required for the child pages found. What if we extract actual links, and 
            'open each one as links, rather than iterating over the table cells...... 
            'Next
            ie.Close()
        End Using
    End Sub

    Public Sub wait(ByVal terms As String, ByRef ie As IE)
        While Not ie.ContainsText(terms)
            Try
                ie.WaitUntilContainsText(terms, 1)
            Catch
                ie.Back()
            End Try
        End While
    End Sub
End Module
