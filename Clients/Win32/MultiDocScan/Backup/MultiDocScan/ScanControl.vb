
Imports System.ComponentModel
Imports System.Text
Imports DTI.ImageMan.Twain
Imports DTI.ImageMan.Barcode
Imports DTI.ImageMan.Barcode.DataMatrix.detector
Imports PdfSharp
Imports PdfSharp.Drawing
Imports PdfSharp.Pdf
Imports System.IO
Imports System.Collections
Imports System
Imports System.Drawing
Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration
Imports System.Configuration.ConfigurationSettings
'/f "3" "IINV" "../../Scandocs/demo/IINV" "T8" 
'/f  "1 PORD ../../scandocs/demo/PORD PO12000017"
Public Class ScanControl
    Public Shared imgs As New ArrayList

    ' Single Scan Console
    ' Version 1.0
    ' Andy Mackintosh
    ' 14/12/2012
    ' This application is intended to be used from within Priority. It accepts one parameter /f {filename with full path but no filetype}
    ' Once ran it will call the scanner and grab whatever document is loaded, then it will convert it to pdf and save it as the filename provided
    ' I have set the needed scanner contols up as application level variables in the ScanSettings class, these are read from an XML file and can
    ' therefore be changed by simply editing the file

    Public Class ScanSettings
        Private ptype As DTI.ImageMan.Twain.PixelTypes
        Private mpAG As Integer
        Private ui As DTI.ImageMan.Twain.UserInterfaces
        Private resol As Integer
        Public Property PagType() As DTI.ImageMan.Twain.PixelTypes
            Get
                Return ptype
            End Get
            Set(ByVal value As DTI.ImageMan.Twain.PixelTypes)
                ptype = value
            End Set
        End Property
        Public Property mpg() As Integer
            Get
                Return mpAG
            End Get
            Set(ByVal value As Integer)
                mpAG = value
            End Set
        End Property
        Public Property uid() As DTI.ImageMan.Twain.UserInterfaces
            Get
                Return ui
            End Get
            Set(ByVal value As DTI.ImageMan.Twain.UserInterfaces)
                ui = value
            End Set
        End Property
        Public Property resolu() As Integer
            Get
                Return resol
            End Get
            Set(ByVal value As Integer)
                resol = value
            End Set
        End Property
        Public Sub New(ByVal pt As DTI.ImageMan.Twain.PixelTypes, ByVal pgs As Integer, ByVal uids As DTI.ImageMan.Twain.UserInterfaces, ByVal res As Integer)
            PagType = pt
            mpg = pgs
            uid = uids
            resol = res
        End Sub

        Public Shared Function writesettings(ByVal PixType As Integer, ByVal MaxPage As Integer, ByVal UsrIntFc As Integer, ByVal Res As Integer) As Boolean
            Try
                My.Settings.PixType = PixType
                My.Settings.MaxPage = MaxPage
                My.Settings.UserIntface = UsrIntFc
                My.Settings.Resolut = Res
                My.Settings.Save()
                Return True
            Catch ex As Exception
                Return False
            End Try
        End Function
    End Class
    Public Class ScanDocument
        Private SCAN_DOC_DIR As String
        Private SCAN_DATE As Integer
        Private SCAN_PROCESSED As Char
        Private SCAN_BATCH_NO As Integer
        Public Property doc_dir() As String
            Get
                Return SCAN_DOC_DIR
            End Get
            Set(ByVal value As String)
                SCAN_DOC_DIR = value
            End Set
        End Property
        Public Property doc_date() As Integer
            Get
                Return SCAN_DATE
            End Get
            Set(ByVal value As Integer)
                SCAN_DATE = value
            End Set
        End Property
        Public Property processed() As Char
            Get
                Return SCAN_PROCESSED
            End Get
            Set(ByVal value As Char)
                SCAN_PROCESSED = value
            End Set
        End Property
        Public Property batch_no() As Integer
            Get
                Return SCAN_BATCH_NO
            End Get
            Set(ByVal value As Integer)
                SCAN_BATCH_NO = value
            End Set
        End Property

        Public Sub New(ByVal dir As String, ByVal sdate As Integer, ByVal proc As Char, ByVal bno As Integer)

            doc_dir = dir
            doc_date = sdate
            processed = proc
            batch_no = bno

        End Sub
    End Class
    Public Shared Function scannall(ByVal f As ScanDocument)
        Dim D As ScanControl.ScanSettings
        D = New ScanControl.ScanSettings(My.Settings("pixtype"), My.Settings("maxpage"), My.Settings("userintface"), My.Settings("resolut"))

        'The arraylist will hold all the pages scanned in as bitmap images. I have used arraylist as I dont have to specify any bounds as I dont know them
        Dim tw As New DTI.ImageMan.Twain.TwainControl

        Try
            Using tw
                With tw
                    .PixelType = D.PagType
                    .MaxPages = D.mpg
                    .UserInterface = D.uid
                    .Resolution = D.resolu
                End With
                'Instantiates the twain control
                Dim img As Image
                'Creates a holder image to capture each page
                'imgs = New ArrayList
                'instantiate the arraylist
                '
                'Dim capabilityValue As Object
                'Dim dataType As DTI.ImageMan.Twain.DataType

                'Dim retVal As Boolean = tw.GetCapability(DTI.ImageMan.Twain.Capabilities.FeederLoaded, capabilityValue, dataType)

                'If retVal AndAlso CInt(capabilityValue) <> 0 Then

                '    If retVal AndAlso CInt(capabilityValue) <> 0 Then
                '        'MessageBox.Show("Feeder is loaded with paper")
                '    End If
                'Else

                'End If



                Try
                    img = tw.ScanPage()
                Catch ex As Exception
                    write_error(ex.ToString, 1)
                    Return False
                End Try

                'grab the first page
                While Not (img Is Nothing)
                    Try
                        imgs.Add(img)
                        img = tw.ScanPage()

                    Catch ex As Exception
                        Console.WriteLine(ex.ToString)
                        Return False
                    End Try
                End While
            End Using
            'The above loop will keep scanning until there are no pages left to scan
            ' now we need to break the image list down into pdf's by barcode
            process(f)
            Return True
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try




    End Function
    Public Shared Sub write_error(ByVal errmsg As String, ByVal usr As Integer)
        Dim con As New SqlConnection
        Dim cmd As New SqlCommand
        With con
            .ConnectionString = My.Settings.PriorityDB.ToString 'GetConnectionString("PriorityDB")

        End With
        Dim sdate As DateTime
        sdate = FormatDateTime("1/1/1988", DateFormat.ShortDate)
        con.Open()
        cmd.Connection = con
        cmd.CommandText = "insert into ZEMG_ERRMSGLOG (LOGGEDBYPROCNAME,LOGGEDDATE,[MESSAGE],T$USER) values ( 'Scanner',@ddate,@message,@user)"
        cmd.Parameters.AddWithValue("ddate", DateDiff(DateInterval.Minute, sdate, Now))
        cmd.Parameters.AddWithValue("message", errmsg)
        cmd.Parameters.AddWithValue("user", usr)
        cmd.ExecuteNonQuery()
    End Sub
    Public Shared Sub write_log(ByVal DOC_DIR As String, ByVal File_Name As String, ByVal S_Date As Integer, ByVal sc_ty_co As String, ByVal SDC As String, ByVal S_Batch_NO As Integer)
        Try
            Dim con As New SqlConnection
            Dim cmd As New SqlCommand
            Dim cs As String
            cs = My.Settings("PriorityDB")
            With con
                .ConnectionString = My.Settings.PriorityDB.ToString 'GetConnectionString("PriorityDB")
            End With
            con.Open()
            cmd.Connection = con
            cmd.CommandText = "insert into ZEMG_SCANNINGLOG (SCAN_DOC_DIR,SCAN_FILE_NAME,SCAN_DATE,SCAN_PROCESSED,SCAN_TYPE_CODE,SCAN_DOC_CODE,SCAN_BATCH_NO) values ( @docdir,@filename ,@ddate,'N',@TYCODE,@SDCO,@batchno)"

            cmd.Parameters.AddWithValue("ddate", S_Date)
            cmd.Parameters.AddWithValue("docdir", DOC_DIR)
            cmd.Parameters.AddWithValue("filename", File_Name)
            cmd.Parameters.AddWithValue("batchno", S_Batch_NO)
            cmd.Parameters.AddWithValue("TYCODE", sc_ty_co)
            cmd.Parameters.AddWithValue("SDCO", SDC)
            cmd.ExecuteNonQuery()
        Catch ex As Exception
            write_error(ex.ToString, 1)
            Console.WriteLine("Write Failed: " & ex.ToString)
        End Try

    End Sub
    Public Shared Function GetConnectionString(ByVal connectionS As String) As String
        'this function will check on the existence of the app settings connection string named priority db, if it cant find it it will write an error and quit. The function will also scheck the company used in the connection string, if it is different it will update the string.
        'variable to hold our connection string for returning it (Data Source=POTTER\PRI;Initial Catalog=demo;Integrated Security=True)

        Dim strReturn As New String("")

        'check to see if the user provided a company name  


        If Not String.IsNullOrEmpty(connectionS) Then

            strReturn = ConfigurationManager.ConnectionStrings(connectionS).ConnectionString

            'Next we check the existing connection strinbg to see if the company names match

        Else
            'no connection string name was provided  
            'get the default connection string  
            strReturn = ConfigurationManager.ConnectionStrings("YourConnectionName").ConnectionString
        End If
        'return the connection string to the caller 
        Return strReturn

    End Function
    Public Shared Sub ChecktConnectionString(ByVal company As String)
        'this function will check on the existence of the app settings connection string named priority db, if it cant find it it will write an error and quit. The function will also scheck the company used in the connection string, if it is different it will update the string.
        'variable to hold our connection string for returning it (Data Source=POTTER\PRI;Initial Catalog=demo;Integrated Security=True)

        Dim strReturn As New String("")

        'check to see if the user provided a company name  


        If Not String.IsNullOrEmpty(company) Then
            'a company name was provided  
            'get the connection string by the name provided  
            Try
                strReturn = ConfigurationManager.ConnectionStrings("prioritydb").ConnectionString
            Catch ex As Exception
                Console.WriteLine("Database connection string not set in the settings file")
                write_error("Database connection not found in the settings file.", 1)

            End Try


            'Next we check the existing connection strinbg to see if the company names match
            Dim connectionparts() As String = strReturn.Split(";")
            Dim initcat() As String = connectionparts(1).Split("=")
            Dim cat As String = initcat(1)
            Dim constr As String = ""
            If cat <> company Then
                Try
                    Dim config As System.Configuration.Configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None)
                    strReturn = connectionparts(0) & ";" & "Initial Catalog=" & company & ";" & connectionparts(2)
                    config.ConnectionStrings.ConnectionStrings("PriorityDB").ConnectionString = strReturn
                    config.Save()
                Catch ex As Exception

                End Try


            End If
        Else
            'no connection string name was provided  
            'get the default connection string  
            strReturn = ConfigurationManager.ConnectionStrings("YourConnectionName").ConnectionString
        End If
        'return the connection string to the caller 


    End Sub
    'Public Shared Function scannall_fake(ByVal f As ScanDocument)
    '    Dim D As ScanControl.ScanSettings
    '    D = New ScanControl.ScanSettings(My.Settings("pixtype"), My.Settings("maxpage"), My.Settings("userintface"), My.Settings("resolut"))
    '    Dim imgs As New ArrayList
    '    'The arraylist will hold all the pages scanned in as bitmap images. I have used arraylist as I dont have to specify any bounds as I dont know them
    '    'Dim tw As New DTI.ImageMan.Twain.TwainControl
    '    'Using tw


    '    '    With tw
    '    '        .PixelType = D.PagType
    '    '        .MaxPages = D.mpg
    '    '        .UserInterface = D.uid
    '    '        .Resolution = D.resolu
    '    '    End With
    '    '    'Instantiates the twain control
    '    '    Dim img As Image
    '    '    'Creates a holder image to capture each page
    '    '    imgs = New ArrayList
    '    '    'instantiate the arraylist
    '    '    '
    '    '    'Dim capabilityValue As Object
    '    '    'Dim dataType As DTI.ImageMan.Twain.DataType

    '    '    'Dim retVal As Boolean = tw.GetCapability(DTI.ImageMan.Twain.Capabilities.FeederLoaded, capabilityValue, dataType)

    '    '    'If retVal AndAlso CInt(capabilityValue) <> 0 Then

    '    '    '    If retVal AndAlso CInt(capabilityValue) <> 0 Then
    '    '    '        'MessageBox.Show("Feeder is loaded with paper")
    '    '    '    End If
    '    '    'Else

    '    '    'End If



    '    '    Try
    '    '        img = tw.ScanPage()
    '    '    Catch ex As Exception
    '    '        write_error(ex.ToString, 1)
    '    '        Return False
    '    '    End Try

    '    '    'grab the first page
    '    '    While Not (img Is Nothing)
    '    '        Try
    '    '            imgs.Add(img)
    '    '            img = tw.ScanPage()

    '    '        Catch ex As Exception
    '    '            Console.WriteLine(ex.ToString)
    '    '            Return False
    '    '        End Try
    '    '    End While
    '    'End Using
    '    ''The above loop will keep scanning until there are no pages left to scan
    '    'Dim doc As New PdfDocument
    '    ''next we create an instance of a pdf document to feed the images into we have to do this on an image by image basis merrily creating pages as we go
    '    'Dim count As Integer = 0
    '    ''I use count to keep track on whichpage we are adding. Ready cool off we go then!
    '    'doc.Pages.Add(New PdfPage)
    '    ''Add the pages control into the pdf
    '    'For Each img In imgs
    '    '    'loop through the images

    '    '    Try
    '    '        doc.AddPage()
    '    '        'add a blank page to the pdf
    '    '        Dim xgr As XGraphics
    '    '        xgr = XGraphics.FromPdfPage(doc.Pages(count))
    '    '        'Turn the page into a graphic to allow the image to be added
    '    '        Dim imgx As XImage
    '    '        imgx = XImage.FromGdiPlusImage(img)
    '    '        'As the pdfer only accepts the ximage format I need to convert my image from an in memory image to an ximage
    '    '        xgr.DrawImage(imgx, 0, 0)
    '    '        'draw the image to the page
    '    '    Catch ex As Exception
    '    '        Console.WriteLine(ex.Message)
    '    '        Return False

    '    '        'if any bad things happen (and they can be careful out there kids!!) tell the users about what went wrong
    '    '    End Try
    '    '    count += 1

    '    'Next
    '    Try
    '        If Not Directory.Exists(f.doc_dir) Then
    '            Directory.CreateDirectory(f.doc_dir)
    '            File.Copy("c:\stuff\test.pdf", f.full_file)
    '        Else
    '            File.Copy("c:\stuff\test.pdf", f.full_file)
    '        End If
    '        Threading.Thread.Sleep(50000)
    '        write_log(f.doc_dir, f.file_name, f.doc_date, f.batch_no)
    '        Console.WriteLine("Written to DB : " & f.doc_dir & " " & f.doc_date & " " & f.file_name & " " & f.batch_no)
    '        ''console.readline()


    '    Catch ex As Exception
    '        Console.WriteLine(ex.ToString)
    '        Return False
    '    End Try

    '    Return True

    'End Function
    Public Shared Sub process(ByVal f As ScanDocument)
        Console.WriteLine("Scanning Complete - Checking images for Barcodes")
        Dim img As Image
        Dim count As Integer = 0
        Dim imgind As Integer = 0
        Dim start As Integer = 0
        Dim decoder As New DTI.ImageMan.Barcode.BarcodeDecoder
        Dim filename As String = ""
        Dim scan_type_code As String = ""
        Dim scan_doc_code As String = ""


        Dim yyyy, mm, dd As Integer
        yyyy = DatePart(DateInterval.Year, Today)
        mm = DatePart(DateInterval.Month, Today)
        dd = DatePart(DateInterval.Day, Today)
        Dim fname As String
        Dim extra As String = ""
        Dim SCAN_D_CODE As String


        Dim bformats As New List(Of BarcodeFormat)
        bformats.Add(BarcodeFormat.Code39)
        Dim decodedBarcode As Result
        Dim doc As New PdfDocument
        doc.Pages.Add(New PdfPage)
        For Each img In imgs
            imgind = imgs.IndexOf(img)
            Console.WriteLine("Checking Page - " & (imgind + 1))
            Try

                decodedBarcode = Nothing
                decodedBarcode = decoder.Decode(img, bformats)
                'End If

                If decodedBarcode IsNot Nothing Then
                    Console.WriteLine("Barcode found on page - " & (imgind + 1))
                    Dim hold As String
                    hold = decodedBarcode.Text
                    scan_type_code = hold.Substring(0, 5)
                    scan_doc_code = hold.Substring(6)
                    scan_type_code = scan_type_code.Replace(" ", "")
                    Dim direc As String
                    direc = f.doc_dir & "\" & scan_type_code & "\"
                    If Directory.Exists(direc) = False Then
                        Directory.CreateDirectory(direc)
                    End If



                    If imgind <> 0 Then

                        filename = f.doc_dir & "\" & scan_type_code & "\" & scan_doc_code & yyyy & mm & dd & ".pdf"

                        fname = scan_doc_code & yyyy & mm & dd & ".pdf"

                        SCAN_D_CODE = scan_doc_code

                        If File.Exists(filename) = True Then


                            filename = f.doc_dir & "\" & scan_type_code & "\" & scan_doc_code & yyyy & mm & dd & "_" & 1 & ".pdf"
                            fname = scan_doc_code & yyyy & mm & dd & "_" & 1 & ".pdf"
                            Dim x As Integer = 2
                            Do While File.Exists(filename) = True
                                'This will keep adding to the ending number until it gets to one that it can use
                                filename = f.doc_dir & "\" & scan_type_code & "\" & scan_doc_code & yyyy & mm & dd & "_" & x & ".pdf"
                                fname = scan_doc_code & yyyy & mm & dd & "_" & x & ".pdf"
                                x += 1
                            Loop
                            ScanControl.write_error("File exists - Renaming to " & filename, 1)
                        Else

                        End If



                        doc.Save(filename)
                        doc = Nothing
                        doc = New PdfDocument
                        doc.Pages.Add(New PdfPage)
                        start = imgind + 1
                        count = 0
                        'write_log(ByVal DOC_DIR As String, ByVal File_Name As String, ByVal S_Date As Integer, ByVal S_Batch_NO As Integer)
                        write_log(f.doc_dir, fname, f.doc_date, scan_type_code, SCAN_D_CODE, f.batch_no)


                    End If
                    doc.AddPage()
                    Dim xgr As XGraphics
                    xgr = XGraphics.FromPdfPage(doc.Pages(count))
                    Dim imgx As XImage
                    imgx = XImage.FromGdiPlusImage(img)

                    xgr.DrawImage(imgx, 0, 0)
                End If

            Catch e1 As DTI.ImageMan.Barcode.NotFoundException
                doc.AddPage()
                Dim xgr As XGraphics
                xgr = XGraphics.FromPdfPage(doc.Pages(count))
                Dim imgx As XImage
                imgx = XImage.FromGdiPlusImage(img)

                xgr.DrawImage(imgx, 0, 0)
            Catch ex As Exception
                MsgBox(ex.Message)
            End Try
            count += 1

        Next

        filename = f.doc_dir & "\" & scan_type_code & "\" & scan_doc_code & yyyy & mm & dd & ".pdf"

        fname = scan_doc_code & yyyy & mm & dd & ".pdf"

        SCAN_D_CODE = scan_doc_code

        If File.Exists(filename) = True Then
            'Console.WriteLine("This filename already exists please check the name provided")
            filename = f.doc_dir & "\" & scan_type_code & "\" & scan_doc_code & yyyy & mm & dd & "_" & 1 & ".pdf"
            fname = scan_doc_code & yyyy & mm & dd & "_" & 1 & ".pdf"
            Dim x As Integer = 2
            Do While File.Exists(filename) = True
                'This will keep adding to the ending number until it gets to one that it can use
                filename = f.doc_dir & "\" & scan_type_code & "\" & scan_doc_code & yyyy & mm & dd & "_" & x & ".pdf"
                fname = scan_doc_code & yyyy & mm & dd & "_" & x & ".pdf"
                x += 1
            Loop
            ScanControl.write_error("File exists - Renaming to " & filename, 1)
        End If

        doc.Save(filename)
        write_log(f.doc_dir, fname, f.doc_date, scan_type_code, SCAN_D_CODE, f.batch_no)
        doc = Nothing

    End Sub
End Class

