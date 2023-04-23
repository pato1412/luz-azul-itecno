Imports LAFunctions.LuzAzulCommon
Imports Newtonsoft.Json

Public Class frmDepositos
    Private controlador As Controlador
    Private Sub BtnSalir_Click(sender As Object, e As EventArgs) Handles BtnSalir.Click
        Me.Close()
    End Sub

    Private Sub frmDepositos_Load(sender As Object, e As EventArgs) Handles Me.Load

        Dim respuestaDepositos As ResponseDeposito

        'Obtengo la instancia del controlador
        controlador = Controlador.GetInstancia()

        ' Obtengo los depositos del usuario
        respuestaDepositos = controlador.GetDepositosUsuario()

        If (respuestaDepositos.ConsultaExitosa = True) Then
            If respuestaDepositos.rs.Count = 1 Then
                'Si solo tengo un deposito asociado al usuario y el establecmiento, no muestro este popup
                Controlador.AddListDepositoSeleccionados(respuestaDepositos.rs.First)
                Me.Close()
            Else
                ChkListDepositos.ValueMember = "Value"
                ChkListDepositos.DisplayMember = "Text"
                For Each Dep As Deposito In respuestaDepositos.rs
                    ChkListDepositos.Items.Add(New LAFunctions.ComboItem(Dep.Descripcion, Dep.DepositoId))
                Next
                ' Selecciono el primer deposito por defecto
                ChkListDepositos.SetItemChecked(0, True)
            End If
        End If

    End Sub

    Private Sub btnSeleccionar_Click(sender As Object, e As EventArgs) Handles btnSeleccionar.Click

        For i = 0 To ChkListDepositos.CheckedItems.Count - 1
            ' Guardo en la configuracion global los depositos seleccionados
            Dim CurrentDepositoId As String = ChkListDepositos.CheckedItems(i).Value
            Dim pos As Integer = controlador.GetListDepositos().FindIndex(Function(element) element.DepositoId = CurrentDepositoId)
            If pos >= 0 Then
                Dim CurrentDeposito As Deposito = controlador.GetListDepositos()(pos)
                Controlador.AddListDepositoSeleccionados(CurrentDeposito)
            End If
        Next

        Controlador.SetLogEvent("Se ha seleccionado los depositos " + JsonConvert.SerializeObject(controlador.GetListDepositosSeleccionados()))

        Me.Close()
    End Sub
End Class