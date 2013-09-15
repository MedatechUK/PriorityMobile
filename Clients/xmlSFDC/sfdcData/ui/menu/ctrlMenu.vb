Imports System.Xml
Imports System.Windows.Forms

Public Class ctrlMenu
    Inherits Windows.Forms.MainMenu

    Private win As MenuItem
    Private _ue As UserEnv
    Private _update As System.EventHandler

    Public Event ListWindows(ByRef WindowList As MenuItem)

    Public Sub New(ByRef ue As UserEnv, ByRef node As XmlNode, ByRef handler As System.EventHandler, ByVal update As System.EventHandler, ByVal hClose As System.EventHandler)

        _ue = ue
        _update = update

        With Me
            Dim File As New MenuItem
            Dim About As New MenuItem
            File.Text = "File"
            About.Text = "Menu"

            With File
                For Each Menu As XmlNode In node.SelectNodes("menu")
                    Dim mi As New MenuItem
                    mi.Text = Menu.Attributes("name").Value

                    For Each Int As XmlNode In Menu.SelectNodes("interface")
                        With mi.MenuItems
                            Dim si As New MenuItem
                            si.Text = Int.Attributes("name").Value
                            AddHandler si.Click, handler
                            .Add(si)
                        End With
                    Next
                    .MenuItems.Add(mi)
                Next
            End With

            With About
                Dim mi As New MenuItem
                mi.Text = "About"
                AddHandler mi.Click, AddressOf hAboutClick
                .MenuItems.Add(mi)

                Dim pi As New MenuItem
                pi.Text = "Printer"
                AddHandler pi.Click, AddressOf hPrnClick
                .MenuItems.Add(pi)

                win = New MenuItem
                win.Text = "Windows"
                .MenuItems.Add(win)

                Dim sp As New MenuItem
                sp.Text = "-"
                AddHandler sp.Click, hClose
                .MenuItems.Add(sp)

                Dim ci As New MenuItem
                ci.Text = "Close"
                AddHandler ci.Click, hClose
                .MenuItems.Add(ci)

                AddHandler .Popup, AddressOf hWindowClick

            End With

            .MenuItems.Add(File)
            .MenuItems.Add(About)

        End With

    End Sub

    Private Sub hWindowClick(ByVal sender As Object, ByVal e As System.EventArgs)
        RaiseEvent ListWindows(win)
    End Sub

    Private Sub hAboutClick(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim frmAbout As New About(_ue)
        frmAbout.ShowDialog()
        If frmAbout.Result = DialogResult.OK Then
            _update.Invoke(Me, New System.EventArgs)
        End If
    End Sub

    Private Sub hPrnClick(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim prnSet As New PrinterSetting
        With prnSet
            .MACAddress = _ue.MACAddress
            .ShowDialog()
            If .Result = DialogResult.OK Then
                _ue.MACAddress = .MACAddress
            End If
        End With        
    End Sub

End Class