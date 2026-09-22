<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmJuchuTorikomi
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
        Me.components = New System.ComponentModel.Container
        Me.lblFile = New System.Windows.Forms.Label
        Me.txtFile = New System.Windows.Forms.TextBox
        Me.btnSansho = New System.Windows.Forms.Button
        Me.btnTorikomi = New System.Windows.Forms.Button
        Me.lblSetsumei = New System.Windows.Forms.Label
        Me.lblKekka = New System.Windows.Forms.Label
        Me.dgvKekka = New System.Windows.Forms.DataGridView
        Me.btnClose = New System.Windows.Forms.Button
        Me.ofdFile = New System.Windows.Forms.OpenFileDialog
        CType(Me.dgvKekka, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lblFile
        '
        Me.lblFile.AutoSize = True
        Me.lblFile.Location = New System.Drawing.Point(12, 18)
        Me.lblFile.Name = "lblFile"
        Me.lblFile.Size = New System.Drawing.Size(53, 12)
        Me.lblFile.TabIndex = 0
        Me.lblFile.Text = "ファイル"
        '
        'txtFile
        '
        Me.txtFile.Location = New System.Drawing.Point(72, 15)
        Me.txtFile.MaxLength = 260
        Me.txtFile.Name = "txtFile"
        Me.txtFile.Size = New System.Drawing.Size(520, 19)
        Me.txtFile.TabIndex = 1
        '
        'btnSansho
        '
        Me.btnSansho.Location = New System.Drawing.Point(604, 13)
        Me.btnSansho.Name = "btnSansho"
        Me.btnSansho.Size = New System.Drawing.Size(75, 23)
        Me.btnSansho.TabIndex = 2
        Me.btnSansho.Text = "参照(&R)"
        Me.btnSansho.UseVisualStyleBackColor = True
        '
        'btnTorikomi
        '
        Me.btnTorikomi.Location = New System.Drawing.Point(692, 13)
        Me.btnTorikomi.Name = "btnTorikomi"
        Me.btnTorikomi.Size = New System.Drawing.Size(80, 23)
        Me.btnTorikomi.TabIndex = 3
        Me.btnTorikomi.Text = "取込(&I)"
        Me.btnTorikomi.UseVisualStyleBackColor = True
        '
        'lblSetsumei
        '
        Me.lblSetsumei.AutoSize = True
        Me.lblSetsumei.Location = New System.Drawing.Point(72, 44)
        Me.lblSetsumei.Name = "lblSetsumei"
        Me.lblSetsumei.Size = New System.Drawing.Size(437, 12)
        Me.lblSetsumei.TabIndex = 4
        Me.lblSetsumei.Text = "※生産管理が \\SVR01\share\seisan\ に毎晩出しているファイルを選んでください"
        '
        'lblKekka
        '
        Me.lblKekka.AutoSize = True
        Me.lblKekka.Location = New System.Drawing.Point(12, 70)
        Me.lblKekka.Name = "lblKekka"
        Me.lblKekka.Size = New System.Drawing.Size(0, 12)
        Me.lblKekka.TabIndex = 5
        '
        'dgvKekka
        '
        Me.dgvKekka.AllowUserToAddRows = False
        Me.dgvKekka.AllowUserToDeleteRows = False
        Me.dgvKekka.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvKekka.Location = New System.Drawing.Point(12, 90)
        Me.dgvKekka.MultiSelect = False
        Me.dgvKekka.Name = "dgvKekka"
        Me.dgvKekka.ReadOnly = True
        Me.dgvKekka.RowHeadersVisible = False
        Me.dgvKekka.RowTemplate.Height = 21
        Me.dgvKekka.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvKekka.Size = New System.Drawing.Size(760, 280)
        Me.dgvKekka.TabIndex = 6
        '
        'btnClose
        '
        Me.btnClose.Location = New System.Drawing.Point(697, 382)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(75, 23)
        Me.btnClose.TabIndex = 7
        Me.btnClose.Text = "閉じる(&X)"
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'ofdFile
        '
        Me.ofdFile.FileName = ""
        '
        'frmJuchuTorikomi
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(784, 418)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.dgvKekka)
        Me.Controls.Add(Me.lblKekka)
        Me.Controls.Add(Me.lblSetsumei)
        Me.Controls.Add(Me.btnTorikomi)
        Me.Controls.Add(Me.btnSansho)
        Me.Controls.Add(Me.txtFile)
        Me.Controls.Add(Me.lblFile)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.Name = "frmJuchuTorikomi"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "受注取込"
        CType(Me.dgvKekka, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblFile As System.Windows.Forms.Label
    Friend WithEvents txtFile As System.Windows.Forms.TextBox
    Friend WithEvents btnSansho As System.Windows.Forms.Button
    Friend WithEvents btnTorikomi As System.Windows.Forms.Button
    Friend WithEvents lblSetsumei As System.Windows.Forms.Label
    Friend WithEvents lblKekka As System.Windows.Forms.Label
    Friend WithEvents dgvKekka As System.Windows.Forms.DataGridView
    Friend WithEvents btnClose As System.Windows.Forms.Button
    Friend WithEvents ofdFile As System.Windows.Forms.OpenFileDialog
End Class
