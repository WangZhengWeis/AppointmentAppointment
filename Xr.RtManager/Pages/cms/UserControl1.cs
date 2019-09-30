using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Xr.RtManager.Pages.cms
{
    public partial class UserControl1 : UserControl
    {
        public UserControl1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //List<Item> itemList = new List<Item>();
            //Item item = new Item();
            //item.name = "测试12222222222";
            //item.value = "0";
            //itemList.Add(item);
            //item = new Item();
            //item.name = "测试2";
            //item.value = "2";
            //itemList.Add(item);
            //item = new Item();
            //item.name = "测试测试测试测试";
            //item.value = "3";
            //itemList.Add(item);
            //menuControl1.setDataSource(itemList);
        }

        private void menuControl1_MenuItemClick(object sender, EventArgs e)
        {
            Label itemlabel = (Label)sender;
            MessageBox.Show(itemlabel.Name);
        }
    }
}
