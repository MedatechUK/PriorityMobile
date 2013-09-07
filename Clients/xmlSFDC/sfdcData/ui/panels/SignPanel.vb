Imports System.Windows.Forms
Imports System.Drawing
Imports System.Xml
Imports System.IO

Public Class SignPanel
    Inherits iFormPanel

#Region "Inheritance"

    Private _ParentForm As iForm
    Public Overrides ReadOnly Property ParentForm() As iForm
        Get
            Return _ParentForm
        End Get
    End Property

#End Region

#Region "Private Properties"

    Private myPen As System.Drawing.Pen = New System.Drawing.Pen(System.Drawing.Color.Black)
    Private coord As New List(Of Point)

    Private _Signature As New PictureBox
    Public Property Signature() As PictureBox
        Get
            Return _Signature
        End Get
        Set(ByVal value As PictureBox)
            _Signature = value
        End Set
    End Property

    Private _dlgOk As New Button
    Public Property dlgOk() As Button
        Get
            Return _dlgOk
        End Get
        Set(ByVal value As Button)
            _dlgOk = value
        End Set
    End Property

    Private _dlgCancel As New Button
    Public Property dlgCancel() As Button
        Get
            Return _dlgCancel
        End Get
        Set(ByVal value As Button)
            _dlgCancel = value
        End Set
    End Property

    Private _enabled As Boolean = True

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

    Private ReadOnly Property SendXML() As String
        Get
            Dim str As New System.Text.StringBuilder
            Dim xw As XmlWriter = XmlWriter.Create(str)
            With xw
                .WriteStartDocument()
                .WriteStartElement("signature")
                .WriteElementString("vectors", vectors)
                .WriteEndElement()
                .WriteEndDocument()
                .Flush()
            End With
            Return str.ToString
        End Get
    End Property

#End Region

#Region "Initialisation and Finalisation"

    Public Event SaveSignature(ByRef Result As String)

    Public Sub New()

        With Me.Controls

            With Signature
                AddHandler .MouseDown, AddressOf Signature_MouseDown
                AddHandler .MouseMove, AddressOf Signature_MouseMove
                AddHandler .MouseUp, AddressOf Signature_MouseUp
                AddHandler .Paint, AddressOf Signature_Paint
            End With

            .Add(Signature)
            With .Item(.Count - 1)
                .Visible = True
                .BackColor = Color.Red
            End With

            Dim btnPanel As New Panel
            .Add(btnPanel)
            With btnPanel
                .Dock = DockStyle.Bottom

                With btnPanel.Controls
                    With dlgOk
                        .DialogResult = DialogResult.OK
                        .Text = "Ok"
                        AddHandler .Click, AddressOf hButtonClick
                    End With
                    .Add(dlgOk)
                    With .Item(.Count - 1)
                        .Visible = True
                        .Dock = DockStyle.Left
                    End With

                    With dlgCancel
                        .DialogResult = DialogResult.Cancel
                        .Text = "Cancel"
                        AddHandler .Click, AddressOf hButtonClick
                    End With
                    .Add(dlgCancel)
                    With .Item(.Count - 1)
                        .Visible = True
                        .Dock = DockStyle.Right
                    End With

                End With
                .Height = btnPanel.Controls(0).Height
            End With
        End With

    End Sub

    Public Sub Load(ByRef thisForm As iForm)
        _ParentForm = thisForm
    End Sub

#End Region

#Region "Local Control Handlers"

    Private Sub Signature_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs)
        If _enabled Then coord.Add(New Point(e.X, e.Y))
    End Sub

    Private Sub Signature_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs)
        If _enabled Then
            coord.Add(New Point(e.X, e.Y))
            If coord.Count Mod 3 = 1 Then Signature.Invalidate()
        End If
    End Sub

    Private Sub Signature_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs)
        coord.Add(New Point(0, 0))
        If coord.Count Mod 2 = 1 Then Signature.Invalidate()
    End Sub

    Private Sub Signature_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs)
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

    Private Sub hButtonClick(ByVal sender As Object, ByVal e As System.EventArgs)

        Select Case TryCast(sender, Button).DialogResult
            Case DialogResult.OK

                Dim result As String = String.Empty
                Dim posted As Boolean = False
                Dim strPostURL = String.Format("{0}postsig.ashx", ParentForm.ue.Server)
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
                            If attrib.Name = "result" Then
                                result = attrib.Value
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
                    RaiseEvent SaveSignature(result)
                End Try

            Case Else
                RaiseEvent SaveSignature(String.Empty)

        End Select
    End Sub

#End Region

#Region "Public Methods"

    Public Sub DrawSignature(ByVal Serialdata As String)
        With Me
            .coord.Clear()
            Dim points() As String = Split(Serialdata, "\n")
            For Each p As String In points
                If p.Length > 0 Then
                    coord.Add(New Point(CInt(Split(p, "\t")(0)), CInt(Split(p, "\t")(1))))
                End If
            Next
            .Signature.Invalidate()
        End With
    End Sub

    Public Sub Clear()
        coord.Clear()
        Signature.Invalidate()
    End Sub

    Public Function toSerial() As String
        Dim ret As String = ""
        For Each p As Point In coord
            ret += String.Format("{0}\t{1}\n", p.X, p.Y)
        Next
        Return ret
    End Function

    Public Sub ResizeMe(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Resize

        With Signature
            .Top = 3
            .Left = 3
            .Width = Screen.PrimaryScreen.WorkingArea.Width - 6
            .Height = ParentForm.Height - (dlgOk.Height + 6)
        End With

        With dlgOk
            .Width = Screen.PrimaryScreen.WorkingArea.Width / 2
        End With

        With dlgCancel
            .Width = Screen.PrimaryScreen.WorkingArea.Width / 2
        End With

    End Sub

#End Region

End Class
