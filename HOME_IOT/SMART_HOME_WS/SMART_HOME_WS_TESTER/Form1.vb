Imports Newtonsoft.Json
Public Class Form1
	Private WithEvents _binding As BindingSource
	Private _Dispositivo As SMART_HOME_WS.SH_WS.Dispositivo

	Private Sub Form1_Load(sender As Object, e As EventArgs) Handles Me.Load
		_Dispositivo = New SMART_HOME_WS.SH_WS.Dispositivo()
		_Dispositivo.LoadLecturas(100)
		Chart1.DataSource = _Dispositivo.Lecturas
	End Sub

	Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

	End Sub
End Class
