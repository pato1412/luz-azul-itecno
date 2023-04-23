<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class MDIParent1
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MDIParent1))
        Me.MenuStrip = New System.Windows.Forms.MenuStrip()
        Me.InicioToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.EnviarPedidosToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.EnviarPedidoClienteToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.RecepcionPedidosToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AprobarPedidosToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.PickeoPedidosToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.GenerarOrdenPickeoToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.VerOrdenesPickeoToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AdministrarOrdenesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AgruparOrdenesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.FacturacionToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.FacturacionAutomaticaToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ComprasToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.RecepcionFacturasToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ConfiguracionesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ConfigurarFacturacionEstablecimientoToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ConfigurarProductosSenasaToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ConfigurarEmpaquesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ConfigurarFamiliasToleranciasPickeoToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.GenerarExcelDePruebasToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.GenerarArchivoTXTToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.StockToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ControlDeInventarioToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CargardeMermasToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AdministrarToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AdministrarPromocionesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.StatusStrip = New System.Windows.Forms.StatusStrip()
        Me.ToolStripStatusLabelEmpresa = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ToolStripStatusLabelEstablecimiento = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ToolStripStatusLabelUsuario = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ToolTip = New System.Windows.Forms.ToolTip(Me.components)
        Me.ImgBackground = New System.Windows.Forms.PictureBox()
        Me.AsientosDeCierreDeMesToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.MenuStrip.SuspendLayout()
        Me.StatusStrip.SuspendLayout()
        CType(Me.ImgBackground, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'MenuStrip
        '
        Me.MenuStrip.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.MenuStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.InicioToolStripMenuItem, Me.FacturacionToolStripMenuItem, Me.ComprasToolStripMenuItem, Me.ConfiguracionesToolStripMenuItem, Me.StockToolStripMenuItem, Me.AdministrarToolStripMenuItem})
        Me.MenuStrip.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow
        Me.MenuStrip.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip.Name = "MenuStrip"
        Me.MenuStrip.Padding = New System.Windows.Forms.Padding(5, 2, 0, 2)
        Me.MenuStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System
        Me.MenuStrip.Size = New System.Drawing.Size(1093, 38)
        Me.MenuStrip.TabIndex = 5
        Me.MenuStrip.Text = "MenuStrip"
        '
        'InicioToolStripMenuItem
        '
        Me.InicioToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.EnviarPedidosToolStripMenuItem, Me.EnviarPedidoClienteToolStripMenuItem, Me.RecepcionPedidosToolStripMenuItem, Me.AprobarPedidosToolStripMenuItem, Me.PickeoPedidosToolStripMenuItem})
        Me.InicioToolStripMenuItem.Image = CType(resources.GetObject("InicioToolStripMenuItem.Image"), System.Drawing.Image)
        Me.InicioToolStripMenuItem.Name = "InicioToolStripMenuItem"
        Me.InicioToolStripMenuItem.Padding = New System.Windows.Forms.Padding(5)
        Me.InicioToolStripMenuItem.Size = New System.Drawing.Size(95, 34)
        Me.InicioToolStripMenuItem.Text = "Pedidos"
        '
        'EnviarPedidosToolStripMenuItem
        '
        Me.EnviarPedidosToolStripMenuItem.Image = CType(resources.GetObject("EnviarPedidosToolStripMenuItem.Image"), System.Drawing.Image)
        Me.EnviarPedidosToolStripMenuItem.Name = "EnviarPedidosToolStripMenuItem"
        Me.EnviarPedidosToolStripMenuItem.Size = New System.Drawing.Size(232, 26)
        Me.EnviarPedidosToolStripMenuItem.Text = "Enviar Pedidos"
        '
        'EnviarPedidoClienteToolStripMenuItem
        '
        Me.EnviarPedidoClienteToolStripMenuItem.Image = CType(resources.GetObject("EnviarPedidoClienteToolStripMenuItem.Image"), System.Drawing.Image)
        Me.EnviarPedidoClienteToolStripMenuItem.Name = "EnviarPedidoClienteToolStripMenuItem"
        Me.EnviarPedidoClienteToolStripMenuItem.Size = New System.Drawing.Size(232, 26)
        Me.EnviarPedidoClienteToolStripMenuItem.Text = "Enviar Pedido Cliente"
        '
        'RecepcionPedidosToolStripMenuItem
        '
        Me.RecepcionPedidosToolStripMenuItem.Image = CType(resources.GetObject("RecepcionPedidosToolStripMenuItem.Image"), System.Drawing.Image)
        Me.RecepcionPedidosToolStripMenuItem.Name = "RecepcionPedidosToolStripMenuItem"
        Me.RecepcionPedidosToolStripMenuItem.Size = New System.Drawing.Size(232, 26)
        Me.RecepcionPedidosToolStripMenuItem.Text = "Recepcion Pedidos"
        '
        'AprobarPedidosToolStripMenuItem
        '
        Me.AprobarPedidosToolStripMenuItem.Image = CType(resources.GetObject("AprobarPedidosToolStripMenuItem.Image"), System.Drawing.Image)
        Me.AprobarPedidosToolStripMenuItem.Name = "AprobarPedidosToolStripMenuItem"
        Me.AprobarPedidosToolStripMenuItem.Size = New System.Drawing.Size(232, 26)
        Me.AprobarPedidosToolStripMenuItem.Text = "Aprobar Pedidos"
        '
        'PickeoPedidosToolStripMenuItem
        '
        Me.PickeoPedidosToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.GenerarOrdenPickeoToolStripMenuItem, Me.VerOrdenesPickeoToolStripMenuItem, Me.AdministrarOrdenesToolStripMenuItem, Me.AgruparOrdenesToolStripMenuItem})
        Me.PickeoPedidosToolStripMenuItem.Image = CType(resources.GetObject("PickeoPedidosToolStripMenuItem.Image"), System.Drawing.Image)
        Me.PickeoPedidosToolStripMenuItem.Name = "PickeoPedidosToolStripMenuItem"
        Me.PickeoPedidosToolStripMenuItem.Padding = New System.Windows.Forms.Padding(0, 2, 0, 2)
        Me.PickeoPedidosToolStripMenuItem.Size = New System.Drawing.Size(232, 28)
        Me.PickeoPedidosToolStripMenuItem.Text = "Ordenes Pickeo"
        '
        'GenerarOrdenPickeoToolStripMenuItem
        '
        Me.GenerarOrdenPickeoToolStripMenuItem.Name = "GenerarOrdenPickeoToolStripMenuItem"
        Me.GenerarOrdenPickeoToolStripMenuItem.Size = New System.Drawing.Size(236, 26)
        Me.GenerarOrdenPickeoToolStripMenuItem.Text = "Generar Orden Pickeo"
        '
        'VerOrdenesPickeoToolStripMenuItem
        '
        Me.VerOrdenesPickeoToolStripMenuItem.Name = "VerOrdenesPickeoToolStripMenuItem"
        Me.VerOrdenesPickeoToolStripMenuItem.Size = New System.Drawing.Size(236, 26)
        Me.VerOrdenesPickeoToolStripMenuItem.Text = "Realizar Pickeo"
        '
        'AdministrarOrdenesToolStripMenuItem
        '
        Me.AdministrarOrdenesToolStripMenuItem.Name = "AdministrarOrdenesToolStripMenuItem"
        Me.AdministrarOrdenesToolStripMenuItem.Size = New System.Drawing.Size(236, 26)
        Me.AdministrarOrdenesToolStripMenuItem.Text = "Administrar Ordenes"
        '
        'AgruparOrdenesToolStripMenuItem
        '
        Me.AgruparOrdenesToolStripMenuItem.Name = "AgruparOrdenesToolStripMenuItem"
        Me.AgruparOrdenesToolStripMenuItem.Size = New System.Drawing.Size(236, 26)
        Me.AgruparOrdenesToolStripMenuItem.Text = "Agrupar Ordenes"
        '
        'FacturacionToolStripMenuItem
        '
        Me.FacturacionToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FacturacionAutomaticaToolStripMenuItem})
        Me.FacturacionToolStripMenuItem.Image = CType(resources.GetObject("FacturacionToolStripMenuItem.Image"), System.Drawing.Image)
        Me.FacturacionToolStripMenuItem.Name = "FacturacionToolStripMenuItem"
        Me.FacturacionToolStripMenuItem.Padding = New System.Windows.Forms.Padding(5)
        Me.FacturacionToolStripMenuItem.Size = New System.Drawing.Size(118, 34)
        Me.FacturacionToolStripMenuItem.Text = "Facturacion"
        '
        'FacturacionAutomaticaToolStripMenuItem
        '
        Me.FacturacionAutomaticaToolStripMenuItem.Name = "FacturacionAutomaticaToolStripMenuItem"
        Me.FacturacionAutomaticaToolStripMenuItem.Size = New System.Drawing.Size(246, 26)
        Me.FacturacionAutomaticaToolStripMenuItem.Text = "Facturacion automatica"
        '
        'ComprasToolStripMenuItem
        '
        Me.ComprasToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.RecepcionFacturasToolStripMenuItem})
        Me.ComprasToolStripMenuItem.Image = CType(resources.GetObject("ComprasToolStripMenuItem.Image"), System.Drawing.Image)
        Me.ComprasToolStripMenuItem.Name = "ComprasToolStripMenuItem"
        Me.ComprasToolStripMenuItem.Size = New System.Drawing.Size(102, 34)
        Me.ComprasToolStripMenuItem.Text = "Compras"
        '
        'RecepcionFacturasToolStripMenuItem
        '
        Me.RecepcionFacturasToolStripMenuItem.Image = CType(resources.GetObject("RecepcionFacturasToolStripMenuItem.Image"), System.Drawing.Image)
        Me.RecepcionFacturasToolStripMenuItem.Name = "RecepcionFacturasToolStripMenuItem"
        Me.RecepcionFacturasToolStripMenuItem.Size = New System.Drawing.Size(218, 26)
        Me.RecepcionFacturasToolStripMenuItem.Text = "Recepcion Facturas"
        '
        'ConfiguracionesToolStripMenuItem
        '
        Me.ConfiguracionesToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ConfigurarFacturacionEstablecimientoToolStripMenuItem, Me.ConfigurarProductosSenasaToolStripMenuItem, Me.ConfigurarEmpaquesToolStripMenuItem, Me.ConfigurarFamiliasToleranciasPickeoToolStripMenuItem, Me.GenerarExcelDePruebasToolStripMenuItem, Me.GenerarArchivoTXTToolStripMenuItem, Me.AsientosDeCierreDeMesToolStripMenuItem})
        Me.ConfiguracionesToolStripMenuItem.Image = CType(resources.GetObject("ConfiguracionesToolStripMenuItem.Image"), System.Drawing.Image)
        Me.ConfiguracionesToolStripMenuItem.Name = "ConfiguracionesToolStripMenuItem"
        Me.ConfiguracionesToolStripMenuItem.Padding = New System.Windows.Forms.Padding(5)
        Me.ConfiguracionesToolStripMenuItem.Size = New System.Drawing.Size(150, 34)
        Me.ConfiguracionesToolStripMenuItem.Text = "Configuraciones"
        '
        'ConfigurarFacturacionEstablecimientoToolStripMenuItem
        '
        Me.ConfigurarFacturacionEstablecimientoToolStripMenuItem.Name = "ConfigurarFacturacionEstablecimientoToolStripMenuItem"
        Me.ConfigurarFacturacionEstablecimientoToolStripMenuItem.Size = New System.Drawing.Size(286, 26)
        Me.ConfigurarFacturacionEstablecimientoToolStripMenuItem.Text = "Configurar facturacion"
        '
        'ConfigurarProductosSenasaToolStripMenuItem
        '
        Me.ConfigurarProductosSenasaToolStripMenuItem.Name = "ConfigurarProductosSenasaToolStripMenuItem"
        Me.ConfigurarProductosSenasaToolStripMenuItem.Size = New System.Drawing.Size(286, 26)
        Me.ConfigurarProductosSenasaToolStripMenuItem.Text = "Configurar productos Senasa"
        '
        'ConfigurarEmpaquesToolStripMenuItem
        '
        Me.ConfigurarEmpaquesToolStripMenuItem.Name = "ConfigurarEmpaquesToolStripMenuItem"
        Me.ConfigurarEmpaquesToolStripMenuItem.Size = New System.Drawing.Size(286, 26)
        Me.ConfigurarEmpaquesToolStripMenuItem.Text = "Configurar empaques"
        '
        'ConfigurarFamiliasToleranciasPickeoToolStripMenuItem
        '
        Me.ConfigurarFamiliasToleranciasPickeoToolStripMenuItem.Name = "ConfigurarFamiliasToleranciasPickeoToolStripMenuItem"
        Me.ConfigurarFamiliasToleranciasPickeoToolStripMenuItem.Size = New System.Drawing.Size(286, 26)
        Me.ConfigurarFamiliasToleranciasPickeoToolStripMenuItem.Text = "Configurar tolerancias pickeo"
        '
        'GenerarExcelDePruebasToolStripMenuItem
        '
        Me.GenerarExcelDePruebasToolStripMenuItem.Name = "GenerarExcelDePruebasToolStripMenuItem"
        Me.GenerarExcelDePruebasToolStripMenuItem.Size = New System.Drawing.Size(286, 26)
        Me.GenerarExcelDePruebasToolStripMenuItem.Text = "Generar excel de pruebas"
        '
        'GenerarArchivoTXTToolStripMenuItem
        '
        Me.GenerarArchivoTXTToolStripMenuItem.Name = "GenerarArchivoTXTToolStripMenuItem"
        Me.GenerarArchivoTXTToolStripMenuItem.Size = New System.Drawing.Size(286, 26)
        Me.GenerarArchivoTXTToolStripMenuItem.Text = "Generar Archivo TXT"
        '
        'StockToolStripMenuItem
        '
        Me.StockToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ControlDeInventarioToolStripMenuItem, Me.CargardeMermasToolStripMenuItem})
        Me.StockToolStripMenuItem.Image = CType(resources.GetObject("StockToolStripMenuItem.Image"), System.Drawing.Image)
        Me.StockToolStripMenuItem.Name = "StockToolStripMenuItem"
        Me.StockToolStripMenuItem.Size = New System.Drawing.Size(79, 34)
        Me.StockToolStripMenuItem.Text = "Stock"
        '
        'ControlDeInventarioToolStripMenuItem
        '
        Me.ControlDeInventarioToolStripMenuItem.Name = "ControlDeInventarioToolStripMenuItem"
        Me.ControlDeInventarioToolStripMenuItem.Size = New System.Drawing.Size(232, 26)
        Me.ControlDeInventarioToolStripMenuItem.Text = "Control de Inventario"
        '
        'CargardeMermasToolStripMenuItem
        '
        Me.CargardeMermasToolStripMenuItem.Name = "CargardeMermasToolStripMenuItem"
        Me.CargardeMermasToolStripMenuItem.Size = New System.Drawing.Size(232, 26)
        Me.CargardeMermasToolStripMenuItem.Text = "Carga de mermas"
        '
        'AdministrarToolStripMenuItem
        '
        Me.AdministrarToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AdministrarPromocionesToolStripMenuItem})
        Me.AdministrarToolStripMenuItem.Image = CType(resources.GetObject("AdministrarToolStripMenuItem.Image"), System.Drawing.Image)
        Me.AdministrarToolStripMenuItem.Name = "AdministrarToolStripMenuItem"
        Me.AdministrarToolStripMenuItem.Size = New System.Drawing.Size(120, 34)
        Me.AdministrarToolStripMenuItem.Text = "Administrar"
        '
        'AdministrarPromocionesToolStripMenuItem
        '
        Me.AdministrarPromocionesToolStripMenuItem.Name = "AdministrarPromocionesToolStripMenuItem"
        Me.AdministrarPromocionesToolStripMenuItem.Size = New System.Drawing.Size(259, 26)
        Me.AdministrarPromocionesToolStripMenuItem.Text = "Administrar Promociones"
        '
        'StatusStrip
        '
        Me.StatusStrip.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.StatusStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripStatusLabelEmpresa, Me.ToolStripStatusLabelEstablecimiento, Me.ToolStripStatusLabelUsuario})
        Me.StatusStrip.Location = New System.Drawing.Point(0, 564)
        Me.StatusStrip.Name = "StatusStrip"
        Me.StatusStrip.Padding = New System.Windows.Forms.Padding(1, 0, 19, 0)
        Me.StatusStrip.Size = New System.Drawing.Size(1093, 26)
        Me.StatusStrip.TabIndex = 7
        Me.StatusStrip.Text = "StatusStrip"
        '
        'ToolStripStatusLabelEmpresa
        '
        Me.ToolStripStatusLabelEmpresa.Name = "ToolStripStatusLabelEmpresa"
        Me.ToolStripStatusLabelEmpresa.Size = New System.Drawing.Size(357, 20)
        Me.ToolStripStatusLabelEmpresa.Spring = True
        Me.ToolStripStatusLabelEmpresa.Text = "Empresa"
        Me.ToolStripStatusLabelEmpresa.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ToolStripStatusLabelEstablecimiento
        '
        Me.ToolStripStatusLabelEstablecimiento.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ToolStripStatusLabelEstablecimiento.Name = "ToolStripStatusLabelEstablecimiento"
        Me.ToolStripStatusLabelEstablecimiento.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.ToolStripStatusLabelEstablecimiento.Size = New System.Drawing.Size(357, 20)
        Me.ToolStripStatusLabelEstablecimiento.Spring = True
        Me.ToolStripStatusLabelEstablecimiento.Text = "Establecimiento"
        '
        'ToolStripStatusLabelUsuario
        '
        Me.ToolStripStatusLabelUsuario.Name = "ToolStripStatusLabelUsuario"
        Me.ToolStripStatusLabelUsuario.Size = New System.Drawing.Size(357, 20)
        Me.ToolStripStatusLabelUsuario.Spring = True
        Me.ToolStripStatusLabelUsuario.Text = "Usuario"
        Me.ToolStripStatusLabelUsuario.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'ImgBackground
        '
        Me.ImgBackground.BackColor = System.Drawing.Color.Gray
        Me.ImgBackground.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ImgBackground.Image = CType(resources.GetObject("ImgBackground.Image"), System.Drawing.Image)
        Me.ImgBackground.Location = New System.Drawing.Point(0, 38)
        Me.ImgBackground.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.ImgBackground.Name = "ImgBackground"
        Me.ImgBackground.Size = New System.Drawing.Size(1093, 526)
        Me.ImgBackground.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage
        Me.ImgBackground.TabIndex = 9
        Me.ImgBackground.TabStop = False
        '
        'AsientosDeCierreDeMesToolStripMenuItem
        '
        Me.AsientosDeCierreDeMesToolStripMenuItem.Name = "AsientosDeCierreDeMesToolStripMenuItem"
        Me.AsientosDeCierreDeMesToolStripMenuItem.Size = New System.Drawing.Size(286, 26)
        Me.AsientosDeCierreDeMesToolStripMenuItem.Text = "Asientos de cierre de mes"
        '
        'MDIParent1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1093, 590)
        Me.Controls.Add(Me.ImgBackground)
        Me.Controls.Add(Me.MenuStrip)
        Me.Controls.Add(Me.StatusStrip)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.IsMdiContainer = True
        Me.MainMenuStrip = Me.MenuStrip
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.Name = "MDIParent1"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Luz Azul Gestion"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.MenuStrip.ResumeLayout(False)
        Me.MenuStrip.PerformLayout()
        Me.StatusStrip.ResumeLayout(False)
        Me.StatusStrip.PerformLayout()
        CType(Me.ImgBackground, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ToolTip As System.Windows.Forms.ToolTip
    Friend WithEvents ToolStripStatusLabelEmpresa As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents StatusStrip As System.Windows.Forms.StatusStrip
    Friend WithEvents MenuStrip As System.Windows.Forms.MenuStrip
    Friend WithEvents ImgBackground As PictureBox
    Friend WithEvents InicioToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents EnviarPedidosToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents RecepcionPedidosToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolStripStatusLabelEstablecimiento As ToolStripStatusLabel
    Friend WithEvents ToolStripStatusLabelUsuario As ToolStripStatusLabel
    Friend WithEvents ConfiguracionesToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ConfigurarFacturacionEstablecimientoToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents FacturacionToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents FacturacionAutomaticaToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents GenerarExcelDePruebasToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents GenerarArchivoTXTToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents EnviarPedidoClienteToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents AprobarPedidosToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ComprasToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents RecepcionFacturasToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents PickeoPedidosToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents GenerarOrdenPickeoToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents VerOrdenesPickeoToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents AdministrarOrdenesToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents AgruparOrdenesToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ConfigurarProductosSenasaToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ConfigurarEmpaquesToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ConfigurarFamiliasToleranciasPickeoToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents AdministrarToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents AdministrarPromocionesToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents StockToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ControlDeInventarioToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents CargardeMermasToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents AsientosDeCierreDeMesToolStripMenuItem As ToolStripMenuItem
End Class
