Imports System.Xml
Imports System.IO

Public Class Form1

    Private Sub MenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem1.Click

        Try
            ' SECTION 1. Create a DOM Document and load the XML data into it.
            Dim dom As New XmlDocument()
            dom.Load(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) & "\" & "prioritymobile\" & UserEnv.Text & "\calls.xml")

            ' SECTION 2. Initialize the treeview control.
            TreeView.Nodes.Clear()
            TreeView.Nodes.Add(New TreeNode(dom.DocumentElement.Name))
            Dim tNode As New TreeNode()
            tNode = TreeView.Nodes(0)

            ' SECTION 3. Populate the TreeView with the DOM nodes.
            AddNode(dom.DocumentElement, tNode)
            TreeView.ExpandAll()

        Catch xmlEx As XmlException
            MessageBox.Show(xmlEx.Message)
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

    End Sub

    Private Sub AddNode(ByRef inXmlNode As XmlNode, ByRef inTreeNode As TreeNode)
        Dim xNode As XmlNode
        Dim tNode As TreeNode
        Dim nodeList As XmlNodeList
        Dim i As Long

        ' Loop through the XML nodes until the leaf is reached.
        ' Add the nodes to the TreeView during the looping process.
        If inXmlNode.HasChildNodes() Then
            nodeList = inXmlNode.ChildNodes
            For i = 0 To nodeList.Count - 1
                xNode = inXmlNode.ChildNodes(i)
                inTreeNode.Nodes.Add(New TreeNode(xNode.Name))
                tNode = inTreeNode.Nodes(i)
                AddNode(xNode, tNode)
            Next
        Else
            ' Here you need to pull the data from the XmlNode based on the
            ' type of node, whether attribute values are required, and so forth.
            inTreeNode.Text = (inXmlNode.OuterXml).Trim
        End If
    End Sub

    Private Sub Form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim fo As New DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) & "\" & "prioritymobile\")
        For Each env In fo.GetDirectories
            Dim us As New DirectoryInfo(env.FullName)
            For Each user In us.GetDirectories
                UserEnv.Items.Add(env.Name & "\" & user.Name)
            Next
        Next
    End Sub

End Class
