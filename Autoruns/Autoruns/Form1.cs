using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Autoruns
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            InitListView();
            UpdateListView();
        }

        private ImageList IconList;
        private const int pad = 5;
        private Startups autoruns = new Startups();

        private void InitListView()
        {
            listView1.View = View.Details;
            listView1.AllowColumnReorder = false;
            listView1.FullRowSelect = true;
            listView1.HeaderStyle = ColumnHeaderStyle.Nonclickable;
            
            // In order to make the Reg Keys and Startup Folders stand out across 
            // multipule columns, we need to draw it by ourselves instead of using
            // the default draw funcions.
            listView1.OwnerDraw = true;
            listView1.DrawItem += 
                new DrawListViewItemEventHandler(listView1_DrawItem);
            listView1.DrawSubItem += 
                new DrawListViewSubItemEventHandler(listView1_DrawSubItem);
            listView1.DrawColumnHeader += 
                new DrawListViewColumnHeaderEventHandler(listView1_DrawColumnHeader);

            // Add five header columns
            listView1.Columns.Add("Autorun Entry", 320, HorizontalAlignment.Left);
            listView1.Columns.Add("Description", 160, HorizontalAlignment.Left);
            listView1.Columns.Add("Publisher", 240, HorizontalAlignment.Left);
            listView1.Columns.Add("Image Path", 480, HorizontalAlignment.Left);
            listView1.Columns.Add("Timestamp", 140, HorizontalAlignment.Left);

            // Create an image list and add an image.
            IconList = new ImageList();
            IconList.Images.Add(Autoruns.Properties.Resources.Icon_registry);
            IconList.Images.Add(Autoruns.Properties.Resources.Icon_folder);

            // SmallImageList must be set when using IndentCount.
            listView1.SmallImageList = IconList;

            toolStripStatusLabel1.Text = "Finished";
        }

        private void UpdateListView(bool hideEmpty = false)
        {
            listView1.BeginUpdate();
            listView1.Items.Clear();
            foreach(StartupEntry e in autoruns.starupEntrys)
            {
                if(e.IsMainEntry)
                {
                    if (!hideEmpty || hideEmpty && !e.IsEmpty)
                        AddContainerItem(e.EntryName, e.Time);
                }
                else
                {
                    AddAutorunItem(e.EntryName,
                        e.Desctiption,
                        e.Publisher,
                        e.ImagePath,
                        e.Time);
                }
            }
            listView1.EndUpdate();
        }

        private void AddContainerItem(string entry, DateTime time)
        {
            ListViewItem item = new ListViewItem(entry);

            // prepare 5 subitems to be appended to an item of listview
            item.SubItems.Add(string.Empty);
            item.SubItems.Add(string.Empty);
            item.SubItems.Add(string.Empty);
            item.SubItems.Add(time.ToString("G"));

            // no indent to mark it as a ContainerItem, instead of an AutorunItem
            item.IndentCount = 0;

            // Do actual add
            listView1.Items.Add(item);
        }

        private void AddAutorunItem(string entry, string desc, string publisher, string path, DateTime time)
        {
            ListViewItem item = new ListViewItem(entry, 1);
            
            // prepare 5 subitems to be appended to an item of listview
            item.SubItems.Add(desc);
            item.SubItems.Add(publisher);
            item.SubItems.Add(path);
            item.SubItems.Add(time.ToString("G"));
            
            // indent to mark it as a AutorunItem, instead of a ContainerItem
            item.IndentCount = 1;

            SetIconList(path);
            item.ImageKey = path;

            // Do actual add
            listView1.Items.Add(item);
        }

        private void SetIconList(string path)
        {
            FileInfo file = new FileInfo(path);

            // Check to see if the image collection contains an image
            // for this extension, using the extension as a key.
            if (!IconList.Images.ContainsKey(file.FullName))
            {
                // If not, add the image to the image list.
                Icon iconForFile = SystemIcons.WinLogo;
                if (System.IO.File.Exists(file.FullName))
                {
                    iconForFile = System.Drawing.Icon.ExtractAssociatedIcon(file.FullName);
                }
                IconList.Images.Add(path, iconForFile);
            }
        }


        private void hideMicrosoftEntrysToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        // Draws the backgrounds for entire ListView items.
        private void listView1_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            if (e.Item.IndentCount != 0)
                e.DrawDefault = true;
            else
            {
                e.Item.BackColor = Color.FromArgb(208, 208, 255);
                e.DrawBackground();
            }
        }

        // Draws subitem text and applies content-based formatting.
        private void listView1_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {   
            if (e.Item.IndentCount == 0)
            {
                ListView listView = (ListView)sender;

                if (e.ColumnIndex == 0)
                {
                    Image icon;
                    if (e.Item.SubItems[0].Text.Contains("HK"))
                        icon = this.IconList.Images[0];
                    else
                        icon = this.IconList.Images[1];
                    
                    Rectangle rect = new Rectangle();
                    rect.X = pad + e.Bounds.X;
                    rect.Y += e.Bounds.Y;
                    rect.Width = icon.Width;
                    rect.Height = e.Bounds.Height;
                    e.Graphics.DrawImage(icon, rect);
                

                    Rectangle rowBoundsL = e.Item.SubItems[0].Bounds;
                    Rectangle rowBoundsR = e.Item.SubItems[4].Bounds;
                    Rectangle bounds = new Rectangle(rowBoundsL.Left + rect.Width + 2 * pad,
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

        private void hideEmptyLocationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (hideEmptyLocationToolStripMenuItem.Checked == true)
            {
                hideEmptyLocationToolStripMenuItem.Checked = false;
                UpdateListView(false);
            }
            else
            {
                hideEmptyLocationToolStripMenuItem.Checked = true;
                UpdateListView(true);
            }
        }
    }
}
