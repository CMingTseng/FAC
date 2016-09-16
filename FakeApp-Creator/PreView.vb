Imports System.IO
Imports System.Security
Imports Microsoft.Win32

Public Class PreView
    Public frm As Form
    Public html As String
    Public title, footer As String
    Public Enum BrowserEmulationVersion
        [Default] = 0
        Version7 = 7000

        Version8 = 8000
        Version8Standards = 8888
        Version9 = 9000
        Version9Standards = 9999
        Version10 = 10000
        Version10Standards = 10001
        Version11 = 11000
        Version11Edge = 11001
    End Enum

    Private Sub BtnExit_Click(sender As Object, e As EventArgs) Handles BtnExit.Click

        frm.Show()
        Me.Close()
    End Sub
    Private Const InternetExplorerRootKey As String = "Software\Microsoft\Internet Explorer"

    Public Shared Function GetInternetExplorerMajorVersion() As Integer
        Dim result As Integer

        result = 0

        Try
            Dim key As RegistryKey

            key = Registry.LocalMachine.OpenSubKey(InternetExplorerRootKey)

            If key IsNot Nothing Then
                Dim value As Object

                value = If(key.GetValue("svcVersion", Nothing), key.GetValue("Version", Nothing))

                If value IsNot Nothing Then
                    Dim version As String
                    Dim separator As Integer

                    version = value.ToString()
                    separator = version.IndexOf("."c)
                    If separator <> -1 Then
                        Integer.TryParse(version.Substring(0, separator), result)
                    End If
                End If
            End If
            ' The user does not have the permissions required to read from the registry key.
        Catch generatedExceptionName As SecurityException
            ' The user does not have the necessary registry rights.
        Catch generatedExceptionName As UnauthorizedAccessException
        End Try

        Return result
    End Function

    Private Const BrowserEmulationKey As String = InternetExplorerRootKey + "\Main\FeatureControl\FEATURE_BROWSER_EMULATION"

    Public Shared Function GetBrowserEmulationVersion() As BrowserEmulationVersion
        Dim result As BrowserEmulationVersion

        result = BrowserEmulationVersion.[Default]

        Try
            Dim key As RegistryKey

            key = Registry.CurrentUser.OpenSubKey(BrowserEmulationKey, True)
            If key IsNot Nothing Then
                Dim programName As String
                Dim value As Object

                programName = Path.GetFileName(Environment.GetCommandLineArgs()(0))
                value = key.GetValue(programName, Nothing)

                If value IsNot Nothing Then
                    result = DirectCast(Convert.ToInt32(value), BrowserEmulationVersion)
                End If
            End If
            ' The user does not have the permissions required to read from the registry key.
        Catch generatedExceptionName As SecurityException
            ' The user does not have the necessary registry rights.
        Catch generatedExceptionName As UnauthorizedAccessException
        End Try

        Return result
    End Function
    Public Shared Function SetBrowserEmulationVersion(browserEmulationVersion__1 As BrowserEmulationVersion) As Boolean
        Dim result As Boolean

        result = False

        Try
            Dim key As RegistryKey

            key = Registry.CurrentUser.OpenSubKey(BrowserEmulationKey, True)

            If key IsNot Nothing Then
                Dim programName As String

                programName = Path.GetFileName(Environment.GetCommandLineArgs()(0))

                If browserEmulationVersion__1 <> BrowserEmulationVersion.[Default] Then
                    ' if it's a valid value, update or create the value
                    key.SetValue(programName, CInt(browserEmulationVersion__1), RegistryValueKind.DWord)
                Else
                    ' otherwise, remove the existing value
                    key.DeleteValue(programName, False)
                End If

                result = True
            End If
            ' The user does not have the permissions required to read from the registry key.
        Catch generatedExceptionName As SecurityException
            ' The user does not have the necessary registry rights.
        Catch generatedExceptionName As UnauthorizedAccessException
        End Try

        Return result
    End Function

    Public Shared Function SetBrowserEmulationVersion() As Boolean
        Dim ieVersion As Integer
        Dim emulationCode As BrowserEmulationVersion

        ieVersion = GetInternetExplorerMajorVersion()

        If ieVersion >= 11 Then
            emulationCode = BrowserEmulationVersion.Version11
        Else
            Select Case ieVersion
                Case 10
                    emulationCode = BrowserEmulationVersion.Version10
                    Exit Select
                Case 9
                    emulationCode = BrowserEmulationVersion.Version9
                    Exit Select
                Case 8
                    emulationCode = BrowserEmulationVersion.Version8
                    Exit Select
                Case Else
                    emulationCode = BrowserEmulationVersion.Version7
                    Exit Select
            End Select
        End If

        Return SetBrowserEmulationVersion(emulationCode)
    End Function

    Public Shared Function IsBrowserEmulationSet() As Boolean
        Return GetBrowserEmulationVersion() <> BrowserEmulationVersion.[Default]
    End Function


    Private Sub PreView_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsBrowserEmulationSet() Then
            SetBrowserEmulationVersion()
        End If

        WebBrowser1.IsWebBrowserContextMenuEnabled = False
        Me.WebBrowser1.Navigate("about:blank")
        Me.WebBrowser1.Document.Write(String.Empty)
        Me.WebBrowser1.DocumentText = html
    End Sub
End Class