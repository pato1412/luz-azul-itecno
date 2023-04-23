Imports Newtonsoft.Json
Imports LAFunctions.LuzAzulCommon

Public Class frmEstablecimientosMultiple
    Private controlador As Controlador
    Private Sub frmEstablecimientos_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim respuestaEstablecimiento As ResponseEstablecimiento

        'Obtengo la instancia del controlador
        controlador = Controlador.GetInstancia()

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

                'cargo el list de checkboxes
                chkListEstablecimientos.ValueMember = "value"
                chkListEstablecimientos.DisplayMember = "Text"

                For Each Est As LAFunctions.LuzAzulCommon.Establecimiento In respuestaEstablecimiento.rs
                    chkListEstablecimientos.Items.Add(New LAFunctions.ComboItem(Est.Descripcion, Est.EstablecimientoId))
                Next

            End If
        Else
            'Ocurrio un error al obtener el o los establecimientos
            MsgBox(respuestaEstablecimiento.mensaje)
        End If


    End Sub

    Private Sub BtnIngresar_Click(sender As Object, e As EventArgs) Handles BtnIngresar.Click
        'guardo en la configuracion global el establecimiento seleccionado

        For i = 0 To chkListEstablecimientos.CheckedItems.Count - 1
            ' Guardo en la configuracion global los depositos seleccionados
            Dim CurrentEstablecimientoId As String = chkListEstablecimientos.CheckedItems(i).Value
            Dim pos As Integer = controlador.GetListEstablecimientos().FindIndex(Function(element) element.EstablecimientoId = CurrentEstablecimientoId)
            If pos >= 0 Then
                Dim CurrentEstablecimiento As Establecimiento = controlador.GetListEstablecimientos()(pos)
                Controlador.AddListEstablecimientoSeleccionados(CurrentEstablecimiento)
            End If
        Next

        Controlador.SetLogEvent("Se ha seleccionado los establecimientos " + JsonConvert.SerializeObject(controlador.GetListEstablecimientosSeleccionados()))

        Me.Close()

    End Sub

    Private Sub BtnSalir_Click(sender As Object, e As EventArgs) Handles BtnSalir.Click
        Me.Close()
    End Sub

End Class