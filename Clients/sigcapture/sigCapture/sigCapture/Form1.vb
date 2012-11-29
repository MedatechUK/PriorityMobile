Imports System.Xml
Imports System.IO
Imports System.Net

Public Class Form1

#Region "Private Variables"

    Private coord As New List(Of Point)
    Private myPen As System.Drawing.Pen = New System.Drawing.Pen(System.Drawing.Color.Black)

    Private ReadOnly Property SendXML() As String
        Get
            Dim str As New System.Text.StringBuilder
            Dim xw As XmlWriter = XmlWriter.Create(str)
            With xw
                .WriteStartDocument()
                .WriteStartElement("signature")
                .WriteElementString("docno", txt_Docno.Text)
                .WriteElementString("vectors", vectors)
                .WriteEndElement()
                .WriteEndDocument()
                .Flush()
            End With
            Return str.ToString
        End Get
    End Property

    Private ReadOnly Property vectors() As String
        Get

            Dim bstr As String = ""
            Dim y As Integer
            Dim add As Boolean

            bstr = bstr & CStr(coord(0).X) & "," & CStr(coord(0).Y) & "."

            For y = 1 To coord.Count - 1
                If coord(y - 1).X = coord(y).X And coord(y - 1).Y = coord(y).Y Then
                    'same as last
                    add = False
                Else
                    add = True
                End If

                If add Then
                    bstr = bstr & CStr(coord(y).X) & "," & CStr(coord(y).Y) & "."
                End If
            Next

            Return bstr

        End Get
    End Property
#End Region

#Region "Local Control Handlers"

#Region "Signature"

    Private Sub Signature_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Signature.MouseDown
        If Not Me.txt_Docno.Enabled Then
            coord.Add(New Point(e.X, e.Y))
            Me.ToolBar1.Buttons(0).Enabled = True
        Else
            Me.txt_Docno.Focus()
            Beep()
        End If

    End Sub

    Private Sub Signature_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Signature.MouseMove
        If Not Me.txt_Docno.Enabled Then
            coord.Add(New Point(e.X, e.Y))
            If coord.Count Mod 2 = 1 Then Signature.Invalidate()
        End If
    End Sub

    Private Sub Signature_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Signature.MouseUp
        If Not Me.txt_Docno.Enabled Then
            coord.Add(New Point(0, 0))
            If coord.Count Mod 2 = 1 Then Signature.Invalidate()
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

#Region "Barcode"

    Private Sub txt_Docno_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Docno.GotFocus
        txt_Docno.BackColor = Color.Red
    End Sub

    Private Sub txt_Docno_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txt_Docno.KeyPress
        Select Case e.KeyChar
            Case Chr(13)
                Signature.Focus()
                txt_Docno.Enabled = False
        End Select
    End Sub

    Private Sub txt_Docno_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_Docno.LostFocus
        txt_Docno.BackColor = Color.Gray
    End Sub

#End Region

#Region "Toolbar"

    Private Sub ToolBar1_ButtonClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ToolBarButtonClickEventArgs) Handles ToolBar1.ButtonClick

        Select Case e.Button.ImageIndex
            Case 0 ' Send
                Dim posted As Boolean = False
                Dim strPostURL = "http://mobile.aerospheres.com:8080/postsig.ashx"
                Dim xmldata As String = SendXML
                Dim requestStream As Stream = Nothing
                Dim uploadResponse As Net.HttpWebResponse = Nothing
                Dim myEncoder As New System.Text.ASCIIEncoding
                Dim bytes As Byte() = myEncoder.GetBytes(xmldata)
                Dim ms As MemoryStream = New MemoryStream(bytes)

                Try

                    Dim uploadRequest As Net.HttpWebRequest = CType(Net.HttpWebRequest.Create(strPostURL), Net.HttpWebRequest)
                    uploadRequest.Method = "POST"
                    uploadRequest.Proxy = Nothing
                    uploadRequest.SendChunked = True
                    requestStream = uploadRequest.GetRequestStream()

                    ' Upload the XML
                    Dim buffer(1024) As Byte
                    Dim bytesRead As Integer
                    While True
                        bytesRead = ms.Read(buffer, 0, buffer.Length)
                        If bytesRead = 0 Then
                            Exit While
                        End If
                        requestStream.Write(buffer, 0, bytesRead)
                    End While

                    ' The request stream must be closed before getting the response.
                    requestStream.Close()

                    uploadResponse = uploadRequest.GetResponse()

                    Dim thisRequest As New XmlDocument
                    Dim reader As New StreamReader(uploadResponse.GetResponseStream)
                    With thisRequest
                        .LoadXml(reader.ReadToEnd)
                        Dim n As XmlNode = .SelectSingleNode("response")
                        Dim er As Boolean = False
                        For Each attrib As XmlAttribute In n.Attributes
                            If attrib.Name = "status" Then
                                If Not attrib.Value = "200" Then er = True
                            End If
                            If attrib.Name = "message" Then
                                If er Then
                                    Throw New Exception(attrib.Value)
                                End If
                            End If
                        Next
                    End With

                    posted = True

                Catch ex As UriFormatException
                    MsgBox(ex.Message)
                Catch ex As Net.WebException
                    MsgBox(ex.Message)
                Catch ex As Exception
                    MsgBox(ex.Message)
                Finally
                    If uploadResponse IsNot Nothing Then
                        uploadResponse.Close()
                    End If
                    If requestStream IsNot Nothing Then
                        requestStream.Close()
                    End If
                End Try

                If posted Then FormReset()

            Case 1 ' delete
                If MsgBox("Cancel without posting?", MsgBoxStyle.OkCancel) = MsgBoxResult.Ok Then
                    FormReset()
                End If
        End Select
    End Sub

    Private Sub FormReset()
        coord = New List(Of Point)
        Signature.Invalidate()
        With txt_Docno
            .Text = ""
            .Enabled = True
            .Focus()
        End With
        Me.ToolBar1.Buttons(0).Enabled = False
    End Sub

#End Region

#End Region

End Class
