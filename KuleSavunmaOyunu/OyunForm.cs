using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KuleSavunmaOyunu
{
    public partial class OyunForm : Form
    {
        private const int TILE_SIZE = 50;
        private const int GRID_WIDTH = 12;
        private const int GRID_HEIGHT = 10;

        private int[,] mapGrid = new int[,]
        {
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
            {1, 1, 1, 0, 1, 1, 1, 1, 1, 1, 1, 0},
            {0, 0, 1, 0, 1, 0, 0, 0, 0, 0, 1, 0},
            {0, 0, 1, 0, 1, 0, 0, 1, 1, 1, 1, 0},
            {0, 0, 1, 0, 1, 0, 0, 1, 0, 0, 0, 0},
            {0, 0, 1, 0, 1, 0, 0, 1, 0, 0, 0, 0},
            {0, 0, 1, 1, 1, 0, 0, 1, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0}
        };
         private PictureBox[,] visualGrid = new PictureBox[GRID_HEIGHT, GRID_WIDTH];
        public OyunForm()
        {
            InitializeComponent();
            InitializeMap();
        }

        private void InitializeMap()
        {

            for (int y = 0; y < GRID_HEIGHT; y++)
            {
                for (int x = 0; x < GRID_WIDTH; x++)
                {
                    PictureBox tile = new PictureBox();
                    tile.Size = new Size(TILE_SIZE, TILE_SIZE);
                    tile.Location = new Point(x * TILE_SIZE, y * TILE_SIZE);

                    tile.SizeMode = PictureBoxSizeMode.StretchImage;

                    if (mapGrid[y, x] == 1)
                    {
                        // YOL İÇİN RESİM (sand_tile)
                        tile.Image = Properties.Resources.sand_tile;
                        tile.Tag = "Path";
                    }
                    else
                    {
                        // ÇİM İÇİN RESİM (grass_tile_2)
                        tile.Image = Properties.Resources.grass_tile_2;
                        tile.Tag = "Placement";
                    }
                    panel1.Controls.Add(tile);
                }
            }
        }

        private void OyunForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
