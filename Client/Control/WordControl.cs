using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Threading;
using RestSharp;
using System.IO;

namespace Client.Control
{
    public partial class WordControl : UserControl
    {
        public WordControl()
        {
            InitializeComponent();
            tabControl1.Dock = DockStyle.Fill;
            cbx_device.SelectedIndex = 0;
            tbx_urls.TextChanged += (s, e) =>
            {
                label2.Text = "列表行数:" + tbx_urls.Lines.Length;
            };

        }

        private CancellationTokenSource cancellationToken = null;
        private void btn_select_Click(object sender, EventArgs e)
        {
            var urls = tbx_urls.Lines.Where(m => Regex.IsMatch(m, @"^[a-z0-9\.\-]+$") && !m.Equals("")).ToArray();
            if (!urls.Any())
            {
                MessageBox.Show("请参考示例填写正确网址,每行一个", "提示");
                return;
            }
            label2.Text = "网站查询数量：" + urls.Length;
            progressBar1.Value = 0;
            progressBar1.Maximum = urls.Length;


            if (cancellationToken != null && !cancellationToken.IsCancellationRequested)
            {
                cancellationToken.Cancel();
                return;
            }

            cancellationToken = new CancellationTokenSource();
            cancellationToken.Token.Register(() =>
            {
                btn_export.Enabled = btn_clear.Enabled = tbx_urls.Enabled = cbx_device.Enabled = true;
                btn_select.Text = "查询";
            });

            btn_export.Enabled = btn_clear.Enabled = tbx_urls.Enabled = cbx_device.Enabled = false;
            btn_select.Text = "取消";

            var device = cbx_device.Text.ToLower();
            Task.Run(() =>
            {
                var client = new RestSharp.RestClient("https://baidurank.aizhan.com");
                client.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/76.0.3809.100 Safari/537.36";
                client.CookieContainer = new System.Net.CookieContainer();

                foreach (var url in urls)
                {
                    if (cancellationToken.IsCancellationRequested)
                        break;
                    var request = new RestSharp.RestRequest();
                    if (device == "pc")
                    {
                        request.Resource = $"baidu/{url}/";
                    }
                    else
                    {
                        request.Resource = $"mobile/{url}/";
                    }
                    var resp = client.Get(request);
                    if (!resp.IsSuccessful)
                        resp = client.Get(request);
                    if (resp.IsSuccessful)
                    {
                        var htmldoc = new HtmlAgilityPack.HtmlDocument();
                        htmldoc.LoadHtml(resp.Content);
                        var nodes = htmldoc.DocumentNode.SelectNodes("//div[@class='baidurank-list']/table//tr/td[@class='title']/a/../..");
                        NewMethod(url, nodes);

                        var pages = htmldoc.DocumentNode.SelectNodes("//div[@class='baidurank-pager']/ul/a[@rel='nofollow']");
                        if (pages != null)
                        {
                            var pageurls = pages.Select(m => m.GetAttributeValue("href", ""));
                            pageurls.Where(m => m.Contains("http")).ToList().ForEach(m =>
                            {
                                var request2 = new RestSharp.RestRequest();
                                request2.Resource = m.Replace("https://baidurank.aizhan.com/", "");
                                var resp2 = client.Get(request2);
                                if (resp2.IsSuccessful)
                                {
                                    var htmldoc2 = new HtmlAgilityPack.HtmlDocument();
                                    htmldoc2.LoadHtml(resp2.Content);
                                    var nodes2 = htmldoc2.DocumentNode.SelectNodes("//div[@class='baidurank-list']/table//tr/td[@class='title']/a/../..");
                                    NewMethod(url, nodes2);
                                }
                            });

                        }
                    }
                    progressBar1.Invoke(new MethodInvoker(() =>
                    {
                        progressBar1.PerformStep();
                    }));

                }
                cbx_device.Invoke(new MethodInvoker(() =>
                {
                    btn_export.Enabled = btn_clear.Enabled = tbx_urls.Enabled = cbx_device.Enabled = true;
                    btn_select.Text = "查询";
                    label3.Text = "查询完成\r\n共有数量：" + lv_result.Items.Count;
                }));

            }, cancellationToken.Token);

        }

        private void NewMethod(string url, HtmlAgilityPack.HtmlNodeCollection nodes)
        {
            if (nodes == null)
                return;
            foreach (var tr in nodes)
            {
                var word = tr.SelectSingleNode(tr.XPath + "/td[@class='title']");
                var rank = tr.SelectSingleNode(tr.XPath + "/td[@class='center']/span[@class='blue']");
                if (word != null && rank != null)
                {
                    lv_result.Invoke(new MethodInvoker(() =>
                    {
                        lv_result.Items.Add(new ListViewItem(new string[] { lv_result.Items.Count.ToString(), url, word.InnerText.Trim(), cbx_device.Text, rank.InnerText.Replace("\t", "").Replace("\n", "").Trim() }));
                    }));
                }
            }
        }

        private void btn_clear_Click(object sender, EventArgs e)
        {
            lv_result.Items.Clear();
        }

        private void btn_export_Click(object sender, EventArgs e)
        {
            if (lv_result.Items.Count > 0)
            {
                try
                {
                    SaveFileDialog saveFile = new SaveFileDialog();
                    saveFile.Filter = "文本文件(*.txt)|*.txt";
                    saveFile.FileName = "word-" + DateTime.Now.ToString("yyyyMMdd-HHmmss") + ".txt";
                    saveFile.Title = "导出文件";
                    saveFile.CheckPathExists = true;
                    saveFile.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
                    if (saveFile.ShowDialog() == DialogResult.OK)
                    {
                        StringBuilder stringBuilder = new StringBuilder();
                        foreach (ListViewItem item in lv_result.Items)
                        {
                            if (item.SubItems.Count > 4)
                            {
                                stringBuilder.AppendLine($"{item.SubItems[1].Text}\t{item.SubItems[2].Text}\t{item.SubItems[3].Text}\t{item.SubItems[4].Text}");
                            }
                        }
                        File.WriteAllText(saveFile.FileName, stringBuilder.ToString(), Encoding.UTF8);
                        MessageBox.Show("导出成功", "提示");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "错误");
                }
            }
        }
    }
}
