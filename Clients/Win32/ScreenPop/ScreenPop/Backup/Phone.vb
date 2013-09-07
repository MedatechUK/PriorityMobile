Imports ntDictionaryLib
Public Class Phone
    Inherits dCls

    Sub New()

        MyBase.New("ntDictionaryLib.phoneItem")
        ' Initialise the object
        With Me
            .Name = "Phone"
            .Undelete = False
        End With

        Load(tSource.File)

    End Sub

    Public Overrides Function Columns() As String()
        ' Set the columns
        Dim myCols() As String = { _
            "FORMNAME", "TABLE", "FORMID", "PHONECOL" _
        }
        Return myCols
    End Function

    Public Overrides Function Index(ByVal RowData() As String) As String
        Return RowData(0) & "\" & RowData(3)
    End Function

    Public Overrides Sub OnRemoveComplete(ByVal key As Object, ByVal value As Object)
        If Not Seeking Then
            MyBase.OnRemoveComplete(key, value)
            Me.Remove(key)
            Save(tSource.File)
        End If
    End Sub

    Public Overrides Sub OnInsertComplete(ByVal key As Object, ByVal value As Object)
        If Not Seeking Then
            MyBase.OnInsertComplete(key, value)
            Save(tSource.File)
        End If
    End Sub

    Public Overrides Sub OnSetComplete(ByVal key As Object, ByVal oldValue As Object, ByVal newValue As Object)
        If Not Seeking Then
            MyBase.OnSetComplete(key, oldValue, newValue)
            Save(tSource.File)
        End If
    End Sub

    Public Overrides Sub OnRowLeave(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs)
        If Not Seeking Then
            MyBase.OnRowLeave(sender, e)
            Save(tSource.File)
        End If
    End Sub

End Class
