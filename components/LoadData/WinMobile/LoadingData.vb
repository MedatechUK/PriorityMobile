Public Enum tAltCtrlStyle
    ctNone = 0
    ctCalc = 1
    ctKeyb = 2
    ctDate = 3
    ctList = 4
End Enum

Public Structure tCol
    Dim Name As String
    Dim Title As String
    Dim initWidth As Integer
    'Dim TextAlign As System.Windows.Forms.HorizontalAlignment
    Dim AltEntry As tAltCtrlStyle
    Dim ValidExp As String
    Dim SQLValidation As String
    Dim SQLList As String
    Dim DefaultFromCtrl As Object
    Dim ctrlEnabled As Boolean
    Dim Mandatory As Boolean
    Dim MandatoryOnPost As Boolean
End Structure

Public Structure Load

    Private m_t1Records()
    Private m_t2Records()

    Private m_DebugFlag As Boolean
    Private m_Table As String
    Private m_Procedure As String
    Private m_rec1 As String()
    Private m_rec2 As String()
    Private m_typ As String()

    Private m_t1Record As String(,)
    Private m_t2Record As String(,)

    Public WriteOnly Property DebugFlag() As Boolean
        Set(ByVal value As Boolean)
            m_DebugFlag = value
        End Set
    End Property

    Public WriteOnly Property Table() As String
        Set(ByVal value As String)
            m_Table = value
        End Set
    End Property

    Public WriteOnly Property Procedure() As String
        Set(ByVal value As String)
            m_Procedure = value
        End Set
    End Property

    Public WriteOnly Property RecordType1() As String
        Set(ByVal value As String)
            m_rec1 = str2ar(value)
        End Set
    End Property

    Public WriteOnly Property RecordType2() As String
        Set(ByVal value As String)
            m_rec2 = str2ar(value)
        End Set
    End Property

    Public WriteOnly Property RecordTypes() As String
        Set(ByVal value As String)
            m_typ = str2ar(value)
        End Set
    End Property

    Public WriteOnly Property AddRecord(ByVal type As Integer) As String()
        Set(ByVal value As String())
            Try
                Select Case type
                    Case 1
                        ReDim Preserve m_t1Records(UBound(m_t1Records) + 1)
                    Case 2
                        ReDim Preserve m_t2Records(UBound(m_t2Records) + 1)
                    Case Else
                        Throw New Exception
                End Select
            Catch
                Select Case type
                    Case 1
                        ReDim m_t1Records(0)
                    Case 2
                        ReDim m_t2Records(0)
                    Case Else
                        Throw New Exception
                End Select
            End Try

            Select Case type
                Case 1
                    If UBound(value) = UBound(m_rec1) Then
                        m_t1Records(UBound(m_t1Records)) = value
                    Else
                        Throw New Exception
                    End If
                Case 2
                    If UBound(value) = UBound(m_rec2) Then
                        m_t2Records(UBound(m_t2Records)) = value
                    Else
                        Throw New Exception
                    End If
                Case Else
                    Throw New Exception
            End Select
        End Set

    End Property

    Public Function Validate() As Boolean

        If Me.m_Table = "" Then Return False
        If Me.m_Procedure = "" Then Return False
        Return True

    End Function

    Public ReadOnly Property Data() As String(,)

        Get

            ' ************************************
            ' Declares the contents of the header
            ' Do not edit

            Dim xu = UBound(m_typ) + 1
            Dim yu = 4
            yu = yu + UBound(Me.m_t1Records)
            Try
                yu = yu + (UBound(Me.m_t2Records) + 1)
            Catch
                yu = yu + 0
            End Try

            Dim ar(xu, yu) As String

            ar(0, 0) = Me.m_Table
            ar(1, 0) = Me.m_Procedure

            For x As Integer = 0 To UBound(m_rec1)
                ar(x + 1, 1) = m_rec1(x)
            Next

            Try
                For x As Integer = 0 To UBound(m_rec2)
                    ar(x + 1 + (UBound(m_rec1) + 1), 2) = m_rec2(x)
                Next
            Catch
            End Try

            For x As Integer = 0 To UBound(m_typ)
                ar(x + 1, 3) = m_typ(x)
            Next
            ar(0, 1) = "1"
            ar(0, 2) = "2"
            ' *********************************************

            ' *******************************************************************
            ' *** Build the load data to be sent

            ' Type 1 records
            Dim yord As Integer = 4

            For y As Integer = 0 To UBound(Me.m_t1Records)
                ar(0, yord) = "1"
                For x As Integer = 0 To UBound(m_t1Records(y))
                    Dim temp() = m_t1Records(y)
                    ar(x + 1, yord) = temp(x)
                Next
                yord = yord + 1
            Next

            Try
                ' Type 2 records
                For y As Integer = 0 To UBound(Me.m_t2Records)
                    ar(0, yord) = "2"
                    For x As Integer = 0 To UBound(m_t2Records(y))
                        Dim temp() = m_t2Records(y)
                        ar(2 + x + UBound(Me.m_rec1), yord) = temp(x)
                    Next
                    yord = yord + 1
                Next
            Catch
            End Try

            Return ar

        End Get

    End Property

    Public Sub Clear()

        m_t1Records = Nothing
        m_t2Records = Nothing

        m_DebugFlag = False
        m_Table = ""
        m_Procedure = ""
        m_rec1 = Nothing
        m_rec2 = Nothing
        m_typ = Nothing

        m_t1Record = Nothing
        m_t2Record = Nothing

    End Sub

    Private Function str2ar(ByVal str As String) As String()
        Dim tmp As String() = Split(str, ",", , CompareMethod.Text)
        For i As Integer = 0 To UBound(tmp)
            tmp(i) = Trim(tmp(i))
        Next
        Return tmp
    End Function

End Structure
