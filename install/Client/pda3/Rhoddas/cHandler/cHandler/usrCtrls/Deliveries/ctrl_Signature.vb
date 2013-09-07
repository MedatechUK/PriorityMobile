Imports System.Xml
Imports PriorityMobile

Public Class ctrl_Signature
    Inherits iView

#Region "Overrides base methods"

    Public Overrides Sub Bind()
        With Me
            With .txt_PrintName
                .DataBindings.Add("Text", thisForm.TableData, "print")                
            End With
            With .Signature
                .DrawSignature(thisForm.CurrentRow("image"))                
            End With
        End With
    End Sub

    Public Overrides Sub CurrentChanged()
        With Me            
            With .Signature
                .Clear()
                .DrawSignature(thisForm.CurrentRow("image"))                
            End With
        End With
    End Sub

    Public Overrides Sub FormClosing()
        With thisForm
            With .FormData
                Dim n As XmlNode = .SelectSingleNode(thisForm.thisxPath & "/image")
                With n                    
                    .InnerText = Signature.toSerial
                End With
            End With
            .Save()
        End With
    End Sub

#End Region

#Region "Direct Activations"

    Public Overrides Sub DirectActivations(ByRef ToolBar As daToolbar)
        ToolBar.Add(AddressOf hClearSignature, "DELETE.BMP")
    End Sub

    Private Sub hClearSignature()
        Signature.Clear()
    End Sub

#End Region

End Class
