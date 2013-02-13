Friend Class QuestionBase

#Region "Public Events"

    Public Event NewResponse(ByVal QuestionNumber As Integer, ByVal Value As String, ByVal Text As String)

    Public Sub setResponseHandler(ByVal handler As PriorityMobile.QuestionBase.NewResponseEventHandler)
        AddHandler Me.NewResponse, handler
    End Sub

#End Region

#Region "Overridable Methods"

    Public Overridable Sub getSelected(ByRef Value As String, ByRef Text As String)
        Throw New Exception("This method must be overriden.")
    End Sub

#End Region

#Region "Public Properties"

    Private _NP As Dictionary(Of Integer, String)
    Public Property NP() As Dictionary(Of Integer, String)
        Get
            Return _NP
        End Get
        Set(ByVal value As Dictionary(Of Integer, String))
            _NP = value
        End Set
    End Property

    Private _QuestionNumber As Integer
    Public Property QuestionNumber() As Integer
        Get
            Return _QuestionNumber
        End Get
        Set(ByVal value As Integer)
            _QuestionNumber = value
        End Set
    End Property

    Public Property Question() As String
        Get
            Return Me.QuestionText.Text
        End Get
        Set(ByVal value As String)
            Me.QuestionText.Text = value
        End Set
    End Property

    Private _InternalHeight As Integer
    Public ReadOnly Property InternalHeight() As Integer
        Get
            Return _InternalHeight
        End Get
    End Property

#End Region

#Region "Event Handlers"

    Public Sub ValueChange(ByVal sender As QuestionBase)
        Dim ResponseValue As String = Nothing
        Dim ResponseText As String = Nothing
        With sender
            .getSelected(ResponseValue, ResponseText)
            RaiseEvent NewResponse(.QuestionNumber, ResponseValue, ResponseText)
        End With
    End Sub

#End Region

End Class
