Imports LAFunctions.LuzAzulPedidos
Imports Microsoft.Office.Interop
Imports Newtonsoft.Json
Imports System.IO
Imports System.Net
Imports System.Net.Mail
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Xml
Imports System.Web
Public Class Controlador
    Inherits LAFunctions.ControladorCommon

    Private Shared fachadaBD As LAFunctions.LuzAzulPedidos
    Private Shared instancia As Controlador
    Private PathArchivosExcel As String
    Private nroRowExcel As Integer
    Private currentProveedor As Clasificacion
    Private Shared PunteroProveedores As Integer
    Private Shared ListDepositos As List(Of Deposito) = New List(Of Deposito)
    Private Shared ListProveedores As List(Of Clasificacion) = New List(Of Clasificacion)
    Private Shared ListEstablecimientos As List(Of Establecimiento) = New List(Of Establecimiento)
    Private Shared ListAgrupaciones As List(Of Agrupacion) = New List(Of Agrupacion)
    Private Shared ReLProductoAgrupacion As List(Of ReLProductoAgrupacion) = New List(Of ReLProductoAgrupacion)
    Private Shared Cliente As ClientePedido = New ClientePedido("", "", "")
    Private Shared ClienteDireccion As DireccionCliente = New DireccionCliente("", "1", "Sin asignar", "", "", "", "Sin localidad", "", True)
    Private Shared CurrentAgrupacionId As String = ""
    Private Shared CurrentDescripcion As String = ""
    Private Shared CurrentObservaciones As String = ""
    Private Shared Pedido As Pedido = New Pedido
    Private Shared NroPedido As String
    Private Shared MostrarTodosDepositos As Boolean = False
    Private Shared EsPedidoFacturado As Boolean = True
    Private Shared EsPedidoCliente As Boolean = False
    Private Shared strEmails As String
    Private Shared StrDepositos As String = ""
    Private Shared ListClasificacionesClientes As List(Of String) = New List(Of String)
    Private Shared ListProductosOpcionales As List(Of String) = New List(Of String)
    Private ErrorGeneracionExcel As Boolean = False
    Private ErrorEnvioMails As Boolean = False
    Private Sub New()

    End Sub

    Public Shared Function GetInstancia() As Controlador
        If (instancia Is Nothing) Then
            fachadaBD = New LAFunctions.LuzAzulPedidos With {.NombreBaseEnsemble = GlobalSettings.NombreBaseEnsemble}
            instancia = New Controlador()
        End If

        Return instancia
    End Function

    Public Function GetMontoVencido() As Double
        ' Obtengo la deuda del establecimiento siempre que no sea Ensemble
        If Not EsFabrica Then
            Return fachadaBD.GetImporteVencidoCtrlCtaCte(GetCurrentEstablecimientoId())
        Else
            Return 0
        End If
    End Function

    Public Sub SetNroPedido(Nro As String)
        NroPedido = Nro
    End Sub
    Public Function GetNroPedido() As String
        Return NroPedido
    End Function

    Public Sub PunteroSiguiente()
        PunteroProveedores += 1
    End Sub

    Public Sub PunteroAnterior()
        PunteroProveedores -= 1
    End Sub

    Public Function GetPuntero() As Integer
        Return PunteroProveedores
    End Function
    Public Sub SetCurrentProveedor()
        currentProveedor = ListProveedores(PunteroProveedores)
    End Sub

    Public Function GetCurrentProveedor() As Clasificacion
        Return ListProveedores(PunteroProveedores)
    End Function

    Public Function GetCurrentProveedorId() As String
        Return ListProveedores(PunteroProveedores).ClasificacionProdId
    End Function

    Public Function GetPosicionProveedorPedido() As Integer
        Return Pedido.ListProveedores.FindIndex(Function(element) element.proveedorId = currentProveedor.ClasificacionProdId)
    End Function

    Public Function GetCantProveedores() As Integer
        Return ListProveedores.Count
    End Function

    Public Function GetListProveedoresPedido() As List(Of PedidoProveedor)
        Return Pedido.ListProveedores
    End Function
    Public Function GetCurrentAgrupacionId() As String
        Return CurrentAgrupacionId
    End Function

    Public Function GetPosicionAgrupacion(AgrupacionId As String) As Integer
        Return ListAgrupaciones.FindIndex(Function(element) element.AgrupacionId = AgrupacionId)
    End Function

    Public Function GetCurrentAgrupacion() As Agrupacion
        Dim pos As Integer = GetPosicionAgrupacion(CurrentAgrupacionId)
        Return ListAgrupaciones(pos)
    End Function
    Public Function GetAgrupacion(pos As Integer) As Agrupacion
        Return ListAgrupaciones(pos)
    End Function
    Public Sub SetCurrentAgrupacionId(IdAgrupacion As String)
        CurrentAgrupacionId = IdAgrupacion
    End Sub

    Public Function GetCurrentObservaciones() As String
        Return CurrentObservaciones
    End Function

    Public Sub SetCurrentObservaciones(obs As String)
        CurrentObservaciones = obs
    End Sub
    Public Function GetCurrentDescripcion() As String
        Return CurrentDescripcion
    End Function
    Public Sub SetCurrentDescripcion(Desc As String)
        CurrentDescripcion = Desc
    End Sub
    Public Shared Sub SetMostrarTodosDepositos(Mostrar As Boolean)
        MostrarTodosDepositos = Mostrar
    End Sub
    Public Shared Sub SetEspedidoCliente(EsPedido As Boolean)
        EsPedidoCliente = EsPedido
    End Sub
    Public Shared Sub ClearCliente()
        Cliente = New ClientePedido("", "", "")
    End Sub

    Public Sub SetErrorGeneracionExcel(valor As Boolean)
        ErrorGeneracionExcel = valor
    End Sub

    Public Function GetErrorGeneracionExcel() As Boolean
        Return ErrorGeneracionExcel
    End Function

    Public Sub SetErrorEnvioMails(valor As Boolean)
        ErrorEnvioMails = valor
    End Sub

    Public Function GetErrorEnvioMails() As Boolean
        Return ErrorEnvioMails
    End Function

    Public Sub FinalizarInstancia()

        'reseteo el puntero
        PunteroProveedores = 0

        'reseteo el listado de agrupaciones
        ListAgrupaciones = New List(Of Agrupacion)

        'reseteo el listado de proveedores
        ListProveedores = New List(Of Clasificacion)

        'elimino el pedido anterior
        Pedido = New Pedido

        'elimino los depositos anteriores
        ListDepositos = New List(Of Deposito)
        StrDepositos = ""

        instancia = Nothing

    End Sub

    Public Shared Function GetNombreBaseDB() As String
        Return fachadaBD.NombreBase
    End Function

    Public Shared Function Query() As List(Of String)
        Return fachadaBD.Query
    End Function

    Public Function GetDepositosUsuario() As ResponseDeposito
        Dim respuestaDepositos As ResponseDeposito

        respuestaDepositos = fachadaBD.GetDepositosUsuario(UsuarioId, CurrentEstablecimiento.SucursalId)

        If (respuestaDepositos.ConsultaExitosa = False) Then
            ' Si ocurrio un error con el listado de depositos lo muestro en pantalla
            WriteLogFile("Ocurrio un error con los depositos del usuario")
            MsgBox(respuestaDepositos.mensaje, vbExclamation + vbOKOnly, GlobalSettings.TituloMensajes)
        End If
        Return respuestaDepositos
    End Function

    Public Function GetArbolClasificaciones(ListClasificaciones As List(Of String)) As ResponseClasificaciones
        Dim respuestaClasificacion As ResponseClasificaciones

        respuestaClasificacion = fachadaBD.GetArbolClasificaciones(ListClasificaciones, CurrentEstablecimiento.EstablecimientoId, GlobalSettings.NombreClasificacionARealizarPedidos)

        If (respuestaClasificacion.ConsultaExitosa = False) Then
            ' Si ocurrio un error con el listado de proveedores lo muestro en pantalla
            WriteLogFile("Ocurrio un error con el arbol de clasificaciones")
            MsgBox(respuestaClasificacion.mensaje, vbExclamation + vbOKOnly, GlobalSettings.TituloMensajes)
        End If

        Return respuestaClasificacion

    End Function

    Public Sub GuardarDepositos(ListDepositosSeleccionados As List(Of Deposito))

        For Each Dep As Deposito In ListDepositosSeleccionados
            ListDepositos.Add(Dep)
            StrDepositos += Dep.Descripcion + ", "
        Next
        ' Elimino la ultimo ", "
        StrDepositos = StrDepositos.Substring(0, StrDepositos.Length - 2)

    End Sub

    Public Sub GuardarClasificaciones(ListClasificaciones As List(Of String))
        Dim respuestaClasificacion As ResponseClasificaciones
        Dim respuestaEmails As ResponseEmailClasificaciones
        Dim CurrentProveedor As Clasificacion
        Dim pos As Integer

        ' Obtengo las subclasificaciones de las clasificaciones seleccionadas
        Dim ListPadres As List(Of String) = New List(Of String)
        respuestaClasificacion = GetArbolClasificaciones(ListClasificaciones)

        If (respuestaClasificacion.ConsultaExitosa = True And respuestaClasificacion.rs IsNot Nothing) Then
            For Each Clas As Clasificacion In respuestaClasificacion.rs
                ' Guardo en las configuracion global los proveedores hijos
                ListProveedores.Add(New Clasificacion(Clas.ClasificacionProdId, Clas.Descripcion, Clas.ClasificacionProdPadre, Clas.CantDiasReparo, Clas.FrecuenciaPed, Clas.PlazoEntregaPed, Clas.KilosMin, Clas.EstablecimientoIdDistribucion))
                If ListPadres.Contains(Clas.ClasificacionProdPadre) = False Then
                    ListPadres.Add(Clas.ClasificacionProdPadre)
                End If
            Next
        End If

        ' Guardo en las configuracion global los proveedores padre que no tengan hijos
        respuestaClasificacion = GetArbolClasificaciones(New List(Of String))
        Dim lstProveedores As List(Of Clasificacion) = respuestaClasificacion.rs.ToList()
        For Each ClasificacionId As String In ListClasificaciones
            If ListPadres.Contains(ClasificacionId) = False Then
                pos = lstProveedores.FindIndex(Function(element) element.ClasificacionProdId = ClasificacionId)
                If pos >= 0 Then
                    CurrentProveedor = lstProveedores(pos)
                    ListProveedores.Add(New Clasificacion(CurrentProveedor.ClasificacionProdId, CurrentProveedor.Descripcion, "", CurrentProveedor.CantDiasReparo, CurrentProveedor.FrecuenciaPed, CurrentProveedor.PlazoEntregaPed, CurrentProveedor.KilosMin, CurrentProveedor.EstablecimientoIdDistribucion))
                End If
            End If
        Next

        'Busco las direcciones de email de cada uno de los proveedores seleccionados y las agrego al global settings
        respuestaEmails = fachadaBD.GetEmailsClasificaciones(GlobalSettings.ClienteIdClasificaciones)
        If (respuestaEmails.ConsultaExitosa = True) Then
            For Each EmailClas As EmailClasificacion In respuestaEmails.rs
                'voy a pisar el nombre del proveedor por el mismo sin prefijo
                EmailClas.Descripcion = GetNombreProveedorSinPrefijo(EmailClas.Descripcion)

                Dim NombreClass As String = EmailClas.Descripcion
                pos = ListProveedores.FindIndex(Function(element) element.DescripcionSinPrefijo = NombreClass)
                If pos >= 0 Then

                    'Existe una condicion especial que si el establecimiento es cabildo o bahia blanca debo reemplazar el email
                    If CurrentEstablecimiento.EstablecimientoId = GlobalSettings.EstablecimientoIdBahiaBlanca Or CurrentEstablecimiento.EstablecimientoId = GlobalSettings.EstablecimientoIdCabildo Then
                        If EmailClas.Email = GlobalSettings.EmailDepositoOlavarria Then
                            Dim EmailReemplazo As EmailClasificacion = New EmailClasificacion(EmailClas.TipoContactoClienteId, EmailClas.Nombre, EmailClas.Descripcion, GlobalSettings.EmailReemplazo1)
                            Dim EmailReemplazo2 As EmailClasificacion = New EmailClasificacion(EmailClas.TipoContactoClienteId, EmailClas.Nombre, EmailClas.Descripcion, GlobalSettings.EmailReemplazo2)
                            ListProveedores(pos).ListEmails.Add(EmailReemplazo)
                            ListProveedores(pos).ListEmails.Add(EmailReemplazo2)
                        Else
                            ListProveedores(pos).ListEmails.Add(EmailClas)
                        End If
                    Else
                        ListProveedores(pos).ListEmails.Add(EmailClas)
                    End If

                End If
            Next
        End If

        'Ordeno la lista de proveedores por descripcion en orden alfabetico
        ListProveedores = ListProveedores.OrderBy(Function(x) x.Descripcion).ToList()


        LogEvent.Add("Se han seleccionado " + ListProveedores.Count.ToString() + " Proveedores")

    End Sub

    Public Function GetClientes() As ResponseClientes
        Dim respuestaCliente As ResponseClientes
        respuestaCliente = fachadaBD.GetClientes()
        If (respuestaCliente.ConsultaExitosa = False) Then
            WriteLogFile("Ocurrio un error al leer los clientes del proyecto")
        End If
        Return respuestaCliente
    End Function

    Public Sub GuardarConfiguracionesClientes(currentCliente As ComboItemCliente, ByVal ArrayEmails As List(Of String))
        Dim respuestaDepositos As ResponseDeposito
        Dim respuestaClasificacion As ResponseClasificaciones
        Dim respuestaClasificacionHijos As ResponseClasificaciones

        'Guardo la lista de email en una variable para imprimir en la listados
        strEmails = ""
        For Each CurrentEmailList As String In ArrayEmails
            strEmails += CurrentEmailList + ", "
        Next
        'Elimino el ultimo ", "
        strEmails = strEmails.Substring(0, strEmails.Length - 2)

        Cliente.ClienteId = currentCliente.Value
        LogEvent.Add("Cliente Id seleccionado: " + Cliente.ClienteId)

        Cliente.RazonSocial = currentCliente.RazonSocial
        LogEvent.Add("Cliente Razon social: " + Cliente.RazonSocial)

        Cliente.CUIT = currentCliente.Cuit.Replace("-", "")
        LogEvent.Add("Cliente CUIT: " + Cliente.CUIT)

        'Obtengo la direcciones del cliente
        Dim responseDirecciones As ResponseDireccionesCliente = fachadaBD.GetDireccionesCliente(Cliente.ClienteId, True)

        If responseDirecciones.ConsultaExitosa Then
            If responseDirecciones.rs.Count > 0 Then
                Dim direcciones As List(Of DireccionCliente) = responseDirecciones.rs
                'Asigno la primer direccion de entrega 
                ClienteDireccion = direcciones.First
            End If
        End If

        ' Obtengo los depositos del usuario
        respuestaDepositos = GetDepositosUsuario()

        If (respuestaDepositos.ConsultaExitosa = True) Then
            For Each Dep As Deposito In respuestaDepositos.rs
                ' Guardo en la configuracion global los depositos seleccionados
                ListDepositos.Add(Dep)
                StrDepositos += Dep.Descripcion + ", "
            Next
            ' Elimino la ultimo ", "
            StrDepositos = StrDepositos.Substring(0, StrDepositos.Length - 2)
        Else
            ' Si ocurrio un error con el listado de depositos lo muestro en pantalla
            WriteLogFile("Ocurrio un error con los depositos del usuario")
            MsgBox(respuestaDepositos.mensaje, vbExclamation + vbOKOnly, GlobalSettings.TituloMensajes)
        End If


        ' Obtengo la lista de proveedores inicialesp
        respuestaClasificacion = GetArbolClasificaciones(New List(Of String))
        If (respuestaClasificacion.ConsultaExitosa = True) Then


            Dim ListClasificaciones As List(Of String) = New List(Of String)
            For Each Clas As Clasificacion In respuestaClasificacion.rs
                ListClasificaciones.Add(Clas.ClasificacionProdId)
            Next

            ' Obtengo las subclasificaciones de las clasificaciones seleccionadas
            Dim ListPadres As List(Of String) = New List(Of String)
            respuestaClasificacionHijos = GetArbolClasificaciones(ListClasificaciones)

            If (respuestaClasificacionHijos.ConsultaExitosa = True) Then
                For Each Clas As Clasificacion In respuestaClasificacionHijos.rs
                    ' Guardo en las configuracion global los proveedores hijos

                    For Each CurrEmail As String In ArrayEmails
                        Dim EmailCliente As EmailClasificacion = New EmailClasificacion("Tipo Contacto", Clas.DescripcionSinPrefijo, Clas.DescripcionSinPrefijo, CurrEmail)
                        Clas.ListEmails.Add(EmailCliente)
                    Next

                    'verifico si la clasificacion esta en la lista de las habilitadas para pedidos clientes
                    If ListClasificacionesClientes.Contains(Clas.Descripcion.ToLower()) Then
                        ListProveedores.Add(Clas)
                    End If

                    'Guardo la lista de padres que tienen algun hijo
                    If ListPadres.Contains(Clas.ClasificacionProdPadre) = False Then
                        ListPadres.Add(Clas.ClasificacionProdPadre)
                    End If
                Next
            End If

            ' Guardo en las configuracion global los proveedores padre que no tengan hijos
            For Each Clas As Clasificacion In respuestaClasificacion.rs
                If ListPadres.Contains(Clas.ClasificacionProdId) = False Then
                    For Each CurrEmail As String In ArrayEmails
                        Dim EmailCliente As EmailClasificacion = New EmailClasificacion("Tipo Contacto", Clas.DescripcionSinPrefijo, Clas.DescripcionSinPrefijo, CurrEmail)
                        Clas.ListEmails.Add(EmailCliente)
                    Next

                    'verifico si la clasificacion esta en la lista de las habilitadas para pedidos clientes
                    If ListClasificacionesClientes.Contains(Clas.Descripcion.ToLower()) Then
                        ListProveedores.Add(Clas)
                    End If
                End If
            Next

            'Ordeno la lista de proveedores por descripcion sin prefijo en orden alfabetico
            ListProveedores = ListProveedores.OrderBy(Function(x) x.DescripcionSinPrefijo).ToList()

        Else
            ' Si ocurrio un error con el listado de proveedores lo muestro en pantalla
            WriteLogFile("Ocurrio un error con el arbol de clasificaciones")
            MsgBox(respuestaClasificacion.mensaje, vbExclamation + vbOKOnly, GlobalSettings.TituloMensajes)
        End If

        LogEvent.Add("Se han seleccionado " + ListProveedores.Count.ToString() + " Proveedores")


    End Sub
    Public Function GetDireccionCliente() As String
        Dim strDireccion As String

        strDireccion = ClienteDireccion.Calle + " " + ClienteDireccion.Numero

        If ClienteDireccion.Piso <> "0" Then
            strDireccion += " Piso: " + ClienteDireccion.Piso
        End If
        If ClienteDireccion.Dpto <> "" Then
            strDireccion += " Dpto: " + ClienteDireccion.Dpto
        End If

        strDireccion += " " + ClienteDireccion.Localidad + " ( " + ClienteDireccion.Provincia + ")"

        Return strDireccion

    End Function

    Public Function GetListAccionesMultiples() As List(Of LAFunctions.ComboItem)
        Dim listAcciones As List(Of LAFunctions.ComboItem) = New List(Of LAFunctions.ComboItem) From {
            New LAFunctions.ComboItem("--- Acciones multiples ---", GlobalSettings.ListAccionSeleccione),
            New LAFunctions.ComboItem("Poner las cantidades en cero", GlobalSettings.ListAccionPonerACero),
            New LAFunctions.ComboItem("Restaurar valores por defecto", GlobalSettings.ListAccionRestaurar)
        }
        Return listAcciones
    End Function

    Public Function ObtenerDirectorioArchivosExcel() As Boolean
        'CONFIGURACION CLIENTE CIPOLETTI "\\SRV-CIPOLLETTI\Tempo\OneDrive\Distribucion FRQ\PedidosLuzAzul.exe"
        'City Bell \\LUZSERVER\Tempo\GoogleSync\Distribucion FRQ

        'PathArchivosExcel = My.Application.Info.DirectoryPath + "\tmp\" + GlobalSettings.EstablecimientoId + "\"
        'Dim dirRoot As String = System.IO.Directory.GetDirectoryRoot("\\LUZSERVER\Tempo\GoogleSync\Distribucion FRQ\app-pedidos")
        Dim dirRoot As String = System.IO.Directory.GetDirectoryRoot(My.Application.Info.DirectoryPath)
        If dirRoot.Contains("\Tempo") Then
            'si el root contiene la palabra tempo es porque es un directorio en red
            PathArchivosExcel = dirRoot + "\Pedidos\tmp\" + CurrentEstablecimiento.EstablecimientoId + "\"
        Else
            'Este es un root local 
            PathArchivosExcel = dirRoot + "Tempo\Pedidos\tmp\" + CurrentEstablecimiento.EstablecimientoId + "\"
        End If

        Try
            'Elimino la carpeta tmp del establecimiento
            If (My.Computer.FileSystem.DirectoryExists(PathArchivosExcel)) Then
                My.Computer.FileSystem.DeleteDirectory(PathArchivosExcel, FileIO.DeleteDirectoryOption.DeleteAllContents)
            End If

            'Voy a intentar generar una carpeta para los archivos excel y verificar si tengo permisos mas que nada por si estoy en una red 
            If (My.Computer.FileSystem.DirectoryExists(PathArchivosExcel) = False) Then
                My.Computer.FileSystem.CreateDirectory(PathArchivosExcel)
            End If

            Return True

        Catch ex As Exception
            WriteLogFile("NO hay permisos para crear el directorio " + PathArchivosExcel + " - Message: " + ex.Message)
            Return False
        End Try

    End Function
    Public Function GetPathArchivosExcel() As String
        Return PathArchivosExcel
    End Function

    Public Function ValidarMailsProveedores() As Boolean
        'Con este procedimiento valido que cada proveedor del pedido tenga por lo menos un mail asociado porque puede fallar el sincronizador y dejar sin mail las configuraciones
        Dim StrListProveedores As String = ""
        For Each Detalle As Clasificacion In ListProveedores
            If Detalle.ListEmails.Count = 0 Then
                StrListProveedores += Detalle.Descripcion + ", "
            End If
        Next

        If StrListProveedores <> "" Then
            StrListProveedores = StrListProveedores.Substring(0, StrListProveedores.Length - 2)
            Dim strMessage = "La configuracion de mails de los proveedores obtenida de TEMPO no es la correcta, compruebe la sincronizacion. " + vbCrLf + "El sistema no puede continuar, por favor comuniquese con el administrador."
            WriteLogFile("Error al validar los mails de los proveedores: " + strMessage + " - Proveedores sin mails: " + StrListProveedores)
            Return False
        Else
            Return True
        End If

    End Function
    Public Function GetRazonSocialPedido() As String
        If EsPedidoCliente Then
            Return Cliente.RazonSocial
        Else
            Return RazonSocial
        End If
    End Function
    Public Function GetCUITPedido() As String
        If EsPedidoCliente Then
            Return Cliente.CUIT
        Else
            Return CUIT
        End If
    End Function

    Public Function GetLabelEstablecimiento() As String
        If EsPedidoCliente Then
            Return "Cliente: " + Cliente.RazonSocial
        Else
            Return "Establecimiento: " + CurrentEstablecimiento.Descripcion
        End If
    End Function
    Public Function GetLabelDeposito() As String
        If EsPedidoCliente Then
            Return "Email: " + strEmails
        Else
            Return "Depositos: " + StrDepositos
        End If
    End Function

    Public Sub GenerarAgrupaciones()

        'Obtengo el listado de clasificaciones que dependen de la claisficacion "AB Combina Productos"
        Dim RespuestaAgrupaciones As ResponseAgrupaciones
        RespuestaAgrupaciones = fachadaBD.GetAgrupacionesProductos(GlobalSettings.NombreClasificacionAgrupaproductos)

        If RespuestaAgrupaciones.ConsultaExitosa = True Then
            'Guardo las agrupaciones en el objeto global
            ListAgrupaciones = RespuestaAgrupaciones.rs

            'Obtengo los productos de dichas clasificaciones
            Dim respuestaProductos As ResponseProductosAgrupaciones
            respuestaProductos = fachadaBD.GetProductosAgrupaciones(RespuestaAgrupaciones.rs, ListProveedores)
            If respuestaProductos.ConsultaExitosa = True Then
                If respuestaProductos.rs.Count > 0 Then
                    Dim pos As Integer
                    For Each AgrupacionProducto As ProductoAgrupacion In respuestaProductos.rs
                        pos = ListAgrupaciones.FindIndex(Function(element) element.AgrupacionId = AgrupacionProducto.AgrupacionId)
                        If pos >= 0 Then
                            'Asocio el producto actual con la agrupacion del objeto global
                            ListAgrupaciones(pos).ListProductos.Add(AgrupacionProducto.ProductoId)
                        End If

                        ReLProductoAgrupacion.Add(New ReLProductoAgrupacion(AgrupacionProducto.ProductoId, AgrupacionProducto.AgrupacionId))
                    Next
                End If
            End If

        End If

        'recorro el arbol de agrupaciones y borro las que no tienen productos
        For n As Integer = ListAgrupaciones.Count - 1 To 0 Step -1
            If ListAgrupaciones(n).ListProductos.Count = 0 Then
                ListAgrupaciones.RemoveAt(n)
            End If
        Next


    End Sub
    Public Sub ValidarCantidadesEnUnidadesPorBulto(DbGridProveedores As DataGridView, IndexCurrentRow As Integer)
        Try
            Dim CantPedida As Double = Convert.ToDouble(DbGridProveedores.Rows(IndexCurrentRow).Cells("CantidadPedida").Value)
            Dim UnidadesxBulto As Integer = Convert.ToInt32(DbGridProveedores.Rows(IndexCurrentRow).Cells("UnidadesPorBulto").Value)
            Dim Unidad As String = DbGridProveedores.Rows(IndexCurrentRow).Cells("Unidad").Value
            Dim AgrupacionId As String = DbGridProveedores.Rows(IndexCurrentRow).Cells("AgrupacionId").Value
            'verifico si la caantidad pedida es multiplo de la unidades por bulto
            If LCase(Unidad) = "unidad" And UnidadesxBulto > 1 And CantPedida > 0 And AgrupacionId = "" Then
                Dim resto As Double = CantPedida Mod UnidadesxBulto
                If resto <> 0 Then
                    'recorro toda la grilla y guardo en las variables globales el pedido
                    If MsgBox("El Valor ingresado debe ser multiplo de " + UnidadesxBulto.ToString() + vbCrLf + "Si presiona Aceptar se seleccionara el multiplo mayor mas cercano " + vbCrLf + "Si presiona Cancelar se restaurara el valor original del producto", vbCritical + vbOKCancel, GlobalSettings.TituloMensajes) = vbOK Then
                        'redondeo la cantidad de veces para arriba
                        Dim CantVeces As Integer = Math.Ceiling(CantPedida / UnidadesxBulto)
                        Dim TotalAPedir As Integer = CantVeces * UnidadesxBulto
                        DbGridProveedores.Rows(IndexCurrentRow).Cells("CantidadPedida").Value = TotalAPedir.ToString()
                    Else
                        DbGridProveedores.Rows(IndexCurrentRow).Cells("CantidadPedida").Value = DbGridProveedores.Rows(IndexCurrentRow).Cells("CantidadSugerida").Value
                    End If
                End If
            End If
        Catch ex As Exception
            WriteLogFile("Error al ValidarCantidadesEnUnidadesPorBulto: " + ex.Message)
        End Try

    End Sub

    Public Function validarPesoPedido(DbGridProveedores As DataGridView) As Boolean
        Dim currentProveedorId As String = currentProveedor.ClasificacionProdId
        Dim pos As Integer
        pos = ListProveedores.FindIndex(Function(element) element.ClasificacionProdId = currentProveedorId)
        If pos >= 0 Then
            Dim KilosMinProveedor As Integer
            KilosMinProveedor = ListProveedores(pos).KilosMin * 0.95 ' Le doy un margen del 5% de tolerancia
            If KilosMinProveedor > 0 Then
                Dim TotalKgsPedido As Double
                TotalKgsPedido = CalcularPesoPedido(DbGridProveedores)
                If TotalKgsPedido < KilosMinProveedor Then
                    Return False
                End If
            End If
        End If
        Return True
    End Function
    Public Function GetKilosMinProveedor() As Integer
        Dim currentProveedorId As String = currentProveedor.ClasificacionProdId
        Dim pos As Integer
        pos = ListProveedores.FindIndex(Function(element) element.ClasificacionProdId = currentProveedorId)
        If pos >= 0 Then
            Return ListProveedores(pos).KilosMin
        Else
            Return 0
        End If
    End Function

    Public Sub ValidarAgrupaciones()
        'este sub analiza las cantidades de los articulos en cada agrupacion y verifica que sean multiplos de la unidades por bulto
        For indexAgrupacion As Integer = 0 To ListAgrupaciones.Count - 1
            Dim currentAgrupacion As Agrupacion = ListAgrupaciones(indexAgrupacion)

            'recorro todos los proveedores del pedido
            For indexProveedor As Integer = 0 To Pedido.ListProveedores.Count - 1
                Dim ListArticulosAgrupacion As List(Of Integer) = New List(Of Integer)
                'recorro todos los articulos del proveedor
                For indexProducto As Integer = 0 To Pedido.ListProveedores(indexProveedor).ListPedido.Count - 1
                    Dim CurrentProducto As DetallePedido = Pedido.ListProveedores(indexProveedor).ListPedido(indexProducto)
                    'Verifico si el producto corresponde con la agrupacion actual
                    If CurrentProducto.AgrupacionId = currentAgrupacion.AgrupacionId Then
                        'Guardo en la lista el puntero de los productos que me interesan
                        ListArticulosAgrupacion.Add(indexProducto)
                    End If
                Next

                'recorro los productos que pertenecen a la agrupacion actual para realizar la validacion
                If ListArticulosAgrupacion.Count > 0 Then
                    Dim CantidadTotalProducto As Integer = 0

                    For Each indexProductoAgrupacion As Integer In ListArticulosAgrupacion
                        CantidadTotalProducto += Pedido.ListProveedores(indexProveedor).ListPedido(indexProductoAgrupacion).CantidadPedida
                    Next

                    'verifico si la cantidad total es multiplo de las unidades por bulto
                    Dim resto As Double = CantidadTotalProducto Mod currentAgrupacion.UnidadesPorBulto
                    If resto <> 0 Then
                        'redondeo la cantidad de veces para arriba
                        Dim CantVeces As Integer = Math.Ceiling(CantidadTotalProducto / currentAgrupacion.UnidadesPorBulto)

                        Dim TotalAPedir As Integer = CantVeces * currentAgrupacion.UnidadesPorBulto
                        Dim RestoAPedir As Integer = TotalAPedir
                        Dim nro As Integer = 0
                        'recorro cada producto y veo que porcentaje representa del total
                        For Each indexProductoAgrupacion As Integer In ListArticulosAgrupacion
                            Dim PorcentajeAPedir As Double = Pedido.ListProveedores(indexProveedor).ListPedido(indexProductoAgrupacion).CantidadPedida * 100 / CantidadTotalProducto
                            Dim NuevaCantidadAPedir As Integer = CInt(Math.Round(TotalAPedir * PorcentajeAPedir / 100))
                            nro += 1

                            'hay casos en los que el ultimo articulo redondeado es menor que el resto y el promedio no da 
                            ' por lo que el ultimo articulo siempre tiene que tener el resto
                            If nro = ListArticulosAgrupacion.Count Then
                                NuevaCantidadAPedir = RestoAPedir
                            End If


                            If RestoAPedir >= NuevaCantidadAPedir Then
                                'saco del resto la cantidad actual a pedir
                                RestoAPedir -= NuevaCantidadAPedir
                            Else
                                'Si la nueva cantidad a pedir es mayor al resto, tengo que pedir el resto para que el resultado sea multiplo
                                NuevaCantidadAPedir = RestoAPedir
                            End If


                            Pedido.ListProveedores(indexProveedor).ListPedido(indexProductoAgrupacion).CantidadPedida = NuevaCantidadAPedir
                            Pedido.ListProveedores(indexProveedor).ListPedido(indexProductoAgrupacion).CantidadSugerida = NuevaCantidadAPedir
                        Next

                    End If
                End If
            Next
        Next

    End Sub
    Public Function ValidarAgrupacionesProveedor() As Boolean
        'este sub analiza las cantidades de los articulos en cada agrupacion y verifica que sean multiplos de la unidades por bulto
        For indexAgrupacion As Integer = 0 To ListAgrupaciones.Count - 1
            Dim currentAgrupacion As Agrupacion = ListAgrupaciones(indexAgrupacion)

            Dim ListArticulosAgrupacion As List(Of Integer) = New List(Of Integer)
            'recorro todos los articulos del proveedor
            For indexProducto As Integer = 0 To Pedido.ListProveedores(PunteroProveedores).ListPedido.Count - 1
                Dim CurrentProducto As DetallePedido = Pedido.ListProveedores(PunteroProveedores).ListPedido(indexProducto)
                'Verifico si el producto corresponde con la agrupacion actual
                If CurrentProducto.AgrupacionId = currentAgrupacion.AgrupacionId Then
                    'Guardo en la lista el puntero de los productos que me interesan
                    ListArticulosAgrupacion.Add(indexProducto)
                End If
            Next

            'recorro los productos que pertenecen a la agrupacion actual para realizar la validacion
            If ListArticulosAgrupacion.Count > 0 Then
                Dim CantidadTotalProducto As Integer = 0

                For Each indexProductoAgrupacion As Integer In ListArticulosAgrupacion
                    CantidadTotalProducto += Pedido.ListProveedores(PunteroProveedores).ListPedido(indexProductoAgrupacion).CantidadPedida
                Next

                'verifico si la cantidad total es multiplo de las unidades por bulto
                Dim resto As Double = CantidadTotalProducto Mod currentAgrupacion.UnidadesPorBulto
                If resto <> 0 Then
                    'Las cantidades pedidas no coinciden con las unidades por bulto
                    Return False
                End If
            End If
        Next
        Return True
    End Function
    Public Function GetMostrarCeros() As Boolean
        Dim currentProveedorId As String = GetCurrentProveedorId()
        Dim pos As Integer = Pedido.ListProveedores.FindIndex(Function(element) element.proveedorId = currentProveedorId)
        If pos >= 0 Then
            Return Pedido.ListProveedores(pos).MostrarCeros
        Else
            Return False
        End If

    End Function
    Public Function GetMostrarModificados() As Boolean
        Dim currentProveedorId As String = GetCurrentProveedorId()
        Dim pos As Integer = Pedido.ListProveedores.FindIndex(Function(element) element.proveedorId = currentProveedorId)
        If pos >= 0 Then
            Return Pedido.ListProveedores(pos).MostrarSoloModificados
        Else
            Return False
        End If
    End Function
    Public Function GetDatosCurrentProveedor() As List(Of DetallePedido)
        Dim ListProductos As List(Of DetallePedido) = New List(Of DetallePedido)
        Dim currentProveedorId As String = GetCurrentProveedorId()
        Dim pos As Integer = Pedido.ListProveedores.FindIndex(Function(element) element.proveedorId = currentProveedorId)
        If pos >= 0 Then
            ListProductos = Pedido.ListProveedores(pos).ListPedido
        End If
        Return ListProductos
    End Function

    Public Sub IniciarPedidoProveedores()
        Dim respuestaProductos As ResponseListProductos

        ' Cargo todos los articulos de todos los proveedores juntos
        respuestaProductos = fachadaBD.GetProductosProveedor(ListProveedores, ListDepositos, EsFabrica)

        ' Genero un nuevo pedido vacio con todos los productos
        If respuestaProductos.ConsultaExitosa = True Then
            ' Recorro los proveedores del listado global
            For Each Proveedor As Clasificacion In ListProveedores
                Dim NewProveedor As PedidoProveedor = New PedidoProveedor With {
                    .esFiscal = EsPedidoFacturado,
                    .MostrarCeros = True,
                    .proveedorId = Proveedor.ClasificacionProdId,
                    .proveedorNombre = Proveedor.DescripcionSinPrefijo,
                    .ListEmails = Proveedor.ListEmails
                }
                Pedido.ListProveedores.Add(NewProveedor)
            Next

            Dim currentProveedorId As String
            Dim ProdAgrupacionId As String
            Dim pos As Integer
            For Each ListProducto As ListProducto In respuestaProductos.rs
                currentProveedorId = ListProducto.ProveedorId

                'Busco si el producto actual pertenece a alguna agrupacion
                pos = ReLProductoAgrupacion.FindIndex(Function(element) element.ProductoId = ListProducto.ProductoId)
                If pos >= 0 Then
                    ProdAgrupacionId = ReLProductoAgrupacion(pos).AgrupacionId

                    'guardo en la agrupacion actual las propiedades de este producto
                    pos = ListAgrupaciones.FindIndex(Function(element) element.AgrupacionId = ProdAgrupacionId)
                    If pos >= 0 Then
                        If ListAgrupaciones(pos).KgPromedioUnidad = 0 Then
                            ListAgrupaciones(pos).KgPromedioUnidad = ListProducto.KgPromedioUnidad
                            ListAgrupaciones(pos).Unidad = ListProducto.UniPedido
                            ListAgrupaciones(pos).Packing = ListProducto.Packing
                            ListAgrupaciones(pos).UnidadesPorBulto = ListProducto.UnidadesPorBulto
                            ListAgrupaciones(pos).ColorAgrupacion = ListProducto.ColorProducto
                        End If
                    End If
                Else
                    ProdAgrupacionId = ""
                End If

                'SI EL PEDIDO ES REALIZADO POR UN CLIENTE PONGO EL STOCK Y LAS CANTIDADES EN CERO
                If EsPedidoCliente Then
                    ListProducto.Apedir = 0
                    ListProducto.Sugerida = 0
                    ListProducto.Stock = 0
                    ListProducto.Packing = 0
                End If

                Dim detalle As DetallePedido = New DetallePedido(ListProducto.ProductoId, ListProducto.Descripcion, ListProducto.Packing, ListProducto.Stock, ListProducto.Venta, ListProducto.UniPedido, ListProducto.Apedir, ListProducto.Sugerida, ListProducto.KgPromedioUnidad, ListProducto.UnidadesPorBulto, ListProducto.PrecioCompra, ListProducto.SeVendePorPeso, ProdAgrupacionId, "", ListProducto.SePidePorBulto, ListProducto.TipoUnidadMedidaId2)

                pos = Pedido.ListProveedores.FindIndex(Function(element) element.proveedorId = currentProveedorId)
                Pedido.ListProveedores(pos).ListPedido.Add(detalle)
            Next

            'Ordeno el objeto para que los productos agrupados esten juntos
            For n As Integer = Pedido.ListProveedores.Count - 1 To 0 Step -1
                'Pedido.ListProveedores(n).ListPedido.Sort(Function(x, y) x.AgrupacionId.CompareTo(y.AgrupacionId))
                Pedido.ListProveedores(n).ListPedido = Pedido.ListProveedores(n).ListPedido.OrderBy(Function(x) x.AgrupacionId).ToList()
            Next

        Else
            MsgBox(respuestaProductos.mensaje)
            WriteLogFile("Error en IniciarPedidoProveedores: " + respuestaProductos.mensaje)
        End If
    End Sub

    Public Sub GuardarPedidoProveedor(DbGridProveedores As DataGridView, MostrarCeros As Boolean, MostrarSoloModificados As Boolean)
        'Guardo en el log
        LogEvent.Add("Guardar Pedido Proveedor" + PunteroProveedores.ToString())

        'verifico cuales el proveedor actual
        Dim currentProveedorId As String = currentProveedor.ClasificacionProdId
        Dim pos As Integer
        pos = Pedido.ListProveedores.FindIndex(Function(element) element.proveedorId = currentProveedorId)

        If pos >= 0 Then
            Pedido.ListProveedores(pos).esFiscal = EsPedidoFacturado
            Pedido.ListProveedores(pos).MostrarCeros = MostrarCeros
            Pedido.ListProveedores(pos).MostrarSoloModificados = MostrarSoloModificados

            'guardo el precio del pedido en el proveedor
            Dim MontoPedido As Double = CalcularPrecioPedido(DbGridProveedores)
            Pedido.ListProveedores(pos).MontoPedido = MontoPedido

            'guardo el peso del pedido en el proveedor
            Dim TotalKgsPedido As Double = CalcularPesoPedido(DbGridProveedores)
            Pedido.ListProveedores(pos).PesoPedido = TotalKgsPedido
            Pedido.ListProveedores(pos).KilosMin = currentProveedor.KilosMin

            'Guardo esl establecimiento de distribucion
            Pedido.ListProveedores(pos).EstablecimientoIdDistribucion = currentProveedor.EstablecimientoIdDistribucion

            'si las cantidades fueron modificadas guardo la marca en el pedido
            Dim cantidadesModificadas = False
            If VerificarCantidades(DbGridProveedores) Then
                cantidadesModificadas = True
            End If
            Pedido.ListProveedores(pos).CantidadesModificadas = cantidadesModificadas

            'recorro toda la grilla y guardo en las variables globales el pedido
            For Each rowGrilla As DataGridViewRow In DbGridProveedores.Rows
                Dim Detalle As DetallePedido = New DetallePedido(rowGrilla.Cells("Codigo").Value, rowGrilla.Cells("Descripcion").Value, rowGrilla.Cells("Packing").Value, rowGrilla.Cells("Stock").Value, rowGrilla.Cells("Venta").Value, rowGrilla.Cells("Unidad").Value, rowGrilla.Cells("CantidadPedida").Value, rowGrilla.Cells("CantidadSugerida").Value, rowGrilla.Cells("KgPromedioUnidad").Value, rowGrilla.Cells("UnidadesPorBulto").Value, rowGrilla.Cells("PrecioCompra").Value, rowGrilla.Cells("SeVendePorPeso").Value, rowGrilla.Cells("AgrupacionId").Value, rowGrilla.Cells("Observaciones").Value, rowGrilla.Cells("SePidePorBulto").Value, rowGrilla.Cells("TipoUnidadMedidaId2").Value)
                Dim CodigoArticulo As String = rowGrilla.Cells("Codigo").Value
                Dim pos2 As Integer
                pos2 = Pedido.ListProveedores(pos).ListPedido.FindIndex(Function(element) element.Codigo = CodigoArticulo)
                If pos2 >= 0 Then
                    Pedido.ListProveedores(pos).ListPedido(pos2) = Detalle
                End If
            Next

        End If

    End Sub

    Public Function CalcularPesoPedido(DbGridProveedores As DataGridView) As Double
        Dim TotalKgsPedido As Double = 0
        Dim KgsArticulo As Double
        For CurrentRow As Integer = 0 To DbGridProveedores.Rows.Count - 1
            'Si el articulo se vende por unidad o no se pide por bulto NO lo multiplico por la unidades por bulto
            If (LCase(DbGridProveedores.Rows(CurrentRow).Cells("Unidad").Value) = "unidad" Or CBool(DbGridProveedores.Rows(CurrentRow).Cells("SePidePorBulto").Value) = False) Then
                KgsArticulo = DbGridProveedores.Rows(CurrentRow).Cells("KgPromedioUnidad").Value * Convert.ToInt32(DbGridProveedores.Rows(CurrentRow).Cells("CantidadPedida").Value)
            Else
                KgsArticulo = DbGridProveedores.Rows(CurrentRow).Cells("UnidadesPorBulto").Value * DbGridProveedores.Rows(CurrentRow).Cells("KgPromedioUnidad").Value * Convert.ToInt32(DbGridProveedores.Rows(CurrentRow).Cells("CantidadPedida").Value)
            End If
            TotalKgsPedido += KgsArticulo
        Next
        CalcularPesoPedido = TotalKgsPedido
    End Function
    Private Function CalcularPrecioPedido(DbGridProveedores As DataGridView) As Double
        Dim MontoTotalPedido As Double = 0
        Dim PrecioArticulo As Double

        For CurrentRow As Integer = 0 To DbGridProveedores.Rows.Count - 1
            ' Si el articulo se vende por peso multiplico la cantidad por el peso promedio
            If (LCase(DbGridProveedores.Rows(CurrentRow).Cells("SeVendePorPeso").Value) = True) Then
                PrecioArticulo = Math.Round(DbGridProveedores.Rows(CurrentRow).Cells("PrecioCompra").Value * DbGridProveedores.Rows(CurrentRow).Cells("KgPromedioUnidad").Value * Convert.ToInt32(DbGridProveedores.Rows(CurrentRow).Cells("CantidadPedida").Value), 2)
            Else
                PrecioArticulo = Math.Round(DbGridProveedores.Rows(CurrentRow).Cells("PrecioCompra").Value * Convert.ToInt32(DbGridProveedores.Rows(CurrentRow).Cells("CantidadPedida").Value), 2)
            End If

            ' Si el articulo se pide por bulto, debo multiplicarlo por la cantidad por bulto
            If (LCase(DbGridProveedores.Rows(CurrentRow).Cells("Unidad").Value) <> "unidad" And CBool(DbGridProveedores.Rows(CurrentRow).Cells("SePidePorBulto").Value) = True) Then
                PrecioArticulo = Math.Round(DbGridProveedores.Rows(CurrentRow).Cells("UnidadesPorBulto").Value * PrecioArticulo, 2)
            End If

            MontoTotalPedido += PrecioArticulo
        Next
        CalcularPrecioPedido = MontoTotalPedido
    End Function

    Public Function VerificarCantidadesPedido() As Boolean
        Dim CantidadesModificadas = False

        For Each Detalle As PedidoProveedor In Pedido.ListProveedores
            CantidadesModificadas = CantidadesModificadas Or Detalle.CantidadesModificadas
        Next
        Return CantidadesModificadas
    End Function
    Public Function GetListProveedoresModificados() As String
        Dim listProveedoresModificados As String = ""
        For Each Detalle As PedidoProveedor In Pedido.ListProveedores
            If Detalle.CantidadesModificadas Then listProveedoresModificados += " - " + Detalle.proveedorNombre + vbCrLf
        Next
        Return listProveedoresModificados
    End Function


    Public Function ValidarCantidadesPedido() As Boolean
        If Pedido.ListProveedores.Count > 0 Then
            Dim SumCantidades As Integer = 0
            For Each ProveedorPedido As PedidoProveedor In Pedido.ListProveedores
                For Each Detalle As DetallePedido In ProveedorPedido.ListPedido
                    SumCantidades += Detalle.CantidadPedida
                Next
            Next
            If SumCantidades > 0 Then
                Return True
            Else
                Return False
            End If
        Else
            'En el caso de que se haya eliminado un proveedor porque el peso no era suficiente
            Return False
        End If
    End Function


    Public Function VerificarCantidades(DbGridProveedores As DataGridView) As Boolean
        Dim respuesta As Boolean = False
        'recorro toda la grilla y guardo en las variables globales el pedido
        For Each rowGrilla As DataGridViewRow In DbGridProveedores.Rows

            If (IsNumeric(rowGrilla.Cells("CantidadPedida").Value)) Then

                Dim CantPedida As Double = Convert.ToDouble(rowGrilla.Cells("CantidadPedida").Value)
                Dim CantidadSugerida As Double = Convert.ToDouble(rowGrilla.Cells("CantidadSugerida").Value)

                If CantPedida = CantidadSugerida Then
                    respuesta = respuesta Or False
                Else
                    'verifico si el rango es menor al 5%
                    If CantPedida < (CantidadSugerida * 1.05) And CantPedida > (CantidadSugerida * 0.95) Then
                        respuesta = respuesta Or True
                    Else
                        respuesta = respuesta Or True
                    End If
                End If

            End If
        Next
        VerificarCantidades = respuesta
    End Function
    Public Sub EliminarProveedoresPesoInsuficiente()

        For n As Integer = Pedido.ListProveedores.Count - 1 To 0 Step -1
            Dim Detalle As PedidoProveedor = Pedido.ListProveedores(n)
            Dim KilosMinimos = Detalle.KilosMin * 0.95 'Le dejo un margen del 5% a los kilos minimos
            If Detalle.PesoPedido < KilosMinimos Then
                'si el peso del proveedor es menor al minimo lo elimino del pedido
                MsgBox("Se ha eliminado el proveedor " + Detalle.proveedorNombre + " porque no cumple con el minimo de kilos requeridos", MsgBoxStyle.Information + MsgBoxStyle.OkOnly, GlobalSettings.TituloMensajes)
                Pedido.ListProveedores.RemoveAt(n)
            End If
        Next

    End Sub


    Public Sub EliminarRegistrosVacios()
        ' Recorro todo el pedido y los registros con cantidad menor o igual a cero los elimino
        For n As Integer = Pedido.ListProveedores.Count - 1 To 0 Step -1
            Dim Detalle As PedidoProveedor = Pedido.ListProveedores(n)
            For i As Integer = Detalle.ListPedido.Count - 1 To 0 Step -1
                Dim DetallePedido As DetallePedido = Pedido.ListProveedores(n).ListPedido(i)
                If DetallePedido.CantidadPedida <= 0 And DetallePedido.CantidadSugerida <= 0 Then
                    Pedido.ListProveedores(n).ListPedido.RemoveAt(i)
                End If
            Next

            ' Si ya no quedaron registros para el proveedor actual lo elimino del pedido
            If Pedido.ListProveedores(n).ListPedido.Count = 0 Then
                Pedido.ListProveedores.RemoveAt(n)
            End If
        Next
    End Sub
    Public Sub EliminarArchivosProveedores()
        Dim ExcelFilename As String
        Try
            ' Dejo que el sistema ejecute los eventos para que no queden los archivos bloqueados
            Application.DoEvents()

            'Elimino el archivo unificado de notificaciones
            If (My.Computer.FileSystem.DirectoryExists(PathArchivosExcel + "Notificaciones")) Then
                ' Elimino el archivo unificado
                ExcelFilename = PathArchivosExcel + "Notificaciones\Agrupado - " + GetRazonSocialPedido() + " - " + StrDepositos + " - " + Date.Today.ToString("dd.MM.yyyy") + ".xlsx"
                'si el archivo existe lo elimino
                If (My.Computer.FileSystem.FileExists(ExcelFilename)) Then
                    My.Computer.FileSystem.DeleteFile(ExcelFilename)
                End If
                'Elimino la carpeta
                My.Computer.FileSystem.DeleteDirectory(PathArchivosExcel + "Notificaciones", FileIO.DeleteDirectoryOption.DeleteAllContents)
            End If

            'Elimino los archivos unificados por mail
            If Pedido.ListEmails.Count > 0 Then
                For Each EmailList As EmailList In Pedido.ListEmails
                    ' Elimino el archivo unificado
                    ExcelFilename = PathArchivosExcel + EmailList.Email + "\Agrupado - " + GetRazonSocialPedido() + " - " + StrDepositos + " - " + Date.Today.ToString("dd.MM.yyyy") + ".xlsx"
                    'si el archivo existe lo elimino
                    If (My.Computer.FileSystem.FileExists(ExcelFilename)) Then
                        My.Computer.FileSystem.DeleteFile(ExcelFilename)
                    End If
                    'Si existe la carpeta del mail la eliminio
                    If (My.Computer.FileSystem.DirectoryExists(PathArchivosExcel + EmailList.Email)) Then
                        My.Computer.FileSystem.DeleteDirectory(PathArchivosExcel + EmailList.Email, FileIO.DeleteDirectoryOption.DeleteAllContents)
                    End If
                Next
            End If

            ' Elimino los archivos individuales
            If Pedido.ListProveedores.Count > 0 Then
                For Each Proveedor As PedidoProveedor In Pedido.ListProveedores
                    ExcelFilename = PathArchivosExcel + GetRazonSocialPedido() + " - " + Proveedor.proveedorNombre + " - " + StrDepositos + " - " + Date.Today.ToString("dd.MM.yyyy") + ".xlsx"
                    'si el archivo existe lo elimino
                    If (My.Computer.FileSystem.FileExists(ExcelFilename)) Then
                        My.Computer.FileSystem.DeleteFile(ExcelFilename)
                    End If
                Next
            End If

            'Elimino el archivo de modificaciones
            ExcelFilename = PathArchivosExcel + "Modificados - " + GetRazonSocialPedido() + " - " + StrDepositos + " - " + Date.Today.ToString("dd.MM.yyyy") + ".xlsx"
            'si el archivo existe lo elimino
            If (My.Computer.FileSystem.FileExists(ExcelFilename)) Then
                My.Computer.FileSystem.DeleteFile(ExcelFilename)
            End If

            'Elimino el archivo de analisis de datos
            ExcelFilename = PathArchivosExcel + "Analisis Datos - " + GetRazonSocialPedido() + " - " + StrDepositos + " - " + Date.Today.ToString("dd.MM.yyyy") + ".xlsx"
            'si el archivo existe lo elimino
            If (My.Computer.FileSystem.FileExists(ExcelFilename)) Then
                My.Computer.FileSystem.DeleteFile(ExcelFilename)
            End If

            'Elimino la carpeta tmp del establecimiento
            If (My.Computer.FileSystem.DirectoryExists(PathArchivosExcel)) Then
                My.Computer.FileSystem.DeleteDirectory(PathArchivosExcel, FileIO.DeleteDirectoryOption.DeleteAllContents)
            End If

        Catch es As UnauthorizedAccessException
            'verifico si el error es porque no tiene acceso a eliminar los archivos
            'MsgBox("No tienes permiso " + vbCrLf + es.Message, MsgBoxStyle.Critical, GlobalSettings.TituloMensajes)
            Dim strError As String = "GenerarArchivosExcel Error No tienes permisos - Message: " + es.Message + vbCrLf + JsonConvert.SerializeObject(es)
            'GlobalSettings.WriteLogFile(strError)
        Catch ex As Exception
            MsgBox("Ocurrio un error al eliminar los archivos del pedido " + vbCrLf + ex.Message, vbCritical + vbOKOnly, GlobalSettings.TituloMensajes)
            Dim strError As String = "EliminarArchivosProveedores - Message: " + ex.Message + vbCrLf + JsonConvert.SerializeObject(ex)
            WriteLogFile(strError)
        End Try
    End Sub


    Public Function IsValidEmailFormat(ByVal s As String) As Boolean
        Return Regex.IsMatch(s, "^([0-9a-zA-Z]([-\.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$")
    End Function

    Public Shared Sub WriteLogFile(strError As String)

        Dim myFilePath As String = My.Application.Info.DirectoryPath + "\error_log_ " + Today.Day.ToString() + "." + Today.Month.ToString() + "." + Today.Year.ToString() + " " + Now.Hour.ToString() + "." + Now.Minute.ToString() + ".log"

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
            sw.WriteLine("DebugMode: " + DebugMode.ToString())
            sw.WriteLine("UsuarioId: " + UsuarioId)
            sw.WriteLine("EstablecimientoId: " + CurrentEstablecimiento.EstablecimientoId)
            sw.WriteLine("Establecimiento: " + CurrentEstablecimiento.Descripcion)
            sw.WriteLine("RazonSocial: " + RazonSocial)
            sw.WriteLine("EsFabrica: " + EsFabrica.ToString())
            sw.WriteLine("EsAdministrador: " + EsAdministrador.ToString())
            sw.WriteLine("EstablecimientoPropio: " + CurrentEstablecimiento.EsPropio.ToString())
            sw.WriteLine("PunteroProveedores: " + PunteroProveedores.ToString())
            sw.WriteLine("List Depositos: " + JsonConvert.SerializeObject(ListDepositos))
            sw.WriteLine("List Establecimientos: " + JsonConvert.SerializeObject(ListEstablecimientos))
            sw.WriteLine("")
            sw.WriteLine("NroPedido: " + NroPedido)
            sw.WriteLine("List proveedores Pedido: " + JsonConvert.SerializeObject(Pedido.ListProveedores))
            sw.WriteLine("List Emails Pedido: " + JsonConvert.SerializeObject(Pedido.ListEmails))
            sw.WriteLine("")

            'Escribo todos las configuraciones globales para poder ubicar el error
            sw.WriteLine("--------------- Configuration -----------------")
            sw.WriteLine("UrlWebservice: " + GlobalSettings.UrlWebservice)
            sw.WriteLine("UrlWebserviceTest: " + GlobalSettings.UrlWebserviceTest)
            sw.WriteLine("CUITEnsemble: " + GlobalSettings.CUITEnsemble)
            sw.WriteLine("NombreBaseEnsemble: " + GlobalSettings.NombreBaseEnsemble)
            sw.WriteLine("TipoNotificacionId: " + GlobalSettings.TipoNotificacionId)
            sw.WriteLine("SMTP: " + GlobalSettings.SMTP)
            sw.WriteLine("MailFromAddress: " + GlobalSettings.MailFromAddress)
            sw.WriteLine("MailUsername: " + GlobalSettings.MailUsername)
            sw.WriteLine("MailEnableSSL: " + GlobalSettings.MailEnableSSL.ToString())
            sw.WriteLine("MailPort: " + GlobalSettings.MailPort.ToString())
            sw.WriteLine("MailPortGmail: " + GlobalSettings.MailPortGmail.ToString())
            sw.WriteLine("MailSubject: " + GlobalSettings.MailSubject)
            sw.WriteLine("")


            'Escribo la informacionde la excepcion
            sw.WriteLine("--------------- Exception ----------------------")
            sw.WriteLine(strError)
        End Using

    End Sub
    Public Shared Function EnviarMailCritico(ByVal strContenido As String) As Boolean
        Dim contenido As String = ""
        contenido += "UsuarioId: " + UsuarioId + vbCrLf
        contenido += "EstablecimientoId: " + CurrentEstablecimiento.EstablecimientoId + vbCrLf
        contenido += "Establecimiento: " + CurrentEstablecimiento.Descripcion + vbCrLf
        contenido += "RazonSocial: " + RazonSocial + vbCrLf
        contenido += "NroPedido: " + NroPedido + vbCrLf
        contenido += "Contenido: " + vbCrLf
        contenido += strContenido

        Return EnviarMail(GlobalSettings.ListDestinatariosCriticos, GlobalSettings.SubjectCritico, contenido)
    End Function

    Public Function GenerarArchivosExcel() As Boolean
        Dim objApp As Excel.Application
        Dim objBooks As Excel.Workbooks
        Dim bOk As Boolean
        Dim datestart As Date = Date.Now

        LogEvent.Add("Generando archivos excel...")

        ' Create a new instance of Excel and start a new workbook.
        objApp = New Excel.Application()
        objBooks = objApp.Workbooks

        'Return control of Excel to the user.
        'objApp.Visible = False
        'objApp.UserControl = False
        'objApp.DisplayAlerts = False

        bOk = GenerarArchivosProveedores(objBooks)

        Try
            'Verifico si algun workbook quedo abierto
            'For Each CurrentWorkbook As Excel.Workbook In objBooks
            '    CurrentWorkbook.Saved = True
            '    CurrentWorkbook.Close()
            'Next

            objApp.Quit()
            ReleaseObject(objApp)
            End_Excel_App(datestart, Date.Now) ' This closes excel proces

        Catch ex As Exception
            'Si el excel no tiene licencia cuando quiere cerrar la app tira un error, por eso debo continuar igual
            'Dim strError As String = "GenerarArchivosExcel Error al cerrar al aplicacion - Message: " + ex.Message + vbCrLf + JsonConvert.SerializeObject(ex)
            'GlobalSettings.WriteLogFile(strError)
            ReleaseObject(objApp)
            End_Excel_App(datestart, Date.Now) ' This closes excel proces

            bOk = True
        End Try

        If bOk = False Then
            If MsgBox("Ocurrio un problema al Generar los archivos de excel del pedido, si lo desea puede reintentar el proceso o continuar sin los archivos", vbRetryCancel, GlobalSettings.TituloMensajes) = MsgBoxResult.Retry Then
                bOk = GenerarArchivosExcel()
            End If
        End If

        Return bOk

    End Function

    Private Sub CerrarLibroExcel(ByRef objBook As Excel.Workbook)
        'Clean up a little.
        'objSheet = Nothing
        'objSheets = Nothing
        objBook.Close(SaveChanges:=False)
        objBook = Nothing

    End Sub

    Private Function GenerarArchivosProveedores(ByRef objBooksXL As Excel.Workbooks) As Boolean
        'primero obtengo la lista de emails del pedido y los proveedores asociados a esta lista
        AgruparMailsProveedores()

        If Pedido.ListEmails.Count > 0 Then
            For Each EmailList As EmailList In Pedido.ListEmails
                'Genero un archivo unificado con todos los proveedores de este email
                LogEvent.Add("Generando archivo excel unificado para el mail " + EmailList.Email)
                If GenerarArchivoUnificadoExcelporEmail(objBooksXL, EmailList) = False Then
                    'Si alguno de los archivos dio error paro el procedimiento
                    Return False
                End If
            Next
        End If

        'Genero un archivo unificado para las notificaciones
        LogEvent.Add("Generando archivo excel unificado para las notificaciones")
        If GenerarArchivoUnificadoExcelNotificaciones(objBooksXL) = False Then
            Return False
        End If

        'Genero un archivo por cada proveedor
        If Pedido.ListProveedores.Count > 0 Then
            For Each Proveedor As PedidoProveedor In Pedido.ListProveedores
                'si las cantidades pedidas son todas cero no genero el excel
                If VerificarCantidadesPedidas(Proveedor) Then
                    LogEvent.Add("Generando archivo individual para el proveedor " + Proveedor.proveedorNombre)
                    Dim bMostrarObservaciones As Boolean = MostrarObservaciones(Proveedor)
                    If GenerarArchivoExcelProveedor(objBooksXL, Proveedor, bMostrarObservaciones) = False Then
                        Return False
                    End If
                End If
            Next
        End If

        LogEvent.Add("Generando archivo de Analisis de datos")
        If GenerarArchivoExcelAnalisis(objBooksXL) = False Then
            Return False
        End If

        'Genero un archivo solo con los registros modificados
        'If checkValoresModificados() Then
        'LogEvent.Add("Generando archivo de modificaciones")
        'If GenerarArchivoExcelModificados(objBooksXL) = False Then
        'Return False
        'End If
        'End If

        'Si ningun archivo genero error devuelvo true en la funcion
        Return True
    End Function

    Public Function CheckValoresModificados() As Boolean
        If Pedido.ListProveedores.Count > 0 Then
            For Each Proveedor As PedidoProveedor In Pedido.ListProveedores
                For Each DetallePedido As DetallePedido In Proveedor.ListPedido
                    'si la cantidad pedida es 20% menor a la cantidad sugerida o la cantidad pedida es 50% mayor a la cantidad sugerida
                    If DetallePedido.CantidadPedida < (DetallePedido.CantidadSugerida * 0.8) Or DetallePedido.CantidadPedida >= (DetallePedido.CantidadSugerida * 1.5) Then
                        Return True
                    End If
                Next
            Next
        End If
        Return False
    End Function


    Private Function GenerarArchivoUnificadoExcelNotificaciones(ByRef objBooks As Excel.Workbooks) As Boolean
        Try
            Dim objBook As Excel.Workbook
            Dim objSheets As Excel.Sheets
            Dim objSheetXL As Excel.Worksheet

            objBook = objBooks.Add
            objSheets = objBook.Worksheets
            objSheetXL = objSheets(1)

            nroRowExcel = 1
            GenerarEncabezadoUnificado(objSheetXL)

            'asigno todos los proveedores para el envio del mail
            For Each Detalle As PedidoProveedor In Pedido.ListProveedores
                'verifico que las cantidades no sean cero en todas las filas
                If VerificarCantidadesPedidas(Detalle) Then
                    Dim bMostrarObservaciones As Boolean = MostrarObservaciones(Detalle)
                    GenerarDetalleUnificado(objSheetXL, Detalle, bMostrarObservaciones)
                    nroRowExcel += 1
                End If
            Next

            'Guardo los archivos unificados dentro de una carpeta
            If (My.Computer.FileSystem.DirectoryExists(PathArchivosExcel + "Notificaciones") = False) Then
                My.Computer.FileSystem.CreateDirectory(PathArchivosExcel + "Notificaciones")
            End If

            'guardo la planilla para despues adjuntarla en el mail
            Dim ExcelFilename As String = PathArchivosExcel + "Notificaciones\Agrupado - " + GetRazonSocialPedido() + " - " + StrDepositos + " - " + Date.Today.ToString("dd.MM.yyyy") + ".xlsx"

            'si el archivo existe lo elimino
            If (My.Computer.FileSystem.FileExists(ExcelFilename)) Then
                My.Computer.FileSystem.DeleteFile(ExcelFilename)
            End If

            objBook.SaveAs(ExcelFilename)

            CerrarLibroExcel(objBook)

            Return True
        Catch ex As Exception
            Dim strError As String = "GenerarArchivoUnificadoExcelNotificaciones - Message: " + ex.Message + vbCrLf + JsonConvert.SerializeObject(ex)
            WriteLogFile(strError)

            Return False
        End Try
    End Function

    Private Function GenerarArchivoUnificadoExcelporEmail(ByRef objBooks As Excel.Workbooks, ByVal EmailList As EmailList) As Boolean
        Try
            Dim objBook As Excel.Workbook
            Dim objSheets As Excel.Sheets
            Dim objSheetXL As Excel.Worksheet

            objBook = objBooks.Add
            objSheets = objBook.Worksheets
            objSheetXL = objSheets(1)

            nroRowExcel = 1
            GenerarEncabezadoUnificado(objSheetXL)

            For Each NombreProveedor As String In EmailList.ListProveedores
                Dim pos As Integer
                pos = Pedido.ListProveedores.FindIndex(Function(element) element.proveedorNombre = NombreProveedor)
                If pos >= 0 Then
                    Dim Detalle As PedidoProveedor = Pedido.ListProveedores(pos)
                    ' verifica que las cantidades no sean todas cero
                    If VerificarCantidadesPedidas(Detalle) Then
                        Dim bMostrarObservaciones As Boolean = MostrarObservaciones(Detalle)
                        GenerarDetalleUnificado(objSheetXL, Detalle, bMostrarObservaciones)
                        nroRowExcel += 1
                    End If
                End If
            Next

            'Guardo los archivos unificados dentro de una carpeta
            If (My.Computer.FileSystem.DirectoryExists(PathArchivosExcel + EmailList.Email) = False) Then
                My.Computer.FileSystem.CreateDirectory(PathArchivosExcel + EmailList.Email)
            End If

            'guardo la planilla para despues adjuntarla en el mail
            Dim ExcelFilename As String = PathArchivosExcel + EmailList.Email + "\Agrupado - " + GetRazonSocialPedido() + " - " + StrDepositos + " - " + Date.Today.ToString("dd.MM.yyyy") + ".xlsx"

            'si el archivo existe lo elimino
            If (My.Computer.FileSystem.FileExists(ExcelFilename)) Then
                My.Computer.FileSystem.DeleteFile(ExcelFilename)
            End If

            LogEvent.Add("Grabando el archivo excel para el mail " + EmailList.Email)

            objBook.SaveAs(ExcelFilename)

            CerrarLibroExcel(objBook)

            LogEvent.Add("Cerrando la instancia de excel para el mail " + EmailList.Email)
            Return True
        Catch ex As Exception
            Dim strError As String = "GenerarArchivoUnificadoExcelporEmail - Message: " + ex.Message + vbCrLf + JsonConvert.SerializeObject(ex)
            WriteLogFile(strError)

            Return False
        End Try
    End Function

    Private Function GenerarArchivoExcelAnalisis(ByRef objBooks As Excel.Workbooks) As Boolean
        Try
            Dim objBook As Excel.Workbook
            Dim objSheets As Excel.Sheets
            Dim objSheetXL As Excel.Worksheet

            objBook = objBooks.Add
            objSheets = objBook.Worksheets
            objSheetXL = objSheets(1)

            nroRowExcel = 1
            GenerarEncabezadoModificado(objSheetXL)

            GenerarEncabezadoTablaAnalisis(objSheetXL)

            For Each Detalle As PedidoProveedor In Pedido.ListProveedores
                GenerarDetalleAnalisis(objSheetXL, Detalle)
            Next

            'guardo la planilla para despues adjuntarla en el mail
            Dim ExcelFilename As String = PathArchivosExcel + "Analisis Datos - " + GetRazonSocialPedido() + " - " + StrDepositos + " - " + Date.Today.ToString("dd.MM.yyyy") + ".xlsx"
            'si el archivo existe lo elimino
            If (My.Computer.FileSystem.FileExists(ExcelFilename)) Then
                My.Computer.FileSystem.DeleteFile(ExcelFilename)
            End If

            objBook.SaveAs(ExcelFilename)

            CerrarLibroExcel(objBook)
            Return True
        Catch ex As Exception
            Dim strError As String = "GenerarArchivoExcelAnalisis - Message: " + ex.Message + vbCrLf + JsonConvert.SerializeObject(ex)
            WriteLogFile(strError)
            Return False
        End Try


    End Function

    Private Function GenerarArchivoExcelModificados(ByRef objBooks As Excel.Workbooks) As Boolean
        Try
            Dim objBook As Excel.Workbook
            Dim objSheets As Excel.Sheets
            Dim objSheetXL As Excel.Worksheet

            objBook = objBooks.Add
            objSheets = objBook.Worksheets
            objSheetXL = objSheets(1)

            nroRowExcel = 1
            GenerarEncabezadoModificado(objSheetXL)

            GenerarEncabezadoTablaModificados(objSheetXL)

            For Each Detalle As PedidoProveedor In Pedido.ListProveedores
                GenerarDetalleModificados(objSheetXL, Detalle)
            Next

            'guardo la planilla para despues adjuntarla en el mail
            Dim ExcelFilename As String = PathArchivosExcel + "Modificados - " + GetRazonSocialPedido() + " - " + StrDepositos + " - " + Date.Today.ToString("dd.MM.yyyy") + ".xlsx"
            'si el archivo existe lo elimino
            If (My.Computer.FileSystem.FileExists(ExcelFilename)) Then
                My.Computer.FileSystem.DeleteFile(ExcelFilename)
            End If

            objBook.SaveAs(ExcelFilename)

            CerrarLibroExcel(objBook)
            Return True
        Catch ex As Exception
            Dim strError As String = "GenerarArchivoExcelModificados - Message: " + ex.Message + vbCrLf + JsonConvert.SerializeObject(ex)
            WriteLogFile(strError)
            Return False
        End Try

    End Function

    Private Function GenerarArchivoExcelProveedor(ByRef objBooks As Excel.Workbooks, ByVal Detalle As PedidoProveedor, MostrarObservaciones As Boolean) As Boolean
        Try
            Dim objBook As Excel.Workbook
            Dim objSheets As Excel.Sheets
            Dim objSheetXL As Excel.Worksheet

            objBook = objBooks.Add
            objSheets = objBook.Worksheets
            objSheetXL = objSheets(1)

            nroRowExcel = 1
            GenerarEncabezadoProveedor(objSheetXL, Detalle)

            GenerarDetalleProveedor(objSheetXL, Detalle, MostrarObservaciones)

            'guardo la planilla para despues adjuntarla en el mail
            Dim ExcelFilename As String = PathArchivosExcel + GetRazonSocialPedido() + " - " + Detalle.proveedorNombre + " - " + StrDepositos + " - " + Date.Today.ToString("dd.MM.yyyy") + ".xlsx"
            'si el archivo existe lo elimino
            If (My.Computer.FileSystem.FileExists(ExcelFilename)) Then
                My.Computer.FileSystem.DeleteFile(ExcelFilename)
            End If

            objBook.SaveAs(ExcelFilename)

            CerrarLibroExcel(objBook)

            Return True
        Catch ex As Exception
            Dim strError As String = "GenerarArchivoExcelProveedor - Message: " + ex.Message + vbCrLf + JsonConvert.SerializeObject(ex)
            WriteLogFile(strError)

            Return False
        End Try
    End Function


    Private Sub GenerarDetalleAnalisis(ByRef objSheet As Excel.Worksheet, Detalle As PedidoProveedor)
        Dim range As Excel.Range
        Dim Kgs As Double
        Dim TotalKgs As Double
        Dim CantRegistrosModificados As Integer = 0

        For Each DetallePedido As DetallePedido In Detalle.ListPedido
            objSheet.Cells(nroRowExcel, 1) = "'" + Detalle.proveedorNombre
            objSheet.Cells(nroRowExcel, 2) = "'" + DetallePedido.Codigo
            objSheet.Cells(nroRowExcel, 3) = DetallePedido.Descripcion
            objSheet.Cells(nroRowExcel, 4) = DetallePedido.Stock
            objSheet.Cells(nroRowExcel, 5) = DetallePedido.Packing
            objSheet.Cells(nroRowExcel, 6) = DetallePedido.CantidadPedida
            objSheet.Cells(nroRowExcel, 7) = DetallePedido.CantidadSugerida
            objSheet.Cells(nroRowExcel, 8) = DetallePedido.Unidad

            objSheet.Cells(nroRowExcel, 15) = DetallePedido.Observaciones

            'Si el articulo se vende por unidad o no se pide por bulto NO lo multiplico por la unidades por bulto
            If (LCase(DetallePedido.Unidad) = "unidad" Or DetallePedido.SePidePorBulto = False) Then
                Kgs = DetallePedido.KgPromedioUnidad * Convert.ToInt32(DetallePedido.CantidadPedida)
            Else
                Kgs = DetallePedido.UnidadesPorBulto * DetallePedido.KgPromedioUnidad * Convert.ToInt32(DetallePedido.CantidadPedida)
            End If

            objSheet.Cells(nroRowExcel, 11) = Kgs
            TotalKgs += Kgs
            nroRowExcel += 1
            CantRegistrosModificados += 1
        Next
        If CantRegistrosModificados > 0 Then
            range = objSheet.Range("A" + (nroRowExcel - (CantRegistrosModificados + 1)).ToString(), Reflection.Missing.Value)
            range = range.Resize(CantRegistrosModificados + 1, 15)
            range.Borders.LineStyle = Excel.XlLineStyle.xlContinuous
            range.Font.Size = 12

            range = objSheet.Range("A" + (nroRowExcel - CantRegistrosModificados).ToString(), Reflection.Missing.Value)
            range = range.Resize(CantRegistrosModificados, 15)
            range.RowHeight = 22


        End If

        'lluego de generado todo el detalle hago el autofit
        range = objSheet.Range("A1", Reflection.Missing.Value)
        range = range.Resize(nroRowExcel, 15)
        range.Columns.AutoFit()

    End Sub
    Private Sub GenerarEncabezadoUnificado(ByRef objSheet As Excel.Worksheet)
        Dim range As Excel.Range

        objSheet.Cells(nroRowExcel, 1) = "Nro de pedido"
        objSheet.Cells(nroRowExcel, 2) = NroPedido

        nroRowExcel += 1

        If EsPedidoCliente Then
            objSheet.Cells(nroRowExcel, 1) = "Cliente:"
        Else
            objSheet.Cells(nroRowExcel, 1) = "Empresa:"
        End If

        objSheet.Cells(nroRowExcel, 2) = GetRazonSocialPedido()
        objSheet.Cells(nroRowExcel, 4) = GetCUITPedido()
        objSheet.Cells(nroRowExcel, 6) = "Bultos:"
        objSheet.Cells(nroRowExcel, 8) = "Separo:"

        nroRowExcel += 1

        Dim strDepos As String = ""
        For Each Depo As Deposito In ListDepositos
            strDepos += Depo.Descripcion + ", "
        Next
        'Elimino el ultimo ", "
        strDepos = strDepos.Substring(0, strDepos.Length - 2)

        objSheet.Cells(nroRowExcel, 1) = "Deposito:"
        objSheet.Cells(nroRowExcel, 2) = strDepos
        If EsPedidoCliente Then
            'Imprimo la direccion del cliente
            objSheet.Cells(nroRowExcel, 3) = GetDireccionCliente()
        End If
        objSheet.Cells(nroRowExcel, 6) = "Canastos:"
        objSheet.Cells(nroRowExcel, 8) = "Peso:"

        nroRowExcel += 1
        objSheet.Cells(nroRowExcel, 1) = "Fecha:"
        objSheet.Cells(nroRowExcel, 2) = Date.Today.Day.ToString() + "/" + Date.Today.Month.ToString() + "/" + Date.Today.Year.ToString()
        objSheet.Cells(nroRowExcel, 6) = "Pallets:"


        range = objSheet.Range("A" + (nroRowExcel - 3).ToString(), Reflection.Missing.Value)
        range = range.Resize(4, 10)
        range.Font.Bold = True

        nroRowExcel += 2

    End Sub

    Private Sub GenerarEncabezadoModificado(ByRef objSheet As Excel.Worksheet)
        Dim range As Excel.Range

        If EsPedidoCliente Then
            objSheet.Cells(nroRowExcel, 1) = "Cliente:"
        Else
            objSheet.Cells(nroRowExcel, 1) = "Empresa:"
        End If
        objSheet.Cells(nroRowExcel, 2) = GetRazonSocialPedido()

        objSheet.Cells(nroRowExcel, 10) = NroPedido
        range = objSheet.Range("G" + nroRowExcel.ToString(), Reflection.Missing.Value)
        range = range.Resize(1, 3)
        range.Merge()
        objSheet.Cells(nroRowExcel, 7) = "Nro de pedido"
        nroRowExcel += 1

        Dim strDepos As String = ""
        For Each Depo As Deposito In ListDepositos
            strDepos += Depo.Descripcion + ", "
        Next
        'Elimino el ultimo ", "
        strDepos = strDepos.Substring(0, strDepos.Length - 2)

        objSheet.Cells(nroRowExcel, 1) = "Deposito:"
        objSheet.Cells(nroRowExcel, 2) = strDepos
        If EsPedidoCliente Then
            'Imprimo la direccion del cliente
            objSheet.Cells(nroRowExcel, 4) = GetDireccionCliente()
        End If
        nroRowExcel += 1

        objSheet.Cells(nroRowExcel, 1) = "Fecha:"
        objSheet.Cells(nroRowExcel, 2) = Date.Today.Day.ToString() + "/" + Date.Today.Month.ToString() + "/" + Date.Today.Year.ToString()
        nroRowExcel += 1

        range = objSheet.Range("A" + (nroRowExcel - 3).ToString(), Reflection.Missing.Value)
        range = range.Resize(3, 10)
        range.Font.Bold = True
        range.Font.Size = 14

        'Dejo una linea en blanco
        nroRowExcel += 1

    End Sub

    Private Sub GenerarEncabezadoProveedor(ByRef objSheet As Excel.Worksheet, Detalle As PedidoProveedor)
        Dim range As Excel.Range

        If EsPedidoCliente Then
            objSheet.Cells(nroRowExcel, 1) = "Cliente:"
        Else
            objSheet.Cells(nroRowExcel, 1) = "Empresa:"
        End If
        objSheet.Cells(nroRowExcel, 2) = GetRazonSocialPedido()


        objSheet.Cells(nroRowExcel, 10) = NroPedido
        range = objSheet.Range("G" + nroRowExcel.ToString(), Reflection.Missing.Value)
        range = range.Resize(1, 3)
        range.Merge()
        objSheet.Cells(nroRowExcel, 7) = "Nro de pedido"
        nroRowExcel += 1

        Dim strDepos As String = ""
        For Each Depo As Deposito In ListDepositos
            strDepos += Depo.Descripcion + ", "
        Next
        'Elimino el ultimo ", "
        strDepos = strDepos.Substring(0, strDepos.Length - 2)

        objSheet.Cells(nroRowExcel, 1) = "Deposito:"
        objSheet.Cells(nroRowExcel, 2) = strDepos

        If EsPedidoCliente Then
            'Imprimo la direccion del cliente
            objSheet.Cells(nroRowExcel, 4) = GetDireccionCliente()
        End If

        nroRowExcel += 1

        objSheet.Cells(nroRowExcel, 1) = "Fecha:"
        objSheet.Cells(nroRowExcel, 2) = Date.Today.Day.ToString() + "/" + Date.Today.Month.ToString() + "/" + Date.Today.Year.ToString()
        nroRowExcel += 1

        objSheet.Cells(nroRowExcel, 1) = "Clasificacion:"
        objSheet.Cells(nroRowExcel, 2) = Detalle.proveedorNombre
        nroRowExcel += 1

        range = objSheet.Range("A" + (nroRowExcel - 4).ToString(), Reflection.Missing.Value)
        range = range.Resize(5, 10)
        range.Font.Bold = True
        range.Font.Size = 14

        'Dejo una linea en blanco
        nroRowExcel += 1

    End Sub
    Private Sub GenerarEncabezadoTablaModificados(ByRef objSheet As Excel.Worksheet)
        Dim range As Excel.Range
        Dim cantCols As Integer

        objSheet.Cells(nroRowExcel, 1) = "Clasificacion"
        objSheet.Cells(nroRowExcel, 2) = "Cod. Producto"
        objSheet.Cells(nroRowExcel, 3) = "Descripcion"
        objSheet.Cells(nroRowExcel, 4) = "Cant. Pedida"
        objSheet.Cells(nroRowExcel, 5) = "Cant. Sugerida"
        objSheet.Cells(nroRowExcel, 6) = "Un. Medida"
        objSheet.Cells(nroRowExcel, 7) = "Cant. Real"
        objSheet.Cells(nroRowExcel, 8) = "Kg. Reales"
        objSheet.Cells(nroRowExcel, 9) = "Kg. Pedidos"
        objSheet.Cells(nroRowExcel, 10) = "Promedio"
        objSheet.Cells(nroRowExcel, 11) = "Lote"
        objSheet.Cells(nroRowExcel, 12) = "Operario"
        objSheet.Cells(nroRowExcel, 13) = "Observaciones"
        cantCols = 13

        range = objSheet.Range("A" + nroRowExcel.ToString(), Reflection.Missing.Value)
        range = range.Resize(1, cantCols)
        range.Font.Bold = True
        range.Font.Color = Color.White
        range.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Black)

        range.Columns.AutoFit()

        nroRowExcel += 1

    End Sub
    Private Sub GenerarEncabezadoTablaAnalisis(ByRef objSheet As Excel.Worksheet)
        Dim range As Excel.Range
        Dim cantCols As Integer

        objSheet.Cells(nroRowExcel, 1) = "Clasificacion"
        objSheet.Cells(nroRowExcel, 2) = "Cod. Producto"
        objSheet.Cells(nroRowExcel, 3) = "Descripcion"
        objSheet.Cells(nroRowExcel, 4) = "Stock Actual"
        objSheet.Cells(nroRowExcel, 5) = "Stock Necesario"
        objSheet.Cells(nroRowExcel, 6) = "Cant. Pedida"
        objSheet.Cells(nroRowExcel, 7) = "Cant. Sugerida"
        objSheet.Cells(nroRowExcel, 8) = "Un. Medida"
        objSheet.Cells(nroRowExcel, 9) = "Cant. Real"
        objSheet.Cells(nroRowExcel, 10) = "Kg. Reales"
        objSheet.Cells(nroRowExcel, 11) = "Kg. Pedidos"
        objSheet.Cells(nroRowExcel, 12) = "Promedio"
        objSheet.Cells(nroRowExcel, 13) = "Lote"
        objSheet.Cells(nroRowExcel, 14) = "Operario"
        objSheet.Cells(nroRowExcel, 15) = "Observaciones"
        cantCols = 15

        range = objSheet.Range("A" + nroRowExcel.ToString(), Reflection.Missing.Value)
        range = range.Resize(1, cantCols)
        range.Font.Bold = True
        range.Font.Color = Color.White
        range.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Black)

        nroRowExcel += 1

    End Sub

    Private Sub GenerarEncabezadoTabla(ByRef objSheet As Excel.Worksheet, Detalle As PedidoProveedor, PrintNombreProveedor As Boolean, MostrarObservaciones As Boolean)
        Dim range As Excel.Range
        Dim cantCols As Integer

        If PrintNombreProveedor = True Then
            'Este formato es para el archivo unificado
            objSheet.Cells(nroRowExcel, 1) = "Proveedor"
            objSheet.Cells(nroRowExcel, 2) = Detalle.proveedorNombre

            range = objSheet.Range("A" + nroRowExcel.ToString(), Reflection.Missing.Value)
            range = range.Resize(1, 11)
            range.Font.Bold = True
            range.Font.Color = Color.White
            range.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Black)

            range.Borders(Excel.XlBordersIndex.xlEdgeBottom).LineStyle = Excel.XlLineStyle.xlContinuous
            range.Borders(Excel.XlBordersIndex.xlEdgeBottom).Color = Color.White

            nroRowExcel += 1

            objSheet.Cells(nroRowExcel, 1) = "Cod. Producto"
            objSheet.Cells(nroRowExcel, 2) = "Descripcion"
            objSheet.Cells(nroRowExcel, 3) = ""
            objSheet.Cells(nroRowExcel, 4) = "Cant. Pedida"
            objSheet.Cells(nroRowExcel, 5) = "Un. Medida"
            objSheet.Cells(nroRowExcel, 6) = "Cant. Real"
            objSheet.Cells(nroRowExcel, 7) = "Kg. Reales"
            objSheet.Cells(nroRowExcel, 8) = "Kg. Pedidos"
            objSheet.Cells(nroRowExcel, 9) = "Promedio"
            objSheet.Cells(nroRowExcel, 10) = "Lote"
            objSheet.Cells(nroRowExcel, 11) = "Operario"
            cantCols = 11
            If MostrarObservaciones Then
                objSheet.Cells(nroRowExcel, 12) = "Observaciones"
                cantCols = 12
            End If
        Else

            objSheet.Cells(nroRowExcel, 1) = "Cod. Producto"
            objSheet.Cells(nroRowExcel, 2) = "Descripcion"
            objSheet.Cells(nroRowExcel, 3) = "Cant. Pedida"
            objSheet.Cells(nroRowExcel, 4) = "Un. Medida"
            objSheet.Cells(nroRowExcel, 5) = "Cant. Real"
            objSheet.Cells(nroRowExcel, 6) = "Kg. Reales"
            objSheet.Cells(nroRowExcel, 7) = "Kg. Pedidos"
            objSheet.Cells(nroRowExcel, 8) = "Promedio"
            objSheet.Cells(nroRowExcel, 9) = "Lote"
            objSheet.Cells(nroRowExcel, 10) = "Operario"
            cantCols = 10

            If MostrarObservaciones Then
                objSheet.Cells(nroRowExcel, 11) = "Observaciones"
                cantCols = 11
            End If
        End If


        range = objSheet.Range("A" + nroRowExcel.ToString(), Reflection.Missing.Value)
        range = range.Resize(1, cantCols)
        range.Font.Bold = True
        range.Font.Color = Color.White
        range.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Black)
        range.Borders.LineStyle = Excel.XlLineStyle.xlContinuous
        range.Borders.Color = Color.White

        range.Columns.AutoFit()

        nroRowExcel += 1

    End Sub


    Private Sub GenerarDetalleModificados(ByRef objSheet As Excel.Worksheet, Detalle As PedidoProveedor)
        Dim range As Excel.Range
        Dim Kgs As Double
        Dim TotalKgs As Double
        Dim CantRegistrosModificados As Integer = 0

        For Each DetallePedido As DetallePedido In Detalle.ListPedido
            'si la cantidad pedida es 20% menor a la cantidad sugerida o la cantidad pedida es 50% mayor a la cantidad sugerida
            If DetallePedido.CantidadPedida < (DetallePedido.CantidadSugerida * 0.8) Or DetallePedido.CantidadPedida >= (DetallePedido.CantidadSugerida * 1.5) Then
                objSheet.Cells(nroRowExcel, 1) = "'" + Detalle.proveedorNombre
                objSheet.Cells(nroRowExcel, 2) = "'" + DetallePedido.Codigo
                objSheet.Cells(nroRowExcel, 3) = DetallePedido.Descripcion
                objSheet.Cells(nroRowExcel, 4) = DetallePedido.CantidadPedida
                objSheet.Cells(nroRowExcel, 5) = DetallePedido.CantidadSugerida
                objSheet.Cells(nroRowExcel, 6) = DetallePedido.Unidad
                objSheet.Cells(nroRowExcel, 13) = DetallePedido.Observaciones

                'Si el articulo se vende por unidad o no se pide por bulto NO lo multiplico por la unidades por bulto
                If (LCase(DetallePedido.Unidad) = "unidad" Or DetallePedido.SePidePorBulto = False) Then
                    Kgs = DetallePedido.KgPromedioUnidad * Convert.ToInt32(DetallePedido.CantidadPedida)
                Else
                    Kgs = DetallePedido.UnidadesPorBulto * DetallePedido.KgPromedioUnidad * Convert.ToInt32(DetallePedido.CantidadPedida)
                End If

                objSheet.Cells(nroRowExcel, 8) = Kgs
                TotalKgs += Kgs
                nroRowExcel += 1
                CantRegistrosModificados += 1
            End If
        Next
        If CantRegistrosModificados > 0 Then
            range = objSheet.Range("A" + (nroRowExcel - (CantRegistrosModificados + 1)).ToString(), Reflection.Missing.Value)
            range = range.Resize(CantRegistrosModificados + 1, 13)
            range.Borders.LineStyle = Excel.XlLineStyle.xlContinuous
            range.Font.Size = 12
            range.Columns.AutoFit()

            range = objSheet.Range("A" + (nroRowExcel - CantRegistrosModificados).ToString(), Reflection.Missing.Value)
            range = range.Resize(CantRegistrosModificados, 13)
            range.RowHeight = 22
        End If

    End Sub
    Private Sub GenerarDetalleUnificado(ByRef objSheet As Excel.Worksheet, ByVal Detalle As PedidoProveedor, ByVal MostrarObservaciones As Boolean)
        Dim range As Excel.Range
        Dim Kgs As Double
        Dim TotalKgs As Double
        GenerarEncabezadoTabla(objSheet, Detalle, True, MostrarObservaciones)

        Dim cantCols As Integer = IIf(MostrarObservaciones = True, 12, 11)
        Dim Cantfilas As Integer = 0

        For Each DetallePedido As DetallePedido In Detalle.ListPedido
            If DetallePedido.CantidadPedida <> 0 Then
                objSheet.Cells(nroRowExcel, 1) = "'" + DetallePedido.Codigo
                objSheet.Cells(nroRowExcel, 2) = DetallePedido.Descripcion
                objSheet.Cells(nroRowExcel, 4) = DetallePedido.CantidadPedida
                objSheet.Cells(nroRowExcel, 5) = DetallePedido.Unidad
                If MostrarObservaciones Then
                    objSheet.Cells(nroRowExcel, 12) = DetallePedido.Observaciones
                End If

                'Si el articulo se vende por unidad o no se pide por bulto NO lo multiplico por la unidades por bulto
                If (LCase(DetallePedido.Unidad) = "unidad" Or DetallePedido.SePidePorBulto = False) Then
                    Kgs = DetallePedido.KgPromedioUnidad * Convert.ToInt32(DetallePedido.CantidadPedida)
                Else
                    Kgs = DetallePedido.UnidadesPorBulto * DetallePedido.KgPromedioUnidad * Convert.ToInt32(DetallePedido.CantidadPedida)
                End If

                objSheet.Cells(nroRowExcel, 8) = Kgs
                TotalKgs += Kgs
                nroRowExcel += 1
                Cantfilas += 1
            End If
        Next

        If Cantfilas > 0 Then
            range = objSheet.Range("A" + (nroRowExcel - Cantfilas).ToString(), Reflection.Missing.Value)
            range = range.Resize(Cantfilas, cantCols)
            range.Borders.LineStyle = Excel.XlLineStyle.xlContinuous
            range.Font.Size = 12
            range.Columns.AutoFit()
        End If

        objSheet.Cells(nroRowExcel, 1) = "Cant. Items:" + Cantfilas.ToString()
        objSheet.Cells(nroRowExcel, 8) = TotalKgs.ToString()

        range = objSheet.Range("A" + nroRowExcel.ToString(), Reflection.Missing.Value)
        range = range.Resize(1, cantCols)
        range.Font.Bold = True
        range.Font.Color = Color.White
        range.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Black)

        range.Borders(Excel.XlBordersIndex.xlEdgeBottom).LineStyle = Excel.XlLineStyle.xlContinuous
        range.Borders(Excel.XlBordersIndex.xlEdgeBottom).Color = Color.White

        'Luego de generado todo el archivo hago el auto fit del contenido
        range = objSheet.Range("A6", Reflection.Missing.Value)
        range = range.Resize(nroRowExcel - 6, cantCols)
        range.Columns.AutoFit()

    End Sub

    Private Sub GenerarDetalleProveedor(ByRef objSheet As Excel.Worksheet, Detalle As PedidoProveedor, MostrarObservaciones As Boolean)
        Dim range As Excel.Range
        Dim Kgs As Double
        Dim TotalKgs As Double
        GenerarEncabezadoTabla(objSheet, Detalle, False, MostrarObservaciones)

        Dim Cantfilas As Integer = 1
        Dim cantCols As Integer = IIf(MostrarObservaciones = True, 11, 10)
        For Each DetallePedido As DetallePedido In Detalle.ListPedido
            If DetallePedido.CantidadPedida <> 0 Then
                objSheet.Cells(nroRowExcel, 1) = "'" + DetallePedido.Codigo
                objSheet.Cells(nroRowExcel, 2) = DetallePedido.Descripcion
                objSheet.Cells(nroRowExcel, 3) = DetallePedido.CantidadPedida
                objSheet.Cells(nroRowExcel, 4) = DetallePedido.Unidad
                If MostrarObservaciones Then
                    objSheet.Cells(nroRowExcel, 11) = DetallePedido.Observaciones
                End If

                'Si el articulo se vende por unidad o no se pide por bulto NO lo multiplico por la unidades por bulto
                If (LCase(DetallePedido.Unidad) = "unidad" Or DetallePedido.SePidePorBulto = False) Then
                    Kgs = DetallePedido.KgPromedioUnidad * Convert.ToInt32(DetallePedido.CantidadPedida)
                Else
                    Kgs = DetallePedido.UnidadesPorBulto * DetallePedido.KgPromedioUnidad * Convert.ToInt32(DetallePedido.CantidadPedida)
                End If

                objSheet.Cells(nroRowExcel, 7) = Kgs
                TotalKgs += Kgs
                nroRowExcel += 1
                Cantfilas += 1
            End If
        Next
        range = objSheet.Range("A" + (nroRowExcel - Cantfilas).ToString(), Reflection.Missing.Value)
        range = range.Resize(Cantfilas, cantCols)
        range.Borders.LineStyle = Excel.XlLineStyle.xlContinuous
        range.Font.Size = 12
        range.RowHeight = 22
        range.Columns.AutoFit()

        objSheet.Cells(nroRowExcel, 1) = "Total:"
        objSheet.Cells(nroRowExcel, 7) = TotalKgs.ToString()

        range = objSheet.Range("A" + nroRowExcel.ToString(), Reflection.Missing.Value)
        range = range.Resize(1, cantCols)
        range.Font.Size = 12
        range.Font.Bold = True
        range.RowHeight = 22

    End Sub



    Private Sub AgruparMailsProveedores()
        For Each Detalle As PedidoProveedor In Pedido.ListProveedores
            For Each ListMails As EmailClasificacion In Detalle.ListEmails
                Dim pos As Integer
                Dim CurrentEmail As String = ListMails.Email
                pos = Pedido.ListEmails.FindIndex(Function(element) element.Email = CurrentEmail)
                If pos >= 0 Then
                    'ya existe este email en la lista, verifico si tambien ya existe este proveedor
                    Dim pos2 As Integer
                    Dim CurrentProv As String = ListMails.Descripcion
                    pos2 = Pedido.ListEmails(pos).ListProveedores.FindIndex(Function(element) element = CurrentProv)
                    If pos2 < 0 Then
                        'el proveedor es nuevo en la lista de emails
                        Pedido.ListEmails(pos).ListProveedores.Add(CurrentProv)
                    End If
                Else
                    'este email es nuevo
                    Dim ListProveedores As List(Of String) = New List(Of String) From {
                        ListMails.Descripcion
                    }
                    Dim newList As EmailList = New EmailList With {
                        .Email = CurrentEmail,
                        .ListProveedores = ListProveedores
                        }
                    Pedido.ListEmails.Add(newList)
                End If

            Next
        Next

    End Sub

    Private Function MostrarObservaciones(Detalle As PedidoProveedor) As Boolean
        'recorro el pedido para ver si hay por lo menos una observacion
        For Each DetallePedido As DetallePedido In Detalle.ListPedido
            If DetallePedido.Observaciones <> "" Then
                Return True
            End If
        Next
        Return False
    End Function

    Public Function EnviarMails() As Boolean
        Dim bok As Boolean
        bok = EnviarMailsProveedores()
        If bok = False Then
            If MsgBox("Ocurrio un problema al Enviar los mails del pedido, si lo desea puede reintentar el proceso o continuar sin el envio de los mails", vbRetryCancel, GlobalSettings.TituloMensajes) = MsgBoxResult.Retry Then
                bok = EnviarMails()
            End If
        End If
        Return bok
    End Function
    Public Function EnviarMailsProveedores() As Boolean

        LogEvent.Add("Enviando mails... ")

        If Pedido.ListEmails.Count > 0 Then
            For Each EmailList As EmailList In Pedido.ListEmails
                'si alguno de todos los mails da error no continuo
                If EnviarMailProveedor(EmailList, False) = False Then
                    Return False
                End If
            Next
        End If
        If DebugMode Then
            LogEvent.Add("Se ha ejecutado el sistema en modo DEBUG y no se enviaran las notificaciones de la aplicacion")
        Else
            'tengo que mandar notificaciones adicionales obtengo los mails que corresponden a este establecimiento
            Dim respuesta As ResponseEmailAplicacionEstablecimento
            respuesta = fachadaBD.GetEmailsAplicacionEstablecimiento(CurrentEstablecimiento.EstablecimientoId, GlobalSettings.NombreAplicacion, GlobalSettings.TipoNotificacionId)
            If respuesta.ConsultaExitosa Then
                If respuesta.rs.Count > 0 Then

                    'asigno todos los proveedores para el envio del mail
                    Dim ListProveedores As List(Of String) = New List(Of String)
                    For Each proveedor As PedidoProveedor In Pedido.ListProveedores
                        ListProveedores.Add(proveedor.proveedorNombre)
                    Next

                    For Each EmailEstablecimiento As EmailEstablecimiento In respuesta.rs
                        Dim newList As EmailList = New EmailList With {
                        .Email = EmailEstablecimiento.Email,
                        .ListProveedores = ListProveedores
                        }
                        'si alguno de todos los mails da error no continuo
                        If EnviarMailProveedor(newList, True) = False Then
                            Return False
                        End If

                    Next
                End If
            Else
                MsgBox(respuesta.mensaje, vbExclamation + vbOKOnly, GlobalSettings.TituloMensajes)
            End If
        End If
        Return True
    End Function

    Private Function EnviarMailProveedor(EmailList As EmailList, EsNotificacion As Boolean) As Boolean
        Try
            Dim ExcelFilename As String
            Dim mail As New MailMessage()
            Dim SmtpServer As New SmtpClient(GlobalSettings.SMTP)
            mail.From = New MailAddress(GlobalSettings.MailFromAddress)
            mail.[To].Add(EmailList.Email)
            mail.Subject = GetRazonSocialPedido() + " - " + CurrentEstablecimiento.Descripcion + " - " + NroPedido + " - " + Date.Today.ToString("dd.MM.yyyy")

            mail.IsBodyHtml = True
            mail.Body = "<p style='font-size: 14px' > Se ha generado el siguiente pedido de forma automatica por el sistema de Pedidos de Luz Azul </p>"
            mail.Body += "<p style='font-size: 14px' > El resumen del mismo es el siguiente: </p>"
            mail.Body += "<table style='font-size:14px'>"
            mail.Body += "<tr><td style='padding:10px;font-weight:bold;' >PROVEEDOR</td><td style='padding:10px;font-weight:bold;' >KG</td><td style='padding:10px;font-weight:bold;' >IMPORTE</td></tr>" + vbCrLf

            'Archivo unificado por mail
            If EsNotificacion Then
                ExcelFilename = PathArchivosExcel + "Notificaciones\Agrupado - " + GetRazonSocialPedido() + " - " + StrDepositos + " - " + Date.Today.ToString("dd.MM.yyyy") + ".xlsx"
            Else
                ExcelFilename = PathArchivosExcel + EmailList.Email + "\Agrupado - " + GetRazonSocialPedido() + " - " + StrDepositos + " - " + Date.Today.ToString("dd.MM.yyyy") + ".xlsx"
            End If

            'si el archivo existe lo adjunto
            If (My.Computer.FileSystem.FileExists(ExcelFilename)) Then
                Dim attachment As System.Net.Mail.Attachment
                attachment = New System.Net.Mail.Attachment(ExcelFilename)
                mail.Attachments.Add(attachment)
            End If

            'Archivo modificados por mail
            ExcelFilename = PathArchivosExcel + "Modificados - " + GetRazonSocialPedido() + " - " + StrDepositos + " - " + Date.Today.ToString("dd.MM.yyyy") + ".xlsx"

            'si el archivo existe lo adjunto
            If (My.Computer.FileSystem.FileExists(ExcelFilename)) Then
                Dim attachment As System.Net.Mail.Attachment
                attachment = New System.Net.Mail.Attachment(ExcelFilename)
                mail.Attachments.Add(attachment)
            End If

            'Archivo de analisis de datos
            ExcelFilename = PathArchivosExcel + "Analisis Datos - " + GetRazonSocialPedido() + " - " + StrDepositos + " - " + Date.Today.ToString("dd.MM.yyyy") + ".xlsx"

            'si el archivo existe lo adjunto
            If (My.Computer.FileSystem.FileExists(ExcelFilename)) Then
                Dim attachment As System.Net.Mail.Attachment
                attachment = New System.Net.Mail.Attachment(ExcelFilename)
                mail.Attachments.Add(attachment)
            End If

            Dim totalKilos As Double = 0
            Dim totalMonto As Double = 0

            'Archivos por separado por cada proveedor
            For Each NombreProveedor As String In EmailList.ListProveedores
                Dim pos As Integer
                pos = Pedido.ListProveedores.FindIndex(Function(element) element.proveedorNombre = NombreProveedor)
                If pos >= 0 Then
                    Dim Detalle As PedidoProveedor = Pedido.ListProveedores(pos)
                    If VerificarCantidadesPedidas(Detalle) Then
                        ExcelFilename = PathArchivosExcel + GetRazonSocialPedido() + " - " + Detalle.proveedorNombre + " - " + StrDepositos + " - " + Date.Today.ToString("dd.MM.yyyy") + ".xlsx"
                        If (My.Computer.FileSystem.FileExists(ExcelFilename)) Then
                            Dim attachment As System.Net.Mail.Attachment
                            attachment = New System.Net.Mail.Attachment(ExcelFilename)
                            mail.Attachments.Add(attachment)
                        End If
                        totalKilos += Pedido.ListProveedores(pos).PesoPedido
                        totalMonto += Pedido.ListProveedores(pos).MontoPedido

                        mail.Body += "<tr>"
                        mail.Body += "<td style='padding:10px;' >" + Pedido.ListProveedores(pos).proveedorNombre + "</td>"
                        mail.Body += "<td style='padding:10px;text-align:right;' >" + Format(Pedido.ListProveedores(pos).PesoPedido, "#0.00") + "</td>"
                        mail.Body += "<td style='padding:10px;text-align:right;' >$ " + Format(Pedido.ListProveedores(pos).MontoPedido, "#0.00") + "</td>"
                        mail.Body += "</tr>"
                    End If
                End If
            Next

            mail.Body += "<tr>"
            mail.Body += "<td style='padding:10px;font-weight:bold;' >TOTAL</td>"
            mail.Body += "<td style='padding:10px;font-weight:bold;text-align:right;' >" + Format(totalKilos, "#0.00") + "</td>"
            mail.Body += "<td style='padding:10px;font-weight:bold;text-align:right; ' >$ " + Format(totalMonto, "#0.00") + "</td>"
            mail.Body += "</tr>"

            mail.Body += "</table>"

            SmtpServer.Port = GlobalSettings.MailPortGmail
            SmtpServer.Credentials = New System.Net.NetworkCredential(GlobalSettings.MailUsername, GlobalSettings.MailPassword)
            SmtpServer.EnableSsl = GlobalSettings.MailEnableSSL

            SmtpServer.Send(mail)

            EnviarMailProveedor = True

            'Cierro la conexion para liberar los archivos adjuntos
            mail.Dispose()
            SmtpServer.Dispose()
            Application.DoEvents()

        Catch ex As Exception
            Dim strError As String = "EnviarMailProveedor - Message: " + ex.Message + vbCrLf + JsonConvert.SerializeObject(ex)
            WriteLogFile(strError)
            MsgBox("Error al enviar el mail: " + vbCrLf + ex.Message, vbCritical + vbOKOnly, GlobalSettings.TituloMensajes)
            EnviarMailProveedor = False
        End Try

    End Function
    Private Function VerificarCantidadesPedidas(Detalle As PedidoProveedor) As Boolean
        'recorro el pedido para ver si hay por lo menos una observacion
        For Each DetallePedido As DetallePedido In Detalle.ListPedido
            If Convert.ToInt32(DetallePedido.CantidadPedida) > 0 Then
                Return True
            End If
        Next
        Return False
    End Function
    Public Function WSNuevoPedido() As ResponseWS
        Dim respuesta As ResponseWS = New ResponseWS
        Try
            LogEvent.Add("Generar nuevo pedido...")

            Dim response As String
            Dim postData As String = ""
            postData += "establecimientoId=" + CurrentEstablecimiento.EstablecimientoId
            postData += "&usuarioId=" + UsuarioId
            postData += "&clienteId=" + Cliente.ClienteId
            postData += "&cuit=" + HttpUtility.UrlEncode(GetCUITPedido())
            postData += "&razonsocial=" + HttpUtility.UrlEncode(GetRazonSocialPedido())

            Dim urlWeb As String = IIf(DebugMode = True, GlobalSettings.UrlWebserviceTest, GlobalSettings.UrlWebservice)
            response = fachadaBD.PostRequest(postData, urlWeb + "/pedidos/nuevo")

            Dim JsonResponse = Newtonsoft.Json.Linq.JObject.Parse(response)

            If (JsonResponse.ContainsKey("ok")) Then
                respuesta.ConsultaExitosa = True
                respuesta.mensaje = JsonResponse.Item("ok")
            End If
            If (JsonResponse.ContainsKey("error")) Then
                respuesta.ConsultaExitosa = False
                respuesta.mensaje = JsonResponse.Item("error")
            End If
        Catch ex As Exception
            respuesta.ConsultaExitosa = False
            respuesta.mensaje = "Error al obtener la respuesta del webservice"
            Dim strError As String = "WSNuevoPedido - Message: " + ex.Message + vbCrLf + JsonConvert.SerializeObject(ex)
            WriteLogFile(strError)
        End Try

        Return respuesta
    End Function
    Public Function WSDetallePedido(Detalle As PedidoProveedor) As ResponseWS
        Dim respuesta As ResponseWS = New ResponseWS
        Try
            Dim response As String
            Dim postData As String = ""
            postData += "nroPedido=" + NroPedido
            postData += "&proveedorId=" + Detalle.proveedorId
            postData += "&proveedorNombre=" + Detalle.proveedorNombre

            'Agrego el establecimiento de distribucion
            Dim EstDistribucion As String = IIf(Detalle.EstablecimientoIdDistribucion = "", GlobalSettings.EstablecimientoIdDistribucionDefault, Detalle.EstablecimientoIdDistribucion)
            postData += "&establecimientoIdDistribucion=" + EstDistribucion

            postData += "&cantRegistros=" + Detalle.ListPedido.Count.ToString()
            Dim esFiscal As String = "0"
            If Detalle.esFiscal Then
                esFiscal = "1"
            End If
            postData += "&esfiscal=" + esFiscal
            Dim strEmails As String = ""
            If Detalle.ListEmails.Count > 0 Then
                For Each ListEmail As EmailClasificacion In Detalle.ListEmails
                    strEmails += ListEmail.Email + ";"
                Next
                strEmails = strEmails.Substring(0, strEmails.Length - 1) ' Elimino la ultima ;
            End If
            postData += "&listEmails=" + strEmails

            Dim n As Integer = 1
            For Each DetallePedido As DetallePedido In Detalle.ListPedido
                postData += "&cantpedida" + n.ToString() + "=" + DetallePedido.CantidadPedida
                postData += "&cantsugerida" + n.ToString() + "=" + DetallePedido.CantidadSugerida
                postData += "&unidad" + n.ToString() + "=" + DetallePedido.Unidad
                postData += "&codigo" + n.ToString() + "=" + DetallePedido.Codigo
                postData += "&kgpromedio" + n.ToString() + "=" + DetallePedido.KgPromedioUnidad.ToString()
                postData += "&unidadesporbulto" + n.ToString() + "=" + DetallePedido.UnidadesPorBulto.ToString()
                postData += "&observaciones" + n.ToString() + "=" + HttpUtility.UrlEncode(DetallePedido.Observaciones)
                postData += "&stock" + n.ToString() + "=" + DetallePedido.Stock
                postData += "&packing" + n.ToString() + "=" + DetallePedido.Packing
                Dim SePide As Integer = IIf(DetallePedido.SePidePorBulto = True, 1, 0)
                postData += "&sepideporbulto" + n.ToString() + "=" + SePide.ToString()
                postData += "&TipoUnidadMedidaId2" + n.ToString() + "=" + DetallePedido.TipoUnidadMedidaId2.ToString()

                n += 1
            Next

            Dim urlWeb As String = IIf(DebugMode = True, GlobalSettings.UrlWebserviceTest, GlobalSettings.UrlWebservice)
            response = fachadaBD.PostRequest(postData, urlWeb + "/pedidos/detalle")

            Dim JsonResponse = Newtonsoft.Json.Linq.JObject.Parse(response)

            If (JsonResponse.ContainsKey("ok")) Then
                respuesta.ConsultaExitosa = True
                respuesta.mensaje = JsonResponse.Item("ok")
            End If
            If (JsonResponse.ContainsKey("error")) Then
                respuesta.ConsultaExitosa = False
                respuesta.mensaje = JsonResponse.Item("error")
            End If
        Catch ex As Exception
            respuesta.ConsultaExitosa = False
            respuesta.mensaje = "Error al obtener la respuesta del webservice"
            Dim strError As String = "WSDetallePedido - Message: " + ex.Message + vbCrLf + JsonConvert.SerializeObject(ex)
            WriteLogFile(strError)
        End Try

        Return respuesta
    End Function

    Public Function WSSetMailsEnviados() As ResponseWS
        Dim respuesta As ResponseWS = New ResponseWS
        Try
            Dim response As String
            Dim postData As String = ""
            postData += "nropedido=" + NroPedido

            Dim urlWeb As String = IIf(DebugMode = True, GlobalSettings.UrlWebserviceTest, GlobalSettings.UrlWebservice)
            response = fachadaBD.PostRequest(postData, urlWeb + "/pedidos/mails_enviados")

            Dim JsonResponse = Newtonsoft.Json.Linq.JObject.Parse(response)

            If (JsonResponse.ContainsKey("ok")) Then
                respuesta.ConsultaExitosa = True
                respuesta.mensaje = JsonResponse.Item("ok")
            End If
            If (JsonResponse.ContainsKey("error")) Then
                respuesta.ConsultaExitosa = False
                respuesta.mensaje = JsonResponse.Item("error")
            End If
        Catch ex As Exception
            respuesta.ConsultaExitosa = False
            respuesta.mensaje = "Error al obtener la respuesta del webservice"
            Dim strError As String = "WSSetMailsEnviados - Message: " + ex.Message + vbCrLf + JsonConvert.SerializeObject(ex)
            WriteLogFile(strError)
        End Try

        Return respuesta
    End Function

    Public Function WSEliminarPedido() As ResponseWS
        Dim respuesta As ResponseWS = New ResponseWS
        Try
            Dim response As String
            Dim postData As String = ""
            postData += "nropedido=" + NroPedido

            Dim urlWeb As String = IIf(DebugMode = True, GlobalSettings.UrlWebserviceTest, GlobalSettings.UrlWebservice)
            response = fachadaBD.PostRequest(postData, urlWeb + "/pedidos/eliminar")

            Dim JsonResponse = Newtonsoft.Json.Linq.JObject.Parse(response)

            If (JsonResponse.ContainsKey("ok")) Then
                respuesta.ConsultaExitosa = True
                respuesta.mensaje = JsonResponse.Item("ok")
            End If
            If (JsonResponse.ContainsKey("error")) Then
                respuesta.ConsultaExitosa = False
                respuesta.mensaje = JsonResponse.Item("error")
            End If
        Catch ex As Exception
            respuesta.ConsultaExitosa = False
            respuesta.mensaje = "Error al obtener la respuesta del webservice"
            Dim strError As String = "WSNuevoPedido - Message: " + ex.Message + vbCrLf + JsonConvert.SerializeObject(ex)
            WriteLogFile(strError)
        End Try

        Return respuesta
    End Function


    Public Shared Sub LeerConfiguracionesXML()
        Dim doc As XmlDocument = New XmlDocument With {
            .PreserveWhitespace = True
        }
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
                        DebugMode = IIf(nodeListLevel2.Item(0).InnerText.ToLower() = "true", True, False)
                    End If

                    'verifico si hay una configuracion especifica para esta razon social
                    If RazonSocial <> "" Then
                        nodeListLevel2 = book.SelectNodes(RemoveWhitespace(RazonSocial))
                        If nodeListLevel2.Count > 0 Then
                            DebugMode = IIf(nodeListLevel2.Item(0).InnerText.ToLower() = "true", True, False)
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

    Public Sub enviarMailPrueba()
        Dim EmailTo As String = InputBox("A que direccion desea enviar el mail?")
        If IsValidEmailFormat(EmailTo) Then
            Try
                Dim mail As New MailMessage()
                Dim SmtpServer As New SmtpClient(GlobalSettings.SMTP)
                mail.From = New MailAddress(GlobalSettings.MailFromAddress)
                mail.[To].Add(EmailTo)
                mail.Subject = "Mail de pruebas desde el sistema de pedidos de Luz Azul"

                mail.IsBodyHtml = True
                mail.Body = "<p style='font-size: 14px' > Se ha generado el siguiente mail de prueba de forma automatica por el sistema de Pedidos de Luz Azul </p>"
                mail.Body += "<p style='font-size: 14px' > SMTP:  " + GlobalSettings.SMTP + "</p>"
                mail.Body += "<p style='font-size: 14px' > Mail From:  " + GlobalSettings.MailFromAddress + "</p>"
                mail.Body += "<p style='font-size: 14px' > Mail Port:  " + GlobalSettings.MailPort.ToString() + "</p>"
                mail.Body += "<p style='font-size: 14px' > Mail Port Gmail:  " + GlobalSettings.MailPortGmail.ToString() + "</p>"
                mail.Body += "<p style='font-size: 14px' > Mail username:  " + GlobalSettings.MailUsername + "</p>"
                mail.Body += "<p style='font-size: 14px' > Mail Enable SSl:  " + GlobalSettings.MailEnableSSL.ToString() + "</p>"

                SmtpServer.Port = GlobalSettings.MailPortGmail
                SmtpServer.Credentials = New System.Net.NetworkCredential(GlobalSettings.MailUsername, GlobalSettings.MailPassword)
                SmtpServer.EnableSsl = GlobalSettings.MailEnableSSL
                SmtpServer.Timeout = 240000 ' Seteo el timeout en 4 minutos

                SmtpServer.Send(mail)

                MsgBox("Mail enviado exitosamente!", MsgBoxStyle.Information, GlobalSettings.TituloMensajes)
                'Cierro la conexion para liberar los archivos adjuntos
                mail.Dispose()
                SmtpServer.Dispose()
                Application.DoEvents()

            Catch ex As Exception
                MsgBox("Error al enviar el mail de pruebas: " + vbCrLf + ex.Message, vbCritical + vbOKOnly, GlobalSettings.TituloMensajes)

                Dim strMsg As String = "SMTP:  " + GlobalSettings.SMTP + vbCrLf
                strMsg += "Mail From:  " + GlobalSettings.MailFromAddress + vbCrLf
                strMsg += "Mail Port:  " + GlobalSettings.MailPort.ToString() + vbCrLf
                strMsg += "Mail Port Gmail:  " + GlobalSettings.MailPortGmail.ToString() + vbCrLf
                strMsg += "Mail username:  " + GlobalSettings.MailUsername + vbCrLf
                strMsg += "Mail password:  " + GlobalSettings.MailPassword + vbCrLf
                strMsg += "Mail Enable SSl:  " + GlobalSettings.MailEnableSSL.ToString() + vbCrLf
                MsgBox(strMsg, vbCritical + vbOKOnly, GlobalSettings.TituloMensajes)
            End Try
        Else
            MsgBox("El mail ingresado no es valido")
        End If

    End Sub

    Public Function ShowDialogDebug() As String
        Dim strInfo As String
        strInfo = "Debug Mode: " + DebugMode.ToString() + vbCrLf
        strInfo += "Path Archivos: " + GetPathArchivosExcel() + vbCrLf
        Dim urlWeb As String = IIf(DebugMode = True, GlobalSettings.UrlWebserviceTest, GlobalSettings.UrlWebservice)
        strInfo += "Webservice: " + urlWeb + vbCrLf
        strInfo += "Empresa: " + GetRazonSocialPedido() + " (" + fachadaBD.NombreBase + ")" + vbCrLf
        strInfo += "Mail from addres: " + GlobalSettings.MailFromAddress + vbCrLf
        Return strInfo
    End Function

    Public Function GetListProductosAgrupacion() As List(Of DetalleProductoAgrupacion)
        Dim pos As Integer
        Dim ListProductoAgrupaciones As List(Of DetalleProductoAgrupacion) = New List(Of DetalleProductoAgrupacion)
        pos = GetPosicionProveedorPedido()
        If pos >= 0 Then
            Dim ListProductos As List(Of DetallePedido) = Pedido.ListProveedores(pos).ListPedido
            If ListProductos.Count > 0 Then
                'Recorro del pedido los productos que corresponden con la agrupacion actual
                For Each Producto As DetallePedido In ListProductos
                    If Producto.AgrupacionId = CurrentAgrupacionId Then
                        ListProductoAgrupaciones.Add(New DetalleProductoAgrupacion(Producto.Codigo, Producto.Descripcion, Producto.CantidadPedida, Producto.AgrupacionId))
                    End If
                Next
            End If
        End If
        Return ListProductoAgrupaciones
    End Function
    Public Sub GrabarAgrupacion(DbGridAgrupacion As DataGridView)
        Dim pos As Integer
        Dim pos2 As Integer

        pos = GetPosicionProveedorPedido()
        If pos >= 0 Then
            'recorro toda la grilla y guardo en las variables globales el pedido
            For Each rowGrilla As DataGridViewRow In DbGridAgrupacion.Rows
                Dim CodigoArticulo As String = rowGrilla.Cells("ProductoId").Value
                pos2 = Pedido.ListProveedores(pos).ListPedido.FindIndex(Function(element) element.Codigo = CodigoArticulo)
                If pos2 >= 0 Then
                    Pedido.ListProveedores(pos).ListPedido(pos2).CantidadPedida = rowGrilla.Cells("Cantidad").Value
                End If
            Next
        End If
    End Sub
    Public Sub LoadClasificacionesClientes()
        Dim respuesta As ResponseClasificacionesClientes
        respuesta = fachadaBD.GetClasificacionesClientes()
        If respuesta.ConsultaExitosa Then
            ListClasificacionesClientes = respuesta.rs
        End If
    End Sub

    Public Sub CargarProductosOpcionales()
        Dim respuesta As New ResponseProductosClasificacion
        Dim ListaClasificaciones As New List(Of String)
        ListaClasificaciones.Add(GlobalSettings.NombreClasificacionProductosOpcionales)

        ListProductosOpcionales.Clear()
        respuesta = fachadaBD.GetProductosClasificacion(ListaClasificaciones)
        If respuesta.ConsultaExitosa Then
            For Each prod As ProductoClasificacion In respuesta.rs
                ListProductosOpcionales.Add(prod.ProductoId)
            Next
        End If
    End Sub

    Public Function CheckProductoOpcional(ProductoId As String) As Boolean
        Return ListProductosOpcionales.Contains(ProductoId)
    End Function

End Class


