Imports System.ComponentModel
Imports System.Globalization
Imports System.Drawing.Design
Imports System.Drawing

<TypeConverter(GetType(ResultConverter)), _
DescriptionAttribute("Expand to see the action properties.")> _
Public Class StrResult

    Public Event SaveResult(ByVal sender As StrResult)

    Public Sub New()
        'myStates = Nothing
        'CurrentState = Nothing

    End Sub

    Public Sub New(ByRef States As cargo3.States, ByVal State As String)
        myStates = States
        CurrentState = State

    End Sub

    Public Sub New(ByRef States As cargo3.States, ByVal State As String, ByVal Result As cargo3.Result)
        myStates = States
        CurrentState = State

        With Result
            If Not IsNothing(.NextState) Then _NextState = .NextState.Name
            If Not IsNothing(.NextAction) Then _NextAction = .NextAction.Name
            If Not IsNothing(.Script) Then _Script = .Script.Name
        End With
    End Sub

    Private _NextState As String = String.Empty
    <TypeConverter(GetType(StateNameConverter)), _
    DefaultValueAttribute("")> _
    Public Property NextState() As String
        Get
            Return _NextState
        End Get
        Set(ByVal value As String)
            _NextState = value
            If _NextState.Length > 0 Then
                _NextAction = ""
            End If
            RaiseEvent SaveResult(Me)
        End Set
    End Property

    Private _NextAction As String = String.Empty
    <TypeConverter(GetType(ActionNameConverter)), _
    DefaultValueAttribute("")> _
    Public Property NextAction() As String
        Get
            Return _NextAction
        End Get
        Set(ByVal value As String)
            _NextAction = value
            If _NextAction.Length > 0 Then
                _NextState = ""
            End If
            RaiseEvent SaveResult(Me)
        End Set
    End Property

    Private _Script As String = String.Empty
    <Editor(GetType(ScriptEditor), GetType(System.Drawing.Design.UITypeEditor)), _
    DefaultValueAttribute("")> _
    Public Property Script() As String
        Get
            Return _Script
        End Get
        Set(ByVal value As String)
            _Script = value
            RaiseEvent SaveResult(Me)
        End Set
    End Property

End Class

Public Class StateNameConverter
    Inherits StringConverter

    Public Overloads Overrides Function GetStandardValuesSupported( _
                    ByVal context As ITypeDescriptorContext) As Boolean
        Return True
    End Function

    Public Overloads Overrides Function GetStandardValues( _
        ByVal context As ITypeDescriptorContext) _
        As StandardValuesCollection

        Dim ret(0) As String
        ret(0) = ""
        If Not IsNothing(myStates) Then
            For Each st As cargo3.State In myStates.Values
                ReDim Preserve ret(UBound(ret) + 1)
                ret(UBound(ret)) = st.Name
            Next
        End If

        Return New StandardValuesCollection(ret)
    End Function

End Class

Public Class ActionNameConverter
    Inherits StringConverter

    Public Overloads Overrides Function GetStandardValuesSupported( _
                    ByVal context As ITypeDescriptorContext) As Boolean
        Return True
    End Function

    Public Overloads Overrides Function GetStandardValues( _
        ByVal context As ITypeDescriptorContext) _
        As StandardValuesCollection

        Dim ret(0) As String
        ret(0) = ""
        If Not IsNothing(myStates) Then
            For Each act As cargo3.Action In myStates(CurrentState).Actions.Values
                ReDim Preserve ret(UBound(ret) + 1)
                ret(UBound(ret)) = act.Name
            Next
        End If
        Return New StandardValuesCollection(ret)
    End Function

End Class

Public Class ResultConverter
    Inherits ExpandableObjectConverter

    Public Overrides Function CanConvertTo( _
                              ByVal context As ITypeDescriptorContext, _
                              ByVal destinationType As Type) As Boolean
        If (destinationType Is GetType(StrResult)) Then
            Return True
        End If
        Return MyBase.CanConvertFrom(context, destinationType)
    End Function

    Public Overrides Function ConvertTo( _
                              ByVal context As ITypeDescriptorContext, _
                              ByVal culture As CultureInfo, _
                              ByVal value As Object, _
                              ByVal destinationType As System.Type) _
                     As Object
        If (destinationType Is GetType(System.String) _
            AndAlso TypeOf value Is StrResult) Then

            Dim _this As StrResult = CType(value, StrResult)
            Dim ScriptName As String = String.Empty
            If _this.Script.Length > 0 Then
                ScriptName = String.Format("{0}+", _this.Script)
            End If
            Dim ActionName As String = String.Empty
            If _this.NextAction.Length > 0 Then
                ActionName = String.Format("Action:{0}", _this.NextAction)
            End If
            If _this.NextState.Length > 0 Then
                ActionName = String.Format("State:{0}", _this.NextState)
            End If
            Return String.Format("{0}{1}", ScriptName, ActionName)

        End If
        Return MyBase.ConvertTo(context, culture, value, destinationType)
    End Function

    Public Overrides Function ConvertFrom( _
                              ByVal context As ITypeDescriptorContext, _
                              ByVal culture As CultureInfo, _
                              ByVal value As Object) As Object

        If (TypeOf value Is String) Then
            Try
                Dim s As String = CStr(value)
                Dim _this As StrResult = New StrResult()

                With _this
                    If s.Contains("+") Then
                        .Script = s.Split("+")(0)
                    End If
                    If s.Contains("State:") Then
                        .NextState = s.Split(":")(1)
                        .NextAction = ""
                    ElseIf s.Contains("Action:") Then
                        .NextAction = s.Split(":")(1)
                        .NextState = ""
                    End If
                End With

                Return _this
            Catch
                Throw New ArgumentException( _
                    "Can not convert '" & CStr(value) & _
                                      "' to type strResult")

            End Try
        End If
        Return MyBase.ConvertFrom(context, culture, value)
    End Function

End Class

Public Class ScriptEditor
    Inherits UITypeEditor

    Public Overrides Function EditValue(ByVal context As ITypeDescriptorContext, ByVal provider As IServiceProvider, ByVal value As Object) As Object
        With ScriptForm

            If value.ToString.Length > 0 Then
                .ScriptName = value.ToString
            Else
                .ScriptName = Nothing
            End If
            .ShowDialog()
            Return .ResultScript
        End With
    End Function

    Public Overrides Function GetEditStyle(ByVal context As ITypeDescriptorContext) As UITypeEditorEditStyle
        Return UITypeEditorEditStyle.Modal
    End Function

End Class