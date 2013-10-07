Imports PrioritySFDC
Imports Priority
Imports CPCL

Public Class myHandler : Inherits iHandler

    Public Overrides Sub DisabledButtons(ByRef Add As Boolean, ByRef Edit As Boolean, ByRef Copy As Boolean, ByRef Delete As Boolean, ByRef FormPrint As Boolean, ByRef TablePrint As Boolean)
        TablePrint = True
        'Print = True
    End Sub

#Region "Print Routines"

    Public Overrides Sub btn_PrintPress(ByRef thisForm As PrioritySFDC.iForm, ByVal PrintWhat As ePrintWhat)
        With thisForm.Printer
            If Not .Connected Then
                .BeginConnect(thisForm.ue.MACAddress)
                Do While .WaitConnect
                    Threading.Thread.Sleep(100)
                Loop
            End If
            If .Connected Then
                Select Case PrintWhat
                    Case ePrintWhat.Form
                        PrintForm(thisForm)
                    Case ePrintWhat.Table
                        PrintTable(thisForm)
                End Select
            End If
        End With
    End Sub

    Public Overrides Sub PrintForm(ByRef thisForm As PrioritySFDC.iForm)

        Dim smallFont As New PrinterFont(35, 0, 2)
        Dim warhs As String = String.Empty
        Dim bin As String = String.Empty

        With thisForm.ViewMain.FormView.ViewForm
            warhs = .Column(":$.TOWARHSNAME")
            bin = .Column(":$.TOLOCNAME")
            If Not (warhs.Length > 0 And bin.Length > 0) Then
                MsgBox("Please select a warehouse first!", , "Print Error.")
                Exit Sub
            End If
        End With

        Dim fCopies As New frmCopies
        With fCopies
            .ShowDialog()
            If .Result = DialogResult.OK Then

                For copy As Integer = 1 To .NumCopies

                    Dim headerFont As New PrinterFont(50, 5, 2) 'variable width. 
                    Using TestPrint As New CPCL.Label(thisForm.Printer, eLabelStyle.receipt)

                        With TestPrint
                            'logo
                            '.AddImage("roddas.pcx", New Point(186, thisForm.Printer.Dimensions.Height + 10), 147)

                            'line
                            .AddLine(New Point(10, thisForm.Printer.Dimensions.Height + 10), _
                                     New Point(thisForm.Printer.Dimensions.Width - 10, thisForm.Printer.Dimensions.Height + 10), 10, 15)

                            'header = 334px wide
                            .AddText( _
                                String.Format( _
                                    "{0} {1}", warhs, bin) _
                                    , New Point((thisForm.Printer.Dimensions.Width / 2) - 86, _
                                    thisForm.Printer.Dimensions.Height + 10), _
                                    headerFont _
                            )

                            'line
                            .AddLine(New Point(10, thisForm.Printer.Dimensions.Height + 10), _
                                     New Point(thisForm.Printer.Dimensions.Width - 10, thisForm.Printer.Dimensions.Height + 10), 10)

                            Dim code As New qrCode
                            With code
                                .Add("WARHS", warhs)
                                .Add("BIN", bin)
                            End With

                            .AddBarcode( _
                                code.Encode, _
                                    New Point( _
                                        (thisForm.Printer.Dimensions.Width / 2) - 223, _
                                        thisForm.Printer.Dimensions.Height + 10 _
                                    ), _
                                180, _
                                Symbology.QRCODE _
                            )

                            .AddBarcode( _
                                String.Format("{0}", warhs.ToUpper), _
                                    New Point( _
                                        (thisForm.Printer.Dimensions.Width / 2) - 223, _
                                        thisForm.Printer.Dimensions.Height + 60 _
                                    ), _
                                25, _
                                Symbology.CODE39 _
                            )

                            .AddBarcode( _
                                String.Format("{0}", bin), _
                                    New Point( _
                                        (thisForm.Printer.Dimensions.Width / 2) - 223, _
                                        thisForm.Printer.Dimensions.Height + 10 _
                                    ), _
                                25, _
                                Symbology.CODE39 _
                            )

                            'tear 'n' print!
                            .AddTearArea(New Point(0, thisForm.Printer.Dimensions.Height))
                            thisForm.Printer.Print(.toByte)

                        End With
                    End Using
                Next

            End If
        End With

    End Sub

    Public Overrides Sub PrintTable(ByRef thisForm As PrioritySFDC.iForm)

        Dim smallFont As New PrinterFont(35, 0, 2)

        Dim fCopies As New frmCopies
        With fCopies
            .ShowDialog()
            If .Result = DialogResult.OK Then


                Dim p As String
                Dim b As String
                Dim d As String
                With thisForm.ViewMain.TableView.ViewTable.SelectedItem

                    p = .Column(":$.PARTNAME")
                    b = .Column(":$.BARCODE")
                    d = .Column(":$.PARTDES")
                End With

                For copy As Integer = 1 To .NumCopies

                    Dim headerFont As New PrinterFont(50, 5, 2) 'variable width. 
                    Using TestPrint As New CPCL.Label(thisForm.Printer, eLabelStyle.receipt)

                        With TestPrint
                            'logo
                            '.AddImage("roddas.pcx", New Point(186, thisForm.Printer.Dimensions.Height + 10), 147)

                            'line
                            .AddLine(New Point(10, thisForm.Printer.Dimensions.Height + 10), _
                                     New Point(thisForm.Printer.Dimensions.Width - 10, thisForm.Printer.Dimensions.Height + 10), 10, 15)

                            'header = 334px wide
                            .AddText(p, New Point((thisForm.Printer.Dimensions.Width / 2) - 86, thisForm.Printer.Dimensions.Height + 10), _
                                     headerFont)

                            'line
                            .AddLine(New Point(10, thisForm.Printer.Dimensions.Height + 10), _
                                     New Point(thisForm.Printer.Dimensions.Width - 10, thisForm.Printer.Dimensions.Height + 10), 10)


                            ' qr code
                            '<in>
                            '  <i n="PART" v="PART123"/>
                            '  <i n="CUST" v="Goods"/>
                            '  <i n="SERIAL" v="123-456-789"/>
                            '  <i n="LOT" v="123-456-789"/>
                            '  <i n="ACT" v="123-456-789"/>
                            '</in>
                            Dim code As New qrCode
                            'code.Add("WARHS", "Main")
                            'code.Add("BIN", "0")
                            code.Add("BARCODE", b)
                            code.Add("CUST", "Goods")
                            code.Add("SERIAL", "123-456-789")
                            code.Add("LOT", "123-456-789")
                            code.Add("ACT", "Release")

                            .AddBarcode( _
                                code.Encode, _
                                    New Point( _
                                        (thisForm.Printer.Dimensions.Width / 2) - 223, _
                                        thisForm.Printer.Dimensions.Height + 10 _
                                    ), _
                                250, _
                                Symbology.QRCODE _
                            )

                            .AddBarcode( _
                                b, _
                                    New Point( _
                                        (thisForm.Printer.Dimensions.Width / 2) - 223, _
                                        thisForm.Printer.Dimensions.Height + 50 _
                                    ), _
                                50, _
                                Symbology.EAN13 _
                            )

                            Dim part As New ReceiptFormatter(64, _
                                  New FormattedColumn(5, 10, eAlignment.Left), _
                                  New FormattedColumn(32, 16, eAlignment.Left) _
                            )

                            'part.AddRow("Part:", p)
                            'part.AddRow("Decription:", d)
                            'part.AddRow("Barcode:", b)

                            'itemised invoice box
                            For Each StrVal In part.FormattedText
                                .AddText(StrVal, New Point(22, thisForm.Printer.Dimensions.Height), smallFont)
                            Next

                            'tear 'n' print!
                            .AddTearArea(New Point(0, thisForm.Printer.Dimensions.Height))
                            thisForm.Printer.Print(.toByte)

                        End With
                    End Using
                Next

            End If
        End With

    End Sub

