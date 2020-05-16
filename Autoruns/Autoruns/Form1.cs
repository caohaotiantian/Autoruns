using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Autoruns
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            InitListView();
        }

        private void InitListView()
        {
            listView1.View = View.Details;
            listView1.AllowColumnReorder = false;
            listView1.FullRowSelect = true;
            listView1.HeaderStyle = ColumnHeaderStyle.Nonclickable;
            
            // redraw
            listView1.OwnerDraw = true;
            listView1.DrawItem += new DrawListViewItemEventHandler(listView1_DrawItem);
            listView1.DrawSubItem += new DrawListViewSubItemEventHandler(listView1_DrawSubItem);
            listView1.DrawColumnHeader += new DrawListViewColumnHeaderEventHandler(listView1_DrawColumnHeader);

            // add five columns
            listView1.Columns.Add("Autorun Entry", 160, HorizontalAlignment.Left);
            listView1.Columns.Add("Description", -2, HorizontalAlignment.Left);
            listView1.Columns.Add("Publisher", -2, HorizontalAlignment.Left);
            listView1.Columns.Add("Image Path", -2, HorizontalAlignment.Left);
            listView1.Columns.Add("Timestamp", 160, HorizontalAlignment.Left);

            ListViewItem item1 = new ListViewItem("11111111111111111111111111111111111111111111111111111111111111111111111", 0);
            item1.SubItems.Add("");
            item1.SubItems.Add("");
            item1.SubItems.Add("John Smith");
            item1.SubItems.Add("John Smith");
            listView1.Items.Add(item1);
            // Create an image list and add an image.
            ImageList list = new ImageList();
            list.Images.Add(new Bitmap(typeof(Button), "Button.bmp"));
            // SmallImageList must be set when using IndentCount.
            listView1.SmallImageList = list;
        }

        private void hideMicrosoftEntrysToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        // Draws the backgrounds for entire ListView items.
        private void listView1_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            if (e.Item.IndentCount != 0)
                e.DrawDefault = true;
        }

        // Draws subitem text and applies content-based formatting.
        private void listView1_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            // Check if e.Item is a Reg Key.
            if (e.Item.IndentCount == 0)
            {
                ListView listView = (ListView)sender;
                if (e.ColumnIndex == 0)
                {
                    Rectangle rowBoundsL = e.Item.SubItems[0].Bounds;
                    Rectangle rowBoundsR = e.Item.SubItems[4].Bounds;
                    Rectangle bounds = new Rectangle(rowBoundsL.Left,
                        rowBoundsL.Top,
                        rowBoundsR.Left - rowBoundsL.Left,
                        rowBoundsR.Height);
                    TextRenderer.DrawText(e.Graphics,
                        e.Item.SubItems[0].Text,
                        listView.Font,
                        bounds,
                        Color.Black,
                        TextFormatFlags.Left | TextFormatFlags.SingleLine | TextFormatFlags.GlyphOverhangPadding | TextFormatFlags.VerticalCenter | TextFormatFlags.WordEllipsis);
                }
                else if (e.ColumnIndex == 4)
                {
                    TextRenderer.DrawText(e.Graphics,
                        e.Item.SubItems[4].Text,
                        listView.Font,
                        e.Item.SubItems[4].Bounds,
                        Color.Black,
                        TextFormatFlags.Left);
                }

            }
            else
                e.DrawDefault = true;
        }

        // Draws column headers.
        private void listView1_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            e.DrawDefault = true;
        }

    }
}
