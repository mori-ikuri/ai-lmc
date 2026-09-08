<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmSeikyusho
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
        Me.lblNengetsu = New System.Windows.Forms.Label
        Me.txtNen = New System.Windows.Forms.TextBox
        Me.lblNen = New System.Windows.Forms.Label
        Me.cboTsuki = New System.Windows.Forms.ComboBox
        Me.lblTsuki = New System.Windows.Forms.Label
        Me.chkMiInsatsu = New System.Windows.Forms.CheckBox
        Me.btnKensaku = New System.Windows.Forms.Button
        Me.dgvSeikyu = New System.Windows.Forms.DataGridView
        Me.lblKensu = New System.Windows.Forms.Label
        Me.btnHyoji = New System.Windows.Forms.Button
        Me.btnInsatsu = New System.Windows.Forms.Button
        Me.btnClose = New System.Windows.Forms.Button
        Me.crvSeikyusho = New CrystalDecisions.Windows.Forms.CrystalReportViewer
        CType(Me.dgvSeikyu, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lblNengetsu
        '
        Me.lblNengetsu.AutoSize = True
        Me.lblNengetsu.Location = New System.Drawing.Point(12, 15)
        Me.lblNengetsu.Name = "lblNengetsu"
        Me.lblNengetsu.Size = New System.Drawing.Size(53, 12)
        Me.lblNengetsu.TabIndex = 0
        Me.lblNengetsu.Text = "締め年月"
        '
        'txtNen
        '
        Me.txtNen.Location = New System.Drawing.Point(72, 12)
        Me.txtNen.MaxLength = 4
        Me.txtNen.Name = "txtNen"
        Me.txtNen.Size = New System.Drawing.Size(50, 19)
        Me.txtNen.TabIndex = 1
        Me.txtNen.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'lblNen
        '
        Me.lblNen.AutoSize = True
        Me.lblNen.Location = New System.Drawing.Point(126, 15)
        Me.lblNen.Name = "lblNen"
        Me.lblNen.Size = New System.Drawing.Size(17, 12)
        Me.lblNen.TabIndex = 2
        Me.lblNen.Text = "年"
        '
        'cboTsuki
        '
        Me.cboTsuki.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboTsuki.FormattingEnabled = True
        Me.cboTsuki.Location = New System.Drawing.Point(146, 12)
        Me.cboTsuki.Name = "cboTsuki"
        Me.cboTsuki.Size = New System.Drawing.Size(55, 20)
        Me.cboTsuki.TabIndex = 3
        '
        'lblTsuki
        '
        Me.lblTsuki.AutoSize = True
        Me.lblTsuki.Location = New System.Drawing.Point(205, 15)
        Me.lblTsuki.Name = "lblTsuki"
        Me.lblTsuki.Size = New System.Drawing.Size(17, 12)
        Me.lblTsuki.TabIndex = 4
        Me.lblTsuki.Text = "月"
        '
        'chkMiInsatsu
        '
        Me.chkMiInsatsu.AutoSize = True
        Me.chkMiInsatsu.Checked = True
        Me.chkMiInsatsu.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkMiInsatsu.Location = New System.Drawing.Point(240, 14)
        Me.chkMiInsatsu.Name = "chkMiInsatsu"
        Me.chkMiInsatsu.Size = New System.Drawing.Size(97, 16)
        Me.chkMiInsatsu.TabIndex = 5
        Me.chkMiInsatsu.Text = "未印刷だけ"
        Me.chkMiInsatsu.UseVisualStyleBackColor = True
        '
        'btnKensaku
        '
        Me.btnKensaku.Location = New System.Drawing.Point(356, 10)
        Me.btnKensaku.Name = "btnKensaku"
        Me.btnKensaku.Size = New System.Drawing.Size(100, 23)
        Me.btnKensaku.TabIndex = 6
        Me.btnKensaku.Text = "一覧を出す(&F)"
        Me.btnKensaku.UseVisualStyleBackColor = True
        '
        'dgvSeikyu
        '
        Me.dgvSeikyu.AllowUserToAddRows = False
        Me.dgvSeikyu.AllowUserToDeleteRows = False
        Me.dgvSeikyu.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvSeikyu.Location = New System.Drawing.Point(12, 42)
        Me.dgvSeikyu.MultiSelect = False
        Me.dgvSeikyu.Name = "dgvSeikyu"
        Me.dgvSeikyu.ReadOnly = True
        Me.dgvSeikyu.RowHeadersVisible = False
        Me.dgvSeikyu.RowTemplate.Height = 21
        Me.dgvSeikyu.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvSeikyu.Size = New System.Drawing.Size(960, 150)
        Me.dgvSeikyu.TabIndex = 7
        '
        'lblKensu
        '
        Me.lblKensu.AutoSize = True
        Me.lblKensu.Location = New System.Drawing.Point(12, 203)
        Me.lblKensu.Name = "lblKensu"
        Me.lblKensu.Size = New System.Drawing.Size(41, 12)
        Me.lblKensu.TabIndex = 8
        Me.lblKensu.Text = "件数 0 件"
        '
        'btnHyoji
        '
        Me.btnHyoji.Location = New System.Drawing.Point(706, 198)
        Me.btnHyoji.Name = "btnHyoji"
        Me.btnHyoji.Size = New System.Drawing.Size(85, 23)
        Me.btnHyoji.TabIndex = 9
        Me.btnHyoji.Text = "表示(&V)"
        Me.btnHyoji.UseVisualStyleBackColor = True
        '
        'btnInsatsu
        '
        Me.btnInsatsu.Location = New System.Drawing.Point(797, 198)
        Me.btnInsatsu.Name = "btnInsatsu"
        Me.btnInsatsu.Size = New System.Drawing.Size(85, 23)
        Me.btnInsatsu.TabIndex = 10
        Me.btnInsatsu.Text = "印刷(&P)"
        Me.btnInsatsu.UseVisualStyleBackColor = True
        '
        'btnClose
        '
        Me.btnClose.Location = New System.Drawing.Point(888, 198)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(75, 23)
        Me.btnClose.TabIndex = 11
        Me.btnClose.Text = "閉じる(&X)"
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'crvSeikyusho
        '
        Me.crvSeikyusho.ActiveViewIndex = -1
        Me.crvSeikyusho.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.crvSeikyusho.DisplayGroupTree = False
        Me.crvSeikyusho.Location = New System.Drawing.Point(12, 228)
        Me.crvSeikyusho.Name = "crvSeikyusho"
        Me.crvSeikyusho.Size = New System.Drawing.Size(960, 452)
        Me.crvSeikyusho.TabIndex = 12
        '
        'frmSeikyusho
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(984, 692)
        Me.Controls.Add(Me.crvSeikyusho)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.btnInsatsu)
        Me.Controls.Add(Me.btnHyoji)
        Me.Controls.Add(Me.lblKensu)
        Me.Controls.Add(Me.dgvSeikyu)
        Me.Controls.Add(Me.btnKensaku)
        Me.Controls.Add(Me.chkMiInsatsu)
        Me.Controls.Add(Me.lblTsuki)
        Me.Controls.Add(Me.cboTsuki)
        Me.Controls.Add(Me.lblNen)
        Me.Controls.Add(Me.txtNen)
        Me.Controls.Add(Me.lblNengetsu)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.Name = "frmSeikyusho"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "請求書発行"
        CType(Me.dgvSeikyu, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblNengetsu As System.Windows.Forms.Label
    Friend WithEvents txtNen As System.Windows.Forms.TextBox
    Friend WithEvents lblNen As System.Windows.Forms.Label
    Friend WithEvents cboTsuki As System.Windows.Forms.ComboBox
    Friend WithEvents lblTsuki As System.Windows.Forms.Label
    Friend WithEvents chkMiInsatsu As System.Windows.Forms.CheckBox
    Friend WithEvents btnKensaku As System.Windows.Forms.Button
    Friend WithEvents dgvSeikyu As System.Windows.Forms.DataGridView
    Friend WithEvents lblKensu As System.Windows.Forms.Label
    Friend WithEvents btnHyoji As System.Windows.Forms.Button
    Friend WithEvents btnInsatsu As System.Windows.Forms.Button
    Friend WithEvents btnClose As System.Windows.Forms.Button
    Friend WithEvents crvSeikyusho As CrystalDecisions.Windows.Forms.CrystalReportViewer
End Class
