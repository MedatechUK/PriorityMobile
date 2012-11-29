Imports Microsoft.VisualBasic

Public Class PartFamily

    Dim sd As New Priority.SerialData
    Dim ws As New priwebsvc.Service
    Dim _caller As DynamicMaster

    Public Sub New(ByRef DM As DynamicMaster)
        _caller = DM
    End Sub

    Public Sub DrawMenu(ByRef iPage As Page, ByRef Menu As System.Web.UI.WebControls.Menu)

        Menu.Items.Clear()

        Dim sql As String = "select FAMILY, FAMILYDES from FAMILY " & _
                            "where FAMILY in (" & _
                            "select FAMILY from PART " & _
                            "where SHOWINWEB = 'Y')"

        Dim Family(,) As String = sd.DeSerialiseData(ws.GetData(sql))

        If Not (IsNothing(Family)) Then
            For n As Integer = 0 To UBound(Family, 2)
                Dim mnuitm As New System.Web.UI.WebControls.MenuItem()
                With mnuitm
                    .Value = Family(1, n)
                    .Text = "&nbsp;" & Family(1, n)
                    .NavigateUrl = _caller.NamePair.ParseURL("/family.aspx", "FAMILY", Family(0, n))
                End With

                'AddSubItems(WebID, mnuitm, Cats(0, n))
                Menu.Items.Add(mnuitm)
            Next
        End If

    End Sub

End Class
