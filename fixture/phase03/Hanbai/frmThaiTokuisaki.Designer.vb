<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmThaiTokuisaki
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
        Me.lblKensaku = New System.Windows.Forms.Label
        Me.txtKensaku = New System.Windows.Forms.TextBox
        Me.chkShiyoNashi = New System.Windows.Forms.CheckBox
        Me.btnKensaku = New System.Windows.Forms.Button
        Me.lblSetsumei = New System.Windows.Forms.Label
        Me.dgvIchiran = New System.Windows.Forms.DataGridView
        Me.lblKensu = New System.Windows.Forms.Label
        Me.grpNyuryoku = New System.Windows.Forms.GroupBox
        Me.lblTokuisakiCd = New System.Windows.Forms.Label
        Me.txtTokuisakiCd = New System.Windows.Forms.TextBox
        Me.lblTokuisakiNm = New System.Windows.Forms.Label
        Me.txtTokuisakiNm = New System.Windows.Forms.TextBox
        Me.lblBiko = New System.Windows.Forms.Label
        Me.txtBiko = New System.Windows.Forms.TextBox
        Me.btnToroku = New System.Windows.Forms.Button
        Me.btnShiyoNashi = New System.Windows.Forms.Button
        Me.btnClear = New System.Windows.Forms.Button
        Me.btnClose = New System.Windows.Forms.Button
        CType(Me.dgvIchiran, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.grpNyuryoku.SuspendLayout()
        Me.SuspendLayout()
        '
        'lblKensaku
        '
        Me.lblKensaku.AutoSize = True
        Me.lblKensaku.Location = New System.Drawing.Point(12, 15)
        Me.lblKensaku.Name = "lblKensaku"
        Me.lblKensaku.Size = New System.Drawing.Size(53, 12)
        Me.lblKensaku.TabIndex = 0
        Me.lblKensaku.Text = "コード/名"
        '
        'txtKensaku
        '
        Me.txtKensaku.Location = New System.Drawing.Point(72, 12)
        Me.txtKensaku.MaxLength = 40
        Me.txtKensaku.Name = "txtKensaku"
        Me.txtKensaku.Size = New System.Drawing.Size(180, 19)
        Me.txtKensaku.TabIndex = 1
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
        'lblSetsumei
        '
        Me.lblSetsumei.AutoSize = True
        Me.lblSetsumei.Location = New System.Drawing.Point(12, 42)
        Me.lblSetsumei.Name = "lblSetsumei"
        Me.lblSetsumei.Size = New System.Drawing.Size(437, 12)
        Me.lblSetsumei.TabIndex = 4
        Me.lblSetsumei.Text = "※現地のCSVには会社名が入っていないので、ここで名前を付けておく"
        '
        'dgvIchiran
        '
        Me.dgvIchiran.AllowUserToAddRows = False
        Me.dgvIchiran.AllowUserToDeleteRows = False
        Me.dgvIchiran.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvIchiran.Location = New System.Drawing.Point(12, 62)
        Me.dgvIchiran.MultiSelect = False
        Me.dgvIchiran.Name = "dgvIchiran"
        Me.dgvIchiran.ReadOnly = True
        Me.dgvIchiran.RowHeadersVisible = False
        Me.dgvIchiran.RowTemplate.Height = 21
        Me.dgvIchiran.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvIchiran.Size = New System.Drawing.Size(760, 260)
        Me.dgvIchiran.TabIndex = 5
        '
        'lblKensu
        '
        Me.lblKensu.AutoSize = True
        Me.lblKensu.Location = New System.Drawing.Point(12, 330)
        Me.lblKensu.Name = "lblKensu"
        Me.lblKensu.Size = New System.Drawing.Size(41, 12)
        Me.lblKensu.TabIndex = 6
        Me.lblKensu.Text = "件数 0 件"
        '
        'grpNyuryoku
        '
        Me.grpNyuryoku.Controls.Add(Me.lblTokuisakiCd)
        Me.grpNyuryoku.Controls.Add(Me.txtTokuisakiCd)
        Me.grpNyuryoku.Controls.Add(Me.lblTokuisakiNm)
        Me.grpNyuryoku.Controls.Add(Me.txtTokuisakiNm)
        Me.grpNyuryoku.Controls.Add(Me.lblBiko)
        Me.grpNyuryoku.Controls.Add(Me.txtBiko)
        Me.grpNyuryoku.Location = New System.Drawing.Point(12, 350)
        Me.grpNyuryoku.Name = "grpNyuryoku"
        Me.grpNyuryoku.Size = New System.Drawing.Size(760, 95)
        Me.grpNyuryoku.TabIndex = 7
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
        'lblBiko
        '
        Me.lblBiko.AutoSize = True
        Me.lblBiko.Location = New System.Drawing.Point(16, 60)
        Me.lblBiko.Name = "lblBiko"
        Me.lblBiko.Size = New System.Drawing.Size(29, 12)
        Me.lblBiko.TabIndex = 4
        Me.lblBiko.Text = "備考"
        '
        'txtBiko
        '
        Me.txtBiko.Location = New System.Drawing.Point(100, 57)
        Me.txtBiko.MaxLength = 100
        Me.txtBiko.Name = "txtBiko"
        Me.txtBiko.Size = New System.Drawing.Size(600, 19)
        Me.txtBiko.TabIndex = 5
        '
        'btnToroku
        '
        Me.btnToroku.Location = New System.Drawing.Point(452, 455)
        Me.btnToroku.Name = "btnToroku"
        Me.btnToroku.Size = New System.Drawing.Size(75, 23)
        Me.btnToroku.TabIndex = 8
        Me.btnToroku.Text = "登録(&S)"
        Me.btnToroku.UseVisualStyleBackColor = True
        '
        'btnShiyoNashi
        '
        Me.btnShiyoNashi.Location = New System.Drawing.Point(533, 455)
        Me.btnShiyoNashi.Name = "btnShiyoNashi"
        Me.btnShiyoNashi.Size = New System.Drawing.Size(75, 23)
        Me.btnShiyoNashi.TabIndex = 9
        Me.btnShiyoNashi.Text = "使用しない(&D)"
        Me.btnShiyoNashi.UseVisualStyleBackColor = True
        '
        'btnClear
        '
        Me.btnClear.Location = New System.Drawing.Point(614, 455)
        Me.btnClear.Name = "btnClear"
        Me.btnClear.Size = New System.Drawing.Size(75, 23)
        Me.btnClear.TabIndex = 10
        Me.btnClear.Text = "クリア(&C)"
        Me.btnClear.UseVisualStyleBackColor = True
        '
        'btnClose
        '
        Me.btnClose.Location = New System.Drawing.Point(697, 455)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(75, 23)
        Me.btnClose.TabIndex = 11
        Me.btnClose.Text = "閉じる(&X)"
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'frmThaiTokuisaki
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(784, 491)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.btnClear)
        Me.Controls.Add(Me.btnShiyoNashi)
        Me.Controls.Add(Me.btnToroku)
        Me.Controls.Add(Me.grpNyuryoku)
        Me.Controls.Add(Me.lblKensu)
        Me.Controls.Add(Me.dgvIchiran)
        Me.Controls.Add(Me.lblSetsumei)
        Me.Controls.Add(Me.btnKensaku)
        Me.Controls.Add(Me.chkShiyoNashi)
        Me.Controls.Add(Me.txtKensaku)
        Me.Controls.Add(Me.lblKensaku)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.Name = "frmThaiTokuisaki"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "タイ得意先登録"
        CType(Me.dgvIchiran, System.ComponentModel.ISupportInitialize).EndInit()
        Me.grpNyuryoku.ResumeLayout(False)
        Me.grpNyuryoku.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblKensaku As System.Windows.Forms.Label
    Friend WithEvents txtKensaku As System.Windows.Forms.TextBox
    Friend WithEvents chkShiyoNashi As System.Windows.Forms.CheckBox
    Friend WithEvents btnKensaku As System.Windows.Forms.Button
    Friend WithEvents lblSetsumei As System.Windows.Forms.Label
    Friend WithEvents dgvIchiran As System.Windows.Forms.DataGridView
    Friend WithEvents lblKensu As System.Windows.Forms.Label
    Friend WithEvents grpNyuryoku As System.Windows.Forms.GroupBox
    Friend WithEvents lblTokuisakiCd As System.Windows.Forms.Label
    Friend WithEvents txtTokuisakiCd As System.Windows.Forms.TextBox
    Friend WithEvents lblTokuisakiNm As System.Windows.Forms.Label
    Friend WithEvents txtTokuisakiNm As System.Windows.Forms.TextBox
    Friend WithEvents lblBiko As System.Windows.Forms.Label
    Friend WithEvents txtBiko As System.Windows.Forms.TextBox
    Friend WithEvents btnToroku As System.Windows.Forms.Button
    Friend WithEvents btnShiyoNashi As System.Windows.Forms.Button
    Friend WithEvents btnClear As System.Windows.Forms.Button
    Friend WithEvents btnClose As System.Windows.Forms.Button
End Class
