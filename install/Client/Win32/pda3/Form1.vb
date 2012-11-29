Imports System.Xml
Imports System.IO

Public Class Form1

    'Dim doc As New XmlDocument()
    'Dim Table As New Dictionary(Of String, DataTable)

    'Private Sub LoadXMLToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LoadXMLToolStripMenuItem.Click


    '    Dim ue As New UserEnv("tabula", New Uri("http://mobile.emerge-it.co.uk:8080/"))
    '    Dim xf As New xmlForms( _
    '        New OfflineXML(ue, "forms.xml", "forms.ashx"), _
    '        New OfflineXML(ue, "calls.xml", "calls.ashx") _
    '    )

    '    Dim f As xForm = xf.TopForms(xf.ActiveForm).CurrentForm
    '    f.CurrentView += 1

    '    'Dim tf As New List(Of xForm)
    '    'For Each f As XmlNode In xf.TopLevelForms
    '    '    tf.Add(New xForm(xf.Attribute(f, "dataelement")))

    '    '    For Each sf As XmlNode In xf.SubForms(f)
    '    '        Debug.Print(xf.Attribute(sf, "name"))
    '    '    Next

    '    '    Debug.Print(xf.Attribute(f, "name"))
    '    '    For Each v As XmlNode In xf.FormViews(f)
    '    '        Debug.Print(xf.Attribute(v, "control"))
    '    '    Next

    '    'Next

    '    'For Each n As XmlNode In doc.SelectSingleNode("pdadata").ChildNodes
    '    '    Table.Add(n.Name, XMLtoDataset(n.OuterXml))
    '    '    AddHandler Table(n.Name).ColumnChanged, AddressOf hColumnChanged
    '    'Next

    '    'With ListView
    '    '    .Clear()
    '    '    For Each c As System.Data.DataColumn In Table("calls").Columns
    '    '        .Columns.Add(c.ColumnName)
    '    '    Next
    '    '    For Each r As System.Data.DataRow In Table("calls").Rows
    '    '        .Items.Add(New ListViewItem)
    '    '        With .Items(.Items.Count - 1)
    '    '            Dim n As Integer = 0
    '    '            For Each c As System.Data.DataColumn In r.Table.Columns
    '    '                If Not c.ColumnMapping = MappingType.Hidden Then
    '    '                    .SubItems.Add(New System.Windows.Forms.ListViewItem.ListViewSubItem)
    '    '                    .SubItems(n).Text = r.ItemArray(n)
    '    '                    n += 1
    '    '                End If
    '    '            Next
    '    '        End With
    '    '    Next
    '    'End With

    '    'Dim ds As DataSet = XMLtoDataset(doc.SelectSingleNode("pdadata").InnerXml)

    '    DataGridView1.DataSource = Table("calls") 'BindingSource        
    '    TextBox1.DataBindings.Add("Text", Table("calls"), "callnumber")
    '    With ComboBox1
    '        .DataBindings.Add("Text", Table("calls"), "callstatus")
    '        .DataSource = Table("callstate")
    '        .DisplayMember = "name"
    '    End With

    'End Sub

    'Private Function XMLtoDataset(ByVal XML As String) As DataTable

    '    Dim ds As New DataSet

    '    Dim MemoryStream As New System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(XML))
    '    MemoryStream.Seek(0, System.IO.SeekOrigin.Begin)
    '    ds.ReadXml(XmlReader.Create(MemoryStream))

    '    Return ds.Tables(0)

    'End Function

    'Private Sub DataGridView1_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DataGridView1.SelectionChanged
    '    If Not IsNothing(DataGridView1.CurrentRow) Then
    '        Dim ds As DataTable = XMLtoDataset(doc.SelectSingleNode("pdadata").SelectSingleNode(String.Format("servicecall[callnumber={0}{1}{0}]", Chr(34), DataGridView1.CurrentRow.Cells(0).Value)).OuterXml)
    '        DataGridView2.DataSource = ds '.Tables("customer")
    '    End If
    'End Sub

    'Private Sub hColumnChanged(ByVal sender As Object, ByVal e As System.Data.DataColumnChangeEventArgs)
    '    Dim n As XmlNode = doc.SelectSingleNode("pdadata").SelectSingleNode("calls").SelectSingleNode(String.Format("servicecall[callnumber={0}{1}{0}]", Chr(34), DataGridView1.CurrentRow.Cells(0).Value))
    '    Dim n2 As XmlNode = n.SelectSingleNode(e.Column.ColumnName)
    '    n2.InnerText = e.ProposedValue
    '    doc.Save("calls.xml")
    'End Sub

    'Private Sub SaveToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveToolStripMenuItem.Click

    'End Sub

End Class
