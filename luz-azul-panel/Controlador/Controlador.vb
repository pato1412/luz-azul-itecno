﻿Imports LAFunctions.LuzAzulCommon
Imports System.IO
Imports System.Xml

Public Class Controlador
    Inherits LAFunctions.ControladorCommon

    Private Shared fachadaBD As LAFunctions.LuzAzulCommon
    Private Shared instancia As Controlador

    Private Shared MostrarFrameDepositos As Boolean = False

    'configuraciones de los establecimientos  
    Private Shared ListEstablecimientos As New List(Of Establecimiento)
    Private Shared ListEstablecimientosSeleccionados As New List(Of Establecimiento)
    Private Shared ListDepositos As New List(Of Deposito)
    Private Shared MostrarTodosDepositos As Boolean = False
    Private Sub New()

        'Busco en la base los ids de los centros de distribucion
        LoadCentrosDistribucion()


    End Sub

    Public Shared Function GetInstancia() As Controlador
        If (instancia Is Nothing) Then
            fachadaBD = New LAFunctions.LuzAzulCommon With {.NombreBaseEnsemble = LAFunctions.GlobalSetting.NombreBaseEnsemble}
            instancia = New Controlador()
        End If

        Return instancia
    End Function
    Public Shared Sub SetMostrarFrameDepositos(mostrar As Boolean)
        MostrarFrameDepositos = mostrar
    End Sub
    Public Shared Function GetMostrarFrameDepositos() As Boolean
        Return MostrarFrameDepositos
    End Function

    Public Shared Sub SetMostrarTodosDepositos(mostrar As Boolean)
        MostrarTodosDepositos = mostrar
    End Sub
    Public Shared Function GetMostrarTodosDepositos() As Boolean
        Return MostrarTodosDepositos
    End Function
    Public Shared Sub SetCurrentEstablecimiento(ByVal EstableciminetoId As String)
        Dim pos As Integer = ListEstablecimientos.FindIndex(Function(element) element.EstablecimientoId = EstableciminetoId)
        If pos >= 0 Then
            CurrentEstablecimiento = ListEstablecimientos(pos)
        End If
    End Sub

    Public Shared Function GetNombreBaseDB() As String
        Return fachadaBD.NombreBase
    End Function

    Public Shared Function Query() As List(Of String)
        Return fachadaBD.Query
    End Function
    Public Sub SetDatabaseName(ByVal DBName As String)
        fachadaBD.SetNombreBase(DBName)
    End Sub
    Public Function DoLogin(ByVal Usuario As String, ByVal Clave As String) As ResponseLogin
        Dim respuesta As New ResponseLogin
        If CurrentEstablecimiento.DbName <> "" Then
            respuesta = fachadaBD.DoLogin(Usuario, Clave, CurrentEstablecimiento.DbName)
            If (respuesta.PermiteLogin) Then
                UsuarioId = respuesta.mensaje 'usuario logueado
                EsAdministrador = respuesta.EsAdministrador

                'Lo primero que hago es Setear la Base de datos de la FRQ
                SetDatabaseName(CurrentEstablecimiento.DbName)

                'verifico el cuit de la empresa y determino si es fabrica
                LogEvent.Add("VerificarFabrica()")
                Dim respuestaEmpresa As ResponseCuit = fachadaBD.GetCUITEmpresa()
                If respuestaEmpresa.ConsultaExitosa Then
                    CUIT = respuestaEmpresa.CUIT
                    EsFabrica = (respuestaEmpresa.CUIT = LAFunctions.GlobalSetting.CUITEnsemble)
                Else
                    WriteLogFile("La funcion verificar fabrica ha dado error")
                    MsgBox(respuestaEmpresa.mensaje, vbExclamation + vbOKOnly, LAFunctions.GlobalSetting.TituloMensajes)
                End If

                ' Obtengo las configuraciones de envio de mails
                LogEvent.Add("LeerEntidadesMailings()")
                Dim respuestaEntidades As ResponseEntidades = fachadaBD.LeerEntidadesMailings(CurrentEstablecimiento.EstablecimientoId)
                If respuestaEntidades.ConsultaExitosa = True Then
                    'si la configuraciones estan cargadas sobreescribo las que vienen por default
                    LAFunctions.GlobalSetting.SMTP = respuestaEntidades.SMTPServer
                    LAFunctions.GlobalSetting.MailPort = respuestaEntidades.Puerto
                    LAFunctions.GlobalSetting.MailFromAddress = respuestaEntidades.Email
                    LAFunctions.GlobalSetting.MailUsername = respuestaEntidades.UID
                    LAFunctions.GlobalSetting.MailPassword = respuestaEntidades.PWD
                    LAFunctions.GlobalSetting.MailEnableSSL = respuestaEntidades.UsaSSL
                End If

                LogEvent.Add("GetRazonSocial()")
                Dim RespuestaRazonSocial As LAFunctions.LuzAzulCommon.ResponseRazonSocial = fachadaBD.GetRazonSocial()
                If RespuestaRazonSocial.ConsultaExitosa = True Then
                    RazonSocial = RespuestaRazonSocial.RazonSocial
                Else
                    WriteLogFile("La funcion Get Razon social ha dado error")
                    MsgBox(respuesta.mensaje, vbExclamation + vbOKOnly, LAFunctions.GlobalSetting.TituloMensajes)
                End If

                LogEvent.Add("Usuario Logueado")
            End If
        Else
            WriteLogFile("El establecimento seleccionado no ha sido configurado correctamente y no tiene una base de datos asociada")
            MsgBox("El establecimento seleccionado no ha sido configurado", vbExclamation + vbOKOnly, LAFunctions.GlobalSetting.TituloMensajes)
        End If
        Return respuesta
    End Function

    Public Function GetAllEstablecimientos() As ResponseEstablecimiento
        Dim respuestaEstablecimiento As ResponseEstablecimiento

        respuestaEstablecimiento = fachadaBD.GetAllEstablecimientos()
        If (respuestaEstablecimiento.ConsultaExitosa) Then
            ListEstablecimientos = respuestaEstablecimiento.rs
            LogEvent.Add("El usuario tiene " + respuestaEstablecimiento.rs.Count.ToString() + " establecimientos")
        Else
            'Ocurrio un error al obtener el o los establecimientos
            WriteLogFile("Ocurrio un error al obtener el o los establecimientos")
        End If

        Return respuestaEstablecimiento
    End Function

    Public Function GetDepositosUsuario() As ResponseDeposito
        Dim respuestaDepositos As ResponseDeposito

        respuestaDepositos = fachadaBD.GetDepositosUsuario(UsuarioId, CurrentEstablecimiento.SucursalId)

        If (respuestaDepositos.ConsultaExitosa = False) Then
            ' Si ocurrio un error con el listado de depositos lo muestro en pantalla
            WriteLogFile("Ocurrio un error con los depositos del usuario")
            MsgBox(respuestaDepositos.mensaje, vbExclamation + vbOKOnly, LAFunctions.GlobalSetting.TituloMensajes)
        Else
            ListDepositos = respuestaDepositos.rs
        End If
        Return respuestaDepositos
    End Function

    Public Shared Sub WriteLogFile(strError As String)

        Dim myFilePath As String = My.Application.Info.DirectoryPath + "\error_log_main " + Today.Day.ToString() + "." + Today.Month.ToString() + "." + Today.Year.ToString() + " " + Now.Hour.ToString() + "." + Now.Minute.ToString() + ".log"

        Using sw As New StreamWriter(File.Open(myFilePath, FileMode.Append))
            sw.WriteLine("--------------- Archivo generado automaticamente MDI-FORM " + DateTime.Now + "-----------------------")
            sw.WriteLine("")

            'Escribo la cadena de eventos que se ejecutaron
            sw.WriteLine("--------------- Events -------------------------")
            For Each StrEvent In LogEvent
                sw.WriteLine(StrEvent)
            Next
            sw.WriteLine("")

            'Escribo la cadena de queries que se ejecutaron
            sw.WriteLine("--------------- Queries ------------------------")
            For Each StrQuery In fachadaBD.Query
                sw.WriteLine(StrQuery)
            Next
            sw.WriteLine("")

            'Escribo todos las variables globales para poder ubicar el error
            sw.WriteLine("--------------- GlobalSettings -----------------")
            sw.WriteLine("UsuarioId: " + UsuarioId)
            sw.WriteLine("EstablecimientoId: " + CurrentEstablecimiento.EstablecimientoId)
            sw.WriteLine("Establecimiento: " + CurrentEstablecimiento.Descripcion)
            sw.WriteLine("RazonSocial: " + RazonSocial)
            sw.WriteLine("EsFabrica: " + EsFabrica.ToString())
            sw.WriteLine("EsAdministrador: " + EsAdministrador.ToString())
            sw.WriteLine("")

            'Escribo todos las configuraciones globales para poder ubicar el error
            sw.WriteLine("--------------- Configuration -----------------")
            sw.WriteLine("CUITEnsemble: " + LAFunctions.GlobalSetting.CUITEnsemble)
            sw.WriteLine("NombreBaseEnsemble: " + LAFunctions.GlobalSetting.NombreBaseEnsemble)
            sw.WriteLine("SMTP: " + LAFunctions.GlobalSetting.SMTP)
            sw.WriteLine("MailFromAddress: " + LAFunctions.GlobalSetting.MailFromAddress)
            sw.WriteLine("MailUsername: " + LAFunctions.GlobalSetting.MailUsername)
            sw.WriteLine("MailEnableSSL: " + LAFunctions.GlobalSetting.MailEnableSSL.ToString())
            sw.WriteLine("MailPort: " + LAFunctions.GlobalSetting.MailPort.ToString())
            sw.WriteLine("")



            'Escribo la informacionde la excepcion
            sw.WriteLine("--------------- Exception ----------------------")
            sw.WriteLine(strError)
        End Using

    End Sub

    Public Shared Sub LeerConfiguracionesXML()
        Dim doc As XmlDocument = New XmlDocument
        doc.PreserveWhitespace = True
        Dim strPathConfig As String = My.Application.Info.DirectoryPath + "\config.xml"

        'verifico si existe un archivo con la configuracion
        If (My.Computer.FileSystem.FileExists(strPathConfig)) Then
            Try
                doc.Load(strPathConfig)

                Dim book As XmlNode
                Dim nodeListLevel1 As XmlNodeList
                Dim nodeListLevel2 As XmlNodeList
                Dim root As XmlNode = doc.DocumentElement

                'Leo la configuracion de DEBUG
                nodeListLevel1 = root.SelectNodes("debug")
                For Each book In nodeListLevel1
                    'busco la configuracion general de esta propiedad
                    nodeListLevel2 = book.SelectNodes("general")
                    If nodeListLevel2.Count > 0 Then
                        SetDebugMode(IIf(nodeListLevel2.Item(0).InnerText.ToLower() = "true", True, False))
                    End If

                    'verifico si hay una configuracion especifica para esta razon social
                    If RazonSocial <> "" Then
                        nodeListLevel2 = book.SelectNodes(RemoveWhitespace(RazonSocial))
                        If nodeListLevel2.Count > 0 Then
                            SetDebugMode(IIf(nodeListLevel2.Item(0).InnerText.ToLower() = "true", True, False))
                        End If
                    End If
                Next
            Catch ex As Exception
                WriteLogFile("Error al leer el archivo de configuracion: " + ex.Message)
            End Try
        End If

    End Sub
    Public Shared Function RemoveWhitespace(fullString As String) As String
        Return New String(fullString.Where(Function(x) Not Char.IsWhiteSpace(x)).ToArray())
    End Function
    Public Shared Sub ClearDepositosSeleccionados()
        CurrentDepositos.Clear()
    End Sub
    Public Shared Sub AddListDepositoSeleccionados(Dep As Deposito)
        CurrentDepositos.Add(Dep)
    End Sub
    Public Function GetListDepositosSeleccionados() As List(Of Deposito)
        Return CurrentDepositos
    End Function

    Public Function GetListDepositos() As List(Of Deposito)
        Return ListDepositos
    End Function

    Public Sub LoadCentrosDistribucion()
        Dim valor As String
        Dim arrayValores As Array
        Dim respuesta As ResponseConfiguracion

        ListEstablecimientosDitribucion.Clear()

        respuesta = fachadaBD.GetParametroConfiguracion("EstIdDistribucion")
        If respuesta.ConsultaExitosa Then
            valor = respuesta.Valor
            arrayValores = valor.Split(",")
            For Each v As String In arrayValores
                ListEstablecimientosDitribucion.Add(v.Trim)
            Next
        End If
    End Sub

    Public Function EsCentroDistribucion() As Boolean
        Return ListEstablecimientosDitribucion.Contains(CurrentEstablecimiento.EstablecimientoId)
    End Function

End Class
