Imports Newtonsoft.Json

Public Class LuzAzulPedidos
    Inherits LuzAzulCommon
    Public Class ResponseClientes
        Inherits ResponseWS

        Public rs As IEnumerable(Of ClientePedido)
    End Class
    Public Class ResponseAgrupaciones
        Inherits ResponseWS

        Public rs As IEnumerable(Of Agrupacion)
    End Class

    Public Class ResponseListProductos
        Inherits ResponseWS

        Public rs As IEnumerable(Of ListProducto)
    End Class

    Public Class ResponseEmailAplicacionEstablecimento
        Inherits ResponseWS

        Public rs As IEnumerable(Of EmailEstablecimiento)
    End Class

    Public Class ResponseProductosAgrupaciones
        Inherits ResponseWS

        Public rs As IEnumerable(Of ProductoAgrupacion)
    End Class
    Public Class ResponseProductosClasificacion
        Inherits ResponseWS

        Public rs As IEnumerable(Of ProductoClasificacion)
    End Class

    Public Class ResponseClasificacionesClientes
        Inherits ResponseWS

        Public rs As IEnumerable(Of String)
    End Class

    Public Class ClientePedido
        Public Property ClienteId As String
        Public Property RazonSocial As String
        Public Property CUIT As String
        Public Sub New(ByVal strClienteId As String, StrRazonSocial As String, StrCUIT As String)
            ClienteId = strClienteId
            RazonSocial = StrRazonSocial
            CUIT = StrCUIT
        End Sub
    End Class

    Public Class ReLProductoAgrupacion
        Public Property ProductoId As String
        Public Property AgrupacionId As String
        Public Sub New(ByVal ProdId As String, ByVal AgrupId As String)
            ProductoId = ProdId
            AgrupacionId = AgrupId
        End Sub

    End Class
    Public Class Agrupacion
        Public Property AgrupacionId As String
        Public Property Descripcion As String
        Public Property ColorAgrupacion As String
        Public Property Packing As String
        Public Property Unidad As String
        Public Property KgPromedioUnidad As Double
        Public Property UnidadesPorBulto As Integer
        Public Property ListProductos As List(Of String)

        Public Sub New(ByVal StrAgrupacionId As String, DescripcionAgrupacion As String)
            AgrupacionId = StrAgrupacionId
            Descripcion = DescripcionAgrupacion
            ListProductos = New List(Of String)
        End Sub
    End Class

    Public Class DetalleProductoAgrupacion
        Public Property ProductoId As String
        Public Property Descripcion As String
        Public Property Cantidad As String
        Public Property AgrupacionId As String
        Public Sub New(ByVal StrProductoId As String, DescripcionProducto As String, strCantidad As String, ByVal StrAgrupacionId As String)
            ProductoId = StrProductoId
            Descripcion = DescripcionProducto
            Cantidad = strCantidad
            AgrupacionId = StrAgrupacionId
        End Sub
    End Class

    Public Class ListProducto
        Public Property ProveedorId As String
        Public Property ProductoId As String
        Public Property Descripcion As String
        Public Property Stock As String
        Public Property Venta As String
        Public Property Packing As String
        Public Property Sugerida As Double
        Public Property Apedir As Double
        Public Property UniPedido As String
        Public Property KgPromedioUnidad As Double
        Public Property UnidadesPorBulto As Integer
        Public Property PrecioCompra As Double
        Public Property SeVendePorPeso As Boolean
        Public Property SePidePorBulto As Boolean
        Public Property ColorProducto As String
        Public Property TipoUnidadMedidaId2 As String

        Public Sub New(ByVal ProvId As String, ByVal ProdId As String, DescripcionClasificacion As String, ByVal Pack As String, ByVal Cantstock As String, ByVal cantVenta As String, ByVal UnidadPedido As String,
                       ByVal pedir As Double, ByVal sugeridas As Double, ByVal KgPromedioUnid As Double, ByVal UnidadesXBulto As Integer, ByVal PreciodeCompra As Double, SeVendeXPeso As Boolean, ColorProd As String, ByVal bSePidePorBoolto As Boolean, ByVal TipoUnidMedida2 As String)
            ProveedorId = ProvId
            ProductoId = ProdId
            Descripcion = DescripcionClasificacion
            Packing = Pack
            Stock = Cantstock
            Venta = cantVenta
            Apedir = pedir
            Sugerida = sugeridas
            UniPedido = UnidadPedido
            KgPromedioUnidad = KgPromedioUnid
            UnidadesPorBulto = UnidadesXBulto
            PrecioCompra = PreciodeCompra
            SeVendePorPeso = SeVendeXPeso
            ColorProducto = ColorProd
            SePidePorBulto = bSePidePorBoolto
            TipoUnidadMedidaId2 = TipoUnidMedida2
        End Sub
    End Class

    Public Class EmailEstablecimiento
        Public Property AplicacionId As String
        Public Property EstablecimientoId As String
        Public Property TipoNotificacionId As String
        Public Property Email As String
        Public Sub New(ByVal Aplicacion_Id As String, Establecimiento_Id As String, TipoNotificacion_Id As String, Emails As String)
            AplicacionId = Aplicacion_Id
            EstablecimientoId = Establecimiento_Id
            TipoNotificacionId = TipoNotificacion_Id
            Email = Emails
        End Sub
    End Class


    Public Class ProductoAgrupacion
        Public Property AgrupacionId As String
        Public Property ProductoId As String
        Public Sub New(ByVal StrAgrupacionId As String, StrProductosId As String)
            AgrupacionId = StrAgrupacionId
            ProductoId = StrProductosId
        End Sub
    End Class
    Public Class ProductoClasificacion
        Public Property ClasificacionId As String
        Public Property ClasificacionNombre As String
        Public Property ProductoId As String
    End Class

    Public Function GetClientes() As ResponseClientes
        Dim respuesta As New ResponseClientes

        Try
            rs.Source = "SELECT ClienteId, RazonSocial, CUIT FROM Clientes WHERE EstadoClienteId = 1 "
            Query.Add(rs.Source)
            rs.Abrir()
            If Not rs.EOF Then
                Dim ListaRegistros As List(Of ClientePedido) = New List(Of ClientePedido)
                Do While Not rs.EOF
                    ListaRegistros.Add(New ClientePedido(rs("ClienteId").Valor, rs("RazonSocial").Valor, rs("CUIT").Valor))
                    rs.MoveNext()
                Loop
                respuesta.ConsultaExitosa = True
                respuesta.rs = ListaRegistros
            Else
                respuesta.mensaje = "No se encontro el parametro CUIT de la Empresa, comuniquese con el Administrador"
            End If
            rs.Cerrar()
        Catch ex As Exception
            respuesta.mensaje = "Error BD No se encuentra el campo CUIT en los parametros de la empresa"
        End Try

        Return respuesta
    End Function

    Public Function GetEmailsAplicacionEstablecimiento(ByVal EstablecimientoId As String, ByVal NombreAplicacion As String, ByVal TipoNotificacionId As String) As ResponseEmailAplicacionEstablecimento
        Dim respuesta As New ResponseEmailAplicacionEstablecimento
        Dim ListaRegistros As List(Of EmailEstablecimiento) = New List(Of EmailEstablecimiento)

        Try
            rs.Source = "SELECT ren.Email, ren.AplicacionId, ren.EstablecimientoId, ren.TipoNotificacionId  
                FROM " + NombreBaseEnsemble + ".dbo.Aplicaciones a
                JOIN " + NombreBaseEnsemble + ".dbo.RelEstablecimientosNotificaciones ren ON ren.AplicacionId = a.AplicacionId
                WHERE a.Descripcion = '" + NombreAplicacion + "' AND ren.TipoNotificacionId = " + TipoNotificacionId + " 
                AND ren.EstablecimientoId = " + EstablecimientoId

            Query.Add(rs.Source)
            rs.Abrir()
            If Not rs.EOF Then
                Do While Not rs.EOF
                    ListaRegistros.Add(New EmailEstablecimiento(rs("AplicacionId").Valor.ToString(), rs("EstablecimientoId").Valor.ToString(), rs("TipoNotificacionId").Valor.ToString(), rs("Email").Valor))
                    rs.MoveNext()
                Loop
            Else
                respuesta.mensaje = "El establecimiento no tiene email asociados a esta aplicacion"
            End If
            rs.Cerrar()

            respuesta.ConsultaExitosa = True
            respuesta.rs = ListaRegistros
        Catch ex As Exception
            respuesta.mensaje = "Error BD consultando las notificaciones para la aplicación"
        End Try

        Return respuesta
    End Function



    Public Function GetProductosProveedor(ListProv As List(Of Clasificacion), ListDepositos As List(Of Deposito), ByVal EsFabrica As Boolean) As ResponseListProductos
        Dim respuesta As New ResponseListProductos

        Try
            Dim SqlQuery As String = ""
            Dim SqlProveedores As String = ""
            Dim SqlDepositos As String = ""

            ' Creo tabla temporal para poder realizar un JOIN contra los query para obtener la info
            SqlQuery = "CREATE TABLE #tmpConfigPedido (ClasificacionProdId INT, FrecuenciaPed INT, PlazoEntrega INT, DiasReparo INT)"
            conn.Execute(SqlQuery)
            Query.Add(SqlQuery)

            If ListProv.Count > 0 Then
                For Each Clas As Clasificacion In ListProv
                    SqlProveedores += Clas.ClasificacionProdId + ", "
                    conn.Execute("INSERT INTO #tmpConfigPedido VALUES (" + Clas.ClasificacionProdId.ToString() + "," + Clas.FrecuenciaPed.ToString() + "," + Clas.PlazoEntregaPed.ToString() + "," + Clas.CantDiasReparo.ToString + ")")
                Next
                ' Elimino el ultimo ", "
                SqlProveedores = SqlProveedores.Substring(0, SqlProveedores.Length - 2)
            Else
                SqlProveedores = "-9999" 'asigno un proveedor que no exista para que el query no de error y para que no traiga registros
            End If

            If ListDepositos.Count > 0 Then
                For Each Depo As Deposito In ListDepositos
                    SqlDepositos += Depo.DepositoId + ", "
                Next
                ' Elimino el ultimo ", "
                SqlDepositos = SqlDepositos.Substring(0, SqlDepositos.Length - 2)
            Else
                SqlDepositos = "-9999" 'asigno un deposito que no exista para que el query no de error y para que no traiga registros
            End If

            ' Obtengo el collation de la base de datos
            Dim sCollation As String = "SQL_Latin1_General_CP1_CI_AS"
            rs.Source = "SELECT collation_name FROM sys.databases WHERE name = '" & conn.Base & "'"
            Query.Add(rs.Source)
            rs.Abrir()
            If Not rs.EOF() Then sCollation = rs("collation_name").Valor
            rs.Cerrar()

            ' Obtengo la cantidad de dias de venta (Si se vendieron mas de 10 Kg de Cremoso)
            Dim CantDiasVentas As Integer = 14
            rs.Source = "SELECT ISNULL(COUNT(*), 14) CantDiasVenta FROM (SELECT CONVERT(Date, dc.Fecha) Fecha
                FROM DocumentosCabecera dc
                JOIN RelProductosListasPrecios rp ON rp.ProductoId = '001' AND rp.ListaPrecioId = 1
                WHERE dc.EstadoDocumentoId<>3 AND (dc.TipoDocumentoId LIKE 'FC%') AND dc.DepositoId IN (" & SqlDepositos & ")
                AND dc.Fecha BETWEEN CONVERT(Date, DATEADD(d, -14, GETDATE()), 101) AND CONVERT(Date, GETDATE(), 101) AND dc.SucursalId NOT LIKE '80__'
                GROUP BY CONVERT(Date,dc.Fecha), rp.Precio
                HAVING SUM(dc.Total) > (10 * rp.Precio * 1.21)) a"

            Query.Add(rs.Source)
            rs.Abrir()
            ' Divido por dos la cantidad de dias de venta, porque de las 2 semanas voy a tomar el promedio de la semana que me de mayor
            If Not rs.EOF Then CantDiasVentas = IIf(Val(rs("CantDiasVenta").Valor) < 2, 1, Val(rs("CantDiasVenta").Valor) / 2)
            rs.Cerrar()

            Dim sSQLWHEN1erSemanaCommon As String = "dc.Fecha BETWEEN CONVERT(DateTime, CONVERT(VARCHAR(12), CONVERT(Date, DATEADD(d, -14, GETDATE()))) + ' 00:00:00',101) 
	                         AND CONVERT(DateTime, CONVERT(VARCHAR(12), CONVERT(Date, DATEADD(d, -8, GETDATE()))) + ' 23:59:59',101)"
            Dim sSQLWHEN2daSemanaCommon As String = "dc.Fecha BETWEEN CONVERT(DateTime, CONVERT(VARCHAR(12), CONVERT(Date, DATEADD(d, -7, GETDATE()))) + ' 00:00:00',101) AND GETDATE()"

            Dim sSQLJOINClasif As String = "SELECT DISTINCT r.ProductoId, r.ClasificacionProdId, 
                ISNULL(c.FrecuenciaPed, 0) FrecuenciaPed, ISNULL(c.PlazoEntrega, 0) PlazoEntrega, ISNULL(c.DiasReparo, 0) DiasReparo
                FROM RelProductosClasificacionesProductos r 
                JOIN #tmpConfigPedido c ON c.ClasificacionProdId = r.ClasificacionProdId
                WHERE r.ClasificacionProdId IN (" & SqlProveedores & ")"
            Dim sSQLJOINUMedSal As String = "JOIN UnidadesMedida um ON um.TipoUnidadMedidaId = p.TipoUnidadMedidaId2 AND um.UnidadMedidaId = p.UnidadMedidaId2"
            Dim sSQLJOINUMedEnt As String = "JOIN UnidadesMedida um ON um.TipoUnidadMedidaId = p2.TipoUnidadMedidaId2 AND um.UnidadMedidaId = p2.UnidadMedidaId2"

            Dim sSQLSELECTProdEntCommon As String = "SELECT sa.ClasificacionProdId, dd.ProductoId, p.Descripcion, p.Packing, p.PrecioCompra, p.CodBarra, p.CantUnidadMedida1, 
                CASE WHEN CHARINDEX('{#',p.Observaciones)>0 THEN SUBSTRING(p.Observaciones,CHARINDEX('{#',p.Observaciones) + 1, CHARINDEX('}',p.Observaciones) - CHARINDEX('{#',p.Observaciones) - 1) ELSE '' END Color, 
                p.TipoUnidadMedidaId2, p.UnidadMedidaId2, p.CantUnidadMedida2, um.Descripcion UniPedido, sa.Stock, SUM(
                CASE 
                WHEN " & sSQLWHEN1erSemanaCommon & " THEN IIF(dc.TipoDocumentoId LIKE 'NC%', dd.Cantidad*-1, dd.Cantidad) 
                ELSE 0 END) + ISNULL(prod.producido1erSemana,0) Venta1erSemana, SUM(
                CASE 
                WHEN " & sSQLWHEN2daSemanaCommon & " THEN IIF(dc.TipoDocumentoId LIKE 'NC%', dd.Cantidad*-1, dd.Cantidad) 
                ELSE 0 END) + ISNULL(prod.producido2daSemana,0) Venta2daSemana,
                sa.FrecuenciaPed, sa.PlazoEntrega, sa.DiasReparo, ISNULL(cfgpp.UnidadesMin, 0) UnidadesMin "

            Dim sSQLFROMCommon As String = "FROM DocumentosDetalle dd 
                JOIN DocumentosCabecera dc ON dc.DocumentoId=dd.DocumentoId 
                JOIN Productos p ON p.ProductoId=dd.ProductoId"

            Dim sSQLJOINStock As String = "SELECT p.ProductoId, rp.ClasificacionProdId, rp.FrecuenciaPed, rp.PlazoEntrega, rp.DiasReparo, ISNULL(s.StockActual,0) Stock FROM Productos p 
                JOIN (" & sSQLJOINClasif & ") rp ON rp.ProductoId=p.ProductoId 
                LEFT JOIN (SELECT DISTINCT ProductoId, SUM(StockActual) StockActual FROM Stock 
                WHERE DepositoId IN (" & SqlDepositos & ") GROUP BY ProductoId) s ON s.ProductoId=p.ProductoId 
                GROUP BY p.ProductoId, rp.ClasificacionProdId, rp.FrecuenciaPed, rp.PlazoEntrega, rp.DiasReparo, s.StockActual"

            Dim sSQLJOINFormulas As String = "JOIN FormulasProductosSal fs ON fs.ProductoId=dd.ProductoId AND fs.FormulaId=p.FormulaCostoId 
                JOIN FormulasProductosEnt fe ON fe.FormulaId=fs.FormulaId 
                JOIN Productos p2 ON p2.ProductoId=fe.ProductoId"

            Dim sSQLWHERECommon As String = "WHERE dc.Fecha BETWEEN CONVERT(DateTime, CONVERT(VARCHAR(12), CONVERT(Date, DATEADD(d, -14, GETDATE()))) + ' 00:00:00',101) AND GETDATE() 
                AND dc.DepositoId IN (" & SqlDepositos & ") AND dc.SucursalId NOT LIKE '80__'  
                AND (dc.TipoDocumentoId LIKE 'FC%' OR dc.TipoDocumentoId LIKE 'NC%')"

            SqlQuery = "CREATE TABLE #tmpProducido (ProductoId VARCHAR(25) COLLATE " + sCollation + ", Producido1erSemana DECIMAL(18,5), Producido2daSemana DECIMAL(18,5))"
            conn.Execute(SqlQuery)
            Query.Add(SqlQuery)

            SqlQuery = "CREATE TABLE #tmpPedido (ClasificacionProdId INT,
                ProductoId VARCHAR(25) COLLATE " + sCollation + ",  Descripcion VARCHAR(50) COLLATE " + sCollation + ",
                Packing DECIMAL(18,5), PrecioCompra MONEY, CodBarra VARCHAR(25) COLLATE " + sCollation + ",
                CantUnidadMedida1 DECIMAL(18,5), Color VARCHAR(50) COLLATE " + sCollation + ", TipoUnidadMedidaId2 INT, 
                UnidadMedidaId2 INT, CantUnidadMedida2 DECIMAL(18,5), UniPedido VARCHAR(50) COLLATE " + sCollation + ",
                Stock DECIMAL(18,5), Venta1erSemana DECIMAL(18,5), Venta2daSemana DECIMAL(18,5), FrecuenciaPed INT, PlazoEntrega INT, DiasReparo INT, UnidadesMin INT)"
            conn.Execute(SqlQuery)
            Query.Add(SqlQuery)

            Dim sSQLSELECTProducidoCommon As String = "SELECT v.ProductoId, SUM(v.producido1erSemana) producido1erSemana, SUM(v.producido2daSemana) producido2daSemana 
                FROM (SELECT p2.ProductoId, 
	                 SUM(CASE 
                        WHEN " & sSQLWHEN1erSemanaCommon & " THEN IIF(dc.TipoDocumentoId LIKE 'NC%', dd.Cantidad*-1, dd.Cantidad)*(fe.Cantidad/fs.Cantidad) 
                        ELSE 0 END) producido1erSemana,
                    SUM(CASE 
                        WHEN " & sSQLWHEN2daSemanaCommon & " THEN IIF(dc.TipoDocumentoId LIKE 'NC%', dd.Cantidad*-1, dd.Cantidad)*(fe.Cantidad/fs.Cantidad) 
                        ELSE 0 END) producido2daSemana
                    " & sSQLFROMCommon &
                    " " & sSQLJOINFormulas &
                    " JOIN (" & sSQLJOINClasif & ") rp ON rp.ProductoId=p2.ProductoId"

            Dim sSQLGROUPBYPedidoCommon As String = "GROUP BY sa.ClasificacionProdId, dd.ProductoId, p.Descripcion, p.PrecioCompra, p.CodBarra, p.Packing, p.CantUnidadMedida1, p.Observaciones, 
                    p.TipoUnidadMedidaId2, p.UnidadMedidaId2, p.CantUnidadMedida2, um.Descripcion, sa.Stock, prod.producido1erSemana, prod.producido2daSemana,
                    sa.FrecuenciaPed, sa.PlazoEntrega, sa.DiasReparo, cfgpp.UnidadesMin"

            Dim sSQLSELECTPedPromosCommon As String = "SELECT v.ClasificacionProdId, v.ProductoId, v.Descripcion, v.Packing, v.PrecioCompra, v.CodBarra, v.CantUnidadMedida1, v.Color, 
                    v.TipoUnidadMedidaId2, v.UnidadMedidaId2, v.CantUnidadMedida2, v.UniPedido, v.Stock, SUM(v.Producido1erSemana) Producido1erSemana, SUM(v.Producido2daSemana) Producido2daSemana, 
                    v.FrecuenciaPed, v.PlazoEntrega, v.DiasReparo, v.UnidadesMin
                    FROM (SELECT sa.ClasificacionProdId, p2.ProductoId, p2.Descripcion, p2.Packing, p2.PrecioCompra, p2.CodBarra, p2.CantUnidadMedida1, 
                        CASE WHEN CHARINDEX('{#',p2.Observaciones)>0 THEN SUBSTRING(p2.Observaciones,CHARINDEX('{#',p2.Observaciones) + 1, CHARINDEX('}',p2.Observaciones) - CHARINDEX('{#',p2.Observaciones) - 1) ELSE '' END Color, 
                        p2.TipoUnidadMedidaId2, p2.UnidadMedidaId2, p2.CantUnidadMedida2, um.Descripcion UniPedido, sa.Stock, 
                        SUM(CASE 
                            WHEN " & sSQLWHEN1erSemanaCommon & " THEN IIF(dc.TipoDocumentoId LIKE 'NC%', dd.Cantidad*-1, dd.Cantidad)
                            ELSE 0 END)*(fe.Cantidad/fs.Cantidad) Producido1erSemana,
                        SUM(CASE 
                            WHEN " & sSQLWHEN2daSemanaCommon & " THEN IIF(dc.TipoDocumentoId LIKE 'NC%', dd.Cantidad*-1, dd.Cantidad)
                            ELSE 0 END)*(fe.Cantidad/fs.Cantidad) Producido2daSemana,
                        sa.FrecuenciaPed, sa.PlazoEntrega, sa.DiasReparo, ISNULL(cfgpp.UnidadesMin, 0) UnidadesMin"

            Dim sSQLGROUPBYPedPromosCommon As String = "GROUP BY sa.ClasificacionProdId, p2.ProductoId, p2.Descripcion, fe.Cantidad, fs.Cantidad, p2.Packing, p2.PrecioCompra, p2.CodBarra, 
                        p2.Observaciones, p2.CantUnidadMedida1, p2.TipoUnidadMedidaId2, p2.UnidadMedidaId2, p2.CantUnidadMedida2, um.Descripcion, sa.Stock, sa.FrecuenciaPed, sa.PlazoEntrega, 
                        sa.DiasReparo, cfgpp.UnidadesMin) v 
                    GROUP BY v.ClasificacionProdId, v.ProductoId, v.Descripcion, v.Packing, v.PrecioCompra, v.CodBarra, v.CantUnidadMedida1, v.Color,
                    v.TipoUnidadMedidaId2, v.UnidadMedidaId2, v.CantUnidadMedida2, v.UniPedido, v.Stock, v.FrecuenciaPed, v.PlazoEntrega, v.DiasReparo, v.UnidadesMin"

            Dim sSQLPedSinVentaCommon As String = "SELECT sa.ClasificacionProdId, p.ProductoId, p.Descripcion, p.Packing, p.PrecioCompra, p.CodBarra, p.CantUnidadMedida1,
                    CASE WHEN CHARINDEX('{#',p.Observaciones)>0 THEN SUBSTRING(p.Observaciones,CHARINDEX('{#',p.Observaciones) + 1, CHARINDEX('}',p.Observaciones) - CHARINDEX('{#',p.Observaciones) - 1) ELSE '' END Color,
                    p.TipoUnidadMedidaId2, p.UnidadMedidaId2, p.CantUnidadMedida2, um.Descripcion UniPedido,
                    ISNULL(sa.Stock, 0) Stock, 0 Venta1erSemana, 0 Venta2daSemana, sa.FrecuenciaPed, sa.PlazoEntrega, sa.DiasReparo, ISNULL(cfgpp.UnidadesMin, 0) UnidadesMin               
                    FROM Productos p 
                    " & sSQLJOINUMedSal &
                    " JOIN (" & sSQLJOINClasif & ") rp ON rp.ProductoId=p.ProductoId                     
                    LEFT JOIN (" & sSQLJOINStock & ") sa ON sa.ProductoId=p.ProductoId 
                    LEFT JOIN " + NombreBaseEnsemble + "..ConfigPedidoProducto cfgpp ON cfgpp.ProductoId = p.ProductoId
                    LEFT JOIN (SELECT ProductoId FROM #tmpPedido) vs ON vs.ProductoId = p.ProductoId"

            Dim tmpSQL As String = ""

            If EsFabrica = True Then
                Dim sSQLPromos As String = "SELECT DISTINCT(fs.ProductoId) FROM FormulasProductosSal fs
                    JOIN RelFormulasTipoFases r ON r.FormulaId=fs.FormulaId 
                    JOIN " + NombreBaseEnsemble + "..GWRConfiguracion c ON c.Valor=r.TipoFaseId
                    WHERE c.Parametro='GWRTipoFaseIdMigraSuc'"

                tmpSQL = "INSERT INTO #tmpProducido " &
                    sSQLSELECTProducidoCommon &
                        " JOIN RelFormulasTipoFases r ON r.FormulaId=fs.FormulaId 
	                     JOIN " + NombreBaseEnsemble + "..GWRConfiguracion c ON c.Valor=r.TipoFaseId                         
                        " & sSQLWHERECommon &
                        " AND c.Parametro='GWRTipoFaseIdMigraSuc' AND p.FormulaCostoId IS NOT NULL 
	                     GROUP BY p2.ProductoId, fe.Cantidad, fs.Cantidad) v 
                    GROUP BY v.ProductoId"
                conn.Execute(tmpSQL)

                tmpSQL = "INSERT INTO #tmpPedido 
                    " & sSQLSELECTProdEntCommon &
                    " " & sSQLFROMCommon &
                    " " & sSQLJOINUMedSal &
                    " JOIN (" & sSQLJOINStock & ") sa ON sa.ProductoId=dd.ProductoId 
                    LEFT JOIN (" & sSQLPromos & ") fs ON fs.ProductoId=p.ProductoId 
                    LEFT JOIN " + NombreBaseEnsemble + "..ConfigPedidoProducto cfgpp ON cfgpp.ProductoId = dd.ProductoId
                    LEFT JOIN #tmpProducido prod ON prod.ProductoId=p.ProductoId 
                    " & sSQLWHERECommon &
                    " AND dd.Descuento=0 AND fs.ProductoId IS NULL " & sSQLGROUPBYPedidoCommon
                conn.Execute(tmpSQL)
                Query.Add(tmpSQL)

                tmpSQL = "INSERT INTO #tmpPedido " &
                    sSQLSELECTPedPromosCommon &
                        " " & sSQLFROMCommon &
                        " " & sSQLJOINFormulas &
                        " " & sSQLJOINUMedEnt &
                        " JOIN (" & sSQLJOINStock & ") sa ON sa.ProductoId=p2.ProductoId 
                        JOIN (" & sSQLPromos & ") fpromo ON fpromo.ProductoId=p.ProductoId
                        LEFT JOIN " + NombreBaseEnsemble + "..ConfigPedidoProducto cfgpp ON cfgpp.ProductoId = p2.ProductoId
                        LEFT JOIN (SELECT ProductoId FROM #tmpPedido) ddp ON ddp.ProductoId=p2.ProductoId 
                        " & sSQLWHERECommon &
                        " AND p.FormulaCostoId IS NOT NULL AND ddp.ProductoId IS NULL " &
                    sSQLGROUPBYPedPromosCommon
                conn.Execute(tmpSQL)
                Query.Add(tmpSQL)

                tmpSQL = "INSERT INTO #tmpPedido " &
                    sSQLPedSinVentaCommon &
                    " LEFT JOIN (" & sSQLPromos & ") pf ON pf.ProductoId=p.ProductoId 
                    WHERE pf.ProductoId IS NULL AND vs.ProductoId IS NULL"
                conn.Execute(tmpSQL)
                Query.Add(tmpSQL)
            Else
                tmpSQL = "INSERT INTO #tmpProducido " &
                    sSQLSELECTProducidoCommon &
                        " " & sSQLWHERECommon &
                        " AND p.FormulaCostoId IS NOT NULL 
                        GROUP BY p2.ProductoId, fe.Cantidad, fs.Cantidad) v
                    GROUP BY v.ProductoId"
                conn.Execute(tmpSQL)
                Query.Add(tmpSQL)

                tmpSQL = "INSERT INTO #tmpPedido 
                    " & sSQLSELECTProdEntCommon &
                    " " & sSQLFROMCommon &
                    " " & sSQLJOINUMedSal &
                    " JOIN (" & sSQLJOINStock & ") sa ON sa.ProductoId=dd.ProductoId 
                    LEFT JOIN " + NombreBaseEnsemble + "..ConfigPedidoProducto cfgpp ON cfgpp.ProductoId = dd.ProductoId
                    LEFT JOIN #tmpProducido prod ON prod.ProductoId=p.ProductoId 
                    " & sSQLWHERECommon &
                    " AND dd.Descuento=0 AND p.FormulaCostoId IS NULL " & sSQLGROUPBYPedidoCommon
                conn.Execute(tmpSQL)
                Query.Add(tmpSQL)

                tmpSQL = "INSERT INTO #tmpPedido " &
                    sSQLSELECTPedPromosCommon &
                        " " & sSQLFROMCommon &
                        " " & sSQLJOINFormulas &
                        " " & sSQLJOINUMedEnt &
                        " JOIN (" & sSQLJOINStock & ") sa ON sa.ProductoId=p2.ProductoId 
                        LEFT JOIN " + NombreBaseEnsemble + "..ConfigPedidoProducto cfgpp ON cfgpp.ProductoId = p2.ProductoId
                        LEFT JOIN (SELECT ProductoId FROM #tmpPedido) ddp ON ddp.ProductoId=p2.ProductoId  
                        " & sSQLWHERECommon &
                        " AND p.FormulaCostoId IS NOT NULL AND ddp.ProductoId IS NULL " &
                    sSQLGROUPBYPedPromosCommon
                conn.Execute(tmpSQL)
                Query.Add(tmpSQL)

                tmpSQL = "INSERT INTO #tmpPedido " &
                    sSQLPedSinVentaCommon &
                    " WHERE p.FormulaCostoId IS NULL AND vs.ProductoId IS NULL"
                conn.Execute(tmpSQL)
                Query.Add(tmpSQL)
            End If

            'Ejecuto el query
            rs.Source = "SELECT * FROM #tmpPedido ORDER BY ClasificacionProdId, CodBarra, UnidadMedidaId2, CantUnidadMedida2, ProductoId"
            Query.Add(rs.Source)
            rs.Abrir()

            'si encontro un registro
            If Not rs.EOF Then
                Dim ListaRegistros As New List(Of ListProducto)
                Do While Not rs.EOF
                    Dim ProductoId As String = rs("ProductoId").Valor
                    Dim Vta1erSemana As Double = rs("Venta1erSemana").Valor
                    Dim Vta2daSemana As Double = rs("Venta2daSemana").Valor
                    Dim VtaTotal As Double = Vta1erSemana + Vta2daSemana
                    Dim StockTotal As Double = rs("Stock").Valor
                    Dim ColorProducto As String = rs("Color").Valor

                    Dim SeVendePorPeso As Boolean = CInt(rs("TipoUnidadMedidaId2").Valor = 2) Or CInt(rs("TipoUnidadMedidaId2").Valor) = 4
                    Dim SePidePorBulto As Boolean = CInt(rs("UnidadMedidaId2").Valor) <> 0 And Not rs("UniPedido").Valor.ToString().Contains(" x ") And CInt(rs("TipoUnidadMedidaId2").Valor) < 3
                    Dim KgPromedioUnid As Double = IIf(CDbl(rs("CantUnidadMedida1").Valor) = 0, 1, CDbl(rs("CantUnidadMedida1").Valor))
                    Dim UnidadesPorBulto As Double = IIf(CInt(rs("CantUnidadMedida2").Valor) = 0, 1, rs("CantUnidadMedida2").Valor)
                    Dim FrecuenciaPed As Integer = rs("FrecuenciaPed").Valor
                    Dim PlazoEntrega As Integer = rs("PlazoEntrega").Valor
                    Dim DiasReparo As Integer = rs("DiasReparo").Valor
                    Dim PrecioCompra As Double = Val(rs("PrecioCompra").Valor)
                    Dim StockMin As Double = IIf(SeVendePorPeso, rs("UnidadesMin").Valor * KgPromedioUnid, rs("UnidadesMin").Valor)
                    Dim UniPedido As String = IIf(CInt(rs("UnidadMedidaId2").Valor) <> 0 And Not rs("UniPedido").Valor.ToString().Contains(" x "),
                                                  rs("UniPedido").Valor + " x " + UnidadesPorBulto.ToString,
                                                  rs("UniPedido").Valor)

                    Dim VtaDiaria As Double = IIf(Vta1erSemana > Vta2daSemana, Vta1erSemana / CantDiasVentas, Vta2daSemana / CantDiasVentas)
                    Dim VtaProyCalc As Double = (VtaDiaria * FrecuenciaPed) + (VtaDiaria * PlazoEntrega) + (VtaDiaria * DiasReparo)
                    Dim VtaProyectada As Double = IIf(VtaProyCalc >= StockMin, VtaProyCalc, StockMin)
                    Dim VtaUnidProyectada As Integer = IIf(SeVendePorPeso, CInt(Math.Round(VtaProyectada / KgPromedioUnid, 0)), CInt(Math.Round(VtaProyectada, 0)))
                    Dim StockUnidades As Integer = IIf(SeVendePorPeso, CInt(Math.Floor(StockTotal / KgPromedioUnid)), CInt(Math.Floor(StockTotal)))
                    Dim CantAPedir As Double = IIf(SePidePorBulto, (VtaUnidProyectada - StockUnidades) / UnidadesPorBulto, VtaUnidProyectada - StockUnidades)
                    CantAPedir = IIf(CantAPedir < 0, 0, CantAPedir) ' Si la cantidad a pedir es inferior a 0 => 0
                    CantAPedir = IIf(CantAPedir - Math.Truncate(CantAPedir) >= 0.5, Math.Ceiling(CantAPedir), Math.Floor(CantAPedir)) ' Si es superior al 50% pido para arriba
                    ' Se pide por unidad debe ser multiplo de las unidades x bulto
                    CantAPedir = IIf(CInt(rs("UnidadMedidaId2").Valor) = 0 And CantAPedir Mod CInt(UnidadesPorBulto) <> 0 And rs("Color").Valor = "",
                                     Math.Ceiling(CantAPedir / CInt(UnidadesPorBulto)) * CInt(UnidadesPorBulto),
                                     CantAPedir)

                    ListaRegistros.Add(New ListProducto(rs("ClasificacionProdId").Valor, ProductoId, rs("Descripcion").Valor, Format(VtaProyectada, "#0.00000"), Format(StockTotal, "#0.00000"), Format(VtaTotal, "#0.00000"), UniPedido, CantAPedir, CantAPedir, KgPromedioUnid, UnidadesPorBulto, PrecioCompra, SeVendePorPeso, ColorProducto, SePidePorBulto, rs("TipoUnidadMedidaId2").Valor))
                    rs.MoveNext()
                Loop

                respuesta.ConsultaExitosa = True
                respuesta.rs = ListaRegistros
            Else
                respuesta.mensaje = "El proveedor no tiene articulos asociados"
            End If
            rs.Cerrar()
        Catch ex As Exception
            respuesta.mensaje = "Error BD consultando los productos a pedir " + vbCrLf + JsonConvert.SerializeObject(ex)
        Finally
            conn.Execute("IF OBJECT_ID('tempdb..#tmpConfigPedido') IS NOT NULL DROP TABLE #tmpConfigPedido")
            conn.Execute("IF OBJECT_ID('tempdb..#tmpProducido') IS NOT NULL DROP TABLE #tmpProducido")
            conn.Execute("IF OBJECT_ID('tempdb..#tmpPedido') IS NOT NULL DROP TABLE #tmpPedido")
        End Try

        Return respuesta
    End Function
    Public Function GetAgrupacionesProductos(ByVal NombreClasificacionAgrupaproductos As String) As ResponseAgrupaciones
        Dim respuesta As New ResponseAgrupaciones

        Try

            rs.Source = "SELECT ch.ClasificacionProdId, ch.Descripcion FROM ClasificacionesProductos cp
  Join RelArbolClasifProductos ra On ra.ClasifProdIdPadre = cp.ClasificacionProdId
  Join ClasificacionesProductos ch ON ch.ClasificacionProdId = ra.ClasifProdIdHijo
  WHERE cp.Descripcion = '" + NombreClasificacionAgrupaproductos + "'"

            Query.Add(rs.Source)
            rs.Abrir()
            If Not rs.EOF Then
                Dim ListaRegistros As List(Of Agrupacion) = New List(Of Agrupacion)
                Do While Not rs.EOF
                    ListaRegistros.Add(New Agrupacion(rs("ClasificacionProdId").Valor, rs("Descripcion").Valor))
                    rs.MoveNext()
                Loop

                respuesta.ConsultaExitosa = True
                respuesta.rs = ListaRegistros
            Else
                respuesta.mensaje = "No existen agrupaciones de productos en el sistema"
            End If
            rs.Cerrar()
        Catch ex As Exception
            respuesta.mensaje = "Error BD consultando los depositos del usuario"
        End Try

        Return respuesta
    End Function

    Public Function GetProductosAgrupaciones(ListaAgrupaciones As List(Of Agrupacion), ListaProveedores As List(Of Clasificacion)) As ResponseProductosAgrupaciones
        Dim respuesta As New ResponseProductosAgrupaciones
        Dim SqlAgrupaciones As String = ""
        Dim SqlProveedores As String = ""
        Try

            If ListaAgrupaciones.Count > 0 Then
                For Each Agrupacion As Agrupacion In ListaAgrupaciones
                    SqlAgrupaciones += Agrupacion.AgrupacionId + ", "
                Next
                ' Elimino el ultimo ", "
                SqlAgrupaciones = SqlAgrupaciones.Substring(0, SqlAgrupaciones.Length - 2)
            Else
                SqlAgrupaciones = "-9999" 'asigno un agrupacion que no exista para que el query no de error y para que no traiga registros
            End If

            If ListaProveedores.Count > 0 Then
                For Each Clasi As Clasificacion In ListaProveedores
                    SqlProveedores += Clasi.ClasificacionProdId + ", "
                Next
                ' Elimino el ultimo ", "
                SqlProveedores = SqlProveedores.Substring(0, SqlProveedores.Length - 2)
            Else
                SqlProveedores = "-9999" 'asigno un agrupacion que no exista para que el query no de error y para que no traiga registros
            End If


            rs.Source = "SELECT REL1.ProductoId, REL1.ClasificacionProdId 
FROM RelProductosClasificacionesProductos REL1  
INNER JOIN RelProductosClasificacionesProductos REL2 on REL1.ProductoId = REL2.ProductoId 
where REL1.ClasificacionProdId in (" + SqlAgrupaciones + ") and REL2.ClasificacionProdId in (" + SqlProveedores + ")"

            Query.Add(rs.Source)
            rs.Abrir()
            If Not rs.EOF Then
                Dim ListaRegistros As List(Of ProductoAgrupacion) = New List(Of ProductoAgrupacion)
                Do While Not rs.EOF
                    ListaRegistros.Add(New ProductoAgrupacion(rs("ClasificacionProdId").Valor, rs("ProductoId").Valor))
                    rs.MoveNext()
                Loop

                respuesta.ConsultaExitosa = True
                respuesta.rs = ListaRegistros
            Else
                respuesta.mensaje = "No existen agrupaciones de productos en el sistema"
            End If
            rs.Cerrar()
        Catch ex As Exception
            respuesta.mensaje = "Error BD consultando los depositos del usuario"
        End Try

        Return respuesta
    End Function
    Public Function GetClasificacionesClientes() As ResponseClasificacionesClientes
        Dim respuesta As New ResponseClasificacionesClientes
        Dim ListaIds As String = ""
        Dim ListaRegistros As List(Of String) = New List(Of String)
        Try

            rs.Source = "select Valor  FROM [ENSEMBLE].[dbo].[GWRConfiguracion] where Parametro = 'TiposContactoClieIdPed'"
            Query.Add(rs.Source)
            rs.Abrir()
            If Not rs.EOF Then
                Do While Not rs.EOF
                    ListaIds = rs("Valor").Valor
                    rs.MoveNext()
                Loop
            Else
                respuesta.mensaje = "No existen agrupaciones de productos en el sistema"
            End If
            rs.Cerrar()

            If ListaIds <> "" Then
                rs.Source = "select TipoContactoClienteId, Descripcion from TiposContactosClientes where TipoContactoClienteId in (" + ListaIds + ")"
                Query.Add(rs.Source)
                rs.Abrir()
                If Not rs.EOF Then
                    Do While Not rs.EOF
                        ListaRegistros.Add(rs("Descripcion").Valor.ToString().ToLower())
                        rs.MoveNext()
                    Loop
                    respuesta.ConsultaExitosa = True
                    respuesta.rs = ListaRegistros
                Else
                    respuesta.mensaje = "No existen Clientes asociados en el sistema"
                End If
                rs.Cerrar()

            End If
        Catch ex As Exception
            respuesta.mensaje = "Error BD consultando los contactos clientes del usuario"
        End Try
        Return respuesta
    End Function

    Public Function GetProductosClasificacion(ListaClasificaciones As List(Of String)) As ResponseProductosClasificacion
        Dim respuesta As New ResponseProductosClasificacion
        Dim SqlClasificaciones As String = ""
        Try

            If ListaClasificaciones.Count > 0 Then
                For Each Clasi As String In ListaClasificaciones
                    SqlClasificaciones += "'" + Clasi + "', "
                Next
                ' Elimino el ultimo ", "
                SqlClasificaciones = SqlClasificaciones.Substring(0, SqlClasificaciones.Length - 2)
            Else
                SqlClasificaciones = "-9999" 'asigno un agrupacion que no exista para que el query no de error y para que no traiga registros
            End If


            rs.Source = "SELECT RPCP.ProductoId, CP.ClasificacionProdId, CP.Descripcion  FROM
    ClasificacionesProductos CP 
    INNER JOIN RelProductosClasificacionesProductos RPCP on RPCP.ClasificacionProdId = CP.ClasificacionProdId
    WHERE CP.Descripcion in (" + SqlClasificaciones + ")"

            Query.Add(rs.Source)
            rs.Abrir()
            If Not rs.EOF Then
                Dim ListaRegistros As New List(Of ProductoClasificacion)
                Do While Not rs.EOF
                    ListaRegistros.Add(New ProductoClasificacion With {.ClasificacionId = rs("ClasificacionProdId").Valor, .ClasificacionNombre = rs("Descripcion").Valor, .ProductoId = rs("ProductoId").Valor})
                    rs.MoveNext()
                Loop

                respuesta.ConsultaExitosa = True
                respuesta.rs = ListaRegistros
            Else
                respuesta.mensaje = "No existen productos para la casificacion ingresada"
            End If
            rs.Cerrar()
        Catch ex As Exception
            respuesta.mensaje = "Error BD consultando los depositos del usuario"
        End Try

        Return respuesta
    End Function

    Public Function GetImporteVencidoCtrlCtaCte(ByVal EstablecimientoId As String) As Double
        ' Esta funcion sola la utilizan los locales que NO son propios
        Dim vencido As Double = 0

        Try
            ' Inserto la cabecera de los documentos a sincronizar    
            rs.Source = "SELECT TOP 1 e.EstablecimientoId,e.Descripcion, c.ClienteId, c.RazonSocial, ISNULL(vencido.Total, 0) Vencido FROM [TEMPONUBE.SOFTWARETEMPO.COM\SQLEXPRESS,4896].ENSEMBLE.dbo.GWREstablecimientos e
                JOIN [TEMPONUBE.SOFTWARETEMPO.COM\SQLEXPRESS,4896].EMP001.dbo.RelClientesContactos rcc ON rcc.Celular = e.EstablecimientoId
                JOIN [TEMPONUBE.SOFTWARETEMPO.COM\SQLEXPRESS,4896].EMP001.dbo.Clientes c ON c.ClienteId = rcc.ClienteId
                JOIN [TEMPONUBE.SOFTWARETEMPO.COM\SQLEXPRESS,4896].EMP001.dbo.PagosCondicionesVenta pcv ON pcv.CondicionVentaId = c.CondicionVentaId
                JOIN ENSEMBLE.dbo.Establecimientos ep ON ep.EstablecimientoId = e.EstablecimientoId
                LEFT JOIN (SELECT dc.ClienteId, SUM(MontoAplicar) Total FROM [TEMPONUBE.SOFTWARETEMPO.COM\SQLEXPRESS,4896].EMP001.dbo.DocumentosCabecera dc
                        JOIN [TEMPONUBE.SOFTWARETEMPO.COM\SQLEXPRESS,4896].EMP001.dbo.Clientes c ON c.ClienteId = dc.ClienteId        
                        JOIN [TEMPONUBE.SOFTWARETEMPO.COM\SQLEXPRESS,4896].EMP001.dbo.PagosCondicionesVenta pcv ON pcv.CondicionVentaId = c.CondicionVentaId
                        WHERE (dc.TipoDocumentoId LIKE 'FC%' OR dc.TipoDocumentoId LIKE 'ND%') AND dc.EstadoDocumentoId = 1 
		                AND CONVERT(DATE, dc.Fecha) < CONVERT(DATE, DATEADD(d, -1 * pcv.Dias, GETDATE()))
		                GROUP BY dc.ClienteId) vencido ON vencido.ClienteId = c.ClienteId
                WHERE rcc.TipoContactoClienteId = 6 AND e.Alta = 1 AND e.Propio = 0 AND ep.EstablecimientoId = " + EstablecimientoId + " 
                ORDER BY vencido.Total DESC"

            Query.Add(rs.Source)
            rs.Abrir()
            If Not rs.EOF Then vencido = Val(rs("Vencido").Valor)
            rs.Cerrar()
        Catch ex As Exception
            Try
                ' Si da error es porque es el otro Linked Server
                rs.Source = "SELECT TOP 1 e.EstablecimientoId,e.Descripcion, c.ClienteId, c.RazonSocial, ISNULL(vencido.Total, 0) Vencido FROM [50.1.2.2\SQLEXPRESS].ENSEMBLE.dbo.GWREstablecimientos e
                    JOIN [50.1.2.2\SQLEXPRESS].EMP001.dbo.RelClientesContactos rcc ON rcc.Celular = e.EstablecimientoId
                    JOIN [50.1.2.2\SQLEXPRESS].EMP001.dbo.Clientes c ON c.ClienteId = rcc.ClienteId
                    JOIN [50.1.2.2\SQLEXPRESS].EMP001.dbo.PagosCondicionesVenta pcv ON pcv.CondicionVentaId = c.CondicionVentaId
                    JOIN ENSEMBLE.dbo.Establecimientos ep ON ep.EstablecimientoId = e.EstablecimientoId
                    LEFT JOIN (SELECT dc.ClienteId, SUM(MontoAplicar) Total FROM [50.1.2.2\SQLEXPRESS].EMP001.dbo.DocumentosCabecera dc
                            JOIN [50.1.2.2\SQLEXPRESS].EMP001.dbo.Clientes c ON c.ClienteId = dc.ClienteId        
                            JOIN [50.1.2.2\SQLEXPRESS].EMP001.dbo.PagosCondicionesVenta pcv ON pcv.CondicionVentaId = c.CondicionVentaId
                            WHERE (dc.TipoDocumentoId LIKE 'FC%' OR dc.TipoDocumentoId LIKE 'ND%') AND dc.EstadoDocumentoId = 1 
		                    AND CONVERT(DATE, dc.Fecha) < CONVERT(DATE, DATEADD(d, -1 * pcv.Dias, GETDATE()))
		                    GROUP BY dc.ClienteId) vencido ON vencido.ClienteId = c.ClienteId
                    WHERE rcc.TipoContactoClienteId = 6 AND e.Alta = 1 AND e.Propio = 0 AND ep.EstablecimientoId = " + EstablecimientoId + " 
                    ORDER BY vencido.Total DESC"

                rs.Abrir()
                If Not rs.EOF Then vencido = Val(rs("Vencido").Valor)
                rs.Cerrar()
            Catch ex2 As Exception
                ' Si es error es por otra cosa no por el Linked Server
                MsgBox("Error GetImporteVencidoCtrlCtaCte: " & ex2.Message, vbCritical, "Luz Azul Pedidos")
            End Try
        End Try

        Return vencido
    End Function
End Class
