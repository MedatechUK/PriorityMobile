Imports Microsoft.VisualBasic
Imports System.IO

' Contains structures for use in the enclosure control

Public Enum eType
    typeNONE = 0
    typeDDL = 1
    typeTEXT = 2
End Enum

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

#Region "Session Reference"

    Shared olu As OnlineUsers = Nothing
    Public ReadOnly Property thisSession() As Session
        Get
            If IsNothing(olu) Then
                olu = New OnlineUsers()
            End If
            Return olu.CurrentSession(HttpContext.Current)
        End Get
    End Property

#End Region

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

Public Structure tFilterView

    Dim dummy As String
    Shared ws As New priwebsvc.Service
    Shared sd As New Priority.SerialData

    Private _SQL() As String
    Private _Ph() As PlaceHolder
    Private _Cols() As String
    Private _Type() As eType

#Region "Session Reference"

    Shared olu As OnlineUsers = Nothing
    Public ReadOnly Property thisSession() As Session
        Get
            If IsNothing(olu) Then
                olu = New OnlineUsers()
            End If
            Return olu.CurrentSession(HttpContext.Current)
        End Get
    End Property

#End Region

    Public Property FilterSQL(ByVal Index As Integer) As String
        Get
            Return _SQL(Index)
        End Get
        Set(ByVal value As String)
            _SQL(Index) = value
        End Set
    End Property

    Public Property Pholder(ByVal Index As Integer) As PlaceHolder
        Get
            Return _Ph(Index)
        End Get
        Set(ByVal value As PlaceHolder)
            _Ph(Index) = value
        End Set
    End Property

    Public Property phType(ByVal Index As Integer) As eType
        Get
            Return _Type(Index)
        End Get
        Set(ByVal value As eType)
            _Type(Index) = value
        End Set
    End Property

    Public Sub New(ByVal value() As String)
        If Not IsNothing(value) Then
            ReDim _Ph(UBound(value))
            ReDim _SQL(UBound(value))
            ReDim _Cols(UBound(value))
            ReDim _Type(UBound(value))
            _Cols = value
        Else
            _Cols = Nothing
            _SQL = Nothing
            _Ph = Nothing
            _Type = Nothing
        End If
    End Sub

    Public Sub DrawFilters(ByRef _caller As PriDataView)

        For i As Integer = 0 To UBound(_Cols)
            If Not IsNothing(_Ph(i)) Then
                Dim filter(,) As String = sd.DeSerialiseData(ws.GetData(FilterSQL(i)))
                Select Case _Type(i)
                    Case eType.typeDDL
                        Dim f As Boolean = False
                        If Not IsNothing(filter) Then
                            Dim ddl As New DropDownList
                            With ddl
                                .ID = _Cols(i)
                                .AutoPostBack = True
                                .Items.Add("")
                                For y As Integer = 0 To UBound(filter, 2)
                                    .Items.Add(filter(0, y))
                                    If LCase(Trim(filter(0, y))) = thisSession.NamePairs.NamePair(LCase(Trim(_Cols(i)))) Then
                                        .Items(.Items.Count - 1).Selected = True
                                        f = True
                                    End If
                                Next
                            End With
                            AddHandler ddl.SelectedIndexChanged, AddressOf _caller.hDDLCLick
                            With _Ph(i)
                                .Controls.Add(ddl)
                            End With
                            If Not f Then ddl.SelectedIndex = 0
                        End If

                    Case eType.typeTEXT
                        Dim wid As Integer = 0
                        For y As Integer = 0 To UBound(filter, 2)
                            If Len(filter(0, y)) > wid Then wid = Len(filter(0, y))
                        Next
                        Dim text As New TextBox
                        With text
                            .ID = _Cols(i)
                            .Columns = wid
                            .Text = thisSession.NamePairs.NamePair(_Cols(i))
                        End With
                        Dim btn As New Button
                        With btn
                            .ID = "tf_" & _Cols(i)
                            .Text = ">"
                        End With
                        AddHandler btn.Click, AddressOf _caller.hFilterBtn
                        With _Ph(i).Controls
                            .Add(text)
                            .Add(btn)
                        End With

                End Select
            End If
        Next
    End Sub

