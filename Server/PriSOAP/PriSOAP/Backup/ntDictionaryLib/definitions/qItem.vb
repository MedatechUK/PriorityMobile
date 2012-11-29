
Public Class qItem
    Inherits DatasetObjectBase

#Region "Private Variables"

    Private _BubbleID As String
    Private _Filename As String
    Private _Caller As String
    Private _qDate As String
    Private _SOAPProc As String

#End Region

#Region "Initialisation"

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal BubbleID As String, ByVal Filename As String, ByVal Caller As String, ByVal SOAPProc As String)
        MyBase.New()
        _Filename = Filename
        _Caller = Caller
        _qDate = Now.ToString
        _BubbleID = BubbleID
        _SOAPProc = SOAPProc
    End Sub

#End Region

#Region "Public Prioperties"

    Public Property BubbleID() As String
        Get
            Return _BubbleID
        End Get
        Set(ByVal value As String)
            If _BubbleID <> value Then
                _BubbleID = value
                OnPropertyChanged("BubbleID")
            End If
        End Set
    End Property

    Public Property Filename() As String
        Get
            Return _Filename
        End Get
        Set(ByVal value As String)
            If _Filename <> value Then
                _Filename = value
                OnPropertyChanged("Filename")
            End If
        End Set
    End Property

    Public Property qDate() As String
        Get
            Return _qDate
        End Get
        Set(ByVal value As String)
            If _qDate <> value Then
                _qDate = value
                OnPropertyChanged("qDate")
            End If
        End Set
    End Property

    Public Property Caller() As String
        Get
            Return _Caller
        End Get
        Set(ByVal value As String)
            If _Caller <> value Then
                _Caller = value
                OnPropertyChanged("Caller")
            End If
        End Set
    End Property

    Public Property SOAPProc() As String
        Get
            Return _SOAPProc
        End Get
        Set(ByVal value As String)
            If _SOAPProc <> value Then
                _SOAPProc = value
                OnPropertyChanged("SOAPProc")
            End If
        End Set
    End Property

#End Region

End Class
