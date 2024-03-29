﻿Imports PriorityMobile
Imports System.Drawing
Imports System.Xml

Public Class ctrl_Sign
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
                    .Attributes.Append(xmlForms.changedAttribute)
                    .InnerText = Signature.toSerial
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
