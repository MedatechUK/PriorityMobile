Imports System.Xml
Imports System.IO

Public Class xForm

#Region "Private variables"

    Private ControlName As New List(Of String)
    Private RejectingChanges As Boolean = False

#End Region

#Region "Event Declarations"

    Public Event AddUserControl(ByVal ControlName As String, ByRef view As iView, ByRef thisForm As xForm)
    Public Event DrawSubMenu()
    Public Event DrawDirectActivations()
    Public Event DrawForm()
    Public Event StartCalc(ByVal Max As Integer)
    Public Event StartDialog(ByVal frmDialog As PriorityMobile.UserDialog)

#End Region

#Region "Initialisation and finalisation"

    Public Sub New(ByRef thisnode As XmlNode, ByVal xPath As String, ByVal Parent As xForm)
        Try
            With Me
                .thisNode = thisnode
                .Parent = Parent
                .xPath = xPath
                .FormName = thisnode.Attributes("name").Value
                If Not IsNothing(thisnode.Attributes("key")) Then
                    .KeyColumn = thisnode.Attributes("key").Value
                End If
            End With

            If thisnode.SelectNodes("form").Count > 0 Then
                For Each sf As XmlNode In thisnode.SelectNodes("form")
                    SubForms.Add(sf.Attributes("name").Value, New xForm(sf, sf.Attributes("xpath").Value, Me))
                Next
            End If

            If thisnode.SelectNodes("views/view").Count > 0 Then
                For Each vw As XmlNode In thisnode.SelectNodes("views/view")
                    ControlName.Add(vw.Attributes("control").Value.ToUpper)
                Next
            Else
                Throw New Exception(String.Format("Form {0} has no views", thisnode.Attributes("name").Value))
            End If

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

#End Region

