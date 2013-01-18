Public Class CompareFilesByDateCreated : Implements IComparer

    Public Function Compare(ByVal o1 As Object, ByVal o2 As Object) As Integer Implements IComparer.Compare
        Dim fileinfo1 As System.IO.FileInfo = o1
        Dim fileinfo2 As System.IO.FileInfo = o2
        Return DateTime.Compare(fileinfo1.CreationTime, fileinfo2.CreationTime)
    End Function

End Class
