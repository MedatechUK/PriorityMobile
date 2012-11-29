Imports ViewControl
Imports System.Drawing
Imports System.Xml

Public Class ctrl_Sign
    Inherits ViewControl.iView

#Region "Private Variables"

    Private coord As New List(Of Point)
    Private myPen As System.Drawing.Pen = New System.Drawing.Pen(System.Drawing.Color.Black)

#End Region

#Region "Local Control Handlers"

    Private Sub Signature_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Signature.MouseDown
        If IsOnsite() Then coord.Add(New Point(e.X, e.Y))
    End Sub

    Private Sub Signature_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Signature.MouseMove, Signature.MouseUp
        If IsOnsite() Then
            coord.Add(New Point(e.X, e.Y))
            If coord.Count Mod 3 = 1 Then Signature.Invalidate()
        End If
    End Sub

    Private Sub Signature_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles Signature.Paint
        For i As Integer = 1 To coord.Count - 1
            If Not (coord(i - 1).X = 0 And coord(i - 1).Y = 0) And Not (coord(i).X = 0 And coord(i).Y = 0) Then
                e.Graphics.DrawLine(myPen, _
                    coord(i - 1).X, _
                    coord(i - 1).Y, _
                    coord(i).X, _
                    coord(i).Y)
            End If
        Next
    End Sub

#End Region

#Region "Overrides base methods"

    Public Overrides Sub Bind()
        With Me
            .txt_PrintName.DataBindings.Add("Text", thisForm.TableData, "print")
            .txt_PrintName.Enabled = IsOnsite()
            DrawSignature()
        End With
    End Sub

    Public Overrides Sub CurrentChanged()
        With Me
            .txt_PrintName.Enabled = IsOnsite()
            .DrawSignature()
        End With
    End Sub

    Public Overrides Sub FormClosing()
        With thisForm
            With .FormData
                Dim n As XmlNode = thisForm.FormData.SelectSingleNode(thisForm.thisxPath & "/image")                
                With n
                    .Attributes.Append(xmlForms.changedAttribute)
                    .InnerText = ""
                    For Each p As Point In coord
                        .InnerText += String.Format("{0},{1};", p.X, p.Y)
                    Next
                End With
            End With
            .Save()
        End With
    End Sub

#End Region

#Region "Local Methods"

    Private Sub DrawSignature()
        With Me
            .coord.Clear()
            Dim points() As String = Split(.thisForm.TableData.Current("image"), ";")
            For Each p As String In points
                If p.Length > 0 Then
                    coord.Add(New Point(CInt(Split(p, ",")(0)), CInt(Split(p, ",")(1))))
                End If
            Next
            .Signature.Invalidate()
        End With
    End Sub

    Private Function IsOnsite() As Boolean
        Return String.Compare(thisForm.Parent.TableData.Current("callstatus"), "On-Site") = 0
    End Function

#End Region

#Region "Direct Activations"

    Public Overrides Sub DirectActivations(ByRef ToolBar As System.Windows.Forms.ToolStrip)
        ToolBar.Items.Add("", Image.FromFile("icons\DELETE.BMP"), AddressOf hClearSignature)
    End Sub

    Private Sub hClearSignature()
        coord.Clear()
        Signature.Invalidate()
    End Sub

#End Region

End Class
