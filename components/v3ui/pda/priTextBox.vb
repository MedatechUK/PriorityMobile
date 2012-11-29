Imports System.Text.RegularExpressions
Imports System.xml

Public Class priTextBox : Inherits PriBaseCtrl

    Public Sub New(ByRef ct As ctform, ByRef f As xmlnode)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        myCT = ct
        SetProps(f)
        AddHandlers()
        DataBindings.Add("Value", myCT.BindingSource, Column)

    End Sub

    Public Overrides Property DerivedControl() As System.Windows.Forms.Control
        Get
            Return Me.TextBox
        End Get
        Set(ByVal value As System.Windows.Forms.Control)
            Me.TextBox = value
        End Set
    End Property

    Overrides Property Value() As String
        Get
            If Active Or (DG.CurrentRowIndex = -1) Then
                Return TextBox.Text
            Else
                With DG
                    If IsNothing(.Item(.CurrentRowIndex, ColNo)) Then
                        Return ""
                    Else
                        Return .Item(.CurrentRowIndex, ColNo)
                    End If
                End With
            End If
        End Get
        Set(ByVal value As String)
            TextBox.Text = value
        End Set
    End Property

    Public Overrides Property ControlText() As String
        Get
            Return Me.TextBox.Text
        End Get
        Set(ByVal value As String)
            Me.TextBox.Text = value
        End Set
    End Property

End Class
