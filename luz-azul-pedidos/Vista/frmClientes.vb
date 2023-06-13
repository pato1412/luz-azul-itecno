Imports System.Text.RegularExpressions
Imports LAFunctions.LuzAzulPedidos


Public Class frmClientes
    Private controlador As Controlador

    Dim listOnit As List(Of ComboItemCliente) = New List(Of ComboItemCliente)
    Dim listNew As List(Of ComboItemCliente) = New List(Of ComboItemCliente)

    Private Sub frmEstablecimiento_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim respuestaCliente As ResponseClientes

        'Obtengo la instancia del controlador
        controlador = Controlador.GetInstancia()

        'Marco el Pedido como cliente
        Controlador.SetEspedidoCliente(True)
        Controlador.ClearCliente()

        cmbClientes.ValueMember = "Value"
        cmbClientes.DisplayMember = "Text"

        respuestaCliente = controlador.GetClientes()
        If (respuestaCliente.ConsultaExitosa = True) Then
            listOnit = New List(Of ComboItemCliente)
            For Each Cli As ClientePedido In respuestaCliente.rs
                listOnit.Add(New ComboItemCliente(Cli.RazonSocial + " (" + Cli.CUIT + ")", Cli.ClienteId, Cli.RazonSocial, Cli.CUIT))
            Next
            cmbClientes.Items.AddRange(listOnit.ToArray())
        End If

        For Each strMails As String In GlobalSettings.ListEMailsDefault
            ListEmails.Items.Add(strMails)
        Next

        'Cargo en memoria las clasificaciones que estan habilitadas para los clientes
        controlador.LoadClasificacionesClientes()

    End Sub

    Private Sub BtnIngresar_Click(sender As Object, e As EventArgs) Handles BtnIngresar.Click

        lblMensaje.Visible = False

        Dim ArrayEmails As List(Of String) = New List(Of String)

        'Validar la direcciones de email
        If ListEmails.Items.Count = 0 Then
            lblMensaje.Text = "Debe ingresar al menos un email"
            lblMensaje.Visible = True
            Exit Sub
        Else
            For Each CurrentEmailList As String In ListEmails.Items
                ArrayEmails.Add(CurrentEmailList)
            Next
        End If

        If cmbClientes.SelectedIndex < 0 Then
            lblMensaje.Text = "Debe seleccionar un cliente"
            lblMensaje.Visible = True
            Exit Sub
        End If

        Dim currentCliente As ComboItemCliente = cmbClientes.SelectedItem

        controlador.GuardarConfiguracionesClientes(currentCliente, ArrayEmails)


        Dim frmMain = New frmMain
        frmMain.Show()
        Me.Close()
    End Sub
    Function IsValidEmailFormat(ByVal s As String) As Boolean
        Return Regex.IsMatch(s, "^([0-9a-zA-Z]([-\.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$")
    End Function

    Private Sub BtnSalir_Click(sender As Object, e As EventArgs) Handles BtnSalir.Click
        Me.Close()
    End Sub
    Private Sub cmbClientes_TextUpdate(sender As Object, e As EventArgs) Handles cmbClientes.TextUpdate
        cmbClientes.Items.Clear()
        listNew.Clear()
        For Each item As ComboItemCliente In listOnit
            If item.Text.ToLower.Contains(cmbClientes.Text.ToLower) Then
                listNew.Add(item)
            End If
        Next
        cmbClientes.Items.AddRange(listNew.ToArray())
        cmbClientes.SelectionStart = cmbClientes.Text.Length
        Cursor = Cursors.Default
        cmbClientes.DroppedDown = True
    End Sub

    Private Sub BtnAgregar_Click(sender As Object, e As EventArgs) Handles BtnAgregar.Click

        'valido que no ingrese el mismo mail dos veces
        For Each CurrentEmailList As String In ListEmails.Items
            If CurrentEmailList = txtEmail.Text.Trim Then
                lblMensaje.Text = "No puedes ingresar el mismo email"
                lblMensaje.Visible = True
                Exit Sub
            End If
        Next

        If Not IsValidEmailFormat(txtEmail.Text.Trim) Then
            lblMensaje.Text = "El email ingresado no es valido"
            lblMensaje.Visible = True
        Else
            ListEmails.Items.Add(txtEmail.Text.Trim)
            lblMensaje.Visible = False
        End If
    End Sub

    Private Sub ListEmails_KeyDown(sender As Object, e As KeyEventArgs) Handles ListEmails.KeyDown
        If e.KeyCode = Keys.Delete AndAlso ListEmails.SelectedItem <> Nothing Then
            ListEmails.Items.RemoveAt(ListEmails.SelectedIndex)
        End If
    End Sub
End Class



Public Class ComboItemCliente
    Public Sub New(ByVal text As String, ByVal value As Integer, ByVal RazonSocial As String, ByVal cuit As String)
        t = text
        v = value
        c = cuit
        r = RazonSocial
    End Sub

    Private t As String
    Private v As Integer
    Private r As String
    Private c As String

    Public Property Text() As String
        Get
            Return t
        End Get
        Set(ByVal value As String)
            t = value
        End Set
    End Property
    Public Property Value() As Integer
        Get
            Return v
        End Get
        Set(ByVal value As Integer)
            v = value
        End Set
    End Property
    Public Property Cuit() As String
        Get
            Return c
        End Get
        Set(ByVal value As String)
            c = value
        End Set
    End Property

    Public Property RazonSocial() As String
        Get
            Return r
        End Get
        Set(ByVal value As String)
            r = value
        End Set
    End Property


End Class
