Public Class jsonDictionary
    Inherits Dictionary(Of String, Object)

#Region "Public Properties"
    Private _parent As jsonDictionary
    Public ReadOnly Property Parent() As jsonDictionary
        Get
            Return _parent
        End Get
    End Property

#End Region

#Region "Initialisation and Finalisation"

    Public Sub New(ByVal Parent As jsonDictionary, ByVal strJson As String)
        _parent = Parent
        For Each Element As String In SplitJSON(CharTrim(strJson))
            Dim elName As String = CharTrim(Element.Split(":")(0).Trim)
            Select Case Element.Split(":")(1).Trim.Substring(0, 1)
                Case "{"
                    Me.Add(elName, New jsonDictionary(Me, Element.Substring(InStr(Element, ":"))))
                Case "["
                    Dim ElArray As New List(Of jsonDictionary)
                    For Each ArrayElement As String In SplitJSON(CharTrim(Element.Substring(InStr(Element, ":"))))
                        ElArray.Add(New jsonDictionary(Me, ArrayElement))
                    Next
                    Me.Add(elName, ElArray)
                Case Else
                    Me.Add(elName, Element.Split(":")(1).Trim)
            End Select

        Next
    End Sub

#End Region

#Region "Private Methods"

    Public Shared Function CharTrim(ByVal Str As String) As String
        Return Str.Trim.Substring(1, Str.Length - 2)
    End Function

    Private Function SplitJSON(ByVal str As String) As String()

        Dim ret() As String = Nothing
        Dim sqCount As Integer = 0
        Dim clCount As Integer = 0
        Dim buildStr As New System.Text.StringBuilder

        For i As Integer = 0 To str.Length - 1
            Select Case str.Substring(i, 1)
                Case ","
                    If sqCount = 0 And clCount = 0 Then
                        Try
                            ReDim Preserve ret(UBound(ret) + 1)
                        Catch ex As Exception
                            ReDim ret(0)
                        Finally
                            ret(UBound(ret)) = buildStr.ToString
                            buildStr = New System.Text.StringBuilder
                        End Try
                    Else
                        buildStr.Append(str.Substring(i, 1))
                    End If
                Case Else
                    buildStr.Append(str.Substring(i, 1))
                    Select Case str.Substring(i, 1)
                        Case "["
                            sqCount += 1
                        Case "]"
                            sqCount -= 1
                        Case "{"
                            clCount += 1
                        Case "}"
                            clCount -= 1

                    End Select
            End Select
        Next
        If buildStr.ToString.Length > 0 Then
            Try
                ReDim Preserve ret(UBound(ret) + 1)
            Catch ex As Exception
                ReDim ret(0)
            Finally
                ret(UBound(ret)) = buildStr.ToString
            End Try
        End If
        Return ret

    End Function

#End Region

End Class