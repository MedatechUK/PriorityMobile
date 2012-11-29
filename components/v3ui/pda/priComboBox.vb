Imports System.Xml

Public Class priComboBox : Inherits PriBaseCtrl

    Public Sub New(ByRef ct As ctform, ByRef f As XmlNode)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        myCT = ct
        SetProps(f)
        AddHandlers()

        With Me
            .ListValueCol = xmlMe.Attributes("ListValueCol").Value
            .ListTextCol = xmlMe.Attributes("ListTextCol").Value
            .ListSource = xmlMe.Attributes("ListSource").Value
            .ListFilter = xmlMe.Attributes("ListFilter").Value
        End With

        Dim BO As Bind.oBind = myCT.MyDataSet.DataSet(Me.ListSource)
        Dim dc As ComboBox = Me.DerivedControl
        With dc

            .DataBindings.Add("SelectedValue", myCT.BindingSource, Column)
            .DataSource = BO.FilterObject
            .ValueMember = Me.ListValueCol
            .DisplayMember = Me.ListTextCol

        End With

    End Sub

    Public Overrides Property DerivedControl() As System.Windows.Forms.Control
        Get
            Return Me.ComboBox
        End Get
        Set(ByVal value As System.Windows.Forms.Control)
            ComboBox = value
        End Set
    End Property

    Overrides Property Value() As String
        Get
            With ComboBox
                Try
                    If Active Or (DG.CurrentRowIndex = -1) Then
                        Dim pi As System.Reflection.PropertyInfo = .SelectedItem.GetType.GetProperty(Me.ListValueCol)
                        Return CStr(pi.GetValue(.SelectedItem, Nothing))
                    Else
                        With DG
                            If IsNothing(.Item(.CurrentRowIndex, ColNo)) Then
                                Return ""
                            Else
                                Return .Item(.CurrentRowIndex, ColNo)
                            End If
                        End With
                    End If
                Catch
                    Return ""
                End Try
            End With
        End Get
        Set(ByVal value As String)
            ComboBox.SelectedValue = value
        End Set
    End Property

    Public Overrides Property ControlText() As String
        Get
            Return Me.ComboBox.SelectedItem
        End Get
        Set(ByVal value As String)
            Dim pi As System.Reflection.PropertyInfo = Me.ComboBox.Items(0).GetType.GetProperty(Me.ListValueCol)
            For i As Integer = 0 To Me.ComboBox.Items.Count - 1
                If pi.GetValue(Me.ComboBox.Items(i), Nothing) = value Then
                    Me.ComboBox.SelectedIndex = i
                End If
            Next
        End Set
    End Property

End Class
