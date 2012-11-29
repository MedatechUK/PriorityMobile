Imports Microsoft.VisualBasic

Public Enum eType
    typeNONE = 0
    typeDDL = 1
    typeTEXT = 2
End Enum
Public Structure tFilterView

    Dim dummy As String
    Shared ws As New priwebsvc.Service
    Shared sd As New Priority.SerialData

    Private _SQL() As String
    Private _Ph() As PlaceHolder
    Private _Cols() As String
    Private _Type() As eType

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

    Public Sub DrawFilters(ByRef _caller As DynamicMaster)

        For i As Integer = 0 To UBound(_Cols)
            If Not IsNothing(_Ph(i)) Then
                Dim filter(,) As String = sd.DeSerialiseData(ws.GetData(FilterSQL(i)))
                Select Case _Type(i)
                    Case eType.typeDDL
                        Dim f As Boolean = False
                        If Not IsNothing(Filter) Then
                            Dim ddl As New DropDownList
                            With ddl
                                .ID = _Cols(i)
                                .AutoPostBack = True
                                .Items.Add("")
                                For y As Integer = 0 To UBound(Filter, 2)
                                    .Items.Add(Filter(0, y))
                                    If LCase(Trim(Filter(0, y))) = _caller.NamePair.NamePair(LCase(Trim(_Cols(i)))) Then
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
                            .Text = _caller.NamePair.NamePair(_Cols(i))
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

