Public Class LuzAzulConfiguracion
    Inherits LuzAzulCommon

    Public Class CierreMensual
        Property CierreIvaPrueba As New List(Of AsientoCierre)
        Property CierreIva As New List(Of AsientoCierre)
        Property IIBB As New List(Of AsientoCierre)

    End Class

    Public Class AsientoCierre
        Property NroAsiento As String
        Property EstablecimientoNombre As String
        Property CuentaId As String
        Property SubCuentaId As String
        Property SubCuentaDescripcion As String
        Property Concepto As String
        Property Partida As String
        Property Importe As String
    End Class
    Public Class StockDeposito
        Property ProductoId As String
        Property ProductoNombre As String
        Property ClasificacionId As String
        Property TipoUnidadId As String
        Property Unidad As String
        Property Stock As String
        Property CantidadContada As String
        Property Diferencia As String
        Property Precio As String
        Property Subtotal As String
    End Class
    Public Class TipoPromo
        Property TipoPromoId As Integer
        Property Descripcion As String
        Property Margen As Decimal
        Property ClasificacionProdId As String
        Property ProductoIdNotIn As String
        Property ProductoIdIn As String
        Property FormulaAnidada As Integer
        Property EsPromoBancoFrances As Boolean

    End Class

    Public Class ListPromo
        Property FormulaId As String
        Property ProductoId As String
        Property Descripcion As String
        Property PrecioActualConIva As Decimal
        Property PrecioNuevoSinIva As Decimal
        Property PrecioNuevoConIva As Decimal
        Property PorcentajeVariacion As Decimal
        Property UnidadesPorKilo As Decimal
        Property CostoNuevoSinIva As Decimal

    End Class
    Public Class responseAsientoCierre
        Inherits ResponseWS
        Property Items As New List(Of AsientoCierre)
    End Class

    Public Class responseStockDeposito
        Inherits ResponseWS
        Property Items As New List(Of StockDeposito)
    End Class

    Public Class responseListPromo
        Inherits ResponseWS
        Property Items As New List(Of ListPromo)
        Property ItemsAnidados As New List(Of ListPromo)
    End Class
    Public Function GetPromo(ByVal Promo As TipoPromo) As responseListPromo

        Dim respuesta As New responseListPromo
        Dim sqlQuery As String
        Dim pos As Integer

        Try
            sqlQuery = "  SELECT f.FormulaId, fs.ProductoId, f.Descripcion, rp.Precio PrecioVentaSinIva, fe.Cantidad, r.Precio CostoSinIva, fs.Cantidad as CantidadProducida " + vbCrLf
            sqlQuery += "FROM " + NombreBasePrecios + ".dbo.FormulasProductosEnt fe " + vbCrLf
            sqlQuery += "JOIN " + NombreBasePrecios + ".dbo.Formulas f ON f.FormulaId=fe.FormulaId " + vbCrLf
            sqlQuery += "JOIN " + NombreBasePrecios + ".dbo.FormulasProductosSal fs ON fs.FormulaId=f.FormulaId " + vbCrLf

            If Promo.ClasificacionProdId <> "" Then
                sqlQuery += "JOIN " + NombreBasePrecios + ".dbo.RelProductosClasificacionesProductos rpc ON rpc.ProductoId=fs.ProductoId " + vbCrLf
            End If

            sqlQuery += "JOIN " + NombreBasePrecios + ".dbo.RelProductosListasPrecios r ON r.ProductoId=fe.ProductoId " + vbCrLf

            If Promo.ClasificacionProdId <> "" Then
                sqlQuery += "JOIN (SELECT r.ProductoId, r.Precio FROM " + NombreBasePrecios + ".dbo.RelProductosListasPrecios r
            	JOIN " + NombreBasePrecios + ".dbo.RelProductosClasificacionesProductos rpc ON rpc.ProductoId=r.ProductoId
            	WHERE r.ListaPrecioId=1 AND rpc.ClasificacionProdId=" + Promo.ClasificacionProdId + ") rp ON rp.ProductoId=fs.ProductoId " + vbCrLf
            Else
                sqlQuery += "JOIN (SELECT r.ProductoId, r.Precio FROM EMP008..RelProductosListasPrecios r	
                WHERE r.ListaPrecioId = 1 And r.ProductoId in (" + Promo.ProductoIdIn + ") ) rp ON rp.ProductoId=fs.ProductoId " + vbCrLf
            End If

            sqlQuery += " WHERE r.ListaPrecioId=8 " 'Para el analisis de las formulas siempre tomo como referencia el precio de franquicia lista 8

            If Promo.ClasificacionProdId <> "" Then
                sqlQuery += " AND rpc.ClasificacionProdId=" + Promo.ClasificacionProdId + " "
            End If
            If Promo.ProductoIdIn <> "" Then
                sqlQuery += " AND fs.ProductoId IN (" + Promo.ProductoIdIn + ")"
            End If
            If Promo.ProductoIdNotIn <> "" Then
                sqlQuery += " AND fs.ProductoId NOT IN (" + Promo.ProductoIdNotIn + ")"
            End If

            sqlQuery += " ORDER BY fs.ProductoId "

            rs.Source = sqlQuery
            Query.Add(rs.Source)
            rs.Abrir()
            If Not rs.EOF Then
                Dim ListaRegistros As List(Of ListPromo) = New List(Of ListPromo)
                Dim IndiceIva As Decimal = 1.21
                Do While Not rs.EOF
                    Dim registro As New ListPromo
                    registro.FormulaId = rs("FormulaId").Valor
                    registro.ProductoId = rs("ProductoId").Valor
                    registro.Descripcion = rs("Descripcion").Valor

                    Dim PrecioVentaSinIva As Decimal = IIf(Decimal.TryParse(rs("PrecioVentaSinIva").Valor, PrecioVentaSinIva), PrecioVentaSinIva, 0)
                    registro.PrecioActualConIva = Decimal.Round(CDec(PrecioVentaSinIva * IndiceIva), 2)

                    Dim Cantidad As Decimal = IIf(Decimal.TryParse(rs("Cantidad").Valor, Cantidad), Cantidad, 0)
                    Dim CostoSinIva As Decimal = IIf(Decimal.TryParse(rs("CostoSinIva").Valor, CostoSinIva), CostoSinIva, 0)
                    Dim CantidadProducida As Decimal = IIf(Decimal.TryParse(rs("CantidadProducida").Valor, CantidadProducida), CantidadProducida, 0)
                    'Si la formula de salida devuelve una unidad entera es porque es producto se vende por unidad, si devuelve una franccion es porque se vende por kilos y tengo que multiplicar el precio por este factor
                    Dim Factor As Decimal = 1 / CantidadProducida

                    registro.UnidadesPorKilo = Factor
                    registro.CostoNuevoSinIva = Cantidad * CostoSinIva * Factor
                    registro.PrecioNuevoSinIva = registro.CostoNuevoSinIva * Promo.Margen

                    'Como puedo tener una linea por cada productos de la formula las tengo que agrupar
                    pos = ListaRegistros.FindIndex(Function(element) element.FormulaId = registro.FormulaId)
                    If pos >= 0 Then
                        ListaRegistros(pos).PrecioNuevoSinIva += registro.PrecioNuevoSinIva
                        ListaRegistros(pos).CostoNuevoSinIva += registro.CostoNuevoSinIva
                    Else
                        ListaRegistros.Add(registro)
                    End If
                    rs.MoveNext()
                Loop

                'Luego de obtener el costo de todos los productos calculo el precio redondeado 
                For Each registro As ListPromo In ListaRegistros

                    registro.PrecioNuevoConIva = Decimal.Round(registro.PrecioNuevoSinIva * IndiceIva)

                    'EL precio nuevo sin iva lo saco despues de que redondeo el precio final con iva que es el que va a ver el cliente
                    registro.PrecioNuevoSinIva = registro.PrecioNuevoConIva / IndiceIva
                    registro.PorcentajeVariacion = Decimal.Round(1 - (registro.PrecioActualConIva / registro.PrecioNuevoConIva), 2) * 100
                Next

                respuesta.Items = ListaRegistros
                respuesta.ConsultaExitosa = True
            Else
                respuesta.ConsultaExitosa = True
                respuesta.mensaje = "No existen promociones en la tabla de formulas con esos datos"
            End If
            rs.Cerrar()
        Catch ex As Exception
            respuesta.mensaje = "Error BD al consultar la tabla de formulas del sistema "
        End Try

        Return respuesta
    End Function

    Public Function UpdatePrecioPromo(ByVal ProductoId As String, ByVal ListaPrecioIds As String, ByVal Precio As Decimal, ByVal EsListaFranquicia As Boolean) As ResponseWS
        Dim respuesta As New ResponseWS
        Dim sqlQuery As String

        Try
            sqlQuery = "UPDATE " + NombreBasePrecios + ".dbo.RelProductosListasPrecios SET Precio = " + Precio.ToString() + "  where ProductoId = '" + ProductoId + "' "

            If EsListaFranquicia Then
                sqlQuery += " and ListaPrecioId in ( " + ListaPrecioIds + " ) "
            Else
                sqlQuery += " and ListaPrecioId not in ( " + ListaPrecioIds + " ) "
            End If

            conn.Execute(sqlQuery)

            respuesta.ConsultaExitosa = True
        Catch ex As Exception
            respuesta.mensaje = "Error BD al actualizar el precio del producto"
        End Try

        Return respuesta
    End Function

    Public Function UpdatePromosBancoFrances(ByVal ProductoIdIn As String)

        Dim respuesta As New ResponseWS
        Dim sqlQuery As String

        Try
            If ProductoIdIn = "" Then
                ProductoIdIn = "'-1'" 'Pongo esta condicion para que el update no de error pero no actualice ningun producto
            End If

            sqlQuery = "UPDATE rpd
