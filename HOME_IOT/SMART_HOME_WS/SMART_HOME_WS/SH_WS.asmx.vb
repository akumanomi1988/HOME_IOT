Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports System.Data.SqlClient
Imports Newtonsoft.Json
Imports System.Web.Script.Services

<WebService(Namespace:="http://tempuri.org/")>
<WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)>
Public Class SH_WS
	Inherits System.Web.Services.WebService
	Shared _calefaccion As Calefaccion

	Public Sub New()
		If _calefaccion Is Nothing Then _calefaccion = New Calefaccion
	End Sub
	<WebMethod()>
	<Script.Services.ScriptMethod(ResponseFormat:=ResponseFormat.Json, UseHttpGet:=True, XmlSerializeString:=True)>
	Public Function Login(ByVal Nombre_Dispositivo As String, ByVal Usuario As String, ByVal Pwd As String) As Boolean
		Dim dispo As Dispositivo
		dispo = _calefaccion.Dispositivos.Find(Function(x) x.nombre = Nombre_Dispositivo)
		dispo.conectado = True
		Return JsonConvert.SerializeObject(dispo.conectado, Formatting.Indented)
	End Function
	<WebMethod()>
	<Script.Services.ScriptMethod(ResponseFormat:=ResponseFormat.Json, UseHttpGet:=True, XmlSerializeString:=True)>
	Public Function GET_TemperaturaObjetivo() As String
		Return JsonConvert.SerializeObject(_calefaccion.TemperaturaObjetivo, Formatting.Indented)
	End Function
	<WebMethod()>
	<Script.Services.ScriptMethod(ResponseFormat:=ResponseFormat.Json, UseHttpGet:=True, XmlSerializeString:=True)>
	Public Function SET_TemperaturaObjetivo(ByVal NuevaTemp As Double) As String
		_calefaccion.TemperaturaObjetivo = NuevaTemp
		Return JsonConvert.SerializeObject(_calefaccion.TemperaturaObjetivo, Formatting.Indented)
	End Function
	<WebMethod()>
	<Script.Services.ScriptMethod(ResponseFormat:=ResponseFormat.Json, UseHttpGet:=True, XmlSerializeString:=True)>
	Public Function GET_Calefaccion() As String
		Return JsonConvert.SerializeObject(_calefaccion, Formatting.Indented)
	End Function
	<WebMethod()>
	<Script.Services.ScriptMethod(ResponseFormat:=ResponseFormat.Json, UseHttpGet:=True, XmlSerializeString:=True)>
	Public Function GET_Calefaccion_Encendida() As Boolean
		Return JsonConvert.SerializeObject(_calefaccion.Encendido, Formatting.Indented)
	End Function
	<WebMethod()>
	Public Function NEW_Lectura(ByVal NOMBRE_DISP As String, ByVal TEMPERATURA_ACTUAL As Double, ByVal HUMEDAD As Double) As Boolean
		Dim lectura As New Lectura(NOMBRE_DISP, TEMPERATURA_ACTUAL, HUMEDAD, DateTime.Now)
		Dim dispo As Dispositivo
		dispo = _calefaccion.Dispositivos.Find(Function(x) x.nombre = NOMBRE_DISP)
		dispo.conectado = True
		dispo.lecturas.Add(lectura)
		dispo.LecturaMediaEnTiempo(lectura, 5)
		Return lectura.insertada
	End Function

	<WebMethod()>
	Public Function GET_Lecturas(ByVal Dispositivo As String, ByVal cantidad As Integer) As String
		_calefaccion.Dispositivos.Find(Function(x) x.nombre = Dispositivo).LoadLecturas(cantidad)
		Return JsonConvert.SerializeObject(_calefaccion.Dispositivos.Find(Function(x) x.nombre = Dispositivo).lecturas.FindAll(Function(L) L.insertada = True), Formatting.Indented)
	End Function
	<WebMethod()>
	Public Function GET_Lecturas_Fecha(ByVal NombreDispositivo As String, ByVal Fecha As Date) As String
		Return JsonConvert.SerializeObject(_calefaccion.Dispositivos.Find(Function(x) x.nombre = NombreDispositivo).lecturas.FindAll(Function(L) L.fecha_hora.Date = Fecha.Date), Formatting.Indented)
	End Function
	<WebMethod()>
	Public Function GET_Servidor_Fecha() As String
		Return DateTime.Now.ToString
	End Function
	<WebMethod()>
	Public Function GET_Dispositivos_Configuración(ByVal NombreDispositivo As String) As String
		Return JsonConvert.SerializeObject(_calefaccion.Dispositivos.Find(Function(x) x.nombre = NombreDispositivo), Formatting.Indented)
	End Function
	<WebMethod()>
	Public Function NEW_Dispositivo(ByVal NOMBRE_DISP As String, ByVal IP As String, ByVal ROL As String, ByVal LOCALIZACION As String, ByVal MODELO As String) As Boolean
		If _calefaccion.Dispositivos.FindIndex(Function(x) x.nombre = NOMBRE_DISP) >= 0 Then
			Return False
		Else
			Dim dispo As New Dispositivo(NOMBRE_DISP, IP, ROL, LOCALIZACION, MODELO)
			_calefaccion.Dispositivos.Add(dispo)
			Return dispo.Guardar()
		End If
	End Function
	Public Class Dispositivo
