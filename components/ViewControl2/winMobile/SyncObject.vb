Imports System.Xml
Imports System.IO

Public Class SyncObject

#Region "Public Properties"

    Private _PathList As New List(Of String)
    Public Property PathList() As List(Of String)
        Get
            Return _PathList
        End Get
        Set(ByVal value As List(Of String))
            _PathList = value
        End Set
    End Property

    Private _sObject As New List(Of SyncObject)
    Public Property sObject() As List(Of SyncObject)
        Get
            Return _sObject
        End Get
        Set(ByVal value As List(Of SyncObject))
            _sObject = value
        End Set
    End Property
    Public ReadOnly Property ListObjects() As List(Of String)
        Get
            Dim ret As New List(Of String)
            Try

                For Each so As SyncObject In sObject
                    Dim p1 As String = so.Path.Substring(0, so.Path.Length - so.RelativePath.Length)
                    Dim p2 As String = so.RelativePath.Substring(0, so.RelativePath.LastIndexOf("/"))
                    ret.Add(p1 & p2)
                Next
            Catch ex As Exception
                Beep()                            
            End Try
            Return ret
        End Get
    End Property
    Private _Path As String = Nothing
    Public Property Path() As String
        Get
            Return _Path
        End Get
        Set(ByVal value As String)
            _Path = value
        End Set
    End Property
    Private _Key As New List(Of String)
    Public Property Key() As List(Of String)
        Get
            Return _Key
        End Get
        Set(ByVal value As List(Of String))
            _Key = value
        End Set
    End Property
    Private _RelativePath As String = Nothing
    Public Property RelativePath() As String
        Get
            If IsNothing(_RelativePath) Then
                Return _Path
            Else
                Return _RelativePath
            End If
        End Get
        Set(ByVal value As String)
            _RelativePath = value
        End Set
    End Property

#End Region

#Region "Initialisation and Finalisaton"

    Public Sub New(ByVal ObjectNode As XmlNode)
        With ObjectNode.InnerText
            Dim li As Integer = .LastIndexOf("/")
            _Path = .Substring(0, li)
            If .Substring(li + 1, (.Length - (li + 1))).Length > 0 Then _
                _Key.AddRange(Split(.Substring(li + 1, (.Length - (li + 1))), "&"))

        End With
    End Sub

    Public Sub New(ByVal Path As String, ByVal Key As List(Of String), ByVal RelativePath As String)
        _Path = Path
        _Key = Key
        _RelativePath = RelativePath
    End Sub

#End Region

#Region "Public Methods"

    Public Function IsChild(ByVal ObjectNode As XmlNode) As Boolean
        Dim thisPath As String = Nothing
        Dim thisKey As New List(Of String)
        With ObjectNode.InnerText
            Dim li As Integer = .LastIndexOf("/")
            thisPath = .Substring(0, li)
            thisKey.AddRange(Split(.Substring(li + 1, (.Length - (li + 1))), "&"))
        End With

        If String.Compare(Left(thisPath, Me.Path.Length), Me.Path, True) = 0 Then
            Dim oParent As SyncObject = RecurseChild(Me, thisPath)
            oParent.sObject.Add(New SyncObject(thisPath, thisKey, Right(thisPath, thisPath.Length - oParent.Path.Length)))
            Return True
        Else
            Return False
        End If

    End Function

    Public Function xPathQuery(ByVal Path As String, ByVal Node As XmlNode) As String
        Dim i As Integer = 0
        Dim ret As String = Path
        If Key.Count > 0 Then
            ret += "["
            For Each k As String In Key
                ret += String.Format("{1}={0}{2}{0}", _
                        Chr(34), _
                        k, _
                        Node.SelectSingleNode(k).InnerText)
                i += 1
                If i < Key.Count Then
                    ret += " and "
                End If
            Next
            ret += "]"
        End If
        Return ret
    End Function

#End Region

#Region "Private Methods"

    Private Function RecurseChild(ByVal SO As SyncObject, ByVal ThisPath As String) As SyncObject
        For Each s As SyncObject In SO.sObject
            If String.Compare(Left(ThisPath, s.Path.Length), s.Path, True) = 0 Then
                Return RecurseChild(s, ThisPath)
            End If
        Next
        Return SO
    End Function

#End Region

End Class