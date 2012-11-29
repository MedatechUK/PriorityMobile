Imports Bind
Imports Loading
Imports ntEvtlog

Public Class PartTrigger
    Inherits TriggerBase

    Public Overrides Function TriggerType() As System.Type
        Return GetType(Parts)
    End Function

    Public Overrides Sub PREINSERT(ByRef NewObject As Object, ByRef Cancel As Boolean)
        'Cancel = True
        Dim o As Parts = NewObject
        o.DOCNO = InterfaceValue(":$$.DOCNO")

        Dim ob As oBind = Tables("Warehouse")
        Dim w As Warehouse = ob.Item(o.PARTNAME)
        If o.QTY > w.QTY Then
            MsgBox(String.Format("Insufficient stock available." & vbCrLf & _
            "You have {0} {1} listed in stock.", CStr(w.QTY), w.PARTDES))
            Cancel = True
        Else
            w.QTY = w.QTY - o.QTY
            o.PARTDES = w.PARTDES
            ob.Save()
        End If
        MyBase.PREINSERT(NewObject, Cancel)
    End Sub

    Public Overrides Sub PREUPDATE(ByRef NewObject As Object, ByRef OldObject As Object, ByRef Cancel As Boolean)

        'If Not NewObject = OldObject Then
        Dim np As Parts = NewObject
        Dim op As Parts = OldObject

        Dim ob As oBind = Tables("Warehouse")        

        Dim wop As Warehouse = ob.Item(op.PARTNAME)
        Dim wnp As Warehouse = ob.Item(np.PARTNAME)

        Select Case np.PARTNAME
            Case op.PARTNAME
                If np.QTY > (wnp.QTY + op.QTY) Then
                    MsgBox(String.Format("Insufficient stock available." & vbCrLf & _
                    "You have {0} {1} listed in stock.", CStr(wnp.QTY + op.QTY), wnp.PARTDES))
                    Cancel = True
                    Exit Sub
                End If
            Case Else
                If np.QTY > wnp.QTY Then
                    MsgBox(String.Format("Insufficient stock available." & vbCrLf & _
                    "You have {0} {1} listed in stock.", CStr(wnp.QTY), wnp.PARTDES))
                    Cancel = True
                    Exit Sub
                End If
        End Select

        wop.QTY = wop.QTY + op.QTY
        wnp.QTY = wnp.QTY - np.QTY
        np.PARTDES = wnp.PARTDES
        'InterfaceValue(":$.PARTDES") = wnp.PARTDES

        ob.Save()        
        'End If
    End Sub

    Public Overrides Sub POSTDELETE(ByRef OldObject As Object)
        Dim op As Parts = OldObject
        Dim ob As oBind = Tables("Warehouse")
        Dim wop As Warehouse = ob.Item(op.PARTNAME)
        wop.QTY = wop.QTY + op.QTY
        ob.Save()
    End Sub

    Public Overrides Sub POSTUPDATE(ByRef NewObject As Object, ByRef OldObject As Object)
        'Dim o As CustomerItem = NewObject
        'Dim i As Integer = o.CUST
        'Dim ob As oBind = Tables("CustomerItem")

        'Dim ex As String = InterfaceValue(":$.This")
        ''Dim snt As Boolean = False
        ''Do
        ''    Try
        ''        Dim s As String = _
        ''            svc.SendBubble("D:\Program Files\eMerge-IT\Priority SOAP Service\EXAMPLES\signature.txt", Loading.RunMode.Vector)
        ''        snt = True
        ''    Catch ex As Exception
        ''        MsgBox(ex.Message)
        ''        Log(ex.Message, LogEntryType.Err, EvtLogVerbosity.Normal)
        ''    End Try
        ''Loop Until snt

        'With p
        '    .Clear()
        '    .DebugFlag = True
        '    .Table = "ZSFDC_TEST"
        '    .Procedure = "ZSFDCP_TEST"
        '    .Environment = UseEnvironment
        '    .Constants("user") = "tabula"
        '    .RecordType1 = "USERNAME,CUSTNAME,CURDATE"
        '    .RecordType2 = "LINETWO, TEST"
        '    .RecordTypes = "TEXT,TEXT,,TEXT"

        '    ' Type 1 records
        '    Dim t1() As String = { _
        '                        o.CUSTNAME, _
        '                        "%USER%", _
        '                        "%Date8%" _
        '                        }
        '    p.AddRecord(1) = t1

        '    For Each c As CustomerItem In ob.OriginalList
        '        Dim t2() As String = { _
        '                o.CREATEDDATE, _
        '                "%USER% %date%" _
        '                }
        '        p.AddRecord(2) = t2
        '    Next

        '    'ws.LoadData(p.ToSerial)

        '    'Dim fn As String = ob.FileName(tFilePath.post)
        '    '.ToFile(fn)
        '    'svc.SendBubble(fn, Loading.RunMode.ReQ)
        'End With

    End Sub

    Public Overrides Sub POSTINSERT(ByRef NewObject As Object)

        'Dim o As CustomerItem = NewObject
        'Dim ob As oBind = Tables("CustomerItem")
        'With o
        '    Dim i As Integer = 0
        '    Do
        '        i -= 1
        '    Loop Until ob.ContainsKey(CStr(i)) = -1
        '    .CUST = i
        'End With
    End Sub

End Class
