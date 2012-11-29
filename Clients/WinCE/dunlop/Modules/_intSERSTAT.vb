Imports System
Imports System.IO
Imports System.Threading
Imports System.Text.RegularExpressions

Public Class InterfaceSERSTAT
    Inherits SFDCData.iForm


#Region "Initialisation"

    Public Sub New(Optional ByRef App As Form = Nothing)

        InitializeComponent()
        CallerApp = App
        CtrlTable.DisableButtons(True, True, True, True, False)
        CtrlTable.EnableToolbar(False, False, False, False, False)

    End Sub

#End Region

    Public Enum tSendType
        NextPS = 0
        TableScan = 1
    End Enum
    Dim SendType As tSendType = tSendType.NextPS

    Public Overrides Sub FormLoaded()
        MyBase.FormLoaded()
        'SendType = tSendType.NextPS
        'InvokeData("select dbo.NEXTPS('" & UserName & "')")
    End Sub

    Public Overrides Sub FormSettings()

        ' BARCODE
        With field
            .Name = "SERIALNAME"
            .Title = "Works Order"
            .AltEntry = ctrlText.tAltCtrlStyle.ctNone 'ctKeyb 
            .ValidExp = "^.+$"
            ' Second field replaces first field if first field validates ok
            .SQLValidation = "SELECT '%ME%' " & _
                            "FROM SERIAL, SERIALA, SERIALSTATUS " & _
                            "where SERIAL.CLOSED = '' " & _
                            "AND SERIAL.SERIAL = SERIALA.SERIAL " & _
                            "AND SERIALSTATUS.SERIALSTATUS = SERIALA.SERIALSTATUS " & _
                            "AND SERIAL.RELEASE = 'Y' " & _
                            "AND SERIAL.SERIALNAME = '%ME%' " & _
                            "AND SERIALSTATUS.SERIALSTATUSDES = 'Printed'"
            .DefaultFromCtrl = Nothing '
            .ctrlEnabled = True            
        End With
        CtrlForm.AddField(field)

    End Sub

    Public Overrides Sub TableSettings()

        '' BARCODE
        'With col
        '    .Name = "_SERIALNAME"
        '    .Title = "W/O"
        '    .initWidth = 45
        '    .TextAlign = HorizontalAlignment.Left
        '    .AltEntry = ctrlText.tAltCtrlStyle.ctNone 'ctKeyb             
        '    .DefaultFromCtrl = Nothing '
        '    .ctrlEnabled = False
        '    .Mandatory = False            
        'End With
        'CtrlTable.AddCol(col)

        ' TQUANT
        With col
            .Name = "_STATUS"
            .Title = "Status"
            .initWidth = 30
            .TextAlign = HorizontalAlignment.Left
            .AltEntry = ctrlText.tAltCtrlStyle.ctList 'ctKeyb 
            .ValidExp = ValidStr(tRegExValidation.tStatus)
            .SQLList = "SELECT SERIALSTATUSDES FROM SERIALSTATUS " & _
                        "WHERE SERIALSTATUS <> 0"
            .SQLValidation = "SELECT %ME% " & _
                             "FROM SERIALSTATUS " & _
                             "WHERE SERIALSTATUS <> 0"
            .DefaultFromCtrl = Nothing
            .ctrlEnabled = True
            .Mandatory = True
            .MandatoryOnPost = True
        End With
        CtrlTable.AddCol(col)

        With col
            .Name = "_SERIALDES"
            .Title = "Part"
            .initWidth = 60
            .TextAlign = HorizontalAlignment.Left
            .AltEntry = ctrlText.tAltCtrlStyle.ctNone 'ctKeyb             
            .DefaultFromCtrl = Nothing '
            .ctrlEnabled = False
            .Mandatory = False
        End With
        CtrlTable.AddCol(col)

        ' TQUANT
        With col
            .Name = "_QUANT"
            .Title = "QTY"
            .initWidth = 30
            .TextAlign = HorizontalAlignment.Left
            .AltEntry = ctrlText.tAltCtrlStyle.ctNone 'ctKeyb 
            .DefaultFromCtrl = Nothing
            .ctrlEnabled = False
            .Mandatory = False
            .MandatoryOnPost = False
        End With
        CtrlTable.AddCol(col)

        With col
            .Name = "_BEGINDATE"
            .Title = "Begin"
            .initWidth = 45
            .TextAlign = HorizontalAlignment.Left
            .AltEntry = ctrlText.tAltCtrlStyle.ctNone 'ctKeyb             
            .DefaultFromCtrl = Nothing '
            .ctrlEnabled = False
            .Mandatory = False
        End With
        CtrlTable.AddCol(col)

        ' Set the query to load recordtype 2s
        CtrlTable.RecordsSQL = "SELECT SERIALSTATUS.SERIALSTATUSDES as STATUS, SERIALDES, QUANT/1000 AS QUANT,  CONVERT(VARCHAR(8), dbo.MINTODATE(PSDATE), 3) AS BEGINDATE " & _
                                "FROM SERIAL, PART, SERIALA, SERIALSTATUS " & _
                                "WHERE PART.PART = SERIAL.PART " & _
                                "AND SERIAL.SERIAL = SERIALA.SERIAL " & _
                                "AND SERIALSTATUS.SERIALSTATUS = SERIALA.SERIALSTATUS " & _
                                "AND SERIAL.CLOSED = '' " & _
                                "AND SERIAL.RELEASE = 'Y' " & _
                                "AND SERIAL.SERIALNAME = '%SERIALNAME%'"

    End Sub

    Public Overrides Sub TableRXData(ByVal Data(,) As String)

        Try
            For y As Integer = 0 To UBound(Data, 2)
                Dim lvi As New ListViewItem
                With CtrlTable.Table
                    .Items.Add(lvi)
                    .Items(.Items.Count - 1).Text = Data(0, y)
                    .Items(.Items.Count - 1).SubItems.Add(Data(1, y))
                    .Items(.Items.Count - 1).SubItems.Add(Data(2, y))
                    .Items(.Items.Count - 1).SubItems.Add(Data(3, y))                                       
                End With
            Next
        Catch e As Exception
            MsgBox(e.Message)
        End Try

    End Sub

    Public Overrides Sub BeginAdd()
        CtrlTable.mCol(0).ctrlEnabled = False
        CtrlTable.mCol(1).ctrlEnabled = True
        CtrlTable.mCol(2).ctrlEnabled = False
        CtrlTable.mCol(3).ctrlEnabled = True
    End Sub

    Public Overrides Sub BeginEdit()
        CtrlTable.mCol(0).ctrlEnabled = False
        CtrlTable.mCol(1).ctrlEnabled = True
        CtrlTable.mCol(2).ctrlEnabled = False
        CtrlTable.mCol(3).ctrlEnabled = False
    End Sub

    Public Overrides Sub AfterEdit(ByVal TableIndex As Integer)

    End Sub

    Public Overrides Sub ProcessEntry(ByVal ctrl As SFDCData.ctrlText, ByVal Valid As Boolean)
        Select Case Valid
            Case False
                Beep()

            Case True
                Try
                    If ctrl.Data <> "" Then
                        Select Case ctrl.Name
                            Case "SERIALNAME"
                                CtrlTable.BeginLoadRS()
                        End Select
                    End If

                    ' *******************************************************************
                    ' *** Set which controls are enabled

                    'CtrlForm.el(0).CtrlEnabled = Not (Len(CtrlForm.el(0).Data) > 0)
                    'CtrlTable.el(1).CtrlEnabled = Len(CtrlTable.el(0).Data) > 0

                    ' *******************************************************************
                Catch

                End Try

        End Select
    End Sub

    Public Overrides Function VerifyForm() As Boolean
        Return True
    End Function

    Public Overrides Sub ProcessForm()
        Try
            With p
                .DebugFlag = False
                .Procedure = "ZSFDCP_LOAD_SERSTAT"
                .Table = "ZSFDC_LOAD_SERSTAT"
                .RecordType1 = "SERIALNAME,SERIALSTATUSDES"
                .RecordType2 = "LINETWO"
                .RecordTypes = "TEXT,TEXT,TEXT"
            End With

            ' Type 1 records
            Dim t1() As String = { _
                                CtrlForm.ItemValue("SERIALNAME"), _
                                "Picked" _
                                }
            p.AddRecord(1) = t1

            Dim t2() As String = { _
                    "Dummy" _
                    }
            p.AddRecord(2) = t2

        Catch e As Exception
            MsgBox(e.Message)
        End Try
    End Sub

    Public Overrides Sub TableScan(ByVal Value As String)

    End Sub

    Public Overrides Sub EndInvokeData(ByVal Data(,) As String)

        Select Case SendType

        End Select

    End Sub

    Public Overrides Sub AfterAdd(ByVal TableIndex As Integer)

    End Sub

End Class
