Public Class frmLogin
    Private controlador As Controlador

    Private Sub frmLogin_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Verifico si es el sistema se ejecuta en Ensemble o en una Franquicia
        ' Obtengo las configuraciones de envio de mails
        'Obtengo la razon social de la empresa
        controlador = Controlador.GetInstancia()

        'Leo las configuraciones del archivo xml
        Controlador.LeerConfiguracionesXML()

        LblEmpresa.Text = "Empresa: " + Controlador.GetRazonSOcial() + " (" + Controlador.GetNombreBaseDB + ")"
    End Sub

    Private Sub txtUsuario_KeyUp(sender As Object, e As KeyEventArgs) Handles txtUsuario.KeyUp
        ' Si presiono Enter en el campo usuario, hago foco en el campo clave
        If e.KeyValue = 13 Then txtClave.Focus()
    End Sub

    Private Sub txtClave_KeyUp(sender As Object, e As KeyEventArgs) Handles txtClave.KeyUp
        ' Si presiono Enter en el campo clave, genero el click del boton ingresar
        If e.KeyValue = 13 Then BtnIngresar.PerformClick()
    End Sub

    Private Sub BtnIngresar_Click(sender As Object, e As EventArgs) Handles BtnIngresar.Click
        Dim respuesta As LAFunctions.LuzAzulCommon.ResponseLogin

        If (txtUsuario.Text = "" Or txtClave.Text = "") Then
            LblRespuesta.Text = "Por favor ingrese su usuario y clave"
            LblRespuesta.Visible = True
            Exit Sub
        End If

        respuesta = controlador.DoLogin(txtUsuario.Text, txtClave.Text)
        If (respuesta.PermiteLogin) Then
            LblRespuesta.Visible = False

            Controlador.SetLogEvent("Usuario Logueado desde el MDI form")

            Controlador.SetMostrarFrameDepositos(True)

            Dim frmEsts = New frmEstablecimientos()
            frmEsts.ShowDialog()

            If controlador.GetCurrentEstablecimiento().EstablecimientoId <> "" Then

                Dim frmMDI As MDIParent1 = New MDIParent1
                frmMDI.Show()
                Me.Close()

            End If
        Else
            'muestro el mensaje de respuesta
            LblRespuesta.Text = respuesta.Mensaje
            LblRespuesta.Visible = True
        End If
    End Sub

    Private Sub BtnSalir_Click(sender As Object, e As EventArgs) Handles BtnSalir.Click
        Me.Close()
    End Sub

End Class