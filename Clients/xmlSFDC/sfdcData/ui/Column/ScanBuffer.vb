Imports System.Xml

Public Class ScanBuffer

#Region "Events"

    Public Event BufferChanged()

#End Region

#Region "Private Variables"

    Private Enum eSender As Integer
        Column
        table
    End Enum
    Private _Sender As eSender = Nothing

#End Region

#Region "public Properties"

    Private _2d As Boolean = False
    Public Property is2d() As Boolean
        Get
            Return _2d
        End Get
        Set(ByVal value As Boolean)
            _2d = value
        End Set
    End Property

    Private _ScanBuffer As New System.Text.StringBuilder
    Public Property Value() As String
        Get
            Return _ScanBuffer.ToString
        End Get
        Set(ByVal value As String)
            _ScanBuffer = New System.Text.StringBuilder(value)
        End Set
    End Property

    Public ReadOnly Property Length() As Integer
        Get
            Return _ScanBuffer.Length
        End Get
    End Property

    Public ReadOnly Property ScanDictionary() As Dictionary(Of String, String)
        Get
            Dim ret As New Dictionary(Of String, String)
            Dim doc As New Xml.XmlDocument
            Try
                doc.LoadXml(Value)
                For Each item As XmlNode In doc.SelectNodes("in/i")
                    ret.Add(item.Attributes("n").Value, item.Attributes("v").Value)
                Next

            Catch
                MsgBox("Invalid data.", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly)
            End Try

            Return ret
        End Get
    End Property
#End Region

#Region "Initialisation and Finalisation"

    Public Sub New(ByRef Sender As Object)

        Dim _Column As uiColumn = Nothing
        Dim _Table As TablePanel = Nothing

        If Not IsNothing(TryCast(Sender, uiColumn)) Then
            _Sender = eSender.Column
            _Column = Sender

        ElseIf Not IsNothing(TryCast(Sender, TablePanel)) Then
            _Sender = eSender.table
            _Table = Sender

        Else
            Throw New Exception("Invalid ScanBuffer control")

        End If

        Select Case _Sender
            Case eSender.Column
                AddHandler _Column.Label.KeyPress, AddressOf hKeypress

            Case eSender.table
                AddHandler _Table.thisTable.KeyPress, AddressOf hKeypress

        End Select

    End Sub

#End Region

#Region "Public Methods"

    Public Sub Clear()
        _2d = False
        _ScanBuffer = New System.Text.StringBuilder
    End Sub

    Public Sub BackSpace()
        If Length > 0 Then
            _ScanBuffer.Remove(Length - 1, 1)
            RaiseEvent BufferChanged()
        End If
    End Sub

    Public Sub Append(ByVal strVal As String)
        _ScanBuffer.Append(strVal)
        RaiseEvent BufferChanged()
    End Sub

#End Region

#Region "Key Handler"

    Private Sub hKeypress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)

        e.Handled = True
        Select Case Asc(e.KeyChar)
            Case 13, 10, 8, 9

            Case 32
                If Not _ScanBuffer.Length = 0 Then
                    _ScanBuffer.Append(" ")
                End If                

            Case 60
                If _ScanBuffer.Length = 0 Then
                    _2d = True
                End If
                _ScanBuffer.Append("<")
                RaiseEvent BufferChanged()

            Case Else
                _ScanBuffer.Append(e.KeyChar.ToString) '.Replace(Chr(13), "").Replace(Chr(10), "").Trim
                RaiseEvent BufferChanged()

        End Select

    End Sub

#End Region

End Class
