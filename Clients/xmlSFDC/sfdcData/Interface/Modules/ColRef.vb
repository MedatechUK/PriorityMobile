Module ColRef

    Public Function GetColumn(ByVal Sender As cNode, ByVal strColRef As String) As cColumn

        Dim loc As String = strColRef.Split(".")(0).ToLower
        Dim ColName As String = strColRef.Split(".")(1)

        Dim thisContainer As cContainer
        If Not IsNothing(TryCast(Sender, cContainer)) Then            
            thisContainer = Sender
        ElseIf Not IsNothing(TryCast(Sender, cColumn)) Then
            thisContainer = Sender.Parent
        Else
            Throw New cfmtException("Sender is neither a column nor a container.")
        End If

        With thisContainer
            Select Case .NodeType
                Case "form"
                    Select Case loc
                        Case ":$"
                            With .Parent.Form
                                If .Columns.Keys.Contains(ColName) Then
                                    Return .Columns(ColName)
                                Else
                                    Throw New cfmtException("{0} does not contain column {1}.", thisContainer.ContainerType, ColName)
                                End If
                            End With

                        Case ":$$"
                            Throw New cfmtException("Form column {0} cannot refer to upper level form.", ColName)

                    End Select

                Case "table"
                    Select Case loc
                        Case ":$"
                            With .Parent.Table
                                If .Columns.Keys.Contains(ColName) Then
                                    Return .Columns(ColName)
                                Else
                                    Throw New cfmtException("{0} does not contain column {1}.", thisContainer.ContainerType, ColName)
                                End If
                            End With

                        Case ":$$"
                            With .Parent.Form
                                If .Columns.Keys.Contains(ColName) Then
                                    Return .Columns(ColName)
                                Else
                                    Throw New cfmtException("{0} does not contain column {1}.", thisContainer.ContainerType, ColName)
                                End If
                            End With

                    End Select

                Case Else
                    Return Nothing

            End Select

        End With

    End Function

End Module
