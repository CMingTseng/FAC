Public Class agree4
    Public frm As Form
    Private Sub MetroButton2_Click(sender As Object, e As EventArgs) Handles MetroButton2.Click
        Application.Exit()
    End Sub

    Private Sub MetroButton1_Click(sender As Object, e As EventArgs) Handles MetroButton1.Click
        My.Computer.FileSystem.WriteAllText("Ag", "", False)
        frm.Show()
        Me.Close()
    End Sub

    Private Sub agree4_Load(sender As Object, e As EventArgs) Handles Me.Load
        MetroTextBox1.Text = My.Resources.GNU.ToString
    End Sub
End Class