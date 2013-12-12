Imports System.Xml
Imports System.Xml.XPath
Imports System.IO
Imports System.Text
Imports System.ComponentModel



Public Class frmXpather
#Region "Variables and declarations"

    Private lstit As New List(Of lstItems)
    Private blist As New BindingList(Of lstItems)

#End Region

#Region "Functions"
#Region "Add and remove tab items / list items"
    Public Sub add(ByVal xmltxt As String, ByVal typetext As String, ByVal typ As String, Optional ByVal results As String = "")

        Try
            Dim nt As New TabPage
            TabControl1.TabPages.Add(nt)
            TabControl1.SelectedTab = nt
            nt.Name = System.Guid.NewGuid.ToString
            nt.Text = typ
            Dim newtb As New RichTextBox
            nt.Controls.Add(newtb)
            newtb.Dock = DockStyle.Fill
            newtb.Text = xmltxt
            Dim f As New lstItems(DataGridView1.RowCount + 1, nt.Name, typ, results)
            lstit.Add(f)

            BindData()
           
        Catch ex As Exception
            MsgBox("Unable to retrieve XML")
        End Try
    End Sub

    Public Sub remove()
        If DataGridView1.SelectedRows.Count <> 0 Then
            Dim f As String
            f = DataGridView1.SelectedRows.Item(0).Cells(1).Value
            Dim tabname = From it In lstit Where it.TabName = f
            For Each result In tabname
                Dim l As TabPage
                For Each l In TabControl1.TabPages
                    If l.Name = result.TabName Then
                        TabControl1.TabPages.Remove(l)
                        Exit For
                    End If
                Next
            Next
            Dim i As lstItems
            For Each i In lstit
                If i.TabName = f Then
                    lstit.Remove(i)
                    Exit For
                End If
            Next
            BindData()

        End If
    End Sub

    Public Sub BindData()
        blist.Clear()
        For Each h As lstItems In lstit
            blist.Add(h)
        Next
        DataGridView1.Columns(0).DataPropertyName = "ListIndex"
        DataGridView1.Columns(1).DataPropertyName = "TabName"
        DataGridView1.Columns(2).DataPropertyName = "TestText"
        DataGridView1.Columns(3).DataPropertyName = "Results"
        DataGridView1.DataSource = blist
        DataGridView1.ClearSelection()
        If DataGridView1.Rows.Count <> 0 Then
            DataGridView1.Rows(DataGridView1.RowCount - 1).Selected = True
        End If

    End Sub
#End Region

#Region "XML Indenting Functions"

    Private Function IndentXMLString(ByVal XMLStr As String) As String ' Format XML for reading
        Try
            Dim doc As New XmlDocument
            doc.LoadXml(XMLStr)
            Dim ms As New MemoryStream
            Dim xtw As New XmlTextWriter(ms, Encoding.Unicode)
            xtw.Formatting = Formatting.Indented
            doc.WriteContentTo(xtw)
            xtw.Flush()
            ms.Seek(0, SeekOrigin.Begin)
            Using SR As New StreamReader(ms)
                Return SR.ReadToEnd
            End Using
        Catch ex As XmlException
            MsgBox("XML not valid")
            Return Nothing
        End Try

    End Function
    Private Function IndentXML(ByVal XMLD As XmlDocument) As String ' Format XML for reading
        Try

            Dim ms As New MemoryStream
            Dim xtw As New XmlTextWriter(ms, Encoding.Unicode)
            xtw.Formatting = Formatting.Indented
            XMLD.WriteContentTo(xtw)
            xtw.Flush()
            ms.Seek(0, SeekOrigin.Begin)
            Using SR As New StreamReader(ms)
                Return SR.ReadToEnd
            End Using
        Catch ex As XmlException
            MsgBox("XML not valid")
            Return Nothing
        End Try

    End Function

#End Region

#Region "Textbox Functions"
    Private Sub TextBox2_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox2.KeyPress
        If e.KeyChar = Microsoft.VisualBasic.ChrW(Keys.Return) Then
            TestToolStripMenuItem_Click(Nothing, Nothing)
        End If
    End Sub
#End Region

#Region "Datagrid Functions"
    'this handles the focusing of the tabs when a datagrid row is selected
    Private Sub DataGridView1_SelectionChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DataGridView1.SelectionChanged

        If DataGridView1.SelectedRows.Count <> 0 Then
            Dim f As String
            f = DataGridView1.SelectedRows.Item(0).Cells(1).Value

            Dim tabname = From it In lstit Where it.TabName = f
            For Each result In tabname
                TabControl1.SelectTab(result.TabName)
            Next
        End If


    End Sub
