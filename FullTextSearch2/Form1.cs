using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FullTextSearch2
{
    public partial class Form1 : Form
    {

        /*
            + 가로 스크롤 적용
            + 직접 셀렉하면 색깔을 다르게 하고 Delete하면 해당 목록 지우기
            + 많은 데이터를 검색할 때 스피너 적용
            + 
         */
        string[] files = null;

        public Form1()
        {
            InitializeComponent();

            listBox1.AllowDrop = true;
            listBox1.DragDrop += listBoxFiles_DragDrop;
            listBox1.DragEnter += listBoxFiles_DragEnter;
            listBox1.DrawMode = DrawMode.OwnerDrawFixed;
            listBox1.DrawItem += new DrawItemEventHandler(listBox1_DrawItem);
            listBox1.HorizontalScrollbar = true;
            textBox1.KeyDown += textBox1_KeyDown;

        }

        private void listBoxFiles_DragEnter(object sender, DragEventArgs e)
        {
            Debug.WriteLine("listBoxFiles_DragEnter");
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;

        }

        private void listBoxFiles_DragDrop(object sender, DragEventArgs e)
        {
            Debug.WriteLine("listBoxFiles_DragDrop");
            files = (string[])e.Data.GetData(DataFormats.FileDrop);
            foreach (string file in files)
            {
                listBox1.Items.Add(file);
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            listBox1.SelectionMode = SelectionMode.MultiExtended; // 다중 선택 모드
            string result = "";
            string word = textBox1.Text;
            List<int> indexList = new List<int>();

            // selected 초기화
            listBox1.SelectedIndex = -1;

            foreach (var input_items in listBox1.Items)
            {
                // 파일 경로 담기
                result += string.Format("{0} ", input_items);
                string textValue = System.IO.File.ReadAllText(input_items.ToString());
                Debug.WriteLine(textValue);
                // 해당하는 인덱스 담기 
                bool isContains = textValue.IndexOf(word, StringComparison.OrdinalIgnoreCase) >= 0;
                if (isContains)
                {
                    Debug.WriteLine("글자가 포함되어 있습니다.");
                    indexList.Add(listBox1.Items.IndexOf(input_items));
                }

            }
            Debug.WriteLine(indexList);
            //해당하는 데이터 selected 하기
            foreach (var index in indexList)
            {
                listBox1.SetSelected(index, true);
            }

            Debug.WriteLine("button1_Click(list): " + result);
            Debug.WriteLine("button1_Click(word): " + word);
        }

        private void listBox1_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();

            bool isItemSelected = ((e.State & DrawItemState.Selected) == DrawItemState.Selected);
            int itemIndex = e.Index;
            if (itemIndex >= 0 && itemIndex < listBox1.Items.Count)
            {
                Graphics g = e.Graphics;

                // Background Color
                SolidBrush backgroundColorBrush = new SolidBrush((isItemSelected) ? Color.Red : Color.White);
                g.FillRectangle(backgroundColorBrush, e.Bounds);

                // Set text color
                string itemText = listBox1.Items[itemIndex].ToString();

                SolidBrush itemTextColorBrush = (isItemSelected) ? new SolidBrush(Color.White) : new SolidBrush(Color.Black);
                g.DrawString(itemText, e.Font, itemTextColorBrush, listBox1.GetItemRectangle(itemIndex).Location);

                // Clean up
                backgroundColorBrush.Dispose();
                itemTextColorBrush.Dispose();
            }

            e.DrawFocusRectangle();
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button1_Click(sender, e);
            }
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {

        }


        private void DisplayHScroll()
        {
            // 부분적으로 표시되는 항목이 없는지 확인합니다.
            listBox1.IntegralHeight = true;

            // ListBox에 너비가 넓은 항목을 추가합니다.
            for (int x = 0; x < 10; x++)
            {
                listBox1.Items.Add("Item  " + x.ToString() + " is a very large value that requires scroll bars");
            }

            // 가로 스크롤 막대를 표시합니다.
            listBox1.HorizontalScrollbar = true;

            // ListBox에서 가장 큰 항목의 크기를 결정할 때 사용할 Graphics 개체를 만듭니다.
            Graphics g = listBox1.CreateGraphics();

            // 목록의 마지막 항목을 사용하여 MeasureString 메서드를 사용하여 HorizontalExtent의 크기를 결정합니다.
            int hzSize = (int)g.MeasureString(listBox1.Items[listBox1.Items.Count - 1].ToString(), listBox1.Font).Width;
            // HorizontalExtent 속성을 설정합니다.
            listBox1.HorizontalExtent = hzSize;
        }


    }
}