#End Region

    Public Overrides Sub btn_AddPress(ByRef thisForm As PrioritySFDC.iForm)
        'thisForm.Dialog(New dlgTestDialog, "add")
        With thisForm.ViewMain.TableView
            .TableView = TableView.eTableView.vForm
        End With
    End Sub

    Public Overrides Sub btn_PostPress(ByRef thisForm As PrioritySFDC.iForm)
        thisForm.ViewMain.FormView.ViewForm.SelectColumn("supname")
        thisForm.ViewMain.Focus()
        'MyBase.btn_PostPress(thisForm)
    End Sub

    Public Overrides Sub CloseDialog(ByRef thisForm As PrioritySFDC.iForm, ByRef frmDialog As PrioritySFDC.UserDialog)
        Select Case frmDialog.frmName
            Case "alt"
                thisForm.ViewMain.FormView.ViewForm.SelectColumn("supname")
                thisForm.ViewMain.FormView.ViewForm.FocusedControl.thisColumn.isReadOnly = True
                'thisForm.ViewMain.FormView.Focus()

            Case "add"
                If frmDialog.Result = DialogResult.OK Then
                    MyBase.btn_AddPress(thisForm)
                    'thisForm.ViewMain.FormView.ViewForm.Columns("READONLY").Value = "Test"
                    'Dim data As Data.DataTable = thisForm.DataService.ExecuteReader("")
                End If

        End Select

    End Sub

    Public Overrides Sub AltEntry(ByRef uiCol As PrioritySFDC.uiColumn)
        Select Case uiCol.thisColumn.Name.ToLower
            Case "supname"

                uiCol.ParentForm.Dialog(New dlgTestDialog, "alt")

            Case Else
                MyBase.AltEntry(uiCol)

        End Select

    End Sub

    Public Overrides Function InitCalc(ByRef uiCol As PrioritySFDC.uiColumn) As PrioritySFDC.calcSetting
        Select Case uiCol.thisColumn.Name.ToLower
            Case "tquant"
                Dim cs As calcSetting = MyBase.InitCalc(uiCol)
                cs.Max = 10
                cs.Min = -10
                Return cs
            Case Else
                Return MyBase.InitCalc(uiCol)
        End Select

    End Function

    'Public Overrides Sub Scan2d(ByRef sb As PrioritySFDC.TablePanel)
    '    With sb.ParentForm
    '        Dim c As calcSetting = .Calc(New calcSetting(CInt(0), -10, 100, "Test"))
    '        If c.Result = DialogResult.OK Then

    '        End If
    '    End With
    '    For Each k As String In sb.ScanBuffer.ScanDictionary.Keys

    '    Next
    'End Sub

End Class
