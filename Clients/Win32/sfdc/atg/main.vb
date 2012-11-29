Imports System.Threading

Module main

    Public rss(99) As w32SFDCData.iForm

    Public Enum o
        DOTX = 0
        GRVITEM = 1
        GRVBOX = 2
        PARTLU = 3
        STKCNT = 4
        UNTX = 5
        IWHTX = 6
        PAWHTX = 7
        CHSTWHTX = 8
        PSLIP = 9
        BSTKCNT = 10
        UNCNT = 11
        DOCNT = 12
        INVENLOC = 13
        INVENPART = 14
        INVENCYCA = 15
        INVENCYCB = 16
        INVENCYCC = 17

    End Enum

    Public Enum tRegExValidation
        tBarcode = 0
        tWarehouse = 1
        tLocname = 2
        tStatus = 3
        tNumeric = 4
        tGRV = 5
        tSerial = 6
        tWhsTX = 7
    End Enum

    Public ReadOnly Property ValidStr(ByVal rxType As tRegExValidation) As String
        Get
            Dim ret As String = Nothing
            Select Case rxType
                Case tRegExValidation.tBarcode
                    ret = "^[A-Z0-9]{13}$"
                Case tRegExValidation.tWarehouse
                    ret = "^[a-zA-Z0-9]{1}[a-zA-Z0-9]?[a-zA-Z0-9]?[a-zA-Z0-9]?$"
                Case tRegExValidation.tLocname
                    ret = "^[A-Z0-9]+$"
                Case tRegExValidation.tStatus
                    ret = "^[a-zA-Z0-9]+$"
                Case tRegExValidation.tNumeric
                    ret = "^[0-9.]+$"
                Case tRegExValidation.tGRV
                    ret = "^(PO)|(MPR)[0-9]+$"
                Case tRegExValidation.tSerial
                    ret = "^[A-Z]+[0-9]+$"
                Case tRegExValidation.tWhsTX
                    ret = "^WT[0-9]+$"
            End Select
            Return ret
        End Get
    End Property

    Sub MAIN()

        Dim frmMenu As New frmMenu
        With frmMenu
            ' Set the menu item to add modules
            .ModuleMenu = .MenuItem1

            ' Create the required Modules
            .AddRSS(o.GRVBOX, New InterfaceGRV(frmMenu))
            With rss(o.GRVBOX)
                .Argument("SCANACTION") = "OPENFORM"
                .ModuleName = "Receive Boxes"
                .SubMenu = "Goods Receipt"
                .SetBaseForm(frmMenu)
            End With

            .AddRSS(o.GRVITEM, New InterfaceGRV(frmMenu))
            With rss(o.GRVITEM)
                .Argument("SCANACTION") = "INCREMENT"
                .ModuleName = "Receive Items"
                .SubMenu = "Goods Receipt"
                .SetBaseForm(frmMenu)
            End With

            .AddRSS(o.IWHTX, New InterfaceWHTX(frmMenu))
            With rss(o.IWHTX)
                .Argument("TXTYPE") = "INTERWHTX"
                .ModuleName = "Inter-Warehouse Transfer"
                .SubMenu = "Stock Transfer"
                .SetBaseForm(frmMenu)
            End With

            .AddRSS(o.PAWHTX, New InterfaceWHTX(frmMenu))
            With rss(o.PAWHTX)
                .Argument("TXTYPE") = "PUTAWAY"
                .ModuleName = "Stock Put Away"
                .SubMenu = "Stock Transfer"
                .SetBaseForm(frmMenu)
            End With

            .AddRSS(o.CHSTWHTX, New InterfaceWHTX(frmMenu))
            With rss(o.CHSTWHTX)
                .Argument("TXTYPE") = "CHSTATUS"
                .ModuleName = "Change Status"
                .SubMenu = "Stock Transfer"
                .SetBaseForm(frmMenu)
            End With

            .AddRSS(o.PARTLU, New InterfacePARTLU(frmMenu))
            With rss(o.PARTLU)
                .ModuleName = "By Part"
                .Argument("BARCODE") = ""
                .SubMenu = "Inventory Count"
                .SetBaseForm(frmMenu)
            End With

            .AddRSS(o.INVENCYCA, New InterfacePARTLU(frmMenu))
            With rss(o.INVENCYCA)
                .Argument("BARCODE") = ""
                .Argument("SHOWBAL") = "TRUE"
                .Argument("ABC") = "A"
                .ModuleName = "Cycle Count A Parts"
                .SubMenu = "Inventory Count"
                .SetBaseForm(frmMenu)
            End With

            .AddRSS(o.INVENCYCB, New InterfacePARTLU(frmMenu))
            With rss(o.INVENCYCB)
                .Argument("BARCODE") = ""
                .Argument("SHOWBAL") = "TRUE"
                .Argument("ABC") = "B"
                .ModuleName = "Cycle Count B Parts"
                .SubMenu = "Inventory Count"
                .SetBaseForm(frmMenu)
            End With

            .AddRSS(o.INVENCYCC, New InterfacePARTLU(frmMenu))
            With rss(o.INVENCYCC)
                .Argument("BARCODE") = ""
                .Argument("SHOWBAL") = "TRUE"
                .Argument("ABC") = "C"
                .ModuleName = "Cycle Count C Parts"
                .SubMenu = "Inventory Count"
                .SetBaseForm(frmMenu)
            End With

            .AddRSS(o.INVENLOC, New InterfaceSTKCNT(frmMenu))
            With rss(o.INVENLOC)
                .Argument("DOCNO") = ""
                .Argument("SHOWBAL") = "TRUE"
                .Argument("COUNTBY") = "LOC"
                .ModuleName = "By Location"
                .SubMenu = "Inventory Count"
                .SetBaseForm(frmMenu)
            End With

            '.AddRSS(o.BSTKCNT, New InterfaceSTKCNT(frmMenu))
            'With rss(o.BSTKCNT)
            '    .Argument("SHOWBAL") = "FALSE"
            '    .ModuleName = "Blind Count"
            '    .SubMenu = "Inventory Count"
            '    .SetBaseForm(frmMenu)
            'End With

            '.AddRSS(o.UNCNT, New InterfaceUNCNT(frmMenu))
            'With rss(o.UNCNT)
            '    .ModuleName = "Pending Counts"
            '    .SubMenu = "Inventory Count"
            '    .SetBaseForm(frmMenu)
            'End With

            .AddRSS(o.PSLIP, New interfacePSLIP(frmMenu))
            With rss(o.PSLIP)
                .ModuleName = "Packing Slips"
                .SubMenu = "Packing Slips"
                .SetBaseForm(frmMenu)
            End With

            '.AddRSS(o.DOCNT, New interfaceDOCNT(frmMenu))
            'With rss(o.DOCNT)
            '    .Argument("CNTNO") = ""
            '    .ModuleName = "Complete Count"
            '    .ShowOnMenu = False
            '    .SetBaseForm(frmMenu)
            'End With

            ' Run the program
            .MainLoop()

            ' Clean up
            For i As Integer = 0 To UBound(rss)
                If Not IsNothing(rss(i)) Then
                    rss(i).Close()
                    rss(i) = Nothing
                End If
            Next
            .Close()
        End With

    End Sub

    Public Function Get_app_path() As String
        Dim fullPath As String = System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase
        Return fullPath.Substring(0, fullPath.LastIndexOf("\"))
    End Function

End Module
