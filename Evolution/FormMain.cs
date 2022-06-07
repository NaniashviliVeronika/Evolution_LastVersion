using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Evolution
{
    public partial class FormMain : Form
    {
        private Graphics graphics;
        private int resolution;
        private GameEngine gameEngine;

        public FormMain()
        {
            InitializeComponent();
        }

        private void StartGame()
        {
            if (timer.Enabled)
                return;

            nudResolution.Enabled = false;
            nudDensity.Enabled = false;
            resolution = (int)nudResolution.Value;

            gameEngine = new GameEngine
             (
                   rows: pictureBox.Height / resolution,
                   cols: pictureBox.Width / resolution,
                   density:(int)(nudDensity.Minimum) + (int)nudDensity.Maximum - (int)nudDensity.Value
             );
           
            Text = $"Поколение { gameEngine.CurrentGeneration}";

            pictureBox.Image = new Bitmap(pictureBox.Width, pictureBox.Height);
            graphics = Graphics.FromImage(pictureBox.Image);
            timer.Start();
        }

        private void DrawNextGeneration()
        {
            graphics.Clear(Color.Black);
            var field = gameEngine.GetCurrentGeneration();

            for (int x = 0; x < field.GetLength(0); x++)
            {
                for (int y = 0; y < field.GetLength(1); y++)
                {
                    if (field[x, y])
                        graphics.FillRectangle(Brushes.Violet, x * resolution, y * resolution, resolution - 1, resolution - 1);
                }
            }
            pictureBox.Refresh(); 
            Text = $"Поколение { gameEngine.CurrentGeneration}";
            gameEngine.NextGeneration();
        }
        
        private void StopGame()
        {
            if (!timer.Enabled)
                return;
            timer.Stop();
            nudResolution.Enabled = true;
            nudDensity.Enabled = true;
        }
        private void timer_Tick(object sender, EventArgs e)
        {
            DrawNextGeneration();
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            StartGame();
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            StopGame();
        }

        private void pictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (!timer.Enabled)
                return;
            if (e.Button == MouseButtons.Left)
            {
                var x = e.Location.X / resolution;
                var y = e.Location.Y / resolution;
                gameEngine.AddCell(x, y);
            }
            if (e.Button == MouseButtons.Right)
            {
                var x = e.Location.X / resolution;
                var y = e.Location.Y / resolution;
                gameEngine.RemoveCell(x, y);
                
            }
        }
    }
}
