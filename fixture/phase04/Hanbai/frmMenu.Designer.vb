<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMenu
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
        Me.lblTitle = New System.Windows.Forms.Label
        Me.btnTokuisaki = New System.Windows.Forms.Button
        Me.btnTanka = New System.Windows.Forms.Button
        Me.btnJuchu = New System.Windows.Forms.Button
        Me.btnNohin = New System.Windows.Forms.Button
        Me.btnShime = New System.Windows.Forms.Button
        Me.btnSeikyusho = New System.Windows.Forms.Button
        Me.btnNyukin = New System.Windows.Forms.Button
        Me.lblThai = New System.Windows.Forms.Label
        Me.btnThaiTokuisaki = New System.Windows.Forms.Button
        Me.btnThaiTorikomi = New System.Windows.Forms.Button
        Me.btnThaiJuchu = New System.Windows.Forms.Button
        Me.btnThaiNohin = New System.Windows.Forms.Button
        Me.btnThaiNohinIchiran = New System.Windows.Forms.Button
        Me.btnClose = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'lblTitle
        '
        Me.lblTitle.AutoSize = True
        Me.lblTitle.Font = New System.Drawing.Font("ＭＳ Ｐゴシック", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.lblTitle.Location = New System.Drawing.Point(24, 20)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(75, 16)
        Me.lblTitle.TabIndex = 0
        Me.lblTitle.Text = "販売"
        '
        'btnTokuisaki
        '
        Me.btnTokuisaki.Location = New System.Drawing.Point(40, 60)
        Me.btnTokuisaki.Name = "btnTokuisaki"
        Me.btnTokuisaki.Size = New System.Drawing.Size(200, 32)
        Me.btnTokuisaki.TabIndex = 1
        Me.btnTokuisaki.Text = "得意先登録(&T)"
        Me.btnTokuisaki.UseVisualStyleBackColor = True
        '
        'btnTanka
        '
        Me.btnTanka.Location = New System.Drawing.Point(40, 100)
        Me.btnTanka.Name = "btnTanka"
        Me.btnTanka.Size = New System.Drawing.Size(200, 32)
        Me.btnTanka.TabIndex = 2
        Me.btnTanka.Text = "単価登録(&K)"
        Me.btnTanka.UseVisualStyleBackColor = True
        '
        'btnJuchu
        '
        Me.btnJuchu.Location = New System.Drawing.Point(40, 140)
        Me.btnJuchu.Name = "btnJuchu"
        Me.btnJuchu.Size = New System.Drawing.Size(200, 32)
        Me.btnJuchu.TabIndex = 3
        Me.btnJuchu.Text = "受注取込(&I)"
        Me.btnJuchu.UseVisualStyleBackColor = True
        '
        'btnNohin
        '
        Me.btnNohin.Location = New System.Drawing.Point(40, 180)
        Me.btnNohin.Name = "btnNohin"
        Me.btnNohin.Size = New System.Drawing.Size(200, 32)
        Me.btnNohin.TabIndex = 4
        Me.btnNohin.Text = "納品入力(&N)"
        Me.btnNohin.UseVisualStyleBackColor = True
        '
        'btnShime
        '
        Me.btnShime.Location = New System.Drawing.Point(40, 220)
        Me.btnShime.Name = "btnShime"
        Me.btnShime.Size = New System.Drawing.Size(200, 32)
        Me.btnShime.TabIndex = 5
        Me.btnShime.Text = "締め処理(&S)"
        Me.btnShime.UseVisualStyleBackColor = True
        '
        'btnSeikyusho
        '
        Me.btnSeikyusho.Location = New System.Drawing.Point(40, 260)
        Me.btnSeikyusho.Name = "btnSeikyusho"
        Me.btnSeikyusho.Size = New System.Drawing.Size(200, 32)
        Me.btnSeikyusho.TabIndex = 6
        Me.btnSeikyusho.Text = "請求書発行(&P)"
        Me.btnSeikyusho.UseVisualStyleBackColor = True
        '
        'btnNyukin
        '
        Me.btnNyukin.Location = New System.Drawing.Point(40, 300)
        Me.btnNyukin.Name = "btnNyukin"
        Me.btnNyukin.Size = New System.Drawing.Size(200, 32)
        Me.btnNyukin.TabIndex = 7
        Me.btnNyukin.Text = "入金入力(&U)"
        Me.btnNyukin.UseVisualStyleBackColor = True
        '
        'lblThai
        '
        Me.lblThai.AutoSize = True
        Me.lblThai.Location = New System.Drawing.Point(40, 346)
        Me.lblThai.Name = "lblThai"
        Me.lblThai.Size = New System.Drawing.Size(29, 12)
        Me.lblThai.TabIndex = 8
        Me.lblThai.Text = "タイ"
        '
        'btnThaiTokuisaki
        '
        Me.btnThaiTokuisaki.Location = New System.Drawing.Point(40, 364)
        Me.btnThaiTokuisaki.Name = "btnThaiTokuisaki"
        Me.btnThaiTokuisaki.Size = New System.Drawing.Size(200, 32)
        Me.btnThaiTokuisaki.TabIndex = 9
        Me.btnThaiTokuisaki.Text = "タイ得意先登録(&A)"
        Me.btnThaiTokuisaki.UseVisualStyleBackColor = True
        '
        'btnThaiTorikomi
        '
        Me.btnThaiTorikomi.Location = New System.Drawing.Point(40, 404)
        Me.btnThaiTorikomi.Name = "btnThaiTorikomi"
        Me.btnThaiTorikomi.Size = New System.Drawing.Size(200, 32)
        Me.btnThaiTorikomi.TabIndex = 10
        Me.btnThaiTorikomi.Text = "タイ受注取込(&B)"
        Me.btnThaiTorikomi.UseVisualStyleBackColor = True
        '
        'btnThaiJuchu
        '
        Me.btnThaiJuchu.Location = New System.Drawing.Point(40, 444)
        Me.btnThaiJuchu.Name = "btnThaiJuchu"
        Me.btnThaiJuchu.Size = New System.Drawing.Size(200, 32)
        Me.btnThaiJuchu.TabIndex = 11
        Me.btnThaiJuchu.Text = "タイ受注一覧(&E)"
        Me.btnThaiJuchu.UseVisualStyleBackColor = True
        '
        'btnThaiNohin
        '
        Me.btnThaiNohin.Location = New System.Drawing.Point(40, 484)
        Me.btnThaiNohin.Name = "btnThaiNohin"
        Me.btnThaiNohin.Size = New System.Drawing.Size(200, 32)
        Me.btnThaiNohin.TabIndex = 12
        Me.btnThaiNohin.Text = "タイ納品入力(&G)"
        Me.btnThaiNohin.UseVisualStyleBackColor = True
        '
        'btnThaiNohinIchiran
        '
        Me.btnThaiNohinIchiran.Location = New System.Drawing.Point(40, 524)
        Me.btnThaiNohinIchiran.Name = "btnThaiNohinIchiran"
        Me.btnThaiNohinIchiran.Size = New System.Drawing.Size(200, 32)
        Me.btnThaiNohinIchiran.TabIndex = 13
        Me.btnThaiNohinIchiran.Text = "タイ納品一覧(&H)"
        Me.btnThaiNohinIchiran.UseVisualStyleBackColor = True
        '
        'btnClose
        '
        Me.btnClose.Location = New System.Drawing.Point(40, 576)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(200, 32)
        Me.btnClose.TabIndex = 14
        Me.btnClose.Text = "終了(&X)"
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'frmMenu
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(280, 630)
        Me.Controls.Add(Me.btnClose)
        Me.Controls.Add(Me.btnThaiNohinIchiran)
        Me.Controls.Add(Me.btnThaiNohin)
        Me.Controls.Add(Me.btnThaiJuchu)
        Me.Controls.Add(Me.btnThaiTorikomi)
        Me.Controls.Add(Me.btnThaiTokuisaki)
        Me.Controls.Add(Me.lblThai)
        Me.Controls.Add(Me.btnNyukin)
        Me.Controls.Add(Me.btnSeikyusho)
        Me.Controls.Add(Me.btnShime)
        Me.Controls.Add(Me.btnNohin)
        Me.Controls.Add(Me.btnJuchu)
        Me.Controls.Add(Me.btnTanka)
        Me.Controls.Add(Me.btnTokuisaki)
        Me.Controls.Add(Me.lblTitle)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.Name = "frmMenu"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "販売"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblTitle As System.Windows.Forms.Label
    Friend WithEvents btnTokuisaki As System.Windows.Forms.Button
    Friend WithEvents btnTanka As System.Windows.Forms.Button
    Friend WithEvents btnJuchu As System.Windows.Forms.Button
    Friend WithEvents btnNohin As System.Windows.Forms.Button
    Friend WithEvents btnShime As System.Windows.Forms.Button
    Friend WithEvents btnSeikyusho As System.Windows.Forms.Button
    Friend WithEvents btnNyukin As System.Windows.Forms.Button
    Friend WithEvents lblThai As System.Windows.Forms.Label
    Friend WithEvents btnThaiTokuisaki As System.Windows.Forms.Button
    Friend WithEvents btnThaiTorikomi As System.Windows.Forms.Button
    Friend WithEvents btnThaiJuchu As System.Windows.Forms.Button
    Friend WithEvents btnThaiNohin As System.Windows.Forms.Button
    Friend WithEvents btnThaiNohinIchiran As System.Windows.Forms.Button
    Friend WithEvents btnClose As System.Windows.Forms.Button
End Class
