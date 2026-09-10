<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmTanka
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
        Me.lblKensakuHinban = New System.Windows.Forms.Label
        Me.txtKensakuHinban = New System.Windows.Forms.TextBox
        Me.btnKensaku = New System.Windows.Forms.Button
        Me.dgvIchiran = New System.Windows.Forms.DataGridView
        Me.lblKensu = New System.Windows.Forms.Label
        Me.grpNyuryoku = New System.Windows.Forms.GroupBox
        Me.lblHinban = New System.Windows.Forms.Label
        Me.txtHinban = New System.Windows.Forms.TextBox
        Me.lblHinmei = New System.Windows.Forms.Label
        Me.txtHinmei = New System.Windows.Forms.TextBox
        Me.lblTanka = New System.Windows.Forms.Label
        Me.txtTanka = New System.Windows.Forms.TextBox
        Me.lblTankaChui = New System.Windows.Forms.Label
        Me.btnToroku = New System.Windows.Forms.Button
        Me.btnShiyoNashi = New System.Windows.Forms.Button
        Me.btnClear = New System.Windows.Forms.Button
        Me.btnClose = New System.Windows.Forms.Button
        CType(Me.dgvIchiran, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpNyuryoku.SuspendLayout()
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
        'lblKensakuHinban
        '
        Me.lblKensakuHinban.AutoSize = True
        Me.lblKensakuHinban.Location = New System.Drawing.Point(340, 15)
        Me.lblKensakuHinban.Name = "lblKensakuHinban"
        Me.lblKensakuHinban.Size = New System.Drawing.Size(29, 12)
        Me.lblKensakuHinban.TabIndex = 2
        Me.lblKensakuHinban.Text = "品番"
        '
        'txtKensakuHinban
        '
        Me.txtKensakuHinban.Location = New System.Drawing.Point(380, 12)
        Me.txtKensakuHinban.MaxLength = 20
        Me.txtKensakuHinban.Name = "txtKensakuHinban"
        Me.txtKensakuHinban.Size = New System.Drawing.Size(140, 19)
        Me.txtKensakuHinban.TabIndex = 3
        '
        'btnKensaku
        '
        Me.btnKensaku.Location = New System.Drawing.Point(540, 10)
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
        Me.grpNyuryoku.Controls.Add(Me.lblHinban)
        Me.grpNyuryoku.Controls.Add(Me.txtHinban)
        Me.grpNyuryoku.Controls.Add(Me.lblHinmei)
        Me.grpNyuryoku.Controls.Add(Me.txtHinmei)
        Me.grpNyuryoku.Controls.Add(Me.lblTanka)
        Me.grpNyuryoku.Controls.Add(Me.txtTanka)
        Me.grpNyuryoku.Controls.Add(Me.lblTankaChui)
        Me.grpNyuryoku.Location = New System.Drawing.Point(12, 310)
        Me.grpNyuryoku.Name = "grpNyuryoku"
        Me.grpNyuryoku.Size = New System.Drawing.Size(760, 96)
        Me.grpNyuryoku.TabIndex = 7
        Me.grpNyuryoku.TabStop = False
        Me.grpNyuryoku.Text = "入力"
        '
        'lblHinban
        '
        Me.lblHinban.AutoSize = True
        Me.lblHinban.Location = New System.Drawing.Point(16, 25)
        Me.lblHinban.Name = "lblHinban"
        Me.lblHinban.Size = New System.Drawing.Size(29, 12)
        Me.lblHinban.TabIndex = 0
        Me.lblHinban.Text = "品番"
        '
        'txtHinban
        '
        Me.txtHinban.Location = New System.Drawing.Point(100, 22)
        Me.txtHinban.MaxLength = 20
        Me.txtHinban.Name = "txtHinban"
        Me.txtHinban.Size = New System.Drawing.Size(140, 19)
        Me.txtHinban.TabIndex = 1
        '
        'lblHinmei
        '
        Me.lblHinmei.AutoSize = True
        Me.lblHinmei.Location = New System.Drawing.Point(260, 25)
        Me.lblHinmei.Name = "lblHinmei"
        Me.lblHinmei.Size = New System.Drawing.Size(29, 12)
        Me.lblHinmei.TabIndex = 2
        Me.lblHinmei.Text = "品名"
        '
        'txtHinmei
        '
        Me.txtHinmei.Location = New System.Drawing.Point(310, 22)
        Me.txtHinmei.MaxLength = 40
        Me.txtHinmei.Name = "txtHinmei"
        Me.txtHinmei.Size = New System.Drawing.Size(240, 19)
        Me.txtHinmei.TabIndex = 3
        '
        'lblTanka
        '
        Me.lblTanka.AutoSize = True
        Me.lblTanka.Location = New System.Drawing.Point(16, 57)
        Me.lblTanka.Name = "lblTanka"
        Me.lblTanka.Size = New System.Drawing.Size(29, 12)
        Me.lblTanka.TabIndex = 4
        Me.lblTanka.Text = "単価"
        '
        'txtTanka
        '
        Me.txtTanka.Location = New System.Drawing.Point(100, 54)
        Me.txtTanka.MaxLength = 11
        Me.txtTanka.Name = "txtTanka"
        Me.txtTanka.Size = New System.Drawing.Size(140, 19)
        Me.txtTanka.TabIndex = 5
        Me.txtTanka.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblTankaChui
        '
        Me.lblTankaChui.AutoSize = True
        Me.lblTankaChui.Location = New System.Drawing.Point(260, 57)
        Me.lblTankaChui.Name = "lblTankaChui"
        Me.lblTankaChui.Size = New System.Drawing.Size(281, 12)
        Me.lblTankaChui.TabIndex = 6
        Me.lblTankaChui.Text = "※見積の単価を入れる。小数(212.5など)も入れられる"
        '
        'btnToroku
        '
        Me.btnToroku.Location = New System.Drawing.Point(452, 418)
        Me.btnToroku.Name = "btnToroku"
        Me.btnToroku.Size = New System.Drawing.Size(75, 23)
        Me.btnToroku.TabIndex = 8
        Me.btnToroku.Text = "登録(&S)"
        Me.btnToroku.UseVisualStyleBackColor = True
        '
        'btnShiyoNashi
        '
        Me.btnShiyoNashi.Location = New System.Drawing.Point(533, 418)
        Me.btnShiyoNashi.Name = "btnShiyoNashi"
        Me.btnShiyoNashi.Size = New System.Drawing.Size(75, 23)
        Me.btnShiyoNashi.TabIndex = 9
        Me.btnShiyoNashi.Text = "使用しない(&D)"
        Me.btnShiyoNashi.UseVisualStyleBackColor = True
        '
        'btnClear
        '
        Me.btnClear.Location = New System.Drawing.Point(614, 418)
        Me.btnClear.Name = "btnClear"
        Me.btnClear.Size = New System.Drawing.Size(75, 23)
        Me.btnClear.TabIndex = 10
        Me.btnClear.Text = "クリア(&C)"
        Me.btnClear.UseVisualStyleBackColor = True
        '
        'btnClose
        '
        Me.btnClose.Location = New System.Drawing.Point(697, 418)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(75, 23)
        Me.btnClose.TabIndex = 11
        Me.btnClose.Text = "閉じる(&X)"
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'frmTanka
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(784, 454)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.btnClear)
        Me.Controls.Add(Me.btnShiyoNashi)
        Me.Controls.Add(Me.btnToroku)
        Me.Controls.Add(Me.grpNyuryoku)
        Me.Controls.Add(Me.lblKensu)
        Me.Controls.Add(Me.dgvIchiran)
        Me.Controls.Add(Me.btnKensaku)
        Me.Controls.Add(Me.txtKensakuHinban)
        Me.Controls.Add(Me.lblKensakuHinban)
        Me.Controls.Add(Me.cboTokuisaki)
        Me.Controls.Add(Me.lblTokuisaki)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.Name = "frmTanka"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "単価登録"
        CType(Me.dgvIchiran, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpNyuryoku.ResumeLayout(False)
        Me.grpNyuryoku.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblTokuisaki As System.Windows.Forms.Label
    Friend WithEvents cboTokuisaki As System.Windows.Forms.ComboBox
    Friend WithEvents lblKensakuHinban As System.Windows.Forms.Label
    Friend WithEvents txtKensakuHinban As System.Windows.Forms.TextBox
    Friend WithEvents btnKensaku As System.Windows.Forms.Button
    Friend WithEvents dgvIchiran As System.Windows.Forms.DataGridView
    Friend WithEvents lblKensu As System.Windows.Forms.Label
    Friend WithEvents grpNyuryoku As System.Windows.Forms.GroupBox
    Friend WithEvents lblHinban As System.Windows.Forms.Label
    Friend WithEvents txtHinban As System.Windows.Forms.TextBox
    Friend WithEvents lblHinmei As System.Windows.Forms.Label
    Friend WithEvents txtHinmei As System.Windows.Forms.TextBox
    Friend WithEvents lblTanka As System.Windows.Forms.Label
    Friend WithEvents txtTanka As System.Windows.Forms.TextBox
    Friend WithEvents lblTankaChui As System.Windows.Forms.Label
    Friend WithEvents btnToroku As System.Windows.Forms.Button
    Friend WithEvents btnShiyoNashi As System.Windows.Forms.Button
    Friend WithEvents btnClear As System.Windows.Forms.Button
    Friend WithEvents btnClose As System.Windows.Forms.Button
End Class
