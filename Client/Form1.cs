﻿using RestSharp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.Text += " V" + this.ProductVersion;

            tabControl1.Dock = DockStyle.Fill;

            listView1.View = View.Details;
            listView1.GridLines = true;
            listView1.FullRowSelect = true;

            listView1.Columns.Add("#", 50, HorizontalAlignment.Left);
            listView1.Columns.Add("网址", 150, HorizontalAlignment.Left);
            listView1.Columns.Add("关键词", 120, HorizontalAlignment.Left);
            listView1.Columns.Add("类别", 75, HorizontalAlignment.Left);
            listView1.Columns.Add("排名", 75, HorizontalAlignment.Left);

            listView1.ColumnClick += ListView1_ColumnClick;


            tbx_word.TextChanged += (s, e) =>
                  {
                      label1.Text = tbx_word.Lines.Length.ToString();
                  };

            tbx_result.TextChanged += (s, e) =>
            {

                label2.Text = tbx_result.Lines.Length.ToString();
            };


            comboBox1.SelectedIndex = 0;

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
                    if (col == 4||col==0)
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

        private void btn_check_Click(object sender, EventArgs e)
        {

            var lines = tbx_word.Lines;

            var words = new List<Models.SearchResult>();
            foreach (var item in lines)
            {
                if (string.IsNullOrEmpty(item) || string.IsNullOrWhiteSpace(item))
                    continue;

                var sp = item.Split(new[] { '\t', ' ' });
                if (sp.Length > 3)
                {
                    words.Add(new Models.SearchResult()
                    {
                        SearchNodes = new List<Models.SearchNode>(),
                        Host = sp[0],
                        Word = sp[1],
                        Device = sp[2].ToLower()

                    });
                }
            }
            if (words.Count < 1)
            {
                return;
            }

            var client = new RestClient("https://www.baidu.com");
            client.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/76.0.3809.100 Safari/537.36";
            client.FollowRedirects = false;

            var clientm = new RestClient("https://m.baidu.com");
            clientm.UserAgent = "Mozilla/5.0 (iPhone; CPU iPhone OS 11_0 like Mac OS X) AppleWebKit/604.1.38 (KHTML, like Gecko) Version/11.0 Mobile/15A372 Safari/604.1";

            words = words.OrderByDescending(m => m.Host).ToList();
            progressBar1.Maximum = words.Count;
            progressBar1.Value = 0;
            progressBar1.Step = 1;

            progressBar2.Maximum = words.Count;
            progressBar2.Value = 0;
            progressBar2.Step = 1;

            var maxpage = comboBox1.SelectedIndex;
            btn_check.Enabled = false;
            tbx_result.Text = "";
            Task.Run(() =>
            {

                var task1 = words.AsParallel().Select(item =>
                  {

                      return Task.Run(() =>
                      {
                          var htmldoc = new HtmlAgilityPack.HtmlDocument();
                          if (item.Device == "pc")
                          {
                              var request = new RestRequest();
                              request.Resource = $"/s?ie=utf-8&wd={System.Web.HttpUtility.UrlEncode(item.Word)}";
                              var resp = client.Get(request);

                              if (resp.IsSuccessful)
                              {
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
                          }
                          else
                          {
                              var requestm = new RestRequest();
                              requestm.Resource = $"/s?ie=utf-8&wd={System.Web.HttpUtility.UrlEncode(item.Word)}";
                              var respm = clientm.Get(requestm);
                              if (respm.IsSuccessful)
                              {
                                  htmldoc.LoadHtml(respm.Content);
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
                                          requestm.Resource = System.Web.HttpUtility.HtmlDecode(nextpage.GetAttributeValue("href", "").Replace("https://m.baidu.com/", ""));
                                          respm = clientm.Get(requestm);
                                          htmldoc.LoadHtml(respm.Content);
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
                          }

                          progressBar1.BeginInvoke(new MethodInvoker(() =>
                          {
                              progressBar1.PerformStep();
                          }));
                      });

                  });

                Task.WaitAll(task1.ToArray());


                words.ForEach(item =>
                {
                    if (item.SearchNodes != null)
                    {
                        if (item.Device == "pc")
                        {
                            var tasks = item.SearchNodes.AsParallel().Select(m =>
                            {
                                return Task.Run(() =>
                                {
                                    var request2 = new RestRequest();
                                    request2.Resource = m.LinkUrl.Replace("http://www.baidu.com/", "");
                                    var resp2 = client.Head(request2);
                                    if (resp2.StatusCode == System.Net.HttpStatusCode.Found)
                                    {
                                        var l = resp2.Headers.FirstOrDefault(f => f.Name == "Location");
                                        if (l != null)
                                        {
                                            m.Url = new Uri(l.Value.ToString());
                                            Console.WriteLine(m.Url);
                                        }
                                    }
                                });
                            });

                            Task.WaitAll(tasks.ToArray());
                        }
                        else
                        {
                            item.SearchNodes.ForEach(m => m.Url = new Uri(m.LinkUrl));
                        }
                    }
                    progressBar2.BeginInvoke(new MethodInvoker(() =>
                    {
                        progressBar2.PerformStep();
                    }));
                    tbx_result.BeginInvoke(new MethodInvoker(() =>
                    {
                        tbx_result.AppendText(item.ToString() + "\r\n");


                        var lvi = new ListViewItem(listView1.Items.Count.ToString());
                        lvi.SubItems.Add(item.Host);
                        lvi.SubItems.Add(item.Word);
                        lvi.SubItems.Add(item.Device);
                        lvi.SubItems.Add(item.Rank);
                        lvi.UseItemStyleForSubItems = false;
                        listView1.Items.Add(lvi);
                        listView1.EndUpdate();

                    }));
                });
                var file = File.AppendText($"wd-{DateTime.Now.ToString("yyyyMMdd")}.txt");
                file.WriteLine($"=========={DateTime.Now.ToString("yyyy-MM-dd HH:mm")}==========");
                file.WriteLine(tbx_result.Text);
                file.Flush();
                file.Close();

                btn_check.BeginInvoke(new MethodInvoker(() =>
                {
                    btn_check.Enabled = true;
                }));


            });



        }

    }
}
