<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmMain
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
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMain))
        Me.LblNroProveedor = New System.Windows.Forms.Label()
        Me.BtnSiguiente = New System.Windows.Forms.Button()
        Me.LblProveedor = New System.Windows.Forms.Label()
        Me.DbGridProveedores = New System.Windows.Forms.DataGridView()
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.RestaurarValorOriginalToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuObservaciones = New System.Windows.Forms.ToolStripMenuItem()
        Me.BtnAnterior = New System.Windows.Forms.Button()
        Me.ProgressBarEnvio = New System.Windows.Forms.ProgressBar()
        Me.LblEstado = New System.Windows.Forms.Label()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.cmbAcciones = New System.Windows.Forms.ComboBox()
        Me.chkModificados = New System.Windows.Forms.CheckBox()
        Me.LblDepositos = New System.Windows.Forms.Label()
        Me.LblEstablecimiento = New System.Windows.Forms.Label()
        Me.LblKgsPedido = New System.Windows.Forms.Label()
        Me.chkceros = New System.Windows.Forms.CheckBox()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.PictureBox3 = New System.Windows.Forms.PictureBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.PictureBox2 = New System.Windows.Forms.PictureBox()
        Me.FrameEnvio = New System.Windows.Forms.GroupBox()
        Me.PictureBox4 = New System.Windows.Forms.PictureBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.PictureBox5 = New System.Windows.Forms.PictureBox()
        CType(Me.DbGridProveedores, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        CType(Me.PictureBox3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.FrameEnvio.SuspendLayout()
        CType(Me.PictureBox4, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel2.SuspendLayout()
        CType(Me.PictureBox5, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'LblNroProveedor
        '
        Me.LblNroProveedor.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.LblNroProveedor.BackColor = System.Drawing.Color.FromArgb(CType(CType(170, Byte), Integer), CType(CType(199, Byte), Integer), CType(CType(44, Byte), Integer))
        Me.LblNroProveedor.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblNroProveedor.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(70, Byte), Integer), CType(CType(148, Byte), Integer))
        Me.LblNroProveedor.Location = New System.Drawing.Point(-1, 4)
        Me.LblNroProveedor.Name = "LblNroProveedor"
        Me.LblNroProveedor.Size = New System.Drawing.Size(992, 22)
        Me.LblNroProveedor.TabIndex = 0
        Me.LblNroProveedor.Text = "Cargando..."
        Me.LblNroProveedor.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'BtnSiguiente
        '
        Me.BtnSiguiente.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.BtnSiguiente.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(70, Byte), Integer), CType(CType(148, Byte), Integer))
        Me.BtnSiguiente.Cursor = System.Windows.Forms.Cursors.Hand
        Me.BtnSiguiente.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnSiguiente.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnSiguiente.ForeColor = System.Drawing.Color.White
        Me.BtnSiguiente.Location = New System.Drawing.Point(859, 546)
        Me.BtnSiguiente.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.BtnSiguiente.Name = "BtnSiguiente"
        Me.BtnSiguiente.Size = New System.Drawing.Size(159, 47)
        Me.BtnSiguiente.TabIndex = 1
        Me.BtnSiguiente.Text = "Siguiente >"
        Me.BtnSiguiente.UseVisualStyleBackColor = False
        '
        'LblProveedor
        '
        Me.LblProveedor.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.LblProveedor.BackColor = System.Drawing.Color.FromArgb(CType(CType(170, Byte), Integer), CType(CType(199, Byte), Integer), CType(CType(44, Byte), Integer))
        Me.LblProveedor.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblProveedor.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(70, Byte), Integer), CType(CType(148, Byte), Integer))
        Me.LblProveedor.Location = New System.Drawing.Point(-1, 26)
        Me.LblProveedor.Name = "LblProveedor"
        Me.LblProveedor.Size = New System.Drawing.Size(987, 33)
        Me.LblProveedor.TabIndex = 2
        Me.LblProveedor.Text = "Datos Proveedores"
        Me.LblProveedor.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'DbGridProveedores
        '
        Me.DbGridProveedores.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.DbGridProveedores.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DbGridProveedores.ContextMenuStrip = Me.ContextMenuStrip1
        Me.DbGridProveedores.Location = New System.Drawing.Point(29, 159)
        Me.DbGridProveedores.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.DbGridProveedores.Name = "DbGridProveedores"
        Me.DbGridProveedores.RowHeadersWidth = 51
        Me.DbGridProveedores.RowTemplate.Height = 24
        Me.DbGridProveedores.Size = New System.Drawing.Size(989, 304)
        Me.DbGridProveedores.TabIndex = 3
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.RestaurarValorOriginalToolStripMenuItem, Me.ToolStripMenuObservaciones})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(233, 52)
        '
        'RestaurarValorOriginalToolStripMenuItem
        '
        Me.RestaurarValorOriginalToolStripMenuItem.Name = "RestaurarValorOriginalToolStripMenuItem"
        Me.RestaurarValorOriginalToolStripMenuItem.Size = New System.Drawing.Size(232, 24)
        Me.RestaurarValorOriginalToolStripMenuItem.Text = "Restaurar valor original"
        '
        'ToolStripMenuObservaciones
        '
        Me.ToolStripMenuObservaciones.Name = "ToolStripMenuObservaciones"
        Me.ToolStripMenuObservaciones.Size = New System.Drawing.Size(232, 24)
        Me.ToolStripMenuObservaciones.Text = "Agregar Observaciones"
        '
        'BtnAnterior
        '
        Me.BtnAnterior.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.BtnAnterior.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(70, Byte), Integer), CType(CType(148, Byte), Integer))
        Me.BtnAnterior.Cursor = System.Windows.Forms.Cursors.Hand
        Me.BtnAnterior.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnAnterior.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnAnterior.ForeColor = System.Drawing.Color.White
        Me.BtnAnterior.Location = New System.Drawing.Point(693, 546)
        Me.BtnAnterior.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.BtnAnterior.Name = "BtnAnterior"
        Me.BtnAnterior.Size = New System.Drawing.Size(161, 47)
        Me.BtnAnterior.TabIndex = 4
        Me.BtnAnterior.Text = "< Anterior"
        Me.BtnAnterior.UseVisualStyleBackColor = False
        '
        'ProgressBarEnvio
        '
        Me.ProgressBarEnvio.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ProgressBarEnvio.Location = New System.Drawing.Point(29, 491)
        Me.ProgressBarEnvio.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.ProgressBarEnvio.Name = "ProgressBarEnvio"
        Me.ProgressBarEnvio.Size = New System.Drawing.Size(989, 23)
        Me.ProgressBarEnvio.Style = System.Windows.Forms.ProgressBarStyle.Continuous
        Me.ProgressBarEnvio.TabIndex = 5
        '
        'LblEstado
        '
        Me.LblEstado.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.LblEstado.AutoSize = True
        Me.LblEstado.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblEstado.Location = New System.Drawing.Point(27, 468)
        Me.LblEstado.Name = "LblEstado"
        Me.LblEstado.Size = New System.Drawing.Size(89, 18)
        Me.LblEstado.TabIndex = 6
        Me.LblEstado.Text = "Cargando..."
        '
        'Panel1
        '
        Me.Panel1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel1.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(70, Byte), Integer), CType(CType(148, Byte), Integer))
        Me.Panel1.Controls.Add(Me.cmbAcciones)
        Me.Panel1.Controls.Add(Me.chkModificados)
        Me.Panel1.Controls.Add(Me.LblDepositos)
        Me.Panel1.Controls.Add(Me.LblEstablecimiento)
        Me.Panel1.Controls.Add(Me.LblKgsPedido)
        Me.Panel1.Controls.Add(Me.chkceros)
        Me.Panel1.Location = New System.Drawing.Point(29, 76)
        Me.Panel1.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(989, 76)
        Me.Panel1.TabIndex = 7
        '
        'cmbAcciones
        '
        Me.cmbAcciones.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmbAcciones.BackColor = System.Drawing.Color.White
        Me.cmbAcciones.Font = New System.Drawing.Font("Arial", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbAcciones.FormattingEnabled = True
        Me.cmbAcciones.Location = New System.Drawing.Point(419, 6)
        Me.cmbAcciones.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.cmbAcciones.Name = "cmbAcciones"
        Me.cmbAcciones.Size = New System.Drawing.Size(269, 27)
        Me.cmbAcciones.TabIndex = 12
        '
        'chkModificados
        '
        Me.chkModificados.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.chkModificados.AutoSize = True
        Me.chkModificados.Checked = True
        Me.chkModificados.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkModificados.Cursor = System.Windows.Forms.Cursors.Hand
        Me.chkModificados.Font = New System.Drawing.Font("Arial", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkModificados.ForeColor = System.Drawing.Color.White
        Me.chkModificados.Location = New System.Drawing.Point(419, 43)
        Me.chkModificados.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.chkModificados.Name = "chkModificados"
        Me.chkModificados.Size = New System.Drawing.Size(269, 26)
        Me.chkModificados.TabIndex = 11
        Me.chkModificados.Text = "Mostrar solo modificados"
        Me.chkModificados.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkModificados.UseVisualStyleBackColor = True
        '
        'LblDepositos
        '
        Me.LblDepositos.AutoSize = True
        Me.LblDepositos.Font = New System.Drawing.Font("Arial", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblDepositos.ForeColor = System.Drawing.Color.White
        Me.LblDepositos.Location = New System.Drawing.Point(17, 44)
        Me.LblDepositos.Name = "LblDepositos"
        Me.LblDepositos.Size = New System.Drawing.Size(216, 22)
        Me.LblDepositos.TabIndex = 10
        Me.LblDepositos.Text = "Cargando Depositos..."
        Me.LblDepositos.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'LblEstablecimiento
        '
        Me.LblEstablecimiento.AutoSize = True
        Me.LblEstablecimiento.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(70, Byte), Integer), CType(CType(148, Byte), Integer))
        Me.LblEstablecimiento.Font = New System.Drawing.Font("Arial", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblEstablecimiento.ForeColor = System.Drawing.Color.White
        Me.LblEstablecimiento.Location = New System.Drawing.Point(17, 9)
        Me.LblEstablecimiento.Name = "LblEstablecimiento"
        Me.LblEstablecimiento.Size = New System.Drawing.Size(270, 22)
        Me.LblEstablecimiento.TabIndex = 2
        Me.LblEstablecimiento.Text = "Cargando Establecimiento..."
        Me.LblEstablecimiento.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'LblKgsPedido
        '
        Me.LblKgsPedido.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.LblKgsPedido.Font = New System.Drawing.Font("Arial", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblKgsPedido.ForeColor = System.Drawing.Color.White
        Me.LblKgsPedido.Location = New System.Drawing.Point(645, 9)
        Me.LblKgsPedido.Name = "LblKgsPedido"
        Me.LblKgsPedido.Size = New System.Drawing.Size(327, 22)
        Me.LblKgsPedido.TabIndex = 10
        Me.LblKgsPedido.Text = "Kg. Seleccionados: 0.00"
        Me.LblKgsPedido.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'chkceros
        '
        Me.chkceros.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.chkceros.AutoSize = True
        Me.chkceros.Checked = True
        Me.chkceros.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkceros.Cursor = System.Windows.Forms.Cursors.Hand
        Me.chkceros.Font = New System.Drawing.Font("Arial", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkceros.ForeColor = System.Drawing.Color.White
        Me.chkceros.Location = New System.Drawing.Point(703, 43)
        Me.chkceros.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.chkceros.Name = "chkceros"
        Me.chkceros.Size = New System.Drawing.Size(272, 26)
        Me.chkceros.TabIndex = 9
        Me.chkceros.Text = "Mostrar Cant. a Pedir en 0"
        Me.chkceros.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.chkceros.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.GroupBox1.Controls.Add(Me.Label7)
        Me.GroupBox1.Controls.Add(Me.Label6)
        Me.GroupBox1.Controls.Add(Me.Label5)
        Me.GroupBox1.Controls.Add(Me.Label4)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.PictureBox3)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Controls.Add(Me.PictureBox2)
        Me.GroupBox1.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox1.Location = New System.Drawing.Point(29, 526)
        Me.GroupBox1.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Padding = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.GroupBox1.Size = New System.Drawing.Size(607, 81)
        Me.GroupBox1.TabIndex = 10
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "   Leyendas    "
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(405, 52)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(142, 17)
        Me.Label7.TabIndex = 7
        Me.Label7.Text = "Agregar observación"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(282, 52)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(116, 18)
        Me.Label6.TabIndex = 6
        Me.Label6.Text = "(Click derecho)"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Arial", 9.0!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Italic), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(311, 25)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(86, 17)
        Me.Label5.TabIndex = 5
        Me.Label5.Text = " * (Cursiva)"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(405, 25)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(125, 17)
        Me.Label4.TabIndex = 4
        Me.Label4.Text = "Producto opcional"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(41, 52)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(179, 17)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "Cantidad modificada > 5%"
        '
        'PictureBox3
        '
        Me.PictureBox3.BackColor = System.Drawing.Color.LightSalmon
        Me.PictureBox3.Location = New System.Drawing.Point(16, 49)
        Me.PictureBox3.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.PictureBox3.Name = "PictureBox3"
        Me.PictureBox3.Size = New System.Drawing.Size(21, 20)
        Me.PictureBox3.TabIndex = 2
        Me.PictureBox3.TabStop = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(41, 25)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(188, 17)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Cantidad modificada <= 5%"
        '
        'PictureBox2
        '
        Me.PictureBox2.BackColor = System.Drawing.Color.LightGreen
        Me.PictureBox2.Location = New System.Drawing.Point(16, 22)
        Me.PictureBox2.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.PictureBox2.Name = "PictureBox2"
        Me.PictureBox2.Size = New System.Drawing.Size(21, 20)
        Me.PictureBox2.TabIndex = 0
        Me.PictureBox2.TabStop = False
        '
        'FrameEnvio
        '
        Me.FrameEnvio.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.FrameEnvio.AutoSize = True
        Me.FrameEnvio.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.FrameEnvio.Controls.Add(Me.PictureBox4)
        Me.FrameEnvio.Controls.Add(Me.Label3)
        Me.FrameEnvio.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.FrameEnvio.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FrameEnvio.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(70, Byte), Integer), CType(CType(148, Byte), Integer))
        Me.FrameEnvio.Location = New System.Drawing.Point(233, 213)
        Me.FrameEnvio.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.FrameEnvio.Name = "FrameEnvio"
        Me.FrameEnvio.Padding = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.FrameEnvio.Size = New System.Drawing.Size(662, 118)
        Me.FrameEnvio.TabIndex = 11
        Me.FrameEnvio.TabStop = False
        '
        'PictureBox4
        '
        Me.PictureBox4.Image = CType(resources.GetObject("PictureBox4.Image"), System.Drawing.Image)
        Me.PictureBox4.Location = New System.Drawing.Point(21, 22)
        Me.PictureBox4.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.PictureBox4.Name = "PictureBox4"
        Me.PictureBox4.Size = New System.Drawing.Size(77, 74)
        Me.PictureBox4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox4.TabIndex = 1
        Me.PictureBox4.TabStop = False
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Arial", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(104, 38)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(482, 44)
        Me.Label3.TabIndex = 0
        Me.Label3.Text = "Por favor espere mientras el pedido es generado... " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Muchas gracias."
        '
        'Panel2
        '
        Me.Panel2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel2.BackColor = System.Drawing.Color.FromArgb(CType(CType(170, Byte), Integer), CType(CType(199, Byte), Integer), CType(CType(44, Byte), Integer))
        Me.Panel2.Controls.Add(Me.PictureBox5)
        Me.Panel2.Controls.Add(Me.LblProveedor)
        Me.Panel2.Controls.Add(Me.LblNroProveedor)
        Me.Panel2.Location = New System.Drawing.Point(29, 7)
        Me.Panel2.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(989, 63)
        Me.Panel2.TabIndex = 11
        '
        'PictureBox5
        '
        Me.PictureBox5.Image = CType(resources.GetObject("PictureBox5.Image"), System.Drawing.Image)
        Me.PictureBox5.Location = New System.Drawing.Point(15, 10)
        Me.PictureBox5.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.PictureBox5.Name = "PictureBox5"
        Me.PictureBox5.Size = New System.Drawing.Size(56, 42)
        Me.PictureBox5.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox5.TabIndex = 8
        Me.PictureBox5.TabStop = False
        Me.PictureBox5.Visible = False
        '
        'frmMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(1053, 618)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.FrameEnvio)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.LblEstado)
        Me.Controls.Add(Me.ProgressBarEnvio)
        Me.Controls.Add(Me.BtnAnterior)
        Me.Controls.Add(Me.DbGridProveedores)
        Me.Controls.Add(Me.BtnSiguiente)
        Me.Controls.Add(Me.Panel1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.Name = "frmMain"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Luz Azul Pedidos"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        CType(Me.DbGridProveedores, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        CType(Me.PictureBox3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.FrameEnvio.ResumeLayout(False)
        Me.FrameEnvio.PerformLayout()
        CType(Me.PictureBox4, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel2.ResumeLayout(False)
        CType(Me.PictureBox5, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents LblNroProveedor As Label
    Friend WithEvents BtnSiguiente As Button
    Friend WithEvents LblProveedor As Label
    Friend WithEvents DbGridProveedores As DataGridView
    Friend WithEvents BtnAnterior As Button
    Friend WithEvents ProgressBarEnvio As ProgressBar
    Friend WithEvents LblEstado As Label
    Friend WithEvents ContextMenuStrip1 As ContextMenuStrip
    Friend WithEvents RestaurarValorOriginalToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents Panel1 As Panel
    Friend WithEvents chkceros As CheckBox
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents Label2 As Label
    Friend WithEvents PictureBox3 As PictureBox
    Friend WithEvents Label1 As Label
    Friend WithEvents PictureBox2 As PictureBox
    Friend WithEvents FrameEnvio As GroupBox
    Friend WithEvents Label3 As Label
    Friend WithEvents PictureBox4 As PictureBox
    Friend WithEvents LblKgsPedido As Label
    Friend WithEvents Panel2 As Panel
    Friend WithEvents LblDepositos As Label
    Friend WithEvents PictureBox5 As PictureBox
    Friend WithEvents LblEstablecimiento As Label
    Friend WithEvents ToolStripMenuObservaciones As ToolStripMenuItem
    Friend WithEvents chkModificados As CheckBox
    Friend WithEvents cmbAcciones As ComboBox
    Friend WithEvents Label7 As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents Label4 As Label
End Class
