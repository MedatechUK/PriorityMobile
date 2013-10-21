Imports System.IO
Imports System.Xml

Public Class Project : Inherits System.Collections.Generic.Dictionary(Of String, Folder)

    Private proj As New XmlDocument()

    Public Sub New(ByVal Title As String, ByVal SolutionFile As String, ByVal RelativePath As String)
        _Title = Title

        Me.Add("", New Folder("", 1))

        While RelativePath.Substring(0, 3) = "..\"
            RelativePath = RelativePath.Substring(3)
            SolutionFile = SolutionFile.Substring(0, SolutionFile.LastIndexOf("\"))
        End While

        If SolutionFile.Last = ":" Then SolutionFile &= "\"
        _Filename = (IO.Path.Combine(SolutionFile, RelativePath))

        If Not File.Exists(Filename) Then
            Throw New Exception(String.Format("{0} not found.", Filename))
        Else
            'Console.WriteLine("Adding Project: {0}", Filename)
            proj.Load(Filename)
            For Each vb As XmlNode In proj.ChildNodes(1).ChildNodes
                If vb.Name = "ItemGroup" Then
                    For Each comp As XmlNode In vb.ChildNodes
                        If comp.Name = "Compile" Then
                            AddFile(comp.Attributes("Include").Value)
                            'For Each dep As XmlNode In comp.ChildNodes
                            '    If dep.Name = "DependentUpon" Then
                            '        AddFile(dep.InnerText)
                            '    End If
                            'Next
                        End If
                    Next
                End If
            Next
        End If

    End Sub

    Private Sub AddFile(ByVal fn As String)
        If Not IgnoreFolder(fn) And IsFileType(fn, "vb") Then
            'Console.WriteLine("   Adding File: {0}", fn)
            Dim fldpart() As String = fn.Split("\")
            Dim fld As Folder = Me("")
            For i As Integer = 0 To UBound(fldpart) - 1
                If Not fld.Keys.Contains(fldpart(i)) Then
                    fld.Add(fldpart(i), New Folder(fldpart(i), i + 2))
                End If
                fld = fld(fldpart(i))
            Next
            If Not fld.Files.keys.Contains(fldpart(UBound(fldpart))) Then
                fld.Files.Add( _
                    fldpart(UBound(fldpart)), _
                    New CodeFile(Me, _
                      IO.Path.Combine(_Filename.Substring(0, _Filename.LastIndexOf("\")), fn) _
                    ) _
                 )
            End If
        End If
    End Sub

    Private Function IsFileType(ByVal fn As String, ByVal TypeStr As String) As Boolean
        Return String.Compare(fn.Split(".").Last.ToLower, TypeStr) = 0
    End Function

    Private Function IgnoreFolder(ByVal Fn As String) As Boolean
        Dim ret As Boolean = False
        Try
            If Fn.Substring(0, 10) = "My Project" Then
                Return True
            End If
        Catch
            Return False
        End Try

    End Function

    Private _Title As String
    Public ReadOnly Property Title() As String
        Get
            Return _Title
        End Get
    End Property

    Private _Filename As String
    Public ReadOnly Property Filename() As String
        Get
            Return _Filename
        End Get
    End Property

End Class
