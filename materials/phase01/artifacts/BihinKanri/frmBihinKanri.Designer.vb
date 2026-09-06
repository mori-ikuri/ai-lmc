<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmBihinKanri
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
        Me.lblKensakuBasho = New System.Windows.Forms.Label
        Me.cboKensakuBasho = New System.Windows.Forms.ComboBox
        Me.btnKensaku = New System.Windows.Forms.Button
        Me.dgvIchiran = New System.Windows.Forms.DataGridView
        Me.lblKensu = New System.Windows.Forms.Label
        Me.grpNyuryoku = New System.Windows.Forms.GroupBox
        Me.lblBihinCd = New System.Windows.Forms.Label
        Me.txtBihinCd = New System.Windows.Forms.TextBox
        Me.lblBihinNm = New System.Windows.Forms.Label
        Me.txtBihinNm = New System.Windows.Forms.TextBox
        Me.lblKikaku = New System.Windows.Forms.Label
        Me.txtKikaku = New System.Windows.Forms.TextBox
        Me.lblSuryo = New System.Windows.Forms.Label
        Me.txtSuryo = New System.Windows.Forms.TextBox
        Me.lblBasho = New System.Windows.Forms.Label
        Me.cboBasho = New System.Windows.Forms.ComboBox
        Me.lblShutokubi = New System.Windows.Forms.Label
        Me.dtpShutokubi = New System.Windows.Forms.DateTimePicker
        Me.lblKingaku = New System.Windows.Forms.Label
        Me.txtKingaku = New System.Windows.Forms.TextBox
        Me.lblBiko = New System.Windows.Forms.Label
        Me.txtBiko = New System.Windows.Forms.TextBox
        Me.btnToroku = New System.Windows.Forms.Button
        Me.btnSakujo = New System.Windows.Forms.Button
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
        Me.lblKensakuNm.Size = New System.Drawing.Size(29, 12)
        Me.lblKensakuNm.TabIndex = 0
        Me.lblKensakuNm.Text = "品名"
        '
        'txtKensakuNm
        '
        Me.txtKensakuNm.Location = New System.Drawing.Point(56, 12)
        Me.txtKensakuNm.MaxLength = 40
        Me.txtKensakuNm.Name = "txtKensakuNm"
        Me.txtKensakuNm.Size = New System.Drawing.Size(160, 19)
        Me.txtKensakuNm.TabIndex = 1
        '
        'lblKensakuBasho
        '
        Me.lblKensakuBasho.AutoSize = True
        Me.lblKensakuBasho.Location = New System.Drawing.Point(232, 15)
        Me.lblKensakuBasho.Name = "lblKensakuBasho"
        Me.lblKensakuBasho.Size = New System.Drawing.Size(53, 12)
        Me.lblKensakuBasho.TabIndex = 2
        Me.lblKensakuBasho.Text = "保管場所"
        '
        'cboKensakuBasho
        '
        Me.cboKensakuBasho.FormattingEnabled = True
        Me.cboKensakuBasho.Items.AddRange(New Object() {"(すべて)", "本社1F", "本社2F", "本社3F", "倉庫", "工場"})
        Me.cboKensakuBasho.Location = New System.Drawing.Point(296, 12)
        Me.cboKensakuBasho.Name = "cboKensakuBasho"
        Me.cboKensakuBasho.Size = New System.Drawing.Size(120, 20)
        Me.cboKensakuBasho.TabIndex = 3
        Me.cboKensakuBasho.Text = "(すべて)"
        '
        'btnKensaku
        '
        Me.btnKensaku.Location = New System.Drawing.Point(432, 10)
        Me.btnKensaku.Name = "btnKensaku"
        Me.btnKensaku.Size = New System.Drawing.Size(75, 23)
        Me.btnKensaku.TabIndex = 4
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
        Me.dgvIchiran.TabIndex = 5
        '
        'lblKensu
        '
        Me.lblKensu.AutoSize = True
        Me.lblKensu.Location = New System.Drawing.Point(12, 290)
        Me.lblKensu.Name = "lblKensu"
        Me.lblKensu.Size = New System.Drawing.Size(41, 12)
        Me.lblKensu.TabIndex = 6
        Me.lblKensu.Text = "件数 0 件"
        '
        'grpNyuryoku
        '
        Me.grpNyuryoku.Controls.Add(Me.lblBihinCd)
        Me.grpNyuryoku.Controls.Add(Me.txtBihinCd)
        Me.grpNyuryoku.Controls.Add(Me.lblBihinNm)
        Me.grpNyuryoku.Controls.Add(Me.txtBihinNm)
        Me.grpNyuryoku.Controls.Add(Me.lblKikaku)
        Me.grpNyuryoku.Controls.Add(Me.txtKikaku)
        Me.grpNyuryoku.Controls.Add(Me.lblSuryo)
        Me.grpNyuryoku.Controls.Add(Me.txtSuryo)
        Me.grpNyuryoku.Controls.Add(Me.lblBasho)
        Me.grpNyuryoku.Controls.Add(Me.cboBasho)
        Me.grpNyuryoku.Controls.Add(Me.lblShutokubi)
        Me.grpNyuryoku.Controls.Add(Me.dtpShutokubi)
        Me.grpNyuryoku.Controls.Add(Me.lblKingaku)
        Me.grpNyuryoku.Controls.Add(Me.txtKingaku)
        Me.grpNyuryoku.Controls.Add(Me.lblBiko)
        Me.grpNyuryoku.Controls.Add(Me.txtBiko)
        Me.grpNyuryoku.Location = New System.Drawing.Point(12, 310)
        Me.grpNyuryoku.Name = "grpNyuryoku"
        Me.grpNyuryoku.Size = New System.Drawing.Size(760, 130)
        Me.grpNyuryoku.TabIndex = 7
        Me.grpNyuryoku.TabStop = False
        Me.grpNyuryoku.Text = "入力"
        '
        'lblBihinCd
        '
        Me.lblBihinCd.AutoSize = True
        Me.lblBihinCd.Location = New System.Drawing.Point(16, 25)
        Me.lblBihinCd.Name = "lblBihinCd"
        Me.lblBihinCd.Size = New System.Drawing.Size(53, 12)
        Me.lblBihinCd.TabIndex = 0
        Me.lblBihinCd.Text = "備品コード"
        '
        'txtBihinCd
        '
        Me.txtBihinCd.Location = New System.Drawing.Point(88, 22)
        Me.txtBihinCd.Name = "txtBihinCd"
        Me.txtBihinCd.ReadOnly = True
        Me.txtBihinCd.Size = New System.Drawing.Size(90, 19)
        Me.txtBihinCd.TabIndex = 1
        Me.txtBihinCd.TabStop = False
        '
        'lblBihinNm
        '
        Me.lblBihinNm.AutoSize = True
        Me.lblBihinNm.Location = New System.Drawing.Point(200, 25)
        Me.lblBihinNm.Name = "lblBihinNm"
        Me.lblBihinNm.Size = New System.Drawing.Size(29, 12)
        Me.lblBihinNm.TabIndex = 2
        Me.lblBihinNm.Text = "品名"
        '
        'txtBihinNm
        '
        Me.txtBihinNm.Location = New System.Drawing.Point(248, 22)
        Me.txtBihinNm.MaxLength = 40
        Me.txtBihinNm.Name = "txtBihinNm"
        Me.txtBihinNm.Size = New System.Drawing.Size(240, 19)
        Me.txtBihinNm.TabIndex = 3
        '
        'lblKikaku
        '
        Me.lblKikaku.AutoSize = True
        Me.lblKikaku.Location = New System.Drawing.Point(510, 25)
        Me.lblKikaku.Name = "lblKikaku"
        Me.lblKikaku.Size = New System.Drawing.Size(29, 12)
        Me.lblKikaku.TabIndex = 4
        Me.lblKikaku.Text = "規格"
        '
        'txtKikaku
        '
        Me.txtKikaku.Location = New System.Drawing.Point(552, 22)
        Me.txtKikaku.MaxLength = 40
        Me.txtKikaku.Name = "txtKikaku"
        Me.txtKikaku.Size = New System.Drawing.Size(190, 19)
        Me.txtKikaku.TabIndex = 5
        '
        'lblSuryo
        '
        Me.lblSuryo.AutoSize = True
        Me.lblSuryo.Location = New System.Drawing.Point(16, 57)
        Me.lblSuryo.Name = "lblSuryo"
        Me.lblSuryo.Size = New System.Drawing.Size(29, 12)
        Me.lblSuryo.TabIndex = 6
        Me.lblSuryo.Text = "数量"
        '
        'txtSuryo
        '
        Me.txtSuryo.Location = New System.Drawing.Point(88, 54)
        Me.txtSuryo.MaxLength = 8
        Me.txtSuryo.Name = "txtSuryo"
        Me.txtSuryo.Size = New System.Drawing.Size(90, 19)
        Me.txtSuryo.TabIndex = 7
        Me.txtSuryo.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblBasho
        '
        Me.lblBasho.AutoSize = True
        Me.lblBasho.Location = New System.Drawing.Point(200, 57)
        Me.lblBasho.Name = "lblBasho"
        Me.lblBasho.Size = New System.Drawing.Size(53, 12)
        Me.lblBasho.TabIndex = 8
        Me.lblBasho.Text = "保管場所"
        '
        'cboBasho
        '
        Me.cboBasho.FormattingEnabled = True
        Me.cboBasho.Location = New System.Drawing.Point(248, 54)
        Me.cboBasho.Name = "cboBasho"
        Me.cboBasho.Size = New System.Drawing.Size(120, 20)
        Me.cboBasho.TabIndex = 9
        '
        'lblShutokubi
        '
        Me.lblShutokubi.AutoSize = True
        Me.lblShutokubi.Location = New System.Drawing.Point(390, 57)
        Me.lblShutokubi.Name = "lblShutokubi"
        Me.lblShutokubi.Size = New System.Drawing.Size(41, 12)
        Me.lblShutokubi.TabIndex = 10
        Me.lblShutokubi.Text = "取得日"
        '
        'dtpShutokubi
        '
        Me.dtpShutokubi.Format = System.Windows.Forms.DateTimePickerFormat.Short
        Me.dtpShutokubi.Location = New System.Drawing.Point(440, 54)
        Me.dtpShutokubi.Name = "dtpShutokubi"
        Me.dtpShutokubi.Size = New System.Drawing.Size(120, 19)
        Me.dtpShutokubi.TabIndex = 11
        '
        'lblKingaku
        '
        Me.lblKingaku.AutoSize = True
        Me.lblKingaku.Location = New System.Drawing.Point(580, 57)
        Me.lblKingaku.Name = "lblKingaku"
        Me.lblKingaku.Size = New System.Drawing.Size(29, 12)
        Me.lblKingaku.TabIndex = 12
        Me.lblKingaku.Text = "金額"
        '
        'txtKingaku
        '
        Me.txtKingaku.Location = New System.Drawing.Point(624, 54)
        Me.txtKingaku.MaxLength = 10
        Me.txtKingaku.Name = "txtKingaku"
        Me.txtKingaku.Size = New System.Drawing.Size(118, 19)
        Me.txtKingaku.TabIndex = 13
        Me.txtKingaku.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblBiko
        '
        Me.lblBiko.AutoSize = True
        Me.lblBiko.Location = New System.Drawing.Point(16, 92)
        Me.lblBiko.Name = "lblBiko"
        Me.lblBiko.Size = New System.Drawing.Size(29, 12)
        Me.lblBiko.TabIndex = 14
        Me.lblBiko.Text = "備考"
        '
        'txtBiko
        '
        Me.txtBiko.Location = New System.Drawing.Point(88, 89)
        Me.txtBiko.MaxLength = 100
        Me.txtBiko.Name = "txtBiko"
        Me.txtBiko.Size = New System.Drawing.Size(654, 19)
        Me.txtBiko.TabIndex = 15
        '
        'btnToroku
        '
        Me.btnToroku.Location = New System.Drawing.Point(452, 450)
        Me.btnToroku.Name = "btnToroku"
        Me.btnToroku.Size = New System.Drawing.Size(75, 23)
        Me.btnToroku.TabIndex = 8
        Me.btnToroku.Text = "登録(&S)"
        Me.btnToroku.UseVisualStyleBackColor = True
        '
        'btnSakujo
        '
        Me.btnSakujo.Location = New System.Drawing.Point(533, 450)
        Me.btnSakujo.Name = "btnSakujo"
        Me.btnSakujo.Size = New System.Drawing.Size(75, 23)
        Me.btnSakujo.TabIndex = 9
        Me.btnSakujo.Text = "廃棄(&D)"
        Me.btnSakujo.UseVisualStyleBackColor = True
        '
        'btnClear
        '
        Me.btnClear.Location = New System.Drawing.Point(614, 450)
        Me.btnClear.Name = "btnClear"
        Me.btnClear.Size = New System.Drawing.Size(75, 23)
        Me.btnClear.TabIndex = 10
        Me.btnClear.Text = "クリア(&C)"
        Me.btnClear.UseVisualStyleBackColor = True
        '
        'btnClose
        '
        Me.btnClose.Location = New System.Drawing.Point(697, 450)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(75, 23)
        Me.btnClose.TabIndex = 11
        Me.btnClose.Text = "閉じる(&X)"
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'frmBihinKanri
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(784, 486)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.btnClear)
        Me.Controls.Add(Me.btnSakujo)
        Me.Controls.Add(Me.btnToroku)
        Me.Controls.Add(Me.grpNyuryoku)
        Me.Controls.Add(Me.lblKensu)
        Me.Controls.Add(Me.dgvIchiran)
        Me.Controls.Add(Me.btnKensaku)
        Me.Controls.Add(Me.cboKensakuBasho)
        Me.Controls.Add(Me.lblKensakuBasho)
        Me.Controls.Add(Me.txtKensakuNm)
        Me.Controls.Add(Me.lblKensakuNm)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.Name = "frmBihinKanri"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "備品管理"
        CType(Me.dgvIchiran, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpNyuryoku.ResumeLayout(False)
        Me.grpNyuryoku.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblKensakuNm As System.Windows.Forms.Label
    Friend WithEvents txtKensakuNm As System.Windows.Forms.TextBox
    Friend WithEvents lblKensakuBasho As System.Windows.Forms.Label
    Friend WithEvents cboKensakuBasho As System.Windows.Forms.ComboBox
    Friend WithEvents btnKensaku As System.Windows.Forms.Button
    Friend WithEvents dgvIchiran As System.Windows.Forms.DataGridView
    Friend WithEvents lblKensu As System.Windows.Forms.Label
    Friend WithEvents grpNyuryoku As System.Windows.Forms.GroupBox
    Friend WithEvents lblBihinCd As System.Windows.Forms.Label
    Friend WithEvents txtBihinCd As System.Windows.Forms.TextBox
    Friend WithEvents lblBihinNm As System.Windows.Forms.Label
    Friend WithEvents txtBihinNm As System.Windows.Forms.TextBox
    Friend WithEvents lblKikaku As System.Windows.Forms.Label
    Friend WithEvents txtKikaku As System.Windows.Forms.TextBox
    Friend WithEvents lblSuryo As System.Windows.Forms.Label
    Friend WithEvents txtSuryo As System.Windows.Forms.TextBox
    Friend WithEvents lblBasho As System.Windows.Forms.Label
    Friend WithEvents cboBasho As System.Windows.Forms.ComboBox
    Friend WithEvents lblShutokubi As System.Windows.Forms.Label
    Friend WithEvents dtpShutokubi As System.Windows.Forms.DateTimePicker
    Friend WithEvents lblKingaku As System.Windows.Forms.Label
    Friend WithEvents txtKingaku As System.Windows.Forms.TextBox
    Friend WithEvents lblBiko As System.Windows.Forms.Label
    Friend WithEvents txtBiko As System.Windows.Forms.TextBox
    Friend WithEvents btnToroku As System.Windows.Forms.Button
    Friend WithEvents btnSakujo As System.Windows.Forms.Button
    Friend WithEvents btnClear As System.Windows.Forms.Button
    Friend WithEvents btnClose As System.Windows.Forms.Button
End Class
