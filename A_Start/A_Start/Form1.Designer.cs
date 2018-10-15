namespace A_Start
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.PanelMap = new System.Windows.Forms.FlowLayoutPanel();
            this.AddStones = new System.Windows.Forms.Button();
            this.Action = new System.Windows.Forms.Button();
            this.Reset = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // PanelMap
            // 
            this.PanelMap.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.PanelMap.Location = new System.Drawing.Point(12, 12);
            this.PanelMap.Name = "PanelMap";
            this.PanelMap.Size = new System.Drawing.Size(406, 407);
            this.PanelMap.TabIndex = 0;
            this.PanelMap.Paint += new System.Windows.Forms.PaintEventHandler(this.PanelMap_Paint);
            this.PanelMap.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PanelMap_MouseDown);
            // 
            // AddStones
            // 
            this.AddStones.Location = new System.Drawing.Point(451, 12);
            this.AddStones.Name = "AddStones";
            this.AddStones.Size = new System.Drawing.Size(102, 34);
            this.AddStones.TabIndex = 1;
            this.AddStones.Text = "手动添加障碍物";
            this.AddStones.UseVisualStyleBackColor = true;
            this.AddStones.Click += new System.EventHandler(this.AddStones_Click);
            // 
            // Action
            // 
            this.Action.Location = new System.Drawing.Point(451, 74);
            this.Action.Name = "Action";
            this.Action.Size = new System.Drawing.Size(102, 34);
            this.Action.TabIndex = 2;
            this.Action.Text = "开始演示算法";
            this.Action.UseVisualStyleBackColor = true;
            this.Action.Click += new System.EventHandler(this.Action_Click);
            // 
            // Reset
            // 
            this.Reset.Location = new System.Drawing.Point(451, 136);
            this.Reset.Name = "Reset";
            this.Reset.Size = new System.Drawing.Size(102, 34);
            this.Reset.TabIndex = 3;
            this.Reset.Text = "重置";
            this.Reset.UseVisualStyleBackColor = true;
            this.Reset.Click += new System.EventHandler(this.Reset_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(604, 448);
            this.Controls.Add(this.Reset);
            this.Controls.Add(this.Action);
            this.Controls.Add(this.AddStones);
            this.Controls.Add(this.PanelMap);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "A* Automatic Path Finding Algorithm Dynamic Demonstration";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel PanelMap;
        private System.Windows.Forms.Button AddStones;
        private System.Windows.Forms.Button Action;
        private System.Windows.Forms.Button Reset;
    }
}

