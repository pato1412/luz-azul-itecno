Imports LAFunctions.LuzAzulPedidos
Imports Newtonsoft.Json

Public Class frmMain
    Private controlador As Controlador

    Private Sub frmMain_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'Inicio el controlador
        controlador = Controlador.GetInstancia()

        'llenar el combo de acciones multiples
        LlenarcomboAccionesMultiple()

        'voy a centrar el frame con la ventana
        FrameEnvio.Left = (Me.Width / 2) - (FrameEnvio.Width / 2)
        FrameEnvio.Visible = False

        BtnAnterior.Visible = False

        LblEstablecimiento.Text = controlador.GetLabelEstablecimiento()
        LblDepositos.Text = controlador.GetLabelDeposito()

        'verifico si tengo permisos sobre el directorio de los archivos excel
        If Not controlador.ObtenerDirectorioArchivosExcel() Then
            MsgBox("El sistema no tiene permisos para crear los directorios necesarios, por favor comuniquese con el administrador", vbCritical + vbOKOnly, GlobalSettings.TituloMensajes)
            Me.Close()
        End If


        'verifico que los proveedores tengan por lo menos un mail asociado
        If Not controlador.ValidarMailsProveedores() Then
            Dim strMessage = "La configuracion de mails de los proveedores obtenida de TEMPO no es la correcta, compruebe la sincronizacion. " + vbCrLf + "El sistema no puede continuar, por favor comuniquese con el administrador."
            MsgBox(strMessage, vbCritical + vbOKOnly, GlobalSettings.TituloMensajes)
            Me.Close()
        End If

    End Sub

    Private Sub frmMain_Shown(sender As Object, e As EventArgs) Handles Me.Shown

        LblEstado.Text = "Cargando datos proveedor..."
        BtnSiguiente.Enabled = False
        Application.DoEvents()

        controlador.GenerarAgrupaciones()
        Application.DoEvents()

        controlador.CargarProductosOpcionales()
        Application.DoEvents()

        controlador.IniciarPedidoProveedores()
        Application.DoEvents()

        'validarAgrupaciones()
        LblEstado.Text = "Listo"
        Application.DoEvents()

        CargarDatosProveedor()
        BtnSiguiente.Enabled = True
    End Sub

    Private Sub BtnSiguiente_Click(sender As Object, e As EventArgs) Handles BtnSiguiente.Click
        Dim PermiteGuardar As Boolean = True

        'verifico si modifico alguna de las cantidades sugeridas
        If controlador.VerificarCantidades(DbGridProveedores) Then
            If MsgBox("Haz modificado las cantidades sugeridas de uno o mas articulos, deseas continuar?", MsgBoxStyle.Question + MsgBoxStyle.YesNo, GlobalSettings.TituloMensajes) = MsgBoxResult.No Then
                PermiteGuardar = False
            End If
        End If

        'verifico si los kilogramos pedidos superan el minimo del proveedor
        If controlador.validarPesoPedido(DbGridProveedores) = False Then
            Dim peso As Integer = controlador.GetKilosMinProveedor()
            If MsgBox("El pedido no supera los " & Format(peso, "#0.00") & " Kg., que es el peso minimo requerido por el proveedor." + vbCrLf + "Debe completar el peso minimo para enviarlo o en caso contrario de continuar con los proximos proveedores este proveedor no se enviará en el pedido ", vbExclamation + vbOKCancel, GlobalSettings.TituloMensajes) = MsgBoxResult.Cancel Then
                PermiteGuardar = False
            End If
        End If

        'valido las agrupaciones del proveedor
        If controlador.ValidarAgrupacionesProveedor() = False Then
            MsgBox("Las cantidad ingresada en alguna agrupacion no es multiplo de las unidades por bulto, por favor corrija las cantidades", MsgBoxStyle.Exclamation, GlobalSettings.TituloMensajes)
            PermiteGuardar = False
        End If

        If PermiteGuardar Then
            'grabo los datos del pedido actual
            controlador.GuardarPedidoProveedor(DbGridProveedores, chkceros.Checked, chkModificados.Checked)

            'Paso al siguiente proveedor
            controlador.PunteroSiguiente()


            CargarDatosProveedor()

            If controlador.GetPuntero > 0 Then
                BtnAnterior.Visible = True
            End If
        End If
    End Sub
    Private Sub BtnAnterior_Click(sender As Object, e As EventArgs) Handles BtnAnterior.Click
        'grabo los datos del pedido actual
        controlador.GuardarPedidoProveedor(DbGridProveedores, chkceros.Checked, chkModificados.Checked)

        'Paso al proveedor anterior
        controlador.PunteroAnterior()

        CargarDatosProveedor()

        BtnSiguiente.Text = "Siguiente >"

        If controlador.GetPuntero() = 0 Then
            BtnAnterior.Visible = False
        End If
    End Sub


    Private Sub CargarDatosProveedor()
        Controlador.SetLogEvent("Cargar Datos Proveedor " + controlador.GetPuntero().ToString())

        If (controlador.GetPuntero() = controlador.GetCantProveedores()) Then
            ' Es el ultimo pedido por lo que verifico si se modificaron las cantidades de al menos uno
            Dim ConfirmaCantidades As Boolean = True
            If controlador.VerificarCantidadesPedido() Then
                If MsgBox("Haz modificado las cantidades sugeridas de uno o mas articulos en los pedidos realizados, deseas continuar?" & vbCrLf &
                          "-------------------------------------------------------------------------" & vbCrLf &
                          controlador.GetListProveedoresModificados(), MsgBoxStyle.Question + MsgBoxStyle.YesNo, GlobalSettings.TituloMensajes) = MsgBoxResult.No Then
                    ConfirmaCantidades = False
                End If
            Else
                'si no se modificaron las cantidades de ningun igual muestro un mensaje de confirmacion
                If MsgBox("Esta a punto de enviar el pedido con los datos seleccionados, esta seguro que desea continuar?", MsgBoxStyle.Question + MsgBoxStyle.YesNo, GlobalSettings.TituloMensajes) = MsgBoxResult.No Then
                    ConfirmaCantidades = False
                End If
            End If

            If ConfirmaCantidades Then
                If CheckForInternetConnection() Then

                    'Elimino del pedido los proveedores que no cumplan con el minimo de peso requerido
                    controlador.EliminarProveedoresPesoInsuficiente()

                    If controlador.ValidarCantidadesPedido() Then
                        Me.Cursor = Cursors.WaitCursor
                        BtnSiguiente.Enabled = False
                        BtnAnterior.Enabled = False

                        'grabo en el webservice el pedido actual
                        If FinalizarPedido() Then
                            Me.Cursor = Cursors.Default
                            Dim strMsg As String
                            If controlador.GetErrorEnvioMails() Then
                                If controlador.GetErrorGeneracionExcel() Then
                                    'fallaron ambos procesos
                                    strMsg = "El Pedido Nro " + controlador.GetNroPedido() + " ha sido enviado con exito. " + vbCrLf + "Sin embargo hubo errores en la generacion de los archivos y el envio de mails por lo que no recibira el mail de confirmacion, por favor comuniquese con el proveedor para verificar el pedido"
                                Else
                                    'fallo solo el envio de mails
                                    strMsg = "El Pedido Nro " + controlador.GetNroPedido() + " ha sido enviado con exito. " + vbCrLf + "Sin embargo hubo errores en el envio de mails por lo que no recibira el mail de confirmacion, por favor comuniquese con el proveedor para verificar el pedido"
                                End If
                            Else
                                If controlador.GetErrorGeneracionExcel Then
                                    'fallo la generacion de los excels
                                    strMsg = "El Pedido Nro " + controlador.GetNroPedido() + " ha sido enviado con exito. " + vbCrLf + "Sin embargo hubo errores en la generacion de los archivos y el envio de mails por lo que no recibira el mail de confirmacion, por favor comuniquese con el proveedor para verificar el pedido"
                                Else
                                    'No hubo errores
                                    strMsg = "Pedido Nro " + controlador.GetNroPedido() + " enviado con exito, muchas gracias"
                                End If
                            End If
                            MsgBox(strMsg, vbInformation, GlobalSettings.TituloMensajes)
                            Me.Close()
                        Else
                            'Ocurrio un problema al finalizar el pedido
                            Me.Cursor = Cursors.Default
                            FrameEnvio.Visible = False
                            If MsgBox("Ocurrio un problema al finalizar el pedido, si lo desea puede reintentar enviarlo o cerrar la aplicacion", vbRetryCancel, GlobalSettings.TituloMensajes) = MsgBoxResult.Retry Then
                                ProgressBarEnvio.Value = 0
                                CargarDatosProveedor()
                            Else
                                Me.Close()
                            End If
                        End If
                    Else
                        'El pedido esta vacio
                        Me.Cursor = Cursors.Default
                        FrameEnvio.Visible = False
                        MsgBox("El pedido generado no cumple con el peso o sus cantidades son vacias por lo que no se ha enviado ningun pedido al servidor")
                        Me.Close()
                    End If
                Else
                    'NO hay conexion a internet
                    controlador.PunteroAnterior()
                    MsgBox("En este momento no hay conexion a intenet, por favor intentelo mas tarde", vbCritical + vbOK, GlobalSettings.TituloMensajes)
                End If
            Else
                'SI no acepta haber modificado las cantidades
                controlador.PunteroAnterior()
            End If
        Else
            LblNroProveedor.Text = "Proveedor " + (controlador.GetPuntero() + 1).ToString() + " de " + controlador.GetCantProveedores().ToString()


            'Marco en el controlador el proveedor actual
            controlador.SetCurrentProveedor()

            Dim currentProveedor As Clasificacion = controlador.GetCurrentProveedor()
            LblProveedor.Text = currentProveedor.DescripcionSinPrefijo

            'Lleno la grilla con los datos del proveedor actual
            chkceros.Checked = controlador.GetMostrarCeros()
            chkModificados.Checked = controlador.GetMostrarModificados()

            Dim ListProductos As List(Of DetallePedido) = controlador.GetDatosCurrentProveedor()
            If ListProductos.Count > 0 Then

                DbGridProveedores.DataSource = ListProductos
                CargarEstilosGrilla()
                IniciarEstilosGrilla()
            Else
                ' No hay registros en la grilla para el proveedor actual
                DbGridProveedores.DataSource = vbEmpty
            End If

            'reseteo el combo de acciones
            cmbAcciones.SelectedIndex = 0

            If (controlador.GetPuntero() = controlador.GetCantProveedores() - 1) Then BtnSiguiente.Text = "Finalizar"
        End If
    End Sub
    Private Function FinalizarPedido() As Boolean
        Dim respuesta As ResponseWS
        Dim respuesta2 As ResponseWS
        Dim cantProveedores As Integer
        Dim n As Integer = 1

        ' Vacio la grilla antes de eliminar los registros vacio porque sino da error
        DbGridProveedores.DataSource = vbEmpty
        FrameEnvio.Visible = True

        ' Recorro todo el pedido y elimino los registros en donde las cantidades son cero
        controlador.EliminarRegistrosVacios()

        cantProveedores = controlador.GetCantProveedores + 3
        LblEstado.Text = "Generando nuevo pedido..."
        Application.DoEvents()

        ' Hago el POST al webservice para almacenar el pedido en la nube
        respuesta = controlador.WSNuevoPedido()
        If respuesta.ConsultaExitosa = True Then
            ' Si el primer post me devolvio un nro de pedido lo almaceno y envio el detalle del mismo
            controlador.SetNroPedido(respuesta.mensaje)
            ProgressBarEnvio.Maximum = 100
            ProgressBarEnvio.Value = (n * 100) / cantProveedores
            Application.DoEvents()

            ' Envio el detalle al servidor web de cada proveedor
            For Each Detalle As PedidoProveedor In controlador.GetListProveedoresPedido()
                respuesta = controlador.WSDetallePedido(Detalle)
                If respuesta.ConsultaExitosa = False Then
                    'Si al hacer el intento de enviar al webservice y da error escribo un log
                    Dim strError As String = "Ocurrio un error al enviar los datos al webservice :  " + vbCrLf + JsonConvert.SerializeObject(Detalle)
                    Controlador.WriteLogFile(strError)
                    controlador.EnviarMailCritico(strError)
                    respuesta2 = controlador.WSDetallePedido(Detalle)
                    If respuesta2.ConsultaExitosa = False Then
                        Dim strError2 As String = "Ocurrio un error al Reenviar los datos al webservice :  " + vbCrLf + JsonConvert.SerializeObject(Detalle)
                        Controlador.WriteLogFile(strError2)
                        MsgBox("Ocurrio un error al enviar los datos al servidor de internet.", vbCritical + vbOKOnly, GlobalSettings.TituloMensajes)
                        EliminarPedidoWS()
                        Return False
                    End If
                End If
                n += 1
                ProgressBarEnvio.Value = (n * 100) / cantProveedores
                LblEstado.Text = "Grabando datos proveedor " + Detalle.proveedorNombre + "..."
                Controlador.SetLogEvent(LblEstado.Text)
                Application.DoEvents()
            Next

            ' Genero el archivo Excel del pedido
            LblEstado.Text = "Generando Excel..."
            If controlador.GenerarArchivosExcel() Then
                n += 1
                ProgressBarEnvio.Value = (n * 100) / cantProveedores

                'envio el mail con el detalle del pedido y el archivo adjunto
                LblEstado.Text = "Enviando mails... "
                Application.DoEvents()

                If controlador.EnviarMails() Then
                    'si todos los mails se enviaron bien marco en el webservice y elimino los archivos excel
                    respuesta = controlador.WSSetMailsEnviados()
                    If respuesta.ConsultaExitosa Then
                        controlador.EliminarArchivosProveedores()
                    End If
                Else
                    'Como el pedido ya lo tengo completo en el webservice no necesito eliminar el pedido
                    'EliminarPedidoWS()
                    controlador.SetErrorEnvioMails(True)
                    Return True
                End If

                n += 1
                ProgressBarEnvio.Value = (n * 100) / cantProveedores
                LblEstado.Text = "Listo"
                Controlador.SetLogEvent(LblEstado.Text)
            Else
                'Como el pedido ya lo tengo completo en el webservice no necesito eliminar el pedido
                'EliminarPedidoWS()
                'Si la generacion de los excel dio error muestro un mensaje pero dejo continuar el proceso para no tener que enviar el pedido nuevamente
                controlador.SetErrorGeneracionExcel(True)
                Return True
            End If
        Else
            'si ocurrio un error muestro el mensaje
            MsgBox(respuesta.mensaje, vbCritical + vbOK, GlobalSettings.TituloMensajes)
            Return False
        End If
        Return True
    End Function

    Private Sub EliminarPedidoWS()
        Dim respuesta As ResponseWS
        Controlador.SetLogEvent("Eliminar pedido Nro " + controlador.GetNroPedido() + " del webservice, porque el ocurrio un error")
        respuesta = controlador.WSEliminarPedido()
        If respuesta.ConsultaExitosa = True Then
            Controlador.SetLogEvent("Pedido eliminado con exito")
        Else
            Controlador.SetLogEvent("No se pudo eliminar el pedido del servidor")
        End If
    End Sub

    Private Sub LlenarcomboAccionesMultiple()
        cmbAcciones.DisplayMember = "Text"
        cmbAcciones.ValueMember = "Value"
        cmbAcciones.Items.AddRange(controlador.GetListAccionesMultiples().ToArray())
        cmbAcciones.SelectedIndex = 0
    End Sub

    Private Sub SetEstiloCelda(nroRowGrilla As Integer)
        Dim highlightCellStyle As New DataGridViewCellStyle
        Dim pos As Integer

        Dim styleBold As New DataGridViewCellStyle With {
            .Font = New Font(DbGridProveedores.Font, FontStyle.Bold)
        }

        Dim styleItalic As New DataGridViewCellStyle With {
            .Font = New Font(DbGridProveedores.Font, FontStyle.Italic)
        }

        'Si el producto es opcional lo escribo en cursiva
        If controlador.CheckProductoOpcional(DbGridProveedores.Rows(nroRowGrilla).Cells("Codigo").Value) Then
            DbGridProveedores.Rows(nroRowGrilla).Cells("Codigo").Style = styleItalic
            DbGridProveedores.Rows(nroRowGrilla).Cells("Descripcion").Style = styleItalic
            Dim Descripcion As String = DbGridProveedores.Rows(nroRowGrilla).Cells("Descripcion").Value
            If Descripcion.Substring(0, 1) <> "*" Then
                Descripcion = "* " + Descripcion
            End If
            DbGridProveedores.Rows(nroRowGrilla).Cells("Descripcion").Value = Descripcion
        End If


            If (IsNumeric(DbGridProveedores.Rows(nroRowGrilla).Cells("CantidadPedida").Value)) Then
            Dim CantPedida As Double = Convert.ToDouble(DbGridProveedores.Rows(nroRowGrilla).Cells("CantidadPedida").Value)
            Dim CantidadSugerida As Double = Convert.ToDouble(DbGridProveedores.Rows(nroRowGrilla).Cells("CantidadSugerida").Value)

            'No se permiten cantidades negativas
            If CantPedida < 0 Then
                DbGridProveedores.Rows(nroRowGrilla).Cells("CantidadPedida").Value = 0
                CantPedida = 0
            End If

            If CantPedida = CantidadSugerida Then
                highlightCellStyle.ForeColor = Color.Black
            Else
                'verifico si el rango es menor al 5%
                If CantPedida < (CantidadSugerida * 1.05) And CantPedida > (CantidadSugerida * 0.95) Then
                    highlightCellStyle.ForeColor = Color.DarkGreen
                Else
                    highlightCellStyle.ForeColor = Color.DarkRed

                End If
            End If
            DbGridProveedores.Rows(nroRowGrilla).DefaultCellStyle = highlightCellStyle
            If chkceros.Checked = False And CantPedida = 0 Then
                Dim currencyManager1 As CurrencyManager = BindingContext(DbGridProveedores.DataSource)
                currencyManager1.SuspendBinding()
                DbGridProveedores.Rows(nroRowGrilla).Visible = False
                currencyManager1.ResumeBinding()
            Else
                DbGridProveedores.Rows(nroRowGrilla).Visible = True
            End If

            If chkModificados.Checked = True Then
                If CantPedida <> CantidadSugerida Then
                    DbGridProveedores.Rows(nroRowGrilla).Visible = True
                Else
                    Dim currencyManager1 As CurrencyManager = BindingContext(DbGridProveedores.DataSource)
                    currencyManager1.SuspendBinding()
                    DbGridProveedores.Rows(nroRowGrilla).Visible = False
                    currencyManager1.ResumeBinding()
                End If
            End If

        Else
            MsgBox("Solo se permiten numeros", vbInformation + vbOKOnly, GlobalSettings.TituloMensajes)
            DbGridProveedores.Rows(nroRowGrilla).Cells("CantidadPedida").Value = 0
        End If

        'Si el registro actual pertenece a alguna agrupacion cambio el color de la fuente
        If DbGridProveedores.Rows(nroRowGrilla).Cells("AgrupacionId").Value <> "" Then
            Dim FontColor As String = GlobalSettings.DefaulColorAgrupacion
            pos = controlador.GetPosicionAgrupacion(DbGridProveedores.Rows(nroRowGrilla).Cells("AgrupacionId").Value)
            If pos >= 0 Then
                Dim AgrupacionActual = controlador.GetAgrupacion(pos)
                If AgrupacionActual.ColorAgrupacion <> "" Then
                    FontColor = AgrupacionActual.ColorAgrupacion
                End If
            End If
            FontColor = FontColor.Substring(1) 'elimino el numeral
            Dim ArrayColors() As String = FontColor.Split(",")
            If ArrayColors.Length = 3 Then
                highlightCellStyle.BackColor = Color.FromArgb(CInt(ArrayColors(0)), CInt(ArrayColors(1)), CInt(ArrayColors(2)))
                DbGridProveedores.Rows(nroRowGrilla).DefaultCellStyle = highlightCellStyle
            End If
        End If

        'SI el stock o la venta proyectada fueran cero hay que marcarlos en negrita
        Dim ventaProyectada As Double = IIf(Double.TryParse(DbGridProveedores.Rows(nroRowGrilla).Cells("Venta").Value, ventaProyectada), ventaProyectada, 0)
        If (ventaProyectada = 0) Then
            DbGridProveedores.Rows(nroRowGrilla).Cells("Venta").Style = styleBold
        End If
        Dim stock As Double = IIf(Double.TryParse(DbGridProveedores.Rows(nroRowGrilla).Cells("Stock").Value, stock), stock, 0)
        If (stock = 0) Then
            DbGridProveedores.Rows(nroRowGrilla).Cells("Stock").Style = styleBold
        End If


    End Sub
    Private Sub VisualizarPesoPedido()
        controlador = Controlador.GetInstancia()

        Dim TotalKgsPedido As Double
        TotalKgsPedido = controlador.CalcularPesoPedido(DbGridProveedores)
        LblKgsPedido.Text = "Kg. Seleccionados: " + TotalKgsPedido.ToString()
    End Sub

    Private Sub IniciarEstilosGrilla()
        ' Recorro toda la grilla para cargar los estilos de los productos cuya cantidad fue modificada
        For n As Integer = DbGridProveedores.Rows.Count - 1 To 0 Step -1
            SetEstiloCelda(n)
        Next

        VisualizarPesoPedido()
    End Sub
    Private Sub CargarEstilosGrilla()
        Dim styleBoldRight As New DataGridViewCellStyle With {
            .Font = New Font(DbGridProveedores.Font, FontStyle.Bold),
            .Alignment = DataGridViewContentAlignment.TopRight
        }

        Dim styleBold As New DataGridViewCellStyle With {
            .Font = New Font(DbGridProveedores.Font, FontStyle.Bold)
        }

        'oculto las celdas que son internas del pedido    
        DbGridProveedores.Columns("CantidadSugerida").Visible = False
        DbGridProveedores.Columns("KgPromedioUnidad").Visible = False
        DbGridProveedores.Columns("PrecioCompra").Visible = False
        DbGridProveedores.Columns("SePidePorBulto").Visible = False
        DbGridProveedores.Columns("SeVendePorPeso").Visible = False
        DbGridProveedores.Columns("AgrupacionId").Visible = False
        DbGridProveedores.Columns("Observaciones").Visible = False
        DbGridProveedores.Columns("TipoUnidadMedidaId2").Visible = False

        DbGridProveedores.Columns("Codigo").ReadOnly = True

        DbGridProveedores.Columns("Descripcion").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        DbGridProveedores.Columns("Descripcion").ReadOnly = True

        DbGridProveedores.Columns("Stock").ReadOnly = True
        DbGridProveedores.Columns("Stock").HeaderText = "Stock Actual"
        DbGridProveedores.Columns("Stock").DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight

        DbGridProveedores.Columns("Venta").ReadOnly = True
        DbGridProveedores.Columns("Venta").DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight

        DbGridProveedores.Columns("Packing").ReadOnly = True
        DbGridProveedores.Columns("Packing").HeaderText = "Stock Necesario"
        DbGridProveedores.Columns("Packing").DefaultCellStyle.Alignment = DataGridViewContentAlignment.TopRight

        DbGridProveedores.Columns("CantidadPedida").HeaderText = "Cant. a Pedir"
        DbGridProveedores.Columns("CantidadPedida").DefaultCellStyle = styleBoldRight

        DbGridProveedores.Columns("Unidad").ReadOnly = True
        DbGridProveedores.Columns("Unidad").HeaderText = "Unidad de Pedido"
        DbGridProveedores.Columns("Unidad").DefaultCellStyle = styleBold

        DbGridProveedores.Columns("UnidadesPorBulto").ReadOnly = True
        DbGridProveedores.Columns("UnidadesPorBulto").HeaderText = "Unidades por Bulto"
        DbGridProveedores.Columns("UnidadesPorBulto").DefaultCellStyle = styleBoldRight
    End Sub

    Private Sub chkceros_CheckedChanged(sender As Object, e As EventArgs) Handles chkceros.CheckedChanged
        IniciarEstilosGrilla()
    End Sub


    Private Sub DbGridProveedores_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) Handles DbGridProveedores.CellEndEdit
        controlador.ValidarCantidadesEnUnidadesPorBulto(DbGridProveedores, DbGridProveedores.CurrentRow.Index)
        SetEstiloCelda(DbGridProveedores.CurrentRow.Index)
        VisualizarPesoPedido()
    End Sub

    Private Sub RestaurarValorOriginalToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RestaurarValorOriginalToolStripMenuItem.Click

        If DbGridProveedores.CurrentRow IsNot Nothing Then
            Dim highlightCellStyle As New DataGridViewCellStyle
            Dim AgrupacionId As String = DbGridProveedores.CurrentRow.Cells("AgrupacionId").Value
            'verifico si la celda que voy a restaurar pertenece a una agrupacion
            If AgrupacionId <> "" Then
                For CurrentRow2 As Integer = 0 To DbGridProveedores.Rows.Count - 1
                    If DbGridProveedores.Rows(CurrentRow2).Cells("AgrupacionId").Value = AgrupacionId Then
                        DbGridProveedores.Rows(CurrentRow2).Cells("CantidadPedida").Value = DbGridProveedores.Rows(CurrentRow2).Cells("CantidadSugerida").Value
                        highlightCellStyle.BackColor = Color.White
                        DbGridProveedores.Rows(CurrentRow2).DefaultCellStyle = highlightCellStyle
                    End If
                Next
            End If

            DbGridProveedores.CurrentRow.Cells("CantidadPedida").Value = DbGridProveedores.CurrentRow.Cells("CantidadSugerida").Value
            highlightCellStyle.BackColor = Color.White
            DbGridProveedores.Rows(DbGridProveedores.CurrentRow.Index).DefaultCellStyle = highlightCellStyle
            IniciarEstilosGrilla()
            VisualizarPesoPedido()

        End If
    End Sub

    Private Sub DbGridProveedores_DataError(sender As Object, e As DataGridViewDataErrorEventArgs) Handles DbGridProveedores.DataError
        MsgBox("Ocurrio un error inesperado con los datos de la grilla" + vbCrLf + "Columna: " + e.ColumnIndex.ToString() + " - Fila: " + e.RowIndex.ToString(), vbCritical + vbOKOnly, GlobalSettings.TituloMensajes)
    End Sub
    Private Sub DbGridProveedores_EditingControlShowing(sender As Object, e As DataGridViewEditingControlShowingEventArgs) Handles DbGridProveedores.EditingControlShowing
        'SI el campo cantidad tiene una agrupacion no lo dejo editar
        If DbGridProveedores.CurrentCell.ColumnIndex = GlobalSettings.IndiceColumnaCantidadPedida And DbGridProveedores.CurrentRow.Cells("AgrupacionId").Value IsNot "" Then
            controlador.SetCurrentAgrupacionId(DbGridProveedores.CurrentRow.Cells("AgrupacionId").Value)
            Dim frmDialogue As New FormAgrupacion
            frmDialogue.ShowDialog()
            'Cuando termino de modificar el dialog hago el refresh de la grilla
            DbGridProveedores.Refresh()
            DbGridProveedores.ClearSelection()
            BtnSiguiente.Select()
            IniciarEstilosGrilla()
        End If

        'verifico si la celda cantidad es numerico
        If DbGridProveedores.CurrentRow IsNot Nothing Then
            If DbGridProveedores.CurrentCell.ColumnIndex = GlobalSettings.IndiceColumnaCantidadPedida Then AddHandler CType(e.Control, TextBox).KeyPress, AddressOf TextBox_keyPress1
        End If
    End Sub
    Private Sub TextBox_keyPress1(ByVal sender As Object, ByVal e As KeyPressEventArgs)
        If Not (Char.IsDigit(CChar(CStr(e.KeyChar))) Or e.KeyChar = ChrW(Keys.Back)) Then e.Handled = True
    End Sub

    Private Sub DbGridProveedores_CellPainting(sender As Object, e As DataGridViewCellPaintingEventArgs) Handles DbGridProveedores.CellPainting

        If IsGroupedRow(e.RowIndex) Then
            e.AdvancedBorderStyle.Bottom = DataGridViewAdvancedCellBorderStyle.None
        End If

    End Sub
    Private Function IsGroupedRow(currentRow As Integer) As Boolean
        'si las colomnas o filas son menores a 0 no las comparo
        If currentRow < 0 Or currentRow >= DbGridProveedores.RowCount - 1 Then
            Return False
        End If

        Dim valorRowAgrupacion As String = DbGridProveedores.Rows(currentRow).Cells("AgrupacionId").Value.ToString()
        Dim valorRowsiguiente As String = DbGridProveedores.Rows(currentRow + 1).Cells("AgrupacionId").Value.ToString()

        Return (valorRowAgrupacion = valorRowsiguiente And valorRowAgrupacion IsNot "")
    End Function
    Private Sub ToolStripMenuObservaciones_Click(sender As Object, e As EventArgs) Handles ToolStripMenuObservaciones.Click

        If DbGridProveedores.CurrentRow IsNot Nothing Then
            controlador.SetCurrentObservaciones(DbGridProveedores.CurrentRow.Cells("Observaciones").Value)
            controlador.SetCurrentDescripcion(DbGridProveedores.CurrentRow.Cells("Descripcion").Value)
            Dim frmDialogue As New frmobservaciones
            frmDialogue.ShowDialog()

            DbGridProveedores.CurrentRow.Cells("Observaciones").Value = controlador.GetCurrentObservaciones()
            'Cuando termino de modificar el dialog hago el refresh de la grilla
            DbGridProveedores.Refresh()
            DbGridProveedores.ClearSelection()
            BtnSiguiente.Select()
            IniciarEstilosGrilla()
        End If
    End Sub

    Private Sub chkModificados_CheckedChanged(sender As Object, e As EventArgs) Handles chkModificados.CheckedChanged
        IniciarEstilosGrilla()
    End Sub

    Private Sub LblEstablecimiento_DoubleClick(sender As Object, e As EventArgs) Handles LblEstablecimiento.DoubleClick
        MsgBox(controlador.ShowDialogDebug(), MsgBoxStyle.Information, GlobalSettings.TituloMensajes)
    End Sub

    Private Sub LblDepositos_DoubleClick(sender As Object, e As EventArgs) Handles LblDepositos.DoubleClick
        If MsgBox("Desea enviar un mail de pruebas?", MsgBoxStyle.YesNo, GlobalSettings.TituloMensajes) = MsgBoxResult.Yes Then
            controlador.enviarMailPrueba()
        End If
    End Sub

    Private Sub frmMain_Closed(sender As Object, e As EventArgs) Handles Me.Closed
        'Cuando cierro el formulario cierro la instancia del controlador
        controlador.FinalizarInstancia()
    End Sub

    Private Sub cmbAcciones_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbAcciones.SelectedIndexChanged

        'verifico si selecciono poner en cero las cantidades
        If cmbAcciones.SelectedIndex = GlobalSettings.ListAccionPonerACero Then
            If MsgBox("Desea poner todas las cantidades de este proveedor en cero?", MsgBoxStyle.YesNo, GlobalSettings.TituloMensajes) = MsgBoxResult.Yes Then
                For n As Integer = DbGridProveedores.Rows.Count - 1 To 0 Step -1
                    DbGridProveedores.Rows(n).Cells("CantidadPedida").Value = 0
                Next
                IniciarEstilosGrilla()
                cmbAcciones.SelectedIndex = 0
            End If
        End If

        'verifico si selecciono restaurar valores por defecto
        If cmbAcciones.SelectedIndex = GlobalSettings.ListAccionRestaurar Then
            If MsgBox("Desea restaurar las cantidades por las sugeridas por defecto?", MsgBoxStyle.YesNo, GlobalSettings.TituloMensajes) = MsgBoxResult.Yes Then
                Dim CantidadSugerida As Double
                For n As Integer = DbGridProveedores.Rows.Count - 1 To 0 Step -1
                    CantidadSugerida = Convert.ToDouble(DbGridProveedores.Rows(n).Cells("CantidadSugerida").Value)
                    DbGridProveedores.Rows(n).Cells("CantidadPedida").Value = CantidadSugerida
                Next
                IniciarEstilosGrilla()
                cmbAcciones.SelectedIndex = 0
            End If
        End If

    End Sub
End Class