SET rpd.Precio = rp.Precio / 0.85
From " + NombreBasePrecios + "..RelProductosListasPrecios rp
Join " + NombreBasePrecios + "..RelProductosListasPrecios rpd ON rpd.ProductoId = rp.ProductoId
Where rp.ListaPrecioId = 1 And rpd.ListaPrecioId = 11
And rp.ProductoId IN (" + ProductoIdIn + ") "

            conn.Execute(sqlQuery)

            respuesta.ConsultaExitosa = True
        Catch ex As Exception
            respuesta.mensaje = "Error BD al actualizar el precio del producto"
        End Try

        Return respuesta
    End Function

    Public Function GetStockProductos(ByVal ListDepositos As String, ByVal ListaPreciosId As String, ByVal listClasificaciones As String) As responseStockDeposito
        Dim respuesta As New responseStockDeposito
        Dim sqlQuery As String

        Try
            sqlQuery = "SELECT p.ProductoId, p.Descripcion, SUM( s.StockActual) as stock, p.TipoUnidadMedidaId2, UM.Descripcion as UnidadMedida, LP.Precio"

            If listClasificaciones <> "" Then
                sqlQuery += ", CP.ClasificacionProdId "
            Else
                sqlQuery += ", '' as  ClasificacionProdId"
            End If

            sqlQuery += "
                    FROM Productos p
                    LEFT JOIN TipoUnidadesMedida UM on UM.TipoUnidadMedidaId = p.TipoUnidadMedidaId2
                    LEFT JOIN RelProductosListasPrecios LP on LP.ListaPrecioId = " + ListaPreciosId + " and LP.ProductoId = p.ProductoId
					LEFT JOIN Stock s ON s.productoId = p.productoId "

            If listClasificaciones <> "" Then
                sqlQuery += "LEFT JOIN RelProductosClasificacionesProductos CP on CP.ProductoId = p.ProductoId and CP.ClasificacionProdId in (" + listClasificaciones + ")"
            End If

            sqlQuery += " where p.Inactivo = 0 "

            If (ListDepositos <> "") Then
                sqlQuery += " and s.depositoId in (" + ListDepositos + ") "
            End If

            sqlQuery += " Group by  p.ProductoId, p.Descripcion, p.TipoUnidadMedidaId2, UM.Descripcion, LP.Precio"
            If listClasificaciones <> "" Then
                sqlQuery += ", ClasificacionProdId"
            End If

            rs.Source = sqlQuery
            Query.Add(rs.Source)
            rs.Abrir()
            If Not rs.EOF Then
                Dim ListaRegistros As List(Of StockDeposito) = New List(Of StockDeposito)
                Do While Not rs.EOF
                    Dim Registro As New StockDeposito
                    Registro.ProductoId = rs("ProductoId").Valor
                    Registro.ProductoNombre = rs("Descripcion").Valor
                    Registro.TipoUnidadId = rs("TipoUnidadMedidaId2").Valor
                    Registro.Unidad = rs("UnidadMedida").Valor
                    Registro.ClasificacionId = rs("ClasificacionProdId").Valor.ToString()
                    Dim stock As Double = IIf(Double.TryParse(rs("stock").Valor.ToString(), stock), stock, 0)
                    Registro.Stock = Format(stock, "#0.000")
                    Dim precioFRQ As Double = IIf(Double.TryParse(rs("Precio").Valor.ToString(), precioFRQ), precioFRQ, 0)
                    Registro.Precio = Format(precioFRQ, "#0.00")
                    ListaRegistros.Add(Registro)
                    rs.MoveNext()
                Loop

                respuesta.Items = ListaRegistros
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

    Public Function UpdateStockProducto(ByVal DepositoId As String, ByVal ProductoId As String, ByVal StockAnterior As Decimal, StockActual As Decimal, ByVal TipoMovimiento As String, ByVal Concepto As String) As ResponseWS

        Dim respuesta As New ResponseWS
        Dim sqlQuery As String


        Try
            Dim cantidad As Decimal = StockActual - StockAnterior
            If cantidad < 0 Then
                'si es negativa la multiplico por -1 porque necesito solo la diferencia
                cantidad *= -1
            End If
            'Si la cantidad es cero no hay movimiento de stock
            If cantidad <> 0 Then

                'UPDATE EMP008.dbo.Stock set StockActual = 543.13002 where DepositoId = 0 and ProductoId = '009'
                If ProductoId = "" Then
                    ProductoId = "'-1'" 'Pongo esta condicion para que el update no de error pero no actualice ningun producto
                End If

                sqlQuery = "UPDATE " + NombreBase + ".dbo.Stock SET StockActual = " + StockActual.ToString() + " WHERE DepositoId = " + DepositoId + " And ProductoId = '" + ProductoId + "' "
                conn.Execute(sqlQuery)

                'INSERT INTO dbo.MovimientosStock (TipoMovStockId,Fecha,Descripcion,DepositoId,ProductoId,StockAnterior,Cantidad,SaldoStock,DespachoId)
                ' VALUES(1,'2023-01-01 15:00','Ajuste por Merma',0,'009',50,3,53,null)
                sqlQuery = "INSERT INTO dbo.MovimientosStock (TipoMovStockId,Fecha,Descripcion,DepositoId,ProductoId,StockAnterior,Cantidad,SaldoStock,DespachoId) "
                sqlQuery += "VALUES (" + TipoMovimiento + ", '" + Date.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', '" + Concepto + "', " + DepositoId + ", '" + ProductoId + "', "
                sqlQuery += StockAnterior.ToString() + ", " + cantidad.ToString() + ", " + StockActual.ToString() + ", null)"

                conn.Execute(sqlQuery)

            End If

            respuesta.ConsultaExitosa = True
        Catch ex As Exception
            respuesta.mensaje = "Error BD al actualizar el inventario del producto"
        End Try
        Return respuesta
    End Function
    Public Function GenerarAsientoContableMerma(ByVal EjercicioId As String, ByVal NroAsiento As String, ByVal SucursalId As String, ByVal Importe As Decimal, ByVal Configuracion As ConfigAsiento) As ResponseWS
        Dim respuesta As New ResponseWS
        Dim sqlQuery As String

        Try
            'Obtengo la configuracion de las cuentas
            Dim IdCuentaDebe As String = Configuracion.CuentaIdDebe
            Dim IdSubCuentaDebe As String = Configuracion.SubCuentaIdDebe
            Dim IdCuentaHaber As String = Configuracion.CuentaIdHaber
            Dim IdSubCuentaHaber As String = Configuracion.SubCuentaIdHaber
            Dim Concepto As String = Configuracion.ConceptoAsiento
            Dim tipoAsiento As String = Configuracion.TipoAsiento

            Dim InvertirHaber As String = IIf(Importe > 0, "H", "D")
            Dim InvertirDebe As String = IIf(Importe > 0, "D", "H")

            If IdCuentaDebe <> "" And IdCuentaHaber <> "" And IdSubCuentaDebe <> "" And IdSubCuentaHaber <> "" And Concepto <> "" Then

                'INSERT INTO Asientos VALUES(@EjercicioId, @NroAsiento, GETDATE(), 8, @CtaProdPropios, @SubCtaProdPropios, 'D', ABS(@ImporteLinea), @Concepto, 'PRE', 0, NULL)

                sqlQuery = "INSERT INTO Asientos
			VALUES (" + EjercicioId + ", " + NroAsiento + ", GETDATE(), " + SucursalId + ", '" + IdCuentaHaber + "', '" + IdSubCuentaHaber + "', 
					'" + InvertirHaber + "', ABS(" + Importe.ToString() + "), '" + Concepto + "', '" + tipoAsiento + "', 0, NULL)"
                conn.Execute(sqlQuery)

                sqlQuery = "INSERT INTO Asientos
			VALUES (" + EjercicioId + ", " + NroAsiento + ", GETDATE(), " + SucursalId + ", '" + IdCuentaDebe + "', '" + IdSubCuentaDebe + "', 
					'" + InvertirDebe + "', ABS(" + Importe.ToString() + "), '" + Concepto + "', '" + tipoAsiento + "', 0, NULL)"
                conn.Execute(sqlQuery)

                respuesta.ConsultaExitosa = True
            Else
                respuesta.mensaje = "Los parametros de configuracion de asientos no son correctos"
            End If
        Catch ex As Exception
            respuesta.mensaje = "Error BD al actualizar el inventario del producto"
        End Try
        Return respuesta
    End Function
    Function GetCantAsientosCierreIvaPrueba(ByVal Mes As String, ByVal Anio As String, ByVal EstablecimientoId As String) As ResponseWS

        Dim sqlQuery As String

        sqlQuery = "SELECT COUNT(*) as Cantidad FROM Asientos a 
	JOIN Cuentas c ON c.CuentaId = a.CuentaId
	JOIN ENSEMBLE..RelEstablecimientosSucursales res ON res.SucursalId = a.SucursalId	
	WHERE c.Descripcion LIKE '%IVA%' AND c.Descripcion LIKE '%Prueba%' AND c.Descripcion LIKE '%Ingreso%'
	AND a.Concepto LIKE 'IVA%' 
    AND a.Concepto LIKE '%Prueba%'
	AND a.Concepto LIKE '%" + Mes + "/%' 
	AND a.Concepto LIKE '%" + Anio + "%'
    AND res.EstablecimientoId = " + EstablecimientoId + " "

        Return GetCantAsientos(sqlQuery)
    End Function

    Function GetCantAsientosCierreIva(ByVal Mes As String, ByVal Anio As String, ByVal EstablecimientoId As String) As ResponseWS

        Dim sqlQuery As String

        sqlQuery = "SELECT COUNT(*) as Cantidad FROM Asientos a 
	JOIN SubCuentas s ON s.CuentaId = a.CuentaId AND s.SubCuentaId = a.SubCuentaId
	JOIN ENSEMBLE..RelEstablecimientosSucursales res ON res.SucursalId = a.SucursalId	
	WHERE s.Descripcion LIKE '%IVA%' AND s.Descripcion LIKE '%Debito%' AND s.Descripcion LIKE '%21%'
	AND a.Concepto LIKE 'IVA%' 
    AND a.Concepto NOT LIKE '%Prueba%'
	AND a.Concepto LIKE '%" + Mes + "/%' 
	AND a.Concepto LIKE '%" + Anio + "%'
    AND res.EstablecimientoId = " + EstablecimientoId + " "

        Return GetCantAsientos(sqlQuery)
    End Function

    Function GetCantAsientos(ByVal sqlQuery As String) As ResponseWS
        Dim respuesta As New ResponseWS

        Try
            rs.Source = sqlQuery
            Query.Add(rs.Source)
            rs.Abrir()
            If Not rs.EOF Then
                Do While Not rs.EOF
                    respuesta.mensaje = rs("Cantidad").Valor
                    rs.MoveNext()
                Loop
                respuesta.ConsultaExitosa = True
            Else
                respuesta.mensaje = "No existen asientos para la fecha indicada en la base de tempo"
            End If
        Catch ex As Exception
            respuesta.mensaje = "Error BD al consultar los asientos de la base de tempo " + vbCrLf + ex.Message
        Finally
            rs.Cerrar()
        End Try
        Return respuesta
    End Function
    Function GetAsientosIIBB(ByVal Mes As String, ByVal Anio As String, ByVal EstablecimientoId As String) As responseAsientoCierre
        Dim sqlQuery As String

        sqlQuery = "SELECT a.NroAsiento, e.Descripcion  as EstablecimientoNombre, a.CuentaId, a.SubCuentaId, a.Partida, a.Concepto, s.Descripcion as SubCuentaDescripcion, a.importe   
    FROM Asientos a 
	JOIN Cuentas c ON c.CuentaId = a.CuentaId
    JOIN SubCuentas s ON s.CuentaId = a.CuentaId AND s.SubCuentaId = a.SubCuentaId
	JOIN ENSEMBLE..RelEstablecimientosSucursales res ON res.SucursalId = a.SucursalId	
    JOIN ENSEMBLE..Establecimientos E ON E.EstablecimientoId = res.EstablecimientoId 
	WHERE a.Concepto LIKE 'IIBB%' 
	AND a.Concepto LIKE '%" + Mes + "/%' 
	AND a.Concepto LIKE '%" + Anio + "%'
    AND res.EstablecimientoId = " + EstablecimientoId + " "

        Return GetAsientosCierre(sqlQuery)

    End Function

    Function GetAsientosCierreIva(ByVal Mes As String, ByVal Anio As String, ByVal EstablecimientoId As String) As responseAsientoCierre
        Dim sqlQuery As String

        sqlQuery = "SELECT a.NroAsiento, e.Descripcion  as EstablecimientoNombre, a.CuentaId, a.SubCuentaId, a.Partida, a.Concepto, s.Descripcion as SubCuentaDescripcion, a.importe   
    FROM Asientos a 
	JOIN Cuentas c ON c.CuentaId = a.CuentaId
    JOIN SubCuentas s ON s.CuentaId = a.CuentaId AND s.SubCuentaId = a.SubCuentaId
	JOIN ENSEMBLE..RelEstablecimientosSucursales res ON res.SucursalId = a.SucursalId	
    JOIN ENSEMBLE..Establecimientos E ON E.EstablecimientoId = res.EstablecimientoId 
	WHERE a.Concepto LIKE 'IVA%' 
    AND a.Concepto NOT LIKE '%Prueba%'
	AND a.Concepto LIKE '%" + Mes + "/%' 
	AND a.Concepto LIKE '%" + Anio + "%'
    AND res.EstablecimientoId = " + EstablecimientoId + " "

        Return GetAsientosCierre(sqlQuery)

    End Function

    Function GetAsientosCierreIvaPrueba(ByVal Mes As String, ByVal Anio As String, ByVal EstablecimientoId As String) As responseAsientoCierre
        Dim sqlQuery As String

        sqlQuery = "SELECT a.NroAsiento, e.Descripcion  as EstablecimientoNombre, a.CuentaId, a.SubCuentaId, a.Partida, a.Concepto, s.Descripcion as SubCuentaDescripcion, a.importe  
    FROM Asientos a 
	JOIN Cuentas c ON c.CuentaId = a.CuentaId
    JOIN SubCuentas s ON s.CuentaId = a.CuentaId AND s.SubCuentaId = a.SubCuentaId
	JOIN ENSEMBLE..RelEstablecimientosSucursales res ON res.SucursalId = a.SucursalId	
    JOIN ENSEMBLE..Establecimientos E ON E.EstablecimientoId = res.EstablecimientoId 
	WHERE a.Concepto LIKE 'IVA%' 
    AND a.Concepto LIKE '%Prueba%'
	AND a.Concepto LIKE '%" + Mes + "/%' 
	AND a.Concepto LIKE '%" + Anio + "%'
    AND res.EstablecimientoId = " + EstablecimientoId + " "

        Return GetAsientosCierre(sqlQuery)

    End Function


    Function CreateAsientosCierreIvaPrueba(ByVal Mes As String, ByVal Anio As String, ByVal EstablecimientoId As String) As responseAsientoCierre
        Dim sqlQuery As String

        Dim prefijoMes = IIf(Mes.Length < 2, "0", "")
        Dim NombreAsiento As String = "IVA Prueba " + prefijoMes + Mes + "/" + Anio

        sqlQuery = "    
    SELECT '' as NroAsiento, '" + NombreAsiento + "' as Concepto,  e.Descripcion  as EstablecimientoNombre, c.CuentaId, s.SubCuentaId, s.Descripcion as SubCuentaDescripcion , 'H' as Partida, SUM(
	CASE WHEN a.Partida = 'H' THEN a.Importe * -1 ELSE a.Importe END) Importe
	FROM Asientos a
	JOIN Cuentas c ON c.CuentaId = a.CuentaId
	JOIN SubCuentas s ON c.CuentaId = s.CuentaId AND s.SubCuentaId = a.SubCuentaId
	JOIN ENSEMBLE..RelEstablecimientosSucursales res ON res.SucursalId = a.SucursalId
	JOIN ENSEMBLE..Establecimientos e ON e.EstablecimientoId = res.EstablecimientoId	
	WHERE MONTH(a.Fecha) = " + Mes + " AND YEAR(a.Fecha) = " + Anio + " 
    AND res.EstablecimientoId = " + EstablecimientoId + " 
	AND a.SucursalId LIKE '9___' AND c.Descripcion LIKE '%IVA%' AND c.Descripcion LIKE '%Credito%'
	GROUP BY e.Descripcion, c.CuentaId, s.SubCuentaId, s.Descripcion
    UNION
    SELECT '' as NroAsiento, '" + NombreAsiento + "' as Concepto , e.Descripcion as EstablecimientoNombre , c.CuentaId, s.SubCuentaId, s.Descripcion as SubCuentaDescripcion , 'D' as Partida , SUM(
	CASE WHEN a.Partida = 'D' THEN a.Importe * -1 ELSE a.Importe END) Importe
	FROM Asientos a
	JOIN Cuentas c ON c.CuentaId = a.CuentaId
	JOIN SubCuentas s ON c.CuentaId = s.CuentaId AND s.SubCuentaId = a.SubCuentaId
	JOIN ENSEMBLE..RelEstablecimientosSucursales res ON res.SucursalId = a.SucursalId
	JOIN ENSEMBLE..Establecimientos e ON e.EstablecimientoId = res.EstablecimientoId
	WHERE MONTH(a.Fecha) = " + Mes + " AND YEAR(a.Fecha) = " + Anio + " 
    AND res.EstablecimientoId = " + EstablecimientoId + " 
	AND a.SucursalId LIKE '9___' AND c.Descripcion LIKE '%IVA%' AND c.Descripcion LIKE '%Debito%'
	GROUP BY e.Descripcion, c.CuentaId, s.SubCuentaId, s.Descripcion
    UNION
    SELECT '' as NroAsiento, '" + NombreAsiento + "' as Concepto,  e.Descripcion  as EstablecimientoNombre, c.CuentaId, s.SubCuentaId, s.Descripcion as SubCuentaDescripcion , 'H' as Partida, SUM(
	CASE WHEN a.Partida = 'H' THEN a.Importe * -1 ELSE a.Importe END) Importe
	FROM Asientos a
	JOIN Cuentas c ON c.CuentaId = a.CuentaId
	JOIN SubCuentas s ON c.CuentaId = s.CuentaId AND s.SubCuentaId = a.SubCuentaId
	JOIN ENSEMBLE..RelEstablecimientosSucursales res ON res.SucursalId = a.SucursalId
	JOIN ENSEMBLE..Establecimientos e ON e.EstablecimientoId = res.EstablecimientoId
	WHERE MONTH(a.Fecha) = " + Mes + " AND YEAR(a.Fecha) = " + Anio + " 
    AND res.EstablecimientoId = " + EstablecimientoId + " 
	AND a.SucursalId LIKE '9___' AND c.Descripcion LIKE '%IVA%' AND c.Descripcion LIKE '%Percep%'
	GROUP BY e.Descripcion, c.CuentaId, s.SubCuentaId, s.Descripcion
    UNION
    SELECT '' as NroAsiento, '" + NombreAsiento + "' as Concepto,  e.Descripcion  as EstablecimientoNombre, c.CuentaId, s.SubCuentaId, s.Descripcion as SubCuentaDescripcion , 'H' as Partida, SUM(
	CASE WHEN a.Partida = 'H' THEN a.Importe * -1 ELSE a.Importe END) Importe
	FROM Asientos a
	JOIN Cuentas c ON c.CuentaId = a.CuentaId
	JOIN SubCuentas s ON c.CuentaId = s.CuentaId AND s.SubCuentaId = a.SubCuentaId
	JOIN ENSEMBLE..RelEstablecimientosSucursales res ON res.SucursalId = a.SucursalId
	JOIN ENSEMBLE..Establecimientos e ON e.EstablecimientoId = res.EstablecimientoId
	WHERE MONTH(a.Fecha) = " + Mes + " AND YEAR(a.Fecha) = " + Anio + " 
    AND res.EstablecimientoId = " + EstablecimientoId + " 
	AND a.SucursalId LIKE '9___' AND c.Descripcion LIKE '%IVA%' AND c.Descripcion LIKE '%Rete%'
	GROUP BY e.Descripcion, c.CuentaId, s.SubCuentaId, s.Descripcion
    UNION
    SELECT '' as NroAsiento, '" + NombreAsiento + "' as Concepto,  e.Descripcion  as EstablecimientoNombre, c.CuentaId, s.SubCuentaId, s.Descripcion as SubCuentaDescripcion , 'H' as Partida, SUM(
	CASE WHEN a.Partida = 'H' THEN a.Importe * -1 ELSE a.Importe END) Importe
	FROM Asientos a
	JOIN Cuentas c ON c.CuentaId = a.CuentaId
	JOIN SubCuentas s ON c.CuentaId = s.CuentaId AND s.SubCuentaId = a.SubCuentaId
	JOIN ENSEMBLE..RelEstablecimientosSucursales res ON res.SucursalId = a.SucursalId
	JOIN ENSEMBLE..Establecimientos e ON e.EstablecimientoId = res.EstablecimientoId
	WHERE a.Fecha <= DATEADD(day, -1, CONVERT(DATETIME, '" + Anio + "-" + Mes + "-01')) AND a.SucursalId LIKE '9___'
    AND res.EstablecimientoId = " + EstablecimientoId + " 
	AND c.Descripcion LIKE '%IVA%' AND c.Descripcion LIKE '%SAF%'
	GROUP BY e.Descripcion, c.CuentaId, s.SubCuentaId, s.Descripcion"

        Return GetAsientosCierre(sqlQuery)

    End Function

    Function CreateAsientosCierreIva(ByVal Mes As String, ByVal Anio As String, ByVal EstablecimientoId As String) As responseAsientoCierre
        Dim sqlQuery As String

        Dim prefijoMes = IIf(Mes.Length < 2, "0", "")
        Dim NombreAsiento As String = "IVA " + prefijoMes + Mes + "/" + Anio

        sqlQuery = "    
    SELECT '' as NroAsiento, '" + NombreAsiento + "' as Concepto,  e.Descripcion  as EstablecimientoNombre, c.CuentaId, s.SubCuentaId, s.Descripcion as SubCuentaDescripcion , 'H' as Partida, SUM(
	CASE WHEN a.Partida = 'H' THEN a.Importe * -1 ELSE a.Importe END) Importe
	FROM Asientos a
	JOIN Cuentas c ON c.CuentaId = a.CuentaId
	JOIN SubCuentas s ON c.CuentaId = s.CuentaId AND s.SubCuentaId = a.SubCuentaId
	JOIN ENSEMBLE..RelEstablecimientosSucursales res ON res.SucursalId = a.SucursalId
	JOIN ENSEMBLE..Establecimientos e ON e.EstablecimientoId = res.EstablecimientoId	
	WHERE MONTH(a.Fecha) = " + Mes + " AND YEAR(a.Fecha) = " + Anio + " 
    AND res.EstablecimientoId = " + EstablecimientoId + " 
	AND a.SucursalId NOT LIKE '9___' AND c.Descripcion LIKE '%IVA%' AND c.Descripcion LIKE '%Credito%'
	GROUP BY e.Descripcion, c.CuentaId, s.SubCuentaId, s.Descripcion
    UNION
    SELECT '' as NroAsiento, '" + NombreAsiento + "' as Concepto , e.Descripcion as EstablecimientoNombre , c.CuentaId, s.SubCuentaId, s.Descripcion as SubCuentaDescripcion , 'D' as Partida , SUM(
	CASE WHEN a.Partida = 'D' THEN a.Importe * -1 ELSE a.Importe END) Importe
	FROM Asientos a
	JOIN Cuentas c ON c.CuentaId = a.CuentaId
	JOIN SubCuentas s ON c.CuentaId = s.CuentaId AND s.SubCuentaId = a.SubCuentaId
	JOIN ENSEMBLE..RelEstablecimientosSucursales res ON res.SucursalId = a.SucursalId
	JOIN ENSEMBLE..Establecimientos e ON e.EstablecimientoId = res.EstablecimientoId
	WHERE MONTH(a.Fecha) = " + Mes + " AND YEAR(a.Fecha) = " + Anio + " 
    AND res.EstablecimientoId = " + EstablecimientoId + " 
	AND a.SucursalId NOT LIKE '9___' AND c.Descripcion LIKE '%IVA%' AND c.Descripcion LIKE '%Debito%'
	GROUP BY e.Descripcion, c.CuentaId, s.SubCuentaId, s.Descripcion
    UNION
    SELECT '' as NroAsiento, '" + NombreAsiento + "' as Concepto,  e.Descripcion  as EstablecimientoNombre, c.CuentaId, s.SubCuentaId, s.Descripcion as SubCuentaDescripcion , 'H' as Partida, SUM(
	CASE WHEN a.Partida = 'H' THEN a.Importe * -1 ELSE a.Importe END) Importe
	FROM Asientos a
	JOIN Cuentas c ON c.CuentaId = a.CuentaId
	JOIN SubCuentas s ON c.CuentaId = s.CuentaId AND s.SubCuentaId = a.SubCuentaId
	JOIN ENSEMBLE..RelEstablecimientosSucursales res ON res.SucursalId = a.SucursalId
	JOIN ENSEMBLE..Establecimientos e ON e.EstablecimientoId = res.EstablecimientoId
	WHERE MONTH(a.Fecha) = " + Mes + " AND YEAR(a.Fecha) = " + Anio + " 
    AND res.EstablecimientoId = " + EstablecimientoId + " 
	AND a.SucursalId NOT LIKE '9___' AND c.Descripcion LIKE '%IVA%' AND c.Descripcion LIKE '%Percep%'
	GROUP BY e.Descripcion, c.CuentaId, s.SubCuentaId, s.Descripcion
    UNION
    SELECT '' as NroAsiento, '" + NombreAsiento + "' as Concepto,  e.Descripcion  as EstablecimientoNombre, c.CuentaId, s.SubCuentaId, s.Descripcion as SubCuentaDescripcion , 'H' as Partida, SUM(
	CASE WHEN a.Partida = 'H' THEN a.Importe * -1 ELSE a.Importe END) Importe
	FROM Asientos a
	JOIN Cuentas c ON c.CuentaId = a.CuentaId
	JOIN SubCuentas s ON c.CuentaId = s.CuentaId AND s.SubCuentaId = a.SubCuentaId
	JOIN ENSEMBLE..RelEstablecimientosSucursales res ON res.SucursalId = a.SucursalId
	JOIN ENSEMBLE..Establecimientos e ON e.EstablecimientoId = res.EstablecimientoId
	WHERE MONTH(a.Fecha) = " + Mes + " AND YEAR(a.Fecha) = " + Anio + " 
    AND res.EstablecimientoId = " + EstablecimientoId + " 
	AND a.SucursalId NOT LIKE '9___' AND c.Descripcion LIKE '%IVA%' AND c.Descripcion LIKE '%Rete%'
	GROUP BY e.Descripcion, c.CuentaId, s.SubCuentaId, s.Descripcion
    UNION
    SELECT '' as NroAsiento, '" + NombreAsiento + "' as Concepto,  e.Descripcion  as EstablecimientoNombre, c.CuentaId, s.SubCuentaId, s.Descripcion as SubCuentaDescripcion , 'H' as Partida, SUM(
	CASE WHEN a.Partida = 'H' THEN a.Importe * -1 ELSE a.Importe END) Importe
	FROM Asientos a
	JOIN Cuentas c ON c.CuentaId = a.CuentaId
	JOIN SubCuentas s ON c.CuentaId = s.CuentaId AND s.SubCuentaId = a.SubCuentaId
	JOIN ENSEMBLE..RelEstablecimientosSucursales res ON res.SucursalId = a.SucursalId
	JOIN ENSEMBLE..Establecimientos e ON e.EstablecimientoId = res.EstablecimientoId
	WHERE a.Fecha <= DATEADD(day, -1, CONVERT(DATETIME, '" + Anio + "-" + Mes + "-01')) AND a.SucursalId NOT LIKE '9___'
    AND res.EstablecimientoId = " + EstablecimientoId + " 
	AND c.Descripcion LIKE '%IVA%' AND c.Descripcion LIKE '%SAF%'
	GROUP BY e.Descripcion, c.CuentaId, s.SubCuentaId, s.Descripcion"

        Return GetAsientosCierre(sqlQuery)

    End Function

    Function CreateAsientosCierreIIBB(ByVal Mes As String, ByVal Anio As String, ByVal EstablecimientoId As String) As responseAsientoCierre
        Dim sqlQuery As String

        Dim prefijoMes = IIf(Mes.Length < 2, "0", "")
        Dim NombreAsiento As String = "IIBB " + prefijoMes + Mes + "/" + Anio

        sqlQuery = "
	SELECT '' as NroAsiento, '" + NombreAsiento + "' as Concepto, a.Establecimiento as EstablecimientoNombre , c.CuentaId, s.SubcuentaId, s.Descripcion as SubCuentaDescripcion, 'D' as Partida, SUM(a.Importe) as Importe
