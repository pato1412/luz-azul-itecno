Imports LAFunctions.LuzAzulPedidos

Public Class frmobservaciones
    Private controlador As Controlador
    Private Sub BtnSalir_Click(sender As Object, e As EventArgs) Handles BtnSalir.Click
        Me.Close()
    End Sub

    Private Sub BtnGuardar_Click(sender As Object, e As EventArgs) Handles BtnGuardar.Click
        controlador.SetCurrentObservaciones(txtobservaciones.Text)
        Me.Close()
    End Sub

    Private Sub frmobservaciones_Load(sender As Object, e As EventArgs) Handles Me.Load
        'Obtengo la instancia del controlador
        controlador = Controlador.GetInstancia()
        txtobservaciones.Text = controlador.GetCurrentObservaciones()
        Me.Text = controlador.GetCurrentDescripcion()
    End Sub
End Class