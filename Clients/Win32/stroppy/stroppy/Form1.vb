Imports System.IO

Public Class Form1

#Region "Initialisation and Finalisation"

    Private Sub Form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        LoadFolderTree("C:\", MyTreeView)
        LoadFolderTree("C:\", MyTreeView2)
    End Sub

#End Region

#Region "Load Tree"

    Public Sub LoadFolderTree(ByVal path As String, ByVal TV As TreeView)
        Dim basenode As System.Windows.Forms.TreeNode
        If IO.Directory.Exists(path) Then
            If path.Length <= 3 Then
                basenode = TV.Nodes.Add(path)
            Else
                basenode = TV.Nodes.Add(My.Computer.FileSystem.GetName(path))
            End If
            basenode.Tag = path
            LoadDir(path, basenode)
        Else
            Throw New System.IO.DirectoryNotFoundException()
        End If
    End Sub

    Private Sub MyTreeView_AfterExpand(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles MyTreeView.AfterExpand, MyTreeView2.AfterExpand
        For Each n As System.Windows.Forms.TreeNode In e.Node.Nodes
            LoadDir(n.Tag, n)
        Next
    End Sub

    Public Sub LoadDir(ByVal DirPath As String, ByVal Node As Windows.Forms.TreeNode)
        On Error Resume Next
        Dim Dir As String
        Dim Index As Integer
        If Node.Nodes.Count = 0 Then
            For Each Dir In My.Computer.FileSystem.GetDirectories(DirPath)
                Index = Dir.LastIndexOf("\")
                Node.Nodes.Add(Dir.Substring(Index + 1, Dir.Length - Index - 1))
                Node.LastNode.Tag = Dir
                Node.LastNode.ImageIndex = 0
            Next
        End If
    End Sub

#End Region

#Region "Drag and drop"

    Private Sub hMouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles MyTreeView.MouseDown, MyTreeView2.MouseDown

        Dim tree As TreeView = CType(sender, TreeView)
        Dim node As TreeNode

        node = tree.GetNodeAt(e.X, e.Y)
        tree.SelectedNode = node

        If Not node Is Nothing Then
            tree.DoDragDrop(node.Clone(), DragDropEffects.Copy)
        End If

    End Sub

    Private Sub hDragOver(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles MyTreeView.DragOver, MyTreeView2.DragOver
        Dim tree As TreeView = CType(sender, TreeView)

        e.Effect = DragDropEffects.None

        If Not e.Data.GetData(GetType(TreeNode)) Is Nothing Then
            Dim pt As New Point(e.X, e.Y)
            pt = tree.PointToClient(pt)
            Dim node As TreeNode = tree.GetNodeAt(pt)
            If Not node Is Nothing Then
                e.Effect = DragDropEffects.Copy
                tree.SelectedNode = node
            End If

        End If

    End Sub

    Private Sub hDragDrop(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles MyTreeView.DragDrop, MyTreeView2.DragDrop
        Dim tree As TreeView = CType(sender, TreeView)
        Dim pt As New Point(e.X, e.Y)
        pt = tree.PointToClient(pt)

        Dim src As TreeNode = e.Data.GetData(GetType(TreeNode))
        Dim dst As TreeNode = tree.GetNodeAt(pt)

        RecursiveDirectoryCopy(src.Tag, dst.Tag, True, True)

        With dst
            .Nodes.Add(src)
            .Expand()
        End With

    End Sub

#End Region

#Region "Recursive Copy"

    Private Sub RecursiveDirectoryCopy(ByVal sourceDir As String, ByVal destDir As String, ByVal fRecursive As Boolean, ByVal overWrite As Boolean)

        Dim sDir As String
        Dim dDirInfo As IO.DirectoryInfo
        Dim sDirInfo As IO.DirectoryInfo
        Dim sFile As String
        Dim sFileInfo As IO.FileInfo
        Dim dFileInfo As IO.FileInfo

        ' Add trailing separators to the supplied paths if they don't exist. 
        If Not sourceDir.EndsWith(System.IO.Path.DirectorySeparatorChar.ToString()) Then
            sourceDir &= System.IO.Path.DirectorySeparatorChar
        End If
        If Not destDir.EndsWith(System.IO.Path.DirectorySeparatorChar.ToString()) Then
            destDir &= System.IO.Path.DirectorySeparatorChar
        End If

        'If destination directory does not exist, create it. 
        dDirInfo = New System.IO.DirectoryInfo(destDir)
        If dDirInfo.Exists = False Then dDirInfo.Create()
        dDirInfo = Nothing
        ' Recursive switch to continue drilling down into directory structure. 
        If fRecursive Then
            ' Get a list of directories from the current parent. 
            For Each sDir In System.IO.Directory.GetDirectories(sourceDir)
                sDirInfo = New System.IO.DirectoryInfo(sDir)
                dDirInfo = New System.IO.DirectoryInfo(destDir & sDirInfo.Name)
                ' Create the directory if it does not exist. 
                If dDirInfo.Exists = False Then dDirInfo.Create()
                ' Since we are in recursive mode, copy the children also 
                RecursiveDirectoryCopy(sDirInfo.FullName, dDirInfo.FullName, fRecursive, overWrite)
                sDirInfo = Nothing
                dDirInfo = Nothing
            Next
        End If
        ' Get the files from the current parent. 
        For Each sFile In System.IO.Directory.GetFiles(sourceDir)
            sFileInfo = New System.IO.FileInfo(sFile)
            dFileInfo = New System.IO.FileInfo(Replace(sFile, sourceDir, destDir))
            'If File does not exist. Copy. 
            If dFileInfo.Exists = False Then
                sFileInfo.CopyTo(dFileInfo.FullName, overWrite)
            Else
                'If file exists and is the same length (size). Skip. 
                'If file exists and is of different Length (size) and overwrite = True. Copy 
                If sFileInfo.Length <> dFileInfo.Length AndAlso overWrite Then
                    sFileInfo.CopyTo(dFileInfo.FullName, overWrite)
                    'If file exists and is of different Length (size) and overwrite = False. Skip 
                ElseIf sFileInfo.Length <> dFileInfo.Length AndAlso Not overWrite Then
                    Debug.WriteLine(sFileInfo.FullName & " Not copied.")


                End If
            End If

            sFileInfo = Nothing
            dFileInfo = Nothing
        Next
    End Sub

#End Region

End Class
