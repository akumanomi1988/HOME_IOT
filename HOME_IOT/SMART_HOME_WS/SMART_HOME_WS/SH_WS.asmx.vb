Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports System.Data.SqlClient
Imports Newtonsoft.Json
Imports System.Web.Script.Services

' Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente.

'<System.Web.Services.WebService(Namespace:="http://tempuri.org/")> _

'<WebService(Namespace:="http://tempuri.org/")>
'<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
'<System.Web.Script.Services.ScriptService()>

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
		dispo = _calefaccion.Dispositivos.Find(Function(x) x.Nombre = Nombre_Dispositivo)
		dispo.Conectado = True
		Return JsonConvert.SerializeObject(dispo.Conectado, Formatting.Indented)
	End Function
	<WebMethod()>
	<Script.Services.ScriptMethod(ResponseFormat:=ResponseFormat.Json, UseHttpGet:=True, XmlSerializeString:=True)>
	Public Function GET_TemperaturaObjetivo() As String
		Return JsonConvert.SerializeObject(_calefaccion.Configuracion.TemperaturaObjetivo, Formatting.Indented)
	End Function
	<WebMethod()>
	<Script.Services.ScriptMethod(ResponseFormat:=ResponseFormat.Json, UseHttpGet:=True, XmlSerializeString:=True)>
	Public Function SET_TemperaturaObjetivo(ByVal NuevaTemp As Double) As String
		_calefaccion.Configuracion.TemperaturaObjetivo = NuevaTemp
		Return JsonConvert.SerializeObject(_calefaccion.Configuracion.TemperaturaObjetivo, Formatting.Indented)
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
		dispo = _calefaccion.Dispositivos.Find(Function(x) x.Nombre = NOMBRE_DISP)
		dispo.Conectado = True
		dispo.Lecturas.Add(lectura)
		dispo.LecturaMediaEnTiempo(lectura, 5)
		Return lectura.INSERTADA
	End Function

	<WebMethod()>
	Public Function GET_Lecturas(ByVal Dispositivo As String, ByVal cantidad As Integer) As String
		_calefaccion.Dispositivos.Find(Function(x) x.Nombre = Dispositivo).LoadLecturas(cantidad)
		Return JsonConvert.SerializeObject(_calefaccion.Dispositivos.Find(Function(x) x.Nombre = Dispositivo).Lecturas.FindAll(Function(L) L.INSERTADA = True), Formatting.Indented)
	End Function
	<WebMethod()>
	Public Function GET_Lecturas_Fecha(ByVal NombreDispositivo As String, ByVal Fecha As Date) As String
		Return JsonConvert.SerializeObject(_calefaccion.Dispositivos.Find(Function(x) x.Nombre = NombreDispositivo).Lecturas.FindAll(Function(L) L.fecha_hora.Date = Fecha.Date), Formatting.Indented)
	End Function
	<WebMethod()>
	Public Function GET_Servidor_Fecha() As String
		Return DateTime.Now.ToString
	End Function
	<WebMethod()>
	Public Function GET_Dispositivos_Configuración(ByVal NombreDispositivo As String) As String
		Return JsonConvert.SerializeObject(_calefaccion.Dispositivos.Find(Function(x) x.Nombre = NombreDispositivo), Formatting.Indented)
	End Function
	<WebMethod()>
	Public Function NEW_Dispositivo(ByVal NOMBRE_DISP As String, ByVal IP As String, ByVal ROL As String, ByVal LOCALIZACION As String, ByVal MODELO As String) As Boolean
		If _calefaccion.Dispositivos.FindIndex(Function(x) x.Nombre = NOMBRE_DISP) >= 0 Then
			Return False
		Else
			Dim dispo As New Dispositivo(NOMBRE_DISP, IP, ROL, LOCALIZACION, MODELO)
			_calefaccion.Dispositivos.Add(dispo)
			Return dispo.Guardar()
		End If
	End Function
	Public Class Dispositivo
#Region "Propiedades"
		Private _Nombre As String
		Private _IP As String
		Private _Rol As String
		Private _Localizacion As String
		Private _Modelo As String
		Private _Lecturas As List(Of Lectura)
		Private _Conectado As Boolean
		Private _TiempoReconexion As Integer
		Public Property TiempoReconexion() As Integer
			Get
				Return _TiempoReconexion
			End Get
			Set(ByVal value As Integer)
				_TiempoReconexion = value
			End Set
		End Property
		Public Property Conectado() As Boolean
			Get
				Return _Conectado
			End Get
			Set(ByVal value As Boolean)
				If value Then
					TiempoReconexion = 10
				End If
				_Conectado = value
			End Set
		End Property
		Public Property Nombre() As String
			Get
				Return _Nombre
			End Get
			Set(ByVal value As String)
				_Nombre = value
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
		Public Property Rol() As String
			Get
				Return _Rol
			End Get
			Set(ByVal value As String)
				_Rol = value
			End Set
		End Property
		Public Property Localizacion() As String
			Get
				Return _Localizacion
			End Get
			Set(ByVal value As String)
				_Localizacion = value
			End Set
		End Property
		Public Property Modelo() As String
			Get
				Return _Modelo
			End Get
			Set(ByVal value As String)
				_Modelo = value
			End Set
		End Property
		Public Property Lecturas() As List(Of Lectura)
			Get
				Return _Lecturas
			End Get
			Set(ByVal value As List(Of Lectura))
				_Lecturas = value
			End Set
		End Property
