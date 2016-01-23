using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CaptchaRecogition
{
    public partial class ZimoList : Form
    {
        public ZimoList()
        {
            InitializeComponent();
        }
        public ZimoList(string path)
        {
            InitializeComponent();
            zimoPath = path;
        
        }

        private string zimoPath;
        private string[] zimoData;
        private void LoadList()
        {
            if (zimoPath != null)
            {
                list_Zimo.Items.Clear();
                zimoData = File.ReadAllLines(zimoPath);
                list_Zimo.Items.AddRange(zimoData);
            }
        }

 
        //删除已经添加的字模
        private void list_delete_Click_1(object sender, EventArgs e)
        {
            if (list_Zimo.SelectedItem != null)
            {
                
                var list = zimoData.ToList();
                list.RemoveAt(list_Zimo.SelectedIndex);
               zimoData = list.ToArray();
               list_Zimo.Items.Remove(list_Zimo.SelectedItem);
                //todo 写入字模文本
            }
            else
            {
                MessageBox.Show("你没有选择要删除的字模数据");
            }
        }

        private void list_edit_Click_1(object sender, EventArgs e)
        {
            if (list_Zimo.SelectedItem != null)
            {

                groupBox_edit.Visible = true;

                //TODO 写入字模文本
            }
            else
            {
                MessageBox.Show("你没有选择要编辑的字模数据");
            }
        }
       
        private void btn_Cancel_Click_1(object sender, EventArgs e)
        {
            groupBox_edit.Visible = false;
        }
        //确认修改
        private void btn_OK_Click(object sender, EventArgs e)
        {
            string[] zimo = list_Zimo.SelectedItem.ToString().Split(new string[] { "--" }, StringSplitOptions.None);
            string value = txt_listedit.Text + "--" + zimo[1];
            int index = list_Zimo.SelectedIndex;
            list_Zimo.Items.RemoveAt(index);
            list_Zimo.Items.Insert(index, value);
            zimoData[index] = value;
            txt_listedit.Text = "";
            groupBox_edit.Visible = false;
        }
        private void WriteToZimoTxt()
        {
            if (zimoData != null && zimoPath != null)
            {
                File.WriteAllLines(zimoPath, zimoData);
            }

        }

        private void ZimoList_FormClosed(object sender, FormClosedEventArgs e)
        {
            WriteToZimoTxt();
        }

        private void ZimoList_Load(object sender, EventArgs e)
        {
            LoadList();
        }

        private void btn_Close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

      


 

    }
}
