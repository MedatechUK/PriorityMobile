Imports System.Drawing.Design
Imports System.ComponentModel
Imports hookey

Public Class LogicStrEditor
    Inherits UITypeEditor

    Public Overrides Function EditValue(ByVal context As ITypeDescriptorContext, ByVal provider As IServiceProvider, ByVal value As Object) As Object

        Dim act As prop_Action = context.Instance
        Dim lEd As New cargo3.LogicEditor(myStates(CurrentState).Actions(act.Name).Conditions)
        With lEd
            lEd.txt_Logic.Text = value
            .ShowDialog()
            Return .ResultLogic
        End With

    End Function

    Public Overrides Function GetEditStyle(ByVal context As ITypeDescriptorContext) As UITypeEditorEditStyle
        Return UITypeEditorEditStyle.Modal
    End Function

End Class