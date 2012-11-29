Imports System.Reflection
Imports system.io
Imports System.Threading
Imports System.ComponentModel
Public Class oTrigger
    Private _Parent As oDataSet

#Region "Initialisation / finalisation"

    Private _trigger As TriggerBase
    Private _TagetType As Type
    Private ReadOnly Property TagetType() As Type
        Get
            Return _TagetType
        End Get
    End Property

    Public Overrides Function ToString() As String
        Return _trigger.ToString
    End Function

    Public Sub New(ByRef Parent As oDataSet, ByVal dllName As String, ByVal className As String)

        Dim TargetAssembly As Assembly = Assembly.LoadFrom(dllName)
        _TagetType = TargetAssembly.GetType(className, True)
        _trigger = Activator.CreateInstance(TagetType)
        _trigger.SetParent(Parent)       

    End Sub

    Public Sub Trigger(ByVal TT As Integer, ByVal NewObject As Object, ByVal OldObject As Object, ByRef Cancel As Boolean)

        Select Case TT
            Case tTrigger.PREINSERT
                _trigger.PREINSERT(NewObject, Cancel)
            Case tTrigger.POSTINSERT
                _trigger.POSTINSERT(NewObject)
            Case tTrigger.PREUPDATE
                _trigger.PREUPDATE(NewObject, OldObject, Cancel)
            Case tTrigger.POSTUPDATE
                _trigger.POSTUPDATE(NewObject, OldObject)
            Case tTrigger.PREDELETE
                _trigger.PREDELETE(OldObject, Cancel)
            Case tTrigger.POSTDELETE
                _trigger.POSTDELETE(OldObject)
        End Select

    End Sub

#End Region

End Class
