Imports Microsoft.SqlServer.Server
Imports System.Data.SqlClient
Imports System.Net.NetworkInformation

Public Class licence

    <SqlFunction(DataAccess:=DataAccessKind.Read)> _
        Public Shared Function Activated(ByVal EntityID As Integer, ByVal EDES As String) As Integer

        If Not EDES.ToUpper = "ZEMG" Then
            Return CType(1, Integer)
            Exit Function
        End If

        Using conn As New SqlConnection("context connection=true")
            conn.Open()
            Try

                If Validate( _
                    ActivationCode(EntityID, conn), _
                    CheckVal( _
                        PriorityLicence(conn), _
                        ModuleName(EntityID, conn) _
                    ) _
                ) Then
                    Return CType(1, Integer)
                Else
                    Return CType(0, Integer)
                End If

            Catch
                Return CType(1, Integer)
            End Try
        End Using
    End Function

    Private Shared Function ModuleName(ByVal EntityID As Integer, ByVal cn As SqlConnection) As String
        Dim cmd As New SqlCommand(String.Format("SELECT MODULENAME FROM system.dbo.MODULES " & _
                                  "WHERE MODULE = " & _
                                  "(select MODULE from system.dbo.EXECMODULE where T$EXEC = {0})", EntityID), _
                                  cn)
        Return cmd.ExecuteScalar()
    End Function

    Private Shared Function ActivationCode(ByVal EntityID As Integer, ByVal cn As SqlConnection) As String
        Dim cmd As New SqlCommand(String.Format("SELECT MODULEKEY FROM system.dbo.MODULES " & _
                                  "WHERE MODULE = " & _
                                  "(select MODULE from system.dbo.EXECMODULE where T$EXEC = {0})", EntityID), _
                                  cn)
        Return cmd.ExecuteScalar()
    End Function

    Private Shared Function PriorityLicence(ByVal cn As SqlConnection) As String
        Dim cmd As New SqlCommand("SELECT top 1 password FROM system.dbo.t$license", _
                                  cn)
        Return cmd.ExecuteScalar()
    End Function

    Private Shared Function Validate(ByVal key As String, ByVal CheckVal As Integer) As Boolean
        Dim Numbers = "1234567890ABCDEFGHIJKLMNOPQRTSUVWXYZ"
        Dim r As Integer = 0
        For i As Integer = 0 To key.Length - 1
            r += InStr(Numbers, key.Substring(i, 1)) - 1
        Next
        Return CBool(CheckVal = (r - 1))
    End Function

    Private Shared Function CheckVal(ByVal PriorityLicence As String, ByVal ModuleName As String) As Integer

        Dim c As Integer = 0
        For i As Integer = 0 To PriorityLicence.Length - 1
            If IsNumeric(PriorityLicence.Substring(i, 1)) Then
                c += CInt(PriorityLicence.Substring(i, 1))
            End If
        Next
        For i As Integer = 0 To ModuleName.Length - 1
            c += CInt(Right(Asc(ModuleName.Substring(i, 1)).ToString, 1))
        Next

        Return c

    End Function

    Private Class ch
        Private Numbers As String = "1234567890ABCDEFGHIJKLMNOPQRTSUVWXYZ"
        Private _loc As Integer = 0

        Public ReadOnly Property Value() As String
            Get
                Return Numbers.Substring(_loc, 1)
            End Get
        End Property

        Public Function Increment() As Boolean
            If _loc < Numbers.Length - 1 Then
                _loc += 1
                Return True
            Else
                Return False
            End If
        End Function

    End Class

End Class
