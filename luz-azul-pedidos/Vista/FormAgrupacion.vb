Imports LAFunctions.LuzAzulPedidos

Public Class FormAgrupacion
    Private controlador As Controlador
    Dim CurrentAgrupacion As Agrupacion

    Private Sub BtnSalir_Click(sender As Object, e As EventArgs) Handles BtnSalir.Click
        Me.Close()
    End Sub

    Private Sub FormAgrupacion_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim pos As Integer

        controlador = Controlador.GetInstancia()

        'Busco la agrupacion actual
        pos = controlador.GetPosicionAgrupacion(controlador.GetCurrentAgrupacionId())
        If pos >= 0 Then
            CurrentAgrupacion = controlador.GetCurrentAgrupacion()
            LblAgrupacion.Text = CurrentAgrupacion.Descripcion
            LblUnidad.Text = "Unidad de pedido: " + CurrentAgrupacion.Unidad
            LblUnidadesXBulto.Text = "Unidades por bulto: " + CurrentAgrupacion.UnidadesPorBulto.ToString()
        End If

        Dim ListProductoAgrupaciones As List(Of DetalleProductoAgrupacion) = controlador.GetListProductosAgrupacion()
        If ListProductoAgrupaciones.Count > 0 Then
            DbGridAgrupacion.DataSource = ListProductoAgrupaciones

            DbGridAgrupacion.Columns("AgrupacionId").Visible = False
            DbGridAgrupacion.Columns("ProductoId").ReadOnly = True
            DbGridAgrupacion.Columns("Descripcion").AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            DbGridAgrupacion.Columns("Descripcion").ReadOnly = True
        Else
            ' No hay registros en la grilla para el proveedor actual
            DbGridAgrupacion.DataSource = vbEmpty
        End If

        MostrarTotalUnidades()

    End Sub

    Private Sub BtnGuardar_Click(sender As Object, e As EventArgs) Handles BtnGuardar.Click
        Dim pos As Integer
        pos = controlador.GetPosicionProveedorPedido()
        If pos >= 0 Then
            Dim totalUnidades As Double = CalcularTotalUnidades()
            Dim resto As Double = totalUnidades Mod CurrentAgrupacion.UnidadesPorBulto
            If resto = 0 Then
                controlador.GrabarAgrupacion(DbGridAgrupacion)
                Me.Close()
            Else
                MsgBox("El total de unidades ingresado debe ser multiplo de " + CurrentAgrupacion.UnidadesPorBulto.ToString(), vbCritical + vbOKOnly, GlobalSettings.TituloMensajes)
            End If
        End If
    End Sub
    Private Sub DbGridAgrupacion_EditingControlShowing(sender As Object, e As DataGridViewEditingControlShowingEventArgs) Handles DbGridAgrupacion.EditingControlShowing
        'verifico si la celda cantidad es numerico
        If DbGridAgrupacion.CurrentCell.ColumnIndex = 2 Then AddHandler CType(e.Control, TextBox).KeyPress, AddressOf TextBox_keyPress1

    End Sub
    Private Sub TextBox_keyPress1(ByVal sender As Object, ByVal e As KeyPressEventArgs)
        If Not (Char.IsDigit(CChar(CStr(e.KeyChar))) Or e.KeyChar = ChrW(Keys.Back)) Then e.Handled = True
    End Sub
    Private Function CalcularTotalUnidades() As Double
        Dim TotalUnidades As Double = 0
        For CurrentRow As Integer = 0 To DbGridAgrupacion.Rows.Count - 1
            TotalUnidades += Convert.ToInt32(DbGridAgrupacion.Rows(CurrentRow).Cells("Cantidad").Value)
        Next
        CalcularTotalUnidades = TotalUnidades
    End Function
    Private Sub MostrarTotalUnidades()
        LblTotalUnidades.Text = "Total Unidades: " + CalcularTotalUnidades().ToString()
    End Sub

    Private Sub DbGridAgrupacion_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) Handles DbGridAgrupacion.CellEndEdit
        MostrarTotalUnidades()
    End Sub
End Class