#Region "Public Properties"

    Public Property FormData() As XmlDocument
        Get
            Return xmlForms.FormData.Document
        End Get
        Set(ByVal value As XmlDocument)
            xmlForms.FormData.Document = value
        End Set
    End Property

    Public ReadOnly Property LookUp() As lookupData
        Get
            Return xmlForms.DataLookUp
        End Get
    End Property

    Public ReadOnly Property TopForm() As Dictionary(Of String, TopLevelForm)
        Get
            Return xmlForms.TopForm
        End Get
    End Property

    Private _KeyColumn As String = Nothing
    Public Property KeyColumn() As String
        Get
            Return _KeyColumn
        End Get
        Set(ByVal value As String)
            _KeyColumn = value
        End Set
    End Property

    Private WithEvents _TableData As New BindingSource
    Public Property TableData() As BindingSource
        Get
            Return _TableData
        End Get
        Set(ByVal value As BindingSource)
            _TableData = value
        End Set
    End Property

    Private _thisNode As XmlNode
    Public Property thisNode() As XmlNode
        Get
            Return _thisNode
        End Get
        Set(ByVal value As XmlNode)
            _thisNode = value
        End Set
    End Property

    Private _FormName As String
    Public Property FormName() As String
        Get
            Return _FormName
        End Get
        Set(ByVal value As String)
            _FormName = value
        End Set
    End Property

    Private _xpath As String
    Public Property xPath() As String
        Get
            Return _xpath
        End Get
        Set(ByVal value As String)
            _xpath = value
        End Set
    End Property

    Public ReadOnly Property thisxPath()
        Get
            Dim path As String = ""
            Dim p As xForm = Me
            Dim k As String
            Dim repeat As Boolean = True
            Do
                If Not IsNothing(p.thisNode.Attributes("key")) And Not IsNothing(p.TableData.Current) Then
                    k = parseKey(p)
                Else
                    k = ""
                End If
                path = String.Format("{0}{1}{2}", p.xPath, k, path)
                If Not IsNothing(p.Parent) Then
                    p = p.Parent
                Else
                    repeat = False
                End If
            Loop Until Not repeat
            Return path
        End Get
    End Property

    Public ReadOnly Property boundxPath()
        Get
            Dim path As String = Me.xPath
            Dim p As xForm = Me.Parent
            Dim k As String
            Dim repeat As Boolean = True
            If Not IsNothing(p) Then
                Do
                    If Not IsNothing(p.thisNode.Attributes("key")) And Not IsNothing(p.TableData.Current) Then
                        k = parseKey(p)
                    Else
                        k = ""
                    End If
                    path = String.Format("{0}{1}{2}", p.xPath, k, path)
                    If Not IsNothing(p.Parent) Then
                        p = p.Parent
                    Else
                        repeat = False
                    End If
                Loop Until Not repeat
            End If
            Return path
        End Get
    End Property

    Private Function parseKey(ByVal p As xForm) As String

        Dim bstr As String = ""
        Dim keys() As String = Split(p.thisNode.Attributes("key").Value, ",")

        For i As Integer = 0 To keys.Count - 1
            bstr += String.Format( _
                "{1}={0}{2}{0}", _
                Chr(34), _
                keys(i), _
                p.CurrentRow(keys(i)) _
            )
            If i < keys.Count - 1 Then
                bstr += " and "
            End If
        Next

        Return String.Format( _
            "[{0}]", _
            bstr _
        )

    End Function

    Private _parent As xForm
    Public Property Parent() As xForm
        Get
            Return _parent
        End Get
        Set(ByVal value As xForm)
            _parent = value
        End Set
    End Property

    Private _subforms As New Dictionary(Of String, xForm)
    Public Property SubForms() As Dictionary(Of String, xForm)
        Get
            Return _subforms
        End Get
        Set(ByVal value As Dictionary(Of String, xForm))
            _subforms = value
        End Set
    End Property

    Private _Views As New List(Of iView)
    Public Property Views() As List(Of iView)
        Get
            Return _Views
        End Get
        Set(ByVal value As List(Of iView))
            _Views = value
        End Set
    End Property

    Public ReadOnly Property NextViewButton() As String
        Get
            If CurrentView + 1 > Me.Views.Count - 1 Then
                Return Views(0).ButtomImage
            Else
                Return Views(CurrentView + 1).ButtomImage
            End If
        End Get
    End Property

    Private _CurrentView As Integer = 0
    Public Property CurrentView() As Integer
        Get
            Return _CurrentView
        End Get
        Set(ByVal value As Integer)
            If value > Me.Views.Count - 1 Then
                _CurrentView = 0
            Else
                _CurrentView = value
            End If
            Views(CurrentView).CurrentChanged()
            RaiseEvent DrawDirectActivations()
            Views(CurrentView).ViewChanged()
        End Set
    End Property

    Public ReadOnly Property CurrentRow() As Data.DataRowView
        Get
            Return TableData.Current            
        End Get
    End Property

    Public ReadOnly Property Datasource() As DataTable
        Get            
            Return TableData.DataSource
        End Get
    End Property

#End Region

