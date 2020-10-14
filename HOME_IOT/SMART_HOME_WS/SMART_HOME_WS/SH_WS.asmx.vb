Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports System.Data.SqlClient
Imports Newtonsoft.Json

' Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente.
' <System.Web.Script.Services.ScriptService()> _
'<System.Web.Services.WebService(Namespace:="http://tempuri.org/")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)>
<ToolboxItem(False)>
Public Class SH_WS
	Inherits System.Web.Services.WebService

	<WebMethod()>
	Public Function Lecturas_Insertar(ByVal NOMBRE_DISP As String, ByVal TEMPERATURA_ACTUAL As Double, ByVal TEMPERATURA_DESTINO As Double, ByVal HUMEDAD As Double, ByVal HISTERESIS As Double, ByVal ENCENDIDO As Boolean) As Boolean
		Dim query As String
		query = " USE [HomePi]
	INSERT INTO [dbo].[INSTALACION.LECTURAS]
	([NOMBRE_DISP] ,[TEMPERATURA_ACTUAL] ,[TEMPERATURA_DESTINO] ,[HUMEDAD] ,[HISTERESIS] ,[ENCENDIDO])
	VALUES
	(@NOMBRE_DISP,@TEMPERATURA_ACTUAL,@TEMPERATURA_DESTINO,@HUMEDAD,@HISTERESIS,@ENCENDIDO) "
		Using conn As New SqlConnection(My.Settings.CONN)
			Using comm As New SqlCommand()
				With comm
					.Connection = conn
					.CommandType = CommandType.Text
					.CommandText = query
					.Parameters.AddWithValue("@NOMBRE_DISP", NOMBRE_DISP)
					.Parameters.AddWithValue("@TEMPERATURA_ACTUAL", TEMPERATURA_ACTUAL)
					.Parameters.AddWithValue("@TEMPERATURA_DESTINO", TEMPERATURA_DESTINO)
					.Parameters.AddWithValue("@HUMEDAD", HUMEDAD)
					.Parameters.AddWithValue("@HISTERESIS", HISTERESIS)
					.Parameters.AddWithValue("@ENCENDIDO", ENCENDIDO)
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
	<WebMethod()>
	Public Function Lecturas_ultimas(ByVal cantidad As Integer) As String
		Dim query As String
		Dim ds As New DataSet
		If cantidad = 0 Then cantidad = 1
		query = $"USE [HomePi] SELECT TOP {cantidad} * FROM [dbo].[INSTALACION.LECTURAS] ORDER BY FECHA DESC"
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
			query = $"USE [HomePi] SELECT  * FROM [dbo].[INSTALACION.LECTURAS] WHERE FECHA BETWEEN CAST('{FechaIni}' AS DATETIME) AND CAST('{FechaFin}' AS DATETIME) "
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
		query = $"USE [HomePi] SELECT  * FROM [dbo].[INSTALACION.LECTURAS] WHERE  CAST(FECHA AS DATE) = CAST('{FechaIni}' AS DATE) "
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
		query = $"USE [HomePi] SELECT NOMBRE_DISP, IP, ROL, LOCALIZACION, MODELO FROM [INSTALACION.DISPOSITIVOS] WHERE NOMBRE_DISP = @NOMBRE_DISP"
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
		query = $"USE [HomePi] INSERT INTO [dbo].[INSTALACION.DISPOSITIVOS] 
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
End Class