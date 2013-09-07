Imports cargo3
Imports System.ComponentModel
Imports System.Drawing.Design
'''
<DefaultPropertyAttribute("Name")> _
Public Class prop_Action

    Private _action As cargo3.Action
    Private _CurrentState As String

    Public Event Refresh()

    Public Sub New()

    End Sub

    Public Sub New(ByRef Action As cargo3.Action, ByVal CurrentState As String)

        _action = Action
        _Name = Action.Name
        _Logic = Action.Logic
        _CurrentState = CurrentState

        If Action.Results.Keys.Contains(True) Then
            _ResultTrue = New StrResult(myStates, CurrentState, Action.Results(True))
        Else
            _ResultTrue = New StrResult(myStates, CurrentState)
        End If
        AddHandler _ResultTrue.SaveResult, AddressOf hSaveTrueResult

        If Action.Results.Keys.Contains(False) Then
            _ResultFalse = New StrResult(myStates, CurrentState, Action.Results(False))
        Else
            _ResultFalse = New StrResult(myStates, CurrentState)
        End If
        AddHandler _ResultFalse.SaveResult, AddressOf hSaveFalseResult
    End Sub

    Private Sub hSaveTrueResult(ByVal sender As StrResult)
        SaveResult(True, sender)
    End Sub

    Private Sub hSaveFalseResult(ByVal sender As StrResult)
        SaveResult(False, sender)
    End Sub

    Private _Name As String = ""
    '''
    <CategoryAttribute("General"), _
       Browsable(True), _
       [ReadOnly](False), _
       BindableAttribute(False), _
       DefaultValueAttribute(""), _
       DesignOnly(False), _
       DescriptionAttribute("The name of the selected action.")> _
       Public Property Name() As String
        Get
            Return _Name
        End Get
        Set(ByVal Value As String)
            _Name = Value
            RaiseEvent Refresh()
        End Set
    End Property

    Private _Logic As String = ""
    '''
    <CategoryAttribute("Action Logic"), _
       Browsable(True), _
       [ReadOnly](False), _
       BindableAttribute(False), _
       DefaultValueAttribute(""), _
       DesignOnly(False), _
       Editor(GetType(LogicStrEditor), GetType(System.Drawing.Design.UITypeEditor)), _
       DescriptionAttribute("Condition Logic.")> _
       Public Property Logic() As String
        Get
            Return _Logic
        End Get
        Set(ByVal Value As String)
            _Logic = Value
            cargo3.sharedFunctions.myStates(_CurrentState).Actions(Name).Logic = Value
            RaiseEvent Refresh()
        End Set
    End Property

    Private _ResultTrue As cargo3.StrResult
    '''
    <CategoryAttribute("Action Result"), _
       Browsable(True), _
       [ReadOnly](False), _
       BindableAttribute(False), _
       DefaultValueAttribute(""), _
       DesignOnly(False), _
       DescriptionAttribute("Action to take if conditions are met.")> _
       Public Property Result_True() As cargo3.StrResult
        Get
            Return _ResultTrue
        End Get
        Set(ByVal Value As cargo3.StrResult)
            _ResultTrue = Value
            SaveResult(True, Value)
            RaiseEvent Refresh()
        End Set
    End Property

    '       Editor(GetType(ColourEditor), GetType(System.Drawing.Design.UITypeEditor)), _
    Private _ResultFalse As cargo3.StrResult
    '''
    <CategoryAttribute("Action Result"), _
       Browsable(True), _
       [ReadOnly](False), _
       BindableAttribute(False), _
       DefaultValueAttribute(""), _
       DesignOnly(False), _
       DescriptionAttribute("Action to take if conditions are NOT met.")> _
       Public Property Result_False() As cargo3.StrResult
        Get
            Return _ResultFalse
        End Get
        Set(ByVal Value As cargo3.StrResult)
            _ResultFalse = Value
            SaveResult(False, Value)
            RaiseEvent Refresh()
        End Set
    End Property

    Private Sub SaveResult(ByVal Result As Boolean, ByVal Value As cargo3.StrResult)
        If Not (cargo3.sharedFunctions.myStates(_CurrentState).Actions(Name).Results.Keys.Contains(Result)) Then
            cargo3.sharedFunctions.myStates(_CurrentState).Actions(Name).Results.Add(Result, New cargo3.Result())
        End If
        With cargo3.sharedFunctions.myStates(_CurrentState).Actions(Name).Results(Result)
            If Value.NextAction.Length > 0 Then
                .NextAction = myStates(_CurrentState).Actions(Value.NextAction)
            Else
                .NextAction = Nothing
            End If
            If Value.NextState.Length > 0 Then
                .NextState = myStates(Value.NextState)
            Else
                .NextState = Nothing
            End If
            If Value.Script.Length > 0 Then
                .Script = myStates.myScripts(Value.Script)
            Else
                .Script = Nothing
            End If
        End With
    End Sub

End Class