End Structure

Public Structure tNamePairs
    ' This type converts the name pair values of the webpage 
    ' to both URLs and SQL statements

    Shared ws As New priwebsvc.Service
    Shared sd As New Priority.SerialData

    Private _NP(,) As String
    Private _debug As Boolean
    Private _ColTypes(,) As String

#Region "Session Reference"

    Shared olu As OnlineUsers = Nothing
    Public ReadOnly Property thisSession() As Session
        Get
            If IsNothing(olu) Then
                olu = New OnlineUsers()
            End If          
            Return olu.CurrentSession(HttpContext.Current)
        End Get
    End Property

#End Region

    Public Sub New(ByVal DebugOn As Boolean)
        ' Load the name pairs
        _debug = DebugOn
        With HttpContext.Current
            For Each item As String In .Request.QueryString
                Dim i As String = .Server.HtmlDecode(.Request(item))
                If Len(Trim(i)) > 0 Then NamePair(item) = Replace(i, "%", "*")
            Next
        End With
    End Sub

    Public Sub Info(ByVal ph As PlaceHolder)
        If _debug Then
            If Not IsNothing(_NP) Then
                With ph
                    .Controls.Add(New LiteralControl("<table>"))
                    For i As Integer = 0 To UBound(_NP, 2)
                        .Controls.Add(New LiteralControl("<tr><td>" & _NP(0, i) & "</td><td>" & _NP(1, i) & "</td></tr>"))
                    Next
                    .Controls.Add(New LiteralControl("</table>"))
                End With
            End If
        End If
    End Sub

    Public Property NamePair(ByVal Name As String) As String
        Get
            If Not IsNothing(_NP) Then
                For i As Integer = 0 To UBound(_NP, 2)
                    If LCase(Trim(Name)) = LCase(Trim(_NP(0, i))) Then
                        Return _NP(1, i)
                    End If
                Next
            End If
            Return Nothing
        End Get
        Set(ByVal value As String)
            If Not IsNothing(_NP) Then
                For i As Integer = 0 To UBound(_NP, 2)
                    If LCase(Trim(Name)) = LCase(Trim(_NP(0, i))) Then
                        _NP(1, i) = value
                        Exit Property
                    End If
                Next
            End If
            'Not found so add
            Try
                ReDim Preserve _NP(1, UBound(_NP, 2) + 1)
            Catch ex As Exception
                ReDim _NP(1, 0)
            Finally
                _NP(0, UBound(_NP, 2)) = Name
                _NP(1, UBound(_NP, 2)) = value
            End Try
        End Set
    End Property

    Public Function Names() As String()
        Dim ret() As String = Nothing
        If Not IsNothing(_NP) Then
            For i As Integer = 0 To UBound(_NP, 2)
                Try
                    ReDim Preserve ret(UBound(ret) + 1)
                Catch ex As Exception
                    ReDim ret(0)
                Finally
                    ret(UBound(ret)) = _NP(0, i)
                End Try
            Next
        End If
        Return ret
    End Function

    Public Function ParseURL(ByVal BaseURL As String, ByVal Name As String, ByVal Value As String)
        Dim ret As String = ""
        Dim f As Boolean = False
        If Not IsNothing(_NP) Then
            For i As Integer = 0 To UBound(_NP, 2)
                If LCase(Trim(Name)) = LCase(Trim(_NP(0, i))) Then
                    ret = ret & "&" & _NP(0, i) & "=" & Value
                    f = True
                Else
                    ret = ret & "&" & _NP(0, i) & "=" & _NP(1, i)
                End If
            Next
        End If
        If Not f Then
            ret = ret & "&" & Name & "=" & Value
        End If
        Dim x As Integer = InStr(ret, "&")
        ret = BaseURL & "?" & Right(ret, Len(ret) - 1)
        Return ret
    End Function

    Public Function ParseSQL(ByVal View As String)
        Dim ret As String = " "
        Dim Deliminator As String = ""
        Dim op As String
        Dim ValidNames() As String = NamesInFilter(View)
        If Not IsNothing(ValidNames) Then
            loadColTypes(View)
            For i As Integer = 0 To UBound(ValidNames)
                Dim c As String = ColType(ValidNames(i))
                Select Case c
                    Case "nvarchar", "varchar", "char"
                        Deliminator = "'"
                    Case Else
                        Deliminator = ""
                End Select
                Select Case InStr(NamePair(ValidNames(i)), "*")
                    Case Is > 0
                        op = "LIKE"
                        NamePair(ValidNames(i)) = Replace(NamePair(ValidNames(i)), "*", "%")
                    Case Else
                        op = "="
                End Select
                ret = ret & " AND " & UCase(ValidNames(i)) & " " & op & " " & Deliminator & NamePair(ValidNames(i)) & Deliminator
            Next
        End If
        Return ret
    End Function

    Sub loadColTypes(ByVal Table As String)
        ' Get datatypes for columns in view 
        Dim sql As String = "select COLUMN_NAME, DATA_TYPE from INFORMATION_SCHEMA.COLUMNS " & _
                            "where TABLE_NAME = '" & Table & "'"
        ' Select into local array
        _ColTypes = sd.DeSerialiseData(ws.GetData(sql))
    End Sub

    Private Function ColType(ByVal Column As String) As String
        Dim ret As String = ""
        If Not IsNothing(_ColTypes) Then
            For i As Integer = 0 To UBound(_ColTypes, 2)
                If LCase(Trim(_ColTypes(0, i))) = LCase(Trim(Column)) Then
                    ret = LCase(Trim(_ColTypes(1, i)))
                    Exit For
                End If
            Next
        End If
        Return ret
    End Function

    Private Function NamesInFilter(ByVal View As String)
        ' Prevent the enclosure from requesting non-extant names from the view
        Dim ret() As String = Nothing
        Dim Name() As String = ColumnsInTable(View)
        If Not IsNothing(Name) Then
            For i As Integer = 0 To UBound(Name)
                If Not IsNothing(_NP) Then
                    For x As Integer = 0 To UBound(_NP, 2)
                        If LCase(Trim(_NP(0, x))) = LCase(Trim(Name(i))) Then
                            Try
                                ReDim Preserve ret(UBound(ret) + 1)
                            Catch
                                ReDim ret(0)
                            Finally
                                ret(UBound(ret)) = _NP(0, x)
                            End Try
                        End If
                    Next
                End If
            Next
        End If
        Return ret
    End Function

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

