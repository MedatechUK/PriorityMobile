'Imports cargo3
'Imports hookey
'Imports prnscn.capture

'Public Class frm_Coordinates

'    Dim selected As Integer = Nothing

'    Private Sub frm_Coordinates_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
'        With myStates
'            For Each c As cargo3.Coordinate In .myCoordinates.Values
'                With lst_Coord.Items
'                    .Add(c.Name)
'                    With .Item(.Count - 1)
'                        .SubItems.Add(c.x.ToString)
'                        .SubItems.Add(c.y.ToString)
'                    End With
'                End With
'            Next
'        End With
'    End Sub

'    Private Function cSelected() As Coordinate
'        If myStates.myCoordinates.Keys.Contains(lst_Coord.Items(selected).SubItems(0).Text) Then
'            Return myStates.myCoordinates(lst_Coord.Items(selected).SubItems(0).Text)
'        Else
'            myStates.myCoordinates.Add(lst_Coord.Items(selected).SubItems(0).Text, New Coordinate(lst_Coord.Items(selected).SubItems(0).Text, 0, 0))
'            Return myStates.myCoordinates(lst_Coord.Items(selected).SubItems(0).Text)
'        End If
'    End Function

'    Private Sub lst_Coord_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lst_Coord.DoubleClick

'        selected = lst_Coord.SelectedIndices(0)

'        Dim P As New Point(cSelected.x, cSelected.y)
'        Cursor.Position = P
'        SetColour()

'        Dim selecting As New System.Timers.Timer
'        With selecting
'            .Interval = 100
'            AddHandler .Elapsed, AddressOf hSelTimer
'            .Start()
'        End With

'        KeyWait(VK_ESCAPE)

'        selecting.Stop()
'        selecting.Dispose()

'        SetCoord()
'        With cSelected()
'            .x = Cursor.Position.X.ToString
'            .y = Cursor.Position.Y.ToString
'        End With

'        selected = Nothing        

'    End Sub

'    Public Sub SetCoord()
'        If Me.InvokeRequired Then
'            Me.Invoke(New MethodInvoker(AddressOf SetCoord))
'        Else
'            With lst_Coord.Items(selected)
'                .SubItems(1).Text = Cursor.Position.X.ToString
'                .SubItems(2).Text = Cursor.Position.Y.ToString
'            End With
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

'        SetCoord()
'        SetColour()

'    End Sub

'    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
'        With lst_Coord.Items
'            Dim NewName As String = InputBox("Coordinate Name")
'            If NewName.Length > 0 Then
'                .Add(NewName)
'                With .Item(.Count - 1)
'                    .SubItems.Add(0)
'                    .SubItems.Add(0)
'                End With
'            End If
'        End With

'    End Sub

'    Private Sub frm_Coordinates_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize
'        With Me
'            .lst_Coord.Height = .Height - (.PictureBox1.Height + 60)
'        End With
'    End Sub

'End Class