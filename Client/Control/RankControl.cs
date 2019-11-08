using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RestSharp;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections;
using System.Threading;

namespace Client.Control
{
    public partial class RankControl : UserControl
    {
        public RankControl()
        {
            InitializeComponent();
            listView1.View = View.Details;
            listView1.GridLines = true;
            listView1.FullRowSelect = true;

            listView1.Columns.Add("#", 45, HorizontalAlignment.Left);
            listView1.Columns.Add("网址", 150, HorizontalAlignment.Left);
            listView1.Columns.Add("关键词", 120, HorizontalAlignment.Left);
            listView1.Columns.Add("类别", 75, HorizontalAlignment.Left);
            listView1.Columns.Add("排名", 75, HorizontalAlignment.Left);

            listView1.ColumnClick += ListView1_ColumnClick;
            listView1.ContextMenuStrip = contextMenuStrip1;
            listView1.MouseClick += ListView1_MouseClick;

            listView2.View = View.Details;
            listView2.GridLines = true;
            listView2.FullRowSelect = true;
            listView2.Columns.Add("#", 35, HorizontalAlignment.Left);
            listView2.Columns.Add("日期", 120, HorizontalAlignment.Left);
            listView2.DoubleClick += ListView2_DoubleClick;

            label7.Text = string.Empty;
            tbx_word.TextChanged += (s, e) =>
            {
                label1.Text = tbx_word.Lines.Length.ToString();
            };
            

            tbx_result.TextChanged += (s, e) =>
            {

                label2.Text = tbx_result.Lines.Length.ToString();
            };


            comboBox1.SelectedIndex = 0;
            InitHistory();
        }

