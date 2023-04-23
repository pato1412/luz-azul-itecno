Imports Newtonsoft.Json
Imports LAFunctions.LuzAzulCommon

Public Class frmEstablecimientos
    Private controlador As Controlador
    Private Sub frmEstablecimientos_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim respuestaEstablecimiento As ResponseEstablecimiento

        'Obtengo la instancia del controlador
        controlador = Controlador.GetInstancia()

        If Controlador.GetMostrarFrameDepositos() Then
            frameDepositos.Visible = True
        Else
            frameDepositos.Visible = False
        End If

        respuestaEstablecimiento = controlador.GetEstablecimientosUsuario(Controlador.GetUsuarioId())
        If (respuestaEstablecimiento.ConsultaExitosa) Then
            If respuestaEstablecimiento.rs.Count = 1 Then
                'Dim ControladorPedidos As luz_azul_pedidos.Controlador
                'ControladorPedidos = luz_azul_pedidos.Controlador.GetInstancia()

                'tengo un solo establecimiento por lo que no muestro el frm de estableciemientos
                'luz_azul_pedidos.Controlador.SetCurrentEstablecimiento(respuestaEstablecimiento.rs.First)

                Me.Close()
            Else
                Controlador.SetListEstablecimientos(respuestaEstablecimiento.rs)

                'cargo el combo
                ComboEstablecimientos.ValueMember = "Value"
                ComboEstablecimientos.DisplayMember = "Text"

                For Each Est As Establecimiento In respuestaEstablecimiento.rs
                    ComboEstablecimientos.Items.Add(New LAFunctions.ComboItem(Est.Descripcion, Est.EstablecimientoId))
                Next
                ComboEstablecimientos.SelectedIndex = 0
            End If
        Else
            'Ocurrio un error al obtener el o los establecimientos
            MsgBox(respuestaEstablecimiento.mensaje)
        End If


    End Sub

    Private Sub BtnIngresar_Click(sender As Object, e As EventArgs) Handles BtnIngresar.Click
        'guardo en la configuracion global el establecimiento seleccionado
        Dim SelectedEstablecimiento As Establecimiento = New Establecimiento(ComboEstablecimientos.SelectedItem.Value, ComboEstablecimientos.SelectedItem.Text, False)


        Dim CurrentEstablecimientoId As String = ComboEstablecimientos.SelectedItem.Value
        Dim pos As Integer = controlador.GetListEstablecimientos().FindIndex(Function(element) element.EstablecimientoId = CurrentEstablecimientoId)
        If pos >= 0 Then
            Dim CurrentEstablecimiento As Establecimiento = controlador.GetListEstablecimientos()(pos)
            SelectedEstablecimiento.EsPropio = CurrentEstablecimiento.EsPropio
        End If

        Controlador.SetCurrentEstablecimiento(SelectedEstablecimiento)

        Controlador.SetLogEvent("Se ha seleccionado el establecimiento " + ComboEstablecimientos.SelectedItem.Text)


        If Controlador.GetMostrarFrameDepositos() Then
            Controlador.SetMostrarTodosDepositos(optTodosDepositos.Checked)
        End If

        Me.Close()

    End Sub

    Private Sub BtnSalir_Click(sender As Object, e As EventArgs) Handles BtnSalir.Click
        Me.Close()
    End Sub

End Class