Imports System.Windows.Forms

Public Class CtrlList : Inherits CtrlPartial

    Public Declare Sub keybd_event Lib "coredll.dll" (ByVal bVK As Byte, _
    ByVal bScan As Byte, ByVal dwFlags As Integer, ByVal dwExtraInfo _
    As Integer)

    Dim ld As Boolean = False

    'Public Event Complete(ByVal ctrl As Object)

    Public Overrides Sub SetParent(ByVal Ctrl As Object)

        Dim thisCtrl As Windows.Forms.TextBox = Ctrl

        With List
            .Width = thisCtrl.Width
            .Font = thisCtrl.Font
        End With

        With Me
            .Width = thisCtrl.Width
            .Height = thisCtrl.Height '* 1.4
        End With

    End Sub

    Public Overrides Property Data() As String

        Get
            If Me.List.SelectedIndex > -1 Then
                Return Me.List.Items(Me.List.SelectedIndex)
            Else
                Return ""
            End If

        End Get

        Set(ByVal value As String)

            Dim n As Integer
            If Len(value) > 0 And Me.List.Items.Count > 0 Then
                For n = 0 To Me.List.Items.Count - 1
                    If Strings.StrComp("", value, CompareMethod.Text) = 0 Then 'LCase(Me.List.Items(n))
                        ld = True
                        Me.List.SelectedIndex = n
                        ld = False
                        Exit Property
                    End If
                Next
                ld = True
                Me.List.SelectedIndex = 0
                ld = False
            Else
                ld = True
                Try
                    Me.List.SelectedIndex = 0
                Catch
                End Try
                ld = False
            End If

        End Set

    End Property

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        List.Text = ""

    End Sub

    Private Sub CtrlList_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown, List.KeyDown
        Select Case e.KeyValue
            Case 113
                e.Handled = True
                keybd_event(&H73, 0, &H1, 0)
                keybd_event(&H73, 0, &H1, 0)
                Exit Sub
            Case 13
                If Not ld Then
                    e.Handled = True
                    CtrlAccept(Me, tCtrl.ctrlList)
                    Me.Visible = False
                End If
        End Select
    End Sub

    Private Sub CtrlList_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles MyBase.KeyPress

    End Sub

    Private Sub CtrlList_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyUp

    End Sub



    Private Sub ctrlNumber_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize

        With List
            .Top = 0
            .Left = 0
        End With

    End Sub

    Private Sub List_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles List.Click
        List.DropDownStyle = ComboBoxStyle.DropDown
    End Sub

    Sub Accept(ByVal sender As Object, ByVal e As System.EventArgs) Handles List.SelectedIndexChanged
        'If Not ld Then CtrlAccept(Me, tCtrl.ctrlList)
    End Sub

    Public Overrides Sub AddItems(ByVal myArray As Array, ByVal DefaultText As String)

        Dim y As Integer
        Dim arr(,) = myArray

        List.Items.Clear()
        For y = 0 To UBound(arr, 2)
            If Strings.StrComp(DefaultText, arr(0, y), CompareMethod.Text) = 0 Then
                List.Text = CStr(arr(0, y))
                List.Items.Add(arr(0, y))
                ld = True
                List.SelectedIndex = y
                ld = False
            Else
                List.Items.Add(arr(0, y))
            End If
        Next

    End Sub

End Class
