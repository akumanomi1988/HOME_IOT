﻿'------------------------------------------------------------------------------
' <auto-generated>
'     Este código fue generado por una herramienta.
'     Versión de runtime:4.0.30319.42000
'
'     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
'     se vuelve a generar el código.
' </auto-generated>
'------------------------------------------------------------------------------

Option Strict On
Option Explicit On


Namespace SH_WS_Pruebas
    
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0"),  _
     System.ServiceModel.ServiceContractAttribute(ConfigurationName:="SH_WS_Pruebas.SH_WSSoap")>  _
    Public Interface SH_WSSoap
        
        <System.ServiceModel.OperationContractAttribute(Action:="http://tempuri.org/Lecturas_Insertar", ReplyAction:="*"),  _
         System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults:=true)>  _
        Function Lecturas_Insertar(ByVal NOMBRE_DISP As String, ByVal TEMPERATURA_ACTUAL As Double, ByVal HUMEDAD As Double) As Integer
        
        <System.ServiceModel.OperationContractAttribute(Action:="http://tempuri.org/Lecturas_Insertar", ReplyAction:="*")>  _
        Function Lecturas_InsertarAsync(ByVal NOMBRE_DISP As String, ByVal TEMPERATURA_ACTUAL As Double, ByVal HUMEDAD As Double) As System.Threading.Tasks.Task(Of Integer)
        
        <System.ServiceModel.OperationContractAttribute(Action:="http://tempuri.org/Lecturas_ultimas", ReplyAction:="*"),  _
         System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults:=true)>  _
        Function Lecturas_ultimas(ByVal cantidad As Integer) As String
        
        <System.ServiceModel.OperationContractAttribute(Action:="http://tempuri.org/Lecturas_ultimas", ReplyAction:="*")>  _
        Function Lecturas_ultimasAsync(ByVal cantidad As Integer) As System.Threading.Tasks.Task(Of String)
        
        <System.ServiceModel.OperationContractAttribute(Action:="http://tempuri.org/Lecturas_Entre_Fechas", ReplyAction:="*"),  _
         System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults:=true)>  _
        Function Lecturas_Entre_Fechas(ByVal FechaIni As Date, ByVal FechaFin As Date) As String
        
        <System.ServiceModel.OperationContractAttribute(Action:="http://tempuri.org/Lecturas_Entre_Fechas", ReplyAction:="*")>  _
        Function Lecturas_Entre_FechasAsync(ByVal FechaIni As Date, ByVal FechaFin As Date) As System.Threading.Tasks.Task(Of String)
        
        <System.ServiceModel.OperationContractAttribute(Action:="http://tempuri.org/Lecturas_Fecha", ReplyAction:="*"),  _
         System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults:=true)>  _
        Function Lecturas_Fecha(ByVal FechaIni As Date) As String
        
        <System.ServiceModel.OperationContractAttribute(Action:="http://tempuri.org/Lecturas_Fecha", ReplyAction:="*")>  _
        Function Lecturas_FechaAsync(ByVal FechaIni As Date) As System.Threading.Tasks.Task(Of String)
        
        <System.ServiceModel.OperationContractAttribute(Action:="http://tempuri.org/Servidor_Fecha", ReplyAction:="*"),  _
         System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults:=true)>  _
        Function Servidor_Fecha() As String
        
        <System.ServiceModel.OperationContractAttribute(Action:="http://tempuri.org/Servidor_Fecha", ReplyAction:="*")>  _
        Function Servidor_FechaAsync() As System.Threading.Tasks.Task(Of String)
        
        <System.ServiceModel.OperationContractAttribute(Action:="http://tempuri.org/Dispositivos_Configuración", ReplyAction:="*"),  _
         System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults:=true)>  _
        Function Dispositivos_Configuración(ByVal NombreDispositivo As String) As String
        
        <System.ServiceModel.OperationContractAttribute(Action:="http://tempuri.org/Dispositivos_Configuración", ReplyAction:="*")>  _
        Function Dispositivos_ConfiguraciónAsync(ByVal NombreDispositivo As String) As System.Threading.Tasks.Task(Of String)
        
        <System.ServiceModel.OperationContractAttribute(Action:="http://tempuri.org/Dispositivos_Nuevo", ReplyAction:="*"),  _
         System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults:=true)>  _
        Function Dispositivos_Nuevo(ByVal NOMBRE_DISP As String, ByVal IP As String, ByVal ROL As String, ByVal LOCALIZACION As String, ByVal MODELO As String, ByVal USUARIO As String, ByVal PWD As String) As Boolean
        
        <System.ServiceModel.OperationContractAttribute(Action:="http://tempuri.org/Dispositivos_Nuevo", ReplyAction:="*")>  _
        Function Dispositivos_NuevoAsync(ByVal NOMBRE_DISP As String, ByVal IP As String, ByVal ROL As String, ByVal LOCALIZACION As String, ByVal MODELO As String, ByVal USUARIO As String, ByVal PWD As String) As System.Threading.Tasks.Task(Of Boolean)
    End Interface
    
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")>  _
    Public Interface SH_WSSoapChannel
        Inherits SH_WS_Pruebas.SH_WSSoap, System.ServiceModel.IClientChannel
    End Interface
    
    <System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")>  _
    Partial Public Class SH_WSSoapClient
        Inherits System.ServiceModel.ClientBase(Of SH_WS_Pruebas.SH_WSSoap)
        Implements SH_WS_Pruebas.SH_WSSoap
        
        Public Sub New()
            MyBase.New
        End Sub
        
        Public Sub New(ByVal endpointConfigurationName As String)
            MyBase.New(endpointConfigurationName)
        End Sub
        
        Public Sub New(ByVal endpointConfigurationName As String, ByVal remoteAddress As String)
            MyBase.New(endpointConfigurationName, remoteAddress)
        End Sub
        
        Public Sub New(ByVal endpointConfigurationName As String, ByVal remoteAddress As System.ServiceModel.EndpointAddress)
            MyBase.New(endpointConfigurationName, remoteAddress)
        End Sub
        
        Public Sub New(ByVal binding As System.ServiceModel.Channels.Binding, ByVal remoteAddress As System.ServiceModel.EndpointAddress)
            MyBase.New(binding, remoteAddress)
        End Sub
        
        Public Function Lecturas_Insertar(ByVal NOMBRE_DISP As String, ByVal TEMPERATURA_ACTUAL As Double, ByVal HUMEDAD As Double) As Integer Implements SH_WS_Pruebas.SH_WSSoap.Lecturas_Insertar
            Return MyBase.Channel.Lecturas_Insertar(NOMBRE_DISP, TEMPERATURA_ACTUAL, HUMEDAD)
        End Function
        
        Public Function Lecturas_InsertarAsync(ByVal NOMBRE_DISP As String, ByVal TEMPERATURA_ACTUAL As Double, ByVal HUMEDAD As Double) As System.Threading.Tasks.Task(Of Integer) Implements SH_WS_Pruebas.SH_WSSoap.Lecturas_InsertarAsync
            Return MyBase.Channel.Lecturas_InsertarAsync(NOMBRE_DISP, TEMPERATURA_ACTUAL, HUMEDAD)
        End Function
        
        Public Function Lecturas_ultimas(ByVal cantidad As Integer) As String Implements SH_WS_Pruebas.SH_WSSoap.Lecturas_ultimas
            Return MyBase.Channel.Lecturas_ultimas(cantidad)
        End Function
        
        Public Function Lecturas_ultimasAsync(ByVal cantidad As Integer) As System.Threading.Tasks.Task(Of String) Implements SH_WS_Pruebas.SH_WSSoap.Lecturas_ultimasAsync
            Return MyBase.Channel.Lecturas_ultimasAsync(cantidad)
        End Function
        
        Public Function Lecturas_Entre_Fechas(ByVal FechaIni As Date, ByVal FechaFin As Date) As String Implements SH_WS_Pruebas.SH_WSSoap.Lecturas_Entre_Fechas
            Return MyBase.Channel.Lecturas_Entre_Fechas(FechaIni, FechaFin)
        End Function
        
        Public Function Lecturas_Entre_FechasAsync(ByVal FechaIni As Date, ByVal FechaFin As Date) As System.Threading.Tasks.Task(Of String) Implements SH_WS_Pruebas.SH_WSSoap.Lecturas_Entre_FechasAsync
            Return MyBase.Channel.Lecturas_Entre_FechasAsync(FechaIni, FechaFin)
        End Function
        
        Public Function Lecturas_Fecha(ByVal FechaIni As Date) As String Implements SH_WS_Pruebas.SH_WSSoap.Lecturas_Fecha
            Return MyBase.Channel.Lecturas_Fecha(FechaIni)
        End Function
        
        Public Function Lecturas_FechaAsync(ByVal FechaIni As Date) As System.Threading.Tasks.Task(Of String) Implements SH_WS_Pruebas.SH_WSSoap.Lecturas_FechaAsync
            Return MyBase.Channel.Lecturas_FechaAsync(FechaIni)
        End Function
        
        Public Function Servidor_Fecha() As String Implements SH_WS_Pruebas.SH_WSSoap.Servidor_Fecha
            Return MyBase.Channel.Servidor_Fecha
        End Function
        
        Public Function Servidor_FechaAsync() As System.Threading.Tasks.Task(Of String) Implements SH_WS_Pruebas.SH_WSSoap.Servidor_FechaAsync
            Return MyBase.Channel.Servidor_FechaAsync
        End Function
        
        Public Function Dispositivos_Configuración(ByVal NombreDispositivo As String) As String Implements SH_WS_Pruebas.SH_WSSoap.Dispositivos_Configuración
            Return MyBase.Channel.Dispositivos_Configuración(NombreDispositivo)
        End Function
        
        Public Function Dispositivos_ConfiguraciónAsync(ByVal NombreDispositivo As String) As System.Threading.Tasks.Task(Of String) Implements SH_WS_Pruebas.SH_WSSoap.Dispositivos_ConfiguraciónAsync
            Return MyBase.Channel.Dispositivos_ConfiguraciónAsync(NombreDispositivo)
        End Function
        
        Public Function Dispositivos_Nuevo(ByVal NOMBRE_DISP As String, ByVal IP As String, ByVal ROL As String, ByVal LOCALIZACION As String, ByVal MODELO As String, ByVal USUARIO As String, ByVal PWD As String) As Boolean Implements SH_WS_Pruebas.SH_WSSoap.Dispositivos_Nuevo
            Return MyBase.Channel.Dispositivos_Nuevo(NOMBRE_DISP, IP, ROL, LOCALIZACION, MODELO, USUARIO, PWD)
        End Function
        
        Public Function Dispositivos_NuevoAsync(ByVal NOMBRE_DISP As String, ByVal IP As String, ByVal ROL As String, ByVal LOCALIZACION As String, ByVal MODELO As String, ByVal USUARIO As String, ByVal PWD As String) As System.Threading.Tasks.Task(Of Boolean) Implements SH_WS_Pruebas.SH_WSSoap.Dispositivos_NuevoAsync
            Return MyBase.Channel.Dispositivos_NuevoAsync(NOMBRE_DISP, IP, ROL, LOCALIZACION, MODELO, USUARIO, PWD)
        End Function
    End Class
End Namespace