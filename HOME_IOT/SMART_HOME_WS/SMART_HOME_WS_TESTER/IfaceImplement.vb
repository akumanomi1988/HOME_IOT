Imports SMART_HOME_WS_TESTER.SH_WS_Produccion

Public Class IfaceImplement
	Implements SH_WS_Produccion.SH_WSSoap

	Public Function Login(Nombre_Dispositivo As String, Usuario As String, Pwd As String) As Boolean Implements SH_WSSoap.Login
		Throw New NotImplementedException()
	End Function

	Public Function LoginAsync(Nombre_Dispositivo As String, Usuario As String, Pwd As String) As Task(Of Boolean) Implements SH_WSSoap.LoginAsync
		Throw New NotImplementedException()
	End Function

	Public Function GET_TemperaturaObjetivo() As String Implements SH_WSSoap.GET_TemperaturaObjetivo
		Throw New NotImplementedException()
	End Function

	Public Function GET_TemperaturaObjetivoAsync() As Task(Of String) Implements SH_WSSoap.GET_TemperaturaObjetivoAsync
		Throw New NotImplementedException()
	End Function

	Public Function SET_TemperaturaObjetivo(NuevaTemp As Double) As String Implements SH_WSSoap.SET_TemperaturaObjetivo
		Throw New NotImplementedException()
	End Function

	Public Function SET_TemperaturaObjetivoAsync(NuevaTemp As Double) As Task(Of String) Implements SH_WSSoap.SET_TemperaturaObjetivoAsync
		Throw New NotImplementedException()
	End Function

	Public Function GET_Calefaccion() As String Implements SH_WSSoap.GET_Calefaccion
		Throw New NotImplementedException()
	End Function

	Public Function GET_CalefaccionAsync() As Task(Of String) Implements SH_WSSoap.GET_CalefaccionAsync
		Throw New NotImplementedException()
	End Function

	Public Function GET_Calefaccion_Encendida() As Boolean Implements SH_WSSoap.GET_Calefaccion_Encendida
		Throw New NotImplementedException()
	End Function

	Public Function GET_Calefaccion_EncendidaAsync() As Task(Of Boolean) Implements SH_WSSoap.GET_Calefaccion_EncendidaAsync
		Throw New NotImplementedException()
	End Function

	Public Function NEW_Lectura(NOMBRE_DISP As String, TEMPERATURA_ACTUAL As Double, HUMEDAD As Double) As Boolean Implements SH_WSSoap.NEW_Lectura
		Throw New NotImplementedException()
	End Function

	Public Function NEW_LecturaAsync(NOMBRE_DISP As String, TEMPERATURA_ACTUAL As Double, HUMEDAD As Double) As Task(Of Boolean) Implements SH_WSSoap.NEW_LecturaAsync
		Throw New NotImplementedException()
	End Function

	Public Function GET_Lecturas(Dispositivo As String, cantidad As Integer) As String Implements SH_WSSoap.GET_Lecturas
		Throw New NotImplementedException()
	End Function

	Public Function GET_LecturasAsync(Dispositivo As String, cantidad As Integer) As Task(Of String) Implements SH_WSSoap.GET_LecturasAsync
		Throw New NotImplementedException()
	End Function

	Public Function GET_Lecturas_Fecha(NombreDispositivo As String, Fecha As Date) As String Implements SH_WSSoap.GET_Lecturas_Fecha
		Throw New NotImplementedException()
	End Function

	Public Function GET_Lecturas_FechaAsync(NombreDispositivo As String, Fecha As Date) As Task(Of String) Implements SH_WSSoap.GET_Lecturas_FechaAsync
		Throw New NotImplementedException()
	End Function

	Public Function GET_Servidor_Fecha() As String Implements SH_WSSoap.GET_Servidor_Fecha
		Throw New NotImplementedException()
	End Function

	Public Function GET_Servidor_FechaAsync() As Task(Of String) Implements SH_WSSoap.GET_Servidor_FechaAsync
		Throw New NotImplementedException()
	End Function

	Public Function GET_Dispositivos_Configuración(NombreDispositivo As String) As String Implements SH_WSSoap.GET_Dispositivos_Configuración
		Throw New NotImplementedException()
	End Function

	Public Function GET_Dispositivos_ConfiguraciónAsync(NombreDispositivo As String) As Task(Of String) Implements SH_WSSoap.GET_Dispositivos_ConfiguraciónAsync
		Throw New NotImplementedException()
	End Function

	Public Function NEW_Dispositivo(NOMBRE_DISP As String, IP As String, ROL As String, LOCALIZACION As String, MODELO As String) As Boolean Implements SH_WSSoap.NEW_Dispositivo
		Throw New NotImplementedException()
	End Function

	Public Function NEW_DispositivoAsync(NOMBRE_DISP As String, IP As String, ROL As String, LOCALIZACION As String, MODELO As String) As Task(Of Boolean) Implements SH_WSSoap.NEW_DispositivoAsync
		Throw New NotImplementedException()
	End Function
End Class
