Imports Newtonsoft.Json
Public Class Form1
	Private WithEvents _binding As BindingSource
	Private _Dispositivo As SMART_HOME_WS.SH_WS.Dispositivo

	Private Sub Form1_Load(sender As Object, e As EventArgs) Handles Me.Load
		_Dispositivo = New
		Chart1.DataSource = _Dispositivo.lecturas
	End Sub

	Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

	End Sub
End Class