#Region "Propiedades"
		Private _nombre As String
		Private _IP As String
		Private _rol As String
		Private _localizacion As String
		Private _modelo As String
		Private _lecturas As List(Of Lectura)
		Private _conectado As Boolean
		Private _tiempoReconexion As Integer
		Public Property tiempoReconexion() As Integer
			Get
				Return _tiempoReconexion
			End Get
			Set(ByVal value As Integer)
				_tiempoReconexion = value
			End Set
		End Property
		Public Property conectado() As Boolean
			Get
				Return _conectado
			End Get
			Set(ByVal value As Boolean)
				If value Then
					tiempoReconexion = 10
				End If
				_conectado = value
			End Set
		End Property
		Public Property nombre() As String
			Get
				Return _nombre
			End Get
			Set(ByVal value As String)
				_nombre = value
			End Set
		End Property
		Public Property IP() As String
			Get
				Return _IP
			End Get
			Set(ByVal value As String)
				_IP = value
			End Set
		End Property
		Public Property rol() As String
			Get
				Return _rol
			End Get
			Set(ByVal value As String)
				_rol = value
			End Set
		End Property
		Public Property localizacion() As String
			Get
				Return _localizacion
			End Get
			Set(ByVal value As String)
				_localizacion = value
			End Set
		End Property
		Public Property modelo() As String
			Get
				Return _modelo
			End Get
			Set(ByVal value As String)
				_modelo = value
			End Set
		End Property
		Public Property lecturas() As List(Of Lectura)
			Get
				Return _lecturas
			End Get
			Set(ByVal value As List(Of Lectura))
				_lecturas = value
			End Set
		End Property
#End Region
#Region "Constructor"
		Public Sub New(nombre As String, iP As String, rol As String, localizacion As String, modelo As String, Optional CantidadLecturasCargaInicial As Integer = 1)
			_nombre = nombre
			_IP = iP
			_rol = rol
			_localizacion = localizacion
			_modelo = modelo
			_lecturas = New List(Of Lectura)
			LoadLecturas(CantidadLecturasCargaInicial)
		End Sub
