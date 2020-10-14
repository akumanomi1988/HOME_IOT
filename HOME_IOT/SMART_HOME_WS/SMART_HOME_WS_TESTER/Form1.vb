Public Class Form1
	Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
		Dim I As New SH_WS.SH_WSSoapClient
		Label1.Text = I.HelloWorld(" Diego")
	End Sub
End Class