#End Region

#End Region
#Region "Menu Items"
    'Delete tab and list items
    Private Sub RemoveToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RemoveToolStripMenuItem.Click
        remove()
    End Sub

    'run the xpath against the selected tab button
    Private Sub TestToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TestToolStripMenuItem.Click
        Dim xmltext As String = ""

        If TabControl1.TabCount = 0 Then
            'Exit Sub
        Else
            For Each j As Control In TabControl1.SelectedTab.Controls
                If TypeOf j Is RichTextBox Then
                    If j.Text = "" Or TextBox2.Text = "" Then
                        MsgBox("Nothing to test!!")
                        Exit Sub
                    End If
                    xmltext = j.Text
                End If
            Next
        End If



        Try
            Dim doc As New XmlDocument
            doc.LoadXml(xmltext)

            Dim xpathDoc As XPathDocument
            Dim xmlNav As XPathNavigator
            Dim xmlNI As XPathNodeIterator
            xpathDoc = New XPathDocument(New StringReader(xmltext))
            xmlNav = xpathDoc.CreateNavigator()
            Dim f As Integer = 0

            Dim chk As String = ""
            xmlNI = xmlNav.Select(TextBox2.Text)
            'f = xmlNI.Count
            While xmlNI.MoveNext
                f += 1
                chk &= xmlNI.Current.OuterXml & vbCrLf
            End While

            If chk <> "" Then
                Dim res As String
                res = "this check found " & f & " nodes"
                add(chk, "Test Results", "Results", res)

            Else
                MsgBox("Query returned no results")
                Exit Sub
            End If

        Catch ex As XPathException
            MsgBox("The XPATH syntax generated errors, please check and try again. The message was " & ex.Message.ToString)
        Catch ex As XmlException
            MsgBox(ex.Message.ToString)
        End Try



    End Sub

#Region "load Xml Buttons"

    Private Sub LoadFromURLToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LoadFromURLToolStripMenuItem.Click
        Try
            Dim xm As String = InputBox("Please provide a URL", "URL Needed")
            Dim h As New XmlDocument
            h.Load(xm)
            add(IndentXML(h), "XML generated from the Web", "WEB XML")
        Catch ex As Exception
            MsgBox("This address did not work, please try again.")
        End Try

    End Sub

    Private Sub LoadFromClipboardToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LoadFromClipboardToolStripMenuItem.Click
        Try
            Dim h As String
            h = My.Computer.Clipboard.GetText
            add(IndentXMLString(h), "XML Copied from the clipboard", "Copied XML")
        Catch ex As Exception
            MsgBox("XML not valid. Please try again")
        End Try

    End Sub

    Private Sub TypeInToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TypeInToolStripMenuItem.Click
        add("", "XML typed in", "Typed XML")
    End Sub


    Private Sub LoadFromFileToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LoadFromFileToolStripMenuItem.Click
        With OpenFileDialog1
            .Title = "Please select an XML file"
        End With
        If OpenFileDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
            Try

                Dim h As New XmlDocument
                h.Load(OpenFileDialog1.FileName)
                add(IndentXML(h), "XML generated from the Web", "FILE XML")
            Catch ex As Exception
                MsgBox("This file did not open properly, please try again.")
            End Try
        End If

    End Sub

#End Region

#End Region

End Class
Public Class lstItems
    Private lstind As Integer
    Private tbname As String
    Private txt As String
    Private resultstr As String
    Public Property ListIndex() As Integer
        Get
            Return lstind
        End Get
        Set(ByVal value As Integer)
            lstind = value
        End Set
    End Property
    Public Property TabName() As String
        Get
            Return tbname
        End Get
        Set(ByVal value As String)
            tbname = value
        End Set
    End Property
    Public Property TestText() As String
        Get
            Return txt
        End Get
        Set(ByVal value As String)
            txt = value
        End Set
    End Property
    Public Property Results() As String
        Get
            Return resultstr
        End Get
        Set(ByVal value As String)
            resultstr = value
        End Set
    End Property
    Public Sub New(ByVal i As Integer, ByVal tn As String, ByVal tx As String, ByVal res As String)
        ListIndex = i
        TabName = tn
        TestText = tx
        Results = res
    End Sub
    
End Class