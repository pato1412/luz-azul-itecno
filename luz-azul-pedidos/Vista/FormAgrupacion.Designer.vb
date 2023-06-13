<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FormAgrupacion
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
        Me.BtnSalir = New System.Windows.Forms.Button()
        Me.BtnGuardar = New System.Windows.Forms.Button()
        Me.DbGridAgrupacion = New System.Windows.Forms.DataGridView()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.LblUnidadesXBulto = New System.Windows.Forms.Label()
        Me.LblUnidad = New System.Windows.Forms.Label()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.LblAgrupacion = New System.Windows.Forms.Label()
        Me.LblTotalUnidades = New System.Windows.Forms.Label()
        CType(Me.DbGridAgrupacion, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.SuspendLayout()
        '
        'BtnSalir
        '
        Me.BtnSalir.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(70, Byte), Integer), CType(CType(148, Byte), Integer))
        Me.BtnSalir.Cursor = System.Windows.Forms.Cursors.Hand
        Me.BtnSalir.FlatAppearance.BorderSize = 2
        Me.BtnSalir.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnSalir.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnSalir.ForeColor = System.Drawing.Color.White
        Me.BtnSalir.Location = New System.Drawing.Point(242, 307)
        Me.BtnSalir.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.BtnSalir.Name = "BtnSalir"
        Me.BtnSalir.Size = New System.Drawing.Size(147, 40)
        Me.BtnSalir.TabIndex = 10
        Me.BtnSalir.Text = "Salir"
        Me.BtnSalir.UseVisualStyleBackColor = False
        '
        'BtnGuardar
        '
        Me.BtnGuardar.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(70, Byte), Integer), CType(CType(148, Byte), Integer))
        Me.BtnGuardar.Cursor = System.Windows.Forms.Cursors.Hand
        Me.BtnGuardar.FlatAppearance.BorderSize = 2
        Me.BtnGuardar.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnGuardar.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnGuardar.ForeColor = System.Drawing.Color.White
        Me.BtnGuardar.Location = New System.Drawing.Point(71, 307)
        Me.BtnGuardar.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.BtnGuardar.Name = "BtnGuardar"
        Me.BtnGuardar.Size = New System.Drawing.Size(147, 40)
        Me.BtnGuardar.TabIndex = 9
        Me.BtnGuardar.Text = "Guardar"
        Me.BtnGuardar.UseVisualStyleBackColor = False
        '
        'DbGridAgrupacion
        '
        Me.DbGridAgrupacion.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DbGridAgrupacion.Location = New System.Drawing.Point(11, 94)
        Me.DbGridAgrupacion.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.DbGridAgrupacion.Name = "DbGridAgrupacion"
        Me.DbGridAgrupacion.RowHeadersWidth = 51
        Me.DbGridAgrupacion.RowTemplate.Height = 24
        Me.DbGridAgrupacion.Size = New System.Drawing.Size(433, 180)
        Me.DbGridAgrupacion.TabIndex = 11
        '
        'Panel1
        '
        Me.Panel1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel1.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(70, Byte), Integer), CType(CType(148, Byte), Integer))
        Me.Panel1.Controls.Add(Me.LblUnidadesXBulto)
        Me.Panel1.Controls.Add(Me.LblUnidad)
        Me.Panel1.Location = New System.Drawing.Point(11, 47)
        Me.Panel1.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(433, 38)
        Me.Panel1.TabIndex = 12
        '
        'LblUnidadesXBulto
        '
        Me.LblUnidadesXBulto.AutoSize = True
        Me.LblUnidadesXBulto.Font = New System.Drawing.Font("Arial", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblUnidadesXBulto.ForeColor = System.Drawing.Color.White
        Me.LblUnidadesXBulto.Location = New System.Drawing.Point(211, 7)
        Me.LblUnidadesXBulto.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.LblUnidadesXBulto.Name = "LblUnidadesXBulto"
        Me.LblUnidadesXBulto.Size = New System.Drawing.Size(228, 18)
        Me.LblUnidadesXBulto.TabIndex = 10
        Me.LblUnidadesXBulto.Text = "Cargando Unidades por bulto..."
        Me.LblUnidadesXBulto.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'LblUnidad
        '
        Me.LblUnidad.AutoSize = True
        Me.LblUnidad.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(70, Byte), Integer), CType(CType(148, Byte), Integer))
        Me.LblUnidad.Font = New System.Drawing.Font("Arial", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblUnidad.ForeColor = System.Drawing.Color.White
        Me.LblUnidad.Location = New System.Drawing.Point(2, 8)
        Me.LblUnidad.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.LblUnidad.Name = "LblUnidad"
        Me.LblUnidad.Size = New System.Drawing.Size(218, 18)
        Me.LblUnidad.TabIndex = 2
        Me.LblUnidad.Text = "Cargando Unidad de pedido..."
        Me.LblUnidad.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Panel2
        '
        Me.Panel2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel2.BackColor = System.Drawing.Color.FromArgb(CType(CType(170, Byte), Integer), CType(CType(199, Byte), Integer), CType(CType(44, Byte), Integer))
        Me.Panel2.Controls.Add(Me.LblAgrupacion)
        Me.Panel2.Location = New System.Drawing.Point(11, 6)
        Me.Panel2.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(433, 37)
        Me.Panel2.TabIndex = 13
        '
        'LblAgrupacion
        '
        Me.LblAgrupacion.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.LblAgrupacion.BackColor = System.Drawing.Color.FromArgb(CType(CType(170, Byte), Integer), CType(CType(199, Byte), Integer), CType(CType(44, Byte), Integer))
        Me.LblAgrupacion.Font = New System.Drawing.Font("Arial", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblAgrupacion.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(70, Byte), Integer), CType(CType(148, Byte), Integer))
        Me.LblAgrupacion.Location = New System.Drawing.Point(2, 4)
        Me.LblAgrupacion.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.LblAgrupacion.Name = "LblAgrupacion"
        Me.LblAgrupacion.Size = New System.Drawing.Size(428, 27)
        Me.LblAgrupacion.TabIndex = 2
        Me.LblAgrupacion.Text = "Agrupacion"
        Me.LblAgrupacion.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'LblTotalUnidades
        '
        Me.LblTotalUnidades.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.LblTotalUnidades.AutoSize = True
        Me.LblTotalUnidades.Font = New System.Drawing.Font("Arial", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblTotalUnidades.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(70, Byte), Integer), CType(CType(148, Byte), Integer))
        Me.LblTotalUnidades.Location = New System.Drawing.Point(251, 280)
        Me.LblTotalUnidades.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.LblTotalUnidades.Name = "LblTotalUnidades"
        Me.LblTotalUnidades.Size = New System.Drawing.Size(122, 18)
        Me.LblTotalUnidades.TabIndex = 14
        Me.LblTotalUnidades.Text = "Total Unidades: "
        Me.LblTotalUnidades.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'FormAgrupacion
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(453, 356)
        Me.Controls.Add(Me.LblTotalUnidades)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.DbGridAgrupacion)
        Me.Controls.Add(Me.BtnSalir)
        Me.Controls.Add(Me.BtnGuardar)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "FormAgrupacion"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Productos Agrupados"
        CType(Me.DbGridAgrupacion, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents BtnSalir As Button
    Friend WithEvents BtnGuardar As Button
    Friend WithEvents DbGridAgrupacion As DataGridView
    Friend WithEvents Panel1 As Panel
    Friend WithEvents LblUnidadesXBulto As Label
    Friend WithEvents LblUnidad As Label
    Friend WithEvents Panel2 As Panel
    Friend WithEvents LblAgrupacion As Label
    Friend WithEvents LblTotalUnidades As Label
End Class
