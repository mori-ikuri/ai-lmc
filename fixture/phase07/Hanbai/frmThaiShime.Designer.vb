<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmThaiShime
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
        Me.btnKensaku = New System.Windows.Forms.Button
        Me.lblJotai = New System.Windows.Forms.Label
        Me.lblChui = New System.Windows.Forms.Label
        Me.dgvTaisho = New System.Windows.Forms.DataGridView
        Me.lblKensu = New System.Windows.Forms.Label
        Me.lblGokei = New System.Windows.Forms.Label
        Me.lblMeisai = New System.Windows.Forms.Label
        Me.dgvNohin = New System.Windows.Forms.DataGridView
        Me.btnShime = New System.Windows.Forms.Button
        Me.btnTorikeshi = New System.Windows.Forms.Button
        Me.btnClose = New System.Windows.Forms.Button
        CType(Me.dgvTaisho, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgvNohin, System.ComponentModel.ISupportInitialize).BeginInit()
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
        'btnKensaku
        '
        Me.btnKensaku.Location = New System.Drawing.Point(240, 10)
        Me.btnKensaku.Name = "btnKensaku"
        Me.btnKensaku.Size = New System.Drawing.Size(100, 23)
        Me.btnKensaku.TabIndex = 5
        Me.btnKensaku.Text = "対象を出す(&F)"
        Me.btnKensaku.UseVisualStyleBackColor = True
        '
        'lblJotai
        '
        Me.lblJotai.AutoSize = True
        Me.lblJotai.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lblJotai.ForeColor = System.Drawing.Color.Red
        Me.lblJotai.Location = New System.Drawing.Point(360, 15)
        Me.lblJotai.Name = "lblJotai"
        Me.lblJotai.Size = New System.Drawing.Size(0, 12)
        Me.lblJotai.TabIndex = 6
        '
        'lblChui
        '
        Me.lblChui.AutoSize = True
        Me.lblChui.ForeColor = System.Drawing.Color.Red
        Me.lblChui.Location = New System.Drawing.Point(360, 278)
        Me.lblChui.Name = "lblChui"
        Me.lblChui.Size = New System.Drawing.Size(0, 12)
        Me.lblChui.TabIndex = 9
        '
        'dgvTaisho
        '
        Me.dgvTaisho.AllowUserToAddRows = False
        Me.dgvTaisho.AllowUserToDeleteRows = False
        Me.dgvTaisho.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvTaisho.Location = New System.Drawing.Point(12, 42)
        Me.dgvTaisho.MultiSelect = False
        Me.dgvTaisho.Name = "dgvTaisho"
        Me.dgvTaisho.ReadOnly = True
        Me.dgvTaisho.RowHeadersVisible = False
        Me.dgvTaisho.RowTemplate.Height = 21
        Me.dgvTaisho.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvTaisho.Size = New System.Drawing.Size(860, 230)
        Me.dgvTaisho.TabIndex = 7
        '
        'lblKensu
        '
        Me.lblKensu.AutoSize = True
        Me.lblKensu.Location = New System.Drawing.Point(12, 278)
        Me.lblKensu.Name = "lblKensu"
        Me.lblKensu.Size = New System.Drawing.Size(65, 12)
        Me.lblKensu.TabIndex = 8
        Me.lblKensu.Text = "得意先 0 社"
        '
        'lblGokei
        '
        Me.lblGokei.AutoSize = True
        Me.lblGokei.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 11.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lblGokei.Location = New System.Drawing.Point(12, 296)
        Me.lblGokei.Name = "lblGokei"
        Me.lblGokei.Size = New System.Drawing.Size(0, 15)
        Me.lblGokei.TabIndex = 10
        '
        'lblMeisai
        '
        Me.lblMeisai.AutoSize = True
        Me.lblMeisai.Location = New System.Drawing.Point(12, 322)
        Me.lblMeisai.Name = "lblMeisai"
        Me.lblMeisai.Size = New System.Drawing.Size(101, 12)
        Me.lblMeisai.TabIndex = 11
        Me.lblMeisai.Text = "この得意先の納品"
        '
        'dgvNohin
        '
        Me.dgvNohin.AllowUserToAddRows = False
        Me.dgvNohin.AllowUserToDeleteRows = False
        Me.dgvNohin.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvNohin.Location = New System.Drawing.Point(12, 338)
        Me.dgvNohin.MultiSelect = False
        Me.dgvNohin.Name = "dgvNohin"
        Me.dgvNohin.ReadOnly = True
        Me.dgvNohin.RowHeadersVisible = False
        Me.dgvNohin.RowTemplate.Height = 21
        Me.dgvNohin.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvNohin.Size = New System.Drawing.Size(860, 208)
        Me.dgvNohin.TabIndex = 12
        '
        'btnShime
        '
        Me.btnShime.Location = New System.Drawing.Point(616, 560)
        Me.btnShime.Name = "btnShime"
        Me.btnShime.Size = New System.Drawing.Size(85, 23)
        Me.btnShime.TabIndex = 13
        Me.btnShime.Text = "締める(&S)"
        Me.btnShime.UseVisualStyleBackColor = True
        '
        'btnTorikeshi
        '
        Me.btnTorikeshi.Location = New System.Drawing.Point(707, 560)
        Me.btnTorikeshi.Name = "btnTorikeshi"
        Me.btnTorikeshi.Size = New System.Drawing.Size(85, 23)
        Me.btnTorikeshi.TabIndex = 14
        Me.btnTorikeshi.Text = "締め取消(&D)"
        Me.btnTorikeshi.UseVisualStyleBackColor = True
        '
        'btnClose
        '
        Me.btnClose.Location = New System.Drawing.Point(797, 560)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(75, 23)
        Me.btnClose.TabIndex = 15
        Me.btnClose.Text = "閉じる(&X)"
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'frmThaiShime
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(884, 596)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.btnTorikeshi)
        Me.Controls.Add(Me.btnShime)
        Me.Controls.Add(Me.dgvNohin)
        Me.Controls.Add(Me.lblMeisai)
        Me.Controls.Add(Me.lblGokei)
        Me.Controls.Add(Me.lblChui)
        Me.Controls.Add(Me.lblKensu)
        Me.Controls.Add(Me.dgvTaisho)
        Me.Controls.Add(Me.lblJotai)
        Me.Controls.Add(Me.btnKensaku)
        Me.Controls.Add(Me.lblTsuki)
        Me.Controls.Add(Me.cboTsuki)
        Me.Controls.Add(Me.lblNen)
        Me.Controls.Add(Me.txtNen)
        Me.Controls.Add(Me.lblNengetsu)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.Name = "frmThaiShime"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "タイ締め"
        CType(Me.dgvTaisho, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dgvNohin, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblNengetsu As System.Windows.Forms.Label
    Friend WithEvents txtNen As System.Windows.Forms.TextBox
    Friend WithEvents lblNen As System.Windows.Forms.Label
    Friend WithEvents cboTsuki As System.Windows.Forms.ComboBox
    Friend WithEvents lblTsuki As System.Windows.Forms.Label
    Friend WithEvents btnKensaku As System.Windows.Forms.Button
    Friend WithEvents lblJotai As System.Windows.Forms.Label
    Friend WithEvents lblChui As System.Windows.Forms.Label
    Friend WithEvents dgvTaisho As System.Windows.Forms.DataGridView
    Friend WithEvents lblKensu As System.Windows.Forms.Label
    Friend WithEvents lblGokei As System.Windows.Forms.Label
    Friend WithEvents lblMeisai As System.Windows.Forms.Label
    Friend WithEvents dgvNohin As System.Windows.Forms.DataGridView
    Friend WithEvents btnShime As System.Windows.Forms.Button
    Friend WithEvents btnTorikeshi As System.Windows.Forms.Button
    Friend WithEvents btnClose As System.Windows.Forms.Button
End Class
