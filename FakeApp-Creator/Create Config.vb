Imports System
Imports System.IO
Imports System.Xml
Imports System.Text
Imports System.Security.Cryptography
Imports Ionic.Zip

Public Class Create_Config
    Public frmr As Form
    Private Sub BtnExit_Click(sender As Object, e As EventArgs) Handles BtnExit.Click
        frmr.Show()
        Me.Close()
    End Sub

    Private Sub MetroButton1_Click(sender As Object, e As EventArgs)

    End Sub
    Sub DeleteFilesFromFolder(Folder As String)
        Try


            If Directory.Exists(Folder) Then
                For Each _file As String In Directory.GetFiles(Folder)
                    File.Delete(_file)
                Next
                For Each _folder As String In Directory.GetDirectories(Folder)

                    DeleteFilesFromFolder(_folder)
                Next

            End If
        Catch ex As Exception

        End Try
    End Sub
    Private Sub MetroButton4_Click(sender As Object, e As EventArgs) Handles MetroButton4.Click
        If txtWelcome.Text <> "" And MetroTextBox1.Text <> "" And MetroTextBox2.Text <> "" Then
            DeleteFilesFromFolder("Tools\Working")
            My.Computer.FileSystem.CopyFile(txtWelcome.Text, "Tools\Working\package.pac")
            My.Computer.FileSystem.CopyFile(MetroTextBox1.Text, "Tools\Working\Conf.ini")
            My.Computer.FileSystem.CopyFile(MetroTextBox2.Text, "Tools\Working\preview.per")
            My.Computer.FileSystem.CopyFile(txtServer.Text, "Tools\Working\server.srv")
            CFile("config.fac")
            Using zip As ZipFile = New ZipFile()
                zip.Password = GenerateHash((MetroTextBox3.Text))
                zip.Encryption = EncryptionAlgorithm.WinZipAes256
                zip.AddFile("Tools\Working\package.pac", "")
                zip.AddFile("Tools\Working\Conf.ini", "")
                zip.AddFile("Tools\Working\preview.per", "")
                zip.AddFile("Tools\Working\server.srv", "")
                zip.Save("config.fac")
            End Using

            lblMSG.Text = "Done ! Saved In Config.FAC"
            DeleteFilesFromFolder("Tools\Working")
        Else
            lblMSG.Text = "Please Load Files First !"
        End If
    End Sub
    Sub CFile(ByVal file As String)
        If My.Computer.FileSystem.FileExists(file) Then
            My.Computer.FileSystem.DeleteFile(file)
        End If
    End Sub
    Private Sub MetroButton1_Click_1(sender As Object, e As EventArgs) Handles MetroButton1.Click
        OpenFileDialog1.Title = "Select PAC File"
        OpenFileDialog1.Filter = "PAC File|*.pac"
        OpenFileDialog1.Multiselect = False
        If OpenFileDialog1.ShowDialog = DialogResult.OK Then
            txtWelcome.Text = OpenFileDialog1.FileName
        End If
    End Sub

    Private Sub MetroButton2_Click(sender As Object, e As EventArgs) Handles MetroButton2.Click
        OpenFileDialog1.Title = "Select INI File"
        OpenFileDialog1.Filter = "INI File|*.ini"
        OpenFileDialog1.Multiselect = False
        If OpenFileDialog1.ShowDialog = DialogResult.OK Then
            MetroTextBox1.Text = OpenFileDialog1.FileName
        End If
    End Sub

    Private Sub MetroButton3_Click(sender As Object, e As EventArgs) Handles MetroButton3.Click
        OpenFileDialog1.Title = "Select Preview File"
        OpenFileDialog1.Filter = "Preview File|*.per"
        OpenFileDialog1.Multiselect = False
        If OpenFileDialog1.ShowDialog = DialogResult.OK Then
            MetroTextBox2.Text = OpenFileDialog1.FileName
        End If
    End Sub

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

    Private Sub Create_Config_Load(sender As Object, e As EventArgs) Handles Me.Load

    End Sub

    Private Sub MetroButton5_Click(sender As Object, e As EventArgs) Handles MetroButton5.Click
        OpenFileDialog1.Title = "Select Server File"
        OpenFileDialog1.Filter = "Server File|*.srv"
        OpenFileDialog1.Multiselect = False
        If OpenFileDialog1.ShowDialog = DialogResult.OK Then
            txtServer.Text = OpenFileDialog1.FileName
        End If
    End Sub
End Class