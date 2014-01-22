Imports System.ComponentModel
Imports System.Text
Imports DTI.ImageMan.Twain
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
        Private ConnString As String
        Private Server As String
        Private DataBase As String
        Private UName As String
        Private PassWord As String

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
        Public Property DBServ() As String
            Get
                Return Server
            End Get
            Set(ByVal value As String)
                Server = value
            End Set
        End Property
        Public Property DBName() As String
            Get
                Return DataBase
            End Get
            Set(ByVal value As String)
                DataBase = value
            End Set
        End Property
        Public Property DBUname() As String
            Get
                Return UName
            End Get
            Set(ByVal value As String)
                UName = value
            End Set
        End Property
        Public Property DBPassword() As String
            Get
                Return PassWord
            End Get
            Set(ByVal value As String)
                PassWord = value
            End Set
        End Property
        Public ReadOnly Property ConStr() As String
            Get
                Return String.Format("Data Source={0} ;Uid={1};Pwd={2};Initial Catalog={3}", DBServ, DBUname, DBPassword, DBName)
            End Get
        End Property
        Public ReadOnly Property dir() As String
            Get
                Return My.Application.Info.DirectoryPath
            End Get
        End Property
        Public ReadOnly Property SettingsFile()
            Get
                Return String.Format("{0}\{1}", dir, "settings.xml")
            End Get
        End Property
        Public Sub New(ByVal pt As DTI.ImageMan.Twain.PixelTypes, ByVal pgs As Integer, ByVal uids As DTI.ImageMan.Twain.UserInterfaces, ByVal res As Integer, ByVal serv As String, ByVal una As String, ByVal pas As String, ByVal dbn As String)
            PagType = pt
            mpg = pgs
            uid = uids
            resol = res
            DBServ = serv
            DBUname = una
            DBPassword = pas
            DBName = dbn
        End Sub
        Public Sub New()
            If File.Exists(SettingsFile) = False Then
                Console.WriteLine("Application Failed to read the settings file")
            Else
                Dim doc As New XDocument
                doc = XDocument.Load(SettingsFile)
                PagType = doc.<Settings>.<PixType>.Value
                mpg = doc.<Settings>.<MaxPage>.Value
                uid = doc.<Settings>.<UsrIntFc>.Value
                resol = doc.<Settings>.<Res>.Value
                DBServ = doc.<Settings>.<DBServ>.Value
                DBUname = doc.<Settings>.<DBUname>.Value
                DBPassword = doc.<Settings>.<DBPass>.Value
                DBName = doc.<Settings>.<DBName>.Value

            End If


        End Sub

        Public Sub writesettings(ByVal PixType As Integer, ByVal MaxPage As Integer, ByVal UsrIntFc As Integer, ByVal Res As Integer)

           
        End Sub

        Public Sub LoadSettings()

        End Sub
    End Class
    Public Class ScanDocument
        Private SCAN_DOC_DIR As String
        Private SCAN_FILE_NAME As String
        Private SCAN_DATE As Integer
        Private SCAN_PROCESSED As Char
        Private SCAN_BATCH_NO As Integer
        'Private SCAN_TYPE_CODE As String
        'Private SCAN_USER As Integer
        'Private SCAN_COMPANY As String
        Private SCAN_Full_File As String
        Public Property doc_dir() As String
            Get
                Return SCAN_DOC_DIR
            End Get
            Set(ByVal value As String)
                SCAN_DOC_DIR = value
            End Set
        End Property
        Public Property file_name() As String
            Get
                Return SCAN_FILE_NAME
            End Get
            Set(ByVal value As String)
                SCAN_FILE_NAME = value
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
        'Public Property user() As Integer
        '    Get
        '        Return SCAN_USER
        '    End Get
        '    Set(ByVal value As Integer)
        '        SCAN_USER = value
        '    End Set
        'End Property
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
        Public Property full_file() As String
            Get
                Return SCAN_Full_File
            End Get
            Set(ByVal value As String)
                SCAN_Full_File = value
            End Set
        End Property
        'Public Property type_code() As String
        '    Get
        '        Return SCAN_TYPE_CODE
        '    End Get
        '    Set(ByVal value As String)
        '        SCAN_TYPE_CODE = value
        '    End Set
        'End Property
        'Public Property Company() As String
        '    Get
        '        Return SCAN_COMPANY
        '    End Get
        '    Set(ByVal value As String)
        '        SCAN_COMPANY = value
        '    End Set
        'End Property
        Public Sub New(ByVal dir As String, ByVal name As String, ByVal sdate As Integer, ByVal proc As Char, ByVal bno As Integer)
            'Company = S_Company
            doc_dir = dir
            file_name = name
            doc_date = sdate
            processed = proc
            batch_no = bno
            'type_code = tcode
            'user = usr
        End Sub
    End Class
    Public Shared Function scannall(ByVal f As ScanDocument)
        Dim D As ScanControl.ScanSettings
        D = New ScanControl.ScanSettings()
        Dim imgs As New ArrayList
        'The arraylist will hold all the pages scanned in as bitmap images. I have used arraylist as I dont have to specify any bounds as I dont know them
        Dim tw As New DTI.ImageMan.Twain.TwainControl
        Using tw


            With tw
                .PixelType = D.PagType
                .MaxPages = D.mpg
                .UserInterface = UserInterfaces.None
                .Resolution = D.resolu
            End With
            'Instantiates the twain control
            Dim img As Image
            'Creates a holder image to capture each page
            imgs = New ArrayList
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
        Dim doc As New PdfDocument
        'next we create an instance of a pdf document to feed the images into we have to do this on an image by image basis merrily creating pages as we go
        Dim count As Integer = 0
        'I use count to keep track on whichpage we are adding. Ready cool off we go then!
        doc.Pages.Add(New PdfPage)
        'Add the pages control into the pdf
        For Each img In imgs
            'loop through the images

            Try
                doc.AddPage()
                'add a blank page to the pdf
                Dim xgr As XGraphics
                xgr = XGraphics.FromPdfPage(doc.Pages(count))
                'Turn the page into a graphic to allow the image to be added
                Dim imgx As XImage
                imgx = XImage.FromGdiPlusImage(img)
                'As the pdfer only accepts the ximage format I need to convert my image from an in memory image to an ximage
                xgr.DrawImage(imgx, 0, 0)
                'draw the image to the page
            Catch ex As Exception
                Console.WriteLine(ex.Message)
                Return False

                'if any bad things happen (and they can be careful out there kids!!) tell the users about what went wrong
            End Try
            count += 1

        Next
        Try
            If Not Directory.Exists(f.doc_dir) Then
                Directory.CreateDirectory(f.doc_dir)
            End If
            doc.Save(f.full_file)
            write_log(f.doc_dir & "/", f.file_name, f.doc_date, "Single", f.file_name, f.batch_no, D.ConStr)
            Console.WriteLine("Written to DB : " & f.doc_dir & " " & f.doc_date & " " & f.file_name & " " & f.batch_no)


            doc = Nothing
        Catch ex As Exception
            Console.WriteLine(ex.ToString)
            Return False
        End Try

        Return True

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
    Public Shared Sub write_log(ByVal DOC_DIR As String, ByVal File_Name As String, ByVal S_Date As Integer, ByVal sc_ty_co As String, ByVal SDC As String, ByVal S_Batch_NO As Integer, ByVal cn As String)
        Try
            Dim con As New SqlConnection
            Dim cmd As New SqlCommand
            With con
                .ConnectionString = cn 'GetConnectionString("PriorityDB")
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


    Public Shared Function scannall_fake(ByVal f As ScanDocument)
        Dim D As ScanControl.ScanSettings
        D = New ScanControl.ScanSettings()
        Dim imgs As New ArrayList
       
        Try
            If Not Directory.Exists(f.doc_dir) Then
                Directory.CreateDirectory(f.doc_dir)
                File.Copy("c:\stuff\test.pdf", f.full_file)
            Else
                File.Copy("c:\stuff\test.pdf", f.full_file)
            End If
            Threading.Thread.Sleep(50000)
            write_log(f.doc_dir, f.file_name, f.doc_date, "Single", f.file_name, f.batch_no, D.ConStr)
            Console.WriteLine("Written to DB : " & f.doc_dir & " " & f.doc_date & " " & f.file_name & " " & f.batch_no)
            'Console.ReadLine()


        Catch ex As Exception
            Console.WriteLine(ex.ToString)
            Return False
        End Try

        Return True

    End Function

End Class
