Imports System.Drawing.Design
Imports System.ComponentModel
Imports hookey

Public Class KeysStrEditor
    Inherits UITypeEditor

    Public Overrides Function EditValue(ByVal context As ITypeDescriptorContext, ByVal provider As IServiceProvider, ByVal value As Object) As Object

        'Dim act As prop_Action = context.Instance
        Dim KeyEdit As New cargo3.KeyEditor
        With KeyEdit
            .txt_Encoded.Text = value
            .ShowDialog()
            Return .txt_Encoded.Text
        End With

    End Function

    Public Overrides Function GetEditStyle(ByVal context As ITypeDescriptorContext) As UITypeEditorEditStyle
        Return UITypeEditorEditStyle.Modal
    End Function

End Class