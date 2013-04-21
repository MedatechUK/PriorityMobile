Public Class CtrlForm

    Public mGotFocus As Boolean = False

    Public Structure tField
        Dim Name As String
        Dim Title As String
        Dim AltEntry As ctrlText.tAltCtrlStyle
        Dim ValidExp As String
        Dim SQLValidation As String
        Dim SQLList As String
        Dim DefaultFromCtrl As Object
        Dim ctrlEnabled As Boolean
        Dim Data As String
        Dim MandatoryOnPost As Boolean
    End Structure

    Public Event InvokeData(ByVal ctrl As ctrlText, ByVal sql As String)
    Public Event validData(ByVal ctrl As ctrlText, ByVal Valid As Boolean)
    Public Event AltEntry(ByVal ctrl As ctrlText)
    Public Event EndAltEntry(ByVal ctrl As ctrlText, ByVal Top As Integer, ByVal left As Integer, ByVal width As Integer, ByVal height As Integer)
    Public Event isFocused(ByVal HasFocus As Boolean)

    Private mTop As Integer
    Private mleft As Integer
    Private mWidth As Integer
    Private mHeight As Integer

    Public el() As ctrlText
    Dim txt() As ctrlText
    Dim mCol() As tField
    Dim cIndex As Integer
    Dim mFieldHeight As Integer

    Public ReadOnly Property ColNo(ByVal ColumnName As String) As Integer
        Get
            Dim ret As Integer = -1
            If Not IsNothing(mCol) Then
                For i As Integer = 0 To UBound(mCol)
                    If LCase(Trim(ColumnName)) = LCase(Trim(mCol(i).Name)) Then
                        ret = i
                        Exit For
                    End If
                Next
            End If
            Return ret
        End Get
    End Property

    Public Property FieldHeight() As Integer

        ' Get and set Height of the fields
        Get
            FieldHeight = mFieldHeight
        End Get
        Set(ByVal Height As Integer)
            mFieldHeight = Height
        End Set

    End Property

    Public ReadOnly Property InternalHeight() As Integer

        Get
            Return (UBound(el) + 1) * mFieldHeight ' el(UBound(el)).Top + el(UBound(el)).Height
        End Get

    End Property

    Public ReadOnly Property ItemValue(ByVal strName As String) As String
        Get
            Try
                For i As Integer = 0 To UBound(mCol)
                    If LCase(Trim(mCol(i).Name)) = LCase(Trim(strName)) Then
                        Return el(i).Data
                    End If
                Next
                Return ""
            Catch
                Return ""
            End Try

        End Get
    End Property

    Public Sub AddField(ByVal Col As tField)

        Try
            ReDim Preserve mCol(UBound(mCol) + 1)
        Catch ex As Exception
            ReDim mCol(0)
        End Try

        mCol(UBound(mCol)) = Col

    End Sub

    Public Sub DrawCtrls(ByRef sender As Object)

        'el = New ArrayList

        For i As Integer = 0 To UBound(mCol)

            Try
                ReDim Preserve txt(UBound(txt) + 1)
            Catch
                ReDim txt(0)
            End Try

            Try

                txt(UBound(txt)) = New ctrlText


                With txt(UBound(txt))
                    .Name = mCol(i).Name
                    '.Parent = Me
                    .CtrlDes = mCol(i).Title
                    .ValidExp = mCol(i).ValidExp
                    .SQLValidation = mCol(i).SQLValidation
                    .ListExp = mCol(i).SQLList
                    .Label = mCol(i).Title & ":"
                    .Data = mCol(i).Data
                    .AltCtrlStyle = mCol(i).AltEntry
                    .Height = mFieldHeight
                    .Width = Me.Width
                    .Top = i * mFieldHeight
                    .CtrlEnabled = mCol(i).ctrlEnabled
                    .Visible = True
                End With

                'If Not mCol(i).DefaultFromCtrl Is Nothing Then
                '    txt(UBound(txt)).Data = mCol(i).DefaultFromCtrl.data
                'End If

                AddHandler txt(UBound(txt)).ScanBegin, AddressOf Me.Handles_ScanBegin
                AddHandler txt(UBound(txt)).InvokeData, AddressOf Me.handles_SQLValidation
                AddHandler txt(UBound(txt)).ValidData, AddressOf Me.handles_validData
                AddHandler txt(UBound(txt)).ClickMe, AddressOf Me.Handles_clickme
                'AddHandler txt(UBound(txt)).AltEntry, AddressOf Me.Handles_AltEntry
                'AddHandler txt(UBound(txt)).EndAltEntry, AddressOf Me.Handles_EndAltEntry

            Catch x As Exception
                MsgBox(x.Message)
            End Try

            Try
                ReDim Preserve el(UBound(el) + 1)

            Catch ex As Exception
                ReDim el(0)
            End Try

            el(UBound(el)) = txt(UBound(txt))
            'el(UBound(el)).InvokeList()

        Next

        For i As Integer = 0 To UBound(el)
            el(i).Parent = Me
        Next

    End Sub

    Public Sub DisposeCtrls()

        Dim i As Integer

        If Not IsNothing(el) Then
            For i = 0 To UBound(el)
                el(i).Dispose()
            Next
        End If

        el = Nothing
        mCol = Nothing
        txt = Nothing

    End Sub

    Public Sub SetFocusActive()

        Dim i As Integer
        For i = 0 To UBound(el)
            If el(i).DataEntry.Visible = True Then
                el(i).DataEntry.Focus()
                Exit For
            End If
        Next
        Me.mGotFocus = True

    End Sub

    Public Sub DoLostFocus()

        If Not IsNothing(el) Then
            For i As Integer = 0 To UBound(el)
                el(i).LostFocus()
            Next
        End If
        Me.mGotFocus = False

    End Sub

    '*************************************************************************
    'Handlers
    '*************************************************************************

    Private Sub Handles_ScanBegin(ByVal ctrl As ctrlText)

        Dim i As Integer

        For i = 0 To UBound(el)
            If Not (el(i).Name = ctrl.Name) Then
                el(i).LostFocus()
            End If
        Next

    End Sub

    Private Sub handles_SQLValidation(ByVal ctrl As ctrlText, ByVal sql As String)

        RaiseEvent InvokeData(ctrl, sql)

    End Sub

    Private Sub handles_validData(ByVal ctrl As ctrlText, ByVal Valid As Boolean)

        Dim i As Integer
        Dim f As Integer
        Dim fnd As Boolean = False
        Dim ExitDir As Integer ' 0=unspecified 1=up arrow 2=down arrow

        If Valid Then RaiseEvent validData(ctrl, Valid)

        Select Case Valid
            Case False
                ctrl.BeginScan()

            Case True                
                For i = 0 To UBound(el)
                    If el(i).Name = ctrl.Name Then
                        fnd = True
                        ExitDir = el(i).ExitDir
                        f = i
                        Exit For
                    End If
                Next

                If fnd Then
                    Select Case ExitDir
                        Case 0
                            If Not f = UBound(el) Then
                                MoveCursor(f + 1, 1, False)
                            Else
                                MoveCursor(0, 1, False)
                            End If

                        Case -1
                            If Not f = 0 Then
                                MoveCursor(f - 1, -1)                                
                            End If

                        Case -2
                            If Not f = 0 Then
                                MoveCursor(f - 1, -1)
                            Else
                                MoveCursor(UBound(el), -1)
                            End If

                        Case 1
                            If Not f = UBound(el) Then
                                MoveCursor(f + 1, 1)                                
                            End If

                        Case 2
                            If Not f = UBound(el) Then
                                MoveCursor(f + 1, 1)
                            Else
                                MoveCursor(0, 1)                                
                            End If
                        Case 3
                            If Not f = UBound(el) Then
                                MoveCursor(f + 1, 1, False)
                            Else
                                MoveCursor(0, 1, False)
                            End If
                            'Do nothing
                    End Select

                End If

        End Select

    End Sub

    Sub MoveCursor(ByVal NextPos, ByVal St, Optional ByVal igsp = True)

        Dim i As Integer
        Dim en As Integer
        Dim s As Integer

        Select Case St
            Case 1
                en = UBound(el)
                s = 0
            Case -1
                en = 0
                s = UBound(el)
        End Select

        For i = NextPos To en Step St
            If el(i).CtrlEnabled Then
                If igsp Or el(i).Data = "" Then
                    el(i).BeginScan()
                    Exit Sub
                End If
            End If
        Next
        For i = s To NextPos Step St
            If igsp Or el(i).Data = "" Then
                If el(i).CtrlEnabled Then
                    el(i).BeginScan()
                    Exit Sub
                End If
            End If
        Next

    End Sub

    'Private Sub Handles_AltEntry(ByVal ctrl As Object)

    '    If ctrl.ismax Then RaiseEvent AltEntry(Me)

    '    With ctrl
    '        .SetPrevCtrlDim(ctrl.top, ctrl.left, ctrl.width, ctrl.height)
    '        .Top = 0
    '        .Left = 0
    '        .Width = ctrl.Parent.Width
    '        .Height = ctrl.Parent.Height
    '        .BringToFront()
    '    End With

    'End Sub

    'Private Sub Handles_EndAltEntry(ByVal ctrl As Object, ByVal Top As Integer, ByVal left As Integer, ByVal width As Integer, ByVal height As Integer)

    '    RaiseEvent EndAltEntry(Me, mTop, mleft, mWidth, mHeight)

    '    ctrl.top = Top
    '    ctrl.left = left
    '    ctrl.width = width
    '    ctrl.height = height

    'End Sub

    Public Sub SetPrevCtrlDim(ByVal top As Integer, ByVal left As Integer, ByVal width As Integer, ByVal height As Integer)
        mTop = top
        mleft = left
        mWidth = width
        mHeight = height
    End Sub

    Private Sub CtrlForm_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize

        Try
            Me.AutoScroll = CBool(InternalHeight > Me.Height)
            Dim intw As Integer = Me.Width
            If Me.AutoScroll Then intw = intw - 30
            Dim i As Integer
            For i = 0 To UBound(el)
                el(i).Width = intw
            Next
        Catch
        End Try

    End Sub

    Private Sub Handles_clickme()
        If Not mGotFocus Then RaiseEvent isFocused(True)
    End Sub

    Public Sub NameValues(ByRef Arr(,) As String)

        Dim i As Integer

        For i = 0 To UBound(el)
            Try
                ReDim Preserve Arr(1, UBound(Arr, 2) + 1)
            Catch ex As Exception
                ReDim Arr(1, 0)
            End Try

            Arr(0, UBound(Arr, 2)) = el(i).Name
            Arr(1, UBound(Arr, 2)) = el(i).Data

        Next
    End Sub

    Public Function CanPost(ByRef MissingAr() As String) As Boolean

        Dim i As Integer
        Dim cp As Boolean = True

        For i = 0 To UBound(mCol)
            If mCol(i).MandatoryOnPost And el(i).Data = "" Then
                cp = False
                Try
                    ReDim Preserve MissingAr(UBound(MissingAr) + 1)
                Catch ex As Exception
                    ReDim MissingAr(0)
                End Try
                MissingAr(UBound(MissingAr)) = mCol(i).Title
            End If
        Next
        Return cp

    End Function

End Class
