using System;

// Базовый класс
public class StringHolder
{
    protected string _value;

    // Конструктор
    public StringHolder(string value)
    {
        _value = value;
    }

    // Конструктор копирования
    public StringHolder(StringHolder other)
    {
        _value = other._value;
    }

    // Метод, создающий строку из первого и последнего символов поля
    public string GetFirstAndLastChars()
    {
        if (_value.Length < 2)
        {
            return _value;
        }
        return _value[0] + "" + _value[_value.Length - 1];
    }

    // Перегруженный метод ToString()
    public override string ToString()
    {
        return $"StringHolder: {_value}";
    }
}

// Дочерний класс
public class PasswordHolder : StringHolder
{
    private int _length;

    // Конструктор
    public PasswordHolder(string password, int length) : base(password)
    {
        _length = length;
    }

    // Конструктор копирования
    public PasswordHolder(PasswordHolder other) : base(other)
    {
        _length = other._length;
    }

    // Метод, проверяющий, является ли пароль достаточно длинным
    public bool IsPasswordLongEnough()
    {
        return _value.Length >= _length;
    }

    // Метод, маскирующий пароль звездочками
    public string MaskPassword()
    {
        return new string('*', _value.Length);
    }

    // Перегруженный метод ToString()
    public override string ToString()
    {
        return $"PasswordHolder: {_value} (length: {_length})";
    }
}

// Класс для ввода пароля с маскировкой
public class PasswordInput
{
    private string _password;
    private int _maxLength;

    // Конструктор
    public PasswordInput(int maxLength)
    {
        _maxLength = maxLength;
        _password = "";
    }

    // Метод для ввода пароля
    public void InputPassword()
    {
        do
        {
            Console.Write("Введите пароль (пароль имеет длинну {0} символов): ", _maxLength);
            ConsoleKeyInfo key;
            _password = ""; // Сброс пароля перед новым вводом
            do
            {
                key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Enter)
                {
                    break;
                }
                else if (key.Key == ConsoleKey.Backspace && _password.Length > 0)
                {
                    Console.Write("\b \b");
                    _password = _password.Substring(0, _password.Length - 1);
                }
                else if (key.KeyChar >= ' ' && key.KeyChar <= '~' && _password.Length < _maxLength)
                {
                    Console.Write("*");
                    _password += key.KeyChar;
                }
            } while (true);
            Console.WriteLine();

            // Проверка длины пароля
            if (_password.Length < _maxLength)
            {
                Console.WriteLine($"Пароль должен содержать минимум {_maxLength} символов. Попробуйте еще раз.");
            }
        } while (_password.Length < _maxLength); // Повторяем ввод, пока пароль недостаточно длинный
    }

    // Метод для получения введенного пароля
    public string GetPassword()
    {
        return _password;
    }

    // Метод для вывода пароля
    public void ShowPassword()
    {
        Console.WriteLine($"Введенный пароль: {_password}");
    }

    // Метод для маскированного вывода пароля
    public string MaskPassword()
    {
        return new string('*', _password.Length);
    }
}

public class Time
{
    private byte hours;
    private byte minutes;

    // Конструктор
    public Time(byte hours, byte minutes)
    {
        if (hours > 23 || minutes > 59)
            throw new ArgumentOutOfRangeException("Некорректное время.");

        this.hours = hours;
        this.minutes = minutes;
    }

    // Свойства
    public byte Hours
    {
        get { return hours; }
        set
        {
            if (value > 23)
                throw new ArgumentOutOfRangeException("Часы не могут быть больше 23.");
            hours = value;
        }
    }

    public byte Minutes
    {
        get { return minutes; }
        set
        {
            if (value > 59)
                throw new ArgumentOutOfRangeException("Минуты не могут быть больше 59.");
            minutes = value;
        }
    }

    // Унарные операции
    public static Time operator ++(Time t)
    {
        return t.AddMinutes(1);
    }

    public static Time operator --(Time t)
    {
        t.minutes --;
        if (t.minutes < 0)
                { t.hours = (byte)(t.hours - 1); 
            t.minutes = 59;

        }
        if (t.hours < 0)
        {
            t.hours = (byte)(t.hours - 1);
            t.minutes = 59;

        }
        return new Time(t.hours,t.minutes) ; // Возвращаем на 1 минуту назад
    }

