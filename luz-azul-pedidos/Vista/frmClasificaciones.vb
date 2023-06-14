Imports LAFunctions.LuzAzulPedidos

Public Class frmClasificaciones
    Private controlador As Controlador

    Private Sub frmEstablecimiento_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim respuestaDepositos As ResponseDeposito
        Dim respuestaClasificacion As ResponseClasificaciones

        'Obtengo la instancia del controlador
        controlador = Controlador.GetInstancia()


        ' Obtengo los depositos del usuario
        respuestaDepositos = controlador.GetDepositosUsuario()

        If (respuestaDepositos.ConsultaExitosa = True) Then
            ChkListDepositos.ValueMember = "Value"
            ChkListDepositos.DisplayMember = "Text"
            For Each Dep As Deposito In respuestaDepositos.rs
                ChkListDepositos.Items.Add(New LAFunctions.ComboItem(Dep.Descripcion, Dep.DepositoId))
            Next
            ' Selecciono el primer deposito por defecto
            ChkListDepositos.SetItemChecked(0, True)
        End If


        ' Obtengo la lista de proveedores iniciales
        respuestaClasificacion = controlador.GetArbolClasificaciones(New List(Of String))

        If (respuestaClasificacion.ConsultaExitosa = True) Then
            ChkListProveedores.ValueMember = "Value"
            ChkListProveedores.DisplayMember = "Text"
            If respuestaClasificacion.rs IsNot Nothing Then
                For Each Clas As Clasificacion In respuestaClasificacion.rs
                    ChkListProveedores.Items.Add(New LAFunctions.ComboItem(Clas.DescripcionSinPrefijo, Clas.ClasificacionProdId))
                Next
            Else
                MsgBox("Ocurrio un error con el sincronizador de datos de TEMPO, por favor comuniquese con el administrador", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, GlobalSettings.TituloMensajes)
                Controlador.WriteLogFile("Ocurrio un error con el arbol de clasificaciones porque fallo el sincronizador de Datos")
            End If
        End If
    End Sub

    Private Sub BtnIngresar_Click(sender As Object, e As EventArgs) Handles BtnIngresar.Click

        lblMensaje.Visible = False

        If ChkListDepositos.CheckedItems.Count = 0 Then
            lblMensaje.Text = "Por favor seleccione al menos un deposito"
            lblMensaje.Visible = True
            Exit Sub
        End If

        If ChkListProveedores.CheckedItems.Count = 0 Then
            lblMensaje.Text = "Por favor seleccione al menos un proveedor"
            lblMensaje.Visible = True
            Exit Sub
        End If

        Dim i As Integer

        'Guardo los depositos seleccionados
        Dim ListDepositosSeleccionados As List(Of Deposito) = New List(Of Deposito)
        For i = 0 To ChkListDepositos.CheckedItems.Count - 1
            ListDepositosSeleccionados.Add(New Deposito(ChkListDepositos.CheckedItems(i).Value, ChkListDepositos.CheckedItems(i).Text))
        Next
        controlador.GuardarDepositos(ListDepositosSeleccionados)


        ' Guardo en una lista las clasificaciones seleccionadas
        Dim ListClasificaciones As List(Of String) = New List(Of String)
        For i = 0 To ChkListProveedores.CheckedItems.Count - 1
            ListClasificaciones.Add(ChkListProveedores.CheckedItems(i).Value)
        Next

        controlador.GuardarClasificaciones(ListClasificaciones)


        Dim frmMain = New frmMain
        frmMain.Show()
        Me.Close()
    End Sub

    Private Sub BtnSalir_Click(sender As Object, e As EventArgs) Handles BtnSalir.Click
        Me.Close()
    End Sub

    Private Sub frmClasificaciones_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        ' Verifico si puede utilizar la aplicacion siempre que no tenga deuda vencida
        'Dim montoVencido As Double = controlador.GetMontoVencido()
        Dim montoVencido As Double = 0

        If montoVencido > 0 Then
            Dim msgVariable As String = "POSEE DEUDA VENCIDA DE $ " & Format(montoVencido, "#0.00")
            Dim largoMsgVariable As Integer = msgVariable.Length

            Dim msg As String = "***************************************************************" & vbCrLf
            msg += "                                       ¡¡¡ATENCION!!!" & vbCrLf
            msg += vbCrLf
            msg += "                          NO PUEDE REALIZAR EL PEDIDO" & vbCrLf
            msg += ""
            'para centrar el texto cuento los caracteres
            For n As Integer = 1 To Math.Floor((68 - largoMsgVariable) / 2)
                msg += " "
            Next
            msg += msgVariable & vbCrLf
            msg += vbCrLf
            msg += "Realice el deposito correspondiente y mande el comprobante" & vbCrLf
            msg += "                                   al area administrativa" & vbCrLf
            msg += "***************************************************************"

            MsgBox(msg, vbCritical, TitulosMensaje)

            Me.Close()
        End If
    End Sub
End Class