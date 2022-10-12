Public Class Form1
    Public rand As New Random
    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Me.RichTextBox1.AppendText(rand.Next & vbCrLf)
    End Sub

    Dim playpause As Integer = 0
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If playpause = 0 Then
            Me.Button1.Text = "Pause"
            playpause = 1
            Timer1.Start()
        Else
            Me.Button1.Text = "Play"
            playpause = 0
            Timer1.Stop()
        End If

    End Sub
End Class
