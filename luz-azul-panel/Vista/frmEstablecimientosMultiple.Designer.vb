<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmEstablecimientosMultiple
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmEstablecimientosMultiple))
        Me.Label1 = New System.Windows.Forms.Label()
        Me.BtnIngresar = New System.Windows.Forms.Button()
        Me.BtnSalir = New System.Windows.Forms.Button()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.LblRespuesta = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.PictureBox2 = New System.Windows.Forms.PictureBox()
        Me.chkListEstablecimientos = New System.Windows.Forms.CheckedListBox()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Arial", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(70, Byte), Integer), CType(CType(148, Byte), Integer))
        Me.Label1.Location = New System.Drawing.Point(91, 218)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(155, 22)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Establecimientos"
        '
        'BtnIngresar
        '
        Me.BtnIngresar.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(70, Byte), Integer), CType(CType(148, Byte), Integer))
        Me.BtnIngresar.Cursor = System.Windows.Forms.Cursors.Hand
        Me.BtnIngresar.FlatAppearance.BorderSize = 2
        Me.BtnIngresar.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnIngresar.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnIngresar.ForeColor = System.Drawing.Color.White
        Me.BtnIngresar.Location = New System.Drawing.Point(44, 396)
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
        Me.BtnSalir.Location = New System.Drawing.Point(257, 396)
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
        Me.Label3.Location = New System.Drawing.Point(-1, 108)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(504, 32)
        Me.Label3.TabIndex = 7
        Me.Label3.Text = "Seleccione Establecimiento"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'LblRespuesta
        '
        Me.LblRespuesta.AutoSize = True
        Me.LblRespuesta.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LblRespuesta.ForeColor = System.Drawing.Color.FromArgb(CType(CType(168, Byte), Integer), CType(CType(207, Byte), Integer), CType(CType(69, Byte), Integer))
        Me.LblRespuesta.Location = New System.Drawing.Point(45, 320)
        Me.LblRespuesta.Name = "LblRespuesta"
        Me.LblRespuesta.Size = New System.Drawing.Size(0, 24)
        Me.LblRespuesta.TabIndex = 8
        '
        'Label2
        '
        Me.Label2.Font = New System.Drawing.Font("Arial", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(70, Byte), Integer), CType(CType(148, Byte), Integer))
        Me.Label2.Location = New System.Drawing.Point(-1, 148)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(504, 39)
        Me.Label2.TabIndex = 11
        Me.Label2.Text = "Por favor seleccione para que establecimiento " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "desea realizar el pedido"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), System.Drawing.Image)
        Me.PictureBox1.Location = New System.Drawing.Point(-1, 6)
        Me.PictureBox1.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(504, 94)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage
        Me.PictureBox1.TabIndex = 2
        Me.PictureBox1.TabStop = False
        '
        'PictureBox2
        '
        Me.PictureBox2.Image = CType(resources.GetObject("PictureBox2.Image"), System.Drawing.Image)
        Me.PictureBox2.Location = New System.Drawing.Point(49, 209)
        Me.PictureBox2.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.PictureBox2.Name = "PictureBox2"
        Me.PictureBox2.Size = New System.Drawing.Size(35, 28)
        Me.PictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox2.TabIndex = 12
        Me.PictureBox2.TabStop = False
        '
        'chkListEstablecimientos
        '
        Me.chkListEstablecimientos.CheckOnClick = True
        Me.chkListEstablecimientos.FormattingEnabled = True
        Me.chkListEstablecimientos.Location = New System.Drawing.Point(44, 255)
        Me.chkListEstablecimientos.Name = "chkListEstablecimientos"
        Me.chkListEstablecimientos.Size = New System.Drawing.Size(409, 123)
        Me.chkListEstablecimientos.TabIndex = 13
        '
        'frmEstablecimientosMultiple
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(500, 465)
        Me.Controls.Add(Me.chkListEstablecimientos)
        Me.Controls.Add(Me.PictureBox2)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.LblRespuesta)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.BtnSalir)
        Me.Controls.Add(Me.BtnIngresar)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.Label1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmEstablecimientosMultiple"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Luz Azul Pedidos"
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).EndInit()
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
    Friend WithEvents chkListEstablecimientos As CheckedListBox
End Class
