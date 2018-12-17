using System;
using System.Collections.Generic;
using System.Linq;

namespace Plane
{
    class Plane
    {
        int _speed = 0;
        int _height = 0;
        int _scores = 0;
        bool _success = false;
        bool _wasHeightMoreThanNull = false;
        int _countDeletedDispatchers = 0;
        int _maxSpd = 0;
        int _maxHght = 0;
        List<Dispatcher> dispatchers = new List<Dispatcher>();
        public delegate void DispatchersDeleg(Plane plane);
        public event DispatchersDeleg DispathersWork;

        public int Speed { get => _speed; set { _speed = value < 0 ? 0 : _height == 0 && value >= 50 ? 50 : value; Dispather(); _maxSpd = _maxSpd > _speed ? _maxSpd : _speed; } }
        public int Height { get => _height; set { _height = value < 0 ? 0 : _speed > 0 ? value : 0; Dispather(); _maxHght = _maxHght > _height ? _maxHght : _height; } }
        public bool Success { get => _success; }

        public void Flight()
        {
            Console.WriteLine("Q - Добавить диспетчера");
            Console.WriteLine("W - Удалить диспетчера\n");
            Console.WriteLine(@"/\ +250 м (+Shift +500 м)");
            Console.WriteLine(@"\/ -250 м (+Shift -500 м)");
            Console.WriteLine(@"<- -50 км/ч (+Shift -150 км/ч)");
            Console.WriteLine(@"-> +50 км/ч (+Shift +150 км/ч)");
            Console.WriteLine("\nЦель полета: набрать скорость 1000 км/ч");
            Console.WriteLine("Скорость, необходимая для взлета и посадки - 50 км/ч.");
            Console.WriteLine("Если попытаться посадить самолет на скорости, больше 50 км/ч - шасси не выдержат.");
            Console.WriteLine("Если скорость самолета упадет до нуля, самолет совершит падение.");
            Console.WriteLine("Так же, если отклониться от рекомендуемой высоты диспетчера на 1000 м - самолет разобъется.");
            Console.WriteLine("В зависимости от величины отклонения от этого показателя, будут начисляться штраф.");
            Console.WriteLine("Если у диспетчера наберется штраф 1000 и больше очков - пилота отстранят.\n");
            bool fly = true;
            do
            {
                if (dispatchers.Count >= 2)
                {
                    if (_speed >= 1000) Console.WriteLine("\nСовершайте посадку!\n");
                    Console.WriteLine($"{(_speed == 0 ? "\n" : "")}Скорость: {_speed}\nВысота: {_height}\n");
                    ConsoleKeyInfo key = Console.ReadKey(true);
                    try
                    {
                        switch (key.Key)
                        {
                            case ConsoleKey.UpArrow: Height += key.Modifiers == ConsoleModifiers.Shift ? 500 : 250; break;
                            case ConsoleKey.DownArrow: Height -= key.Modifiers == ConsoleModifiers.Shift ? 500 : 250; break;
                            case ConsoleKey.LeftArrow: Speed -= key.Modifiers == ConsoleModifiers.Shift ? 150 : 50; break;
                            case ConsoleKey.RightArrow: Speed += key.Modifiers == ConsoleModifiers.Shift ? 150 : 50; break;
                            case ConsoleKey.Q: AddDispatcher(); break;
                            case ConsoleKey.W: DeleteDispatcher(); break;
                        }
                        if (_speed >= 1000) _success = true;
                        if (_height > 0) _wasHeightMoreThanNull = true;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        fly = false;
                    }
                }
                else { AddDispatcher(); }
            } while (fly);
            if (_success)
            {
                foreach (var dispatcher in dispatchers) _scores += dispatcher.Scores;
                Console.WriteLine($"Суммарное к-во штрафных очков: {_scores} ({dispatchers.Count} активных + {_countDeletedDispatchers} удаленных диспетчеров).");
            }
            Console.WriteLine($"Максимальная скорость: {_maxSpd}\nМаксимальная высота: {_maxHght}");
        }

        private void Dispather()
        {
            if (_wasHeightMoreThanNull) DispathersWork(this);
            Console.WriteLine($"{(_height > 250 ? $"Усредненная высота диспетчеров: {dispatchers.Average(sum => sum.Hp)}" : "")}");
        }

        private void AddDispatcher()
        {
            Console.Write($"{(_wasHeightMoreThanNull ? "\n" : "")}Введите имя диспетчера: ");
            dispatchers.Add(new Dispatcher(Console.ReadLine()));
            DispathersWork += dispatchers.Last().Check;
        }

        private void DeleteDispatcher()
        {
            Console.WriteLine("\nСписок диспетчеров:");
            int indexShow = 1;
            foreach (var dispatcher in dispatchers) Console.WriteLine($"{indexShow++} - {dispatcher}");
            if (dispatchers.Count == 2) Console.WriteLine("Меньше двух диспетчеров не может быть.");
            else
            {
                int index = EnterNumber($"Введите индекс (1-{dispatchers.Count}): ", dispatchers.Count);
                _scores += dispatchers[index].Scores;
                DispathersWork -= dispatchers[index].Check;
                dispatchers.RemoveAt(index);
                _countDeletedDispatchers++;
            }
        }

        private static int EnterNumber(string outputString, int maxCount)
        {
            int number;
            do Console.Write(outputString);
            while (!int.TryParse(Console.ReadLine(), out number) || number < 1 || number > maxCount);
            return number - 1;
        }
    }
}