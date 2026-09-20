<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmTokuisaki
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
        Me.lblKensakuNm = New System.Windows.Forms.Label
        Me.txtKensakuNm = New System.Windows.Forms.TextBox
        Me.chkShiyoNashi = New System.Windows.Forms.CheckBox
        Me.btnKensaku = New System.Windows.Forms.Button
        Me.dgvIchiran = New System.Windows.Forms.DataGridView
        Me.lblKensu = New System.Windows.Forms.Label
        Me.grpNyuryoku = New System.Windows.Forms.GroupBox
        Me.lblTokuisakiCd = New System.Windows.Forms.Label
        Me.txtTokuisakiCd = New System.Windows.Forms.TextBox
        Me.lblTokuisakiNm = New System.Windows.Forms.Label
        Me.txtTokuisakiNm = New System.Windows.Forms.TextBox
        Me.lblSeikyusakiCd = New System.Windows.Forms.Label
        Me.txtSeikyusakiCd = New System.Windows.Forms.TextBox
        Me.lblShimebi = New System.Windows.Forms.Label
        Me.cboShimebi = New System.Windows.Forms.ComboBox
        Me.lblShiharai = New System.Windows.Forms.Label
        Me.txtShiharai = New System.Windows.Forms.TextBox
        Me.lblMarume = New System.Windows.Forms.Label
        Me.cboMarume = New System.Windows.Forms.ComboBox
        Me.lblBiko = New System.Windows.Forms.Label
        Me.txtBiko = New System.Windows.Forms.TextBox
        Me.lblKaishiZan = New System.Windows.Forms.Label
        Me.txtKaishiZan = New System.Windows.Forms.TextBox
        Me.btnToroku = New System.Windows.Forms.Button
        Me.btnShiyoNashi = New System.Windows.Forms.Button
        Me.btnClear = New System.Windows.Forms.Button
        Me.btnClose = New System.Windows.Forms.Button
        CType(Me.dgvIchiran, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpNyuryoku.SuspendLayout()
        Me.SuspendLayout()
        '
        'lblKensakuNm
        '
        Me.lblKensakuNm.AutoSize = True
        Me.lblKensakuNm.Location = New System.Drawing.Point(12, 15)
        Me.lblKensakuNm.Name = "lblKensakuNm"
        Me.lblKensakuNm.Size = New System.Drawing.Size(53, 12)
        Me.lblKensakuNm.TabIndex = 0
        Me.lblKensakuNm.Text = "得意先名"
        '
        'txtKensakuNm
        '
        Me.txtKensakuNm.Location = New System.Drawing.Point(72, 12)
        Me.txtKensakuNm.MaxLength = 40
        Me.txtKensakuNm.Name = "txtKensakuNm"
        Me.txtKensakuNm.Size = New System.Drawing.Size(180, 19)
        Me.txtKensakuNm.TabIndex = 1
        '
        'chkShiyoNashi
        '
        Me.chkShiyoNashi.AutoSize = True
        Me.chkShiyoNashi.Location = New System.Drawing.Point(268, 14)
        Me.chkShiyoNashi.Name = "chkShiyoNashi"
        Me.chkShiyoNashi.Size = New System.Drawing.Size(133, 16)
        Me.chkShiyoNashi.TabIndex = 2
        Me.chkShiyoNashi.Text = "使用しないものも表示"
        Me.chkShiyoNashi.UseVisualStyleBackColor = True
        '
        'btnKensaku
        '
        Me.btnKensaku.Location = New System.Drawing.Point(432, 10)
        Me.btnKensaku.Name = "btnKensaku"
        Me.btnKensaku.Size = New System.Drawing.Size(75, 23)
        Me.btnKensaku.TabIndex = 3
        Me.btnKensaku.Text = "検索(&F)"
        Me.btnKensaku.UseVisualStyleBackColor = True
        '
        'dgvIchiran
        '
        Me.dgvIchiran.AllowUserToAddRows = False
        Me.dgvIchiran.AllowUserToDeleteRows = False
        Me.dgvIchiran.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvIchiran.Location = New System.Drawing.Point(12, 42)
        Me.dgvIchiran.MultiSelect = False
        Me.dgvIchiran.Name = "dgvIchiran"
        Me.dgvIchiran.ReadOnly = True
        Me.dgvIchiran.RowHeadersVisible = False
        Me.dgvIchiran.RowTemplate.Height = 21
        Me.dgvIchiran.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvIchiran.Size = New System.Drawing.Size(760, 240)
        Me.dgvIchiran.TabIndex = 4
        '
        'lblKensu
        '
        Me.lblKensu.AutoSize = True
        Me.lblKensu.Location = New System.Drawing.Point(12, 290)
        Me.lblKensu.Name = "lblKensu"
        Me.lblKensu.Size = New System.Drawing.Size(41, 12)
        Me.lblKensu.TabIndex = 5
        Me.lblKensu.Text = "件数 0 件"
        '
        'grpNyuryoku
        '
        Me.grpNyuryoku.Controls.Add(Me.lblTokuisakiCd)
        Me.grpNyuryoku.Controls.Add(Me.txtTokuisakiCd)
        Me.grpNyuryoku.Controls.Add(Me.lblTokuisakiNm)
        Me.grpNyuryoku.Controls.Add(Me.txtTokuisakiNm)
        Me.grpNyuryoku.Controls.Add(Me.lblSeikyusakiCd)
        Me.grpNyuryoku.Controls.Add(Me.txtSeikyusakiCd)
        Me.grpNyuryoku.Controls.Add(Me.lblShimebi)
        Me.grpNyuryoku.Controls.Add(Me.cboShimebi)
        Me.grpNyuryoku.Controls.Add(Me.lblShiharai)
        Me.grpNyuryoku.Controls.Add(Me.txtShiharai)
        Me.grpNyuryoku.Controls.Add(Me.lblMarume)
        Me.grpNyuryoku.Controls.Add(Me.cboMarume)
        Me.grpNyuryoku.Controls.Add(Me.lblBiko)
        Me.grpNyuryoku.Controls.Add(Me.txtBiko)
        Me.grpNyuryoku.Controls.Add(Me.lblKaishiZan)
        Me.grpNyuryoku.Controls.Add(Me.txtKaishiZan)
        Me.grpNyuryoku.Location = New System.Drawing.Point(12, 310)
        Me.grpNyuryoku.Name = "grpNyuryoku"
        Me.grpNyuryoku.Size = New System.Drawing.Size(760, 130)
        Me.grpNyuryoku.TabIndex = 6
        Me.grpNyuryoku.TabStop = False
        Me.grpNyuryoku.Text = "入力"
        '
        'lblTokuisakiCd
        '
        Me.lblTokuisakiCd.AutoSize = True
        Me.lblTokuisakiCd.Location = New System.Drawing.Point(16, 25)
        Me.lblTokuisakiCd.Name = "lblTokuisakiCd"
        Me.lblTokuisakiCd.Size = New System.Drawing.Size(77, 12)
        Me.lblTokuisakiCd.TabIndex = 0
        Me.lblTokuisakiCd.Text = "得意先コード"
        '
        'txtTokuisakiCd
        '
        Me.txtTokuisakiCd.Location = New System.Drawing.Point(100, 22)
        Me.txtTokuisakiCd.MaxLength = 10
        Me.txtTokuisakiCd.Name = "txtTokuisakiCd"
        Me.txtTokuisakiCd.Size = New System.Drawing.Size(90, 19)
        Me.txtTokuisakiCd.TabIndex = 1
        '
        'lblTokuisakiNm
        '
        Me.lblTokuisakiNm.AutoSize = True
        Me.lblTokuisakiNm.Location = New System.Drawing.Point(210, 25)
        Me.lblTokuisakiNm.Name = "lblTokuisakiNm"
        Me.lblTokuisakiNm.Size = New System.Drawing.Size(53, 12)
        Me.lblTokuisakiNm.TabIndex = 2
        Me.lblTokuisakiNm.Text = "得意先名"
        '
        'txtTokuisakiNm
        '
        Me.txtTokuisakiNm.Location = New System.Drawing.Point(280, 22)
        Me.txtTokuisakiNm.MaxLength = 40
        Me.txtTokuisakiNm.Name = "txtTokuisakiNm"
        Me.txtTokuisakiNm.Size = New System.Drawing.Size(240, 19)
        Me.txtTokuisakiNm.TabIndex = 3
        '
        'lblSeikyusakiCd
        '
        Me.lblSeikyusakiCd.AutoSize = True
        Me.lblSeikyusakiCd.Location = New System.Drawing.Point(540, 25)
        Me.lblSeikyusakiCd.Name = "lblSeikyusakiCd"
        Me.lblSeikyusakiCd.Size = New System.Drawing.Size(65, 12)
        Me.lblSeikyusakiCd.TabIndex = 4
        Me.lblSeikyusakiCd.Text = "請求先コード"
        '
        'txtSeikyusakiCd
        '
        Me.txtSeikyusakiCd.Location = New System.Drawing.Point(620, 22)
        Me.txtSeikyusakiCd.MaxLength = 10
        Me.txtSeikyusakiCd.Name = "txtSeikyusakiCd"
        Me.txtSeikyusakiCd.Size = New System.Drawing.Size(90, 19)
        Me.txtSeikyusakiCd.TabIndex = 5
        '
        'lblShimebi
        '
        Me.lblShimebi.AutoSize = True
        Me.lblShimebi.Location = New System.Drawing.Point(16, 57)
        Me.lblShimebi.Name = "lblShimebi"
        Me.lblShimebi.Size = New System.Drawing.Size(29, 12)
        Me.lblShimebi.TabIndex = 6
        Me.lblShimebi.Text = "締日"
        '
        'cboShimebi
        '
        Me.cboShimebi.FormattingEnabled = True
        Me.cboShimebi.Location = New System.Drawing.Point(100, 54)
        Me.cboShimebi.Name = "cboShimebi"
        Me.cboShimebi.Size = New System.Drawing.Size(90, 20)
        Me.cboShimebi.TabIndex = 7
        '
        'lblShiharai
        '
        Me.lblShiharai.AutoSize = True
        Me.lblShiharai.Location = New System.Drawing.Point(210, 57)
        Me.lblShiharai.Name = "lblShiharai"
        Me.lblShiharai.Size = New System.Drawing.Size(53, 12)
        Me.lblShiharai.TabIndex = 8
        Me.lblShiharai.Text = "支払条件"
        '
        'txtShiharai
        '
        Me.txtShiharai.Location = New System.Drawing.Point(280, 54)
        Me.txtShiharai.MaxLength = 20
        Me.txtShiharai.Name = "txtShiharai"
        Me.txtShiharai.Size = New System.Drawing.Size(150, 19)
        Me.txtShiharai.TabIndex = 9
        '
        'lblMarume
        '
        Me.lblMarume.AutoSize = True
        Me.lblMarume.Location = New System.Drawing.Point(450, 57)
        Me.lblMarume.Name = "lblMarume"
        Me.lblMarume.Size = New System.Drawing.Size(29, 12)
        Me.lblMarume.TabIndex = 10
        Me.lblMarume.Text = "丸め"
        '
        'cboMarume
        '
        Me.cboMarume.FormattingEnabled = True
        Me.cboMarume.Location = New System.Drawing.Point(500, 54)
        Me.cboMarume.Name = "cboMarume"
        Me.cboMarume.Size = New System.Drawing.Size(100, 20)
        Me.cboMarume.TabIndex = 11
        '
        'lblBiko
        '
        Me.lblBiko.AutoSize = True
        Me.lblBiko.Location = New System.Drawing.Point(16, 92)
        Me.lblBiko.Name = "lblBiko"
        Me.lblBiko.Size = New System.Drawing.Size(29, 12)
        Me.lblBiko.TabIndex = 12
        Me.lblBiko.Text = "備考"
        '
        'txtBiko
        '
        Me.txtBiko.Location = New System.Drawing.Point(100, 89)
        Me.txtBiko.MaxLength = 100
        Me.txtBiko.Name = "txtBiko"
        Me.txtBiko.Size = New System.Drawing.Size(480, 19)
        Me.txtBiko.TabIndex = 13
        '
        'lblKaishiZan
        '
        Me.lblKaishiZan.AutoSize = True
        Me.lblKaishiZan.Location = New System.Drawing.Point(600, 92)
        Me.lblKaishiZan.Name = "lblKaishiZan"
        Me.lblKaishiZan.Size = New System.Drawing.Size(53, 12)
        Me.lblKaishiZan.TabIndex = 14
        Me.lblKaishiZan.Text = "開始残高"
        '
        'txtKaishiZan
        '
        Me.txtKaishiZan.Location = New System.Drawing.Point(660, 89)
        Me.txtKaishiZan.MaxLength = 13
        Me.txtKaishiZan.Name = "txtKaishiZan"
        Me.txtKaishiZan.Size = New System.Drawing.Size(90, 19)
        Me.txtKaishiZan.TabIndex = 15
        Me.txtKaishiZan.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'btnToroku
        '
        Me.btnToroku.Location = New System.Drawing.Point(452, 450)
        Me.btnToroku.Name = "btnToroku"
        Me.btnToroku.Size = New System.Drawing.Size(75, 23)
        Me.btnToroku.TabIndex = 7
        Me.btnToroku.Text = "登録(&S)"
        Me.btnToroku.UseVisualStyleBackColor = True
        '
        'btnShiyoNashi
        '
        Me.btnShiyoNashi.Location = New System.Drawing.Point(533, 450)
        Me.btnShiyoNashi.Name = "btnShiyoNashi"
        Me.btnShiyoNashi.Size = New System.Drawing.Size(75, 23)
        Me.btnShiyoNashi.TabIndex = 8
        Me.btnShiyoNashi.Text = "使用しない(&D)"
        Me.btnShiyoNashi.UseVisualStyleBackColor = True
        '
        'btnClear
        '
        Me.btnClear.Location = New System.Drawing.Point(614, 450)
        Me.btnClear.Name = "btnClear"
        Me.btnClear.Size = New System.Drawing.Size(75, 23)
        Me.btnClear.TabIndex = 9
        Me.btnClear.Text = "クリア(&C)"
        Me.btnClear.UseVisualStyleBackColor = True
        '
        'btnClose
        '
        Me.btnClose.Location = New System.Drawing.Point(697, 450)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(75, 23)
        Me.btnClose.TabIndex = 10
        Me.btnClose.Text = "閉じる(&X)"
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'frmTokuisaki
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(784, 486)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.btnClear)
        Me.Controls.Add(Me.btnShiyoNashi)
        Me.Controls.Add(Me.btnToroku)
        Me.Controls.Add(Me.grpNyuryoku)
        Me.Controls.Add(Me.lblKensu)
        Me.Controls.Add(Me.dgvIchiran)
        Me.Controls.Add(Me.btnKensaku)
        Me.Controls.Add(Me.chkShiyoNashi)
        Me.Controls.Add(Me.txtKensakuNm)
        Me.Controls.Add(Me.lblKensakuNm)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.Name = "frmTokuisaki"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "得意先登録"
        CType(Me.dgvIchiran, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpNyuryoku.ResumeLayout(False)
        Me.grpNyuryoku.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblKensakuNm As System.Windows.Forms.Label
    Friend WithEvents txtKensakuNm As System.Windows.Forms.TextBox
    Friend WithEvents chkShiyoNashi As System.Windows.Forms.CheckBox
    Friend WithEvents btnKensaku As System.Windows.Forms.Button
    Friend WithEvents dgvIchiran As System.Windows.Forms.DataGridView
    Friend WithEvents lblKensu As System.Windows.Forms.Label
    Friend WithEvents grpNyuryoku As System.Windows.Forms.GroupBox
    Friend WithEvents lblTokuisakiCd As System.Windows.Forms.Label
    Friend WithEvents txtTokuisakiCd As System.Windows.Forms.TextBox
    Friend WithEvents lblTokuisakiNm As System.Windows.Forms.Label
    Friend WithEvents txtTokuisakiNm As System.Windows.Forms.TextBox
    Friend WithEvents lblSeikyusakiCd As System.Windows.Forms.Label
    Friend WithEvents txtSeikyusakiCd As System.Windows.Forms.TextBox
    Friend WithEvents lblShimebi As System.Windows.Forms.Label
    Friend WithEvents cboShimebi As System.Windows.Forms.ComboBox
    Friend WithEvents lblShiharai As System.Windows.Forms.Label
    Friend WithEvents txtShiharai As System.Windows.Forms.TextBox
    Friend WithEvents lblMarume As System.Windows.Forms.Label
    Friend WithEvents cboMarume As System.Windows.Forms.ComboBox
    Friend WithEvents lblBiko As System.Windows.Forms.Label
    Friend WithEvents txtBiko As System.Windows.Forms.TextBox
    Friend WithEvents lblKaishiZan As System.Windows.Forms.Label
    Friend WithEvents txtKaishiZan As System.Windows.Forms.TextBox
    Friend WithEvents btnToroku As System.Windows.Forms.Button
    Friend WithEvents btnShiyoNashi As System.Windows.Forms.Button
    Friend WithEvents btnClear As System.Windows.Forms.Button
    Friend WithEvents btnClose As System.Windows.Forms.Button
End Class
