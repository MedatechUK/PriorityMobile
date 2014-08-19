Imports PriorityMobile
Imports System.Drawing
Imports System.Xml

Public Class ctrl_sign
    Inherits iView

#Region "Overrides base methods"

    Public Overrides Sub Bind()
        With Me
            With .txt_PrintName
                .DataBindings.Add("Text", thisForm.TableData, "print")
                .Enabled = IsOnsite()
            End With
            With .Signature
                .DrawSignature(thisForm.CurrentRow("image"))
                .Enabled = IsOnsite()
            End With
        End With
    End Sub

    Public Overrides Sub CurrentChanged()
        With Me
            .txt_PrintName.Enabled = IsOnsite()
            With .Signature
                .Clear()
                .DrawSignature(thisForm.CurrentRow("image"))
                .Enabled = IsOnsite()
            End With
        End With
    End Sub

    Public Overrides Sub FormClosing()
        With thisForm
            With .FormData
                Dim n As XmlNode = thisForm.FormData.SelectSingleNode(thisForm.thisxPath & "/image")
                With n
                    xmlForms.SetNodeChanged(n)
                    '.Attributes.Append(xmlForms.changedAttribute)
                    .InnerText = Signature.toSerial
                End With
                Dim t As XmlNode = thisForm.FormData.SelectSingleNode(thisForm.thisxPath & "/print")
                With t
                    xmlForms.SetNodeChanged(t)
                    '.Attributes.Append(xmlForms.changedAttribute)
                    .InnerText = txt_PrintName.Text
                End With
            End With
            .Save()
        End With
    End Sub

#End Region

#Region "Local Methods"

    Private Function IsOnsite() As Boolean
        Return xmlForms.StatusRule(thisForm.Parent.CurrentRow("callstatus"), eStatusRule.active)
    End Function

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
