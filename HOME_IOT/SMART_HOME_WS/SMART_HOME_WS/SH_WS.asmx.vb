Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports System.Data.SqlClient
Imports Newtonsoft.Json
Imports System.Web.Script.Services

' Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente.

'<System.Web.Services.WebService(Namespace:="http://tempuri.org/")> _

'<WebService(Namespace:="http://tempuri.org/")>
<System.Web.Script.Services.ScriptService()>
<WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)>
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Public Class SH_WS
	Inherits System.Web.Services.WebService
	Shared _calefaccion As Calefaccion
	Shared _listaLecturas As List(Of Lectura)

	Public Sub New()
		If _calefaccion Is Nothing Then _calefaccion = New Calefaccion
		If _listaLecturas Is Nothing Then _listaLecturas = New List(Of Lectura)
	End Sub
	<WebMethod(CacheDuration:=0)>
	Public Function GETCalefaccion() As String
		Return JsonConvert.SerializeObject(_calefaccion)
	End Function
	<WebMethod(CacheDuration:=0)>
	<Script.Services.ScriptMethod(ResponseFormat:=ResponseFormat.Json, UseHttpGet:=True, XmlSerializeString:=False)>
	Public Function Calefaccion_Estado(ByVal Temperatura As String) As String
		Dim query As String
		Dim ds As New DataTable
		query = $"USE [HomePi] SELECT TOP 1 
	  case when  [TEMPERATURA_DESTINO] - [HISTERESIS] > @tempActual then 1 else 0 end as Encendido
	  ,case when [TEMPERATURA_DESTINO] + [HISTERESIS] < @tempActual then 1 else 0 end as Apagado
		FROM [HomePi].[dbo].[CONFIGURACION]"
		Using conn As New SqlConnection(My.Settings.CONN)
			conn.Open()
			Using comm As New SqlCommand()
				With comm
					.Connection = conn
					.CommandType = CommandType.Text
					.CommandText = query
					.Parameters.AddWithValue("@tempActual", Temperatura)
				End With
				Using da As New SqlDataAdapter(comm)
					da.Fill(ds)
				End Using
			End Using
		End Using
		For Each row As DataRow In ds.Rows
			If row("Encendido") = 1 And _calefaccion.EstadoEncendido = False Then
				_calefaccion.EstadoEncendido = True
			End If
			If row("Apagado") = 1 And _calefaccion.EstadoEncendido = True Then
				_calefaccion.EstadoEncendido = False
			End If
		Next
		Return JsonConvert.SerializeObject(_calefaccion)
	End Function
	<WebMethod()>
	Public Function Lecturas_Insertar(ByVal NOMBRE_DISP As String, ByVal TEMPERATURA_ACTUAL As Double, ByVal HUMEDAD As Double) As Integer
		Dim lectura As New Lectura(NOMBRE_DISP, TEMPERATURA_ACTUAL, HUMEDAD, DateTime.Now)
		_listaLecturas.Add(lectura)
		lectura = LecturaMediaEnTiempo(lectura, 20)
		If lectura.INSERTADA Then
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
						.Parameters.AddWithValue("@NOMBRE_DISP", lectura.NOMBRE_DISP)
						.Parameters.AddWithValue("@TEMPERATURA_ACTUAL", lectura.TEMPERATURA_ACTUAL)
						.Parameters.AddWithValue("@HUMEDAD", lectura.HUMEDAD)
					End With
					Try
						conn.Open()
						comm.ExecuteNonQuery()
					Catch ex As Exception
						Return -1
					End Try
				End Using
			End Using
			Return 1
		Else
			Return 0
		End If
	End Function
	Private Function LecturaMediaEnTiempo(lectura As Lectura, minutos As Integer) As Lectura
		Dim tiempo As DateTime
		tiempo = Now.AddMinutes(minutos * -1)
		Dim lec As Lectura = _listaLecturas.FindLast(Function(x) x.NOMBRE_DISP = lectura.NOMBRE_DISP And x.INSERTADA = True)
		If lec IsNot Nothing Then
			If tiempo > lec.fecha_hora Then
				Dim tmp_listaLecturas As List(Of Lectura) = _listaLecturas.FindAll(Function(x) x.fecha_hora > lec.fecha_hora)
				Dim temMedia As Double
				Dim hmedia As Double
				For i = 0 To tmp_listaLecturas.Count - 1
					temMedia += tmp_listaLecturas(i).TEMPERATURA_ACTUAL
					hmedia += tmp_listaLecturas(i).HUMEDAD
				Next
				lectura = New Lectura(lectura.NOMBRE_DISP, temMedia / tmp_listaLecturas.Count, hmedia / tmp_listaLecturas.Count, Now)
				lectura.INSERTADA = True
				Return lectura
			Else
				Return lectura
			End If
		Else
			lectura.INSERTADA = True
			_listaLecturas.Add(lectura)
			Return lectura
		End If
	End Function

	<WebMethod()>
	Public Function Lecturas_ultimas(ByVal cantidad As Integer) As String
		Dim query As String
		Dim ds As New DataSet
		If cantidad = 0 Then cantidad = 1
		query = $"USE [HomePi] SELECT TOP {cantidad} * FROM [dbo].[LECTURAS] ORDER BY FECHA DESC"
		Using conn As New SqlConnection(My.Settings.CONN)
			conn.Open()
			Using da As New SqlDataAdapter(query, conn)
				da.Fill(ds)
			End Using
		End Using
		Return JsonConvert.SerializeObject(ds, Formatting.Indented)
	End Function
	<WebMethod()>
	Public Function Lecturas_Entre_Fechas(ByVal FechaIni As DateTime, ByVal FechaFin As DateTime) As String
		Dim query As String
		Dim ds As New DataSet
		If FechaIni >= FechaFin Then
			Return Nothing
		Else
			query = $"USE [HomePi] SELECT  * FROM [dbo].[LECTURAS] WHERE FECHA BETWEEN CAST('{FechaIni}' AS DATETIME) AND CAST('{FechaFin}' AS DATETIME) "
			Using conn As New SqlConnection(My.Settings.CONN)
				conn.Open()
				Using da As New SqlDataAdapter(query, conn)
					da.Fill(ds)
				End Using
			End Using
			Return JsonConvert.SerializeObject(ds, Formatting.Indented)
		End If
	End Function
	<WebMethod()>
	Public Function Lecturas_Fecha(ByVal FechaIni As Date) As String
		Dim query As String
		Dim ds As New DataSet
		query = $"USE [HomePi] SELECT  * FROM [dbo].[LECTURAS] WHERE  CAST(FECHA AS DATE) = CAST('{FechaIni}' AS DATE) "
		Using conn As New SqlConnection(My.Settings.CONN)
			conn.Open()
			Using da As New SqlDataAdapter(query, conn)
				da.Fill(ds)
			End Using
		End Using
		Return JsonConvert.SerializeObject(ds, Formatting.Indented)
	End Function
	<WebMethod()>
	Public Function Servidor_Fecha() As String
		Return DateTime.Now.ToString
	End Function
	<WebMethod()>
	Public Function Dispositivos_Configuración(ByVal NombreDispositivo As String) As String
		Dim query As String
		Dim ds As New DataSet
		query = $"USE [HomePi] SELECT NOMBRE_DISP, IP, ROL, LOCALIZACION, MODELO FROM [DISPOSITIVOS] WHERE NOMBRE_DISP = @NOMBRE_DISP"
		Using conn As New SqlConnection(My.Settings.CONN)
			conn.Open()
			Using comm As New SqlCommand()
				With comm
					.Connection = conn
					.CommandType = CommandType.Text
					.CommandText = query
					.Parameters.AddWithValue("@NOMBRE_DISP", NombreDispositivo)
				End With
				Using da As New SqlDataAdapter(comm)
					da.Fill(ds)
				End Using
			End Using
		End Using

		Return JsonConvert.SerializeObject(ds, Formatting.Indented)
	End Function
	<WebMethod()>
	Public Function Dispositivos_Nuevo(ByVal NOMBRE_DISP As String, ByVal IP As String, ByVal ROL As String, ByVal LOCALIZACION As String, ByVal MODELO As String, ByVal USUARIO As String, ByVal PWD As String) As Boolean
		Dim query As String
		Dim ds As New DataSet
		query = $"USE [HomePi] INSERT INTO [dbo].[DISPOSITIVOS] 
			([NOMBRE_DISP] ,[IP] ,[ROL] ,[LOCALIZACION] ,[MODELO] ,[USUARIO] ,[PWD]) VALUES(@NOMBRE_DISP,@IP,@ROL,@LOCALIZACION,@MODELO,@USUARIO,@PWD)"
		Using conn As New SqlConnection(My.Settings.CONN)
			conn.Open()
			Using comm As New SqlCommand()
				With comm
					.Connection = conn
					.CommandType = CommandType.Text
					.CommandText = query
					.Parameters.AddWithValue("@NOMBRE_DISP", NOMBRE_DISP)
					.Parameters.AddWithValue("@IP", IP)
					.Parameters.AddWithValue("@ROL", ROL)
					.Parameters.AddWithValue("@LOCALIZACION", LOCALIZACION)
					.Parameters.AddWithValue("@MODELO", MODELO)
					.Parameters.AddWithValue("@USUARIO", USUARIO)
					.Parameters.AddWithValue("@PWD", PWD)
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

		Return JsonConvert.SerializeObject(ds, Formatting.Indented)
	End Function
	Private Class Dispositivo
		Private _Nombre As String
		Public Property Nombre() As String
			Get
				Return _Nombre
			End Get
			Set(ByVal value As String)
				_Nombre = value
			End Set
		End Property
		Private _IP As String
		Public Property IP() As String
			Get
				Return _IP
			End Get
			Set(ByVal value As String)
				_IP = value
			End Set
		End Property
		Private _Rol As String
		Public Property Rol() As String
			Get
				Return _Rol
			End Get
			Set(ByVal value As String)
				_Rol = value
			End Set
		End Property
		Private _Localizacion As String
		Public Property Localizacion() As String
			Get
				Return _Localizacion
			End Get
			Set(ByVal value As String)
				_Localizacion = value
			End Set
		End Property
		Private _Modelo As String
		Public Property Modelo() As String
			Get
				Return _Modelo
			End Get
			Set(ByVal value As String)
				_Modelo = value
			End Set
		End Property
		Public Sub New(nombre As String, iP As String, rol As String, localizacion As String, modelo As String)
			_Nombre = nombre
			_IP = iP
			_Rol = rol
			_Localizacion = localizacion
			_Modelo = modelo
		End Sub
	End Class
	Private Class Configuracion
		Private _TemperaturaDestino As Double
		Public Property TemperaturaDestino() As Double
			Get
				Return _TemperaturaDestino
			End Get
			Set
				_TemperaturaDestino = Value
			End Set
		End Property

		Private _Histeresis As Double
		Public Property Histeresis() As Double
			Get
				Return _Histeresis
			End Get
			Set(ByVal value As Double)
				_Histeresis = value
			End Set
		End Property
		Private _UsuarioCambio As String
		Public Property UsuarioCambio() As String
			Get
				Return _UsuarioCambio
			End Get
			Set(ByVal value As String)
				_UsuarioCambio = value
			End Set
		End Property
		Private _FechaCambio As DateTime


		Public Property FechaCambio() As DateTime
			Get
				Return _FechaCambio
			End Get
			Set(ByVal value As DateTime)
				_FechaCambio = value
			End Set
		End Property
		Public Sub New(temperaturaDestino As Double, histeresis As Double, usuarioCambio As String, fechaCambio As Date)
			_TemperaturaDestino = temperaturaDestino
			_Histeresis = histeresis
			_UsuarioCambio = usuarioCambio
			_FechaCambio = fechaCambio
		End Sub
		Public Sub New()

		End Sub

	End Class
	Private Class Lectura
		Private _NOMBRE_DISP As String
		Private _TEMPERATURA_ACTUAL As Double
		Private _HUMEDAD As Double
		Private _fecha_hora As Date
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

		Public Property TEMPERATURA_ACTUAL As Double
			Get
				Return _TEMPERATURA_ACTUAL
			End Get
			Set
				_TEMPERATURA_ACTUAL = Value
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
		Public Sub New(nOMBRE_DISP As String, tEMPERATURA_ACTUAL As Double, hUMEDAD As Double, fECHA_HORA As DateTime)
			If nOMBRE_DISP Is Nothing Then
				Throw New ArgumentNullException(NameOf(nOMBRE_DISP))
			End If

			Me.NOMBRE_DISP = nOMBRE_DISP
			Me.TEMPERATURA_ACTUAL = tEMPERATURA_ACTUAL
			Me.HUMEDAD = hUMEDAD
			Me.fecha_hora = fECHA_HORA
		End Sub
		Private Property _INSERTADA As Boolean
		Public Property INSERTADA As Boolean
			Get
				Return _INSERTADA
			End Get
			Set
				_INSERTADA = Value
			End Set
		End Property
	End Class
	Private Class Calefaccion
		Private _EstadoEncendido As Boolean
		Public Property EstadoEncendido() As Boolean
			Get
				Return _EstadoEncendido
			End Get
			Set(ByVal value As Boolean)
				_EstadoEncendido = value
			End Set
		End Property

		Private _dispositivos As List(Of Dispositivo)
		Private _configuracion As Configuracion
		Public Sub New()
			_dispositivos = recupera_Dispositivos()
			_configuracion = recupera_Configuracion()
		End Sub
		Private Function recupera_Dispositivos() As List(Of Dispositivo)
			Dim pList As New List(Of Dispositivo)
			Dim query As String
			Dim ds As New DataTable
			query = $"USE [HomePi] SELECT [NOMBRE_DISP] ,[IP] ,[ROL] ,[LOCALIZACION] ,[MODELO] FROM [dbo].[DISPOSITIVOS] "
			Using conn As New SqlConnection(My.Settings.CONN)
				conn.Open()
				Using da As New SqlDataAdapter(query, conn)
					da.Fill(ds)
				End Using
			End Using
			For Each row As DataRow In ds.Rows
				Dim disp As New Dispositivo(row("NOMBRE_DISP"), row("IP"), row("ROL"), row("LOCALIZACION"), row("MODELO"))
				pList.Add(disp)
			Next
			Return pList
		End Function
		Private Function recupera_Configuracion() As Configuracion
			Dim pConfiguracion As New Configuracion
			Dim query As String
			Dim ds As New DataTable
			query = $"USE [HomePi] SELECT top 1 [TEMPERATURA_DESTINO], [HISTERESIS] ,[USUARIO_CAMBIO] ,[FECHA_CAMBIO] FROM [dbo].[CONFIGURACION]"
			Using conn As New SqlConnection(My.Settings.CONN)
				conn.Open()
				Using da As New SqlDataAdapter(query, conn)
					da.Fill(ds)
				End Using
			End Using
			For Each row As DataRow In ds.Rows
				pConfiguracion.TemperaturaDestino = row("TEMPERATURA_DESTINO")
				pConfiguracion.Histeresis = row("HISTERESIS")
				pConfiguracion.UsuarioCambio = row("USUARIO_CAMBIO")
				pConfiguracion.FechaCambio = row("FECHA_CAMBIO")
			Next
			'pConfiguracion = New Configuracion()
			Return pConfiguracion
		End Function
	End Class

End Class

