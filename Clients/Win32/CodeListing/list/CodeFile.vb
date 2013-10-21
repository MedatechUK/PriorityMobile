Imports System.IO
Imports CodeFormatter

Public Class CodeFile

    Private _Project As Project
    Public ReadOnly Property Project() As Project
        Get
            Return _Project
        End Get
    End Property

    Public Sub New(ByRef ParentProject As Project, ByVal FilePath As String)

        Path = FilePath
        _Project = Project

        Dim bStr As New System.Text.StringBuilder
        Using sr As New StreamReader(FilePath)
            With bStr
                .AppendLine()
                .AppendFormat("'{0}", New String("*", 80)).Replace(" ", "&nbsp;").AppendLine()
                .AppendFormat("'{0}{1}{0}", New String("*", 3), New String(" ", 74)).Replace(" ", "&nbsp;").AppendLine()

                WriteFileProp(bStr, "Project", ParentProject.Title)
                WriteFileProp(bStr, "Project File", ParentProject.Filename)
                WriteFileProp(bStr, "File Name", Name)
                WriteFileProp(bStr, "File Path", Path.Replace(ParentProject.Filename.Substring(0, ParentProject.Filename.LastIndexOf("\")), ""))
                WriteFileProp(bStr, "Author", "SimonBarnett@emerge-it.co.uk")
                WriteFileProp(bStr, "Created ", Created)
                WriteFileProp(bStr, "Last Modified", LastModified)

                .AppendFormat("'{0}{1}{0}", New String("*", 3), New String(" ", 74)).Replace(" ", "&nbsp;").AppendLine()
                .AppendFormat("'{0}", New String("*", 80)).Replace(" ", "&nbsp;").AppendLine()
                .AppendLine()
                .Append(sr.ReadToEnd)
                .AppendLine()

                _CodeData = bStr.ToString
            End With
        End Using

    End Sub

    Private Sub WriteFileProp(ByRef sw As System.Text.StringBuilder, ByVal Name As String, ByVal Value As String)
        sw.AppendFormat("'{0}{1}{0}", _
            New String( _
                "*", _
                3 _
            ), _
            String.Format( _
                "{0}{1}", _
                String.Format( _
                    "   {0}: {1}", _
                    Name, _
                    Value _
                ), _
                New String( _
                    " ", _
                    74 _
                ) _
            ).Substring(0, 74).Replace(" ", "&nbsp;") _
        ).AppendLine()
    End Sub

    Private _Name As String
    Public Property Name() As String
        Get
            Return _Name
        End Get
        Set(ByVal value As String)
            _Name = value
        End Set
    End Property

    Private _Path As String
    Public Property Path() As String
        Get
            Return _Path
        End Get
        Set(ByVal value As String)
            _Path = value
            _Name = value.Split("\").Last
        End Set
    End Property

    Private _CodeData As String
    Public Property CodeData() As String
        Get
            Dim fmt As New CodeFormatterExtension
            Dim op As New CodeFormatterExtension.HighlightOptions
            op.DisplayLineNumbers = True
            op.Language = "vb"
            op.Title = Name

            _CodeData = _CodeData.Replace(vbCrLf & "    ", vbCrLf & Chr(9))
            While InStr(_CodeData, Chr(9) & "    ") > 0
                _CodeData = _CodeData.Replace(Chr(9) & "    ", Chr(9) & Chr(9))
            End While
            Return fmt.Highlight(op, _CodeData)

        End Get
        Set(ByVal value As String)
            _CodeData = value
        End Set
    End Property

    Public ReadOnly Property Created() As String
        Get
            Return File.GetCreationTime(Path).ToString("dd/MM/yy")
        End Get
    End Property

    Public ReadOnly Property LastModified() As String
        Get
            Return File.GetLastWriteTime(Path).ToString("dd/MM/yy")
        End Get
    End Property

End Class