    // Операции приведения типа
    public static explicit operator byte(Time t)
    {
        return t.hours; // Возвращаем только часы
    }

    public static implicit operator bool(Time t)
    {
        return t.hours != 0 || t.minutes != 0; // true, если не равно 00:00
    }

    // Бинарные операции
    public static Time operator +(Time t, uint minutesToAdd)
    {
        return t.AddMinutes(minutesToAdd);
    }

    public static Time operator -(Time t, uint minutesToSubtract)
    {
        Time new_t = new Time(t.hours, t.minutes);
        if (minutesToSubtract > 60)
        {
            byte new_hours = (byte)(new_t.hours - minutesToSubtract/60);
        }
        if (minutesToSubtract > t.minutes)
        {
            
            new_t.hours = (byte) (t.hours - 1);
            new_t.minutes  = (byte)((59 + (t.minutes - minutesToSubtract)  )%60);
        }
        return new_t;
    }

    public Time AddMinutes(uint minutesToAdd)
    {
        int totalMinutes = this.minutes + (int)minutesToAdd;
        byte newHours = (byte)((this.hours + totalMinutes / 60) % 24);
        byte newMinutes = (byte)(totalMinutes % 60);
        return new Time(newHours, newMinutes);
    }

    // Переопределение метода ToString()
    public override string ToString()
    {
        return $"{hours:D2}:{minutes:D2}";
    }

    // Метод для безопасного ввода данных
    public static byte GetValidByteInput(string prompt, byte maxValue)
    {
        byte result;
        while (true)
        {
            Console.Write(prompt);
            string input = Console.ReadLine();
            if (byte.TryParse(input, out result) && result <= maxValue)
            {
                return result;
            }
            Console.WriteLine($"Ошибка: введите число от 0 до {maxValue}.");
        }
    }

    // Метод для безопасного ввода uint
    public static uint GetValidUIntInput(string prompt)
    {
        uint result;
        while (true)
        {
            Console.Write(prompt);
            string input = Console.ReadLine();
            if (uint.TryParse(input, out result))
            {
                return result;
            }
            Console.WriteLine("Ошибка: введите неотрицательное целое число.");
        }
    }
}

// Класс для тестирования
class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Тестирование задания 1:");

        PasswordInput passwordInput = new PasswordInput(16);
        passwordInput.InputPassword();

        Console.WriteLine("Показать пароль? (y/n)");
        string answer = Console.ReadLine();
        if (answer.ToLower() == "y")
        {
            passwordInput.ShowPassword();
        }
        else
        {
            Console.WriteLine($"Маскированный пароль: {passwordInput.MaskPassword()}");
        }

        byte hours = Time.GetValidByteInput("Введите часы (0-23): ", 23);
        byte minutes = Time.GetValidByteInput("Введите минуты (0-59): ", 59);

        Time time = new Time(hours, minutes);
        Console.WriteLine("Текущее время: " + time);

        uint minutesToAdd = Time.GetValidUIntInput("Введите количество минут для добавления: ");
        Time newTime = time + minutesToAdd; // Используем перегруженный оператор +
        Console.WriteLine("Новое время после добавления: " + newTime);

        uint minutesToSubtract = Time.GetValidUIntInput("Введите количество минут для вычитания: ");
        Time subtractedTime = time - minutesToSubtract; // Используем перегруженный оператор -
        Console.WriteLine("Новое время после вычитания: " + subtractedTime);

        // Тестирование унарных операторов
        Time incrementedTime = ++time; // Используем перегруженный оператор ++
        Console.WriteLine("Время после добавления одной минуты: " + incrementedTime);

        Time decrementedTime = --time; // Используем перегруженный оператор --
        Console.WriteLine("Время после вычитания одной минуты: " + decrementedTime);

        // Тестирование приведения типов
        byte hoursOnly = (byte)time; // Приведение к типу byte
        Console.WriteLine($"Часы: {hoursOnly}");

        bool isNotZero = time; // Неявное приведение к типу bool
        Console.WriteLine($"Время не равно 00:00? {isNotZero}");
    }
}
