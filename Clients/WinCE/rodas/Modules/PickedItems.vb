Public Class PickedItems
    Private mPICK As Integer
    Private mROUTE As String
    Private mPart As String
    Private mPicked As Integer
    Public Property pick() As Integer
        Get
            Return mPICK
        End Get
        Set(ByVal value As Integer)
            mPICK = value
        End Set
    End Property
    Public Property route() As String
        Get
            Return mROUTE
        End Get
        Set(ByVal value As String)
            mROUTE = value
        End Set
    End Property
    Public Property Part() As String
        Get
            Return mPart
        End Get
        Set(ByVal value As String)
            mPart = value
        End Set
    End Property
    Public Property picked() As Integer
        Get
            Return mPicked
        End Get
        Set(ByVal value As Integer)
            mPicked = value
        End Set
    End Property
    Public Sub New(ByVal pic As Integer, ByVal rou As String, ByVal par As String, ByVal pica As Integer)
        pick = pic
        route = rou
        Part = par
        picked = pica
    End Sub
End Class