#End Region
#Region "Constructor"
		Public Sub New(nombre As String, iP As String, rol As String, localizacion As String, modelo As String, Optional CantidadLecturasCargaInicial As Integer = 1)
			_Nombre = nombre
			_IP = iP
			_Rol = rol
			_Localizacion = localizacion
			_Modelo = modelo
			_Lecturas = New List(Of Lectura)
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
						.Parameters.AddWithValue("@NOMBRE_DISP", Nombre)
						.Parameters.AddWithValue("@IP", IP)
						.Parameters.AddWithValue("@ROL", Rol)
						.Parameters.AddWithValue("@LOCALIZACION", Localizacion)
						.Parameters.AddWithValue("@MODELO", Modelo)
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
						.Parameters.AddWithValue("@NOMBRE_DISP", Nombre)
					End With
					Using da As New SqlDataAdapter(com)
						da.Fill(dt)
					End Using
				End Using
			End Using
			Lecturas.RemoveAll(Function(x) x.INSERTADA = True)
			For Each row As DataRow In dt.Rows
				Try
					Me.Lecturas.Add(New Lectura(row("NOMBRE_DISP"), row("TEMPERATURA_ACTUAL"), row("HUMEDAD"), row("FECHA"), True))
				Catch
					Console.WriteLine("Error: Lectura ya existe en la lista")
				End Try
			Next
			Me.Lecturas = Me.Lecturas.OrderByDescending(Function(x) x.fecha_hora).ToList
		End Sub
		Public Function LecturaMediaEnTiempo(lectura As Lectura, minutos As Integer) As Lectura
			Dim tiempo As DateTime
			Dim lecTMP As Lectura
			tiempo = Now.AddMinutes(minutos * -1)
			Dim lec As Lectura = Lecturas.FindLast(Function(x) x.NOMBRE_DISP = lectura.NOMBRE_DISP And x.INSERTADA = True)
			If lec IsNot Nothing Then
				If tiempo > lec.fecha_hora Then
					Dim tmp_listaLecturas As List(Of Lectura) = Lecturas.FindAll(Function(x) x.fecha_hora > lec.fecha_hora)
					Dim temMedia As Double
					Dim hmedia As Double
					For i = 0 To tmp_listaLecturas.Count - 1
						temMedia += tmp_listaLecturas(i).TEMPERATURA
						hmedia += tmp_listaLecturas(i).HUMEDAD
					Next
					lecTMP = New Lectura(lectura.NOMBRE_DISP, temMedia / tmp_listaLecturas.Count, hmedia / tmp_listaLecturas.Count, Now)
					If lecTMP.Guardar() Then
						lecTMP.INSERTADA = True
						Lecturas.Add(lecTMP)
					End If
					Return lecTMP
				Else
					Return lectura
				End If
			Else
				If lectura.Guardar() Then
					lectura.INSERTADA = True
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
		Public Function ActualizarDatos() As Boolean
			Dim query As String
			Dim dt As New DataTable
			query = $"UPDATE CONFIGURACION set
					FECHA_CAMBIO = @FECHA_CAMBIO,
					HISTERESIS = @HISTERESIS,
					TEMPERATURA_DESTINO = @TEMPERATURA_DESTINO,
					USUARIO_CAMBIO = @USUARIO_CAMBIO"
			Using conn As New SqlConnection(My.Settings.CONN)
				Using comm As New SqlCommand()
					With comm
						.Connection = conn
						.CommandType = CommandType.Text
						.CommandText = query
						.Parameters.AddWithValue("@HISTERESIS", _Histeresis)
						.Parameters.AddWithValue("@FECHA_CAMBIO", DateTime.Now)
						.Parameters.AddWithValue("@TEMPERATURA_DESTINO", _TemperaturaObjetivo)
						.Parameters.AddWithValue("@USUARIO_CAMBIO", _UsuarioCambio)
					End With
					Try
						conn.Open()
						comm.ExecuteNonQuery()
						RaiseEvent Cambio()
					Catch ex As Exception
						Return False
					End Try
					Return True
				End Using
			End Using
		End Function