#End Region
#Region "Funciones"
		Public Function Guardar() As Boolean
			Dim query As String
			Dim dt As New DataTable
			query = $"USE [HomePi] INSERT INTO [dbo].[DISPOSITIVOS] 
			([NOMBRE_DISP] ,[IP] ,[ROL] ,[LOCALIZACION] ,[MODELO]) VALUES(@NOMBRE_DISP,@IP,@ROL,@LOCALIZACION,@MODELO)"
			Using conn As New SqlConnection(My.Settings.CONN)
				Using comm As New SqlCommand()
					With comm
						.Connection = conn
						.CommandType = CommandType.Text
						.CommandText = query
						.Parameters.AddWithValue("@NOMBRE_DISP", nombre)
						.Parameters.AddWithValue("@IP", IP)
						.Parameters.AddWithValue("@ROL", rol)
						.Parameters.AddWithValue("@LOCALIZACION", localizacion)
						.Parameters.AddWithValue("@MODELO", modelo)
					End With
					Try
						conn.Open()
						comm.ExecuteNonQuery()
					Catch ex As Exception
						Return False
					End Try
					Return True
				End Using
			End Using
		End Function
		Public Sub LoadLecturas(cantidad As Integer)
			Dim query As String
			Dim dt As New DataTable
			If cantidad = 0 Then cantidad = 1
			query = $"USE [HomePi] SELECT TOP {cantidad} * FROM [dbo].[LECTURAS] where NOMBRE_DISP = @NOMBRE_DISP ORDER BY FECHA DESC"
			Using conn As New SqlConnection(My.Settings.CONN)
				conn.Open()
				Using com As New SqlCommand
					With com
						.Connection = conn
						.CommandType = CommandType.Text
						.CommandText = query
						.Parameters.AddWithValue("@NOMBRE_DISP", nombre)
					End With
					Using da As New SqlDataAdapter(com)
						da.Fill(dt)
					End Using
				End Using
			End Using
			lecturas.RemoveAll(Function(x) x.insertada = True)
			For Each row As DataRow In dt.Rows
				Try
					Me.lecturas.Add(New Lectura(row("NOMBRE_DISP"), row("TEMPERATURA_ACTUAL"), row("HUMEDAD"), row("FECHA"), True))
				Catch
					Console.WriteLine("Error: Lectura ya existe en la lista")
				End Try
			Next
			Me.lecturas = Me.lecturas.OrderByDescending(Function(x) x.fecha_hora).ToList
		End Sub
		Public Function LecturaMediaEnTiempo(lectura As Lectura, minutos As Integer) As Lectura
			Dim tiempo As DateTime
			Dim lecTMP As Lectura
			tiempo = Now.AddMinutes(minutos * -1)
			Dim lec As Lectura = lecturas.FindLast(Function(x) x.nombreDispositivo = lectura.nombreDispositivo And x.insertada = True)
			If lec IsNot Nothing Then
				If tiempo > lec.fecha_hora Then
					Dim tmp_listaLecturas As List(Of Lectura) = lecturas.FindAll(Function(x) x.fecha_hora > lec.fecha_hora)
					Dim temMedia As Double
					Dim hmedia As Double
					For i = 0 To tmp_listaLecturas.Count - 1
						temMedia += tmp_listaLecturas(i).temperatura
						hmedia += tmp_listaLecturas(i).humedad
					Next
					lecTMP = New Lectura(lectura.nombreDispositivo, temMedia / tmp_listaLecturas.Count, hmedia / tmp_listaLecturas.Count, Now)
					If lecTMP.Guardar() Then
						lecTMP.insertada = True
						lecturas.Add(lecTMP)
					End If
					Return lecTMP
				Else
					Return lectura
				End If
			Else
				If lectura.Guardar() Then
					lectura.insertada = True
				End If

				Return lectura
			End If
		End Function
#End Region
	End Class
	Public Class Configuracion
		Public Event Cambio()
#Region "Propiedades"
		Private _TemperaturaObjetivo As Double
		Private _Histeresis As Double
		Private _UsuarioCambio As String
		Private _FechaCambio As DateTime
		Private _FrecuenciaMuestreo As Integer
		Public Property FrecuenciaMuestreo() As Integer
			Get
				Return _FrecuenciaMuestreo
			End Get
			Set(ByVal value As Integer)
				_FrecuenciaMuestreo = value
			End Set
		End Property
		Public Property TemperaturaObjetivo() As Double
			Get
				Return _TemperaturaObjetivo
			End Get
			Set(ByVal value As Double)
				_TemperaturaObjetivo = value
			End Set
		End Property
		Public Property Histeresis() As Double
			Get
				Return _Histeresis
			End Get
			Set(ByVal value As Double)
				_Histeresis = value
			End Set
		End Property
		Public Property UsuarioCambio() As String
			Get
				Return _UsuarioCambio
			End Get
			Set(ByVal value As String)
				_UsuarioCambio = value
			End Set
		End Property
		Public Property FechaCambio() As DateTime
			Get
				Return _FechaCambio
			End Get
			Set(ByVal value As DateTime)
				_FechaCambio = value
			End Set
		End Property

