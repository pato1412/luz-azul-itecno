Public Class LuzAzulCompras
    Inherits LuzAzulCommon

    Public Class ResponseDocumento
        Inherits ResponseWS

        Public rs As IEnumerable(Of Documento)
    End Class

    Public Class CprasResponseDetalle
        Inherits ResponseWS

        Public rs As IEnumerable(Of CprasDetalleDocumento)
    End Class

    Public Class Proveedor
        Public Property ProveedorId As String
        Public Property RazonSocial As String
        Public Property PosIVAId As Integer
        Public Property CondicionVentaId As Integer
        Public Property JurisdiccionIIBBId As String
        Public Property CUIT As String
    End Class

    Public Class Documento
        Public DocumentoId As Long
        Public Sucursal As Sucursal
        Public SucursalId As Integer
        Public TipoDocumentoId As String
        Public Numero As Long
        Public Fecha As Date
        Public Deposito As Deposito
        Public DepositoDestino As Deposito
        Public Proveedor As Proveedor
        Public ListDetalle As New List(Of CprasDetalleDocumento)
        Public Impuestos As Double
        Public PercepIIBB As Double
        Public PercepIVA As Double
        Public PercepSUSS As Double
        Public PercepGcia As Double
        Public Subtotal As Double
        Public JurisdiccionIIBBId As String
        Public TieneError As Boolean

        Public Function GetSubtotal() As Double
            Dim subtotal As Double

            For Each detalle In ListDetalle
                subtotal += detalle.Cantidad * detalle.Unitario
            Next

            Return subtotal
        End Function

        Public Function GetTotal() As Double
            Dim total As Double

            For Each detalle In ListDetalle
                total += detalle.Cantidad * (detalle.Unitario * (1 + (detalle.AlicuotaIVA / 100)))
            Next

            total += Impuestos + PercepIIBB + PercepGcia + PercepIVA + PercepSUSS

            Return total
        End Function
    End Class

    Public Class CprasDetalleDocumento
        Public ProductoId As String
        Public Cantidad As Double
        Public Unitario As Double
        Public Descuento As Boolean
        Public AlicuotaIVA As Double
        Public Descripcion As String
        Public PorcDescuento As Double
    End Class

    Public Function GetDocumentosACargar(ByVal EstablecimientoId As Integer, ByVal UsuarioId As String, ByVal EsAdministrador As Boolean) As ResponseDocumento
        Dim respuestaSucursales As New ResponseSucursal
        Dim respuesta As New ResponseDocumento

        Dim sucursalFiscal As Sucursal
        Dim sucursalPrueba As Sucursal

        Try
            respuestaSucursales = GetSucursalesUsuario(UsuarioId, EstablecimientoId, False, EsAdministrador)

            If Not respuestaSucursales.ConsultaExitosa Then
                respuesta.mensaje = "Error BD consultando las sucursales del usuario"
                Return respuesta
            ElseIf (respuestaSucursales.rs.Count = 0) Then
                respuesta.mensaje = "No se encontraron sucursales asociadas al usuario"
                Return respuesta
            End If

            sucursalFiscal = respuestaSucursales.rs.ToList.Find(Function(sucursal) sucursal.SucursalId < 1000 And Not sucursal.Descripcion.ToUpper.Contains("USAR"))
            If sucursalFiscal Is Nothing Then
                respuesta.mensaje = "No se encontro sucursal fiscal para cargar los documentos"
                Return respuesta
            End If

            sucursalPrueba = respuestaSucursales.rs.ToList.Find(Function(sucursal) sucursal.SucursalId >= 9000 And Not sucursal.Descripcion.ToUpper.Contains("USAR"))
            If sucursalPrueba Is Nothing Then
                respuesta.mensaje = "No se encontro sucursal prueba para cargar los documentos"
                Return respuesta
            End If
        Catch ex As Exception
            respuesta.mensaje = "Error BD consultando las sucursales del usuario"
            Return respuesta
        End Try

        Dim respuestaDepositos As New ResponseDeposito

        Try
            respuestaDepositos = GetDepositosUsuario(UsuarioId, EstablecimientoId, False, EsAdministrador, False)

            If Not respuestaDepositos.ConsultaExitosa Then
                respuesta.mensaje = "Error BD consultando los depositos del usuario"
                Return respuesta
            ElseIf (respuestaDepositos.rs.Count = 0) Then
                respuesta.mensaje = "No se encontraron depositos asociados al usuario"
                Return respuesta
            End If
        Catch ex As Exception
            respuesta.mensaje = "Error BD consultando las depositos del usuario"
            Return respuesta
        End Try

        Try
            Dim deposito As Deposito = respuestaDepositos.rs.ToList.First

            ' Busco la cabecera de los documentos a cargar
            rs.Source = "SELECT tmp.SucursalId, tmp.TipoDocumentoId, tmp.Numero, ISNULL(p.ProveedorId, '') ProveedorId, ISNULL(p.CUIT, '99-99999999-9') CUIT,
                ISNULL(p.RazonSocial, '') RazonSocial, ISNULL(p.PosIVAId, 0) PosIVAId, ISNULL(p.CondicionVentaId, 0) CondicionVentaId, 
                ISNULL(p.JurisdiccionIIBBId, '') JurisdiccionIIBBId, tmp.Fecha, tmp.Subtotal, tmp.Impuestos, tmp.PercepIIBB, tmp.PercepIVA, 
                tmp.PercepGcia, tmp.PercepSUSS, ISNULL(tmp.JurisdiccionIIBBId, '') JurisIIBBId 
                FROM ENSEMBLE.dbo.TmpSincroCprasDocumentosCabecera tmp                                
                LEFT JOIN Proveedores p ON p.CUIT = tmp.CUITProveedor
                WHERE tmp.EstablecimientoId = " & EstablecimientoId
            Query.Add(rs.Source)
            rs.Abrir()

            If Not rs.EOF Then
                Dim documento As Documento
                Dim ListaRegistros As New List(Of Documento)
                Dim pos As Integer
                Dim docIdParcial As Integer = 1

                Do While Not rs.EOF
                    Dim proveedor As Proveedor

                    If rs("ProveedorId").Valor = "" Then
                        proveedor = Nothing
                    Else
                        proveedor = New Proveedor With {
                            .ProveedorId = rs("ProveedorId").Valor,
                            .RazonSocial = rs("RazonSocial").Valor,
                            .CondicionVentaId = rs("CondicionVentaId").Valor,
                            .PosIVAId = rs("PosIVAId").Valor,
                            .CUIT = rs("CUIT").Valor,
                            .JurisdiccionIIBBId = rs("JurisdiccionIIBBId").Valor
                        }
                    End If

                    documento = Nothing
                    pos = ListaRegistros.ToList.FindIndex(Function(element)
                                                              Return element.Proveedor.ProveedorId = rs("ProveedorId").Valor And
                                                                element.SucursalId = rs("SucursalId").Valor And
                                                                element.TipoDocumentoId = rs("TipoDocumentoId").Valor And
                                                                element.Numero = rs("Numero").Valor
                                                          End Function)
                    If pos <= 0 Then
                        documento = New Documento With {
                            .DocumentoId = docIdParcial,
                            .Sucursal = IIf(rs("SucursalId").Valor >= 9000, sucursalPrueba, sucursalFiscal),
                            .SucursalId = rs("SucursalId").Valor,
                            .TipoDocumentoId = IIf(rs("SucursalId").Valor >= 9000, "FCB", rs("TipoDocumentoId").Valor),
                            .Numero = rs("Numero").Valor,
                            .Fecha = CDate(rs("Fecha").Valor),
                            .Proveedor = proveedor,
                            .Deposito = deposito,
                            .DepositoDestino = deposito,
                            .Impuestos = rs("Impuestos").Valor,
                            .PercepIIBB = rs("PercepIIBB").Valor,
                            .PercepIVA = rs("PercepIVA").Valor,
                            .PercepGcia = rs("PercepGcia").Valor,
                            .PercepSUSS = rs("PercepSUSS").Valor,
                            .JurisdiccionIIBBId = rs("JurisIIBBId").Valor,
                            .Subtotal = rs("Subtotal").Valor,
                            .ListDetalle = New List(Of CprasDetalleDocumento)
                        }

                        docIdParcial += 1
                        ListaRegistros.Add(documento)
                    End If

                    rs.MoveNext()
                Loop

                respuesta.rs = ListaRegistros
            End If
            rs.Cerrar()

            ' Busco el detalle de los documentos a cargar
            rs.Source = "SELECT tmp.SucursalId, tmp.TipoDocumentoId, tmp.Numero, ISNULL(p.ProveedorId, '') ProveedorId, tmp.ProductoId,
                tmp.Cantidad, tmp.Unitario, tmp.Descuento, tmp.AlicuotaIVA, tmp.Descripcion, tmp.PorcDescuento
                FROM ENSEMBLE.dbo.TmpSincroCprasDocumentosDetalle tmp 
                LEFT JOIN Proveedores p ON p.CUIT = tmp.CUITProveedor
                WHERE tmp.EstablecimientoId = " & EstablecimientoId
            Query.Add(rs.Source)
            rs.Abrir()

            If Not rs.EOF Then
                Dim documento As Documento

                Do While Not rs.EOF
                    documento = Nothing
                    documento = respuesta.rs.ToList.Find(Function(element)
                                                             Return element.Proveedor.ProveedorId = rs("ProveedorId").Valor And
                                                                element.SucursalId = rs("SucursalId").Valor And
                                                                element.TipoDocumentoId.Substring(0, 2) = rs("TipoDocumentoId").Valor.ToString().Substring(0, 2) And
                                                                element.Numero = rs("Numero").Valor
                                                         End Function)

                    If Not (documento Is Nothing) Then
                        documento.ListDetalle.Add(New CprasDetalleDocumento With {
                            .ProductoId = rs("ProductoId").Valor,
                            .Cantidad = rs("Cantidad").Valor,
                            .Unitario = rs("Unitario").Valor,
                            .Descuento = CBool(rs("Descuento").Valor),
                            .AlicuotaIVA = rs("AlicuotaIVA").Valor,
                            .Descripcion = rs("Descripcion").Valor,
                            .PorcDescuento = rs("PorcDescuento").Valor
                        })
                    End If

                    rs.MoveNext()
                Loop
            End If

            respuesta.ConsultaExitosa = True
            rs.Cerrar()
        Catch ex As Exception
            respuesta.mensaje = "Error BD consultando los documentos a cargar"
        End Try

        Return respuesta
    End Function

    Public Function GetConceptosParticulares() As CprasResponseDetalle
        Dim respuesta As New CprasResponseDetalle

        Try
            ' Busco los conceptos particulares de gastos
            rs.Source = "SELECT * FROM Productos                
                WHERE TipoProductoId = 1 AND LEFT(ProductoId, 2) = '99' AND 
                (Descripcion LIKE '%Software%' OR Descripcion LIKE '%Publici%' 
                OR Descripcion LIKE '%Indumentaria%' OR Descripcion LIKE '%Papelera%')"
            Query.Add(rs.Source)
            rs.Abrir()

            If Not rs.EOF Then
                Dim detalle As CprasDetalleDocumento
                Dim ListaRegistros As New List(Of CprasDetalleDocumento)

                Do While Not rs.EOF
                    detalle = New CprasDetalleDocumento With {
                            .ProductoId = rs("ProductoId").Valor,
                            .Descripcion = rs("Descripcion").Valor,
                            .Cantidad = 1,
                            .AlicuotaIVA = 21,
                            .Descuento = False,
                            .PorcDescuento = 0,
                            .Unitario = 0
                        }

                    ListaRegistros.Add(detalle)
                    rs.MoveNext()
                Loop

                respuesta.rs = ListaRegistros
            End If

            respuesta.ConsultaExitosa = True
            rs.Cerrar()
        Catch ex As Exception
            respuesta.mensaje = "Error BD consultando los conceptos particulares de compras"
        End Try

        Return respuesta
    End Function

    Public Sub DeleteDocumentoTmp(ByVal establecimientoId As Integer, ByVal sucursalId As Integer, ByVal tipoDocumentoId As String, ByVal numero As Long, ByVal CUIT As String)
        Try
            conn.BeginTransaction()
            conn.Execute("DELETE FROM ENSEMBLE.dbo.TmpSincroCprasDocumentosDetalle 
                        WHERE EstablecimientoId = " & establecimientoId.ToString() & " 
                        AND SucursalId = " & sucursalId.ToString() & " 
                        AND LEFT(TipoDocumentoId, 2) = '" & tipoDocumentoId.Substring(0, 2) & "' 
                        AND Numero = " & numero.ToString() & "
                        AND CUITProveedor = '" & CUIT & "'")

            conn.Execute("DELETE FROM ENSEMBLE.dbo.TmpSincroCprasDocumentosCabecera 
                        WHERE EstablecimientoId = " & establecimientoId.ToString() & " 
                        AND SucursalId = " & sucursalId.ToString() & " 
                        AND LEFT(TipoDocumentoId, 2) = '" & tipoDocumentoId.Substring(0, 2) & "' 
                        AND Numero = " & numero.ToString() & "
                        AND CUITProveedor = '" & CUIT & "'")
            conn.CommitTransaction()
        Catch ex As Exception
            conn.RollbackTransaction()
        End Try
    End Sub

    Public Sub CompararDocumentosSinCargar()
        ' Borro todos los documentos pendientes de sincronizar
        conn.Execute("DELETE FROM " & NombreBaseEnsemble & ".dbo.TmpSincroCprasDocumentosDetalle")
        conn.Execute("DELETE FROM " & NombreBaseEnsemble & ".dbo.TmpSincroCprasDocumentosCabecera")

        Try
            ' Inserto la cabecera de los documentos a sincronizar    
            conn.Execute("INSERT INTO " & NombreBaseEnsemble & ".dbo.TmpSincroCprasDocumentosCabecera 
                SELECT dc.EstablecimientoId, dc.SucursalId, dc.TipoDocumentoId, dc.Numero, '30-71211042-9', 
                dc.Fecha, dc.Subtotal, dc.Impuestos, dc.PercepIVA, dc.PercepGcia, dc.PercepIIBB, dc.PercepSUSS, dc.JurisdiccionIIBBId
                FROM (SELECT e.EstablecimientoId, dc.SucursalId, dc.TipoDocumentoId, dc.Numero, dc.Fecha, 
                    dc.Subtotal, dc.Impuestos, dc.PercepIVA, dc.PercepGcia, dc.PercepIIBB, dc.PercepSUSS, j.JurisdiccionIIBBId
                    FROM [TEMPONUBE.SOFTWARETEMPO.COM\SQLEXPRESS,4896].EMP001.dbo.DocumentosCabecera dc 
	                 JOIN [TEMPONUBE.SOFTWARETEMPO.COM\SQLEXPRESS,4896].EMP001.dbo.RelClientesContactos r ON r.ClienteId = dc.ClienteId
	                 JOIN " & NombreBaseEnsemble & ".dbo.Establecimientos e ON r.Celular = e.EstablecimientoId
                    JOIN " & NombreBaseEnsemble & ".dbo.GWRConfiguracion cfg ON cfg.Parametro = 'FechaCtrlDifDocs'
                    LEFT JOIN [TEMPONUBE.SOFTWARETEMPO.COM\SQLEXPRESS,4896].EMP001.dbo.RelDocumentosJurisdiccionesIIBB j ON j.DocumentoId = dc.DocumentoId 
                    WHERE r.TipoContactoClienteId = 6 AND (dc.TipoDocumentoId Like 'FC%') AND dc.Fecha >= Convert(DateTime, cfg.Valor + ' 00:00:00',101) 
                    AND dc.EstadoDocumentoId <> 3 AND dc.Total > 0) dc
            LEFT JOIN (SELECT dc.ProveedorId, dc.TipoDocumentoId, dc.SucursalId, dc.Numero FROM CprasDocumentosCabecera dc 
                    JOIN Proveedores p ON p.ProveedorId = dc.ProveedorId 
                    WHERE p.CUIT = '30-71211042-9') cdc ON LEFT(cdc.TipoDocumentoId, 2) = LEFT(dc.TipoDocumentoId, 2) AND cdc.SucursalId = dc.SucursalId AND cdc.Numero = dc.Numero
            WHERE dc.Fecha <= GETDATE() And cdc.ProveedorId Is NULL")

            ' Inserto el detalle de los documentos a sincronizar
            conn.Execute("INSERT INTO " & NombreBaseEnsemble & ".dbo.TmpSincroCprasDocumentosDetalle
                SELECT tmp.EstablecimientoId,  dc.SucursalId, dc.TipoDocumentoId, dc.Numero, '30-71211042-9', 
                dd.ProductoId, dd.Cantidad, dd.Unitario, dd.Descuento, dd.AlicuotaIVA, dd.Descripcion, dd.PorcDescuento
                FROM [TEMPONUBE.SOFTWARETEMPO.COM\SQLEXPRESS,4896].EMP001.dbo.DocumentosDetalle dd 
                JOIN [TEMPONUBE.SOFTWARETEMPO.COM\SQLEXPRESS,4896].EMP001.dbo.DocumentosCabecera dc ON dc.DocumentoId = dd.DocumentoId
                JOIN [TEMPONUBE.SOFTWARETEMPO.COM\SQLEXPRESS,4896].EMP001.dbo.RelClientesContactos r ON r.ClienteId = dc.ClienteId
                JOIN " & NombreBaseEnsemble & ".dbo.TmpSincroCprasDocumentosCabecera tmp ON tmp.EstablecimientoId = r.Celular And tmp.SucursalId = dc.SucursalId And tmp.TipoDocumentoId = dc.TipoDocumentoId And tmp.Numero = dc.Numero
                JOIN " & NombreBaseEnsemble & ".dbo.GWRConfiguracion cfg ON cfg.Parametro = 'FechaCtrlDifDocs'
                JOIN Productos p ON p.ProductoId = dd.ProductoId
                WHERE r.TipoContactoClienteId = 6 And (dc.TipoDocumentoId Like 'FC%') AND dc.Fecha >= Convert(DateTime, cfg.Valor + ' 00:00:00',101) 
                AND dc.EstadoDocumentoId <> 3 AND dc.Total > 0")
        Catch ex As Exception
            ' Si da error es porque es el otro Linked Server
            conn.Execute("INSERT INTO ENSEMBLE.dbo.TmpSincroCprasDocumentosCabecera 
                SELECT dc.EstablecimientoId, dc.SucursalId, dc.TipoDocumentoId, dc.Numero, '30-71211042-9', 
                dc.Fecha, dc.Subtotal, dc.Impuestos, dc.PercepIVA, dc.PercepGcia, dc.PercepIIBB, dc.PercepSUSS, dc.JurisdiccionIIBBId
                FROM (SELECT e.EstablecimientoId, dc.SucursalId, dc.TipoDocumentoId, dc.Numero, dc.Fecha, 
                    dc.Subtotal, dc.Impuestos, dc.PercepIVA, dc.PercepGcia, dc.PercepIIBB, dc.PercepSUSS, j.JurisdiccionIIBBId
                    FROM [50.1.2.2\SQLEXPRESS].EMP001.dbo.DocumentosCabecera dc 
	                 JOIN [50.1.2.2\SQLEXPRESS].EMP001.dbo.RelClientesContactos r On r.ClienteId = dc.ClienteId And dc.DireccionId = r.DireccionId
	                 JOIN " & NombreBaseEnsemble & ".dbo.Establecimientos e ON r.Celular = e.EstablecimientoId
                    JOIN " & NombreBaseEnsemble & ".dbo.GWRConfiguracion cfg ON cfg.Parametro = 'FechaCtrlDifDocs'
                    LEFT JOIN [50.1.2.2\SQLEXPRESS].EMP001.dbo.RelDocumentosJurisdiccionesIIBB j ON j.DocumentoId = dc.DocumentoId 
                    WHERE r.TipoContactoClienteId = 6 AND (dc.TipoDocumentoId Like 'FC%') AND dc.Fecha >= Convert(DateTime, cfg.Valor + ' 00:00:00',101) 
                    AND dc.EstadoDocumentoId <> 3 AND dc.Total > 0) dc
            LEFT JOIN (SELECT dc.ProveedorId, dc.TipoDocumentoId, dc.SucursalId, dc.Numero FROM CprasDocumentosCabecera dc 
                    JOIN Proveedores p ON p.ProveedorId = dc.ProveedorId 
                    WHERE p.CUIT = '30-71211042-9') cdc ON LEFT(cdc.TipoDocumentoId, 2) = LEFT(dc.TipoDocumentoId, 2) AND cdc.SucursalId = dc.SucursalId AND cdc.Numero = dc.Numero
            WHERE dc.Fecha <= GETDATE() And cdc.ProveedorId Is NULL")

            ' Inserto el detalle de los documentos a sincronizar
            conn.Execute("INSERT INTO " & NombreBaseEnsemble & ".dbo.TmpSincroCprasDocumentosDetalle
                SELECT tmp.EstablecimientoId,  dc.SucursalId, dc.TipoDocumentoId, dc.Numero, '30-71211042-9', 
                dd.ProductoId, dd.Cantidad, dd.Unitario, dd.Descuento, dd.AlicuotaIVA, dd.Descripcion, dd.PorcDescuento
                FROM [50.1.2.2\SQLEXPRESS].EMP001.dbo.DocumentosDetalle dd 
                JOIN [50.1.2.2\SQLEXPRESS].EMP001.dbo.DocumentosCabecera dc ON dc.DocumentoId = dd.DocumentoId
                JOIN [50.1.2.2\SQLEXPRESS].EMP001.dbo.RelClientesContactos r ON r.ClienteId = dc.ClienteId And dc.DireccionId = r.DireccionId
                JOIN " & NombreBaseEnsemble & ".dbo.TmpSincroCprasDocumentosCabecera tmp ON tmp.EstablecimientoId = r.Celular And tmp.SucursalId = dc.SucursalId And tmp.TipoDocumentoId = dc.TipoDocumentoId And tmp.Numero = dc.Numero
                JOIN " & NombreBaseEnsemble & ".dbo.GWRConfiguracion cfg ON cfg.Parametro = 'FechaCtrlDifDocs'
                JOIN Productos p ON p.ProductoId = dd.ProductoId
                WHERE r.TipoContactoClienteId = 6 And (dc.TipoDocumentoId Like 'FC%') AND dc.Fecha >= Convert(DateTime, cfg.Valor + ' 00:00:00',101) 
                AND dc.EstadoDocumentoId <> 3 AND dc.Total > 0")
        End Try
    End Sub

End Class
