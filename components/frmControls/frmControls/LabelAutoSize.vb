
Module CeAutoSizeLabel




    ' Externals ----------------------


    Private Structure RECT
        Public Left As Integer
        Public Top As Integer
        Public Right As Integer
        Public Bottom As Integer
    End Structure


    Private Const DT_CALCRECT As Integer = &H400


    Private Const DT_CENTER As Integer = &H1


    Private Const DT_LEFT As Integer = &H0


    Private Const DT_RIGHT As Integer = &H2


    Private Const DT_TOP As Integer = &H0


    Private Const DT_WORDBREAK As Integer = &H10


    Private Declare Function DeleteObject Lib "coredll.dll" (ByVal hObject As IntPtr) As Integer


    Private Declare Function DrawText Lib "coredll.dll" (ByVal hdc As IntPtr, ByVal lpStr As String, ByVal nCount As Integer, ByRef lpRect As RECT, ByVal wFormat As Integer) As Integer


    Private Declare Function SelectObject Lib "coredll.dll" (ByVal hdc As IntPtr, ByVal hObject As IntPtr) As IntPtr


    Public Sub AutoSizeLabelHeight(ByVal ctlLabel As Label)




        ' Auto size the height of a Label control based on the contents of the label.


        ' Note: This routine is best called from the Form's Resize event so that changes to screen size and orientation


        ' will force the Label height to be adjusted.


        Dim uRECT As RECT


        Try




            ' Create a Graphics object. We need a Graphics object so we can get a handle to a Device Context to be used


            ' later in the DrawText API. However the Label control in CF2.0 does not support the CreateGraphics method,


            ' so create the Graphics object from the form on which the Label is located. 


            ' Note: This can cause an exception when this routine is called from a Form's Resize event. Probably because 


            ' the form is not fully initialised and the Resize event may be called one or more times during form


            ' initialisation. Therefore, the entire routine is wrapped in a Try/Catch block.


            Dim objGraphics As Graphics = ctlLabel.TopLevelControl.CreateGraphics


            ' -------------------------------------------------------------


            ' Note: An alternative to the above method of creating a Graphics object is to create a Bitmap object and 


            ' obtain the Graphics object from it as follows.


            'Dim objBitmap As New Bitmap(1, 1)


            'Dim objGraphics As Graphics = Graphics.FromImage(objBitmap)


            ' And remembering to dispose of the Bitmap object in a cleanup at the end of the routine as follows.


            'objBitmap.Dispose()


            ' This method would remove the need for the Try/Catch block, but means that Bitmap objects are repeatedly 


            ' being created and destroyed.


            ' -------------------------------------------------------------


            ' Get the handle to the Device Context of the Graphics object


            Dim hDc As IntPtr = objGraphics.GetHdc


            With ctlLabel




                ' Get the handle to the Font of the Label


                Dim hFont As IntPtr = .Font.ToHfont


                ' Apply the Font to the Graphics object


                Dim hFontOld As IntPtr = SelectObject(hDc, hFont)


                ' Set the initial size of the Rect

                uRECT.Right = .Width

                uRECT.Bottom = .Height


                ' Build the base format


                Dim lFormat As Integer = DT_CALCRECT Or DT_WORDBREAK Or DT_TOP


                ' -------------------------------------------------------------


                ' Adjust the format to the Label's text alignment.


                ' Note: This probably isn't necessary as the horizontal alignment of text shouldn't affect the text


                ' height calculation. But just in case...


                Select Case .TextAlign




                    Case ContentAlignment.TopLeft Or DT_LEFT


                        lFormat = lFormat





                    Case ContentAlignment.TopCenter Or DT_CENTER


                        lFormat = lFormat





                    Case ContentAlignment.TopRight Or DT_RIGHT


                        lFormat = lFormat





                End Select


                ' -------------------------------------------------------------


                ' Calculate the Rect of the text


                If DrawText(hDc, .Text, -1, uRECT, lFormat) <> 0 Then ' Success




                    ' Apply the new height to the label

                    .Height = uRECT.Bottom


                End If


                ' Cleanup ----------------------------


                ' Set the Font of the Graphics object back to the original

                SelectObject(hDc, hFontOld)


                ' Delete the handle to the Font of the Label

                DeleteObject(hFont)


            End With


            ' Clean up the Graphics object

            objGraphics.Dispose()


        Catch




            ' Do nothing


            'Debug.WriteLine("AutoSizeLabelHeight failed.")


        End Try


    End Sub

End Module