#End Region
#Region "Constructores"
		Public Sub New(TemperaturaObjetivo As Double, histeresis As Double, usuarioCambio As String, fechaCambio As Date)
			_TemperaturaObjetivo = TemperaturaObjetivo
			_Histeresis = histeresis
			_UsuarioCambio = usuarioCambio
			_FechaCambio = fechaCambio

		End Sub
		Public Sub New(TemperaturaObjetivo As Double, histeresis As Double, usuarioCambio As String, fechaCambio As Date, frecuenciaMuestreo As Integer)
			_TemperaturaObjetivo = TemperaturaObjetivo
			_Histeresis = histeresis
			_UsuarioCambio = usuarioCambio
			_FechaCambio = fechaCambio
			_FrecuenciaMuestreo = frecuenciaMuestreo
		End Sub
		Public Sub New()
		End Sub
#End Region
#Region "Funciones"
		Public Function updateConfiguracion() As Boolean
			Dim query As String
			Dim dt As New DataTable
			query = $"UPDATE CONFIGURACION set
					FECHA_CAMBIO = @FECHA_CAMBIO,
					HISTERESIS = @HISTERESIS,
					TEMPERATURA_DESTINO = @TEMPERATURA_DESTINO"
			Using conn As New SqlConnection(My.Settings.CONN)
				Using comm As New SqlCommand()
					With comm
						.Connection = conn
						.CommandType = CommandType.Text
						.CommandText = query
						.Parameters.AddWithValue("@HISTERESIS", _Histeresis)
						.Parameters.AddWithValue("@FECHA_CAMBIO", DateTime.Now)
						.Parameters.AddWithValue("@TEMPERATURA_DESTINO", _TemperaturaObjetivo)
					End With
					Try
						conn.Open()
						comm.ExecuteNonQuery()
						RaiseEvent Cambio()
					Catch ex As Exception
						Return False
					End Try
				End Using
			End Using
			Return True
		End Function
#End Region
	End Class
	Public Class Lectura
#Region "Propiedades"
		Private _nombreDispositivo As String
		Private _temperatura As Double
		Private _humedad As Double
		Private _fecha_hora As Date
		Private _insertada As Boolean
		Public Property fecha_hora As DateTime
			Get
				Return _fecha_hora
			End Get
			Set
				_fecha_hora = Value
			End Set
		End Property
		Public Property nombreDispositivo As String
			Get
				Return _nombreDispositivo
			End Get
			Set
				_nombreDispositivo = Value
			End Set
		End Property
		Public Property temperatura As Double
			Get
				Return _temperatura
			End Get
			Set
				_temperatura = Value
			End Set
		End Property
		Public Property humedad As Double
			Get
				Return _humedad
			End Get
			Set
				_humedad = Value
			End Set
		End Property
		Public Property insertada As Boolean
			Get
				Return _insertada
			End Get
			Set
				_insertada = Value
			End Set
		End Property
#End Region
#Region "Constructores"
		Public Sub New(pNOMBRE_DISP As String, pTEMPERATURA_ACTUAL As Double, pHUMEDAD As Double, pFECHA_HORA As DateTime)
			Me.nombreDispositivo = pNOMBRE_DISP
			Me.temperatura = pTEMPERATURA_ACTUAL
			Me.humedad = pHUMEDAD
			Me.fecha_hora = pFECHA_HORA
		End Sub
		Public Sub New(pNOMBRE_DISP As String, pTEMPERATURA_ACTUAL As Double, pHUMEDAD As Double, pFECHA_HORA As DateTime, ByVal pInsertada As Boolean)
			Me.nombreDispositivo = pNOMBRE_DISP
			Me.temperatura = pTEMPERATURA_ACTUAL
			Me.humedad = pHUMEDAD
			Me.fecha_hora = pFECHA_HORA
			Me.insertada = pInsertada
		End Sub
#End Region
#Region "Funciones"
		Public Function Guardar() As Boolean
			Dim query As String
			query = " USE [HomePi]
			INSERT INTO [dbo].[LECTURAS]
			([NOMBRE_DISP] ,[TEMPERATURA_ACTUAL] ,[HUMEDAD])
			VALUES
			(@NOMBRE_DISP,@TEMPERATURA_ACTUAL,@HUMEDAD) "
			Using conn As New SqlConnection(My.Settings.CONN)
				Using comm As New SqlCommand()
					With comm
						.Connection = conn
						.CommandType = CommandType.Text
						.CommandText = query
						.Parameters.AddWithValue("@NOMBRE_DISP", _nombreDispositivo)
						.Parameters.AddWithValue("@TEMPERATURA_ACTUAL", _temperatura)
						.Parameters.AddWithValue("@HUMEDAD", _humedad)
					End With
					Try
						conn.Open()
						comm.ExecuteNonQuery()
					Catch ex As Exception
						Return False
					End Try
				End Using
			End Using
			Return True
		End Function
