﻿Imports System.IO
Imports System.Xml

Public Class GlobalSetting

    Public Shared NombreAplicacion As String = "Pedidos"
    Public Shared NombreBaseEnsemble As String = "ENSEMBLE"
    Public Shared NombreBasePrecios As String = "EMP008"
    Public Shared NombreBaseMaestros As String = "LuzAzulDB"

    Public Shared CUITEnsemble As String = "30712110429"

    'Configuracion del server web
    Public Shared DBConecctionString As String = "tcp:luzazul.cfm2g7bbnqws.us-east-2.rds.amazonaws.com,1433"
    Public Shared DBUsername As String = "admin"
    Public Shared DBPassword As String = "UpWAXosx(b"

    'Configuraciones para el envio de emails
    Public Shared SMTP As String = "smtp.gmail.com"
    Public Shared MailFromAddress As String = "contacto@luz-azul.com.ar"
    Public Shared MailPort As Integer = 587
    Public Shared MailUsername As String = "contacto@luz-azul.com.ar"
    Public Shared MailPassword As String = "luzazul499"
    Public Shared MailEnableSSL As Boolean = True

    ' Miscelaneas
    Public Shared TituloMensajes As String = "Luz Azul"

    Public Shared NombreAppPedidos As String = "Luz Azul Pedidos.exe"
    Public Shared NombreAppConfiguracion As String = "Luz Azul Configuracion.exe"
    Public Shared NombreAppEmisionDocumentos As String = "Luz Azul Emision Doc.exe"
    Public Shared NombreAppRecepcionPedidos As String = "Luz Azul Recepcion Pedidos.exe"
    Public Shared NombreAppCompras As String = "Luz Azul Compras.exe"

End Class
