Public Class DisplayStudent

    Private rndcount As Integer = 0
    Private myStudents As List(Of String)
    Private RandomClass As New Random()
    Private _RollDelay As Integer
    Private _FlashDelay As Integer

    Public Sub New(ByVal Students As List(Of String), Optional ByVal RollDelay As Integer = 50, Optional ByVal Flashdelay As Integer = 20)
        InitializeComponent()
        myStudents = Students
        _RollDelay = RollDelay
        _FlashDelay = Flashdelay
    End Sub

    Private Sub DisplayStudent_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        With lblStudent
            .ForeColor = Color.Blue
            .Text = RandomStudent()
        End With
    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        rndcount += 1
        With lblStudent
            .Text = RandomStudent()
            If rndcount = _RollDelay Then
                rndcount = 0
                Timer1.Enabled = False
                .ForeColor = Color.Red
                Timer2.Enabled = True
            End If
        End With
    End Sub

    Private Function RandomStudent() As String             
        Return myStudents(RandomClass.Next(myStudents.Count))
    End Function

    Private Sub Timer2_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer2.Tick
        rndcount += 1
        With lblStudent
            Select Case rndcount Mod 2
                Case 1
                    .ForeColor = Color.Red
                    If rndcount < (_FlashDelay / 4) * 3 Then
                        .Font = New Font(.Font.Name, .Font.Size + 1, FontStyle.Bold)
                    Else
                        .Font = New Font(.Font.Name, .Font.Size - 1, FontStyle.Bold)
                    End If
                Case Else
                    .ForeColor = Color.Blue
            End Select
            If rndcount = _FlashDelay Then
                rndcount = 0
                Timer2.Enabled = False
                Me.Close()
            End If
        End With
    End Sub

End Class