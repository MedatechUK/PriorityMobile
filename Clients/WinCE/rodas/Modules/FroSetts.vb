Public Class FroSetts
    Private _ptype As Decimal
    Private _rtype As Decimal
    Private _incp As Boolean
    Private _incr As Boolean
    Private _ptemg As Decimal
    Private _ftemg As Decimal
    Private _wrangepf As Decimal
    Private _wrangept As Integer
    Private _rtempg As Decimal
    Private _wrangerf As Decimal
    Private _wrangert As Decimal
    Private _Over As Boolean
    Private _overmins As Integer

    Public Property ptype() As Decimal
        Get
            Return _ptype
        End Get
        Set(ByVal value As Decimal)
            _ptype = value

        End Set
    End Property
    Public Property rtype() As Decimal
        Get
            Return _rtype
        End Get
        Set(ByVal value As Decimal)
            _rtype = value
        End Set
    End Property
    Public Property incp() As Boolean
        Get
            Return _incp
        End Get
        Set(ByVal value As Boolean)
            _incp = value
        End Set
    End Property
    Public Property incr() As Boolean
        Get
            Return _incr
        End Get
        Set(ByVal value As Boolean)
            _incr = value
        End Set
    End Property
    Public Property ptemg() As Decimal
        Get
            Return _ptemg
        End Get
        Set(ByVal value As Decimal)
            _ptemg = value
        End Set
    End Property
    Public Property ftemg() As Decimal
        Get
            Return _ftemg
        End Get
        Set(ByVal value As Decimal)
            _ftemg = value
        End Set
    End Property
    Public Property wrangepf() As Decimal
        Get
            Return _wrangepf
        End Get
        Set(ByVal value As Decimal)
            _wrangepf = value
        End Set
    End Property
    Public Property wrangept() As Decimal
        Get
            Return _wrangept
        End Get
        Set(ByVal value As Decimal)
            _wrangept = value
        End Set
    End Property
    Public Property rtempg() As Decimal
        Get
            Return _rtempg
        End Get
        Set(ByVal value As Decimal)
            _rtempg = value
        End Set
    End Property
    Public Property wrangerf() As Decimal
        Get
            Return _wrangerf
        End Get
        Set(ByVal value As Decimal)
            _wrangerf = value
        End Set
    End Property
    Public Property wrangert() As Decimal
        Get
            Return _wrangert
        End Get
        Set(ByVal value As Decimal)
            _wrangert = value
        End Set
    End Property
    Public Property over() As Boolean
        Get
            Return _Over
        End Get
        Set(ByVal value As Boolean)
            _Over = value
        End Set
    End Property
    Public Property overmins() As Integer
        Get
            Return _overmins
        End Get
        Set(ByVal value As Integer)
            _overmins = value
        End Set
    End Property
    Public Sub New(ByVal pty As Integer, ByVal rt As Integer, ByVal ip As Boolean, ByVal ir As Boolean, ByVal pt As Decimal, ByVal ft As Decimal, ByVal wrf As Decimal, ByVal wrt As Decimal, ByVal rte As Decimal, ByVal wff As Decimal, ByVal wft As Decimal, ByVal o As Boolean, ByVal omin As Integer)
        ptype = pty
        rtype = rt
        incp = ip
        incr = ir
        ptemg = pt
        ftemg = ft
        wrangepf = wrf
        wrangept = wrt
        rtempg = rte
        wrangerf = wff
        wrangert = wft
        over = o
        overmins = omin

    End Sub

End Class
Public Class SendError
    Private _datRow As ListViewItem
    Private _temp As Decimal
    Public Property DatRow() As ListViewItem
        Get
            Return _DatRow
        End Get
        Set(ByVal value As ListViewItem)
            _DatRow = value
        End Set
    End Property
    Public Property Temp() As Decimal
        Get
            Return _temp
        End Get
        Set(ByVal value As Decimal)
            _temp = value
        End Set
    End Property
    Public Sub New(ByVal d As ListViewItem, ByVal t As Decimal)
        DatRow = d
    End Sub
    Public Shared Sub ProcError(ByVal tdate As Integer, ByVal pn As String, ByVal pd As String, ByVal serial As String, ByVal un As String, ByVal tem As Decimal, ByVal fr As String, ByVal fa As String, ByVal ft As String)
        Dim ws As New PriWebSVC.Service
        Dim sd As New ceLoadData.SerialData
        Dim test As New ceLoadData.Load
        With test
            .Clear()
            .NoData = False
           
        End With
        Try
            'INSERT INTO [dbo].[ZEMG_TEMPCHECK] ([TDATE],[PARTNAME],[PARTDES],[SERIAL] ,[USERNAME],[TEMP],[FROZEN])
            With test
                .DebugFlag = False
                .Procedure = "ZEMG_TEMPCHECK"
                .Table = "ZEMG_LOAD_TEMPCHECK"
                .RecordType1 = "TDATE,PARTNAME,PARTDES,SERIAL,USERNAME,TEMP,FROZEN,FAILED,FAIL_TYPE"
                .RecordType2 = "TEMP2,TEMP3"
                .RecordTypes = "TEXT,TEXT,TEXT,TEXT,TEXT,,TEXT,TEXT,TEXT,TEXT,TEXT"
            End With

            Dim t1() As String = { _
            tdate, _
            pn, _
            pd, _
            serial, _
            un, _
            tem, _
            fr, _
            fa, _
            ft _
}
            Dim t2() As String = { _
            pn, _
            pn _
                        }
            test.AddRecord(1) = t1
            With test
                Dim d(,) As String = .Data
                If Not (.NoData) And Not IsNothing(d) Then
                    ws.LoadData(sd.SerialiseDataArray(d))
                Else
                    MsgBox("There was a problem creating the bubble")
                End If
            End With

        Catch ex As Exception
            MsgBox(ex.ToString())
        End Try


        test = Nothing
    End Sub
End Class