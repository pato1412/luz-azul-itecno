Public Class LuzAzulEmision
    Inherits LuzAzulCommon
    Public Class ResponseConfigGlobales
        Inherits ResponseWS

        Public LineasEnDetalle As Integer
        Public FormaPercepIIBB As String
        Public LiqComisionFCoREC As String
    End Class

    Public Class ResponseDatosDocumento
        Inherits ResponseWS

        Public rs As IEnumerable(Of DatosDocumento)
    End Class

    Public Class Documento
        Public DocumentoId As Long
        Public Fecha As Date
        Public TipoDocumento As String
        Public Sucursal As Sucursal
        Public Numero As Long
        Public Deposito As Deposito
        Public DepositoDestino As Deposito
        Public Cliente As Cliente
        Public ListDetalle As New CustomVbCollection
        Public ListPecepIIBB As New CustomVbCollection
        Public PercepIVA As Double
        Public PercepIVARG5329 As Double
        Public PercepSUSS As Double
        Public PercepGcias As Double
        Public Total As Double
        Public Descripcion As String
        Public CAE As String

        Public Sub New(ByVal Cliente As Cliente)
            Me.Cliente = Cliente
        End Sub
    End Class

    Public Class DiferenciaProducto
        Public ProductoId As String
        Public Descripcion As String
        Public CantNecesario As Double
        Public StockActual As Double
        Public Diferencia As Double
    End Class

    Public Class DatosDocumento
        Public DocumentoId As Long
        Public TipoDocumento As String
        Public Numero As Long
        Public Total As Double
        Public CAE As String
    End Class

    Public Class OrdenPendienteFC
        Public ordenPickeoId As String
        Public establecimientoId As String
        Public descEstablecimiento As String
        Public fecha As Date
        Public estadoId As String
        Public estado As String
        Public razonSocial As String
        Public CUIT As String
        Public detalle As New List(Of DetalleDocumento)
    End Class

    Public Function GetConfigGlobales(ByVal tipoDocumento As String, ByVal sucursal As Integer) As ResponseConfigGlobales
        Dim respuesta As New ResponseConfigGlobales

        Try
            ' Lineas en Detalle
            rs.Source = "SELECT LineasDetalle FROM Gestion.dbo.RelFormulariosEmpresas r
                JOIN Gestion.dbo.Formularios f ON f.FormularioId = r.FormularioId
                WHERE r.Empresa = '" + Me.NombreBase + "' AND r.SucursalId = " + sucursal.ToString() + " 
                AND f.TipoDocumentoId = '" + tipoDocumento + "'"
            Query.Add(rs.Source)
            rs.Abrir()
            If Not rs.EOF Then
                respuesta.ConsultaExitosa = True
                respuesta.LineasEnDetalle = CInt(rs("LineasDetalle").Valor)
            Else
                respuesta.ConsultaExitosa = False
                respuesta.mensaje = "No se encontro formato de impresión para el documento de tipo " + tipoDocumento + " y sucursal " + sucursal.ToString() + ", comuniquese con el Administrador"
            End If
            rs.Cerrar()
        Catch ex As Exception
            respuesta.ConsultaExitosa = False
            respuesta.mensaje = "Error DB consultando LineasDetalle para el documento de tipo " + tipoDocumento + " y sucursal " + sucursal.ToString()
        End Try

        If respuesta.ConsultaExitosa Then
            Try
                rs.Source = "SELECT Parametro, Valor FROM Configuracion WHERE Parametro IN ('FormaPercepIIBB','LiqComisionFCoREC')"
                Query.Add(rs.Source)
                rs.Abrir()
                If Not rs.EOF Then
                    Do While Not rs.EOF
                        Select Case rs("Parametro").Valor
                            Case "FormaPercepIIBB" : respuesta.FormaPercepIIBB = rs("Valor").Valor
                            Case "LiqComisionFCoREC" : respuesta.LiqComisionFCoREC = rs("Valor").Valor
                        End Select
                        rs.MoveNext()
                    Loop
                    respuesta.ConsultaExitosa = True
                Else
                    respuesta.ConsultaExitosa = False
                    respuesta.mensaje = "No se encontro la forma en que se calculan las percepciones de IIBB ni la liquidacion de comisiones, comuniquese con el Administrador"
                End If
                rs.Cerrar()
            Catch ex As Exception
                respuesta.ConsultaExitosa = False
                respuesta.mensaje = "Error DB consultando la forma en que se calculan las percepciones de IIBB y la liquidacion de comisiones"
            End Try
        End If

        Return respuesta
    End Function

    Public Function GetDatosDocumentos(ByVal ListaIdDocumentos As String) As ResponseDatosDocumento
        Dim respuesta As New ResponseDatosDocumento

        Try
            ' Busco a un cliente con la Razon Social y el Deposito que me pasan porque un mismo cliente puede tener mas de un local
            rs.Source = "SELECT DocumentoId, TipoDocumentoId, Numero, Total, ISNULL(CAE, '') CAE FROM DocumentosCabecera WHERE DocumentoId IN (" + ListaIdDocumentos + ")"
            Query.Add(rs.Source)
            rs.Abrir()

            If Not rs.EOF Then
                Dim documento As DatosDocumento
                Dim ListaRegistros As New List(Of DatosDocumento)

                Do While Not rs.EOF
                    documento = New DatosDocumento With {
                        .DocumentoId = rs("DocumentoId").Valor,
                        .TipoDocumento = rs("TipoDocumentoId").Valor,
                        .Numero = rs("Numero").Valor,
                        .Total = rs("Total").Valor,
                        .CAE = rs("CAE").Valor
                    }

                    ListaRegistros.Add(documento)
                    rs.MoveNext()
                Loop

                respuesta.ConsultaExitosa = True
                respuesta.rs = ListaRegistros
            End If

            rs.Cerrar()
        Catch ex As Exception
            respuesta.mensaje = "Error BD consultando los Documentos con IDs " & ListaIdDocumentos
        End Try

        Return respuesta
    End Function

    ''' <summary>
    ''' Obtiene el Saldo en Cta Cte de un cliente.
    ''' </summary>
    ''' <param name="clienteId">Codigo del cliente</param>
    ''' <param name="monedaId">Codigo de la moneda en que se esta evaluando el saldo</param>
    ''' <param name="docIdSel">Lista separada por (,) de los REMs o FCs seleccionados que no se tienen que tomar en cuenta para el control crediticio</param>
    ''' <return>Devulve el importe en cuenta corriente del clienteen caso de exito y 0 en caso de error.</return>
    Public Function GetSaldoCtaCte(clienteId As String, Optional monedaId As Long = 0, Optional docIdSel As String = "") As Double
        Dim saldo As Double = 0

        Try
            'Saldo en CtaCte
            rs.Source = "SELECT c.CtrlCredito, ISNULL( 
                SUM(CASE
                    WHEN TipoDocumentoId LIKE 'FC%' OR TipoDocumentoId LIKE 'ND%' OR TipoDocumentoId = 'REM' THEN ISNULL((MontoAplicar/ISNULL(rd.Cotizacion,1))*(md.CotizaVsBase/m.CotizaVsBase), 0)
                    WHEN TipoDocumentoId LIKE 'NC%' OR TipoDocumentoId = 'PAG' OR TipoDocumentoId = 'REC' THEN (ISNULL((MontoAplicar/ISNULL(rd.Cotizacion,1))*(md.CotizaVsBase/m.CotizaVsBase),0)) * -1
                    END), 0) Saldo             
                FROM DocumentosCabecera d 
                JOIN Clientes c ON c.ClienteId = d.ClienteId
                JOIN Sucursales s ON s.SucursalId = d.SucursalId
                JOIN Gestion.dbo.Monedas m ON m.MonedaId = " & monedaId.ToString() & " 
                JOIN Gestion..Monedas md ON md.MonedaId = d.MonedaId 
                LEFT JOIN RelDocumentosCabeceraCotizaciones r ON r.DocumentoId = d.DocumentoId AND r.MonedaId =  " & monedaId.ToString() & " 
                LEFT JOIN RelDocumentosCabeceraCotizaciones rd ON rd.DocumentoId = d.DocumentoId AND rd.MonedaId = d.MonedaId 
                WHERE (((TipoDocumentoId LIKE 'FC%' OR TipoDocumentoId LIKE 'NC%' OR TipoDocumentoId LIKE 'ND%' OR TipoDocumentoId = 'PAG' OR TipoDocumentoId = 'REC') AND EstadoDocumentoId <> 3) 
                OR (TipoDocumentoId = 'REM' AND EstadoDocumentoId = 1)) AND c.ClienteId='" & clienteId & "' AND s.Suspendida = 0 "

            If docIdSel <> "" Then rs.Source &= "AND d.DocumentoId NOT IN (" & docIdSel & ") "
            rs.Source &= "GROUP BY c.CtrlCredito"
            rs.Abrir()
            If Not rs.EOF Then saldo = Val(rs("Saldo").Valor)
            rs.Cerrar()

            'Sumatoria de los detalles de los remitos parcialmente facturacion
            rs.Source = "SELECT ISNULL(SUM(dp.Total),0) Saldo 
                FROM DocumentosCabecera d 
                LEFT JOIN (SELECT dp.DocumentoId,dp.ProductoId,SUM(dp.Cantidad)*(dd.Unitario/ISNULL(rd.Cotizacion,1))*(md.CotizaVsBase/m.CotizaVsBase) Total FROM DetallePreparado dp
                    JOIN DocumentosCabecera d ON d.DocumentoId = dp.DocumentoId
                    JOIN DocumentosDetalle dd ON dd.DocumentoId = dp.DocumentoId AND dp.ProductoId = dd.ProductoId
                    JOIN Gestion..Monedas m ON m.MonedaId = " & monedaId.ToString() & "
                    JOIN Gestion..Monedas md ON md.MonedaId = d.MonedaId
                    LEFT JOIN RelDocumentosCabeceraCotizaciones r ON r.DocumentoId = d.DocumentoId AND r.MonedaId = " & monedaId.ToString() & "
                    LEFT JOIN RelDocumentosCabeceraCotizaciones rd ON rd.DocumentoId = d.DocumentoId AND rd.MonedaId = d.MonedaId 
                    JOIN Clientes c ON c.ClienteId = d.ClienteId
                    JOIN Sucursales s ON s.SucursalId = d.SucursalId
                    WHERE d.TipoDocumentoId='REM' AND d.EstadoDocumentoId=1 AND c.ClienteId='" & clienteId & "' AND s.Suspendida=0
                    GROUP BY dp.DocumentoId,dp.ProductoId,dd.Unitario,rd.Cotizacion,md.CotizaVsBase,m.CotizaVsBase) dp ON dp.DocumentoId=d.DocumentoId"

            If docIdSel <> "" Then rs.Source &= " WHERE d.DocumentoId NOT IN (" & docIdSel & ") "
            rs.Abrir()
            If Not rs.EOF Then saldo -= Val(rs("Saldo").Valor)
            rs.Cerrar()
        Catch ex As Exception
        End Try

        Return saldo
    End Function

    Public Function GetUltNum(TipoDoc As String, SucursalId As Short, Optional bEsCompras As Boolean = False,
                               Optional bNCNumFC As Boolean = False, Optional bNDNumFC As Boolean = False) As Long
        Dim sTipoABusc As String
        Dim x As Integer
        Dim lUltNumero As Long = -1

        ' Trae el proximo numero de documento disponible
        rs.Source = "SELECT Parametro,Valor FROM Configuracion WHERE parametro IN ('NCNumFC','NDNumFC')"
        rs.Abrir()
        Do While Not rs.EOF
            Select Case rs("Parametro").Valor
                Case "NCNumFC"
                    bNCNumFC = (rs("Valor").Valor = "S")
                Case "NDNumFC"
                    bNDNumFC = (rs("Valor").Valor = "S")
            End Select
            rs.MoveNext()
        Loop
        rs.Cerrar()

        sTipoABusc = TipoDoc

        ' Si esta configurado usar la numeracion correlativa de facturas
        ' con NCs busco el ultimo nro de FCs, NCs o NDs mas alto
        If bEsCompras = False And (TipoDoc = "NCA" Or TipoDoc = "NCB" Or TipoDoc = "NCE") And bNCNumFC Then
            sTipoABusc = "FC" & Right(Trim(TipoDoc), 1)
        End If

        ' Si esta configurado usar la numeracion correlativa de facturas
        ' con NDs busco el ultimo nro de FCs, NCs o NDs mas alto
        If bEsCompras = False And (TipoDoc = "NDA" Or TipoDoc = "NDB" Or TipoDoc = "NDE") And bNDNumFC Then
            sTipoABusc = "FC" & Right(Trim(TipoDoc), 1)
        End If

        Do
            rs.Source = "SELECT UltNro FROM NumerosDocumentos WHERE SucursalId=" & SucursalId & " AND TipoDocumentoId='" & sTipoABusc & "'"
            rs.Abrir()
            lUltNumero = rs("UltNro").Valor + 1
            rs.Cerrar()

            If Not bEsCompras Then
                'Si esta usado en la base de datos retorno -1.
                rs.Source = "SELECT COUNT(*) Cuenta FROM DocumentosCabecera WHERE TipoDocumentoId='" & sTipoABusc & "' AND SucursalId=" & SucursalId & " AND numero=" & GetUltNum
                rs.Abrir()
                If rs("Cuenta").Valor > 0 Then lUltNumero = -1
                rs.Cerrar()
            Else
                'Si esta usado en la base de datos retorno -1.
                rs.Source = "SELECT COUNT(*) Cuenta FROM cprasDocumentosCabecera WHERE TipoDocumentoId='" & sTipoABusc & "' AND SucursalIdDest=" & SucursalId & " AND numero=" & GetUltNum
                rs.Abrir()
                If rs("Cuenta").Valor > 0 Then lUltNumero = -1
                rs.Cerrar()
            End If
        Loop Until ((lUltNumero <> -1) Or (x = 100))

        Return lUltNumero
    End Function

End Class
