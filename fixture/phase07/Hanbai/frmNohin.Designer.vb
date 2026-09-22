<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmNohin
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
        Me.chkZanAri = New System.Windows.Forms.CheckBox
        Me.btnKensaku = New System.Windows.Forms.Button
        Me.lblMarume = New System.Windows.Forms.Label
        Me.dgvJuchu = New System.Windows.Forms.DataGridView
        Me.lblKensu = New System.Windows.Forms.Label
        Me.grpNyuryoku = New System.Windows.Forms.GroupBox
        Me.lblDenpyoNo = New System.Windows.Forms.Label
        Me.txtDenpyoNo = New System.Windows.Forms.TextBox
        Me.lblNohinbi = New System.Windows.Forms.Label
        Me.dtpNohinbi = New System.Windows.Forms.DateTimePicker
        Me.lblHinban = New System.Windows.Forms.Label
        Me.txtHinban = New System.Windows.Forms.TextBox
        Me.lblHinmei = New System.Windows.Forms.Label
        Me.txtHinmei = New System.Windows.Forms.TextBox
        Me.lblSuryo = New System.Windows.Forms.Label
        Me.txtSuryo = New System.Windows.Forms.TextBox
        Me.lblTanka = New System.Windows.Forms.Label
        Me.txtTanka = New System.Windows.Forms.TextBox
        Me.lblKingaku = New System.Windows.Forms.Label
        Me.txtKingaku = New System.Windows.Forms.TextBox
        Me.chkMusho = New System.Windows.Forms.CheckBox
        Me.lblBiko = New System.Windows.Forms.Label
        Me.txtBiko = New System.Windows.Forms.TextBox
        Me.lblChui = New System.Windows.Forms.Label
        Me.btnToroku = New System.Windows.Forms.Button
        Me.btnClear = New System.Windows.Forms.Button
        Me.lblNohinIchiran = New System.Windows.Forms.Label
        Me.dgvNohin = New System.Windows.Forms.DataGridView
        Me.btnTorikeshi = New System.Windows.Forms.Button
        Me.btnClose = New System.Windows.Forms.Button
        CType(Me.dgvJuchu, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpNyuryoku.SuspendLayout()
        CType(Me.dgvNohin, System.ComponentModel.ISupportInitialize).BeginInit()
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
        Me.cboTokuisaki.Size = New System.Drawing.Size(260, 20)
        Me.cboTokuisaki.TabIndex = 1
        '
        'chkZanAri
        '
        Me.chkZanAri.AutoSize = True
        Me.chkZanAri.Checked = True
        Me.chkZanAri.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkZanAri.Location = New System.Drawing.Point(340, 14)
        Me.chkZanAri.Name = "chkZanAri"
        Me.chkZanAri.Size = New System.Drawing.Size(121, 16)
        Me.chkZanAri.TabIndex = 2
        Me.chkZanAri.Text = "残のあるものだけ"
        Me.chkZanAri.UseVisualStyleBackColor = True
        '
        'btnKensaku
        '
        Me.btnKensaku.Location = New System.Drawing.Point(480, 10)
        Me.btnKensaku.Name = "btnKensaku"
        Me.btnKensaku.Size = New System.Drawing.Size(75, 23)
        Me.btnKensaku.TabIndex = 3
        Me.btnKensaku.Text = "検索(&F)"
        Me.btnKensaku.UseVisualStyleBackColor = True
        '
        'lblMarume
        '
        Me.lblMarume.AutoSize = True
        Me.lblMarume.Location = New System.Drawing.Point(580, 15)
        Me.lblMarume.Name = "lblMarume"
        Me.lblMarume.Size = New System.Drawing.Size(77, 12)
        Me.lblMarume.TabIndex = 4
        Me.lblMarume.Text = "丸め 切り捨て"
        '
        'dgvJuchu
        '
        Me.dgvJuchu.AllowUserToAddRows = False
        Me.dgvJuchu.AllowUserToDeleteRows = False
        Me.dgvJuchu.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvJuchu.Location = New System.Drawing.Point(12, 42)
        Me.dgvJuchu.MultiSelect = False
        Me.dgvJuchu.Name = "dgvJuchu"
        Me.dgvJuchu.ReadOnly = True
        Me.dgvJuchu.RowHeadersVisible = False
        Me.dgvJuchu.RowTemplate.Height = 21
        Me.dgvJuchu.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvJuchu.Size = New System.Drawing.Size(860, 200)
        Me.dgvJuchu.TabIndex = 5
        '
        'lblKensu
        '
        Me.lblKensu.AutoSize = True
        Me.lblKensu.Location = New System.Drawing.Point(12, 248)
        Me.lblKensu.Name = "lblKensu"
        Me.lblKensu.Size = New System.Drawing.Size(41, 12)
        Me.lblKensu.TabIndex = 6
        Me.lblKensu.Text = "件数 0 件"
        '
        'grpNyuryoku
        '
        Me.grpNyuryoku.Controls.Add(Me.lblDenpyoNo)
        Me.grpNyuryoku.Controls.Add(Me.txtDenpyoNo)
        Me.grpNyuryoku.Controls.Add(Me.lblNohinbi)
        Me.grpNyuryoku.Controls.Add(Me.dtpNohinbi)
        Me.grpNyuryoku.Controls.Add(Me.lblHinban)
        Me.grpNyuryoku.Controls.Add(Me.txtHinban)
        Me.grpNyuryoku.Controls.Add(Me.lblHinmei)
        Me.grpNyuryoku.Controls.Add(Me.txtHinmei)
        Me.grpNyuryoku.Controls.Add(Me.lblSuryo)
        Me.grpNyuryoku.Controls.Add(Me.txtSuryo)
        Me.grpNyuryoku.Controls.Add(Me.lblTanka)
        Me.grpNyuryoku.Controls.Add(Me.txtTanka)
        Me.grpNyuryoku.Controls.Add(Me.lblKingaku)
        Me.grpNyuryoku.Controls.Add(Me.txtKingaku)
        Me.grpNyuryoku.Controls.Add(Me.chkMusho)
        Me.grpNyuryoku.Controls.Add(Me.lblBiko)
        Me.grpNyuryoku.Controls.Add(Me.txtBiko)
        Me.grpNyuryoku.Controls.Add(Me.lblChui)
        Me.grpNyuryoku.Location = New System.Drawing.Point(12, 266)
        Me.grpNyuryoku.Name = "grpNyuryoku"
        Me.grpNyuryoku.Size = New System.Drawing.Size(860, 130)
        Me.grpNyuryoku.TabIndex = 7
        Me.grpNyuryoku.TabStop = False
        Me.grpNyuryoku.Text = "入力"
        '
        'lblDenpyoNo
        '
        Me.lblDenpyoNo.AutoSize = True
        Me.lblDenpyoNo.Location = New System.Drawing.Point(16, 25)
        Me.lblDenpyoNo.Name = "lblDenpyoNo"
        Me.lblDenpyoNo.Size = New System.Drawing.Size(53, 12)
        Me.lblDenpyoNo.TabIndex = 0
        Me.lblDenpyoNo.Text = "伝票番号"
        '
        'txtDenpyoNo
        '
        Me.txtDenpyoNo.Location = New System.Drawing.Point(90, 22)
        Me.txtDenpyoNo.Name = "txtDenpyoNo"
        Me.txtDenpyoNo.ReadOnly = True
        Me.txtDenpyoNo.Size = New System.Drawing.Size(100, 19)
        Me.txtDenpyoNo.TabIndex = 1
        Me.txtDenpyoNo.TabStop = False
        '
        'lblNohinbi
        '
        Me.lblNohinbi.AutoSize = True
        Me.lblNohinbi.Location = New System.Drawing.Point(210, 25)
        Me.lblNohinbi.Name = "lblNohinbi"
        Me.lblNohinbi.Size = New System.Drawing.Size(41, 12)
        Me.lblNohinbi.TabIndex = 2
        Me.lblNohinbi.Text = "納品日"
        '
        'dtpNohinbi
        '
        Me.dtpNohinbi.Format = System.Windows.Forms.DateTimePickerFormat.Short
        Me.dtpNohinbi.Location = New System.Drawing.Point(260, 22)
        Me.dtpNohinbi.Name = "dtpNohinbi"
        Me.dtpNohinbi.Size = New System.Drawing.Size(120, 19)
        Me.dtpNohinbi.TabIndex = 3
        '
        'lblHinban
        '
        Me.lblHinban.AutoSize = True
        Me.lblHinban.Location = New System.Drawing.Point(400, 25)
        Me.lblHinban.Name = "lblHinban"
        Me.lblHinban.Size = New System.Drawing.Size(29, 12)
        Me.lblHinban.TabIndex = 4
        Me.lblHinban.Text = "品番"
        '
        'txtHinban
        '
        Me.txtHinban.Location = New System.Drawing.Point(440, 22)
        Me.txtHinban.MaxLength = 20
        Me.txtHinban.Name = "txtHinban"
        Me.txtHinban.ReadOnly = True
        Me.txtHinban.Size = New System.Drawing.Size(120, 19)
        Me.txtHinban.TabIndex = 5
        Me.txtHinban.TabStop = False
        '
        'lblHinmei
        '
        Me.lblHinmei.AutoSize = True
        Me.lblHinmei.Location = New System.Drawing.Point(580, 25)
        Me.lblHinmei.Name = "lblHinmei"
        Me.lblHinmei.Size = New System.Drawing.Size(29, 12)
        Me.lblHinmei.TabIndex = 6
        Me.lblHinmei.Text = "品名"
        '
        'txtHinmei
        '
        Me.txtHinmei.Location = New System.Drawing.Point(620, 22)
        Me.txtHinmei.MaxLength = 40
        Me.txtHinmei.Name = "txtHinmei"
        Me.txtHinmei.Size = New System.Drawing.Size(220, 19)
        Me.txtHinmei.TabIndex = 7
        '
        'lblSuryo
        '
        Me.lblSuryo.AutoSize = True
        Me.lblSuryo.Location = New System.Drawing.Point(16, 57)
        Me.lblSuryo.Name = "lblSuryo"
        Me.lblSuryo.Size = New System.Drawing.Size(29, 12)
        Me.lblSuryo.TabIndex = 8
        Me.lblSuryo.Text = "数量"
        '
        'txtSuryo
        '
        Me.txtSuryo.Location = New System.Drawing.Point(90, 54)
        Me.txtSuryo.MaxLength = 8
        Me.txtSuryo.Name = "txtSuryo"
        Me.txtSuryo.Size = New System.Drawing.Size(100, 19)
        Me.txtSuryo.TabIndex = 9
        Me.txtSuryo.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblTanka
        '
        Me.lblTanka.AutoSize = True
        Me.lblTanka.Location = New System.Drawing.Point(210, 57)
        Me.lblTanka.Name = "lblTanka"
        Me.lblTanka.Size = New System.Drawing.Size(29, 12)
        Me.lblTanka.TabIndex = 10
        Me.lblTanka.Text = "単価"
        '
        'txtTanka
        '
        Me.txtTanka.Location = New System.Drawing.Point(260, 54)
        Me.txtTanka.MaxLength = 11
        Me.txtTanka.Name = "txtTanka"
        Me.txtTanka.Size = New System.Drawing.Size(120, 19)
        Me.txtTanka.TabIndex = 11
        Me.txtTanka.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblKingaku
        '
        Me.lblKingaku.AutoSize = True
        Me.lblKingaku.Location = New System.Drawing.Point(400, 57)
        Me.lblKingaku.Name = "lblKingaku"
        Me.lblKingaku.Size = New System.Drawing.Size(29, 12)
        Me.lblKingaku.TabIndex = 12
        Me.lblKingaku.Text = "金額"
        '
        'txtKingaku
        '
        Me.txtKingaku.Location = New System.Drawing.Point(440, 54)
        Me.txtKingaku.Name = "txtKingaku"
        Me.txtKingaku.ReadOnly = True
        Me.txtKingaku.Size = New System.Drawing.Size(120, 19)
        Me.txtKingaku.TabIndex = 13
        Me.txtKingaku.TabStop = False
        Me.txtKingaku.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'chkMusho
        '
        Me.chkMusho.AutoSize = True
        Me.chkMusho.Location = New System.Drawing.Point(580, 56)
        Me.chkMusho.Name = "chkMusho"
        Me.chkMusho.Size = New System.Drawing.Size(73, 16)
        Me.chkMusho.TabIndex = 14
        Me.chkMusho.Text = "無償支給"
        Me.chkMusho.UseVisualStyleBackColor = True
        '
        'lblBiko
        '
        Me.lblBiko.AutoSize = True
        Me.lblBiko.Location = New System.Drawing.Point(16, 89)
        Me.lblBiko.Name = "lblBiko"
        Me.lblBiko.Size = New System.Drawing.Size(29, 12)
        Me.lblBiko.TabIndex = 15
        Me.lblBiko.Text = "備考"
        '
        'txtBiko
        '
        Me.txtBiko.Location = New System.Drawing.Point(90, 86)
        Me.txtBiko.MaxLength = 100
        Me.txtBiko.Name = "txtBiko"
        Me.txtBiko.Size = New System.Drawing.Size(470, 19)
        Me.txtBiko.TabIndex = 16
        '
        'lblChui
        '
        Me.lblChui.AutoSize = True
        Me.lblChui.ForeColor = System.Drawing.Color.Red
        Me.lblChui.Location = New System.Drawing.Point(580, 89)
        Me.lblChui.Name = "lblChui"
        Me.lblChui.Size = New System.Drawing.Size(0, 12)
        Me.lblChui.TabIndex = 17
        '
        'btnToroku
        '
        Me.btnToroku.Location = New System.Drawing.Point(716, 402)
        Me.btnToroku.Name = "btnToroku"
        Me.btnToroku.Size = New System.Drawing.Size(75, 23)
        Me.btnToroku.TabIndex = 8
        Me.btnToroku.Text = "登録(&S)"
        Me.btnToroku.UseVisualStyleBackColor = True
        '
        'btnClear
        '
        Me.btnClear.Location = New System.Drawing.Point(797, 402)
        Me.btnClear.Name = "btnClear"
        Me.btnClear.Size = New System.Drawing.Size(75, 23)
        Me.btnClear.TabIndex = 9
        Me.btnClear.Text = "クリア(&C)"
        Me.btnClear.UseVisualStyleBackColor = True
        '
        'lblNohinIchiran
        '
        Me.lblNohinIchiran.AutoSize = True
        Me.lblNohinIchiran.Location = New System.Drawing.Point(12, 436)
        Me.lblNohinIchiran.Name = "lblNohinIchiran"
        Me.lblNohinIchiran.Size = New System.Drawing.Size(101, 12)
        Me.lblNohinIchiran.TabIndex = 10
        Me.lblNohinIchiran.Text = "この受注の納品"
        '
        'dgvNohin
        '
        Me.dgvNohin.AllowUserToAddRows = False
        Me.dgvNohin.AllowUserToDeleteRows = False
        Me.dgvNohin.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvNohin.Location = New System.Drawing.Point(12, 452)
        Me.dgvNohin.MultiSelect = False
        Me.dgvNohin.Name = "dgvNohin"
        Me.dgvNohin.ReadOnly = True
        Me.dgvNohin.RowHeadersVisible = False
        Me.dgvNohin.RowTemplate.Height = 21
        Me.dgvNohin.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvNohin.Size = New System.Drawing.Size(860, 110)
        Me.dgvNohin.TabIndex = 11
        '
        'btnTorikeshi
        '
        Me.btnTorikeshi.Location = New System.Drawing.Point(716, 570)
        Me.btnTorikeshi.Name = "btnTorikeshi"
        Me.btnTorikeshi.Size = New System.Drawing.Size(75, 23)
        Me.btnTorikeshi.TabIndex = 12
        Me.btnTorikeshi.Text = "取消(&D)"
        Me.btnTorikeshi.UseVisualStyleBackColor = True
        '
        'btnClose
        '
        Me.btnClose.Location = New System.Drawing.Point(797, 570)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(75, 23)
        Me.btnClose.TabIndex = 13
        Me.btnClose.Text = "閉じる(&X)"
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'frmNohin
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(884, 606)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.btnTorikeshi)
        Me.Controls.Add(Me.dgvNohin)
        Me.Controls.Add(Me.lblNohinIchiran)
        Me.Controls.Add(Me.btnClear)
        Me.Controls.Add(Me.btnToroku)
        Me.Controls.Add(Me.grpNyuryoku)
        Me.Controls.Add(Me.lblKensu)
        Me.Controls.Add(Me.dgvJuchu)
        Me.Controls.Add(Me.lblMarume)
        Me.Controls.Add(Me.btnKensaku)
        Me.Controls.Add(Me.chkZanAri)
        Me.Controls.Add(Me.cboTokuisaki)
        Me.Controls.Add(Me.lblTokuisaki)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.Name = "frmNohin"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "納品入力"
        CType(Me.dgvJuchu, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpNyuryoku.ResumeLayout(False)
        Me.grpNyuryoku.PerformLayout()
        CType(Me.dgvNohin, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblTokuisaki As System.Windows.Forms.Label
    Friend WithEvents cboTokuisaki As System.Windows.Forms.ComboBox
    Friend WithEvents chkZanAri As System.Windows.Forms.CheckBox
    Friend WithEvents btnKensaku As System.Windows.Forms.Button
    Friend WithEvents lblMarume As System.Windows.Forms.Label
    Friend WithEvents dgvJuchu As System.Windows.Forms.DataGridView
    Friend WithEvents lblKensu As System.Windows.Forms.Label
    Friend WithEvents grpNyuryoku As System.Windows.Forms.GroupBox
    Friend WithEvents lblDenpyoNo As System.Windows.Forms.Label
    Friend WithEvents txtDenpyoNo As System.Windows.Forms.TextBox
    Friend WithEvents lblNohinbi As System.Windows.Forms.Label
    Friend WithEvents dtpNohinbi As System.Windows.Forms.DateTimePicker
    Friend WithEvents lblHinban As System.Windows.Forms.Label
    Friend WithEvents txtHinban As System.Windows.Forms.TextBox
    Friend WithEvents lblHinmei As System.Windows.Forms.Label
    Friend WithEvents txtHinmei As System.Windows.Forms.TextBox
    Friend WithEvents lblSuryo As System.Windows.Forms.Label
    Friend WithEvents txtSuryo As System.Windows.Forms.TextBox
    Friend WithEvents lblTanka As System.Windows.Forms.Label
    Friend WithEvents txtTanka As System.Windows.Forms.TextBox
    Friend WithEvents lblKingaku As System.Windows.Forms.Label
    Friend WithEvents txtKingaku As System.Windows.Forms.TextBox
    Friend WithEvents chkMusho As System.Windows.Forms.CheckBox
    Friend WithEvents lblBiko As System.Windows.Forms.Label
    Friend WithEvents txtBiko As System.Windows.Forms.TextBox
    Friend WithEvents lblChui As System.Windows.Forms.Label
    Friend WithEvents btnToroku As System.Windows.Forms.Button
    Friend WithEvents btnClear As System.Windows.Forms.Button
    Friend WithEvents lblNohinIchiran As System.Windows.Forms.Label
    Friend WithEvents dgvNohin As System.Windows.Forms.DataGridView
    Friend WithEvents btnTorikeshi As System.Windows.Forms.Button
    Friend WithEvents btnClose As System.Windows.Forms.Button
End Class
