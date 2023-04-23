Public Class LuzAzulRecepcion
    Inherits LuzAzulCommon
    Public Class RelFamiliasToleranciasProductosComplete
        Inherits RelFamiliasToleranciasProductos
        Public Property Descripcion As String
        Public Property ProductoNombre As String

    End Class

    Public Class RelFamiliasToleranciasProductos
        Public Property FamiliaToleranciaId As String
        Public Property ProductoId As String

    End Class
    Public Class FamiliaToleranciaPickeo
        Public Property FamiliaToleranciaId As String
        Public Property Descripcion As String
        Public Property ToleranciaAceptable As Double
        Public Property ToleranciaCritica As Double
        Public Property ToleranciaSevera As Double
    End Class


    Public Class RelEmpaqueProducto
        Public Property EmpaqueId As String
        Public Property Descripcion As String
        Public Property Peso As String
        Public Property Orden As String
        Public Property ProductoId As String
        Public Property ProductoNombre As String

    End Class

        Public Class Empaque
            Public Property EmpaqueId As String
            Public Property Descripcion As String
            Public Property Peso As String
        End Class

        Public Class RelProductosSenasa
            Public Property CodProducto As String
            Public Property nroOficial As String
            Public Property Establecimiento As String
            Public Property CodProductoSenasa As String
            Public Property CodUnicoSenasa As String

        End Class
        Public Class EstablecimientoSenasa
            Public Property nroOficial As String
            Public Property Descripcion As String
            Public Property ListCertificados As New List(Of certificadosDeOrigen)

        End Class

    Public Class LocalidadSenasa
        Public Property Codigo As String
        Public Property Localidad As String
        Public Property Partido As String
        Public Property Provincia As String
    End Class

    Public Class certificadosDeOrigen
            Public Property CUVE As String
            Public Property certificadoDeOrigen As String
            Public Property consumoInterno As String
            Public Property fechaSalida As String
            Public Property prefijoPermisoTransito As String
            Public Property DetalleProductos As New List(Of detalleCertificadoDeOrigen)
            Public Property Tropas As New List(Of Tropa)

        End Class
        Public Class Tropa
            Public Property fechaDeElaboracion As String
            Public Property fechaDeVencimiento As String
            Public Property lote As String

        End Class

        Public Class detalleCertificadoDeOrigen
            Public Property cantidad As String
            Public Property codEnvasePrimarioSENASA As String
            Public Property codEnvaseSecundarioSENASA As String
            Public Property envasePrimario As String
            Public Property envaseSecundario As String
            Public Property marca As String
            Public Property pesoBruto As String
            Public Property pesoNeto As String
            Public Property codigoProductoCAPA As String
            Public Property productoobservacion As String

        End Class

        Public Class responseCertificadoSenasa
            Inherits ResponseWS
            Public Errores As List(Of String) = New List(Of String)
            Public nroTransaccion As String
            Public idImportacion As String

        End Class
        Public Class responseFechaUltimoPedido
            Inherits ResponseWS
            Public items As List(Of ItemUltimoPedido) = New List(Of ItemUltimoPedido)
        End Class
        Public Class responseAprobar
            Inherits ResponseWS
            Public items As List(Of ItemListarAprobar) = New List(Of ItemListarAprobar)
        End Class
        Public Class ItemListarAprobar
            Public Property Seleccionar As Boolean
            Public Property PedidoId As String
            Public Property EstablecimientoId As String
            Public Property EstablecimientoNombre As String
            Public Property RazonSocial As String
            Public Property CUIT As String
            Public Property Proveedor As String
            Public Property Fecha As String
            Public Property CantidadArticulos As String
            Public Property EstadoId As Integer
            Public Property EstadoNombre As String
            Public Property DiaPedido As String
            Public Property DiaFrecuencia As String
            Public Property PosibleRechazado As Boolean
            Public Property ClienteId As String
            Public Property MailsEnviados As String

        End Class

        Public Class ItemUltimoPedido
            Public Property EstablecimientoId As String
            Public Property Proveedor As String
            Public Property Fecha As Date
        End Class

        Public Class ItemPedidoFaltante
            Public Property IdEstablecimiento As String
            Public Property NombreEstablecimiento As String
            Public Property Proveedor As String
            Public Property FechaUltimoPedido As String
            Public Property FechaProximoPedido As String
            Public Property Frecuencia As Integer
            Public Property Faltante As Boolean

        End Class
        Public Class responseListar
            Inherits ResponseWS
            Public items As List(Of ItemListarPedido) = New List(Of ItemListarPedido)
        End Class

        Public Class ItemListarPedido
            Public Property Seleccionar As Boolean
            Public Property pedidoProveedorId As String
            Public Property pedidoId As String
            Public Property establecimientoId As String
            Public Property establecimientoNombre As String
            Public Property razonSocial As String
            Public Property CUIT As String
            Public Property proveedor As String

            Public Property fecha As String
            Public Property CantidadArticulos As String
        End Class
        Public Class responseListarOrdenes
            Inherits ResponseWS
            Public items As List(Of ItemListarOrdenesPickeo) = New List(Of ItemListarOrdenesPickeo)
        End Class

        Public Class ItemListarOrdenesPickeo
            Public Property Seleccionar As Boolean
            Public Property establecimientoId As String
            Public Property establecimientoNombre As String
            Public Property razonSocial As String
            Public Property CUIT As String
            Public Property ordenPickeoId As String
            Public Property fecha As String
            Public Property estadoId As String
            Public Property estado As String
            Public Property ClienteId As String
        End Class

        Public Class responseDetalleAgrupar
            Inherits ResponseWS
            Public items As List(Of ItemDetallePedido) = New List(Of ItemDetallePedido)
        End Class

        Public Class ItemDetallePedido
            Public Property pedidoDetalleId As String
            Public Property productoId As String
            Public Property productoNombre As String
            Public Property productoOrden As String
            Public Property proveedorNombre As String
            Public Property proveedorConPrefijo As String
            Public Property stock As String
            Public Property cantPedida As String
            Public Property cantSugerida As String
            Public Property unidad As String
            Public Property unidadesPorBulto As String
            Public Property KgPromedioUnidad As String
            Public Property SeVendePorPeso As Boolean
            Public Property SePidePorBulto As Boolean
            Public Property Observaciones As String
            Public Property TipoUnidadMedida2 As Integer

        End Class
        Public Class responseDetallePickeo
            Inherits ResponseWS
            Public items As List(Of ItemDetallePickeo) = New List(Of ItemDetallePickeo)
        End Class
        Public Class ItemDetallePickeo
            Public Property pedidoDetalleId As String
            Public Property proveedorNombre As String
            Public Property proveedorConPrefijo As String
            Public Property productoId As String
            Public Property productoNombre As String
            Public Property productoOrden As String
            Public Property stock As String
            Public Property cantPedida As String
            Public Property unidad As String
            Public Property cantReal As String
            Public Property Cajas As Integer
            Public Property taraCaja As Decimal
            Public Property ListTaraCaja As List(Of Tara) = New List(Of Tara)
            Public Property taraPallet As Decimal
            Public Property kgReal As Decimal
            Public Property kgPedidos As Decimal
            Public Property unidadesPorBulto As String
            Public Property KgPromedioUnidad As String
            Public Property SeVendePorPeso As Boolean
            Public Property SePidePorBulto As Boolean
            Public Property Lote As String
            Public Property Observaciones As String
            Public Property ObservacionesProducto As String
            Public Property TipoUnidadMedida2 As Integer
            Public Property carga As String

        End Class

        Public Class ItemDetallePendientes
            Public Property pedidoDetalleId As String
            Public Property productoId As String
            Public Property proveedorNombre As String
            Public Property productoNombre As String
            Public Property productoOrden As String
            Public Property nroPallet As String
            Public Property cantPedida As String
            Public Property unidad As String
            Public Property cantReal As String
            Public Property kgNetos As String

        End Class
        Public Class responseOrdenesAgrupadas
            Inherits ResponseWS
            Public items As List(Of ItemDetalleAgrupado) = New List(Of ItemDetalleAgrupado)
        End Class
        Public Class ItemDetalleAgrupado
            Public Property proveedorNombre As String
            Public Property productoId As String
            Public Property codigoSenasa As String
            Public Property productoNombre As String
            Public Property productoOrden As String
            Public Property cantReal As String
            Public Property unidad As String
            Public Property unidades As String
            Public Property kgNetos As Decimal
            Public Property unidadesPorBulto As String

        End Class

    Public Class Pallet
        Public Property Numero As String
        Public Property Info As List(Of PalletInfo) = New List(Of PalletInfo)
        Public Property Items As List(Of ItemDetallePickeo) = New List(Of ItemDetallePickeo)
    End Class

    Public Class PalletInfo
        Public Property Carga As String
        Public Property Hora_inicio As Date
        Public Property Hora_fin As Date
        Public Property Supervisor As String
        Public Property TaraPallet As Decimal
        Public Property Peso As Decimal
        Public Property Observaciones As String
        Public Property Identificador As String
    End Class

    Public Class OrdenPickeo
        Public Property Numero As String
        Public Property Estado As String
        Public Property EstadoNombre As String
        Public Property Preparo As String
        Public Property Separo As String
        Public Property Observaciones As String
        Public Property Hora_inicio As Date
        Public Property Hora_fin As Date
        Public Property Pallets As List(Of Pallet) = New List(Of Pallet)
    End Class
        Public Class Tara
            Public Property Descripcion As String
            Public Property Peso As Decimal
            Public Sub New(ByVal nombreTara As String, ByVal tara As Decimal)
                Descripcion = nombreTara
                Peso = tara
            End Sub
        End Class
        Public Class ProductoTara
            Inherits Tara
            Public Property productoId As String
            Public Sub New(ByVal ProdId As String, ByVal nombreTara As String, ByVal tara As Decimal)
                MyBase.New(nombreTara, tara)
                productoId = ProdId
            End Sub
        End Class
        Public Class responseProductoTara
            Inherits ResponseWS
            Public items As List(Of ProductoTara) = New List(Of ProductoTara)
        End Class

        Public Class PalletTaraDetalle
            Public Property pickeoTaraId As String
            Public Property ordenPickeoId As String
            Public Property nroPallet As String
            Public Property descripcion As String
            Public Property peso As Double
            Public Property esPesoPallet As Boolean

        End Class
    Public Class ProductoDescripcion
        Public Property productoId As String
        Public Property productoNombre As String
        Public Property productoOrden As String
        Public Property KgPromedioUnidad As Double
        Public Property SeVendePorPeso As Boolean
        Public Property TipoUnidadMedida2 As Integer
        Public Sub New(ByVal ProdId As String, ByVal Descripcion As String, ByVal Orden As String, ByVal BSeVendePorPeso As Boolean, ByVal kgsPromedio As Double, unidadMedida2 As Integer)
            productoId = ProdId
            productoNombre = Descripcion
            SeVendePorPeso = BSeVendePorPeso
            KgPromedioUnidad = kgsPromedio
            productoOrden = Orden
            TipoUnidadMedida2 = unidadMedida2
        End Sub
    End Class
    Public Class responseProductoDescripcion
            Inherits ResponseWS
            Public items As List(Of ProductoDescripcion) = New List(Of ProductoDescripcion)
        End Class
        Public Class ProductoStock
            Public Property productoId As String
            Public Property stock As String

            Public Sub New(ByVal ProdId As String, ByVal Dstock As String)
                productoId = ProdId
                stock = Dstock
            End Sub
        End Class
    Public Class responseRelEmpaqueProductos
        Inherits ResponseWS
        Public Items As List(Of RelEmpaqueProducto) = New List(Of RelEmpaqueProducto)
    End Class
    Public Class responseRelFamiliasToleranciasProductosComplete
        Inherits ResponseWS
        Public Items As List(Of RelFamiliasToleranciasProductosComplete) = New List(Of RelFamiliasToleranciasProductosComplete)
    End Class

    Public Class responseRelFamiliasToleranciasProductos
        Inherits ResponseWS
        Public Items As List(Of RelFamiliasToleranciasProductos) = New List(Of RelFamiliasToleranciasProductos)
    End Class
    Public Class responseFamiliaToleranciaPickeo
        Inherits ResponseWS
        Public Items As List(Of FamiliaToleranciaPickeo) = New List(Of FamiliaToleranciaPickeo)
    End Class
    Public Class responseEmpaques
        Inherits ResponseWS
        Public Items As List(Of Empaque) = New List(Of Empaque)
    End Class

    Public Class responseLocalidadesSenasa
        Inherits ResponseWS
        Public Items As List(Of LocalidadSenasa) = New List(Of LocalidadSenasa)
    End Class

    Public Class responseEstablecimientosSenasa
            Inherits ResponseWS
            Public Items As List(Of EstablecimientoSenasa) = New List(Of EstablecimientoSenasa)
        End Class
        Public Class responseRelProductosSenasa
            Inherits ResponseWS
            Public Items As List(Of RelProductosSenasa) = New List(Of RelProductosSenasa)
        End Class
        Public Class responseProductoStock
            Inherits ResponseWS
            Public items As List(Of ProductoStock) = New List(Of ProductoStock)
        End Class
        Public Class responseFrecuencia
            Inherits ResponseWS
            Public items As List(Of FrecuenciaEstablecimientoProveedor) = New List(Of FrecuenciaEstablecimientoProveedor)
        End Class
        Public Class responseExcluidos
            Inherits ResponseWS
            Public items As List(Of String) = New List(Of String)
        End Class
        Public Class responseValidarPickeo
            Inherits ResponseWS
            Public EsCritico As Boolean = False
            Public EsCriticoPeso As Boolean = False
            Public EsSevero As Boolean = False
            Public CompletoCampos As Boolean = False
            Public Mensajes As New List(Of String)
        End Class


        Public Class FrecuenciaEstablecimientoProveedor
            Public Property establecimientoId As String
            Public Property establecimientoNombre As String
            Public Property proveedor As String
            Public Property dia As String
            Public Property frecuencia As Integer
            Public Sub New(ByVal idEstablecimiento As String, ByVal nombreEstablecimiento As String, ByVal proveedorNombre As String, ByVal nombreDia As String, ByVal iFrecuencia As Integer)
                establecimientoId = idEstablecimiento
                establecimientoNombre = nombreEstablecimiento
                proveedor = proveedorNombre
                dia = nombreDia
                frecuencia = iFrecuencia
            End Sub

        End Class
        Public Class DiaSemana
            Public Property NroDia As Integer
            Public Property NombreDia As String
            Public Sub New(ByVal Nro As Integer, Nombre As String)
                NroDia = Nro
                NombreDia = Nombre
            End Sub
        End Class

        Public Class EstadoPedido
            Public Property EstadoId As Integer
            Public Property EstadoNombre As String
            Public Property Items As List(Of ItemListarAprobar) = New List(Of ItemListarAprobar)

            Public Sub New(ByVal id As Integer, ByVal Nombre As String)
                EstadoId = id
                EstadoNombre = Nombre
            End Sub
        End Class

    Public Function GetDescripcionesProductos(ByVal listProductos As String) As responseProductoDescripcion
            Dim respuesta As New responseProductoDescripcion
            Dim sqlQuery As String

            Try

                sqlQuery = "SELECT p.ProductoId, p.Descripcion, p.TipoUnidadMedidaId2, p.UnidadMedidaId2, p.CantUnidadMedida1, um.Descripcion UniPedido, p.CodBarra
                    FROM Productos p
                    JOIN UnidadesMedida um ON um.TipoUnidadMedidaId = p.TipoUnidadMedidaId2 AND um.UnidadMedidaId = p.UnidadMedidaId2
                    where Inactivo = 0 and ProductoId in (" + listProductos + ")"

                rs.Source = sqlQuery
                Query.Add(rs.Source)
                rs.Abrir()
                If Not rs.EOF Then
                    Dim ListaRegistros As List(Of ProductoDescripcion) = New List(Of ProductoDescripcion)
                    Do While Not rs.EOF
                        'Este campo dejo de usarse desde tempo y ahora se toma desde el webservice
                        'Dim SePidePorBulto As Boolean = CInt(rs("UnidadMedidaId2").Valor) <> 0 And Not rs("UniPedido").Valor.ToString().Contains(" x ") And CInt(rs("TipoUnidadMedidaId2").Valor) < 3

                        Dim SeVendePorPeso As Boolean = CInt(rs("TipoUnidadMedidaId2").Valor = 2) Or CInt(rs("TipoUnidadMedidaId2").Valor) = 4
                        Dim KgPromedioUnid As Double = IIf(CDbl(rs("CantUnidadMedida1").Valor) = 0, 1, CDbl(rs("CantUnidadMedida1").Valor))
                        Dim TipoUnidadMedida2 As Integer = Integer.Parse(rs("TipoUnidadMedidaId2").Valor)

                        ListaRegistros.Add(New ProductoDescripcion(rs("ProductoId").Valor, rs("Descripcion").Valor, rs("CodBarra").Valor, SeVendePorPeso, KgPromedioUnid, TipoUnidadMedida2))
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
        Public Function GetTaraProductos(ByVal listProductos As String) As responseProductoTara
            Dim respuesta As New responseProductoTara
            Dim sqlQuery As String

            Try

                sqlQuery = "
            SELECT REP.ProductoId, REP.EmpaqueId, E.Descripcion, E.Peso, REP.Orden 
            FROM " + NombreBaseEnsemble + ".dbo.Empaques E
            INNER JOIN " + NombreBaseEnsemble + ".dbo.RelEmpaquesProductos REP on E.EmpaqueId = REP.EmpaqueId
            WHERE REP.ProductoId in (" + listProductos + ") ORDER BY Orden ASC"

                rs.Source = sqlQuery
                Query.Add(rs.Source)
                rs.Abrir()
                If Not rs.EOF Then
                    Dim ListaRegistros As List(Of ProductoTara) = New List(Of ProductoTara)
                    Do While Not rs.EOF
                        ListaRegistros.Add(New ProductoTara(rs("ProductoId").Valor, rs("Descripcion").Valor, rs("Peso").Valor))
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

        Public Function GetStockProductos(ByVal listProductos As String, ByVal ListDepositos As String) As responseProductoStock
            Dim respuesta As New responseProductoStock
            Dim sqlQuery As String

            Try

                sqlQuery = "SELECT p.ProductoId, p.Descripcion, SUM( s.StockActual) as stock
                    FROM Productos p
					Left JOIN Stock s ON s.productoId = p.productoId "

                sqlQuery += " where p.Inactivo = 0 "

                If (listProductos <> "") Then
                    sqlQuery += " and p.ProductoId in (" + listProductos + ") "
                End If

                If (ListDepositos <> "") Then
                    sqlQuery += " and depositoId in (" + ListDepositos + ") "
                End If

                sqlQuery += " Group by  p.ProductoId, p.Descripcion"

                rs.Source = sqlQuery
                Query.Add(rs.Source)
                rs.Abrir()
                If Not rs.EOF Then
                    Dim ListaRegistros As List(Of ProductoStock) = New List(Of ProductoStock)
                    Do While Not rs.EOF
                    Dim stock As Double = IIf(Double.TryParse(rs("stock").Valor.ToString(), stock), stock, 0)
                    ListaRegistros.Add(New ProductoStock(rs("ProductoId").Valor, Format(stock, "#0.00000")))
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
        Public Function GetListEstablecimientos(ByVal EsFabrica As Boolean) As ResponseEstablecimiento
            Dim respuesta As New ResponseEstablecimiento
            Dim sqlQuery As String

            Try
                Dim NombreTablaEstablecimiento As String = "Establecimientos"
                Dim EsPropio As Integer = 0

                If EsFabrica Then
                    NombreTablaEstablecimiento = "GWREstablecimientos"
                    EsPropio = 1
                End If

                sqlQuery = "SELECT EstablecimientoId, Descripcion
                    FROM " + NombreBaseEnsemble + ".dbo." + NombreTablaEstablecimiento

                rs.Source = sqlQuery
                Query.Add(rs.Source)
                rs.Abrir()
                If Not rs.EOF Then
                    Dim ListaRegistros As List(Of Establecimiento) = New List(Of Establecimiento)
                    Do While Not rs.EOF
                        ListaRegistros.Add(New Establecimiento(rs("EstablecimientoId").Valor, rs("Descripcion").Valor, EsPropio))
                        rs.MoveNext()
                    Loop

                    respuesta.rs = ListaRegistros
                    respuesta.ConsultaExitosa = True
                Else
                    respuesta.mensaje = "El usuario no tiene establecimientos asociados"
                End If
                rs.Cerrar()
            Catch ex As Exception
                respuesta.mensaje = "Error BD al consultar los establecimientos del usuario"
            End Try

            Return respuesta
        End Function

        Public Function GetFrecuenciaProveedores(ByVal EsFabrica As Boolean) As responseFrecuencia
            Dim respuesta As New responseFrecuencia
            Dim sqlQuery As String

            Try
                Dim NombreTablaEstablecimiento As String = "Establecimientos"
                Dim EsPropio As Integer = 0

                If EsFabrica Then
                    NombreTablaEstablecimiento = "GWREstablecimientos"
                    EsPropio = 1
                End If

                sqlQuery = "SELECT  e.EstablecimientoId , e.Descripcion Sucursal,
          rr.Nombre, LEFT(rr.Telefono, CHARINDEX(';', rr.Telefono) - 1) Dia, REVERSE(LEFT(REVERSE(rr.Telefono), CHARINDEX(';', REVERSE(rr.Telefono)) - 1)) Frecuencia, 
          rr.Celular, rr.Email, re.ClienteId, rr.ContactoClienteId 
        FROM " + NombreBaseEnsemble + ".dbo." + NombreTablaEstablecimiento + " e
        JOIN RelClientesContactos re ON re.Celular = e.EstablecimientoId
        JOIN RelClientesContactos rr ON rr.ClienteId = re.ClienteId
        WHERE re.TipoContactoClienteId=6 AND rr.TipoContactoClienteId=7 AND e.Alta = 1 "

                rs.Source = sqlQuery
                Query.Add(rs.Source)
                rs.Abrir()
                If Not rs.EOF Then
                    Dim ListaRegistros As List(Of FrecuenciaEstablecimientoProveedor) = New List(Of FrecuenciaEstablecimientoProveedor)
                    Do While Not rs.EOF
                        ListaRegistros.Add(New FrecuenciaEstablecimientoProveedor(rs("EstablecimientoId").Valor, rs("Sucursal").Valor, rs("Nombre").Valor, rs("Dia").Valor, rs("Frecuencia").Valor))
                        rs.MoveNext()
                    Loop
                    respuesta.items = ListaRegistros
                    respuesta.ConsultaExitosa = True
                Else
                    respuesta.mensaje = "El usuario no tiene establecimientos asociados"
                End If
                rs.Cerrar()
            Catch ex As Exception
                respuesta.mensaje = "Error BD al consultar los establecimientos del usuario"
            End Try

            Return respuesta
        End Function
        Public Function GetProductosExcluidosErroresCriticos() As responseExcluidos
            Dim respuesta As New responseExcluidos
            Dim sqlQuery As String

            Try
                Dim NombreTablaEstablecimiento As String = "Establecimientos"
                Dim EsPropio As Integer = 0

                sqlQuery = "SELECT  EntidadId  
        FROM " + NombreBaseEnsemble + ".dbo.GWRRelReglasSincronizacionEntidades 
        WHERE ReglaSincroId = 7 "

                rs.Source = sqlQuery
                Query.Add(rs.Source)
                rs.Abrir()
                If Not rs.EOF Then
                    Dim ListaRegistros As List(Of String) = New List(Of String)
                    Do While Not rs.EOF
                        ListaRegistros.Add(rs("EntidadId").Valor)
                        rs.MoveNext()
                    Loop
                    respuesta.items = ListaRegistros
                    respuesta.ConsultaExitosa = True
                Else
                    respuesta.ConsultaExitosa = True
                    respuesta.mensaje = "No existen productos excluidos"
                End If
                rs.Cerrar()
            Catch ex As Exception
                respuesta.mensaje = "Error BD al consultar los establecimientos del usuario"
            End Try

            Return respuesta
        End Function

    Public Function GetToleraciaPallet() As Double
        Dim tolerancia As Double
        Dim respuesta As ResponseConfiguracion
        respuesta = GetParametroConfiguracion("ToleranciaPesoPallet")
        If respuesta.ConsultaExitosa Then
            tolerancia = IIf(Double.TryParse(respuesta.Valor, tolerancia), tolerancia, 0)
        End If

        Return tolerancia
    End Function

    Public Function GetToleraciaPalletDefaultPeso() As Double
        Return GetToleracia("FamiliaIdToleranciaPeso")
    End Function
    Public Function GetToleraciaPalletDefaultUnidad() As Double
        Return GetToleracia("FamiliaIdToleranciaUnidad")
    End Function

    Public Function GetToleracia(strParametro As String) As Double
        Dim tolerancia As Integer
        Dim respuesta As ResponseConfiguracion

        respuesta = GetParametroConfiguracion(strParametro)
        If respuesta.ConsultaExitosa Then
            tolerancia = IIf(Integer.TryParse(respuesta.Valor, tolerancia), tolerancia, 0)
        End If

        Return tolerancia
    End Function

    Public Function GetEstablecimientosSenasa() As responseEstablecimientosSenasa
            Dim respuesta As New responseEstablecimientosSenasa
            Dim sqlQuery As String

            Try
                sqlQuery = "SELECT  nroOficial, Descripcion FROM " + NombreBaseEnsemble + ".dbo.SENASAEstablecimientos "

                rs.Source = sqlQuery
                Query.Add(rs.Source)
                rs.Abrir()
                If Not rs.EOF Then
                    Dim ListaRegistros As List(Of EstablecimientoSenasa) = New List(Of EstablecimientoSenasa)
                    Do While Not rs.EOF
                        Dim registro As New EstablecimientoSenasa
                        registro.Descripcion = rs("Descripcion").Valor
                        registro.nroOficial = rs("nroOficial").Valor
                        ListaRegistros.Add(registro)
                        rs.MoveNext()
                    Loop
                    respuesta.Items = ListaRegistros
                    respuesta.ConsultaExitosa = True
                Else
                    respuesta.ConsultaExitosa = True
                    respuesta.mensaje = "No existen establecimientos asociados a SENASA"
                End If
                rs.Cerrar()
            Catch ex As Exception
                respuesta.mensaje = "Error BD al consultar la tabla de establecimientos de senasa"
            End Try

            Return respuesta
        End Function
    Public Function GetProvinciasSenasa() As responseLocalidadesSenasa
        Dim respuesta As New responseLocalidadesSenasa
        Dim sqlQuery As String

        Try
            sqlQuery = "  SELECT DISTINCT(Provincia) as Provincia FROM " + NombreBaseEnsemble + ".dbo.SENASALocalidades "
            rs.Source = sqlQuery
            Query.Add(rs.Source)
            rs.Abrir()
            If Not rs.EOF Then
                Dim ListaRegistros As List(Of LocalidadSenasa) = New List(Of LocalidadSenasa)
                Do While Not rs.EOF
                    Dim registro As New LocalidadSenasa
                    registro.Provincia = rs("Provincia").Valor
                    ListaRegistros.Add(registro)
                    rs.MoveNext()
                Loop
                respuesta.Items = ListaRegistros
                respuesta.ConsultaExitosa = True
            Else
                respuesta.ConsultaExitosa = True
                respuesta.mensaje = "No existen provincias en la tabla localidades de SENASA"
            End If
            rs.Cerrar()
        Catch ex As Exception
            respuesta.mensaje = "Error BD al consultar la tabla de localidades de senasa"
        End Try

        Return respuesta
    End Function
    Public Function GetPartidosSenasa(ByVal provincia As String) As responseLocalidadesSenasa
        Dim respuesta As New responseLocalidadesSenasa
        Dim sqlQuery As String

        Try
            sqlQuery = "  SELECT DISTINCT(Partido) as Partido,  Provincia FROM " + NombreBaseEnsemble + ".dbo.SENASALocalidades where Provincia = '" + provincia + "' "
            rs.Source = sqlQuery
            Query.Add(rs.Source)
            rs.Abrir()
            If Not rs.EOF Then
                Dim ListaRegistros As List(Of LocalidadSenasa) = New List(Of LocalidadSenasa)
                Do While Not rs.EOF
                    Dim registro As New LocalidadSenasa
                    registro.Partido = rs("Partido").Valor
                    registro.Provincia = rs("Provincia").Valor
                    ListaRegistros.Add(registro)
                    rs.MoveNext()
                Loop
                respuesta.Items = ListaRegistros
                respuesta.ConsultaExitosa = True
            Else
                respuesta.ConsultaExitosa = True
                respuesta.mensaje = "No existen provincias en la tabla localidades de SENASA"
            End If
            rs.Cerrar()
        Catch ex As Exception
            respuesta.mensaje = "Error BD al consultar la tabla de localidades de senasa"
        End Try

        Return respuesta
    End Function
    Public Function GetLocalidadesSenasa(ByVal provincia As String, ByVal partido As String) As responseLocalidadesSenasa
        Dim respuesta As New responseLocalidadesSenasa
        Dim sqlQuery As String

        Try
            sqlQuery = "  SELECT Codigo, Localidad, Partido,  Provincia FROM " + NombreBaseEnsemble + ".dbo.SENASALocalidades where Provincia = '" + provincia + "' and Partido = '" + partido + "' "
            rs.Source = sqlQuery
            Query.Add(rs.Source)
            rs.Abrir()
            If Not rs.EOF Then
                Dim ListaRegistros As List(Of LocalidadSenasa) = New List(Of LocalidadSenasa)
                Do While Not rs.EOF
                    Dim registro As New LocalidadSenasa
                    registro.Codigo = rs("Codigo").Valor
                    registro.Localidad = rs("Localidad").Valor
                    registro.Partido = rs("Partido").Valor
                    registro.Provincia = rs("Provincia").Valor
                    ListaRegistros.Add(registro)
                    rs.MoveNext()
                Loop
                respuesta.Items = ListaRegistros
                respuesta.ConsultaExitosa = True
            Else
                respuesta.ConsultaExitosa = True
                respuesta.mensaje = "No existen provincias en la tabla localidades de SENASA"
            End If
            rs.Cerrar()
        Catch ex As Exception
            respuesta.mensaje = "Error BD al consultar la tabla de localidades de senasa"
        End Try

        Return respuesta
    End Function

    Public Function GetRelProductosSenasa() As responseRelProductosSenasa
            Dim respuesta As New responseRelProductosSenasa
            Dim sqlQuery As String

            Try
                sqlQuery = "SELECT  RSP.CodProducto, RSP.nroOficial, RSP.CodProductoSenasa, SE.Descripcion FROM " + NombreBaseEnsemble + ".dbo.RElSENASAProductos RSP 
                        INNER JOIN " + NombreBaseEnsemble + ".dbo.SENASAEstablecimientos SE on SE.nroOficial = RSP.nroOficial "

                rs.Source = sqlQuery
                Query.Add(rs.Source)
                rs.Abrir()
                If Not rs.EOF Then
                    Dim ListaRegistros As List(Of RelProductosSenasa) = New List(Of RelProductosSenasa)
                    Do While Not rs.EOF
                        Dim registro As New RelProductosSenasa
                        registro.CodProducto = rs("CodProducto").Valor
                        registro.nroOficial = rs("nroOficial").Valor
                        registro.Establecimiento = rs("Descripcion").Valor
                        registro.CodProductoSenasa = rs("CodProductoSenasa").Valor
                        'En todos los registros de Senasa el codigo tiene como prefijo el nro Oficial para que no se dupliquen
                        registro.CodUnicoSenasa = rs("nroOficial").Valor + "-" + rs("CodProductoSenasa").Valor

                        ListaRegistros.Add(registro)
                        rs.MoveNext()
                    Loop
                    respuesta.Items = ListaRegistros
                    respuesta.ConsultaExitosa = True
                Else
                    respuesta.ConsultaExitosa = True
                    respuesta.mensaje = "No existen productos asociados a SENASA"
                End If
                rs.Cerrar()
            Catch ex As Exception
                respuesta.mensaje = "Error BD al consultar la tabla de relacion de productos de senasa"
            End Try

            Return respuesta
        End Function
        Public Function SetRelProductosSenasa(ByVal nroOficial As String, ByVal CodigoProducto As String, ByVal CodigoSenasa As String) As ResponseWS
            Dim respuesta As New ResponseWS
            Dim sqlQuery As String

            Try
                sqlQuery = "INSERT INTO " + NombreBaseEnsemble + ".dbo.RElSENASAProductos (CodProducto,nroOficial,CodProductoSenasa)
            VALUES ('" + CodigoProducto + "', '" + nroOficial + "', '" + CodigoSenasa + "')"
                conn.Execute(sqlQuery)

                respuesta.ConsultaExitosa = True
            Catch ex As Exception
                respuesta.mensaje = "Error BD al insertar la tabla de relacion de productos de senasa"
            End Try

            Return respuesta
        End Function
        Public Function DeleteRelProductosSenasa(ByVal nroOficial As String, ByVal CodigoProducto As String, ByVal CodigoSenasa As String) As ResponseWS
            Dim respuesta As New ResponseWS
            Dim sqlQuery As String

            Try
                sqlQuery = "DELETE FROM " + NombreBaseEnsemble + ".dbo.RElSENASAProductos where CodProducto = '" + CodigoProducto + "' and nroOficial = '" + nroOficial + "' and CodProductoSenasa = '" + CodigoSenasa + "' "
                conn.Execute(sqlQuery)

                respuesta.ConsultaExitosa = True
            Catch ex As Exception
                respuesta.mensaje = "Error BD al eliminar la tabla de relacion de productos de senasa"
            End Try

            Return respuesta
        End Function
        Public Function DeleteRelEmpaqueProducto(ByVal CodigoProducto As String, ByVal EmpaqueId As String) As ResponseWS
            Dim respuesta As New ResponseWS
            Dim sqlQuery As String

            Try
                sqlQuery = "DELETE FROM " + NombreBaseEnsemble + ".dbo.RelEmpaquesProductos where ProductoId = '" + CodigoProducto + "' and EmpaqueId = " + EmpaqueId + " "
                conn.Execute(sqlQuery)

                respuesta.ConsultaExitosa = True
            Catch ex As Exception
                respuesta.mensaje = "Error BD al eliminar la tabla de relacion de productos"
            End Try

            Return respuesta
        End Function
        Public Function DeleteEmpaque(ByVal EmpaqueId As String) As ResponseWS
            Dim respuesta As New ResponseWS
            Dim sqlQuery As String

            Try
                sqlQuery = "DELETE FROM " + NombreBaseEnsemble + ".dbo.Empaques where EmpaqueId = " + EmpaqueId + " "
                conn.Execute(sqlQuery)

                respuesta.ConsultaExitosa = True
            Catch ex As Exception
                respuesta.mensaje = "Error BD al eliminar el empaque seleccionado"
            End Try

            Return respuesta
        End Function
        Public Function UpdateOrderEmpaqueProducto(ByVal CodigoProducto As String, ByVal EmpaqueId As String, ByVal Orden As String) As ResponseWS
            Dim respuesta As New ResponseWS
            Dim sqlQuery As String

            Try
                sqlQuery = "UPDATE " + NombreBaseEnsemble + ".dbo.RelEmpaquesProductos SET Orden = " + Orden + "  where ProductoId = '" + CodigoProducto + "' and EmpaqueId = " + EmpaqueId + " "
                conn.Execute(sqlQuery)

                respuesta.ConsultaExitosa = True
            Catch ex As Exception
                respuesta.mensaje = "Error BD al eliminar la tabla de relacion de productos de senasa"
            End Try

            Return respuesta
        End Function
        Public Function UpdateDescripcionPesoEmpaque(ByVal EmpaqueId As String, ByVal Descripcion As String, ByVal Peso As String) As ResponseWS
            Dim respuesta As New ResponseWS
            Dim sqlQuery As String

            Try
                sqlQuery = "UPDATE " + NombreBaseEnsemble + ".dbo.Empaques SET Descripcion = '" + Descripcion + "', Peso = " + Peso + "  where EmpaqueId = " + EmpaqueId + " "
                conn.Execute(sqlQuery)

                respuesta.ConsultaExitosa = True
            Catch ex As Exception
                respuesta.mensaje = "Error BD al actualizar la tabla de empaques"
            End Try

            Return respuesta
        End Function

        Public Function GetEmpaques() As responseEmpaques
            Dim respuesta As New responseEmpaques
            Dim sqlQuery As String

            Try
                sqlQuery = "SELECT  EmpaqueId, Descripcion, Peso FROM " + NombreBaseEnsemble + ".dbo.Empaques ORDER BY Descripcion ASC "

                rs.Source = sqlQuery
                Query.Add(rs.Source)
                rs.Abrir()
                If Not rs.EOF Then
                    Dim ListaRegistros As List(Of Empaque) = New List(Of Empaque)
                    Do While Not rs.EOF
                        Dim registro As New Empaque
                        registro.EmpaqueId = rs("EmpaqueId").Valor
                        registro.Descripcion = rs("Descripcion").Valor
                        registro.Peso = rs("Peso").Valor
                        ListaRegistros.Add(registro)
                        rs.MoveNext()
                    Loop
                    respuesta.Items = ListaRegistros
                    respuesta.ConsultaExitosa = True
                Else
                    respuesta.ConsultaExitosa = True
                    respuesta.mensaje = "No existen Empaques en la base de datos"
                End If
                rs.Cerrar()
            Catch ex As Exception
                respuesta.mensaje = "Error BD al consultar la tabla de relacion de productos de senasa"
            End Try

            Return respuesta
        End Function

        Public Function GetRelEmpaquesProductos() As responseRelEmpaqueProductos
            Dim respuesta As New responseRelEmpaqueProductos
            Dim sqlQuery As String

            Try
                sqlQuery = "
            SELECT REP.ProductoId, REP.EmpaqueId, E.Descripcion, E.Peso, REP.Orden, P.Descripcion as ProductoNombre 
            FROM " + NombreBaseEnsemble + ".dbo.Empaques E
            INNER JOIN " + NombreBaseEnsemble + ".dbo.RelEmpaquesProductos REP on E.EmpaqueId = REP.EmpaqueId
            INNER JOIN Productos P on REP.ProductoId = P.ProductoId
            ORDER BY P.Descripcion ASC, REP.Orden ASC, E.Descripcion ASC "

                rs.Source = sqlQuery
                Query.Add(rs.Source)
                rs.Abrir()
                If Not rs.EOF Then
                    Dim ListaRegistros As List(Of RelEmpaqueProducto) = New List(Of RelEmpaqueProducto)
                    Do While Not rs.EOF
                        Dim registro As New RelEmpaqueProducto
                        registro.ProductoId = rs("ProductoId").Valor
                        registro.EmpaqueId = rs("EmpaqueId").Valor
                        registro.Descripcion = rs("Descripcion").Valor
                        registro.Peso = rs("Peso").Valor
                        registro.Orden = rs("Orden").Valor
                        registro.ProductoNombre = rs("ProductoNombre").Valor

                        ListaRegistros.Add(registro)
                        rs.MoveNext()
                    Loop
                    respuesta.Items = ListaRegistros
                    respuesta.ConsultaExitosa = True
                Else
                    respuesta.ConsultaExitosa = True
                    respuesta.mensaje = "No existen productos relacionados a algun empaque"
                End If
                rs.Cerrar()
            Catch ex As Exception
                respuesta.mensaje = "Error BD al consultar la tabla de relacion de productos empaques"
            End Try

            Return respuesta
        End Function
        Public Function SetRelEmpaqueProducto(ByVal EmpaqueId As String, ByVal ProductoId As String, ByVal Orden As String) As ResponseWS
            Dim respuesta As New ResponseWS
            Dim sqlQuery As String

            Try
                sqlQuery = "INSERT INTO " + NombreBaseEnsemble + ".dbo.RelEmpaquesProductos (ProductoId,EmpaqueId,Orden)
            VALUES ('" + ProductoId + "', " + EmpaqueId + ", " + Orden + ")"
                conn.Execute(sqlQuery)

                respuesta.ConsultaExitosa = True
            Catch ex As Exception
                respuesta.mensaje = "Error BD al insertar la tabla de relacion de empques productos "
            End Try

            Return respuesta
        End Function
        Public Function SetEmpaque(ByVal Descripcion As String, ByVal Peso As String) As ResponseWS
            Dim respuesta As New ResponseWS
            Dim sqlQuery As String

            Try
                sqlQuery = "SELECT  MAX(EmpaqueId) as EmpaqueId FROM " + NombreBaseEnsemble + ".dbo.Empaques "

                rs.Source = sqlQuery
                Query.Add(rs.Source)
                rs.Abrir()
                Dim UltimoEmpaqueId As Integer = 0
                If Not rs.EOF Then
                    Do While Not rs.EOF
                        UltimoEmpaqueId = IIf(Integer.TryParse(rs("EmpaqueId").Valor, UltimoEmpaqueId), UltimoEmpaqueId, 0)
                        rs.MoveNext()
                    Loop
                Else
                    respuesta.mensaje = "No existen Empaques en la base de datos"
                End If
                rs.Cerrar()

                'incremento el ultimo id
                UltimoEmpaqueId += 1

                sqlQuery = "INSERT INTO " + NombreBaseEnsemble + ".dbo.Empaques (EmpaqueId, Descripcion,Peso) VALUES (" + UltimoEmpaqueId.ToString() + ", '" + Descripcion + "', " + Peso + ")"
                conn.Execute(sqlQuery)
                respuesta.ConsultaExitosa = True

            Catch ex As Exception
                respuesta.mensaje = "Error BD al insertar la tabla de relacion de empques productos "
            End Try

            Return respuesta
        End Function

    Public Function GetFamiliasToleranciaPickeo() As responseFamiliaToleranciaPickeo
        Dim respuesta As New responseFamiliaToleranciaPickeo
        Dim sqlQuery As String

        Try
            sqlQuery = "SELECT  FamiliaToleranciaId, Descripcion, ToleranciaAceptable, ToleranciaCritica, ToleranciaSevera FROM " + NombreBaseEnsemble + ".dbo.FamiliasToleranciaPickeo ORDER BY Descripcion ASC "

            rs.Source = sqlQuery
            Query.Add(rs.Source)
            rs.Abrir()
            If Not rs.EOF Then
                Dim ListaRegistros As List(Of FamiliaToleranciaPickeo) = New List(Of FamiliaToleranciaPickeo)
                Do While Not rs.EOF
                    Dim registro As New FamiliaToleranciaPickeo
                    registro.FamiliaToleranciaId = rs("FamiliaToleranciaId").Valor
                    registro.Descripcion = rs("Descripcion").Valor
                    registro.ToleranciaAceptable = IIf(Double.TryParse(rs("ToleranciaAceptable").Valor, registro.ToleranciaAceptable), registro.ToleranciaAceptable, 0)
                    registro.ToleranciaCritica = IIf(Double.TryParse(rs("ToleranciaCritica").Valor, registro.ToleranciaCritica), registro.ToleranciaCritica, 0)
                    registro.ToleranciaSevera = IIf(Double.TryParse(rs("ToleranciaSevera").Valor, registro.ToleranciaSevera), registro.ToleranciaSevera, 0)
                    ListaRegistros.Add(registro)
                    rs.MoveNext()
                Loop
                respuesta.Items = ListaRegistros
                respuesta.ConsultaExitosa = True
            Else
                respuesta.ConsultaExitosa = True
                respuesta.mensaje = "No existen Empaques en la base de datos"
            End If
            rs.Cerrar()
        Catch ex As Exception
            respuesta.mensaje = "Error BD al consultar la tabla de relacion de productos de senasa"
        End Try

        Return respuesta
    End Function

    Public Function GetRelFamiliasToleranciaPickeo() As responseRelFamiliasToleranciasProductos
        Dim respuesta As New responseRelFamiliasToleranciasProductos
        Dim sqlQuery As String

        Try
            sqlQuery = "SELECT  FamiliaToleranciaId, ProductoId  FROM " + NombreBaseEnsemble + ".dbo.RelFamiliasToleranciasProductos ORDER BY ProductoId ASC "

            rs.Source = sqlQuery
            Query.Add(rs.Source)
            rs.Abrir()
            If Not rs.EOF Then
                Dim ListaRegistros As List(Of RelFamiliasToleranciasProductos) = New List(Of RelFamiliasToleranciasProductos)
                Do While Not rs.EOF
                    Dim registro As New RelFamiliasToleranciasProductos
                    registro.FamiliaToleranciaId = rs("FamiliaToleranciaId").Valor
                    registro.ProductoId = rs("ProductoId").Valor
                    ListaRegistros.Add(registro)
                    rs.MoveNext()
                Loop
                respuesta.Items = ListaRegistros
                respuesta.ConsultaExitosa = True
            Else
                respuesta.ConsultaExitosa = True
                respuesta.mensaje = "No existen Familias asociadas en la base de datos"
            End If
            rs.Cerrar()
        Catch ex As Exception
            respuesta.mensaje = "Error BD al consultar la tabla de relacion de productos de senasa"
        End Try

        Return respuesta
    End Function

    Public Function GetAllRelFamiliasToleranciaPickeo() As responseRelFamiliasToleranciasProductosComplete
        Dim respuesta As New responseRelFamiliasToleranciasProductosComplete
        Dim sqlQuery As String

        Try
            sqlQuery = "SELECT  RFTP.FamiliaToleranciaId, FTP.Descripcion, RFTP.ProductoId, p.Descripcion as ProductoNombre  FROM " + NombreBaseEnsemble + ".dbo.RelFamiliasToleranciasProductos RFTP 
                        INNER JOIN productos p on RFTP.ProductoId = p.ProductoId  
                        INNER JOIN " + NombreBaseEnsemble + ".dbo.FamiliasToleranciaPickeo FTP on FTP.FamiliaToleranciaId = RFTP.FamiliaToleranciaId ORDER BY FTP.Descripcion, p.ProductoId ASC "

            rs.Source = sqlQuery
            Query.Add(rs.Source)
            rs.Abrir()
            If Not rs.EOF Then
                Dim ListaRegistros As List(Of RelFamiliasToleranciasProductosComplete) = New List(Of RelFamiliasToleranciasProductosComplete)
                Do While Not rs.EOF
                    Dim registro As New RelFamiliasToleranciasProductosComplete
                    registro.FamiliaToleranciaId = rs("FamiliaToleranciaId").Valor
                    registro.ProductoId = rs("ProductoId").Valor
                    registro.Descripcion = rs("Descripcion").Valor
                    registro.ProductoNombre = rs("ProductoNombre").Valor
                    ListaRegistros.Add(registro)
                    rs.MoveNext()
                Loop
                respuesta.Items = ListaRegistros
                respuesta.ConsultaExitosa = True
            Else
                respuesta.ConsultaExitosa = True
                respuesta.mensaje = "No existen Familias asociadas en la base de datos"
            End If
            rs.Cerrar()
        Catch ex As Exception
            respuesta.mensaje = "Error BD al consultar la tabla de relacion de productos de senasa"
        End Try

        Return respuesta
    End Function

    Public Function SetRelFamiliasToleranciasProducto(ByVal FamiliaId As String, ByVal ProductoId As String) As ResponseWS
        Dim respuesta As New ResponseWS
        Dim sqlQuery As String

        Try
            sqlQuery = "INSERT INTO " + NombreBaseEnsemble + ".dbo.RelFamiliasToleranciasProductos (FamiliaToleranciaId, ProductoId)
            VALUES (" + FamiliaId + ", '" + ProductoId + "')"
            conn.Execute(sqlQuery)

            respuesta.ConsultaExitosa = True
        Catch ex As Exception
            respuesta.mensaje = "Error BD al insertar la tabla de relacion de familias de tolerancias y productos "
        End Try

        Return respuesta
    End Function

    Public Function DeleteRelFamiliasToleranciasProducto(ByVal FamiliaId As String, ByVal ProductoId As String) As ResponseWS
        Dim respuesta As New ResponseWS
        Dim sqlQuery As String

        Try
            sqlQuery = "DELETE FROM " + NombreBaseEnsemble + ".dbo.RelFamiliasToleranciasProductos where ProductoId = '" + ProductoId + "' and FamiliaToleranciaId = " + FamiliaId + " "
            conn.Execute(sqlQuery)

            respuesta.ConsultaExitosa = True
        Catch ex As Exception
            respuesta.mensaje = "Error BD al eliminar la tabla de relacion de familias productos"
        End Try

        Return respuesta
    End Function


End Class
