Imports System.Xml
Imports System.Windows.Forms

Public Class ctrlMenu
    Inherits Windows.Forms.MainMenu

    Public Sub New(ByRef node As XmlNode, ByRef handler As System.EventHandler)
        With Me
            Dim File As New MenuItem
            Dim About As New MenuItem
            File.Text = "File"
            About.Text = "About"

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

            .MenuItems.Add(File)
            .MenuItems.Add(About)
        End With
    End Sub

End Class