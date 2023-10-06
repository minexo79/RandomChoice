using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RandomChoice.Misc
{
    public class FileOperator
    {
        public string FilePath = @"C:\RandomChoice\";
        public string FileName = "choice.txt";

        public FileOperator()
        {
            InitialFileCheck();
        }

        public void InitialFileCheck()
        {
            // 檢查C槽內有無txt記錄檔，若無則新增檔案
            if (!Directory.Exists(FilePath))
                Directory.CreateDirectory(FilePath);

            // 開啟該文件檔
            using (var stream = OpenFile(FilePath + FileName))
            {
                if (stream.Length == 0)
                    WriteFile(stream, App.defaultChoice); // 如果是新建的檔案，則會寫入預設資料
                else
                    App.choice = ReadFile(stream); // 若內部已有資料，則讀取並儲存至陣列

                // 關閉該文件檔
                CloseFile(stream);
            }
        }

        public FileStream OpenFile(string path, FileMode mode = FileMode.OpenOrCreate)
        {
            return File.Open(path, mode);
        }

        public string[]? ReadFile(FileStream stream)
        {
            string[] result = null;

            if (stream.CanRead)
            {
                // 開啟StreamReader
                using (var reader = new StreamReader(stream))
                {
                    // 讀取檔案內容，使用CRLF分隔
                    result = reader.ReadToEnd().Split("\r\n");
                    // 去掉最後一個CRLF所產生的空行
                    result = result.Where(val => val != "").ToArray();
                    // 關閉StreamReader
                    reader.Close();
                }
            }

            return result;
        }

        public string[]? WriteFile(FileStream stream, string[] _element)
        {
            string[] result = null;

            if (stream.CanWrite)
            {
                // 關閉StreamWriter
                using (var writer = new StreamWriter(stream))
                {
                    // Flush 掉 Txt 的檔案
                    writer.Flush();

                    // 將所有元素寫入到文字檔內
                    foreach (string item in _element)
                        writer.Write(item + "\r\n");

                    // 關閉StreamWriter
                    writer.Close();
                }
            }

            return result;
        }

        public void CloseFile(FileStream stream)
        {
            stream.Close();
        }
    }
}
