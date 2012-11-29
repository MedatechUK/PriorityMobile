Imports System.Threading

Module main

    Public Const KEYEVENTF_KEYUP As Integer = &H2
    Public Const KEYEVENTF_KEYDOWN As Integer = &H0

    Public Const VK_LBUTTON = &H1
    Public Const VK_RBUTTON = &H2
    Public Const VK_CANCEL = &H3
    Public Const VK_MBUTTON = &H4
    Public Const VK_BACK = &H8
    Public Const VK_TAB = &H9
    Public Const VK_CLEAR = &HC
    Public Const VK_RETURN = &HD
    Public Const VK_SHIFT = &H10
    Public Const VK_CONTROL = &H11
    Public Const VK_MENU = &H12
    Public Const VK_PAUSE = &H13
    Public Const VK_CAPITAL = &H14
    Public Const VK_ESCAPE = &H1B
    Public Const VK_SPACE = &H20
    Public Const VK_PRIOR = &H21
    Public Const VK_NEXT = &H22
    Public Const VK_END = &H23
    Public Const VK_HOME = &H24
    Public Const VK_LEFT = &H25
    Public Const VK_UP = &H26
    Public Const VK_RIGHT = &H27
    Public Const VK_DOWN = &H28
    Public Const VK_SELECT = &H29
    Public Const VK_PRINT = &H2A
    Public Const VK_EXECUTE = &H2B
    Public Const VK_SNAPSHOT = &H2C
    Public Const VK_INSERT = &H2D
    Public Const VK_DELETE = &H2E
    Public Const VK_HELP = &H2F
    Public Const VK_0 = &H30
    Public Const VK_1 = &H31
    Public Const VK_2 = &H32
    Public Const VK_3 = &H33
    Public Const VK_4 = &H34
    Public Const VK_5 = &H35
    Public Const VK_6 = &H36
    Public Const VK_7 = &H37
    Public Const VK_8 = &H38
    Public Const VK_9 = &H39
    Public Const VK_A = &H41
    Public Const VK_B = &H42
    Public Const VK_C = &H43
    Public Const VK_D = &H44
    Public Const VK_E = &H45
    Public Const VK_F = &H46
    Public Const VK_G = &H47
    Public Const VK_H = &H48
    Public Const VK_I = &H49
    Public Const VK_J = &H4A
    Public Const VK_K = &H4B
    Public Const VK_L = &H4C
    Public Const VK_M = &H4D
    Public Const VK_N = &H4E
    Public Const VK_O = &H4F
    Public Const VK_P = &H50
    Public Const VK_Q = &H51
    Public Const VK_R = &H52
    Public Const VK_S = &H53
    Public Const VK_T = &H54
    Public Const VK_U = &H55
    Public Const VK_V = &H56
    Public Const VK_W = &H57
    Public Const VK_X = &H58
    Public Const VK_Y = &H59
    Public Const VK_Z = &H5A
    Public Const VK_STARTKEY = &H5B
    Public Const VK_CONTEXTKEY = &H5D
    Public Const VK_NUMPAD0 = &H60
    Public Const VK_NUMPAD1 = &H61
    Public Const VK_NUMPAD2 = &H62
    Public Const VK_NUMPAD3 = &H63
    Public Const VK_NUMPAD4 = &H64
    Public Const VK_NUMPAD5 = &H65
    Public Const VK_NUMPAD6 = &H66
    Public Const VK_NUMPAD7 = &H67
    Public Const VK_NUMPAD8 = &H68
    Public Const VK_NUMPAD9 = &H69
    Public Const VK_MULTIPLY = &H6A
    Public Const VK_ADD = &H6B
    Public Const VK_SEPARATOR = &H6C
    Public Const VK_SUBTRACT = &H6D
    Public Const VK_DECIMAL = &H6E
    Public Const VK_DIVIDE = &H6F
    Public Const VK_F1 = &H70
    Public Const VK_F2 = &H71
    Public Const VK_F3 = &H72
    Public Const VK_F4 = &H73
    Public Const VK_F5 = &H74
    Public Const VK_F6 = &H75
    Public Const VK_F7 = &H76
    Public Const VK_F8 = &H77
    Public Const VK_F9 = &H78
    Public Const VK_F10 = &H79
    Public Const VK_F11 = &H7A
    Public Const VK_F12 = &H7B
    Public Const VK_F13 = &H7C
    Public Const VK_F14 = &H7D
    Public Const VK_F15 = &H7E
    Public Const VK_F16 = &H7F
    Public Const VK_F17 = &H80
    Public Const VK_F18 = &H81
    Public Const VK_F19 = &H82
    Public Const VK_F20 = &H83
    Public Const VK_F21 = &H84
    Public Const VK_F22 = &H85
    Public Const VK_F23 = &H86
    Public Const VK_F24 = &H87
    Public Const VK_NUMLOCK = &H90
    Public Const VK_OEM_SCROLL = &H91
    Public Const VK_OEM_1 = &HBA
    Public Const VK_OEM_PLUS = &HBB
    Public Const VK_OEM_COMMA = &HBC
    Public Const VK_OEM_MINUS = &HBD
    Public Const VK_OEM_PERIOD = &HBE
    Public Const VK_OEM_2 = &HBF
    Public Const VK_OEM_3 = &HC0
    Public Const VK_OEM_4 = &HDB
    Public Const VK_OEM_5 = &HDC
    Public Const VK_OEM_6 = &HDD
    Public Const VK_OEM_7 = &HDE
    Public Const VK_OEM_8 = &HDF
    Public Const VK_ICO_F17 = &HE0
    Public Const VK_ICO_F18 = &HE1
    Public Const VK_OEM102 = &HE2
    Public Const VK_ICO_HELP = &HE3
    Public Const VK_ICO_00 = &HE4
    Public Const VK_ICO_CLEAR = &HE6
    Public Const VK_OEM_RESET = &HE9
    Public Const VK_OEM_JUMP = &HEA
    Public Const VK_OEM_PA1 = &HEB
    Public Const VK_OEM_PA2 = &HEC
    Public Const VK_OEM_PA3 = &HED
    Public Const VK_OEM_WSCTRL = &HEE
    Public Const VK_OEM_CUSEL = &HEF
    Public Const VK_OEM_ATTN = &HF0
    Public Const VK_OEM_FINNISH = &HF1
    Public Const VK_OEM_COPY = &HF2
    Public Const VK_OEM_AUTO = &HF3
    Public Const VK_OEM_ENLW = &HF4
    Public Const VK_OEM_BACKTAB = &HF5
    Public Const VK_ATTN = &HF6
    Public Const VK_CRSEL = &HF7
    Public Const VK_EXSEL = &HF8
    Public Const VK_EREOF = &HF9
    Public Const VK_PLAY = &HFA
    Public Const VK_ZOOM = &HFB
    Public Const VK_NONAME = &HFC
    Public Const VK_PA1 = &HFD
    Public Const VK_OEM_CLEAR = &HFE

    Public rss(99) As SFDCData.iForm

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
        PRODREP = 19
        ISSUEKIT = 20
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
        tWO = 8
        tString = 9
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
                Case tRegExValidation.tWhsTX
                    ret = "^WT[0-9]+$"
                Case tRegExValidation.tString
                    ret = "^[0-9A-Za-z]+$"
                Case tRegExValidation.tSerial
                    ret = "^[A-Z]+[0-9]+$"
                Case tRegExValidation.tWO
                    ret = "^[0-9]{10}+$|^LOT[0-9]{7}$"
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

            '.AddRSS(o.PSLIP, New interfacePSLIP(frmMenu))
            'With rss(o.PSLIP)
            '    .ModuleName = "Packing Slips"
            '    .SubMenu = "Packing Slips"
            '    .SetBaseForm(frmMenu)
            'End With

            '.AddRSS(o.DOCNT, New interfaceDOCNT(frmMenu))
            'With rss(o.DOCNT)
            '    .Argument("CNTNO") = ""
            '    .ModuleName = "Complete Count"
            '    .ShowOnMenu = False
            '    .SetBaseForm(frmMenu)
            'End With

            .AddRSS(o.PRODREP, New interfacePRODREP(frmMenu))
            With rss(o.PRODREP)
                '    .Argument("CNTNO") = ""
                .ModuleName = "Report Production"
                .SubMenu = "Work Orders"
                .ShowOnMenu = True
                .SetBaseForm(frmMenu)
            End With

            .AddRSS(o.ISSUEKIT, New interfaceKITISSUE(frmMenu))
            With rss(o.ISSUEKIT)
                '    .Argument("CNTNO") = ""
                .ModuleName = "Issue Kit"
                .SubMenu = "Work Orders"
                .ShowOnMenu = True
                .SetBaseForm(frmMenu)
            End With

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
