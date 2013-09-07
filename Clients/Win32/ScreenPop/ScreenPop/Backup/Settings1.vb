
Namespace My
    
    'This class allows you to handle specific events on the settings class:
    ' The SettingChanging event is raised before a setting's value is changed.
    ' The PropertyChanged event is raised after a setting's value is changed.
    ' The SettingsLoaded event is raised after the setting values are loaded.
    ' The SettingsSaving event is raised before the setting values are saved.
    Partial Friend NotInheritable Class MySettings
        Private _changed As Boolean = False
        Public Property Changed() As Boolean
            Get
                Return _changed
            End Get
            Set(ByVal value As Boolean)
                _changed = value
            End Set
        End Property
        Protected Overrides Sub OnPropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs)
            MyBase.OnPropertyChanged(sender, e)
            changed = True
        End Sub
    End Class
End Namespace