        private void ListView1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                contextMenuStrip1.Show(listView1, e.Location);
            }
        }

        private void ListView2_DoubleClick(object sender, EventArgs e)
        {
            var items = listView2.SelectedItems;
            var filename = items[0].SubItems[1].Text;
            filename = Path.Combine(Directory.GetCurrentDirectory(), "wd-" + filename + ".txt");
            if (File.Exists(filename))
            {
                try
                {
                    var lines = File.ReadAllLines(filename);
                    listView1.Items.Clear();
                    int index = 0;
                    for (int i = 0; i < lines.Length; i++)
                    {
                        var item = lines[i];
                        var sp = item.Split(new[] { '\t', ' ' });
                        if (sp.Length > 3)
                        {
                            if (Regex.IsMatch(sp[3], @"^[\d]+$"))
                            {
                                var rank = int.Parse(sp[3]);
                                if (rank < 11)
                                {
                                    index += 1;
                                }
                            }

                            var lvi = new ListViewItem(listView1.Items.Count.ToString());
                            lvi.SubItems.Add(sp[0]);
                            lvi.SubItems.Add(sp[1]);
                            lvi.SubItems.Add(sp[2]);
                            lvi.SubItems.Add(sp[3]);
                            lvi.UseItemStyleForSubItems = false;
                            listView1.Items.Add(lvi);
                        }
                    }
                    listView1.EndUpdate();
                    label5.Text = "总计：" + listView1.Items.Count + " 首页：" + index;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("提示", "读取文件错误");
                }
            }
        }



        public void InitHistory()
        {
            var dic = Directory.GetFiles(Directory.GetCurrentDirectory(), "wd-*.txt");
            if (dic != null && dic.Any())
            {
                for (int i = 0; i < dic.Length; i++)
                {
                    var file = dic[i];

                    var filename = Path.GetFileName(file);
                    var lvi = new ListViewItem((i + 1).ToString());
                    lvi.SubItems.Add(Regex.Match(filename, @"\d+").Value);
                    listView2.Items.Add(lvi);
                }
            }
        }

        public class ListViewItemComparer : IComparer
        {
            private int col;
            private SortOrder SortOrder;
            public ListViewItemComparer(int cid, SortOrder sortOrder)
            {
                col = cid;
                SortOrder = sortOrder;
            }
            public int Compare(object x, object y)
            {

                int returnVal = -1;
                var a = ((ListViewItem)x).SubItems[col].Text;
                var b = ((ListViewItem)y).SubItems[col].Text;
                if (SortOrder == SortOrder.Ascending)
                {
                    returnVal = a.CompareTo(b);
                    if (col == 4 || col == 0)
                    {
                        returnVal = int.Parse(a.Replace("+", "")).CompareTo(int.Parse(b.Replace("+", "")));
                    }
                }
                else
                {
                    returnVal = b.CompareTo(a);
                    if (col == 4 || col == 0)
                    {
                        returnVal = int.Parse(b.Replace("+", "")).CompareTo(int.Parse(a.Replace("+", "")));
                    }

                }
                return returnVal;
            }
        }

        private void ListView1_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (listView1.Columns[e.Column].Tag == null)
            {
                listView1.Columns[e.Column].Tag = SortOrder.Ascending;
            }
            var so = (SortOrder)Enum.Parse(typeof(SortOrder), listView1.Columns[e.Column].Tag.ToString());
            listView1.ListViewItemSorter = new ListViewItemComparer(e.Column, so);
            listView1.Sort();

            foreach (ColumnHeader item in listView1.Columns)
            {
                item.Text = item.Text.Replace("↓", "").Replace("↑", "").Trim();
            }

            listView1.Columns[e.Column].Tag = so == SortOrder.Descending ? SortOrder.Ascending : SortOrder.Descending;
            var tmp = listView1.Columns[e.Column].Text;
            listView1.Columns[e.Column].Text = so == SortOrder.Descending ? tmp + " ↓" : tmp + " ↑";

        }

        private CancellationTokenSource cancellationToken = null;

        private void btn_check_Click(object sender, EventArgs e)
        {

            if (cancellationToken != null && !cancellationToken.IsCancellationRequested)
            {
                cancellationToken.Cancel();
                return;
            }
            cancellationToken = new CancellationTokenSource();
            cancellationToken.Token.Register(() =>
            {
                label7.Text = "取消查询 ";
                btn_check.Text = "查询";
            });

            label7.Text = string.Empty;
            var lines = tbx_word.Lines;
            var words = new List<Models.SearchResult>();
            foreach (var item in lines)
            {
                if (string.IsNullOrEmpty(item) || string.IsNullOrWhiteSpace(item))
                    continue;

                var sp = item.Split(new[] { '\t'});
                if (sp.Length > 3)
                {
                    words.Add(new Models.SearchResult()
                    {
                        SearchNodes = new List<Models.SearchNode>(),
                        Host = sp[0].Trim(),
                        Word = sp[1].Trim(),
                        Device = sp[2].ToLower()

                    });
                }
            }
            if (words.Count < 1)
            {
                return;
            }
            words = words.OrderByDescending(m => m.Host).ToList();
            progressBar1.Maximum = words.Count;
            progressBar1.Value = 0;
            progressBar1.Step = 1;

            progressBar2.Maximum = words.Count;
            progressBar2.Value = 0;
            progressBar2.Step = 1;

            var maxpage = comboBox1.SelectedIndex;
            btn_check.Text = "取消查询";
            tbx_result.Clear();
            Task.Run(() =>
            {


                words.AsParallel().ForAll(item =>
                {

                    try
                    {
                        if (cancellationToken.IsCancellationRequested)
                            return;
                        var client = new RestClient("http://www.baidu.com");
                        client.CookieContainer = new System.Net.CookieContainer();
                        client.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/76.0.3809.100 Safari/537.36";
                        client.FollowRedirects = false;

                        var status = "";
                        var request = new RestRequest();
                        var htmldoc = new HtmlAgilityPack.HtmlDocument();
                        if (item.Device == "pc")
                        {

                            #region pc


                            request.Resource = $"s?ie=utf-8&wd={System.Web.HttpUtility.UrlEncode(item.Word)}";
                            var resp = client.Get(request);
                            for (int i = 0; i < 2; i++)
                            {
                                if (!resp.IsSuccessful)
                                {
                                    Task.Delay(2000);
                                    resp = client.Get(request);
                                }
                                else
                                {
                                    break;
                                }
                            }
                            if (resp.IsSuccessful)
                            {
                                status = resp.StatusCode.ToString();
                                htmldoc.LoadHtml(resp.Content);
                                var nodes = htmldoc.DocumentNode.SelectNodes("//div[@id='content_left']/div/h3//a");
                                var nextpage = htmldoc.DocumentNode.SelectSingleNode("//*[@id='page']/a[@class='n']");
                                if (nodes == null)
                                    return;
                                nodes.ToList().ForEach(m =>
                                {
                                    item.SearchNodes.Add(new Models.SearchNode
                                    {
                                        LinkUrl = m.GetAttributeValue("href", ""),
                                        Rank = m.SelectSingleNode(m.XPath + "/../..").GetAttributeValue("id", "0"),
                                        Title = m.InnerText

                                    });
                                });
                                #region 剩余页码                                                
                                for (int i = 0; i < maxpage; i++)
                                {
                                    if (nextpage != null)
                                    {
                                        request.Resource = nextpage.GetAttributeValue("href", "");
                                        resp = client.Get(request);
                                        if (resp.IsSuccessful)
                                        {
                                            htmldoc.LoadHtml(resp.Content);
                                            nextpage = htmldoc.DocumentNode.SelectSingleNode("//*[@id='page']/a[@class='n'][2]");
                                            nodes = htmldoc.DocumentNode.SelectNodes("//div[@id='content_left']/div/h3//a");
                                            if (nodes != null)
                                            {
                                                nodes.ToList().ForEach(m =>
                                                {
                                                    item.SearchNodes.Add(new Models.SearchNode
                                                    {
                                                        LinkUrl = m.GetAttributeValue("href", ""),
                                                        Rank = m.SelectSingleNode(m.XPath + "/../..").GetAttributeValue("id", "0"),
                                                        Title = m.InnerText
                                                    });
                                                });
                                            }
                                        }

                                        //  Console.WriteLine(Task.CurrentId + "-" + i);

                                    }
                                }
                                #endregion
                            }

                            #endregion
                        }
                        else
                        {
                            #region mobile

                            client.UserAgent = "Mozilla/5.0 (iPhone; CPU iPhone OS 11_0 like Mac OS X) AppleWebKit/604.1.38 (KHTML, like Gecko) Version/11.0 Mobile/15A372 Safari/604.1";

                            //var resp = client.Get(request);
                            request.Resource = $"s?ie=utf-8&wd={System.Web.HttpUtility.UrlEncode(item.Word)}";
                            var resp = client.Get(request);
                            if (resp.StatusCode == System.Net.HttpStatusCode.Found)
                            {

                                var l = resp.Headers.FirstOrDefault(f => f.Name == "Location");
                                if (l != null)
                                {
                                    request.Resource = l.Value.ToString();
                                    resp = client.Get(request);
                                }
                            }
                            if (resp.IsSuccessful)
                            {
                                status = resp.StatusCode.ToString();

                                htmldoc.LoadHtml(resp.Content);
                                var nodes = htmldoc.DocumentNode.SelectNodes("//div[@class='results']/div[@order][@data-log]");
                                var nextpage = htmldoc.DocumentNode.SelectSingleNode("//a[@class='new-nextpage-only']");
                                nodes.ToList().ForEach(m =>
                                {
                                    var tn = m.SelectSingleNode(m.XPath + "//h3");
                                    item.SearchNodes.Add(new Models.SearchNode
                                    {
                                        Rank = m.GetAttributeValue("order", "0"),
                                        LinkUrl = Regex.Match(m.GetAttributeValue("data-log", "{'mu':'about:blank'}").Replace("'mu':''", "'mu':'about:blank'"), "(?<='mu':').+(?=')").Value, //string.IsNullOrEmpty(mulnk) ? "about:blank" : mulnk,
                                        Title = tn != null ? tn.InnerText : ""
                                    });
                                });
                                for (int i = 0; i < maxpage; i++)
                                {
                                    if (nextpage != null)
                                    {
                                        request.Resource = System.Web.HttpUtility.HtmlDecode(nextpage.GetAttributeValue("href", "").Replace("https://www.baidu.com/", ""));
                                        resp = client.Get(request);
                                        htmldoc.LoadHtml(resp.Content);
                                        nodes = htmldoc.DocumentNode.SelectNodes("//div[@class='results']/div[@order][@data-log]");
                                        nextpage = htmldoc.DocumentNode.SelectSingleNode("//a[@class='new-nextpage-only' or @class='new-nextpage']");
                                        if (nodes != null)
                                        {
                                            nodes.ToList().ForEach(m =>
                                            {
                                                var tn = m.SelectSingleNode(m.XPath + "//h3");
                                                item.SearchNodes.Add(new Models.SearchNode
                                                {
                                                    Rank = maxpage.ToString() + m.GetAttributeValue("order", "0"),
                                                    LinkUrl = Regex.Match(m.GetAttributeValue("data-log", "{'mu':'about:blank'}").Replace("'mu':''", "'mu':'about:blank'"), "(?<='mu':').+(?=')").Value, //string.IsNullOrEmpty(mulnk) ? "about:blank" : mulnk,
                                                    Title = tn != null ? tn.InnerText : ""
                                                });
                                            });
                                        }
                                    }
                                }
                            }
                            #endregion
                        }

                        progressBar1.Invoke(new MethodInvoker(() =>
                        {

                            //  toolTip1.Show(status, label7);

                            label7.Text ="查询预热："+ item.Word;
                            progressBar1.PerformStep();
                        }));
                        //  });
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }

                });
                if (cancellationToken.IsCancellationRequested)
                    return;

                var client2 = new RestClient("http://www.baidu.com");
                client2.CookieContainer = new System.Net.CookieContainer();
                client2.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/76.0.3809.100 Safari/537.36";
                client2.FollowRedirects = false;
                int index = 0;
                words.ForEach(item =>
                {
                    progressBar2.BeginInvoke(new MethodInvoker(() =>
                    {
                        label7.Text ="查询进度："+ item.Word.ToString();
                        progressBar2.PerformStep();
                    }));
                    if (item.SearchNodes != null)
                    {
                        if (item.Device == "pc")
                        {
                            try
                            {
                                item.SearchNodes.AsParallel().ForAll(m =>
                                {
                                    if (cancellationToken.IsCancellationRequested)
                                        return;

                                    var request2 = new RestRequest();
                                    request2.Resource = m.LinkUrl.Replace("http://www.baidu.com/", "");
                                    var resp2 = client2.Head(request2);
                                    if (resp2.StatusCode == System.Net.HttpStatusCode.Found)
                                    {
                                        var l = resp2.Headers.FirstOrDefault(f => f.Name == "Location");
                                        if (l != null && l.Value.ToString().StartsWith("http"))
                                        {
                                            m.Url = new Uri(l.Value.ToString());
                                        }
                                    }

                                });

                            }
                            catch (Exception ex)
                            {

                            }
                            // Task.WaitAll(tasks.ToArray());
                        }
                        else
                        {
                            item.SearchNodes.ForEach(m =>
                            {
                                try
                                {
                                    if (m.LinkUrl.StartsWith("about:blank"))
                                        m.Url = new Uri("about:blank");
                                    else
                                        m.Url = new Uri(m.LinkUrl.StartsWith("http") ? m.LinkUrl : "http://" + m.LinkUrl);
                                }
                                catch (Exception ex)
                                {

                                }
                            });
                        }
                    }
                    if (!item.Rank.Contains("+") && item.Rank.Length < 2)
                    {
                        var rank = int.Parse(item.Rank);
                        if (rank > 0 && rank < 11)
                        {
                            index += 1;
                        }
                    }
                    tbx_result.Invoke(new MethodInvoker(() =>
                    {
                        tbx_result.AppendText(item.ToString() + "\r\n");
                        var lvi = new ListViewItem(listView1.Items.Count.ToString());
                        lvi.SubItems.Add(item.Host);
                        lvi.SubItems.Add(item.Word);
                        lvi.SubItems.Add(item.Device);
                        lvi.SubItems.Add(item.Rank);
                        lvi.UseItemStyleForSubItems = false;
                        listView1.Items.Add(lvi);                        

                    }));

                });
                listView1.EndUpdate();
                var file = File.AppendText($"wd-{DateTime.Now.ToString("yyyyMMdd")}.txt");
                file.WriteLine($"=========={DateTime.Now.ToString("yyyy-MM-dd HH:mm")}==========");
                file.WriteLine(tbx_result.Text);
                file.Flush();
                file.Close();

                btn_check.BeginInvoke(new MethodInvoker(() =>
                {
                    label7.Text = "查询完成：首页数量 " + index;
                    btn_check.Text = "查询";

                }));


            }, cancellationToken.Token);



        }

        private void ToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.Filter = "文本文件(*.txt)|*.txt";
            saveFile.FileName = "rank-" + DateTime.Now.ToString("yyyyMMdd-HHmmss") + ".txt";
            saveFile.Title = "导出文件";
            saveFile.CheckPathExists = true;
            saveFile.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            if (saveFile.ShowDialog() == DialogResult.OK)
            {
                StringBuilder stringBuilder = new StringBuilder();
                foreach (ListViewItem item in listView1.Items)
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

        private void ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count < 1)
            {
                MessageBox.Show("当前没有选中内容", "提示");
                return;
            }

            //复制选中
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.Filter = "文本文件(*.txt)|*.txt";
            saveFile.FileName = "rank-" + DateTime.Now.ToString("yyyyMMdd-HHmmss") + ".txt";
            saveFile.Title = "导出文件";
            saveFile.CheckPathExists = true;
            saveFile.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            if (saveFile.ShowDialog() == DialogResult.OK)
            {
                StringBuilder stringBuilder = new StringBuilder();
                foreach (ListViewItem item in listView1.SelectedItems)
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
    }
}
