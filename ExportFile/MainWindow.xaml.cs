using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ExportFile
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnExport_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            string _sourcePath = TBPath.Text;
            string _targetPath = TBPath2.Text;
            string[] _exts = TBFileExtension.Text.Replace(" ", "").Split(',');

            string source_path = string.Format(@"{0}", _sourcePath); //@"C:\Users\user\Desktop\old";
            string target_path = string.Format(@"{0}", _targetPath); //@"C:\Users\user\Desktop\new";

            bool bToday = (bool)RBToday.IsChecked;

            List<string> fileList = new List<string>();


            try
            {
                DirectoryInfo di = new DirectoryInfo(source_path);

                foreach (string ext in _exts)
                {
                    foreach (FileInfo fi in di.GetFiles("*."+ext))//($".{ext}"))
                    {
                        string sFile = System.IO.Path.ChangeExtension(fi.Name, $".{ext}");
                        string sourceFile = System.IO.Path.Combine(source_path, sFile); // 현 경로
                        string targetFile = System.IO.Path.Combine(target_path, sFile); // 복사 경로
                        try
                        {
                            //오늘 수정된 파일만 복사
                            if (bToday)
                            {
                                // 파일 수정 날짜
                                string sModiDate = File.GetLastWriteTime(sourceFile).Date.ToString("yyyyMMdd");
                                string dateTime = DateTime.Today.ToString("yyyyMMdd");//DateTime.Now

                                if (sModiDate.Equals(dateTime))//날짜 같으면 복사
                                {
                                    try
                                    {
                                        // 파일 복사
                                        System.IO.File.Copy(sourceFile, targetFile, true);//true : 덮어쓰기
                                    }
                                    catch (Exception ex)
                                    {
                                        //내용,제목
                                        //MessageBox.Show(ex.Message.ToString() + "\t\n: " + ex.StackTrace.ToString(), "Exception (3-1)");
                                        string msg = string.Format("Exception (3-1) - {0} \t\n {1} \t\n", ex.Message.ToString(), ex.StackTrace.ToString());
                                        AddLog(msg);
                                    }
                                    finally
                                    {
                                        fileList.Add(sFile);

                                        AddLog(sFile);
                                    }
                                }
                               
                            }
                            else//전체 파일 복사
                            {
                                try
                                {
                                    // 파일 복사
                                    System.IO.File.Copy(sourceFile, targetFile, true);//true : 덮어쓰기
                                }
                                catch(Exception ex)
                                {
                                    //내용,제목
                                    //MessageBox.Show(ex.Message.ToString() + "\t\n: " + ex.StackTrace.ToString(), "Exception (3-2)");
                                    string msg = string.Format("Exception (3-2) - {0} \t\n {1} \t\n", ex.Message.ToString(), ex.StackTrace.ToString());
                                    AddLog(msg);
                                }
                                finally
                                {
                                    fileList.Add(sFile);

                                    AddLog(sFile);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            //내용,제목
                            //MessageBox.Show(ex.Message.ToString() + "\t\n: " + ex.StackTrace.ToString(), "Exception (2)");
                            string msg = string.Format("Exception (2) - {0} \t\n {1} \t\n",ex.Message.ToString() , ex.StackTrace.ToString());

                            AddLog(msg);
                        }
                        //finally
                        //{
                        //    fileList.Add(sFile);
                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                //내용,제목
                //MessageBox.Show(ex.Message.ToString() + "\t\n: " + ex.StackTrace.ToString(), "Exception (1)");
                string msg = string.Format("Exception (1) - {0} \t\n {1} \t\n", ex.Message.ToString(), ex.StackTrace.ToString());
                AddLog(msg);
            }
            finally
            {
                string files = string.Join("\t\n", fileList);
                //내용,제목
                string msg = $"{target_path} ({fileList.Count}개)";

                AddLog(msg);

                //파일 목록
                //if (!string.IsNullOrEmpty(files))
                //{
                //    msg += "\t\n" + files;
                //}

                //MessageBox.Show(msg, "파일 복사 완료");


                string strFolderPath = TBPath2.Text;
                System.Diagnostics.Process.Start(@strFolderPath);
            }

        }


        public void AddLog(string msg)
        {
            TextRange range = new TextRange(TBLog.Document.ContentStart, TBLog.Document.ContentEnd);

            string totMsg = msg + Environment.NewLine + range.Text;

            FlowDocument fd = new FlowDocument();
            fd.Blocks.Add(new Paragraph(new Run(totMsg)));
            TBLog.Document = fd;
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            TextBox item = ((sender as Border).Child as TextBox);

            item.Focus();

            item.SelectionStart = item.Text.Length;
            item.SelectionLength = 0;

            if (e.ClickCount == 2)
            {
                item.SelectAll();
            }
            //((TextBox)(sender as Border).Child))
        }
    }
}
