Imports System.Xml

Public Class PriDatePick : Inherits PriBaseCtrl

    Private basedt As DateTime = New DateTime("1988", "1", "1")

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
            Return dt
        End Get
        Set(ByVal value As System.Windows.Forms.Control)
            dt = value
        End Set
    End Property

    Public Overrides Property Value() As String
        Get
            If Active Or (DG.CurrentRowIndex = -1) Then
                Return DateDiff(DateInterval.Minute, #1/1/1988#, dt.Value).ToString
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
            dt.Value = basedt.AddMinutes(CInt(value))
        End Set
    End Property

    Public Property Format() As DateTimePickerFormat
        Get
            Return dt.Format
        End Get
        Set(ByVal value As DateTimePickerFormat)
            dt.Format = value
        End Set
    End Property

End Class
