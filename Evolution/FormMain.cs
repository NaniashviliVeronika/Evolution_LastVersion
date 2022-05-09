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
        private Graphics graphics;//класс чтоб мы могли рисовать
        private int resolution;//класс для разрешения игры
        private GameEngine gameEngine;

        public FormMain()
        {
            InitializeComponent();
        }

        private void StartGame()
        {
            if (timer.Enabled)//пока таймкер тикает то  мы не должны иметь возможность создать новую игру
                return;//если таймер включен выходим из этого метода

            nudResolution.Enabled = false;//пока играет работает мы не должны менять разрешение и плотность
            nudDensity.Enabled = false;
            resolution = (int)nudResolution.Value;//при нажажатии на кнопку  Старт мы будем присваивать значение nudResolution(Разрешение);

            gameEngine = new GameEngine
             (
                   rows: pictureBox.Height / resolution,//рассчитываем количество строк и колонок для массива чтобы создть игровое поле(: присваивание)
                   cols: pictureBox.Width / resolution,
                   density:(int)(nudDensity.Minimum) + (int)nudDensity.Maximum - (int)nudDensity.Value//чем больше плотность населения,тем больше клеток
             );
           
            Text = $"Поколение { gameEngine.CurrentGeneration}";

            pictureBox.Image = new Bitmap(pictureBox.Width, pictureBox.Height);//создаем картинку размер кот.соответсвует размеру pictureBox
            graphics = Graphics.FromImage(pictureBox.Image);//создаем пременную кот. мы создали выше и для этого обращаемся к классу Graphics.Теперь мы можем отрисовывть фигуры
            timer.Start();
        }

        private void DrawNextGeneration()// отрисовка след. поколения
        {
            graphics.Clear(Color.Black);//каждый раз при генреции нового поколения,очищаем игровое поле
            var field = gameEngine.GetCurrentGeneration();//обращаемся к gameEngine и просим  у него информацию о текущем поколении

            for (int x = 0; x < field.GetLength(0); x++)
            {
                for (int y = 0; y < field.GetLength(1); y++)
                {
                    if (field[x, y])
                        graphics.FillRectangle(Brushes.Violet, x * resolution, y * resolution, resolution - 1, resolution - 1);//рисуем квадртики(координаты умножили на разрешение чтобы сдвигать квадратки по координатам)ширина,высота;отнимаем от resolution-1(каждая клетка при отрисовке будет на 1 пиксель меньше)
                }
            }
            pictureBox.Refresh(); //чтобы перерисовалось игровое поле
            Text = $"Поколение { gameEngine.CurrentGeneration}";
            gameEngine.NextGeneration();//просим gameEngine сгенерировать след. поколение
        }
        
        private void StopGame()
        {
            if (!timer.Enabled)//если таймер не включен
                return;//то выходим из этого метода
            timer.Stop();//если таймер включен то мы его остановим
            nudResolution.Enabled = true;//когда мы останавливаем игру,то мы имеем право включить плонть населения и разрешение
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
            if (e.Button == MouseButtons.Left)//если мы держим левую кнопку мыши,то создаем клетки
            {
                var x = e.Location.X / resolution;//находим координаты x,y для нашего массива
                var y = e.Location.Y / resolution;
                gameEngine.AddCell(x, y);
            }
            if (e.Button == MouseButtons.Right)//если мы держим правую кнопку мыши,то удаляем клетки
            {
                var x = e.Location.X / resolution;
                var y = e.Location.Y / resolution;
                gameEngine.RemoveCell(x, y);
                
            }
        }
    }
}
