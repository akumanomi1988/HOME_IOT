Imports Newtonsoft.Json
Public Class Form1
	Private WithEvents _binding As BindingSource
	Private _wsClient As SH_WS_Produccion.SH_WSSoapClient
	Private _calefaccion As SMART_HOME_WS.SH_WS.Calefaccion
	Private Sub Form1_Load(sender As Object, e As EventArgs) Handles Me.Load
		_wsClient = New SH_WS_Produccion.SH_WSSoapClient()
		_calefaccion = JsonConvert.DeserializeObject(_wsClient.GET_Calefaccion(), GetType(SMART_HOME_WS.SH_WS.Calefaccion))
		_binding.DataSource = _wsClient.GET_Calefaccion()
	End Sub
End Class
