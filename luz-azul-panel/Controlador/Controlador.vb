Imports LAFunctions.LuzAzulCommon
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

        'verifico el cuit de la empresa y determino si es fabrica
        LogEvent.Add("VerificarFabrica()")
        Dim respuesta As ResponseCuit = fachadaBD.GetCUITEmpresa()
        If respuesta.ConsultaExitosa Then
            CUIT = respuesta.CUIT
            EsFabrica = (respuesta.CUIT = MDI_GlobalSettings.CUITEnsemble)
        Else
            WriteLogFile("La funcion verificar fabrica ha dado error")
            MsgBox(respuesta.mensaje, vbExclamation + vbOKOnly, MDI_GlobalSettings.TituloMensajes)
        End If

        ' Obtengo las configuraciones de envio de mails
        LogEvent.Add("LeerEntidadesMailings()")
        Dim respuestaEntidades As ResponseEntidades = fachadaBD.LeerEntidadesMailings()
        If respuestaEntidades.ConsultaExitosa = True Then
            'si la configuraciones estan cargadas sobreescribo las que vienen por default
            MDI_GlobalSettings.SMTP = respuestaEntidades.SMTPServer
            MDI_GlobalSettings.MailPort = respuestaEntidades.Puerto
            MDI_GlobalSettings.MailFromAddress = respuestaEntidades.Email
            MDI_GlobalSettings.MailUsername = respuestaEntidades.UID
            MDI_GlobalSettings.MailPassword = respuestaEntidades.PWD
            MDI_GlobalSettings.MailEnableSSL = respuestaEntidades.UsaSSL
        End If

        LogEvent.Add("GetRazonSocial()")
        Dim RespuestaRazonSocial As LAFunctions.LuzAzulCommon.ResponseRazonSocial = fachadaBD.GetRazonSocial()
        If RespuestaRazonSocial.ConsultaExitosa = True Then
            RazonSocial = RespuestaRazonSocial.RazonSocial
        Else
            WriteLogFile("La funcion Get Razon social ha dado error")
            MsgBox(respuesta.mensaje, vbExclamation + vbOKOnly, MDI_GlobalSettings.TituloMensajes)
        End If


    End Sub

    Public Shared Function GetInstancia() As Controlador
        If (instancia Is Nothing) Then
            fachadaBD = New LAFunctions.LuzAzulCommon With {.NombreBaseEnsemble = MDI_GlobalSettings.NombreBaseEnsemble}
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
    Public Shared Sub AddListEstablecimientoSeleccionados(est As Establecimiento)
        ListEstablecimientosSeleccionados.Add(est)
    End Sub
    Public Shared Sub SetListEstablecimientosSeleccionados(Ests As List(Of Establecimiento))
        ListEstablecimientosSeleccionados = Ests
    End Sub

    Public Function GetListEstablecimientosSeleccionados() As List(Of Establecimiento)
        Return ListEstablecimientosSeleccionados
    End Function

    Public Shared Sub SetListEstablecimientos(Ests As List(Of Establecimiento))
        ListEstablecimientos = Ests
    End Sub
    Public Function GetListEstablecimientos() As List(Of Establecimiento)
        Return ListEstablecimientos
    End Function
    Public Shared Function GetNombreBaseDB() As String
        Return fachadaBD.NombreBase
    End Function

    Public Shared Function Query() As List(Of String)
        Return fachadaBD.Query
    End Function
    Public Function DoLogin(ByVal Usuario As String, ByVal Clave As String) As ResponseLogin
        Dim respuesta As ResponseLogin
        respuesta = fachadaBD.DoLogin(Usuario, Clave)
        If (respuesta.PermiteLogin) Then
            UsuarioId = respuesta.mensaje 'usuario logueado
            EsAdministrador = respuesta.EsAdministrador

            LogEvent.Add("Usuario Logueado")
        End If

        Return respuesta
    End Function

    Public Function GetEstablecimientosUsuario(ByVal UsuarioId As String) As ResponseEstablecimiento
        Dim respuestaEstablecimiento As ResponseEstablecimiento

        respuestaEstablecimiento = fachadaBD.GetEstablecimientosUsuario(UsuarioId, EsFabrica, EsAdministrador)
        If (respuestaEstablecimiento.ConsultaExitosa) Then
            If respuestaEstablecimiento.rs.Count = 1 Then
                LogEvent.Add("El usuario tiene un solo establecimiento asociado")
            Else
                ListEstablecimientos = respuestaEstablecimiento.rs
                LogEvent.Add("El usuario tiene " + respuestaEstablecimiento.rs.Count.ToString() + " establecimientos")
            End If
        Else
            'Ocurrio un error al obtener el o los establecimientos
            WriteLogFile("Ocurrio un error al obtener el o los establecimientos")
        End If

        Return respuestaEstablecimiento

    End Function

    Public Function GetDepositosUsuario() As ResponseDeposito
        Dim respuestaDepositos As ResponseDeposito

        respuestaDepositos = fachadaBD.GetDepositosUsuario(UsuarioId, CurrentEstablecimiento.EstablecimientoId, EsFabrica, EsAdministrador, MostrarTodosDepositos)

        If (respuestaDepositos.ConsultaExitosa = False) Then
            ' Si ocurrio un error con el listado de depositos lo muestro en pantalla
            WriteLogFile("Ocurrio un error con los depositos del usuario")
            MsgBox(respuestaDepositos.mensaje, vbExclamation + vbOKOnly, MDI_GlobalSettings.TituloMensajes)
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
            sw.WriteLine("CUITEnsemble: " + MDI_GlobalSettings.CUITEnsemble)
            sw.WriteLine("NombreBaseEnsemble: " + MDI_GlobalSettings.NombreBaseEnsemble)
            sw.WriteLine("SMTP: " + MDI_GlobalSettings.SMTP)
            sw.WriteLine("MailFromAddress: " + MDI_GlobalSettings.MailFromAddress)
            sw.WriteLine("MailUsername: " + MDI_GlobalSettings.MailUsername)
            sw.WriteLine("MailEnableSSL: " + MDI_GlobalSettings.MailEnableSSL.ToString())
            sw.WriteLine("MailPort: " + MDI_GlobalSettings.MailPort.ToString())
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
