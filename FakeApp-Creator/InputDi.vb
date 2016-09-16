Public Class InputDi
    Public frm As Form
    Public title As String
    Public que As String

    Private Sub MetroButton1_Click(sender As Object, e As EventArgs) Handles MetroButton1.Click
        frm.Text = MetroTextBox1.Text
        frm.Visible = True
        frm.Show()
        Me.Close()
    End Sub

    Private Sub InputDi_Load(sender As Object, e As EventArgs) Handles Me.Load
        MetroTile1.Text = title
        MetroLabel1.Text = que
    End Sub
End Class