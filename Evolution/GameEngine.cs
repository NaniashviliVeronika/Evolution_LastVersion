using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evolution
{
    public class GameEngine
    {
        public uint CurrentGeneration { get; private set; } //подсчитываем поколения (uint потому,что отрицательных чисел здесь не будет) (get считвает свойство,set присваивает свойсву новое значение)(private set помогает вносить измения в поле только внутри класса GameEngine) 
        private bool[,] field;//для хранения информации(ж.клетка или м.,есть ли у нее соседи или нету)
        private readonly int rows;//количество строк и колонок для массива(readonly позволяет причвоить значения в конструкторе,но затем эти значения мы изменить не можем,как const)
        private readonly int cols;

        public GameEngine(int rows,int cols,int density)//строк и колонки,определяющие размер игрвого поля и размер клеток
        {
            this.rows = rows;
            this.cols = cols;
            field = new bool[cols, rows];//выделить для массива(игрового поля) место в памяти
            Random rnd = new Random();//для генрации клеток по полю
            for (int x = 0; x < cols; x++)//создаем первое поколение клеток для этого перебираем все элементы массива
            {
                for (int y = 0; y < rows; y++)
                {
                    field[x, y] = rnd.Next(density) == 0;//генератор случайных чисел будет генерировать числа  от 0 до параметра, который придет в density
                }
            }
        }

        public void NextGeneration()//выполняем рассчет след. поколения 
        {
            var newfield = new bool[cols, rows];//для хранения новых данных,поскольку мы не можем изменять данные в старом массиве
            for (int x = 0; x < cols; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    var NeighboursCount = CountNeighbours(x, y);//подсчитываем соседей
                    var hasLife = field[x, y];//помещаем информацию есть ли какая-то живая клетка по текущим координатам
                    if (!hasLife && NeighboursCount == 3)//если клетка не живая и у нее 3 соседа то здесь может зародиться жизнь
                        newfield[x, y] = true;
                    else if (hasLife && NeighboursCount < 2 || NeighboursCount > 3)//если клетка не живая и количество соседей меньше 2 и больше 3,то клетка погибает
                        newfield[x, y] = false;
                    else
                        newfield[x, y] = field[x, y];//то что было в текущей клетке у нас будет и в следующем поколении
                }
            }
            field = newfield;
            CurrentGeneration++;//после расссчета след. поколения должен выпсолняться подсчет поколения
        }

        public bool[,] GetCurrentGeneration()//получать рассчет следующего поколения
        {
            var result = new bool[cols, rows];//создаем копию массива field
            for (int x = 0; x < cols; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    result[x, y] = field[x, y];//из старого массива копируем данные в новый массив
                }
            }
                return result;
        }
        private int CountNeighbours(int x, int y)//метод подсчета соседей(записываем параметры клетки чьих соседей хотим подсчитать)
        {
            int count = 0;
            for (int i = -1; i < 2; i++)//пробегаемся по всем соседям теущей клетки
            {
                for (int j = -1; j < 2; j++)
                {
                    var col = (x + i + cols) % cols;//это позволит нам взглянуть за край прямоугольника (если у нас крайняя леваая клетка, то мы сможем взгялнуть на ее соседа с првой стороны прямоугольника)
                    var row = (y + j + rows) % rows;

                    var itself = col == x && row == y;//если обращаемся к координатам текущей клетки,то нам нельзя ее считать
                    var hasLife = field[col, row];//подсчитваем живых соседей

                    if (hasLife && !itself)//если в данной клтке есть жизнь и это не самопроверка
                        count++;//нашли живого соседа
                }
            }
            return count;
        }
        private bool CellPosition(int x, int y)//выход за границы pixturebox
        {
            return x >= 0 && y >= 0 && x < cols && y < rows;
        }

        private void UpdateCell(int x, int y, bool state)//принимает координты клетки чье состояние мы хотим обновить(bool потому что массив field тоже такой)
        {
            if (CellPosition(x, y))
                field[x, y] = state;//таким обазом мы можем лио добавить,либо удалить клетку
        }

        public void AddCell(int x, int y)
        {
            UpdateCell(x, y, state: true);
        }

        public void RemoveCell(int x, int y)
        {
            UpdateCell(x, y, state: false);
        }
    }
}
