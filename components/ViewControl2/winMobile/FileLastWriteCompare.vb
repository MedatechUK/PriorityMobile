Public Class FileLastWriteCompare : Implements IComparer

    Public Function Compare(ByVal o1 As Object, ByVal o2 As Object) As Integer Implements IComparer.Compare
        Dim file1 As System.IO.FileInfo = o1
        Dim file2 As System.IO.FileInfo = o2
        Return DateTime.Compare(file1.LastWriteTime, file2.LastWriteTime)
    End Function

End Class