End Structure

Public Structure tTemplate

    Private _Path As String
    Private _TemplateName As String
    Public _html As String
    Public PreHTML As String
    Public PostHTML As String

    Public Sub New(ByVal ipage As Page, ByVal Template As String)

        _html = ""
        _Path = ipage.Server.MapPath("/HTMLTemplates/" & Template)
        If File.Exists(_Path) Then
            Dim sr As New StreamReader(_Path)
            With sr
                _html = .ReadToEnd
                .Close()
            End With
        End If

        If InStr(_html, "$REPEAT$") > 0 Then
            Dim tmp() As String = Split(_html, "$REPEAT$", , CompareMethod.Text)
            PreHTML = tmp(0)
            _html = tmp(1)
        End If

        If InStr(_html, "$/REPEAT$") > 0 Then
            Dim tmp() As String = Split(_html, "$/REPEAT$", , CompareMethod.Text)
            PostHTML = tmp(1)
            _html = tmp(0)
        End If

    End Sub

    Public Sub Parser(ByRef Html As String, ByVal NamePair(,) As String, ByRef ph As PlaceHolder, ByVal MOD2 As Integer)

        For i As Integer = 0 To UBound(NamePair, 2)
            Html = Replace(Html, "%" & NamePair(0, i) & "%", NamePair(1, i), , , CompareMethod.Text)
        Next
        Html = Replace(Html, "%MOD2%", MOD2, , , CompareMethod.Text)
        With ph
            Dim htmlpart() As String = getHTMLPart(Html)
            Dim i As Integer = 0

            While i <= UBound(htmlpart)
                If i Mod 2 = 0 Then
                    If Len(Trim(htmlpart(i))) > 0 Then
                        .Controls.Add(New LiteralControl(htmlpart(i)))
                    End If
                    i = i + 1
                Else
                    If Len(Trim(htmlpart(i))) > 0 Then
                        Dim cmd() As String = Split(Trim(htmlpart(i)), " ")
                        Dim str As String = Trim(Right(Trim(htmlpart(i)), Len(Trim(htmlpart(i))) - (Len(cmd(0)))))

                        ' Perform as script command
                        Select Case LCase(Trim(cmd(0)))
                            Case "if"
                                Dim haselse As Boolean = _
                                    CBool(LCase(Trim(Split(Trim(htmlpart(i + 2)), " ")(0))) = "else")
                                If Evaluate(str, NamePair) Then
                                    .Controls.Add(New LiteralControl(htmlpart(i + 1)))
                                Else
                                    If haselse Then
                                        .Controls.Add(New LiteralControl(htmlpart(i + 3)))
                                    End If
                                End If

                                If haselse Then
                                    i = i + 5
                                Else
                                    i = i + 3
                                End If

                            Case "ph"
                                Dim tph As New PlaceHolder
                                tph.ID = str
                                .Controls.Add(tph)
                                i = i + 1

                        End Select
                    Else
                        i = i + 1
                    End If
                End If
            End While

        End With

    End Sub

    Private Function getHTMLPart(ByVal html) As String()

        Dim ret() As String = Nothing

        If Not InStr(html, "<%", CompareMethod.Text) > 0 Then
            newpart(ret, html)
        Else
            ' Has script
            While InStr(html, "<%", CompareMethod.Text) > 0
                Dim t() As String
                t = Split(html, "<%", , CompareMethod.Text)
                newpart(ret, t(0))
                html = Right(html, Len(html) - (Len(t(0)) + 2))
                t = Split(html, "%>", , CompareMethod.Text)
                newpart(ret, t(0))
                html = Right(html, Len(html) - (Len(t(0)) + 2))
            End While
            newpart(ret, html)
        End If
        Return ret

    End Function

    Private Sub newpart(ByRef Ar() As String, ByVal item As String)
        Try
            ReDim Preserve Ar(UBound(Ar) + 1)
        Catch ex As Exception
            ReDim Ar(0)
        Finally
            Ar(UBound(Ar)) = item
        End Try
    End Sub

    Private Function Evaluate(ByRef Condition As String, ByVal NamePair(,) As String) As Boolean

        Dim ret As Boolean = False
        Dim c() As String = Split(LCase(Condition), "and")
        Dim barray(UBound(c)) As Boolean

        Dim name As String = ""
        Dim value As String = ""
        Dim compare As Integer = 0

        For i As Integer = 0 To UBound(c)
            If InStr(c(i), "=") > 0 Then
                name = Split(c(i), "=")(0)
                value = Split(c(i), "=")(1)
                compare = 1
            ElseIf InStr(c(i), "<>") > 0 Then
                name = Split(c(i), "<>")(0)
                value = Split(c(i), "<>")(1)
                compare = 2
            End If
            For y As Integer = 0 To UBound(NamePair, 2)
                name = LCase(Trim(Replace(name, NamePair(0, y), NamePair(1, y), , , CompareMethod.Text)))
                value = LCase(Trim(Replace(value, NamePair(0, y), NamePair(1, y), , , CompareMethod.Text)))
            Next
            Select Case compare
                Case 1
                    barray(i) = CBool(name = value)
                Case 2
                    barray(i) = Not (CBool(name = value))
            End Select
        Next

        For i As Integer = 0 To UBound(barray)
            If Not barray(i) Then
                Return False
            End If
        Next
        Return True
    End Function

End Structure

