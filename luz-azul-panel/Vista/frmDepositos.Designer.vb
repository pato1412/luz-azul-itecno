<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmDepositos
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
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
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.BtnSalir = New System.Windows.Forms.Button()
        Me.ChkListDepositos = New System.Windows.Forms.CheckedListBox()
        Me.btnSeleccionar = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'Label3
        '
        Me.Label3.Font = New System.Drawing.Font("Arial", 16.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(70, Byte), Integer), CType(CType(148, Byte), Integer))
        Me.Label3.Location = New System.Drawing.Point(12, 9)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(414, 32)
        Me.Label3.TabIndex = 27
        Me.Label3.Text = "Depositos"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'BtnSalir
        '
        Me.BtnSalir.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(70, Byte), Integer), CType(CType(148, Byte), Integer))
        Me.BtnSalir.Cursor = System.Windows.Forms.Cursors.Hand
        Me.BtnSalir.FlatAppearance.BorderSize = 2
        Me.BtnSalir.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.BtnSalir.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BtnSalir.ForeColor = System.Drawing.Color.White
        Me.BtnSalir.Location = New System.Drawing.Point(19, 250)
        Me.BtnSalir.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.BtnSalir.Name = "BtnSalir"
        Me.BtnSalir.Size = New System.Drawing.Size(196, 49)
        Me.BtnSalir.TabIndex = 26
        Me.BtnSalir.Text = "Salir"
        Me.BtnSalir.UseVisualStyleBackColor = False
        '
        'ChkListDepositos
        '
        Me.ChkListDepositos.CheckOnClick = True
        Me.ChkListDepositos.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ChkListDepositos.FormattingEnabled = True
        Me.ChkListDepositos.Location = New System.Drawing.Point(19, 53)
        Me.ChkListDepositos.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.ChkListDepositos.Name = "ChkListDepositos"
        Me.ChkListDepositos.Size = New System.Drawing.Size(414, 172)
        Me.ChkListDepositos.TabIndex = 28
        '
        'btnSeleccionar
        '
        Me.btnSeleccionar.BackColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(70, Byte), Integer), CType(CType(148, Byte), Integer))
        Me.btnSeleccionar.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnSeleccionar.FlatAppearance.BorderSize = 2
        Me.btnSeleccionar.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSeleccionar.Font = New System.Drawing.Font("Arial", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnSeleccionar.ForeColor = System.Drawing.Color.White
        Me.btnSeleccionar.Location = New System.Drawing.Point(237, 250)
        Me.btnSeleccionar.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
        Me.btnSeleccionar.Name = "btnSeleccionar"
        Me.btnSeleccionar.Size = New System.Drawing.Size(196, 49)
        Me.btnSeleccionar.TabIndex = 29
        Me.btnSeleccionar.Text = "Seleccionar"
        Me.btnSeleccionar.UseVisualStyleBackColor = False
        '
        'frmDepositos
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(446, 310)
        Me.Controls.Add(Me.btnSeleccionar)
        Me.Controls.Add(Me.ChkListDepositos)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.BtnSalir)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmDepositos"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Depositos"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Label3 As Label
    Friend WithEvents BtnSalir As Button
    Friend WithEvents ChkListDepositos As CheckedListBox
    Friend WithEvents btnSeleccionar As Button
End Class
