Imports Microsoft.VisualBasic

Public Structure tEnclosure
    Shared ws As New priwebsvc.Service
    Shared sd As New Priority.SerialData

    Dim name As String
    Dim ddl As tFilterView

    Public ItemId() As String
    Public ItemPH() As PlaceHolder

    Private _table As String
    Private _UniqueField As String

    Private _PreHTML As PlaceHolder
    Private _PostHTML As PlaceHolder

    Public Sub New(ByVal Table As String, ByVal UniqueField As String)
        _table = Table
        _UniqueField = UniqueField
    End Sub

    Public Function Count()
        If Not IsNothing(ItemId) Then
            Return UBound(ItemId)
        Else
            Return -1
        End If
    End Function

    Public Sub Create(ByVal Template As tTemplate, ByVal NamePair As tNamePairs, ByVal FilterView As String, ByRef ph As PlaceHolder)

        Dim ret(,) As String = Nothing
        Dim Part(,) As String = sd.DeSerialiseData(ws.GetData( _
            "select DISTINCT " & _UniqueField & " from " & _table & _
            " WHERE 0=0 " & NamePair.ParseSQL(FilterView)))

        If Not IsNothing(Part) Then
            _PreHTML = New PlaceHolder
            ph.Controls.Add(_PreHTML)
            For i As Integer = 0 To UBound(Part, 2)
                Dim phPart As New PlaceHolder
                With phPart
                    .ID = _UniqueField & Part(0, i)
                End With
                ph.Controls.Add(phPart)
                NewItem(Part(0, i), phPart)
            Next
            _PostHTML = New PlaceHolder
            ph.Controls.Add(_PostHTML)
        End If

    End Sub

    Public Sub LoadTemplate(ByVal Template As tTemplate, ByVal DataView As String, ByVal NamePair As tNamePairs)

        Dim AllData(,) As String
        Dim PartData(,) As String
        Dim cols() = ColumnsInTable(DataView)
        ddl = New tFilterView(cols)
        Dim phcount As Integer = 0

        If Not IsNothing(cols) Then
            Dim sql As String = "select "
            For i As Integer = 0 To UBound(cols)
                ddl.FilterSQL(i) = "Select distinct " & cols(i) & " FROM " & DataView & " Where 0=0 AND ("
                sql = sql & cols(i)
                If i < UBound(cols) Then sql = sql & ", "
            Next
            sql = sql & " FROM " & DataView & " Where 0=0 AND ("
            Dim tsql As String = ""
            For i As Integer = 0 To Me.Count
                tsql = tsql & Me._UniqueField & "=" & Me.ItemId(i)
                If i < Me.Count Then tsql = tsql & " OR "
            Next
            sql = sql & tsql & ") "

            AllData = sd.DeSerialiseData(ws.GetData(sql))
            If IsNothing(AllData) Then Exit Sub
            Dim tpnames(,) As String
            ReDim tpnames(1, UBound(AllData, 1))

            For x As Integer = 0 To UBound(AllData, 1)
                tpnames(0, x) = cols(x)
                tpnames(1, x) = AllData(x, 0)
            Next

            ' Parse pre/Post html
            Dim pre As String = Template.PreHTML
            Template.Parser(pre, tpnames, _PreHTML, 0)
            Dim post As String = Template.PostHTML
            Template.Parser(post, tpnames, _PostHTML, 0)
            ' Check the prehtml for filters
            For y As Integer = 0 To UBound(cols)
                Dim Ph_filter As New PlaceHolder
                With ddl
                    .phType(y) = eType.typeNONE

                    Ph_filter = _PreHTML.FindControl("ph_Filter" & cols(y))
                    If Not IsNothing(Ph_filter) Then
                        .phType(y) = eType.typeDDL
                        .Pholder(y) = Ph_filter
                    End If

                    Ph_filter = _PreHTML.FindControl("ph_TextFilter" & cols(y))
                    If Not IsNothing(Ph_filter) Then
                        .phType(y) = eType.typeTEXT
                        .Pholder(y) = Ph_filter
                    End If

                End With
            Next

            sql = sql & NamePair.ParseSQL(DataView)
            For i As Integer = 0 To UBound(cols)
                ddl.FilterSQL(i) = ddl.FilterSQL(i) & tsql & ")"
            Next

            PartData = sd.DeSerialiseData(ws.GetData(sql))
            If Not IsNothing(PartData) And Not IsNothing(ItemId) Then
                If Left(PartData(0, 0), 1) = "!" Then Exit Sub
                Dim ordinal As Integer = -1
                For i As Integer = 0 To UBound(cols)
                    If LCase(Trim(cols(i))) = LCase(Trim(Me._UniqueField)) Then
                        ordinal = i
                        Exit For
                    End If
                Next

                For i As Integer = 0 To UBound(ItemId)
                    For p As Integer = 0 To UBound(PartData, 2)
                        If LCase(Trim(PartData(ordinal, p))) = LCase(Trim(ItemId(i))) Then
                            For x As Integer = 0 To UBound(PartData, 1)
                                tpnames(0, x) = cols(x)
                                tpnames(1, x) = PartData(x, p)
                            Next

                            With ItemPH(i)
                                Dim html As String = Template._html
                                Template.Parser(html, tpnames, ItemPH(i), (phcount Mod 2))
                                phcount = phcount + 1
                            End With

                            Exit For
                        End If
                    Next
                Next
            End If
        End If

    End Sub

    Private Function ColumnsInTable(ByVal Table As String) As String()
        Dim ret() As String = Nothing
        Dim sql As String = "SELECT [name] AS [Columns] " & _
              "FROM syscolumns " & _
              "WHERE [id] = Object_Id('" & Table & "')"
        Dim cols(,) As String = sd.DeSerialiseData(ws.GetData(sql))
        If Not IsNothing(cols) Then
            For i As Integer = 0 To UBound(cols, 2)
                Try
                    ReDim Preserve ret(UBound(ret) + 1)
                Catch ex As Exception
                    ReDim ret(0)
                Finally
                    ret(UBound(ret)) = cols(0, i)
                End Try
            Next
        End If
        Return ret
    End Function

    Private Sub NewItem(ByVal NewItemId As String, ByRef NewItemPH As PlaceHolder)
        Try
            ReDim Preserve ItemId(UBound(ItemId) + 1)
            ReDim Preserve ItemPH(UBound(ItemPH) + 1)
        Catch ex As Exception
            ReDim ItemId(0)
            ReDim ItemPH(0)
        Finally
            ItemId(UBound(ItemId)) = NewItemId
            ItemPH(UBound(ItemPH)) = NewItemPH
        End Try
    End Sub

End Structure