#End Region
	End Class
	Public Class Lectura
#Region "Propiedades"
		Private _NOMBRE_DISP As String
		Private _TEMPERATURA As Double
		Private _HUMEDAD As Double
		Private _fecha_hora As Date
		Private _INSERTADA As Boolean
		Public Property fecha_hora As DateTime
			Get
				Return _fecha_hora
			End Get
			Set
				_fecha_hora = Value
			End Set
		End Property
		Public Property NOMBRE_DISP As String
			Get
				Return _NOMBRE_DISP
			End Get
			Set
				_NOMBRE_DISP = Value
			End Set
		End Property
		Public Property TEMPERATURA As Double
			Get
				Return _TEMPERATURA
			End Get
			Set
				_TEMPERATURA = Value
			End Set
		End Property
		Public Property HUMEDAD As Double
			Get
				Return _HUMEDAD
			End Get
			Set
				_HUMEDAD = Value
			End Set
		End Property
		Public Property INSERTADA As Boolean
			Get
				Return _INSERTADA
			End Get
			Set
				_INSERTADA = Value
			End Set
		End Property
#End Region
#Region "Constructores"
		Public Sub New(pNOMBRE_DISP As String, pTEMPERATURA_ACTUAL As Double, pHUMEDAD As Double, pFECHA_HORA As DateTime)
			Me.NOMBRE_DISP = pNOMBRE_DISP
			Me.TEMPERATURA = pTEMPERATURA_ACTUAL
			Me.HUMEDAD = pHUMEDAD
			Me.fecha_hora = pFECHA_HORA
		End Sub
		Public Sub New(pNOMBRE_DISP As String, pTEMPERATURA_ACTUAL As Double, pHUMEDAD As Double, pFECHA_HORA As DateTime, ByVal pInsertada As Boolean)
			Me.NOMBRE_DISP = pNOMBRE_DISP
			Me.TEMPERATURA = pTEMPERATURA_ACTUAL
			Me.HUMEDAD = pHUMEDAD
			Me.fecha_hora = pFECHA_HORA
			Me.INSERTADA = pInsertada
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
						.Parameters.AddWithValue("@NOMBRE_DISP", _NOMBRE_DISP)
						.Parameters.AddWithValue("@TEMPERATURA_ACTUAL", _TEMPERATURA)
						.Parameters.AddWithValue("@HUMEDAD", _HUMEDAD)
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
#Region "Propiedades"

		Private _Encendido As Boolean
		Private _Dispositivos As List(Of Dispositivo)
		Private _Configuracion As Configuracion
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
		Public Property Configuracion() As Configuracion
			Get
				Return _Configuracion
			End Get
			Set(ByVal value As Configuracion)
				_Configuracion = value
			End Set
		End Property
#End Region
#Region "Constructores"
		Public Sub New()
			Me._Dispositivos = recupera_Dispositivos()
			Me._Configuracion = GET_myConfiguracion()
			AddHandler Me._Configuracion.Cambio, AddressOf Update_Configuracion
		End Sub

#End Region
#Region "Funciones"
		Private Sub Calefaccion_Estado()
			If _Configuracion.TemperaturaObjetivo - _Configuracion.Histeresis > GET_MediaUltimasLecturas() And _Encendido = False Then
				_Encendido = True
			End If
			If _Configuracion.TemperaturaObjetivo - _Configuracion.Histeresis < GET_MediaUltimasLecturas() And _Encendido = True Then
				_Encendido = False
			End If
		End Sub
		Private Function GET_MediaUltimasLecturas() As Double
			Dim LecturasAcumuladas As Double = 0
			Dim contador As Integer = 0
			For Each disp In _Dispositivos.FindAll(Function(X) X.Conectado = True)
				LecturasAcumuladas += disp.Lecturas.FindLast(Function(x) x.INSERTADA = True).TEMPERATURA
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
		Private Sub Update_Configuracion()
			_Configuracion = GET_myConfiguracion()
		End Sub
		Private Function GET_myConfiguracion() As Configuracion
			Dim pConfiguracion As New Configuracion
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
				pConfiguracion.TemperaturaObjetivo = row("TEMPERATURA_DESTINO")
				pConfiguracion.Histeresis = row("HISTERESIS")
				pConfiguracion.UsuarioCambio = row("USUARIO_CAMBIO")
				pConfiguracion.FechaCambio = row("FECHA_CAMBIO")
				pConfiguracion.FrecuenciaMuestreo = row("FRECUENCIA_MUESTREO")
			Next
			Return pConfiguracion
		End Function
#End Region
	End Class
End Class

