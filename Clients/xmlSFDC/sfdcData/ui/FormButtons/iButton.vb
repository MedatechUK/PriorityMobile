Imports System.Drawing

Public Class iButton

    Private _imgEnabled As Bitmap
    Private _imgDisabled As Bitmap

    Public Event ButtonClick(ByVal Button As eFormButtons)

#Region "Initialisation and finalisation"

    Public Sub New(ByVal Tag As Integer, ByRef ButtonBox As System.Windows.Forms.PictureBox, ByVal imgEnabled As Bitmap, ByVal imgDisabled As Bitmap)

        _Tag = Tag
        _ButtonBox = ButtonBox

        AddHandler _ButtonBox.MouseDown, AddressOf hPressButton
        AddHandler _ButtonBox.MouseUp, AddressOf hReleaseButton

        _Enabled = True
        _Disabled = False

        _imgEnabled = imgEnabled
        _imgDisabled = imgDisabled

    End Sub

#End Region

#Region "Public Properties"

    Private _Tag As Integer
    Public Property Tag() As Integer
        Get
            Return _Tag
        End Get
        Set(ByVal value As Integer)
            _Tag = value
        End Set
    End Property

    Private _Enabled As Boolean
    Public Property Enabled() As Boolean
        Get
            Return _Enabled
        End Get
        Set(ByVal value As Boolean)
            _Enabled = value
            Select Case _Enabled And Not (_Disabled)
                Case True
                    ButtonBox.Image = _imgEnabled
                Case Else
                    ButtonBox.Image = _imgDisabled
            End Select
        End Set
    End Property

    Private _Disabled As Boolean
    Public Property Disabled() As Boolean
        Get
            Return _Disabled
        End Get
        Set(ByVal value As Boolean)
            _Disabled = value
            Select Case _Disabled
                Case True
                    ButtonBox.Image = _imgDisabled
                Case Else
                    Select Case _Enabled
                        Case True
                            ButtonBox.Image = _imgEnabled
                        Case Else
                            ButtonBox.Image = _imgDisabled
                    End Select
            End Select
        End Set
    End Property

    Private _ButtonBox As System.Windows.Forms.PictureBox
    Public Property ButtonBox() As System.Windows.Forms.PictureBox
        Get
            Return _ButtonBox
        End Get
        Set(ByVal value As System.Windows.Forms.PictureBox)
            _ButtonBox = value
        End Set
    End Property

#End Region

#Region "Button Press Handlers"

    Private Sub hPressButton(ByVal sender As Object, ByVal e As System.EventArgs)
        If Enabled Then
            Dim btn As System.Windows.Forms.PictureBox = TryCast(sender, System.Windows.Forms.PictureBox)
            If Not IsNothing(btn) Then
                btn.Image = _imgDisabled
            End If
        End If
    End Sub

    Private Sub hReleaseButton(ByVal sender As Object, ByVal e As System.EventArgs)
        If Enabled Then
            Dim btn As System.Windows.Forms.PictureBox = TryCast(sender, System.Windows.Forms.PictureBox)
            If Not IsNothing(btn) Then
                btn.Image = _imgEnabled
                RaiseEvent ButtonClick(Tag)
            End If
        End If
    End Sub

#End Region

#Region "Public Metods"

    Public Sub Click()
        RaiseEvent ButtonClick(Me.Tag)
    End Sub

#End Region

End Class