#End Region
	End Class
	Public Class Calefaccion
		Inherits Configuracion
#Region "Propiedades"

		Private _Encendido As Boolean
		Private _Dispositivos As List(Of Dispositivo)
		'Private _Configuracion As Configuracion
		Public ReadOnly Property Encendido() As Boolean
			Get
				Calefaccion_Estado()
				Return _Encendido
			End Get
		End Property
		Public Property Dispositivos() As List(Of Dispositivo)
			Get
				Return _Dispositivos
			End Get
			Set(ByVal value As List(Of Dispositivo))
				_Dispositivos = value
			End Set
		End Property
		'Public Property Configuracion() As Configuracion
		'	Get
		'		Return _Configuracion
		'	End Get
		'	Set(ByVal value As Configuracion)
		'		_Configuracion = value
		'	End Set
		'End Property
#End Region
#Region "Constructores"
		Public Sub New()
			Me._Dispositivos = recupera_Dispositivos()
			GET_myConfiguracion()
			AddHandler Cambio, AddressOf reloadConfiguracion
		End Sub

#End Region
#Region "Funciones"
		Private Sub Calefaccion_Estado()
			If TemperaturaObjetivo - Histeresis > GET_MediaUltimasLecturas() And _Encendido = False Then
				_Encendido = True
			End If
			If TemperaturaObjetivo - Histeresis < GET_MediaUltimasLecturas() And _Encendido = True Then
				_Encendido = False
			End If
		End Sub
		Private Function GET_MediaUltimasLecturas() As Double
			Dim LecturasAcumuladas As Double = 0
			Dim contador As Integer = 0
			For Each disp In _Dispositivos.FindAll(Function(X) X.conectado = True)
				LecturasAcumuladas += disp.lecturas.FindLast(Function(x) x.insertada = True).temperatura
				contador += 1
			Next
			Return LecturasAcumuladas / contador
		End Function
		Public Function recupera_Dispositivos() As List(Of Dispositivo)
			Dim pList As New List(Of Dispositivo)
			Dim query As String
			Dim dt As New DataTable
			query = $"USE [HomePi] SELECT [NOMBRE_DISP] ,[IP] ,[ROL] ,[LOCALIZACION] ,[MODELO] FROM [dbo].[DISPOSITIVOS] "
			Using conn As New SqlConnection(My.Settings.CONN)
				conn.Open()
				Using da As New SqlDataAdapter(query, conn)
					da.Fill(dt)
				End Using
			End Using
			For Each row As DataRow In dt.Rows
				Dim disp As New Dispositivo(row("NOMBRE_DISP"), row("IP"), row("ROL"), row("LOCALIZACION"), row("MODELO"))
				pList.Add(disp)
			Next
			Return pList
		End Function
		Private Sub reloadConfiguracion()
			GET_myConfiguracion()
		End Sub
		Private Sub GET_myConfiguracion()
			Dim query As String
			Dim dt As New DataTable
			query = $"USE [HomePi] SELECT top 1 [TEMPERATURA_DESTINO], [HISTERESIS] ,[USUARIO_CAMBIO] ,[FECHA_CAMBIO],[FRECUENCIA_MUESTREO] FROM [dbo].[CONFIGURACION]"
			Using conn As New SqlConnection(My.Settings.CONN)
				conn.Open()
				Using da As New SqlDataAdapter(query, conn)
					da.Fill(dt)
				End Using
			End Using
			For Each row As DataRow In dt.Rows
				TemperaturaObjetivo = row("TEMPERATURA_DESTINO")
				Histeresis = row("HISTERESIS")
				UsuarioCambio = row("USUARIO_CAMBIO")
				FechaCambio = row("FECHA_CAMBIO")
				FrecuenciaMuestreo = row("FRECUENCIA_MUESTREO")
			Next
		End Sub
#End Region
	End Class
End Class

