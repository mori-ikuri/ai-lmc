<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmThaiJuchu
    Inherits System.Windows.Forms.Form

    'フォームがコンポーネントの一覧をクリーンアップするために dispose をオーバーライドします。
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Windows フォーム デザイナで必要です。
    Private components As System.ComponentModel.IContainer

    'メモ: 以下のプロシージャは Windows フォーム デザイナで必要です。
    'Windows フォーム デザイナを使用して変更できます。
    'コード エディタを使って変更しないでください。
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.lblTokuisaki = New System.Windows.Forms.Label
        Me.cboTokuisaki = New System.Windows.Forms.ComboBox
        Me.chkKikan = New System.Windows.Forms.CheckBox
        Me.dtpFrom = New System.Windows.Forms.DateTimePicker
        Me.lblKara = New System.Windows.Forms.Label
        Me.dtpTo = New System.Windows.Forms.DateTimePicker
        Me.lblHinban = New System.Windows.Forms.Label
        Me.txtHinban = New System.Windows.Forms.TextBox
        Me.btnKensaku = New System.Windows.Forms.Button
        Me.dgvIchiran = New System.Windows.Forms.DataGridView
        Me.lblKensu = New System.Windows.Forms.Label
        Me.btnClose = New System.Windows.Forms.Button
        CType(Me.dgvIchiran, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lblTokuisaki
        '
        Me.lblTokuisaki.AutoSize = True
        Me.lblTokuisaki.Location = New System.Drawing.Point(12, 15)
        Me.lblTokuisaki.Name = "lblTokuisaki"
        Me.lblTokuisaki.Size = New System.Drawing.Size(41, 12)
        Me.lblTokuisaki.TabIndex = 0
        Me.lblTokuisaki.Text = "得意先"
        '
        'cboTokuisaki
        '
        Me.cboTokuisaki.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboTokuisaki.FormattingEnabled = True
        Me.cboTokuisaki.Location = New System.Drawing.Point(60, 12)
        Me.cboTokuisaki.Name = "cboTokuisaki"
        Me.cboTokuisaki.Size = New System.Drawing.Size(220, 20)
        Me.cboTokuisaki.TabIndex = 1
        '
        'chkKikan
        '
        Me.chkKikan.AutoSize = True
        Me.chkKikan.Checked = True
        Me.chkKikan.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkKikan.Location = New System.Drawing.Point(296, 15)
        Me.chkKikan.Name = "chkKikan"
        Me.chkKikan.Size = New System.Drawing.Size(61, 16)
        Me.chkKikan.TabIndex = 2
        Me.chkKikan.Text = "受注日"
        Me.chkKikan.UseVisualStyleBackColor = True
        '
        'dtpFrom
        '
        Me.dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Short
        Me.dtpFrom.Location = New System.Drawing.Point(362, 12)
        Me.dtpFrom.Name = "dtpFrom"
        Me.dtpFrom.Size = New System.Drawing.Size(110, 19)
        Me.dtpFrom.TabIndex = 3
        '
        'lblKara
        '
        Me.lblKara.AutoSize = True
        Me.lblKara.Location = New System.Drawing.Point(478, 15)
        Me.lblKara.Name = "lblKara"
        Me.lblKara.Size = New System.Drawing.Size(20, 12)
        Me.lblKara.TabIndex = 4
        Me.lblKara.Text = "～"
        '
        'dtpTo
        '
        Me.dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.Short
        Me.dtpTo.Location = New System.Drawing.Point(502, 12)
        Me.dtpTo.Name = "dtpTo"
        Me.dtpTo.Size = New System.Drawing.Size(110, 19)
        Me.dtpTo.TabIndex = 5
        '
        'lblHinban
        '
        Me.lblHinban.AutoSize = True
        Me.lblHinban.Location = New System.Drawing.Point(12, 43)
        Me.lblHinban.Name = "lblHinban"
        Me.lblHinban.Size = New System.Drawing.Size(29, 12)
        Me.lblHinban.TabIndex = 6
        Me.lblHinban.Text = "品番"
        '
        'txtHinban
        '
        Me.txtHinban.Location = New System.Drawing.Point(60, 40)
        Me.txtHinban.MaxLength = 20
        Me.txtHinban.Name = "txtHinban"
        Me.txtHinban.Size = New System.Drawing.Size(150, 19)
        Me.txtHinban.TabIndex = 7
        '
        'btnKensaku
        '
        Me.btnKensaku.Location = New System.Drawing.Point(628, 38)
        Me.btnKensaku.Name = "btnKensaku"
        Me.btnKensaku.Size = New System.Drawing.Size(75, 23)
        Me.btnKensaku.TabIndex = 8
        Me.btnKensaku.Text = "検索(&F)"
        Me.btnKensaku.UseVisualStyleBackColor = True
        '
        'dgvIchiran
        '
        Me.dgvIchiran.AllowUserToAddRows = False
        Me.dgvIchiran.AllowUserToDeleteRows = False
        Me.dgvIchiran.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvIchiran.Location = New System.Drawing.Point(12, 70)
        Me.dgvIchiran.MultiSelect = False
        Me.dgvIchiran.Name = "dgvIchiran"
        Me.dgvIchiran.ReadOnly = True
        Me.dgvIchiran.RowHeadersVisible = False
        Me.dgvIchiran.RowTemplate.Height = 21
        Me.dgvIchiran.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvIchiran.Size = New System.Drawing.Size(960, 350)
        Me.dgvIchiran.TabIndex = 9
        '
        'lblKensu
        '
        Me.lblKensu.AutoSize = True
        Me.lblKensu.Location = New System.Drawing.Point(12, 430)
        Me.lblKensu.Name = "lblKensu"
        Me.lblKensu.Size = New System.Drawing.Size(41, 12)
        Me.lblKensu.TabIndex = 10
        Me.lblKensu.Text = "件数 0 件"
        '
        'btnClose
        '
        Me.btnClose.Location = New System.Drawing.Point(897, 428)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(75, 23)
        Me.btnClose.TabIndex = 11
        Me.btnClose.Text = "閉じる(&X)"
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'frmThaiJuchu
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(984, 464)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.lblKensu)
        Me.Controls.Add(Me.dgvIchiran)
        Me.Controls.Add(Me.btnKensaku)
        Me.Controls.Add(Me.txtHinban)
        Me.Controls.Add(Me.lblHinban)
        Me.Controls.Add(Me.dtpTo)
        Me.Controls.Add(Me.lblKara)
        Me.Controls.Add(Me.dtpFrom)
        Me.Controls.Add(Me.chkKikan)
        Me.Controls.Add(Me.cboTokuisaki)
        Me.Controls.Add(Me.lblTokuisaki)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.Name = "frmThaiJuchu"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "タイ受注一覧"
        CType(Me.dgvIchiran, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblTokuisaki As System.Windows.Forms.Label
    Friend WithEvents cboTokuisaki As System.Windows.Forms.ComboBox
    Friend WithEvents chkKikan As System.Windows.Forms.CheckBox
    Friend WithEvents dtpFrom As System.Windows.Forms.DateTimePicker
    Friend WithEvents lblKara As System.Windows.Forms.Label
    Friend WithEvents dtpTo As System.Windows.Forms.DateTimePicker
    Friend WithEvents lblHinban As System.Windows.Forms.Label
    Friend WithEvents txtHinban As System.Windows.Forms.TextBox
    Friend WithEvents btnKensaku As System.Windows.Forms.Button
    Friend WithEvents dgvIchiran As System.Windows.Forms.DataGridView
    Friend WithEvents lblKensu As System.Windows.Forms.Label
    Friend WithEvents btnClose As System.Windows.Forms.Button
End Class
