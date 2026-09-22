<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmNyukin
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
        Me.lblSeikyusaki = New System.Windows.Forms.Label
        Me.cboSeikyusaki = New System.Windows.Forms.ComboBox
        Me.lblZan = New System.Windows.Forms.Label
        Me.grpNyuryoku = New System.Windows.Forms.GroupBox
        Me.lblNyukinbi = New System.Windows.Forms.Label
        Me.dtpNyukinbi = New System.Windows.Forms.DateTimePicker
        Me.lblKingaku = New System.Windows.Forms.Label
        Me.txtKingaku = New System.Windows.Forms.TextBox
        Me.lblHoho = New System.Windows.Forms.Label
        Me.cboHoho = New System.Windows.Forms.ComboBox
        Me.lblBiko = New System.Windows.Forms.Label
        Me.txtBiko = New System.Windows.Forms.TextBox
        Me.btnToroku = New System.Windows.Forms.Button
        Me.btnClear = New System.Windows.Forms.Button
        Me.lblSeikyuIchiran = New System.Windows.Forms.Label
        Me.dgvSeikyu = New System.Windows.Forms.DataGridView
        Me.lblNyukinIchiran = New System.Windows.Forms.Label
        Me.lblKensu = New System.Windows.Forms.Label
        Me.dgvNyukin = New System.Windows.Forms.DataGridView
        Me.btnTorikeshi = New System.Windows.Forms.Button
        Me.btnClose = New System.Windows.Forms.Button
        Me.grpNyuryoku.SuspendLayout()
        CType(Me.dgvSeikyu, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgvNyukin, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lblSeikyusaki
        '
        Me.lblSeikyusaki.AutoSize = True
        Me.lblSeikyusaki.Location = New System.Drawing.Point(12, 15)
        Me.lblSeikyusaki.Name = "lblSeikyusaki"
        Me.lblSeikyusaki.Size = New System.Drawing.Size(41, 12)
        Me.lblSeikyusaki.TabIndex = 0
        Me.lblSeikyusaki.Text = "請求先"
        '
        'cboSeikyusaki
        '
        Me.cboSeikyusaki.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboSeikyusaki.FormattingEnabled = True
        Me.cboSeikyusaki.Location = New System.Drawing.Point(60, 12)
        Me.cboSeikyusaki.Name = "cboSeikyusaki"
        Me.cboSeikyusaki.Size = New System.Drawing.Size(260, 20)
        Me.cboSeikyusaki.TabIndex = 1
        '
        'lblZan
        '
        Me.lblZan.AutoSize = True
        Me.lblZan.Location = New System.Drawing.Point(340, 15)
        Me.lblZan.Name = "lblZan"
        Me.lblZan.Size = New System.Drawing.Size(0, 12)
        Me.lblZan.TabIndex = 2
        '
        'grpNyuryoku
        '
        Me.grpNyuryoku.Controls.Add(Me.lblNyukinbi)
        Me.grpNyuryoku.Controls.Add(Me.dtpNyukinbi)
        Me.grpNyuryoku.Controls.Add(Me.lblKingaku)
        Me.grpNyuryoku.Controls.Add(Me.txtKingaku)
        Me.grpNyuryoku.Controls.Add(Me.lblHoho)
        Me.grpNyuryoku.Controls.Add(Me.cboHoho)
        Me.grpNyuryoku.Controls.Add(Me.lblBiko)
        Me.grpNyuryoku.Controls.Add(Me.txtBiko)
        Me.grpNyuryoku.Location = New System.Drawing.Point(12, 42)
        Me.grpNyuryoku.Name = "grpNyuryoku"
        Me.grpNyuryoku.Size = New System.Drawing.Size(860, 66)
        Me.grpNyuryoku.TabIndex = 3
        Me.grpNyuryoku.TabStop = False
        Me.grpNyuryoku.Text = "入力"
        '
        'lblNyukinbi
        '
        Me.lblNyukinbi.AutoSize = True
        Me.lblNyukinbi.Location = New System.Drawing.Point(16, 28)
        Me.lblNyukinbi.Name = "lblNyukinbi"
        Me.lblNyukinbi.Size = New System.Drawing.Size(41, 12)
        Me.lblNyukinbi.TabIndex = 0
        Me.lblNyukinbi.Text = "入金日"
        '
        'dtpNyukinbi
        '
        Me.dtpNyukinbi.Format = System.Windows.Forms.DateTimePickerFormat.Short
        Me.dtpNyukinbi.Location = New System.Drawing.Point(66, 25)
        Me.dtpNyukinbi.Name = "dtpNyukinbi"
        Me.dtpNyukinbi.Size = New System.Drawing.Size(120, 19)
        Me.dtpNyukinbi.TabIndex = 1
        '
        'lblKingaku
        '
        Me.lblKingaku.AutoSize = True
        Me.lblKingaku.Location = New System.Drawing.Point(206, 28)
        Me.lblKingaku.Name = "lblKingaku"
        Me.lblKingaku.Size = New System.Drawing.Size(41, 12)
        Me.lblKingaku.TabIndex = 2
        Me.lblKingaku.Text = "入金額"
        '
        'txtKingaku
        '
        Me.txtKingaku.Location = New System.Drawing.Point(256, 25)
        Me.txtKingaku.MaxLength = 13
        Me.txtKingaku.Name = "txtKingaku"
        Me.txtKingaku.Size = New System.Drawing.Size(120, 19)
        Me.txtKingaku.TabIndex = 3
        Me.txtKingaku.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblHoho
        '
        Me.lblHoho.AutoSize = True
        Me.lblHoho.Location = New System.Drawing.Point(396, 28)
        Me.lblHoho.Name = "lblHoho"
        Me.lblHoho.Size = New System.Drawing.Size(29, 12)
        Me.lblHoho.TabIndex = 4
        Me.lblHoho.Text = "方法"
        '
        'cboHoho
        '
        Me.cboHoho.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboHoho.FormattingEnabled = True
        Me.cboHoho.Location = New System.Drawing.Point(436, 25)
        Me.cboHoho.Name = "cboHoho"
        Me.cboHoho.Size = New System.Drawing.Size(80, 20)
        Me.cboHoho.TabIndex = 5
        '
        'lblBiko
        '
        Me.lblBiko.AutoSize = True
        Me.lblBiko.Location = New System.Drawing.Point(536, 28)
        Me.lblBiko.Name = "lblBiko"
        Me.lblBiko.Size = New System.Drawing.Size(29, 12)
        Me.lblBiko.TabIndex = 6
        Me.lblBiko.Text = "備考"
        '
        'txtBiko
        '
        Me.txtBiko.Location = New System.Drawing.Point(576, 25)
        Me.txtBiko.MaxLength = 100
        Me.txtBiko.Name = "txtBiko"
        Me.txtBiko.Size = New System.Drawing.Size(266, 19)
        Me.txtBiko.TabIndex = 7
        '
        'btnToroku
        '
        Me.btnToroku.Location = New System.Drawing.Point(716, 116)
        Me.btnToroku.Name = "btnToroku"
        Me.btnToroku.Size = New System.Drawing.Size(75, 23)
        Me.btnToroku.TabIndex = 4
        Me.btnToroku.Text = "登録(&S)"
        Me.btnToroku.UseVisualStyleBackColor = True
        '
        'btnClear
        '
        Me.btnClear.Location = New System.Drawing.Point(797, 116)
        Me.btnClear.Name = "btnClear"
        Me.btnClear.Size = New System.Drawing.Size(75, 23)
        Me.btnClear.TabIndex = 5
        Me.btnClear.Text = "クリア(&C)"
        Me.btnClear.UseVisualStyleBackColor = True
        '
        'lblSeikyuIchiran
        '
        Me.lblSeikyuIchiran.AutoSize = True
        Me.lblSeikyuIchiran.Location = New System.Drawing.Point(12, 150)
        Me.lblSeikyuIchiran.Name = "lblSeikyuIchiran"
        Me.lblSeikyuIchiran.Size = New System.Drawing.Size(29, 12)
        Me.lblSeikyuIchiran.TabIndex = 6
        Me.lblSeikyuIchiran.Text = "請求"
        '
        'dgvSeikyu
        '
        Me.dgvSeikyu.AllowUserToAddRows = False
        Me.dgvSeikyu.AllowUserToDeleteRows = False
        Me.dgvSeikyu.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvSeikyu.Location = New System.Drawing.Point(12, 166)
        Me.dgvSeikyu.MultiSelect = False
        Me.dgvSeikyu.Name = "dgvSeikyu"
        Me.dgvSeikyu.ReadOnly = True
        Me.dgvSeikyu.RowHeadersVisible = False
        Me.dgvSeikyu.RowTemplate.Height = 21
        Me.dgvSeikyu.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvSeikyu.Size = New System.Drawing.Size(860, 170)
        Me.dgvSeikyu.TabIndex = 7
        '
        'lblNyukinIchiran
        '
        Me.lblNyukinIchiran.AutoSize = True
        Me.lblNyukinIchiran.Location = New System.Drawing.Point(12, 346)
        Me.lblNyukinIchiran.Name = "lblNyukinIchiran"
        Me.lblNyukinIchiran.Size = New System.Drawing.Size(29, 12)
        Me.lblNyukinIchiran.TabIndex = 8
        Me.lblNyukinIchiran.Text = "入金"
        '
        'lblKensu
        '
        Me.lblKensu.AutoSize = True
        Me.lblKensu.Location = New System.Drawing.Point(70, 346)
        Me.lblKensu.Name = "lblKensu"
        Me.lblKensu.Size = New System.Drawing.Size(41, 12)
        Me.lblKensu.TabIndex = 9
        Me.lblKensu.Text = "入金 0 件"
        '
        'dgvNyukin
        '
        Me.dgvNyukin.AllowUserToAddRows = False
        Me.dgvNyukin.AllowUserToDeleteRows = False
        Me.dgvNyukin.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvNyukin.Location = New System.Drawing.Point(12, 362)
        Me.dgvNyukin.MultiSelect = False
        Me.dgvNyukin.Name = "dgvNyukin"
        Me.dgvNyukin.ReadOnly = True
        Me.dgvNyukin.RowHeadersVisible = False
        Me.dgvNyukin.RowTemplate.Height = 21
        Me.dgvNyukin.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvNyukin.Size = New System.Drawing.Size(860, 170)
        Me.dgvNyukin.TabIndex = 10
        '
        'btnTorikeshi
        '
        Me.btnTorikeshi.Location = New System.Drawing.Point(716, 540)
        Me.btnTorikeshi.Name = "btnTorikeshi"
        Me.btnTorikeshi.Size = New System.Drawing.Size(75, 23)
        Me.btnTorikeshi.TabIndex = 11
        Me.btnTorikeshi.Text = "取消(&D)"
        Me.btnTorikeshi.UseVisualStyleBackColor = True
        '
        'btnClose
        '
        Me.btnClose.Location = New System.Drawing.Point(797, 540)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(75, 23)
        Me.btnClose.TabIndex = 12
        Me.btnClose.Text = "閉じる(&X)"
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'frmNyukin
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(884, 576)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.btnTorikeshi)
        Me.Controls.Add(Me.dgvNyukin)
        Me.Controls.Add(Me.lblKensu)
        Me.Controls.Add(Me.lblNyukinIchiran)
        Me.Controls.Add(Me.dgvSeikyu)
        Me.Controls.Add(Me.lblSeikyuIchiran)
        Me.Controls.Add(Me.btnClear)
        Me.Controls.Add(Me.btnToroku)
        Me.Controls.Add(Me.grpNyuryoku)
        Me.Controls.Add(Me.lblZan)
        Me.Controls.Add(Me.cboSeikyusaki)
        Me.Controls.Add(Me.lblSeikyusaki)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.Name = "frmNyukin"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "入金入力"
        Me.grpNyuryoku.ResumeLayout(False)
        Me.grpNyuryoku.PerformLayout()
        CType(Me.dgvSeikyu, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dgvNyukin, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblSeikyusaki As System.Windows.Forms.Label
    Friend WithEvents cboSeikyusaki As System.Windows.Forms.ComboBox
    Friend WithEvents lblZan As System.Windows.Forms.Label
    Friend WithEvents grpNyuryoku As System.Windows.Forms.GroupBox
    Friend WithEvents lblNyukinbi As System.Windows.Forms.Label
    Friend WithEvents dtpNyukinbi As System.Windows.Forms.DateTimePicker
    Friend WithEvents lblKingaku As System.Windows.Forms.Label
    Friend WithEvents txtKingaku As System.Windows.Forms.TextBox
    Friend WithEvents lblHoho As System.Windows.Forms.Label
    Friend WithEvents cboHoho As System.Windows.Forms.ComboBox
    Friend WithEvents lblBiko As System.Windows.Forms.Label
    Friend WithEvents txtBiko As System.Windows.Forms.TextBox
    Friend WithEvents btnToroku As System.Windows.Forms.Button
    Friend WithEvents btnClear As System.Windows.Forms.Button
    Friend WithEvents lblSeikyuIchiran As System.Windows.Forms.Label
    Friend WithEvents dgvSeikyu As System.Windows.Forms.DataGridView
    Friend WithEvents lblNyukinIchiran As System.Windows.Forms.Label
    Friend WithEvents lblKensu As System.Windows.Forms.Label
    Friend WithEvents dgvNyukin As System.Windows.Forms.DataGridView
    Friend WithEvents btnTorikeshi As System.Windows.Forms.Button
    Friend WithEvents btnClose As System.Windows.Forms.Button
End Class
