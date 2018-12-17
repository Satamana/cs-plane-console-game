using System;

namespace Plane
{
    class Dispatcher
    {
        string _name;
        int _correction = 0;
        int _scores = 0;
        int _hp;
        Random rand = new Random();
        public string Name { get => _name; set => _name = value; }
        public int Scores { get => _scores; }
        public int Hp { get => _hp; }

        public Dispatcher(string name)
        {
            _name = name;
            _correction = rand.Next(-200, 200);
        }

        public void Check(Plane pl)
        {
            _hp = 7 * pl.Speed - _correction;
            Console.WriteLine($"{_name}: рекомендуемая высота полета {_hp}. Штраф: {_scores}");
            if (pl.Speed == 0 && pl.Height > 0) throw new Exception("Самолет разбился. Скорость достигла нуля на высоте.");
            if (pl.Speed > 50 && pl.Height == 0) throw new Exception("Самолет разбился. Попытка посадки на большой скорости.");
            if (pl.Speed == 0 && pl.Height == 0 && pl.Success) throw new Exception("Самолет совершил посадку. Цель (1000 км/ч) достигнута.");
            if (pl.Speed == 0 && pl.Height == 0 && !pl.Success) throw new Exception("Самолет совершил посадку. Цель (1000 км/ч) не достигнута.");
            if (pl.Speed > 1000)
            {
                _scores += 100;
                Console.WriteLine("Снижайте скорость.");
            }
            if (Math.Abs(pl.Height - _hp) > 300 && Math.Abs(pl.Height - _hp) <= 600) _scores += 25;
            else if (Math.Abs(pl.Height - _hp) > 600 && Math.Abs(pl.Height - _hp) <= 1000) _scores += 50;
            else if (Math.Abs(pl.Height - _hp) > 1000) throw new Exception("Самолет разбился. ");
            if (_scores >= 1000) throw new Exception("Непригоден к полетам.");
        }

        public override string ToString() => $"{_name}";
    }
}