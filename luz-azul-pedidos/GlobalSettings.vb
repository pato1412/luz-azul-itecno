
Public Class GlobalSettings

    Public Shared NombreAplicacion As String = "Pedidos"
    Public Shared TipoNotificacionId As String = "1"
    Public Shared UrlWebserviceTest As String = "http://wstest.luz-azul.com.ar"
    Public Shared UrlWebservice As String = "http://webservice.luz-azul.com.ar"
    Public Shared NombreBaseEnsemble As String = "ENSEMBLE"
    Public Shared NombreClasificacionARealizarPedidos As String = "Aa Realizar Pedidos"
    Public Shared NombreClasificacionAgrupaproductos As String = "AB Combina Pedidos"
    Public Shared NombreClasificacionProductosOpcionales As String = "AC Productos Opcionales"
    Public Shared CUITEnsemble As String = "30712110429"

    Public Shared DefaulColorAgrupacion As String = "#30,34,255"

    'Para validar la columna Cantidad Pedida en la grilla necesito saber el indice
    Public Shared IndiceColumnaCantidadPedida As Integer = 5

    'El combo de acciones multiples
    Public Shared ListAccionSeleccione As Integer = 0
    Public Shared ListAccionPonerACero As Integer = 1
    Public Shared ListAccionRestaurar As Integer = 2

    'Configuraciones para utilizar el sistema con un cliente
    Public Shared ListEMailsDefault As List(Of String) = New List(Of String) From {"pedidos@luz-azul.com.ar", "expedicionbarracas@luz-azul.com.ar", "pedidosbarracas@luz-azul.com.ar"}

    'Este el el id del cliente que tiene cargados los mails de contacto de los proveedores
    Public Shared ClienteIdClasificaciones As String = "2"

    'Establecimiento Id default como centro de distribucion si no tiene configuracion en pedido proveedor
    Public Shared EstablecimientoIdDistribucionDefault As String = "10"

    'Configuraciones para el envio de emails
    Public Shared SMTP As String = "smtp.gmail.com"
    Public Shared MailFromAddress As String = "contacto@luz-azul.com.ar"
    Public Shared MailSubject As String = "Notificacion de envio de pedido realizado por el sistema - Nro de pedido: "
    Public Shared MailPort As Integer = 587
    Public Shared MailUsername As String = "contacto@luz-azul.com.ar"
    Public Shared MailPassword As String = "luzazul499"
    Public Shared MailEnableSSL As Boolean = True
    Public Shared MailPortGmail As Integer = 587

    'Configuracion Mails Criticos
    Public Shared ListDestinatariosCriticos As List(Of String) = New List(Of String) From {"pato1412@gmail.com", "gustavowroldan@gmail.com", "pedidos@luz-azul.com.ar"}
    Public Shared SubjectCritico As String = "Ocurrio un error critico en el sistema de envio de Pedidos"

    ' Miscelaneas
    Public Shared TituloMensajes As String = "Luz Azul - Pedidos"

    'Logs

End Class

