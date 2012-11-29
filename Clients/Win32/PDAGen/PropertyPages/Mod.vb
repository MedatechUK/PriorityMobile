Imports bind
Imports dataclasses
Module staticRS

    Private _rs As oDataSet
    Public Sub SetRS(ByRef rs As oDataSet)
        _rs = rs
    End Sub
    Public Function rs() As oDataSet
        Return _rs
    End Function

    Private _fld As InterfaceItem
    Public Sub SetFld(ByRef Fld As InterfaceItem)
        _fld = Fld
    End Sub
    Public Function fld() As InterfaceItem
        Return _fld
    End Function

    Private _frm As FormItem
    Public Sub SetFrm(ByRef Frm As FormItem)
        _frm = Frm
    End Sub
    Public Function SetFrm() As FormItem
        Return _frm
    End Function
End Module
