' This type converts the name pair values of the webpage 
' to both URLs and SQL statements
Imports Microsoft.VisualBasic

Public Structure tNamePairs
    Shared ws As New priwebsvc.Service
    Shared sd As New Priority.SerialData

    Private _NP(,) As String
    Private _debug As Boolean
    Private _ColTypes(,) As String

    Public Sub New(ByVal iPage As Page, ByVal DebugOn As Boolean)
        ' Load the name pairs
        _debug = DebugOn
        For Each item As String In iPage.Request.QueryString
            Dim i As String = iPage.Server.HtmlDecode(iPage.Request(item))
            If Len(Trim(i)) > 0 Then NamePair(item) = Replace(i, "%", "*")
        Next
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