#Region "Public Methods"

    Public Sub Bind()

        Try
            If Me.xPath.Length = 0 Then Exit Sub

            Dim ds As New DataSet
            Dim node As XmlNode = xmlForms.FormData.Document.SelectSingleNode(Me.boundxPath)

            If Not IsNothing(node) Then

                Dim retstr As String = String.Format("<{0}>", node.ParentNode.Name)
                For Each n As XmlNode In xmlForms.FormData.Document.SelectNodes(Me.boundxPath)
                    retstr += Replace(n.OuterXml, String.Format(" changed={0}1{0}", Chr(34)), "", , , CompareMethod.Text)
                Next
                retstr += String.Format("</{0}>", node.ParentNode.Name)

                Dim MemoryStream As New System.IO.MemoryStream( _
                    System.Text.Encoding.UTF8.GetBytes( _
                        xmlForms.changedRegex.Replace(retstr, "") _
                        ) _
                    )

                MemoryStream.Seek(0, System.IO.SeekOrigin.Begin)
                ds.ReadXml(XmlReader.Create(MemoryStream))
                ds.EnforceConstraints = False

                Dim td As DataTable = ds.Tables(0)
                With td
                    RemoveHandler .ColumnChanged, AddressOf hColumnChanged
                    RemoveHandler .ColumnChanging, AddressOf hColumnChanging
                    RemoveHandler .TableNewRow, AddressOf hTableNewRow

                    AddHandler .ColumnChanged, AddressOf hColumnChanged
                    AddHandler .ColumnChanging, AddressOf hColumnChanging
                    AddHandler .TableNewRow, AddressOf hTableNewRow
                End With

                TableData.DataSource = td
                RemoveHandler TableData.CurrentChanged, AddressOf hCurrentChanged
                AddHandler TableData.CurrentChanged, AddressOf hCurrentChanged

                For Each V As iView In Me.Views
                    With V
                        .ClearBindings()
                        .Bind()
                        .Bound = True
                    End With
                Next

                RaiseEvent DrawSubMenu()
                RaiseEvent DrawDirectActivations()                

            Else
                Throw New Exception("xPath not found")
            End If

        Catch ex As Exception
            MsgBox(String.Format("Error {0} retrieving data with xpath {1}", ex.Message, _xpath))
        End Try

    End Sub

    Public Sub LoadViewControls()

        For Each s As String In ControlName
            Try
                Dim ViewControl As iView = Nothing
                RaiseEvent AddUserControl(s, ViewControl, Me)
                If Not IsNothing(ViewControl) Then
                    Me.Views.Add(ViewControl)
                Else
                    Throw New Exception( _
                        String.Format( _
                            "Form {1} requested view {0} that was not found in the user interface.", _
                            s, _
                            Me.FormName _
                          ) _
                    )
                End If
            Catch ex As Exception
                MsgBox(ex.Message)
            End Try
        Next

    End Sub

    Public Function VisibleSubForms() As Dictionary(Of String, xForm)
        Dim ret As New Dictionary(Of String, xForm)
        For Each SF As String In _subforms.Keys
            If Views(CurrentView).SubFormVisible(SF) Then
                ret.Add(SF, _subforms(SF))
            End If
        Next
        Return ret
    End Function

    Public Sub RefreshDirectActivations()
        RaiseEvent DrawDirectActivations()
    End Sub

    Public Sub RefreshSubForms()
        RaiseEvent DrawSubMenu()
    End Sub

    Public Sub RefreshForm()
        RaiseEvent DrawForm()
    End Sub

    Public Sub Save()
        With xmlForms.FormData
            .Document.Save(.LocalFile)
        End With
    End Sub

    Public Sub FormClosing()
        For Each View As iView In Views
            View.FormClosing()
        Next
    End Sub

    Public Sub Calc(ByVal Max As Integer)
        RaiseEvent StartCalc(Max)
    End Sub

    Public Sub Dialog(ByVal frmDialog As PriorityMobile.UserDialog)
        RaiseEvent StartDialog(frmDialog)
    End Sub

    Public Sub Log(ByVal FormatString As String, ByVal ParamArray Values() As String)
        xmlForms.Log(FormatString, values)
    End Sub

    Public Sub Log(ByVal FormatString As String)
        xmlForms.Log(FormatString)
    End Sub

    Public Function thisUserEnv() As UserEnv
        Return xmlForms.UserEnv
    End Function

    Public Function CreateNode(ByRef ParentNode As XmlNode, ByVal NodeName As String, Optional ByVal NodeInnerText As String = Nothing) As XmlNode
        With ParentNode
            .AppendChild(xmlForms.FormData.Document.CreateElement(NodeName))
            If Not IsNothing(NodeInnerText) Then
                .ChildNodes(.ChildNodes.Count - 1).InnerText = NodeInnerText
            End If
            Return .ChildNodes(.ChildNodes.Count - 1)
        End With
    End Function

    Public Function DateFromInt(ByVal IntDate As Integer) As Date
        Return DateAdd(DateInterval.Minute, IntDate, New Date(1988, 1, 1))
    End Function

    Public Overloads Function DateToInt(ByVal thisDate As Date) As Integer
        Return DateDiff(DateInterval.Minute, #1/1/1988#, thisDate)
    End Function

    Public Overloads Function DateToInt()
        Return DateDiff(DateInterval.Minute, #1/1/1988#, Now)
    End Function

    Public Overloads Function DateToInt8(ByVal thisDate As Date) As Integer
        Return DateDiff(DateInterval.Minute, #1/1/1988#, New Date(thisDate.Year, thisDate.Month, thisDate.Day))
    End Function

    Public Overloads Function DateToInt8() As Integer
        Return DateDiff(DateInterval.Minute, #1/1/1988#, New Date(Now.Year, Now.Month, Now.Day))
    End Function

    Public Function NowTimeToMin()
        Return DateDiff(DateInterval.Minute, New Date(Now.Year, Now.Month, Now.Day), Now)
    End Function

    Public Function Printer() As CPCL.LabelPrinter
        Return prn
    End Function

    Public Property MACAddress() As String
        Get
            Return prnmac
        End Get
        Set(ByVal value As String)
            prnmac = value
            Dim f As String = xmlForms.UserEnv.LocalFolder & "\prnmac.txt"
            While File.Exists(f)
                File.Delete(f)
                System.Threading.Thread.Sleep(100)
            End While
            Using sr As New StreamWriter(f)
                sr.Write(value)
            End Using
        End Set
    End Property

#End Region

#Region "Event Handlers"

    Private Sub hColumnChanged(ByVal sender As Object, ByVal e As System.Data.DataColumnChangeEventArgs)

        With xmlForms.FormData

            Dim n As XmlNode = .Document.SelectSingleNode(Me.thisxPath).SelectSingleNode(e.Column.ColumnName)

            If Not RejectingChanges Then

                If Not e.Row.HasErrors Then
                    If Not String.Compare(n.InnerText, e.ProposedValue) = 0 Then
                        With n
                            .InnerText = e.ProposedValue
                            .Attributes.Append(xmlForms.changedAttribute)
                        End With
                        .Document.Save(.LocalFile)
                        For Each View As iView In Views
                            View.RowUpdated(e.Column.ColumnName, e.ProposedValue)
                        Next
                        RaiseEvent DrawSubMenu()

                        Dim dt As DataTable = TableData.DataSource
                        dt.AcceptChanges()
                    End If

                Else
                    e.Row.ClearErrors()
                    With xmlForms.FormData
                        With n
                            RejectingChanges = True
                            e.Row(e.Column.ColumnName) = .InnerText
                            CurrentRow(e.Column.ColumnName) = .InnerText
                            RejectingChanges = False
                        End With
                    End With

                End If

            End If
        End With
    End Sub

    Private Sub hColumnChanging(ByVal sender As Object, ByVal e As System.Data.DataColumnChangeEventArgs)
        With xmlForms.FormData
            If Not RejectingChanges Then
                Dim n As XmlNode = .Document.SelectSingleNode(Me.thisxPath).SelectSingleNode(e.Column.ColumnName)
                If Not String.Compare(n.InnerText, e.ProposedValue) = 0 Then
                    If Not Views(CurrentView).ValidColumn(e.Column.ColumnName, e.ProposedValue) Then
                        e.Row.SetColumnError(e.Column.ColumnName, "Error")
                    End If
                End If
            End If
        End With
    End Sub

    Private Sub hTableNewRow(ByVal sender As Object, ByVal e As System.Data.DataTableNewRowEventArgs)

    End Sub

    Private Sub hCurrentChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        For Each V As iView In Me.Views
            V.CurrentChanged()
        Next
        RaiseEvent DrawSubMenu()
    End Sub

#End Region

End Class