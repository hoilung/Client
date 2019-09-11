﻿namespace Client.Control
{
    partial class LinkControl
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

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.btx_outlink = new System.Windows.Forms.TextBox();
            this.btn_add = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.btn_pub = new System.Windows.Forms.Button();
            this.tbx_urls = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.nd_num = new System.Windows.Forms.NumericUpDown();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.progressBar2 = new System.Windows.Forms.ProgressBar();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lb_current = new System.Windows.Forms.Label();
            this.lb_total = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.lv_link = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nd_num)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(760, 450);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.label8);
            this.tabPage1.Controls.Add(this.label7);
            this.tabPage1.Controls.Add(this.lb_total);
            this.tabPage1.Controls.Add(this.lb_current);
            this.tabPage1.Controls.Add(this.label4);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.progressBar2);
            this.tabPage1.Controls.Add(this.progressBar1);
            this.tabPage1.Controls.Add(this.nd_num);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.tbx_urls);
            this.tabPage1.Controls.Add(this.btn_pub);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(752, 424);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "批量发布";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.label1);
            this.tabPage2.Controls.Add(this.btn_add);
            this.tabPage2.Controls.Add(this.btx_outlink);
            this.tabPage2.Controls.Add(this.lv_link);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(752, 424);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "外链管理";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // btx_outlink
            // 
            this.btx_outlink.Location = new System.Drawing.Point(6, 16);
            this.btx_outlink.Multiline = true;
            this.btx_outlink.Name = "btx_outlink";
            this.btx_outlink.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.btx_outlink.Size = new System.Drawing.Size(557, 130);
            this.btx_outlink.TabIndex = 1;
            this.btx_outlink.WordWrap = false;
            // 
            // btn_add
            // 
            this.btn_add.Location = new System.Drawing.Point(569, 16);
            this.btn_add.Name = "btn_add";
            this.btn_add.Size = new System.Drawing.Size(75, 23);
            this.btn_add.TabIndex = 2;
            this.btn_add.Text = "添加外链";
            this.btn_add.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 154);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(491, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "新增外链需要每行一个，{url}为网址变量 参考示例：https://www.aizhan.com/cha/{url}/\r\n";
            // 
            // btn_pub
            // 
            this.btn_pub.Location = new System.Drawing.Point(652, 15);
            this.btn_pub.Name = "btn_pub";
            this.btn_pub.Size = new System.Drawing.Size(75, 23);
            this.btn_pub.TabIndex = 0;
            this.btn_pub.Text = "开始发布";
            this.btn_pub.UseVisualStyleBackColor = true;
            // 
            // tbx_urls
            // 
            this.tbx_urls.Location = new System.Drawing.Point(75, 18);
            this.tbx_urls.Multiline = true;
            this.tbx_urls.Name = "tbx_urls";
            this.tbx_urls.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbx_urls.Size = new System.Drawing.Size(474, 182);
            this.tbx_urls.TabIndex = 1;
            this.tbx_urls.WordWrap = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "网站列表";
            // 
            // nd_num
            // 
            this.nd_num.Location = new System.Drawing.Point(555, 18);
            this.nd_num.Name = "nd_num";
            this.nd_num.Size = new System.Drawing.Size(91, 21);
            this.nd_num.TabIndex = 3;
            this.nd_num.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(75, 206);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(474, 23);
            this.progressBar1.Step = 1;
            this.progressBar1.TabIndex = 4;
            // 
            // progressBar2
            // 
            this.progressBar2.Location = new System.Drawing.Point(75, 235);
            this.progressBar2.Name = "progressBar2";
            this.progressBar2.Size = new System.Drawing.Size(474, 23);
            this.progressBar2.Step = 1;
            this.progressBar2.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 211);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 6;
            this.label3.Text = "当前进度";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(16, 240);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 12);
            this.label4.TabIndex = 7;
            this.label4.Text = "总进度";
            // 
            // lb_current
            // 
            this.lb_current.AutoSize = true;
            this.lb_current.Location = new System.Drawing.Point(623, 211);
            this.lb_current.Name = "lb_current";
            this.lb_current.Size = new System.Drawing.Size(23, 12);
            this.lb_current.TabIndex = 8;
            this.lb_current.Text = "0/0";
            // 
            // lb_total
            // 
            this.lb_total.AutoSize = true;
            this.lb_total.Location = new System.Drawing.Point(623, 240);
            this.lb_total.Name = "lb_total";
            this.lb_total.Size = new System.Drawing.Size(23, 12);
            this.lb_total.TabIndex = 9;
            this.lb_total.Text = "0/0";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(16, 289);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(41, 12);
            this.label7.TabIndex = 10;
            this.label7.Text = "说明：";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(73, 289);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(425, 48);
            this.label8.TabIndex = 11;
            this.label8.Text = "要发布的外链网站每行一个，设置好批量发布的总次数后，点【开始发布】即可\r\n例如:\r\nwww.baidu.com\r\nwww.baidu2.com";
            // 
            // lv_link
            // 
            this.lv_link.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.lv_link.FullRowSelect = true;
            this.lv_link.GridLines = true;
            this.lv_link.HideSelection = false;
            this.lv_link.Location = new System.Drawing.Point(6, 184);
            this.lv_link.Name = "lv_link";
            this.lv_link.Size = new System.Drawing.Size(740, 234);
            this.lv_link.TabIndex = 0;
            this.lv_link.UseCompatibleStateImageBehavior = false;
            this.lv_link.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "#";
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "外链地址";
            this.columnHeader2.Width = 500;
            // 
            // LinkControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl1);
            this.Name = "LinkControl";
            this.Size = new System.Drawing.Size(760, 450);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nd_num)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TextBox btx_outlink;
        private System.Windows.Forms.Button btn_add;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_pub;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbx_urls;
        private System.Windows.Forms.NumericUpDown nd_num;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.ProgressBar progressBar2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lb_current;
        private System.Windows.Forms.Label lb_total;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ListView lv_link;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
    }
}