FROM (SELECT e.Descripcion Establecimiento, c.CuentaId, s.SubcuentaId, s.Descripcion, 'D' Partida,SUM(
		CASE
		WHEN a.Partida = 'D' THEN a.Importe * -1
		ELSE a.Importe
		END) * 0.035 Importe
		FROM Asientos a
		JOIN Cuentas c ON c.CuentaId = a.CuentaId
		JOIN SubCuentas s ON c.CuentaId = s.CuentaId AND s.SubCuentaId = a.SubCuentaId
		JOIN ENSEMBLE..RelEstablecimientosSucursales res ON res.SucursalId = a.SucursalId
		JOIN ENSEMBLE..Establecimientos e ON e.EstablecimientoId = res.EstablecimientoId
		WHERE MONTH(a.Fecha) = " + Mes + " AND YEAR(a.Fecha) = " + Anio + " 
        AND res.EstablecimientoId = " + EstablecimientoId + "     
        AND a.SucursalId NOT LIKE '9___' AND c.Descripcion LIKE '%Ingreso%'
		GROUP BY e.Descripcion, c.CuentaId, s.SubcuentaId, s.Descripcion) a, Cuentas c
	JOIN SubCuentas s ON s.CuentaId=c.CuentaId
	WHERE c.Descripcion LIKE '%IIBB%' AND c.CuentaId LIKE '5%'
	GROUP BY a.Establecimiento, c.CuentaId, s.SubcuentaId, s.Descripcion
    UNION
    SELECT '' as NroAsiento, '" + NombreAsiento + "' as Concepto,  e.Descripcion  as EstablecimientoNombre, c.CuentaId, s.SubCuentaId, s.Descripcion as SubCuentaDescripcion , 'H' as Partida, SUM(
	CASE WHEN a.Partida = 'H' THEN a.Importe * -1 ELSE a.Importe END) Importe
	FROM Asientos a
	JOIN Cuentas c ON c.CuentaId = a.CuentaId
	JOIN SubCuentas s ON c.CuentaId = s.CuentaId AND s.SubCuentaId = a.SubCuentaId
	JOIN ENSEMBLE..RelEstablecimientosSucursales res ON res.SucursalId = a.SucursalId
	JOIN ENSEMBLE..Establecimientos e ON e.EstablecimientoId = res.EstablecimientoId
	WHERE MONTH(a.Fecha) = " + Mes + " AND YEAR(a.Fecha) = " + Anio + " 
    AND res.EstablecimientoId = " + EstablecimientoId + " 
	AND a.SucursalId NOT LIKE '9___' AND c.Descripcion LIKE '%Suf%' AND c.Descripcion LIKE '%Percep%' AND s.Descripcion LIKE '%IIBB%'
	GROUP BY e.Descripcion, c.CuentaId, s.SubcuentaId,s.Descripcion
    UNION
    SELECT '' as NroAsiento, '" + NombreAsiento + "' as Concepto,  e.Descripcion  as EstablecimientoNombre, c.CuentaId, s.SubCuentaId, s.Descripcion as SubCuentaDescripcion , 'H' as Partida, SUM(
	CASE WHEN a.Partida = 'H' THEN a.Importe * -1 ELSE a.Importe END) Importe
	FROM Asientos a
	JOIN Cuentas c ON c.CuentaId = a.CuentaId
	JOIN SubCuentas s ON c.CuentaId = s.CuentaId AND s.SubCuentaId = a.SubCuentaId
	JOIN ENSEMBLE..RelEstablecimientosSucursales res ON res.SucursalId = a.SucursalId
	JOIN ENSEMBLE..Establecimientos e ON e.EstablecimientoId = res.EstablecimientoId
	WHERE a.Fecha <= DATEADD(day, -1, CONVERT(DATETIME, '" + Anio + "-" + Mes + "-01')) AND a.SucursalId NOT LIKE '9___' 
    AND res.EstablecimientoId = " + EstablecimientoId + " 
	AND c.Descripcion LIKE '%IIBB%' AND c.Descripcion LIKE '%Favor%'
	GROUP BY e.Descripcion, c.CuentaId, s.SubCuentaId, s.Descripcion"

        Return GetAsientosCierre(sqlQuery)

    End Function


    Function GetAsientosCierre(ByVal sqlQuery As String) As responseAsientoCierre
        Dim respuesta As New responseAsientoCierre

        Try
            rs.Source = sqlQuery
            Query.Add(rs.Source)
            rs.Abrir()
            If Not rs.EOF Then
                Dim Lista As New List(Of AsientoCierre)
                Do While Not rs.EOF
                    Dim Asiento As New AsientoCierre
                    Asiento.NroAsiento = rs("NroAsiento").Valor
                    Asiento.EstablecimientoNombre = rs("EstablecimientoNombre").Valor
                    Asiento.CuentaId = rs("CuentaId").Valor
                    Asiento.SubCuentaId = rs("SubCuentaId").Valor
                    Asiento.SubCuentaDescripcion = rs("SubCuentaDescripcion").Valor
                    Asiento.Partida = rs("Partida").Valor
                    Asiento.Concepto = rs("Concepto").Valor
                    Asiento.Importe = rs("importe").Valor
                    Lista.Add(Asiento)
                    rs.MoveNext()
                Loop
                respuesta.ConsultaExitosa = True
                respuesta.Items = Lista
            Else
                respuesta.mensaje = "No existen asientos para la fecha indicada en la base de tempo"
            End If
            rs.Cerrar()
        Catch ex As Exception
            respuesta.mensaje = "Error BD al consultar el asiento de la base de tempo " + vbCrLf + ex.Message
        End Try

        Return respuesta
    End Function

    Public Function GenerarAsientoContableCierre(ByVal EjercicioId As String, ByVal NroAsiento As String, ByVal SucursalId As String, ByVal FechaAsiento As String, ByVal ObjAsiento As AsientoCierre) As ResponseWS
        Dim respuesta As New ResponseWS
        Dim sqlQuery As String

        Try

            'Obtengo la configuracion de las cuentas
            Dim IdCuenta As String = ObjAsiento.CuentaId
            Dim IdSubCuenta As String = ObjAsiento.SubCuentaId
            Dim Partida As String = ObjAsiento.Partida
            Dim Concepto As String = ObjAsiento.Concepto
            Dim tipoAsiento As String = "MAN"
            Dim Importe As Decimal = IIf(Decimal.TryParse(ObjAsiento.Importe, Importe), Importe, 0)
            If IdCuenta <> "" And IdSubCuenta <> "" And Concepto <> "" Then

                'INSERT INTO Asientos VALUES(@EjercicioId, @NroAsiento, GETDATE(), 8, @CtaProdPropios, @SubCtaProdPropios, 'D', ABS(@ImporteLinea), @Concepto, 'PRE', 0, NULL)
                If Importe > 0 Then
                    sqlQuery = "INSERT INTO Asientos
                    VALUES (" + EjercicioId + ", " + NroAsiento + ", '" + FechaAsiento + "' , " + SucursalId + ", '" + IdCuenta + "', '" + IdSubCuenta + "', 
					'" + Partida + "', ABS(" + ObjAsiento.Importe.ToString() + "), '" + Concepto + "', '" + tipoAsiento + "', 0, NULL)"
                    conn.Execute(sqlQuery)
                End If

                respuesta.ConsultaExitosa = True
            Else
                respuesta.mensaje = "Los parametros de configuracion de asientos no son correctos"
            End If
        Catch ex As Exception
            respuesta.mensaje = "Error BD al generar el asiento contable"
        End Try
        Return respuesta
    End Function


End Class
