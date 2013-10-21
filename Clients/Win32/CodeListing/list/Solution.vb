Imports System.IO
Imports System.Text.RegularExpressions

Public Class Solution

    Public Sub New()
        _FileName = "\Shared"
    End Sub

    Public Sub New(ByRef Solutions As Dictionary(Of String, Solution), ByVal FileName As String)
        _FileName = FileName
        If File.Exists(FileName) Then
            Using sr As New StreamReader(FileName)
                For Each mtch As String In rxMatch( _
                    New Regex( _
                        "\nProject.*" _
                    ), _
                    sr.ReadToEnd _
                )
                    Dim proj As String() = mtch.Split("=")(1).Trim.Replace(Chr(34), "").Split(", ")
                    If proj(1).Trim.Split(".").Last = "vbproj" Then

                        Dim F As Boolean = False
                        Dim pname As String = proj(1).Trim.Substring(proj(1).Trim.LastIndexOf("\") + 1)
                        If Not (Solutions("Shared").Projects.Keys.Contains(pname)) Then
                            For Each key As String In Solutions.Keys
                                If Not key = "Shared" Then
                                    If Solutions(key).Projects.Keys.Contains(pname) Then
                                        F = True
                                        Solutions("Shared").Projects.Add(pname, Solutions(key).Projects(pname))
                                        Solutions(key).Projects.Remove(pname)
                                        Exit For
                                    End If
                                End If
                            Next
                        End If

                        If Not F Then
                            _Projects.Add(pname, New Project(proj(0), FileName.Substring(0, FileName.LastIndexOf("\")), proj(1).Trim))
                        End If

                    End If

                Next
            End Using
        Else
            Throw New Exception(String.Format("Invalid Solution File [{0}].", FileName))
        End If
    End Sub

    Private _FileName As String
    Private Property FileName() As String
        Get
            Return _FileName
        End Get
        Set(ByVal value As String)
            _FileName = value
        End Set
    End Property

    Private _Name As String
    Public ReadOnly Property Name() As String
        Get
            Return _FileName.Substring(_FileName.LastIndexOf("\") + 1)
        End Get        
    End Property

    Private _Projects As New Dictionary(Of String, Project)
    Public Property Projects() As Dictionary(Of String, Project)
        Get
            Return _Projects
        End Get
        Set(ByVal value As Dictionary(Of String, Project))
            _Projects = value
        End Set
    End Property

    Private Function rxMatch(ByVal Pattern As Regex, ByRef SearchString As String) As List(Of String)
        Dim ret As New List(Of String)
        Dim M As Match = Pattern.Match(SearchString)
        Do While M.Success
            If Not ret.Contains(M.Value) Then
                ret.Add(M.Value)
            End If
            M = M.NextMatch
        Loop
        Return ret
    End Function

End Class
