Imports System.Xml

Public Class lookupData

#Region "Private Variables"

    Private _BindSources As New Dictionary(Of String, BindingSource)
    Private doc As XmlDocument

#End Region

#Region "initialisation and finalisation"

    Public Sub New(ByVal xmlData As OfflineXML)

        doc = xmlData.Document

        For Each l As XmlNode In xmlData.Document.SelectSingleNode("lookup").ChildNodes
            If Not IsNothing(l.SelectSingleNode(String.Format("current[@value={0}*{0}]", Chr(34)))) Then
                Dim ds As New DataSet

                Dim MemoryStream As New System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(String.Format("<{0}>", l.Name) & l.SelectSingleNode(String.Format("current[@value={0}*{0}]", Chr(34))).InnerXml & String.Format("</{0}>", l.Name)))
                MemoryStream.Seek(0, System.IO.SeekOrigin.Begin)
                ds.ReadXml(XmlReader.Create(MemoryStream))

                Dim bs As New BindingSource
                bs.DataSource = ds.Tables(0)

                _BindSources.Add(l.Name, bs)
            
            End If
        Next

    End Sub

#End Region

#Region "Public Methods"

    Public Function BindSource(ByVal Table As String, Optional ByVal Current As String = "*") As BindingSource

        If Current = "*" Then
            If _BindSources.Keys.Contains(Table) Then
                Return _BindSources(Table)
            Else
                MsgBox(String.Format("The lookup table {0} was not found.", Table))
                Return Nothing
            End If
        Else
            Dim ds As New DataSet

            If Not IsNothing( _
                doc.SelectSingleNode( _
                    String.Format( _
                        "lookup/{1}/current[@value={0}{2}{0}]", _
                        Chr(34), _
                        Table, _
                        Current _
                    ) _
                ) _
            ) Then

                Dim MemoryStream As New System.IO.MemoryStream( _
                    System.Text.Encoding.UTF8.GetBytes( _
                        String.Format("<{0}>", Table) & _
                        doc.SelectSingleNode( _
                            String.Format( _
                                "lookup/{1}/current[@value={0}{2}{0}]", _
                                Chr(34), _
                                Table, _
                                Current _
                            ) _
                        ).InnerXml & _
                        String.Format("</{0}>", Table) _
                    ) _
                )

                MemoryStream.Seek(0, System.IO.SeekOrigin.Begin)
                ds.ReadXml(XmlReader.Create(MemoryStream))

                Dim bs As New BindingSource
                bs.DataSource = ds.Tables(0)
                Return bs
            Else
                If _BindSources.Keys.Contains(Table) Then
                    Return _BindSources(Table)
                Else
                    MsgBox(String.Format("The lookup table {0} was not found.", Table))
                    Return Nothing
                End If
            End If
        End If

    End Function

    Public Function AllowedValues(ByVal Table As String) As List(Of String)
        Dim ret As New List(Of String)
        For Each n As XmlNode In doc.SelectNodes(String.Format("lookup/{0}/*/option/value", Table))
            If Not ret.Contains(n.InnerText) Then
                ret.Add(n.InnerText)
            End If
        Next
        Return ret
    End Function

#End Region

End Class
