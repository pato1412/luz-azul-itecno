Imports Newtonsoft.Json
Imports LAFunctions.LuzAzulCommon
Public Class ControladorCommon
    Const CUITEnsemble = "30712110429"

    Public Shared DebugMode As Boolean = False
    Public Shared LogEvent As List(Of String) = New List(Of String)
    Public Shared UsuarioId As String = ""
    Public Shared RazonSocial As String = ""
    Public Shared CUIT As String = ""
    Public Shared EsAdministrador As Boolean = False
    Public Shared EsFabrica As Boolean = False
    Public Shared CurrentEstablecimiento As Establecimiento = New Establecimiento("", "", False)
    Public Shared CurrentDepositos As New List(Of Deposito)
    Public Shared ListEstablecimientosDitribucion As New List(Of String)

    Public LACommon As New LuzAzulCommon

    Public Shared Sub SetDebugMode(Debug As Boolean)
        DebugMode = Debug
    End Sub
    Public Function GetDebugMode() As Boolean
        Return DebugMode
    End Function
    Public Shared Sub SetLogEvent(strEvent As String)
        LogEvent.Add(strEvent)
    End Sub

    Public Shared Sub SetUsuarioId(IdUsuario As String)
        UsuarioId = IdUsuario
    End Sub
    Public Function GetUsuarioId() As String
        Return UsuarioId
    End Function
    Public Shared Sub SetRazonSocial(Razon As String)
        RazonSocial = Razon
    End Sub
    Public Function GetRazonSocial() As String
        Return RazonSocial
    End Function

    Public Shared Sub SetCuit(NroCuit As String)
        CUIT = NroCuit
    End Sub
    Public Function GetCuit() As String
        Return CUIT
    End Function
    Public Shared Sub SetEsAdministrador(EsAdmin As Boolean)
        EsAdministrador = EsAdmin
    End Sub

    Public Function GetEsAdministrador() As Boolean
        Return EsAdministrador
    End Function

    Public Shared Sub SetEsFabrica(BFabrica As Boolean)
        EsFabrica = BFabrica
    End Sub
    Public Function GetEsFabrica() As Boolean
        Return EsFabrica
    End Function
    Public Shared Sub SetCurrentEstablecimiento(Est As Establecimiento)
        CurrentEstablecimiento = Est
    End Sub

    Public Function GetCurrentEstablecimiento() As Establecimiento
        Return CurrentEstablecimiento
    End Function
    Public Function GetCurrentEstablecimientoId() As String
        Return CurrentEstablecimiento.EstablecimientoId
    End Function
    Public Shared Sub SetCurrentDeposito(Dep As List(Of Deposito))
        CurrentDepositos = Dep
    End Sub
    Public Function GetCurrentDepositos() As List(Of Deposito)
        Return CurrentDepositos
    End Function
    Public Shared Sub SetListEstablecimientosDitribucion(Dep As List(Of String))
        ListEstablecimientosDitribucion = Dep
    End Sub
    Public Function GetListEstablecimientosDitribucion() As List(Of String)
        Return ListEstablecimientosDitribucion
    End Function

    Public Function InformarFaltantes(ByVal nroOrden As String, ByVal razonSocial As String, ByVal CUIT As String, ByVal EstablecimientoId As String) As String
        Dim ListFaltantes As New List(Of DetalleDocumento)

        Try
            Dim response As String
            Dim postData As String = ""
            postData += "ordenPickeoId=" + nroOrden.ToString()

            Dim urlWeb As String = IIf(DebugMode = True, "https://wstest.luz-azul.com.ar", "https://webservice.luz-azul.com.ar")
            response = LACommon.PostRequest(postData, urlWeb + "/pedidos/detalle_facturacion_pendiente")

            Dim JsonResponse = Linq.JArray.Parse(response)
            For Each item In JsonResponse
                ' Solo si es un pendiente guardo el detalle
                If Val(item("nropallet").ToString()) = 0 Then
                    Dim detalleDoc As New DetalleDocumento With {
                        .NroPallet = item("nropallet").ToString(),
                        .ProductoId = item("productoId").ToString(),
                        .CantUnidades = IIf(item("cantidad") Is Nothing, 0, item("cantidad")),
                        .CantKilogramos = IIf(item("Kgnetos") Is Nothing, item("CantTotal"), item("Kgnetos")) 'Guardo la Cantidad Pedida
                    }

                    ListFaltantes.Add(detalleDoc)
                End If
            Next
        Catch ex As Exception
            Dim strError As String = "Error al obtener la respuesta del webservice - InformarFaltantes - Message: " + ex.Message + vbCrLf + JsonConvert.SerializeObject(ex)
            Return strError
        End Try

        If ListFaltantes.Count > 0 Then
            Dim respuestaCliente As ResponseCliente

            Try
                respuestaCliente = LACommon.GetCliente(razonSocial, EstablecimientoId, CUIT, CUITEnsemble)

                If respuestaCliente.ConsultaExitosa Then
                    Dim listaIdProductos As String = ""

                    ' Obtengo la lista separada por , de los productos obtenidos del WS
                    For Each Detalle As DetalleDocumento In ListFaltantes
                        listaIdProductos += "'" & Detalle.ProductoId & "',"
                    Next
                    ' Elimino la ultima ,
                    listaIdProductos = listaIdProductos.Substring(0, listaIdProductos.Length - 1)

                    ' Obtengo los productos de la BD
                    Dim respuestaDetalle As ResponseDetalle
                    respuestaDetalle = LACommon.GetDatosProductos(listaIdProductos, respuestaCliente.Cliente.ListaPrecioId, 0)
                    ' Error
                    If Not respuestaDetalle.ConsultaExitosa Then
                        Return "InformarFaltantes - No se pudieron obtener los productos de la BD. " & vbCrLf & respuestaDetalle.mensaje
                    End If

                    Dim pos As Integer
                    Dim item As DetalleDocumento

                    ' Agrego la descripcion del producto
                    For Each detalle As DetalleDocumento In ListFaltantes
                        item = Nothing
                        pos = respuestaDetalle.rs.ToList.FindIndex(Function(element) element.ProductoId = detalle.ProductoId)
                        If pos >= 0 Then
                            item = respuestaDetalle.rs.ToList(pos)
                            detalle.Descripcion = item.Descripcion
                        End If
                    Next

                    Dim ListEmails As List(Of String) = respuestaCliente.Cliente.ListEmails

                    If ListEmails.Count > 0 Then
                        Dim cuerpoMail As String = "<p style='font-size: 14px' >El siguiente mail se generó de forma automática para informar los faltantes en su próximo pedido. Nro de Orden: " + nroOrden + "</p>"
                        cuerpoMail += "<p style='font-size: 14px' > A continuación se detallan los productos: </p>"
                        cuerpoMail += "<table style='font-size:14px'>"
                        cuerpoMail += "<tr><td style='padding:10px;font-weight:bold;' >CODIGO</td><td style='padding:10px;font-weight:bold;' >DESCRIPCION</td>"
                        cuerpoMail += "<td style='padding:10px;font-weight:bold;' >CANT. PEDIDA</td><td style='padding:10px;font-weight:bold;' >CANT. FALTANTE</td></tr>" + vbCrLf


                        For Each detalle In ListFaltantes
                            cuerpoMail += "<tr>"
                            cuerpoMail += "<td style='padding:10px;' >" + detalle.ProductoId + "</td>"
                            cuerpoMail += "<td style='padding:10px;' >" + detalle.Descripcion + "</td>"
                            cuerpoMail += "<td style='padding:10px;text-align:right' >" + detalle.CantKilogramos.ToString() + "</td>"
                            cuerpoMail += "<td style='padding:10px;text-align:right' >" + detalle.CantUnidades.ToString() + "</td>"
                            cuerpoMail += "</tr>"
                        Next

                        cuerpoMail += "</table>"
                        cuerpoMail += "<p style='font-size: 14px' > Disculpe las molestias ocasionadas. </p>"

                        Dim respuesta As Boolean = EnviarMail(ListEmails,
                                                                "Informe Faltantes - " & respuestaCliente.Cliente.RazonSocial & " - " & DateTime.Now.ToString("dd/MM/yyyy"),
                                                                cuerpoMail)

                        If Not respuesta Then Return "InformarFaltantes - Message: No se pudo enviar el mail de faltantes."
                    End If
                Else
                    Return "InformarFaltantes - Message: No se pudo obtener al cliente de la BD. " & vbCrLf & respuestaCliente.mensaje
                End If
            Catch ex As Exception
                Return "InformarFaltantes - Error armando el cuerpo del mail. "
            End Try
        End If


        Return ""
    End Function

    Public Sub ClearLog()
        LogEvent.Clear()
        LACommon.Query.Clear()
    End Sub

    'Esta funcion es para eliminar de memoria el excel
    Public Sub ReleaseObject(ByVal obj As Object)
        Try
            Runtime.InteropServices.Marshal.ReleaseComObject(obj)
            obj = Nothing
        Catch ex As Exception
            obj = Nothing
        Finally
            GC.Collect()
        End Try
    End Sub

    Public Sub End_Excel_App(datestart As Date, dateEnd As Date)
        Try
            Dim xlp() As Process = Process.GetProcessesByName("EXCEL")
            For Each Process As Process In xlp
                If Process.StartTime >= datestart And Process.StartTime <= dateEnd Then
                    Process.Kill()
                    Exit For
                End If
            Next
        Catch ex As Exception
            Dim strError As String = "End_Excel_App - Message: " + ex.Message + vbCrLf + JsonConvert.SerializeObject(ex)
            'WriteLogFile(strError)
        End Try
    End Sub

    Public Function GetListMeses() As List(Of ComboItem)
        Dim ListMeses As New List(Of LAFunctions.ComboItem)
        ListMeses.Add(New LAFunctions.ComboItem("Enero", 1))
        ListMeses.Add(New LAFunctions.ComboItem("Febrero", 2))
        ListMeses.Add(New LAFunctions.ComboItem("Marzo", 3))
        ListMeses.Add(New LAFunctions.ComboItem("Abril", 4))
        ListMeses.Add(New LAFunctions.ComboItem("Mayo", 5))
        ListMeses.Add(New LAFunctions.ComboItem("Junio", 6))
        ListMeses.Add(New LAFunctions.ComboItem("Julio", 7))
        ListMeses.Add(New LAFunctions.ComboItem("Agosto", 8))
        ListMeses.Add(New LAFunctions.ComboItem("Septiembre", 9))
        ListMeses.Add(New LAFunctions.ComboItem("Octubre", 10))
        ListMeses.Add(New LAFunctions.ComboItem("Noviembre", 11))
        ListMeses.Add(New LAFunctions.ComboItem("Diciembre", 12))
        Return ListMeses
    End Function

End Class
