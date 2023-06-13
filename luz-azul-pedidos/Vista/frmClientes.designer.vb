<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmClientes
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmClientes))
        Me.Label1 = New System.Windows.Forms.Label()
        Me.BtnIngresar = New System.Windows.Forms.Button()
        Me.BtnSalir = New System.Windows.Forms.Button()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.LblRespuesta = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.PictureBox3 = New System.Windows.Forms.PictureBox()
        Me.PictureBox2 = New System.Windows.Forms.PictureBox()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.lblMensaje = New System.Windows.Forms.Label()
        Me.txtEmail = New System.Windows.Forms.TextBox()
        Me.cmbClientes = New System.Windows.Forms.ComboBox()
        Me.BtnAgregar = New System.Windows.Forms.Button()
        Me.ListEmails = New System.Windows.Forms.ListBox()
        CType(Me.PictureBox3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Arial", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(70, Byte), Integer), CType(CType(148, Byte), Integer))
        Me.Label1.Location = New System.Drawing.Point(65, 243)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(69, 22)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Cliente"
        '
        'BtnIngresar
        '
        Me.BtnIngresar.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(70, Byte), Integer), CType(CType(148, Byte), Integer))
        Me.BtnIngresar.Cursor = System.Windows.Forms.Cursors.Hand
        Me.BtnIngresar.FlatAppearance.BorderSize = 2
        Me.BtnIngresar.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnIngresar.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnIngresar.ForeColor = System.Drawing.Color.White
        Me.BtnIngresar.Location = New System.Drawing.Point(68, 550)
        Me.BtnIngresar.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.BtnIngresar.Name = "BtnIngresar"
        Me.BtnIngresar.Size = New System.Drawing.Size(196, 49)
        Me.BtnIngresar.TabIndex = 5
        Me.BtnIngresar.Text = "Seleccionar"
        Me.BtnIngresar.UseVisualStyleBackColor = False
        '
        'BtnSalir
        '
        Me.BtnSalir.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(70, Byte), Integer), CType(CType(148, Byte), Integer))
        Me.BtnSalir.Cursor = System.Windows.Forms.Cursors.Hand
        Me.BtnSalir.FlatAppearance.BorderSize = 2
        Me.BtnSalir.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnSalir.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnSalir.ForeColor = System.Drawing.Color.White
        Me.BtnSalir.Location = New System.Drawing.Point(285, 550)
        Me.BtnSalir.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.BtnSalir.Name = "BtnSalir"
        Me.BtnSalir.Size = New System.Drawing.Size(196, 49)
        Me.BtnSalir.TabIndex = 6
        Me.BtnSalir.Text = "Salir"
        Me.BtnSalir.UseVisualStyleBackColor = False
        '
        'Label3
        '
        Me.Label3.Font = New System.Drawing.Font("Arial", 16.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(70, Byte), Integer), CType(CType(148, Byte), Integer))
        Me.Label3.Location = New System.Drawing.Point(0, 118)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(552, 32)
        Me.Label3.TabIndex = 7
        Me.Label3.Text = "Seleccione Cliente"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'LblRespuesta
        '
        Me.LblRespuesta.AutoSize = True
        Me.LblRespuesta.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblRespuesta.ForeColor = System.Drawing.Color.FromArgb(CType(CType(168, Byte), Integer), CType(CType(207, Byte), Integer), CType(CType(69, Byte), Integer))
        Me.LblRespuesta.Location = New System.Drawing.Point(53, 334)
        Me.LblRespuesta.Name = "LblRespuesta"
        Me.LblRespuesta.Size = New System.Drawing.Size(0, 24)
        Me.LblRespuesta.TabIndex = 8
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Arial", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(70, Byte), Integer), CType(CType(148, Byte), Integer))
        Me.Label2.Location = New System.Drawing.Point(65, 336)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(57, 22)
        Me.Label2.TabIndex = 10
        Me.Label2.Text = "Email"
        '
        'PictureBox3
        '
        Me.PictureBox3.Image = CType(resources.GetObject("PictureBox3.Image"), System.Drawing.Image)
        Me.PictureBox3.Location = New System.Drawing.Point(29, 329)
        Me.PictureBox3.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.PictureBox3.Name = "PictureBox3"
        Me.PictureBox3.Size = New System.Drawing.Size(32, 32)
        Me.PictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox3.TabIndex = 13
        Me.PictureBox3.TabStop = False
        '
        'PictureBox2
        '
        Me.PictureBox2.Image = CType(resources.GetObject("PictureBox2.Image"), System.Drawing.Image)
        Me.PictureBox2.Location = New System.Drawing.Point(29, 236)
        Me.PictureBox2.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.PictureBox2.Name = "PictureBox2"
        Me.PictureBox2.Size = New System.Drawing.Size(32, 30)
        Me.PictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox2.TabIndex = 12
        Me.PictureBox2.TabStop = False
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), System.Drawing.Image)
        Me.PictureBox1.Location = New System.Drawing.Point(0, 7)
        Me.PictureBox1.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(552, 101)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage
        Me.PictureBox1.TabIndex = 2
        Me.PictureBox1.TabStop = False
        '
        'Label4
        '
        Me.Label4.Font = New System.Drawing.Font("Arial", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(70, Byte), Integer), CType(CType(148, Byte), Integer))
        Me.Label4.Location = New System.Drawing.Point(1, 158)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(551, 65)
        Me.Label4.TabIndex = 14
        Me.Label4.Text = "Por favor seleccione el cliente de la lista e ingrese la dirección de email donde" &
    " se enviara el pedido. Si desea ingresar mas de una dirección de email agregelas" &
    " en la lista."
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblMensaje
        '
        Me.lblMensaje.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblMensaje.ForeColor = System.Drawing.Color.Red
        Me.lblMensaje.Location = New System.Drawing.Point(0, 517)
        Me.lblMensaje.Name = "lblMensaje"
        Me.lblMensaje.Size = New System.Drawing.Size(552, 28)
        Me.lblMensaje.TabIndex = 15
        Me.lblMensaje.Text = "Ingrese usuario y contraseña"
        Me.lblMensaje.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.lblMensaje.Visible = False
        '
        'txtEmail
        '
        Me.txtEmail.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtEmail.BackColor = System.Drawing.Color.White
        Me.txtEmail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtEmail.Font = New System.Drawing.Font("Arial", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtEmail.Location = New System.Drawing.Point(29, 365)
        Me.txtEmail.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.txtEmail.MaxLength = 100
        Me.txtEmail.Name = "txtEmail"
        Me.txtEmail.Size = New System.Drawing.Size(353, 27)
        Me.txtEmail.TabIndex = 17
        '
        'cmbClientes
        '
        Me.cmbClientes.Font = New System.Drawing.Font("Arial", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbClientes.FormattingEnabled = True
        Me.cmbClientes.Location = New System.Drawing.Point(29, 271)
        Me.cmbClientes.Name = "cmbClientes"
        Me.cmbClientes.Size = New System.Drawing.Size(479, 27)
        Me.cmbClientes.TabIndex = 19
        '
        'BtnAgregar
        '
        Me.BtnAgregar.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(70, Byte), Integer), CType(CType(148, Byte), Integer))
        Me.BtnAgregar.Cursor = System.Windows.Forms.Cursors.Hand
        Me.BtnAgregar.FlatAppearance.BorderSize = 2
        Me.BtnAgregar.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnAgregar.Font = New System.Drawing.Font("Arial", 10.2!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnAgregar.ForeColor = System.Drawing.Color.White
        Me.BtnAgregar.Location = New System.Drawing.Point(403, 354)
        Me.BtnAgregar.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.BtnAgregar.Name = "BtnAgregar"
        Me.BtnAgregar.Size = New System.Drawing.Size(105, 38)
        Me.BtnAgregar.TabIndex = 21
        Me.BtnAgregar.Text = "Agregar"
        Me.BtnAgregar.UseVisualStyleBackColor = False
        '
        'ListEmails
        '
        Me.ListEmails.Font = New System.Drawing.Font("Arial", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ListEmails.FormattingEnabled = True
        Me.ListEmails.ItemHeight = 19
        Me.ListEmails.Location = New System.Drawing.Point(29, 418)
        Me.ListEmails.Name = "ListEmails"
        Me.ListEmails.Size = New System.Drawing.Size(479, 80)
        Me.ListEmails.TabIndex = 20
        '
        'frmClientes
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(552, 616)
        Me.Controls.Add(Me.BtnAgregar)
        Me.Controls.Add(Me.ListEmails)
        Me.Controls.Add(Me.cmbClientes)
        Me.Controls.Add(Me.txtEmail)
        Me.Controls.Add(Me.lblMensaje)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.PictureBox3)
        Me.Controls.Add(Me.PictureBox2)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.LblRespuesta)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.BtnSalir)
        Me.Controls.Add(Me.BtnIngresar)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.Label1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmClientes"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Luz Azul Pedidos"
        CType(Me.PictureBox3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Label1 As Label
    Friend WithEvents PictureBox1 As PictureBox
    Friend WithEvents BtnIngresar As Button
    Friend WithEvents BtnSalir As Button
    Friend WithEvents Label3 As Label
    Friend WithEvents LblRespuesta As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents PictureBox2 As PictureBox
    Friend WithEvents PictureBox3 As PictureBox
    Friend WithEvents Label4 As Label
    Friend WithEvents lblMensaje As Label
    Friend WithEvents txtEmail As TextBox
    Friend WithEvents cmbClientes As ComboBox
    Friend WithEvents BtnAgregar As Button
    Friend WithEvents ListEmails As ListBox
End Class
