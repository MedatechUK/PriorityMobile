Imports System.Xml

Public Class cColumns
    Inherits System.Collections.Generic.Dictionary(Of String, cColumn)

    Public Sub New(ByRef Parent As cContainer, ByRef thisNode As XmlNode)
        Try
            'Dim index As New List(Of Integer)

            For Each col As XmlNode In thisNode.SelectNodes("column")
                Me.Add(col.Attributes("name").Value, New cColumn(Parent, col))
                'If Not index.Contains(CInt(col.Attributes("pos").Value)) Then _
                '    index.Add(CInt(col.Attributes("pos").Value))
            Next

            'index.Sort(AddressOf sortPOS)
            'For Each pos As Integer In index                
            '    For Each col As XmlNode In thisNode.SelectNodes(String.Format("column[@pos='{0}']", pos.ToString))
            '        Me.Add(col.Attributes("name").Value, New cColumn(Parent, col))
            '    Next                
            'Next

        Catch ex As Exception
            Throw New cfmtException("Failed to load columns. {0}", ex.Message)
        End Try
    End Sub

    Public Function ColumnAlias(ByRef Name As String) As cColumn
        If Me.Keys.Contains(Name) Then
            Return Me(Name)
        Else
            For Each c As cColumn In Me.Values
                If String.Compare(Name, c.BarcodeField) = 0 Then
                    Return c
                End If
            Next
        End If
        Return Nothing
    End Function

    'Private Shared Function sortPOS(ByVal x As Integer, ByVal y As Integer)
    '    Return x.CompareTo(y)
    'End Function

End Class
