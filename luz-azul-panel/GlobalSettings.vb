Imports System.IO

Public Class GlobalSettings
    Public Shared NombreAplicacion As String = "Agrupacion Pedidos"
    Public Shared TipoNotificacionId As String = "1"
    Public Shared UrlWebservice As String = "http://webservice.luz-azul.com.ar"
    'Public Shared UrlWebservice As String = "http://wstest.luz-azul.com.ar"
    Public Shared NombreBaseEnsemble As String = "ENSEMBLE"
    Public Shared NombreClasificacionARealizarPedidos As String = "Aa Realizar Pedidos"
    Public Shared NombreClasificacionAgrupaproductos As String = "AB Combina Pedidos"
    Public Shared CUITEnsemble As String = "30712110429"
    Public Shared UsuarioId As String = ""
    Public Shared EsAdministrador As Boolean = False
    Public Shared CurrentAgrupacionId As String = ""
    Public Shared CurrentDescripcion As String = ""
    Public Shared CurrentObservaciones As String = ""
    Public Shared StrDepositos As String = ""
    Public Shared NroPedido As String
    Public Shared EsFabrica As Boolean = False
    Public Shared db As New LAFunctions.LAFunctions

    Public Shared RazonSocial As String = ""

    ' Miscelaneas
    Public Shared TituloMensajes As String = "Luz Azul - Pedidos"

    'Logs
    Public Shared LogEvent As List(Of String) = New List(Of String)
    Public Shared LogQuery As List(Of String) = New List(Of String)

    Public Shared Sub WriteLogFile(strError As String)

        Dim myFilePath As String = My.Application.Info.DirectoryPath + "\error_log " + Today.Day.ToString() + "." + Today.Month.ToString() + "." + Today.Year.ToString() + " " + Now.Hour.ToString() + "." + Now.Minute.ToString() + ".log"

        Using sw As New StreamWriter(File.Open(myFilePath, FileMode.Append))
            sw.WriteLine("--------------- Archivo generado automaticamente " + DateTime.Now + "-----------------------")
            sw.WriteLine("")

            'Escribo la cadena de eventos que se ejecutaron
            sw.WriteLine("--------------- Events -------------------------")
            For Each StrEvent In GlobalSettings.LogEvent
                sw.WriteLine(StrEvent)
            Next
            sw.WriteLine("")

            'Escribo la cadena de queries que se ejecutaron
            sw.WriteLine("--------------- Queries ------------------------")
            For Each StrQuery In GlobalSettings.LogQuery
                sw.WriteLine(StrQuery)
            Next
            sw.WriteLine("")

            'Escribo todos las variables globales para poder ubicar el error
            sw.WriteLine("--------------- GlobalSettings -----------------")
            sw.WriteLine("UsuarioId: " + GlobalSettings.UsuarioId)
            sw.WriteLine("")
            sw.WriteLine("NroPedido: " + GlobalSettings.NroPedido)
            sw.WriteLine("")

            'Escribo todos las configuraciones globales para poder ubicar el error
            sw.WriteLine("--------------- Configuration -----------------")
            sw.WriteLine("UrlWebservice: " + GlobalSettings.UrlWebservice)
            sw.WriteLine("CUITEnsemble: " + GlobalSettings.CUITEnsemble)
            sw.WriteLine("NombreBaseEnsemble: " + GlobalSettings.NombreBaseEnsemble)
            sw.WriteLine("TipoNotificacionId: " + GlobalSettings.TipoNotificacionId)
            sw.WriteLine("")

            'Escribo la informacionde la excepcion
            sw.WriteLine("--------------- Exception ----------------------")
            sw.WriteLine(strError)
        End Using

    End Sub

End Class

Public Class ComboItem
    Public Sub New(ByVal text As String, ByVal value As Integer)
        t = text
        v = value
    End Sub

    Private t As String
    Private v As Integer
    Public Property Text() As String
        Get
            Return t
        End Get
        Set(ByVal value As String)
            t = value
        End Set
    End Property
    Public Property Value() As Integer
        Get
            Return v
        End Get
        Set(ByVal value As Integer)
            v = value
        End Set
    End Property
End Class
