'Imports prnscn.capture
'Imports hookey

'Public Class frm_Colours

'    Dim selected As Integer = Nothing

'    Private Sub frm_Colours_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

'        With myStates
'            For Each c As cargo3.Colour In .myColours.Values
'                With lst_Colour.Items
'                    .Add(c.Name)
'                End With
'            Next
'        End With

'    End Sub

'    Private Function cSelected() As cargo3.Colour
'        If myStates.myColours.Keys.Contains(lst_Colour.Items(selected).SubItems(0).Text) Then
'            Return myStates.myColours(lst_Colour.Items(selected).SubItems(0).Text)
'        Else
'            myStates.myColours.Add(lst_Colour.Items(selected).SubItems(0).Text, New cargo3.Colour(lst_Colour.Items(selected).SubItems(0).Text, 0, 0, 0))
'            Return myStates.myColours(lst_Colour.Items(selected).SubItems(0).Text)
'        End If
'    End Function

'    Private Sub lst_Colour_ItemSelectionChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.ListViewItemSelectionChangedEventArgs) Handles lst_Colour.ItemSelectionChanged
'        With PictureBox1
'            If lst_Colour.SelectedIndices.Count > 0 Then
'                selected = lst_Colour.SelectedIndices(0)
'                Dim c As cargo3.Colour = cSelected()
'                .BackColor = Color.FromArgb(c.Red, c.Green, c.Blue)
'            Else
'                selected = Nothing
'                .BackColor = Color.FromName("CONTROL")
'            End If            
'        End With

'    End Sub

'    Private Sub picDoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles PictureBox1.DoubleClick

'        If Not IsNothing(selected) Then

'            Dim selecting As New System.Timers.Timer
'            With selecting
'                .Interval = 100
'                AddHandler .Elapsed, AddressOf hSelTimer
'                .Start()
'            End With

'            KeyWait(VK_ESCAPE)

'            selecting.Stop()
'            selecting.Dispose()

'            SetColour()

'            With cSelected()
'                .Red = PictureBox1.BackColor.R
'                .Green = PictureBox1.BackColor.G
'                .Blue = PictureBox1.BackColor.B
'            End With

'            selected = Nothing

'        End If

'    End Sub

'    Public Sub SetColour()
'        If Me.InvokeRequired Then
'            Me.Invoke(New MethodInvoker(AddressOf SetColour))
'        Else
'            Dim ret As Color
'            Dim l As Point = Cursor.Position
'            With l
'                Dim bmp As Bitmap = PrintScreen(.X + 1, .Y + 1)
'                ret = bmp.GetPixel(.X, .Y)
'            End With
'            With PictureBox1
'                .BackColor = ret
'            End With
'        End If
'    End Sub

'    Private Sub hSelTimer(ByVal sender As Object, ByVal e As System.EventArgs)

'        SetColour()

'    End Sub

'    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
'        With lst_Colour.Items
'            Dim newName As String = InputBox("Colour Name")
'            If newName.Length > 0 Then
'                .Add(newName)
'            End If
'        End With
'    End Sub

'    Private Sub ContextMenuStrip1_Opening(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ContextMenuStrip1.Opening
'        If Not IsNothing(selected) Then            
'            With ContextMenuStrip1.Items
'                .Clear()
'                For Each c As cargo3.Coordinate In myStates.myCoordinates.Values
'                    .Add(c.Name)
'                    With .Item(.Count - 1)
'                        .Tag = c.Name
'                        AddHandler .Click, AddressOf hSelectCoord
'                    End With
'                Next
'            End With
'        End If
'    End Sub

'    Private Sub hSelectCoord(ByVal sender As Object, ByVal e As System.EventArgs)

'        Dim cm As ToolStripMenuItem = sender
'        Dim l As New Point(myStates.myCoordinates(cm.Tag).x, myStates.myCoordinates(cm.Tag).y)
'        Cursor.Position = l
'        SetColour()

'        With cSelected()
'            .Red = PictureBox1.BackColor.R
'            .Green = PictureBox1.BackColor.G
'            .Blue = PictureBox1.BackColor.B
'        End With

'        selected = Nothing

'    End Sub

'    Private Sub frm_Colours_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize
'        With Me
'            .lst_Colour.Height = .Height - (70 + .PictureBox1.Height)
'        End With
'    End Sub
'End Class