Imports VBA
Imports System.IO
Imports System.Net
Imports System.Net.Mail
Imports System.Text
Imports GJAAccDatos
Imports GJAEncripta

Imports System.Data.SqlClient

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

Public Class LuzAzulCommon
    Public myConn As SqlConnection
    Public myCmd As SqlCommand
    Public myReader As SqlDataReader

    Public conn As Conexion = New Conexion
    Public rs As RegSet = New RegSet
    Public Query As List(Of String) = New List(Of String)
    Public Shared TitulosMensaje As String = GlobalSetting.TituloMensajes
    Public Property NombreBase As String = ""
    Public Property NombreBaseEnsemble As String = GlobalSetting.NombreBaseEnsemble
    Public Property NombreBasePrecios As String = GlobalSetting.NombreBasePrecios
    Public Property NombreBaseMaestros As String = GlobalSetting.NombreBaseMaestros

    'Configuraciones para el envio de emails
    Private Shared MailFromAddress As String = GlobalSetting.MailFromAddress
    Private Shared MailUsername As String = GlobalSetting.MailUsername
    Private Shared MailPassword As String = GlobalSetting.MailPassword
    Private Shared SMTP As String = GlobalSetting.SMTP
    Private Shared MailPort As Integer = GlobalSetting.MailPort
    Private Shared MailEnableSSL As Boolean = GlobalSetting.MailEnableSSL

    Public Sub New()
        Try

            ' Intento abrir la conexion a la base de datos

            'Create a Connection object.
            myConn = New SqlConnection(GetSqlServerConnectionString())

            myConn.Open()

        Catch ex As Exception
            MsgBox("Error al establecer la conexion a la base de datos", vbOKOnly, TitulosMensaje)
        End Try

    End Sub

    Private Function GetSqlServerConnectionString() As String
        GetSqlServerConnectionString = "Server=" + GlobalSetting.DBConecctionString + ";Database=" + NombreBaseMaestros + ";Uid=" + GlobalSetting.DBUsername + ";Pwd=" + GlobalSetting.DBPassword + ";Connection Timeout=30;"
    End Function

    Protected Overrides Sub Finalize()
        'Cuando se finaliza el objeto se cierran todas las conexiones
        'If myConn.State Then
        'myConn.Close()
        'End If

        MyBase.Finalize()
    End Sub
    '---- SECCION PARA DECLARAR LAS CLASES DE RESPUESTA ----
    Public Class ResponseWS
        Public mensaje As String
        Public ConsultaExitosa As Boolean = False
    End Class
    Public Class ResponseLogin
        Inherits ResponseWS
        Public PermiteLogin As Boolean = False
        Public EsAdministrador As Boolean
    End Class
    Public Class ResponseRazonSocial
        Inherits ResponseWS
        Public RazonSocial As String
    End Class

    Public Class ResponseEntidades
        Inherits ResponseWS

        Public Email As String
        Public UID As String
        Public PWD As String
        Public SMTPServer As String
        Public Puerto As String
        Public UsaSSL As Boolean
    End Class
    Public Class ResponseCuit
        Inherits ResponseWS
        Public CUIT As String = ""
    End Class
    Public Class ResponseVerificar
        Inherits ResponseWS
        Public EsFabrica As Boolean = False
    End Class
    Public Class ResponseEstablecimiento
        Inherits ResponseWS
        Public rs As IEnumerable(Of Establecimiento)
    End Class

    Public Class ResponsePorcFacturacion
        Inherits ResponseWS
        Public PorFacturacion As Double = 0
    End Class

    Public Class ResponseClasificaciones
        Inherits ResponseWS
        Public rs As IEnumerable(Of Clasificacion)
    End Class

    Public Class ResponseDeposito
        Inherits ResponseWS

        Public rs As IEnumerable(Of Deposito)
    End Class

    Public Class ResponseSucursal
        Inherits ResponseWS

        Public rs As IEnumerable(Of Sucursal)
    End Class
    Public Class ResponseEmailClasificaciones
        Inherits ResponseWS

        Public rs As IEnumerable(Of EmailClasificacion)
    End Class

    Public Class ResponseDireccionesCliente
        Inherits ResponseWS

        Public rs As IEnumerable(Of DireccionCliente)
    End Class

    Public Class ResponseConfiguracion
        Inherits ResponseWS

        Public Parametro As String
        Public Tipo As String
        Public Valor As String
    End Class

    Public Class ResponseCliente
        Inherits ResponseWS

        Public Cliente As Cliente
    End Class

    Public Class ResponseDetalle
        Inherits ResponseWS

        Public rs As IEnumerable(Of DetalleDocumento)
    End Class
    Public Class responseListProductosSimple
        Inherits ResponseWS
        Public items As List(Of ListProductoSimple) = New List(Of ListProductoSimple)
    End Class
    Public Class responseConfigAsientos
        Inherits ResponseWS
        Property Items As New List(Of ConfigAsiento)
    End Class

    '---- OBJETOS ----
    Public Class ListProductoSimple
        Public Property productoId As String
        Public Property productoNombre As String
    End Class

    Public Class Establecimiento
        Public Property EstablecimientoId As String
        Public Property Descripcion As String
        Public Property EsPropio As Boolean
        Public Property DbName As String
        Public Property SucursalId As String

        Public Sub New(ByVal IdEstablecimiento As String, DescripcionEstablecimiento As String, Propio As Boolean, ByVal Database As String, ByVal Sucursal As String)
            EstablecimientoId = IdEstablecimiento
            Descripcion = DescripcionEstablecimiento
            EsPropio = Propio
            DbName = Database
            SucursalId = Sucursal
        End Sub
    End Class

    Public Class Clasificacion
        Public Property ClasificacionProdId As String
        Public Property Descripcion As String
        Public Property DescripcionSinPrefijo As String
        Public Property CantDiasReparo As Integer
        Public Property FrecuenciaPed As Integer
        Public Property PlazoEntregaPed As Integer
        Public Property KilosMin As Integer
        Public Property EstablecimientoIdDistribucion As String
        Public Property ClasificacionProdPadre As String
        Public Property ClasificacionesHijas As List(Of Clasificacion) = New List(Of Clasificacion)
        Public Property ListEmails As List(Of EmailClasificacion) = New List(Of EmailClasificacion)
        Public Sub New(ByVal ClasificacionId As String, DescripcionClasificacion As String, ClasificacionPadre As String, strCantDiasReparo As Integer, strFrecuenciaPed As Integer, strPlazoEntregaPed As Integer, strKilosMin As Integer, strEstablecimientoDistribucion As String)
            ClasificacionProdId = ClasificacionId
            Descripcion = DescripcionClasificacion
            ClasificacionProdPadre = ClasificacionPadre
            CantDiasReparo = strCantDiasReparo
            FrecuenciaPed = strFrecuenciaPed
            PlazoEntregaPed = strPlazoEntregaPed
            KilosMin = strKilosMin
            DescripcionSinPrefijo = GetNombreProveedorSinPrefijo(DescripcionClasificacion)
            EstablecimientoIdDistribucion = strEstablecimientoDistribucion
        End Sub
    End Class

    Public Class EmailClasificacion
        Public Property Nombre As String
        Public Property Descripcion As String
        Public Property Email As String
        Public Sub New(NombreClasificacion As String, DescripcionClasificacion As String, EmailClasificacion As String)
            Nombre = NombreClasificacion
            Descripcion = DescripcionClasificacion
            Email = EmailClasificacion
        End Sub
    End Class
    Public Class Deposito
        Public Property DepositoId As String
        Public Property Descripcion As String

        Public Sub New(ByVal IdDeposito As String, DescripcionDeposito As String)
            DepositoId = IdDeposito
            Descripcion = DescripcionDeposito
        End Sub
    End Class

    Public Class Sucursal
        Public Property SucursalId As String
        Public Property Descripcion As String
        Public Property LlevaPercep As Boolean
        Public Property TipoSuc As String

        Public Sub New(ByVal SucursalId As String, Descripcion As String, LlevaPercep As Boolean, TipoSuc As String)
            Me.SucursalId = SucursalId
            Me.Descripcion = Descripcion
            Me.LlevaPercep = LlevaPercep
            Me.TipoSuc = TipoSuc
        End Sub
    End Class
    Public Class DireccionCliente
        Public Property ClienteId As String
        Public Property DireccionId As String
        Public Property Calle As String
        Public Property Numero As String
        Public Property Piso As String
        Public Property Dpto As String
        Public Property Localidad As String
        Public Property Provincia As String
        Public Property Entrega As Boolean

        Public Sub New(ByVal sClienteId As String, ByVal SdireccionId As String, ByVal SCalle As String, ByVal SNumero As String, ByVal SPiso As String, ByVal SDpto As String, ByVal SLocalidad As String, ByVal sProvincia As String, ByVal bEntrega As Boolean)
            ClienteId = sClienteId
            DireccionId = SdireccionId
            Calle = SCalle
            Numero = SNumero
            Piso = SPiso
            Dpto = SDpto
            Localidad = SLocalidad
            Provincia = sProvincia
            Entrega = bEntrega
        End Sub
    End Class
    Public Class DetallePedido
        Public Property Codigo As String
        Public Property Descripcion As String
        Public Property Stock As String
        Public Property Venta As String
        Public Property Packing As String
        Public Property CantidadPedida As String
        Public Property CantidadSugerida As String
        Public Property Unidad As String
        Public Property KgPromedioUnidad As Double
        Public Property UnidadesPorBulto As Integer
        Public Property PrecioCompra As Double
        Public Property SeVendePorPeso As Boolean
        Public Property SePidePorBulto As Boolean
        Public Property AgrupacionId As String
        Public Property Observaciones As String
        Public Property TipoUnidadMedidaId2 As String

        Public Sub New(ByVal Cod As String, Descrip As String, ByVal Pack As String, ByVal Cantstock As String, ByVal cantVenta As String, ByVal UnidadPedido As String, ByVal pedir As String, ByVal sugeridas As String, KgPromUnidad As Double, UnidadesXbulto As Integer, PrecioDeCompra As Double, SeVendexPeso As Boolean, IdAgrupacion As String, strObservaciones As String, ByVal bSePidePorBulto As Boolean, ByVal TipoUnidadMedida2 As String)
            Codigo = Cod
            Descripcion = Descrip
            Packing = Pack
            Stock = Cantstock
            Venta = cantVenta
            Unidad = UnidadPedido
            CantidadPedida = pedir
            CantidadSugerida = sugeridas
            KgPromedioUnidad = KgPromUnidad
            UnidadesPorBulto = UnidadesXbulto
            PrecioCompra = PrecioDeCompra
            SeVendePorPeso = SeVendexPeso
            AgrupacionId = IdAgrupacion
            Observaciones = strObservaciones
            SePidePorBulto = bSePidePorBulto
            TipoUnidadMedidaId2 = TipoUnidadMedida2
        End Sub
    End Class

    Public Class EmailList
        Public Property Email As String
        Public Property ListProveedores As List(Of String) = New List(Of String)
    End Class

    Public Class Pedido
        Public fecha As Date
        Public establecimientoId As String
        Public usuarioId As String
        Public ListProveedores As List(Of PedidoProveedor) = New List(Of PedidoProveedor)
        Public ListEmails As List(Of EmailList) = New List(Of EmailList)
    End Class


    Public Class PedidoProveedor
        Public proveedorId As String
        Public proveedorNombre As String
        Public esFiscal As Boolean
        Public MostrarCeros As Boolean
        Public MostrarSoloModificados As Boolean
        Public CantidadesModificadas As Boolean
        Public ListEmails As List(Of EmailClasificacion) = New List(Of EmailClasificacion)
        Public ListPedido As List(Of DetallePedido) = New List(Of DetallePedido)
        Public PesoPedido As Double
        Public KilosMin As Integer
        Public MontoPedido As Double
        Public EstablecimientoIdDistribucion As String
    End Class

    Public Class DetalleDocumento
        Public ProductoId As String
        Public NroPallet As String
        Public CantUnidades As Integer
        Public CantKilogramos As Double
        Public Descripcion As String
        Public AlicuotaIVA As Double
        Public TipoProductoId As Integer
        Public Precio As Double
        Public SeVendePorUnidad As Boolean
        Public SePidePorUnidad As Boolean
        Public UnidadesPorBulto As Integer
        Public ProductoIdUni As String
        Public DescripcionUni As String
        Public TipoProductoIdUni As Integer
        Public PrecioUni As Double
        Public Stock As Double
        Public StockUnidades As Double
        Public EsPrueba As Boolean
        Public SeVendePorKgFijos As Boolean
        Public KgPromedioxUnid As Double
    End Class

    Public Class EstablecimientoCliente
        Public Property EstablecimientoId As Integer
        Public Property Descripcion As String
        Public Property PorcFactura As Double
        Public Property DireccionId As Integer

        Public Sub New()

        End Sub

        Public Sub New(ByVal EstablecimientoId As Integer, ByVal Descripcion As String, ByVal PorcFactura As Double, ByVal DireccionId As Integer)
            Me.EstablecimientoId = EstablecimientoId
            Me.Descripcion = Descripcion
            Me.PorcFactura = PorcFactura
            Me.DireccionId = DireccionId
        End Sub
    End Class

    Public Class EscalaPercepcion
        Public Property EscalaPercepcionId As Integer
        Public Property TipoPercepcion As String
        Public Property Base As Double
        Public Property TomaBaseNoImponible As Boolean
        Public Property PorcPercepcion As Double
        Public Property Descripcion As String
        Public Property JurisdiccionIIBBId As String

        Public Sub New(ByVal EscalaPercepcionId As Integer, ByVal TipoPercepcion As String, ByVal Base As Double, ByVal TomaBaseNoImponible As Boolean,
                       ByVal PorcPercepcion As Double, ByVal Descripcion As String, ByVal JurisdiccionIIBBId As String)
            Me.EscalaPercepcionId = EscalaPercepcionId
            Me.TipoPercepcion = TipoPercepcion
            Me.Base = Base
            Me.TomaBaseNoImponible = TomaBaseNoImponible
            Me.PorcPercepcion = PorcPercepcion
            Me.Descripcion = Descripcion
            Me.JurisdiccionIIBBId = JurisdiccionIIBBId
        End Sub
    End Class

    Public Class Cliente
        Public Property ClienteId As String
        Public Property RazonSocial As String
        Public Property PosIVAId As Integer
        Public Property ListaPrecioId As Integer
        Public Property DireccionId As Integer
        Public Property VendedorId As String
        Public Property CondicionVentaId As Integer
        Public Property JurisdiccionIIBBId As String
        Public Property CUIT As String
        Public Property Recordatorio As String
        Public Property CtrlCredito As Double

        Public Property ListEscalasPecep As New List(Of EscalaPercepcion)
        Public Property ListEstablecimientos As New List(Of EstablecimientoCliente)
        Public Property ListEmails As New List(Of String)
        Public Sub New(ByVal ClienteId As String, RazonSocial As String, PosIVAId As Integer, ListaPrecioId As Integer,
                       DireccionId As Integer, VendedorId As String, CondicionVentaId As Integer, JurisdiccionIIBBId As String,
                       CUIT As String, Recordatorio As String, CtrlCredito As Double)
            Me.ClienteId = ClienteId
            Me.RazonSocial = RazonSocial
            Me.PosIVAId = PosIVAId
            Me.ListaPrecioId = ListaPrecioId
            Me.DireccionId = DireccionId
            Me.VendedorId = VendedorId
            Me.CondicionVentaId = CondicionVentaId
            Me.JurisdiccionIIBBId = JurisdiccionIIBBId
            Me.CUIT = CUIT
            Me.Recordatorio = Recordatorio
            Me.CtrlCredito = CtrlCredito
        End Sub
    End Class
    Public Class ConfigAsiento
        Property configId As Integer
        Property Descripcion As String
        Property CuentaIdHaber As String
        Property SubCuentaIdHaber As String
        Property CuentaIdDebe As String
        Property SubCuentaIdDebe As String
        Property TipoAsiento As String
        Property ConceptoAsiento As String
    End Class


    '---- FUNCIONES Y PROCEDIMIENTOS DE LA CLASE ----

    Public Function PostRequest(postdata As String, WsUrl As String) As String
        Dim request As WebRequest = WebRequest.Create(WsUrl)
        request.Method = "POST"
        Dim byteArray As Byte() = Encoding.UTF8.GetBytes(postdata)
        request.ContentType = "application/x-www-form-urlencoded"
        request.ContentLength = byteArray.Length
        Dim dataStream As Stream = request.GetRequestStream()
        dataStream.Write(byteArray, 0, byteArray.Length)
        dataStream.Close()
        Dim response As WebResponse = request.GetResponse()
        dataStream = response.GetResponseStream()
        Dim reader As New StreamReader(dataStream)
        Dim responseFromServer As String = reader.ReadToEnd()
        reader.Close()
        dataStream.Close()
        response.Close()
        Return responseFromServer
    End Function
    Public Shared Function CheckForInternetConnection() As Boolean
        Try
            Using client = New WebClient()
                Using stream = client.OpenRead("http://www.google.com")
                    Return True
                End Using
            End Using
        Catch
            Return False
        End Try
    End Function

    Public Function EncodePassword(ByVal Cadena As String) As String
        Dim nuevaCadena As String = ""
        Dim orden As Integer = 1
        Dim Letra As String

        For i = 1 To Len(Cadena)
            Letra = Mid(Cadena, i, 1)

            If orden = 1 Then
                Letra = Asc(Letra) + 15
            ElseIf orden = 2 Then
                Letra = Asc(Letra) - 22
            ElseIf orden = 3 Then
                Letra = Asc(Letra) - 13
            ElseIf orden = 4 Then
                Letra = Asc(Letra) + 11
            End If
            If orden = 4 Then
                orden = 1
            Else
                orden = CInt(orden) + 1
            End If

            nuevaCadena += Chr(Letra)
        Next

        nuevaCadena = StrReverse(nuevaCadena)
        Return nuevaCadena
    End Function
    Public Function DoLogin(ByVal usuario As String, ByVal clave As String, ByVal DatabaseName As String) As ResponseLogin
        Dim HashPassword As String = EncodePassword(clave) ' Encripto para comparar con BD
        Dim respuesta As New ResponseLogin

        Try
            Dim sqlQuery As String = "SELECT * FROM " + DatabaseName + ".dbo.Usuarios WHERE Usuario = '" + usuario + "' AND Password = '" + HashPassword + "'"

            Query.Add(sqlQuery)
            myCmd = New SqlCommand(sqlQuery, myConn)
            myReader = myCmd.ExecuteReader()

            If myReader.HasRows Then

                Dim ListaRegistros As List(Of Establecimiento) = New List(Of Establecimiento)
                Do While myReader.Read()

                    respuesta.PermiteLogin = True
                    respuesta.mensaje = myReader.GetValue(myReader.GetOrdinal("Usuario"))

                    Dim Categoria As String = IIf(IsDBNull(myReader.GetValue(myReader.GetOrdinal("Categoria"))), "", myReader.GetValue(myReader.GetOrdinal("Categoria")))
                    If Categoria = "Administrador" Then
                        respuesta.EsAdministrador = True
                    Else
                        respuesta.EsAdministrador = False
                    End If
                Loop
                respuesta.ConsultaExitosa = True
            Else
                respuesta.PermiteLogin = False
                respuesta.mensaje = "Usuario o clave incorrectos"
            End If
            myReader.Close()

        Catch ex As Exception
            respuesta.mensaje = "Error BD iniciando sesión de usuario"
        End Try

        Return respuesta
    End Function
    Public Sub SetNombreBase(ByVal Name As String)
        NombreBase = Name
    End Sub
    Public Function DoLoginSupervisor(ByVal usuario As String, ByVal clave As String, ByVal GrupoId As String) As ResponseLogin
        Dim Crip As Encripta = New Encripta
        Dim HashPassword As String = Crip.Crypt(clave) ' Encripto para comparar con BD
        Dim respuesta As New ResponseLogin

        Try
            rs.Source = "SELECT * FROM Usuarios WHERE UsuarioId = '" + usuario + "' AND Password = '" + HashPassword + "'"
            Query.Add(rs.Source)
            rs.Abrir()
            If Not rs.EOF Then
                'respuesta.PermiteLogin = True
                respuesta.mensaje = rs("usuarioId").Valor
                rs.Cerrar()

                ' Determino si el usuario corresponde al grupo de administradores
                rs.Source = "SELECT COUNT(*) EsAdministrador FROM RelUsuariosGrupos WHERE GrupoId = '" + GrupoId + "' AND UsuarioId = '" + usuario + "'"
                Query.Add(rs.Source)
                rs.Abrir()
                If Not rs.EOF() Then
                    If Val(rs("EsAdministrador").Valor) > 0 Then
                        respuesta.PermiteLogin = True
                    Else
                        respuesta.PermiteLogin = False
                        respuesta.mensaje = "El usuario ingresado no tiene permisos suficientes"
                    End If
                Else
                    respuesta.mensaje = "El usuario ingresado no tiene permisos suficientes"
                    respuesta.PermiteLogin = False
                End If
                rs.Cerrar()
            Else
                respuesta.PermiteLogin = False
                respuesta.mensaje = "Usuario o clave incorrectos"
            End If
        Catch ex As Exception
            respuesta.mensaje = "Error BD iniciando sesión de usuario"
        End Try

        Return respuesta
    End Function

    Public Function LeerEntidadesMailings(ByVal EstablecimientoId As String) As ResponseEntidades
        Dim respuesta As New ResponseEntidades

        Try
            Dim sqlQuery As String = "SELECT TOP 1 * FROM " + NombreBaseEnsemble + ".dbo.EntidadesMailings WHERE EstablecimientoId = " + EstablecimientoId + " or EstablecimientoId is null order by EstablecimientoId DESC"
            Query.Add(sqlQuery)
            myCmd = New SqlCommand(sqlQuery, myConn)
            myReader = myCmd.ExecuteReader()
            If myReader.HasRows Then
                Do While myReader.Read()

                    ' Obtengo la configuracion del Mail por Defecto
                    respuesta.Email = myReader.GetValue(myReader.GetOrdinal("Email"))
                    respuesta.UID = myReader.GetValue(myReader.GetOrdinal("UID"))
                    respuesta.PWD = myReader.GetValue(myReader.GetOrdinal("PWD"))
                    respuesta.SMTPServer = myReader.GetValue(myReader.GetOrdinal("SMTPServer"))
                    respuesta.Puerto = myReader.GetValue(myReader.GetOrdinal("Puerto"))
                    respuesta.UsaSSL = myReader.GetValue(myReader.GetOrdinal("UsaSSL"))
                Loop
                respuesta.ConsultaExitosa = True
            Else
                respuesta.mensaje = "Error DB consultando configuración Mail por Defecto"
            End If
            myReader.Close()
        Catch ex As Exception
            respuesta.mensaje = "Error DB consultando configuración Mail por Defecto"
        End Try
        Return respuesta
    End Function
    Public Function GetRazonSocial() As ResponseRazonSocial
        Dim respuesta As New ResponseRazonSocial

        Try
            Dim sqlQuery As String = "SELECT TOP 1 * FROM " + NombreBase + ".dbo.Parametros WHERE Variable = 'ssEmpresa'"

            Query.Add(sqlQuery)
            myCmd = New SqlCommand(sqlQuery, myConn)
            myReader = myCmd.ExecuteReader()
            If myReader.HasRows Then
                Do While myReader.Read()
                    respuesta.RazonSocial = Trim(myReader.GetValue(myReader.GetOrdinal("Valor")))
                Loop
                respuesta.ConsultaExitosa = True
            Else
                respuesta.mensaje = "No se encontro el parametro Razon Social de la Empresa, comuniquese con el Administrador"
            End If
            myReader.Close()

        Catch ex As Exception
            respuesta.mensaje = "Error DB consultando Razon Social"
        End Try

        Return respuesta
    End Function
    Public Function GetCUITEmpresa() As ResponseCuit
        Dim respuesta As New ResponseCuit

        Try
            Dim sqlQuery As String = "SELECT * FROM " + NombreBase + ".dbo.Parametros Configuracion WHERE Variable = 'ssCUITFCe' "

            myCmd = New SqlCommand(sqlQuery, myConn)
            myReader = myCmd.ExecuteReader()
            If myReader.HasRows Then
                Do While myReader.Read()
                    respuesta.CUIT = myReader.GetValue(myReader.GetOrdinal("Valor"))
                Loop
                respuesta.ConsultaExitosa = True
            Else
                respuesta.mensaje = "No se encontro el parametro CUIT de la Empresa, comuniquese con el Administrador"
            End If
            myReader.Close()
        Catch ex As Exception
            respuesta.mensaje = "Error BD No se encuentra el campo CUIT en los parametros de la empresa"
        End Try

        Return respuesta
    End Function

    Public Function VerificarFabrica(ByVal CUITEnsemble As String) As ResponseVerificar
        Dim respuesta As New ResponseVerificar

        Try
            rs.Source = "SELECT * FROM Configuracion WHERE Parametro = 'CUIT' AND Valor = '" + CUITEnsemble + "'"
            Query.Add(rs.Source)
            rs.Abrir()
            If Not rs.EOF Then respuesta.EsFabrica = True
            rs.Cerrar()

            respuesta.ConsultaExitosa = True
        Catch ex As Exception
            respuesta.mensaje = "Error BD verificando si es Fabrica"
        End Try

        Return respuesta
    End Function
    Public Function GetAllEstablecimientos() As ResponseEstablecimiento
        Dim respuesta As New ResponseEstablecimiento
        Dim sqlQuery As String

        Try
            Dim NombreTablaEstablecimiento As String = "GWREstablecimientos"
            Dim EsPropio As Boolean = False

            sqlQuery = "SELECT e.EstablecimientoId, e.Descripcion, e.Propio, e.DbName, e.SucursalId
                    FROM " + NombreBaseEnsemble + ".dbo." + NombreTablaEstablecimiento + " e WHERE Alta = 1 ORDER BY e.Descripcion "

            myCmd = New SqlCommand(sqlQuery, myConn)
            myReader = myCmd.ExecuteReader()
            If myReader.HasRows Then

                Dim ListaRegistros As List(Of Establecimiento) = New List(Of Establecimiento)
                Do While myReader.Read()
                    EsPropio = IIf(Boolean.TryParse(myReader.GetValue(myReader.GetOrdinal("Propio")), EsPropio), EsPropio, False)
                    ListaRegistros.Add(New Establecimiento(myReader.GetValue(myReader.GetOrdinal("EstablecimientoId")), myReader.GetValue(myReader.GetOrdinal("Descripcion")), EsPropio, myReader.GetValue(myReader.GetOrdinal("DbName")), myReader.GetValue(myReader.GetOrdinal("SucursalId"))))
                Loop

                respuesta.rs = ListaRegistros
                respuesta.ConsultaExitosa = True
            Else
                respuesta.mensaje = "No hay establecimientos registrados en ENSEMBLE"
            End If
            myReader.Close()
        Catch ex As Exception
            respuesta.mensaje = "Error BD al consultar los establecimientos"
        End Try

        Return respuesta
    End Function
    Public Function GetTipoFacturacionEstablecimiento(ByVal EstablecimientoId As String, ByVal EsFabrica As Boolean) As ResponsePorcFacturacion
        Dim respuesta As New ResponsePorcFacturacion
        Dim sqlQuery As String

        Try

            Dim NombreTablaEstablecimiento As String = "Establecimientos"
            If EsFabrica Then
                NombreTablaEstablecimiento = "GWREstablecimientos"
            End If

            'update dbo.GWREstablecimientos set TipoFacturacion = '50/50' where EstablecimientoId = 1
            sqlQuery = "Select ISNULL(PorcFactura, 0)  PorcFactura from [" + NombreBaseEnsemble + "].dbo." + NombreTablaEstablecimiento + " where EstablecimientoId = " + EstablecimientoId

            rs.Source = sqlQuery

            Query.Add(rs.Source)
            rs.Abrir()
            If Not rs.EOF Then
                Do While Not rs.EOF
                    respuesta.PorFacturacion = Convert.ToDouble(rs("PorcFactura").Valor)
                    rs.MoveNext()
                Loop
                respuesta.ConsultaExitosa = True
            Else
                respuesta.ConsultaExitosa = True
                respuesta.mensaje = "El establecimiento no tiene un porcentaje de factura"
            End If
            rs.Cerrar()
        Catch ex As Exception
            respuesta.mensaje = "Error BD al consultar los establecimientos del usuario"
        End Try

        Return respuesta
    End Function

    Public Function UpdateTipoFacturacionEstablecimiento(ByVal EstablecimientoId As String, ByVal PorcentajeFacturacion As Double, ByVal EsFabrica As Boolean) As ResponseEstablecimiento
        Dim respuesta As New ResponseEstablecimiento
        Dim sqlQuery As String
        Try
            Dim NombreTablaEstablecimiento As String = "Establecimientos"
            If EsFabrica Then
                NombreTablaEstablecimiento = "GWREstablecimientos"
            End If

            'update dbo.GWREstablecimientos set TipoFacturacion = '50/50' where EstablecimientoId = 1
            sqlQuery = "UPDATE [" + NombreBaseEnsemble + "].dbo." + NombreTablaEstablecimiento + " set PorcFactura = " + PorcentajeFacturacion.ToString() + " where EstablecimientoId = " + EstablecimientoId

            Query.Add(rs.Source)
            conn.Execute(sqlQuery)

            respuesta.ConsultaExitosa = True
            respuesta.mensaje = "Actualizacion de facturacion exitosa"

        Catch ex As Exception
            respuesta.ConsultaExitosa = False
            respuesta.mensaje = "Error BD al consultar los establecimientos del usuario"
        End Try
        Return respuesta
    End Function

    Public Function GetArbolClasificaciones(ByVal IdsClasificaciones As List(Of String), ByVal EstablecimientoId As String, ByVal NombreClasificacionARealizarPedidos As String) As ResponseClasificaciones
        Dim respuesta As New ResponseClasificaciones
        Dim sqlQuery As String

        Try
            sqlQuery = "SELECT ch.ClasificacionProdId, ch.Descripcion, cp.ClasificacionProdId ClasificacionProdPadre  "
            If EstablecimientoId = "" Then
                sqlQuery += ", 0 as CantDiasReparo, 0 as FrecuenciaPed, 0 as PlazoEntregaPed, 0 as KilosMin, 0 as EstablecimientoIdDistribucion "
            Else
                sqlQuery += ", cpp.CantDiasReparo, cpp.FrecuenciaPed, cpp.PlazoEntregaPed, cpp.KilosMin, cpp.EstablecimientoIdDistribucion "
            End If

            sqlQuery += " FROM " + NombreBaseEnsemble + ".dbo.ClasificacionesProductos cp
                JOIN " + NombreBaseEnsemble + ".dbo.RelArbolClasifProductos ra ON ra.ClasifProdIdPadre = cp.ClasificacionProdId
                JOIN " + NombreBaseEnsemble + ".dbo.ClasificacionesProductos ch ON ch.ClasificacionProdId = ra.ClasifProdIdHijo "

            If EstablecimientoId <> "" Then
                sqlQuery += "Left JOIN " + NombreBaseEnsemble + ".dbo.ConfigPedidoProveedor cpp ON cpp.EstablecimientoId = " + EstablecimientoId + " AND cpp.NombreProveedor = ch.Descripcion "
            End If


            If IdsClasificaciones.Count > 0 Then
                ' Si recibo una lista de ids es porque quiero los hijos de determiandas clasificaciones
                Dim strIds As String = ""
                For Each IdClas As String In IdsClasificaciones
                    strIds += IdClas + ", "
                Next
                'Elimino el ultimo ", "
                strIds = strIds.Substring(0, strIds.Length - 2)
                sqlQuery += "WHERE cp.ClasificacionProdId IN (" + strIds + ") ORDER BY ch.Descripcion"
            Else
                'Si no recibo ningun ids obtengo el arbol raiz
                sqlQuery += "WHERE cp.Descripcion = '" + NombreClasificacionARealizarPedidos + "' ORDER BY ch.Descripcion"
            End If


            Query.Add(sqlQuery)
            myCmd = New SqlCommand(sqlQuery, myConn)
            myReader = myCmd.ExecuteReader()
            If myReader.HasRows Then
                Dim ListaRegistros As List(Of Clasificacion) = New List(Of Clasificacion)
                Dim CantDiasReparo As Integer
                Dim FrecuenciaPed As Integer
                Dim PlazoEntregaPed As Integer
                Dim KilosMin As Integer
                Dim Distribuidor As String

                Do While myReader.Read()
                    CantDiasReparo = If(IsDBNull(myReader.GetValue(myReader.GetOrdinal("CantDiasReparo"))), 0, Integer.Parse(myReader.GetValue(myReader.GetOrdinal("CantDiasReparo"))))
                    FrecuenciaPed = If(IsDBNull(myReader.GetValue(myReader.GetOrdinal("FrecuenciaPed"))), 0, Integer.Parse(myReader.GetValue(myReader.GetOrdinal("FrecuenciaPed"))))
                    PlazoEntregaPed = If(IsDBNull(myReader.GetValue(myReader.GetOrdinal("PlazoEntregaPed"))), 0, Integer.Parse(myReader.GetValue(myReader.GetOrdinal("PlazoEntregaPed"))))
                    KilosMin = If(IsDBNull(myReader.GetValue(myReader.GetOrdinal("KilosMin"))), 0, Integer.Parse(myReader.GetValue(myReader.GetOrdinal("KilosMin"))))
                    Distribuidor = If(IsDBNull(myReader.GetValue(myReader.GetOrdinal("EstablecimientoIdDistribucion"))), "", myReader.GetValue(myReader.GetOrdinal("EstablecimientoIdDistribucion")))

                    Dim currentClass = New Clasificacion(myReader.GetValue(myReader.GetOrdinal("ClasificacionProdId")), myReader.GetValue(myReader.GetOrdinal("Descripcion")), myReader.GetValue(myReader.GetOrdinal("ClasificacionProdPadre")), CantDiasReparo, FrecuenciaPed, PlazoEntregaPed, KilosMin, Distribuidor)

                    ListaRegistros.Add(currentClass)
                Loop

                respuesta.ConsultaExitosa = True
                respuesta.rs = ListaRegistros
            Else
                respuesta.ConsultaExitosa = True
                respuesta.mensaje = "No se encontraron clasificaciones para realizar pedidos"
            End If
            myReader.Close()
        Catch ex As Exception
            respuesta.mensaje = "Error BD consultando el arbol de clasificaciones"
        End Try

        Return respuesta
    End Function
    Public Function GetEmailsClasificaciones(ByVal EstablecimientoId As String) As ResponseEmailClasificaciones
        Dim respuesta As New ResponseEmailClasificaciones

        Try
            Dim sqlQuery As String = "SELECT NombreProveedor, Descripcion, Email, EstablecimientoId
            FROM " + NombreBaseEnsemble + ".[dbo].[ConfigEmailsProveedor] WHERE EstablecimientoId is null or EstablecimientoId = " + EstablecimientoId

            Query.Add(sqlQuery)
            myCmd = New SqlCommand(sqlQuery, myConn)
            myReader = myCmd.ExecuteReader()
            If myReader.HasRows Then
                Dim ListaRegistros As List(Of EmailClasificacion) = New List(Of EmailClasificacion)
                Do While myReader.Read()
                    ListaRegistros.Add(New EmailClasificacion(myReader.GetValue(myReader.GetOrdinal("Descripcion")), myReader.GetValue(myReader.GetOrdinal("NombreProveedor")), myReader.GetValue(myReader.GetOrdinal("Email"))))
                Loop
                respuesta.rs = ListaRegistros
                respuesta.ConsultaExitosa = True
            Else
                respuesta.mensaje = "La configuracion de email de proveedores no es correcta"
            End If
            myReader.Close()
        Catch ex As Exception
            respuesta.mensaje = "Error al establecer la conexion con la base de datos"
        End Try

        Return respuesta
    End Function

    Public Function GetDepositosUsuario(ByVal UsuarioId As String, ByVal SucursalId As String) As ResponseDeposito
        Dim respuesta As New ResponseDeposito
        Dim sqlQuery As String

        Try
            sqlQuery = "SELECT IdDepositos , Nombre , IdSucursales FROM " + NombreBase + ".dbo.Depositos e WHERE IdSucursales = " + SucursalId + " ORDER BY Nombre ASC "

            myCmd = New SqlCommand(sqlQuery, myConn)
            myReader = myCmd.ExecuteReader()
            If myReader.HasRows Then
                Dim ListaRegistros As New List(Of Deposito)
                Do While myReader.Read()
                    ListaRegistros.Add(New Deposito(myReader.GetValue(myReader.GetOrdinal("IdDepositos")), myReader.GetValue(myReader.GetOrdinal("Nombre"))))
                Loop
                respuesta.rs = ListaRegistros
                respuesta.ConsultaExitosa = True
            Else
                respuesta.mensaje = "El usuario no tiene depositos asociados"
            End If
            myReader.Close()
        Catch ex As Exception
            respuesta.mensaje = "Error BD consultando los depositos del usuario"
        End Try

        Return respuesta
    End Function

    Public Function GetDepositos() As ResponseDeposito
        Dim respuesta As New ResponseDeposito

        Try
            rs.Source = "SELECT DepositoId, Descripcion FROM Depositos"
            Query.Add(rs.Source)
            rs.Abrir()
            If Not rs.EOF Then
                Dim ListaRegistros As New List(Of Deposito)
                Do While Not rs.EOF
                    ListaRegistros.Add(New Deposito(rs("DepositoId").Valor, rs("Descripcion").Valor))
                    rs.MoveNext()
                Loop

                respuesta.ConsultaExitosa = True
                respuesta.rs = ListaRegistros
            Else
                respuesta.mensaje = "No existen depositos"
            End If
            rs.Cerrar()
        Catch ex As Exception
            respuesta.mensaje = "Error BD consultando todos los depositos"
        End Try

        Return respuesta
    End Function

    Public Function GetSucursalesUsuario(ByVal UsuarioId As String, ByVal EstablecimientoId As String, ByVal EsFabrica As Boolean, ByVal EsAdministrador As Boolean) As ResponseSucursal
        Dim respuesta As New ResponseSucursal
        Dim sqlQuery As String

        Try
            Dim NombreTablaRelEstablecimiento As String = "RelEstablecimientosSucursales"
            If EsFabrica = True Then NombreTablaRelEstablecimiento = "GWRRelEstablecimientosSucursales"

            If EsAdministrador Then
                sqlQuery = "SELECT s.SucursalId, s.Descripcion, s.LlevaPercep, s.Tipo FROM Sucursales s 
                    JOIN " + NombreBaseEnsemble + ".dbo." + NombreTablaRelEstablecimiento + " res ON res.SucursalId = s.SucursalId 
                    WHERE s.Suspendida = 0 AND res.EstablecimientoId = " + EstablecimientoId
            Else
                sqlQuery = "SELECT s.SucursalId, s.Descripcion, s.LlevaPercep, s.Tipo FROM Sucursales s
                    JOIN RelUsuariosSucursales rus ON rus.SucursalId = s.SucursalId 
                    JOIN " + NombreBaseEnsemble + ".dbo." + NombreTablaRelEstablecimiento + " res ON res.SucursalId = rus.SucursalId
                    WHERE s.Suspendida = 0 AND rus.UsuarioId = '" + UsuarioId + "' AND res.EstablecimientoId = " + EstablecimientoId
            End If

            rs.Source = sqlQuery
            Query.Add(rs.Source)
            rs.Abrir()
            If Not rs.EOF Then
                Dim ListaRegistros As New List(Of Sucursal)
                Do While Not rs.EOF
                    ListaRegistros.Add(New Sucursal(rs("SucursalId").Valor, rs("Descripcion").Valor, rs("LlevaPercep").Valor, rs("Tipo").Valor))
                    rs.MoveNext()
                Loop

                respuesta.ConsultaExitosa = True
                respuesta.rs = ListaRegistros
            Else
                respuesta.mensaje = "El usuario no tiene sucursales asociadas"
            End If
            rs.Cerrar()
        Catch ex As Exception
            respuesta.mensaje = "Error BD consultando las sucursales del usuario"
        End Try

        Return respuesta
    End Function

    Public Function GetDireccionesCliente(ByVal ClienteId As String, SoloDireccionesEntrega As Boolean) As ResponseDireccionesCliente
        Dim respuesta As New ResponseDireccionesCliente
        Dim sqlQuery As String

        Try

            sqlQuery = "Select CD.ClienteId, CD.DireccionId, CD.Calle, CD.Numero, CD.Piso, CD.Depto, CD.Localidad, p.Descripcion as Provincia, CD.Entrega   
                        from RelClientesDirecciones CD 
                        Inner join Gestion.dbo.Provincias P on p.PaisId = cd.PaisId and p.ProvinciaId = cd.ProvinciaId where ClienteId = '" + ClienteId + "' "

            If SoloDireccionesEntrega Then
                sqlQuery += " And Entrega = 1 "
            End If

            rs.Source = sqlQuery
            Query.Add(rs.Source)
            rs.Abrir()
            If Not rs.EOF Then
                Dim ListaRegistros As New List(Of DireccionCliente)
                Do While Not rs.EOF
                    Dim calle As String = If(IsDBNull(rs("Calle").Valor), "", rs("Calle").Valor)
                    Dim numero As String = If(IsDBNull(rs("Numero").Valor), "", rs("Numero").Valor)
                    Dim piso As String = If(IsDBNull(rs("Piso").Valor), "", rs("Piso").Valor)
                    Dim depto As String = If(IsDBNull(rs("Depto").Valor), "", rs("Depto").Valor)
                    Dim Loc As String = If(IsDBNull(rs("Localidad").Valor), "", rs("Localidad").Valor)
                    Dim prov As String = If(IsDBNull(rs("Provincia").Valor), "", rs("Provincia").Valor)
                    Dim Entrega As Boolean = If(IsDBNull(rs("Entrega").Valor), "", Boolean.Parse(rs("Entrega").Valor))

                    ListaRegistros.Add(New DireccionCliente(rs("ClienteId").Valor, rs("DireccionId").Valor, calle, numero, piso, depto, Loc, prov, Entrega))
                    rs.MoveNext()
                Loop

                respuesta.ConsultaExitosa = True
                respuesta.rs = ListaRegistros
            Else
                respuesta.mensaje = "El cliente no tiene direcciones asociadas"
            End If
            rs.Cerrar()
        Catch ex As Exception
            respuesta.mensaje = "Error BD consultando las sucursales del usuario"
        End Try

        Return respuesta
    End Function

    Public Shared Function GetNombreProveedorSinPrefijo(ByVal NombreProveedor As String) As String
        Dim pos As Integer
        Dim NombreSinPrefijo As String = NombreProveedor
        pos = NombreProveedor.IndexOf(" ")
        If pos >= 0 Then
            NombreSinPrefijo = NombreProveedor.Substring(pos + 1)
        End If
        Return NombreSinPrefijo
    End Function

    ''' <summary>
    ''' Envia un mail con los parametros informados.
    ''' </summary>
    ''' <param name="ListEnviarA">Lista de Emails a donde se quiere enviar</param>
    ''' <param name="Asunto">Descripcion corta del contenido del Email</param>
    ''' <param name="Cuerpo">Contenido detallado del Email</param>
    ''' <param name="ListAdjuntos">Lista de rutas absolutas de los archivos a adjuntar</param>
    ''' <return>Devulve True en caso de exito.</return>
    Public Shared Function EnviarMail(ListEnviarA As List(Of String), Asunto As String, Cuerpo As String, Optional ListAdjuntos As List(Of String) = Nothing) As Boolean
        Try
            Dim Email As New MailMessage()
            Dim SmtpServer As New SmtpClient(SMTP)

            Email.From = New MailAddress(MailFromAddress)
            Email.To.Add(String.Join(",", ListEnviarA))
            Email.Subject = Asunto

            Email.IsBodyHtml = Cuerpo.Contains("</")
            Email.Body = Cuerpo

            If ListAdjuntos IsNot Nothing Then
                For Each RutaAdjunto In ListAdjuntos
                    If (My.Computer.FileSystem.FileExists(RutaAdjunto)) Then
                        Email.Attachments.Add(New Attachment(RutaAdjunto))
                    End If
                Next
            End If

            If SMTP.Contains("gmail") Then
                SmtpServer.Port = 587
            Else
                SmtpServer.Port = MailPort
            End If

            SmtpServer.Credentials = New NetworkCredential(MailUsername, MailPassword)
            SmtpServer.EnableSsl = MailEnableSSL

            SmtpServer.Send(Email)

            Email.Dispose()
            SmtpServer.Dispose()

            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Function GetParametroConfiguracion(ByVal NombreParametro) As ResponseConfiguracion
        Dim respuesta As New ResponseConfiguracion
        Dim sqlQuery As String

        Try
            sqlQuery = "SELECT Parametro , Tipo, Valor  
                    FROM " + NombreBaseEnsemble + ".dbo.[GWRConfiguracion]  
                    WHERE Parametro = '" + NombreParametro + "'"


            myCmd = New SqlCommand(sqlQuery, myConn)

            myReader = myCmd.ExecuteReader()
            If myReader.HasRows Then
                Do While myReader.Read()
                    'MsgBox(myReader.GetValue(myReader.GetOrdinal("Nombre")))
                    respuesta.Tipo = myReader.GetValue(myReader.GetOrdinal("Tipo"))
                    respuesta.Parametro = myReader.GetValue(myReader.GetOrdinal("Parametro"))
                    respuesta.Valor = myReader.GetValue(myReader.GetOrdinal("Valor"))

                Loop
                respuesta.ConsultaExitosa = True
            Else
                respuesta.mensaje = "No existe el parametro de configuracion"
            End If

            myReader.Close()
        Catch ex As Exception
            respuesta.mensaje = "Error BD consultando el parametro de configuracion"
        End Try

        Return respuesta
    End Function
    Public Function GetCurrentEjercicioContable() As ResponseWS
        Dim respuesta As New ResponseWS
        Dim sqlQuery As String

        Try
            sqlQuery = "SELECT EjercicioId  
                    FROM Ejercicios  
                    WHERE  EnUso = 1"

            rs.Source = sqlQuery
            Query.Add(sqlQuery)
            rs.Abrir()
            If Not rs.EOF Then
                Do While Not rs.EOF
                    respuesta.mensaje = rs("EjercicioId").Valor
                    rs.MoveNext()
                Loop
                respuesta.ConsultaExitosa = True
            Else
                respuesta.mensaje = "No existen Ejercicios Contables"
            End If
            rs.Cerrar()
        Catch ex As Exception
            respuesta.mensaje = "Error BD consultando todos los Ejercicios contables"
        End Try

        Return respuesta
    End Function

    Public Function GetProximoAsientoContable(ByVal NumeroEjercicio As String) As ResponseWS
        Dim respuesta As New ResponseWS
        Dim sqlQuery As String

        Try
            sqlQuery = "	SELECT (ISNULL(MAX(NroAsiento),0) + 1) as Numero FROM Asientos
                    WHERE EjercicioId = " + NumeroEjercicio

            rs.Source = sqlQuery
            Query.Add(sqlQuery)
            rs.Abrir()
            If Not rs.EOF Then
                Dim ListaRegistros As New List(Of Deposito)
                Do While Not rs.EOF
                    respuesta.mensaje = rs("Numero").Valor
                    rs.MoveNext()
                Loop
                respuesta.ConsultaExitosa = True
            Else
                respuesta.mensaje = "No existen Asientos"
            End If
            rs.Cerrar()
        Catch ex As Exception
            respuesta.mensaje = "Error BD consultando todos los Asientos"
        End Try

        Return respuesta
    End Function

    Public Function GetConfiguracionesAsientos() As responseConfigAsientos
        Dim respuesta As New responseConfigAsientos
        Dim sqlQuery As String

        Try
            sqlQuery = "SELECT ConfigId, Descripcion,CuentaIdHaber,SubCuentaIdHaber,CuentaIdDebe,SubCuentaIdDebe,TipoAsiento, ConceptoAsiento  
                    FROM " + NombreBaseEnsemble + ".dbo.ConfigAsientos "

            rs.Source = sqlQuery
            Query.Add(sqlQuery)
            rs.Abrir()
            If Not rs.EOF Then
                Do While Not rs.EOF
                    Dim Element As New ConfigAsiento

                    Element.configId = rs("ConfigId").Valor
                    Element.Descripcion = rs("Descripcion").Valor
                    Element.CuentaIdHaber = rs("CuentaIdHaber").Valor
                    Element.SubCuentaIdHaber = rs("SubCuentaIdHaber").Valor
                    Element.CuentaIdDebe = rs("CuentaIdDebe").Valor
                    Element.SubCuentaIdDebe = rs("SubCuentaIdDebe").Valor
                    Element.TipoAsiento = rs("TipoAsiento").Valor
                    Element.ConceptoAsiento = rs("ConceptoAsiento").Valor

                    respuesta.Items.Add(Element)

                    rs.MoveNext()
                Loop
                respuesta.ConsultaExitosa = True
            Else
                respuesta.mensaje = "No existen configuraciones de asientos"
            End If
            rs.Cerrar()
        Catch ex As Exception
            respuesta.mensaje = "Error BD consultando todos los configuracion de asientos"
        End Try

        Return respuesta
    End Function


    Public Function GetCliente(ByVal RazonSocial As String, ByVal DepositoOEstablecimiento As String, ByVal CUIT As String, ByVal CUITEnsemble As String) As ResponseCliente
        Dim respuesta As New ResponseCliente

        Try
            ' Si el CUIT es de ENSEMBLE traigo el primer cliente de la BD y no me interesa cargar las percepciones
            If CUIT.Replace("-", "") = CUITEnsemble Then
                rs.Source = "SELECT TOP 1 c.ClienteId, c.RazonSocial, c.PosIVAId, c.ListaPrecioId, c.CUIT, r.DireccionId,
                    ISNULL(c.VendedorId, '') VendedorId, c.CondicionVentaId, ISNULL(c.JurisdiccionIIBBId, '') JurisdiccionIIBBId, 
                    c.CUIT, ISNULL(c.Recordatorio, '') Recordatorio, ISNULL(CtrlCredito, 0) CtrlCredito
                    FROM Clientes c 
                    JOIN RelClientesDirecciones r ON r.ClienteId = c.ClienteId AND r.Fiscal = 1"
                Query.Add(rs.Source)
                rs.Abrir()

                If Not rs.EOF Then
                    respuesta.ConsultaExitosa = True
                    respuesta.Cliente = New Cliente(rs("ClienteId").Valor, rs("RazonSocial").Valor, CInt(rs("PosIVAId").Valor),
                                                    CInt(rs("ListaPrecioId").Valor), CInt(rs("DireccionId").Valor), rs("VendedorId").Valor,
                                                    CInt(rs("CondicionVentaId").Valor), rs("JurisdiccionIIBBId").Valor, CUITEnsemble,
                                                    rs("Recordatorio").Valor, rs("CtrlCredito").Valor)

                    respuesta.Cliente.ListEstablecimientos.Add(New EstablecimientoCliente(1, "Ensemble S.R.L", 100, CInt(rs("DireccionId").Valor)))
                    rs.Cerrar()
                End If

                Return respuesta
            End If

            ' Busco a un cliente con la Razon Social y el Deposito o Establecimiento que me pasan porque un mismo cliente puede tener mas de un local
            rs.Source = "SELECT c.ClienteId, c.RazonSocial, c.PosIVAId, c.ListaPrecioId, r.DireccionId, 
                    ISNULL(c.VendedorId, '') VendedorId, c.CondicionVentaId, ISNULL(c.JurisdiccionIIBBId, '') JurisdiccionIIBBId, 
                    c.CUIT, ISNULL(c.Recordatorio, '') Recordatorio, ISNULL(CtrlCredito, 0) CtrlCredito
                    FROM Clientes c
                    JOIN RelClientesDirecciones r ON r.ClienteId = c.ClienteId AND r.Fiscal = 1                    
                    LEFT JOIN RelClientesEscalasPercepciones rcep ON rcep.ClienteId = c.ClienteId "

            ' Me pasaron un establecimiento si lo puedo convertir en numero...
            If Val(DepositoOEstablecimiento) > 0 Then
                rs.Source &= "JOIN RelClientesContactos re ON re.ClienteId = c.ClienteId AND re.TipoContactoClienteId = 6
                    WHERE c.RazonSocial LIKE '%" + RazonSocial + "%' AND re.Celular = '" + DepositoOEstablecimiento + "' AND c.CUIT = '" + CUIT + "'"
            Else
                ' ...sino es el nombre del establecimiento
                rs.Source &= "WHERE c.RazonSocial LIKE '%" + RazonSocial + "%' AND c.RazonSocial LIKE '%(" + DepositoOEstablecimiento + "%' AND c.CUIT = '" + CUIT + "'"
            End If

            Query.Add(rs.Source)
            rs.Abrir()

            If Not rs.EOF Then
                respuesta.ConsultaExitosa = True
                respuesta.Cliente = New Cliente(rs("ClienteId").Valor, rs("RazonSocial").Valor, CInt(rs("PosIVAId").Valor),
                                                CInt(rs("ListaPrecioId").Valor), CInt(rs("DireccionId").Valor), rs("VendedorId").Valor,
                                                CInt(rs("CondicionVentaId").Valor), rs("JurisdiccionIIBBId").Valor, rs("CUIT").Valor,
                                                rs("Recordatorio").Valor, rs("CtrlCredito").Valor)
                rs.Cerrar()
            Else
                ' No encuentro al cliente por Razon Social y Deposito, lo busco solo por Razon Social
                rs.Cerrar()
                rs.Source = "SELECT c.ClienteId, c.RazonSocial, c.PosIVAId, c.ListaPrecioId, r.DireccionId, 
                    ISNULL(c.VendedorId, '') VendedorId, c.CondicionVentaId, ISNULL(c.JurisdiccionIIBBId, '') JurisdiccionIIBBId, 
                    c.CUIT, ISNULL(c.Recordatorio, '') Recordatorio, ISNULL(CtrlCredito, 0) CtrlCredito
                    FROM Clientes c
                    JOIN RelClientesDirecciones r ON r.ClienteId = c.ClienteId AND r.Fiscal = 1                    
                    WHERE c.RazonSocial LIKE '%" + RazonSocial + "%' AND c.CUIT = '" + CUIT + "'"
                Query.Add(rs.Source)
                rs.Abrir()

                If Not rs.EOF Then
                    respuesta.ConsultaExitosa = True
                    respuesta.Cliente = New Cliente(rs("ClienteId").Valor, rs("RazonSocial").Valor, CInt(rs("PosIVAId").Valor),
                                                    CInt(rs("ListaPrecioId").Valor), CInt(rs("DireccionId").Valor), rs("VendedorId").Valor,
                                                    CInt(rs("CondicionVentaId").Valor), rs("JurisdiccionIIBBId").Valor, rs("CUIT").Valor,
                                                    rs("Recordatorio").Valor, rs("CtrlCredito").Valor)
                Else
                    respuesta.mensaje = "No se pudo encontrar al cliente con Razon Social " & RazonSocial & ", Establecimiento " & DepositoOEstablecimiento & " y CUIT " & CUIT
                End If
                rs.Cerrar()
            End If
        Catch ex As Exception
            respuesta.mensaje = "Error BD consultando al cliente con Razon Social " & RazonSocial & ", Establecimiento " & DepositoOEstablecimiento & " y CUIT " & CUIT
        End Try

        If respuesta.ConsultaExitosa Then
            Try
                ' Traigo todas las Escalas de Perpceciones y las alicuotas particulares del cliente de existir
                rs.Source = "SELECT ep.EscalaPercepcionId, ep.TipoPercepcion, ep.Base, ep.TomaBaseNoImpo, ep.Descripcion,
                    ISNULL(ep.JurisdiccionIIBBId,'') JurisdiccionIIBBId, 
                    CASE WHEN rce.PorcPercepParticular IS NULL THEN ep.PorcPercep ELSE rce.PorcPercepParticular END PorcPercep 
                    FROM EscalasPercepciones ep
                    LEFT JOIN (SELECT * FROM RelClientesEscalasPercepciones WHERE ClienteId = '" + respuesta.Cliente.ClienteId + "') rce ON ep.EscalaPercepcionId = rce.EscalaPercepcionId"
                Query.Add(rs.Source)
                rs.Abrir()

                If Not rs.EOF Then
                    Dim ListaRegistros As New List(Of EscalaPercepcion)
                    Do While Not rs.EOF
                        ListaRegistros.Add(New EscalaPercepcion(rs("EscalaPercepcionId").Valor, rs("TipoPercepcion").Valor, rs("Base").Valor, rs("TomaBaseNoImpo").Valor,
                                                                rs("PorcPercep").Valor, rs("Descripcion").Valor, rs("JurisdiccionIIBBId").Valor))
                        rs.MoveNext()
                    Loop
                    rs.Cerrar()

                    respuesta.Cliente.ListEscalasPecep = ListaRegistros
                End If
            Catch ex As Exception
                respuesta.ConsultaExitosa = False
                respuesta.mensaje = "Error BD consultando las escalas de percepciones para el Cliente (" + respuesta.Cliente.ClienteId + ") " + respuesta.Cliente.RazonSocial
            End Try

            Try
                ' Traigo todos los establecimientos asociados al cliente de existir
                rs.Source = "SELECT e.EstablecimientoId, e.Descripcion, e.PorcFactura, ISNULL(rcc.DireccionId, -1) DireccionId, ISNULL(rcd.Descripcion, '') DescDireccion
                    FROM " & NombreBaseEnsemble & ".dbo.GWREstablecimientos e 
                    JOIN RelClientesContactos rcc ON rcc.Celular = e.EstablecimientoId
                    LEFT JOIN RelClientesDirecciones rcd ON rcd.ClienteId = rcc.ClienteId AND rcd.DireccionId = rcc.DireccionId
                    WHERE rcc.ClienteId = '" + respuesta.Cliente.ClienteId + "' AND rcc.TipoContactoClienteId = 6"
                Query.Add(rs.Source)
                rs.Abrir()

                If Not rs.EOF Then
                    Dim ListaRegistros As New List(Of EstablecimientoCliente)
                    Do While Not rs.EOF
                        ListaRegistros.Add(New EstablecimientoCliente(rs("EstablecimientoId").Valor,
                                                                      rs("Descripcion").Valor + IIf(rs("DescDireccion").Valor <> "", " (" + rs("DescDireccion").Valor + ")", ""),
                                                                      rs("PorcFactura").Valor, CInt(rs("DireccionId").Valor)))
                        rs.MoveNext()
                    Loop
                    rs.Cerrar()

                    respuesta.Cliente.ListEstablecimientos = ListaRegistros
                Else
                    ' Si no encuentro establecimientos es porque no es una franquicia
                    respuesta.Cliente.ListEstablecimientos.Add(New EstablecimientoCliente(0, respuesta.Cliente.RazonSocial, 100, respuesta.Cliente.DireccionId))
                End If
            Catch ex As Exception
                respuesta.ConsultaExitosa = False
                respuesta.mensaje = "Error BD consultando los establecimientos asociados al Cliente (" + respuesta.Cliente.ClienteId + ") " + respuesta.Cliente.RazonSocial
            End Try

            Try
                Dim listTipoContactoIdMail As String = ""

                ' Traigo los tipos de contactos a los cuales quiero enviar mails
                rs.Source = "SELECT Valor FROM Configuracion WHERE Parametro = 'TipoContactoIdMail'"
                Query.Add(rs.Source)
                rs.Abrir()
                If Not rs.EOF Then listTipoContactoIdMail = rs("Valor").Valor
                rs.Cerrar()

                ' Traigo todos los emails de los contactos para envio
                rs.Source = "SELECT Email FROM RelClientesContactos                                         
                    WHERE ClienteId = '" + respuesta.Cliente.ClienteId + "' AND Email IS NOT NULL AND Email <> ''"
                If listTipoContactoIdMail <> "" Then rs.Source += " AND TipoContactoClienteId IN (" & listTipoContactoIdMail & ")"
                Query.Add(rs.Source)
                rs.Abrir()

                Dim ListaRegistros As New List(Of String)
                If Not rs.EOF Then
                    Do While Not rs.EOF
                        If Not ListaRegistros.Contains(rs("Email").Valor) Then ListaRegistros.Add(rs("Email").Valor)
                        rs.MoveNext()
                    Loop
                End If
                rs.Cerrar()

                ' Traigo todos los emails del cliente 2 para informar los faltantes
                rs.Source = "SELECT Email FROM RelClientesContactos                                        
                    WHERE ClienteId = '2' AND Email IS NOT NULL AND Email <> ''
                    AND TipoContactoClienteId = 8"
                Query.Add(rs.Source)
                rs.Abrir()

                If Not rs.EOF Then
                    Do While Not rs.EOF
                        If Not ListaRegistros.Contains(rs("Email").Valor) Then ListaRegistros.Add(rs("Email").Valor)
                        rs.MoveNext()
                    Loop
                End If
                rs.Cerrar()

                respuesta.Cliente.ListEmails = ListaRegistros
            Catch ex As Exception
                respuesta.ConsultaExitosa = False
                respuesta.mensaje = "Error BD consultando los emails asociados al Cliente (" + respuesta.Cliente.ClienteId + ") " + respuesta.Cliente.RazonSocial
            End Try
        End If

        Return respuesta
    End Function

    Public Function GetDatosProductos(ByVal ListaIdProductos As String, ByVal ListaPrecioId As Integer, ByVal DepositoId As Integer) As ResponseDetalle
        Dim respuesta As New ResponseDetalle

        Try
            ' Busco a un cliente con la Razon Social y el Deposito que me pasan porque un mismo cliente puede tener mas de un local
            rs.Source = "Select p.ProductoId, p.Descripcion, p.IVA, p.TipoProductoId, p.TipoUnidadMedidaId2, p.UnidadMedidaId2, p.CantUnidadMedida2, p.CantUnidadMedida1, 
                ISNULL(rpl.Precio, 0) Precio, ISNULL(puni.ProductoId, '') ProductoIdUni, ISNULL(puni.Descripcion, '') DescripcionUni,
                ISNULL(puni.TipoProductoId, '') TipoProductoIdUni, ISNULL(rpunil.Precio, '') PrecioUni, ISNULL(s.StockActual, 0) Stock, 
                ISNULL(suni.StockActual, 0) StockUni, ISNULL(rprueba.ProductoId, '') EsPrueba
                FROM Productos p
                JOIN UnidadesMedida um ON um.TipoUnidadMedidaId = p.TipoUnidadMedidaId2 AND um.UnidadMedidaId = p.UnidadMedidaId2
                LEFT JOIN Productos puni ON puni.ProductoId = '9' + p.ProductoId
                LEFT JOIN (SELECT ProductoId, StockActual FROM Stock WHERE DepositoId = " + DepositoId.ToString() + ") s ON s.ProductoId = p.ProductoId
                LEFT JOIN (SELECT ProductoId, StockActual FROM Stock WHERE DepositoId = " + DepositoId.ToString() + ") suni ON suni.ProductoId = puni.ProductoId
                LEFT JOIN (SELECT * FROM RelProductosListasPrecios WHERE ListaPrecioId = " + ListaPrecioId.ToString() + ") rpl ON rpl.ProductoId = p.ProductoId                
                LEFT JOIN (SELECT * FROM RelProductosListasPrecios WHERE ListaPrecioId = " + ListaPrecioId.ToString() + ") rpunil ON rpunil.ProductoId = puni.ProductoId
                LEFT JOIN (SELECT DISTINCT rp.ProductoId FROM RelProductosClasificacionesProductos rp
	                JOIN ClasificacionesProductos cp ON cp.ClasificacionProdId = rp.ClasificacionProdId
	                JOIN ENSEMBLE.dbo.GWRRelReglasSincronizacionEntidades rse ON rse.EntidadId = cp.ClasificacionProdId
	                WHERE rse.ReglaSincroId = 6) rprueba ON rprueba.ProductoId = p.ProductoId
                WHERE p.ProductoId IN (" + ListaIdProductos + ")"
            Query.Add(rs.Source)
            rs.Abrir()

            If Not rs.EOF Then
                Dim detalle As DetalleDocumento
                Dim ListaRegistros As New List(Of DetalleDocumento)

                Do While Not rs.EOF
                    detalle = New DetalleDocumento With {
                        .ProductoId = rs("ProductoId").Valor,
                        .Descripcion = rs("Descripcion").Valor,
                        .AlicuotaIVA = rs("IVA").Valor,
                        .TipoProductoId = rs("TipoProductoId").Valor,
                        .SeVendePorUnidad = (rs("TipoUnidadMedidaId2").Valor = 1 Or rs("TipoUnidadMedidaId2").Valor = 3),
                        .SePidePorUnidad = (rs("UnidadMedidaId2").Valor = 0 Or rs("TipoUnidadMedidaId2").Valor = 3 Or rs("TipoUnidadMedidaId2").Valor = 4),
                        .SeVendePorKgFijos = rs("TipoUnidadMedidaId2").Valor = 4,
                        .KgPromedioxUnid = CDbl(rs("CantUnidadMedida1").Valor),
                        .UnidadesPorBulto = CInt(rs("CantUnidadMedida2").Valor),
                        .Precio = rs("Precio").Valor,
                        .ProductoIdUni = rs("ProductoIdUni").Valor,
                        .DescripcionUni = rs("DescripcionUni").Valor,
                        .TipoProductoIdUni = rs("TipoProductoIdUni").Valor,
                        .PrecioUni = rs("PrecioUni").Valor,
                        .Stock = rs("Stock").Valor,
                        .StockUnidades = rs("StockUni").Valor,
                        .EsPrueba = (rs("EsPrueba").Valor <> "")
                    }

                    ListaRegistros.Add(detalle)
                    rs.MoveNext()
                Loop

                respuesta.ConsultaExitosa = True
                respuesta.rs = ListaRegistros
            End If

            rs.Cerrar()
        Catch ex As Exception
            respuesta.mensaje = "Error BD consultando los productos con IDs " & ListaIdProductos & " y Lista de Precios " & ListaPrecioId
        End Try

        Return respuesta
    End Function


    Public Function GetDireccionesEstablecimiento(ByVal EstablecimientoId As String) As ResponseDireccionesCliente
        Dim respuesta As New ResponseDireccionesCliente
        Dim sqlQuery As String

        Try

            sqlQuery = "
            Select RCD.ClienteId, RCD.DireccionId, RCD.Calle, RCD.Numero, RCD.Piso, RCD.Depto, RCD.Localidad, p.Descripcion as Provincia, RCD.Entrega
            From RelClientesContactos RCC
            INNER JOIN RelClientesDirecciones RCD on RCC.ClienteId = RCD.ClienteId and RCC.DireccionId = RCD.DireccionId
            Inner join Gestion.dbo.Provincias P on p.PaisId = RCD.PaisId and p.ProvinciaId = RCD.ProvinciaId
            Where RCC.TipoContactoClienteId = 6 and RCC.Celular = '" + EstablecimientoId + "'"

            rs.Source = sqlQuery
            Query.Add(rs.Source)
            rs.Abrir()
            If Not rs.EOF Then
                Dim ListaRegistros As New List(Of DireccionCliente)
                Do While Not rs.EOF
                    Dim calle As String = If(IsDBNull(rs("Calle").Valor), "", rs("Calle").Valor)
                    Dim numero As String = If(IsDBNull(rs("Numero").Valor), "", rs("Numero").Valor)
                    Dim piso As String = If(IsDBNull(rs("Piso").Valor), "", rs("Piso").Valor)
                    Dim depto As String = If(IsDBNull(rs("Depto").Valor), "", rs("Depto").Valor)
                    Dim Loc As String = If(IsDBNull(rs("Localidad").Valor), "", rs("Localidad").Valor)
                    Dim prov As String = If(IsDBNull(rs("Provincia").Valor), "", rs("Provincia").Valor)
                    Dim Entrega As Boolean = If(IsDBNull(rs("Entrega").Valor), "", Boolean.Parse(rs("Entrega").Valor))

                    ListaRegistros.Add(New DireccionCliente(rs("ClienteId").Valor, rs("DireccionId").Valor, calle, numero, piso, depto, Loc, prov, Entrega))
                    rs.MoveNext()
                Loop

                respuesta.ConsultaExitosa = True
                respuesta.rs = ListaRegistros
            Else
                respuesta.mensaje = "El cliente no tiene direcciones asociadas"
            End If
            rs.Cerrar()
        Catch ex As Exception
            respuesta.mensaje = "Error BD consultando las sucursales del usuario"
        End Try

        Return respuesta
    End Function

    Public Function GetListProductos() As responseListProductosSimple
        Dim respuesta As New responseListProductosSimple
        Dim sqlQuery As String

        Try

            sqlQuery = "SELECT ProductoId, Descripcion FROM Productos where Inactivo = 0 "

            rs.Source = sqlQuery
            Query.Add(rs.Source)
            rs.Abrir()
            If Not rs.EOF Then
                Dim ListaRegistros As List(Of ListProductoSimple) = New List(Of ListProductoSimple)
                Do While Not rs.EOF
                    Dim prod As New ListProductoSimple
                    prod.productoId = rs("ProductoId").Valor
                    prod.productoNombre = rs("Descripcion").Valor
                    ListaRegistros.Add(prod)
                    rs.MoveNext()
                Loop
                respuesta.items = ListaRegistros
                respuesta.ConsultaExitosa = True
            Else
                respuesta.mensaje = "No existen los productos en la base de tempo"
            End If
            rs.Cerrar()
        Catch ex As Exception
            respuesta.mensaje = "Error BD al consultar los productos de la base de tempo"
        End Try
        Return respuesta
    End Function


    Public Class CustomVbCollection

        Implements Collection

        Private _items As Dictionary(Of Object, Object) = New Dictionary(Of Object, Object)
        Public Sub Add(ByRef item As Object, Optional ByRef key As Object = Nothing, Optional ByRef before As Object = Nothing, Optional ByRef after As Object = Nothing) _
        Implements Collection.Add

            _items.Add(key, item)

        End Sub
        Public Function Count() As Integer Implements Collection.Count

            Return _items.Count

        End Function
        Public Function GetEnumerator() As IEnumerator Implements Collection.GetEnumerator

            Return _items.Values.GetEnumerator()

        End Function
        Public Function GetEnumerato() As IEnumerator Implements IEnumerable.GetEnumerator

            Return _items.Values.GetEnumerator()

        End Function
        Public Function Item(ByRef index As Object) Implements Collection.Item

            Return _items(index)

        End Function
        Public Sub Remove(ByRef index As Object) Implements Collection.Remove

            _items.Remove(index)

        End Sub

    End Class

End Class

