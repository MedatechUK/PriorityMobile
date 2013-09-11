Imports System.Threading
Imports System.Data.SqlClient
Imports System.Xml

Partial Class _default
    Inherits iSettings

    Protected Overloads Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then

            With Me

                .txterror.Text = ApplicationSetting("sysinfo", "System not started.")

                Try
                    Using connection As New GenericConnection(System.Configuration.ConfigurationManager.AppSettings.Get("PROVIDER"))
                        connection.ConnectionString = System.Configuration.ConfigurationManager.AppSettings.Get("DSN")
                        connection.Open()
                        Dim cmd As GenericCommand
                        cmd = connection.CreateCommand()
                        Select Case System.Configuration.ConfigurationManager.AppSettings.Get("PROVIDER")
                            Case 1 ' MSSQL
                                cmd.CommandText = "SELECT name FROM sys.sysdatabases " & _
                                                  "where status2 =1627389952 and name not in ('system','pritempdb') " & _
                                                  "and NOT LEFT(name,len('ReportServer$')) = 'ReportServer$'"
                            Case 2 ' Oracle
                                cmd.CommandText = "select dname from environment WHERE dname <> ' '"
                        End Select
                        Dim dataReader As GenericDataReader = _
                                     cmd.ExecuteReader()
                        While dataReader.Read
                            .Environments.Items.Add(dataReader.Item(0))
                        End While
                    End Using
                Catch ex As Exception

                End Try

                With .Environments
                    If Not IsNothing(.Items.FindByText(Environment)) Then _
                        .Items.FindByText(Environment).Selected = True
                End With

                With .LstTimeout()
                    If Not IsNothing(.Items.FINDBYVALUE(LoadingTimeout)) Then _
                        .Items.FINDBYVALUE(LoadingTimeout).Selected = True
                End With

                With .Verbosity
                    If Not IsNothing(.Items.FindByValue(LogVerbosity)) Then _
                        .Items.FindByValue(LogVerbosity).Selected = True
                End With
            End With
        End If
    End Sub

    Protected Sub Save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Btn.Click

        Try
            With System.Configuration.ConfigurationManager.AppSettings
                If Not (CStr(Me.Environments.SelectedValue) = .Get("Environment")) Then
                    ApplicationSetting("Environment") = CStr(Me.Environments.SelectedValue)
                    .Set("Environment", CStr(Me.Environments.SelectedValue))
                End If
                If Not (CInt(Me.Verbosity.SelectedValue) = CInt(.Get("LogVerbosity"))) Then
                    ApplicationSetting("LogVerbosity") = CStr(Me.Verbosity.SelectedValue)
                    .Set("LogVerbosity", CStr(Me.Verbosity.SelectedValue))
                End If
                If Not (CInt(Me.LstTimeout.SelectedValue) = CInt(.Get("LoadingTimeout"))) Then
                    ApplicationSetting("LoadingTimeout") = CStr(Me.LstTimeout.SelectedValue)
                    .Set("LoadingTimeout", CStr(Me.LstTimeout.SelectedValue))
                End If
            End With
        Catch ex As Exception
            Throw New Exception(String.Format("Failed opening {0}. {1}", webConfigPath, ex.Message))
        End Try

    End Sub


End Class
