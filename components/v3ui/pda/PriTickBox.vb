Imports System.Xml

Public Class PriTickBox : Inherits PriBaseCtrl

    Public Sub New(ByRef ct As ctform, ByRef f As XmlNode)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        myCT = ct
        SetProps(f)
        AddHandlers()
        DataBindings.Add("Value", myCT.BindingSource, xmlMe.InnerText)

    End Sub


    Public Overrides Property DerivedControl() As System.Windows.Forms.Control
        Get
            Return Me.CheckBox
        End Get
        Set(ByVal value As System.Windows.Forms.Control)
            Me.CheckBox = value
        End Set
    End Property

    Overrides Property Value() As String
        Get
            If Active Or (DG.CurrentRowIndex = -1) Then
                Select Case Me.CheckBox.Checked
                    Case True
                        Return "Y"
                    Case Else
                        Return ""
                End Select
            Else
                With DG
                    If IsNothing(.Item(.CurrentRowIndex, ColNo)) Then
                        Return ""
                    Else
                        Select Case .Item(.CurrentRowIndex, ColNo)
                            Case True
                                Return "Y"
                            Case Else
                                Return ""
                        End Select
                    End If
                End With
            End If

        End Get
        Set(ByVal value As String)
            Me.CheckBox.Checked = CBool(value = "Y")
        End Set
    End Property

End Class
