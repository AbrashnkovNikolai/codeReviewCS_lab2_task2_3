using System;

// Базовый класс StringHolder.
public class StringHolder
{
    // Защищенное поле для хранения строки.
    protected string _value;

    // Конструктор для инициализации строки.
    public StringHolder(string value)
    {
        _value = value;
    }

    // Конструктор копирования для создания копии объекта.
    public StringHolder(StringHolder other)
    {
        _value = other._value;
    }

    // Метод для получения строки из первого и последнего символов.
    public string GetFirstAndLastChars()
    {
        if (_value.Length < 2)
        {
            return _value;
        }
        return _value[0] + "" + _value[_value.Length - 1];
    }

    // Перегрузка метода ToString для вывода строки.
    public override string ToString()
    {
        return $"StringHolder: {_value}";
    }
}

// Дочерний класс PasswordHolder.
public class PasswordHolder : StringHolder
{
    // Член для хранения длины пароля.
    private int _length;

    // Конструктор для инициализации пароля и длины.
    public PasswordHolder(string password, int length) : base(password)
    {
        _length = length;
    }

    // Конструктор копирования 
    public PasswordHolder(PasswordHolder other) : base(other)
    {
        _length = other._length;
    }

    // Метод для проверки длины пароля.
    public bool IsPasswordLongEnough()
    {
        return _value.Length >= _length;
    }

    // Метод для маскировки пароля.
    public string MaskPassword()
    {
        return new string('*', _value.Length);
    }

    // Перегрузка метода ToString для вывода пароля и длины.
    public override string ToString()
    {
        return $"PasswordHolder: {_value} (length: {_length})";
    }
}

// Класс для ввода пароля с маскировкой.
public class PasswordInput
{
    
    private string _password;

    
    private int _minLength;

    // Конструктор для инициализации минимальной длины пароля.
    public PasswordInput(int minLength)
    {
        _minLength = minLength;
        _password = "";
    }


    public void InputPassword()
    {
        do
        {
            Console.Write("Задайте пароль (пароль должен содержать минимум {0} символов): ", _minLength);
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
                else if (key.KeyChar >= ' ' && key.KeyChar <= '~')
                {
                    Console.Write("*");
                    _password += key.KeyChar;
                }
            } while (true);
            Console.WriteLine();

            // Проверка длины пароля
            if (_password.Length < _minLength)
            {
                Console.WriteLine($"Пароль должен содержать минимум {_minLength} символов. Попробуйте еще раз.");
            }
        } while (_password.Length < _minLength);
    }

    // Метод для получения введенного пароля.
    public string GetPassword()
    {
        return _password;
    }

    
    public void ShowPassword()
    {
        Console.WriteLine($"Введенный пароль: {_password}");
    }

    
    public string MaskPassword()
    {
        return new string('*', _password.Length);
    }
}

// Класс времени 
public class Time
{
    
    private byte hours;

    
    private byte minutes;

    // Конструктор для инициализации часов и минут.
    public Time(byte hours, byte minutes)
    {
        this.hours = hours;
        this.minutes = minutes;
        NormalizeTime();
    }

    // Метод для нормализации времени.
    private void NormalizeTime()
    {
        if (minutes >= 60)
        {
            hours += (byte)(minutes / 60);
            minutes %= 60;
        }
        hours %= 24; // Часы должны быть в диапазоне от 0 до 23
    }

    // Перегрузка оператора сложения для добавления минут.
    public static Time operator +(Time time, uint minutesToAdd)
    {
        uint totalMinutes = (uint)(time.minutes + minutesToAdd);
        byte newHours = (byte)((time.hours + totalMinutes / 60) % 24);
        byte newMinutes = (byte)(totalMinutes % 60);
        return new Time(newHours, newMinutes);
    }

    // Перегрузка оператора вычитания для вычитания минут.
    public static Time operator -(Time time, uint minutesToSubtract)
    {
        int totalMinutes = time.minutes - (int)minutesToSubtract;
        
        byte newHours = time.hours;
        byte newMinutes;

        // Если totalMinutes отрицательные, корректируем часы и минуты
        if (totalMinutes < 0)
        {
            int hoursToSubtract = (int)Math.Ceiling((double)(-totalMinutes) / 60);
            newHours = (byte)((newHours - hoursToSubtract + 24) % 24); 
            newMinutes = (byte)((totalMinutes + 60 * hoursToSubtract) % 60); 
        }
        else
        {
            newMinutes = (byte)totalMinutes; // Если totalMinutes не отрицательные
        }

        return new Time(newHours, newMinutes);
    }

    // Перегрузка оператора инкремента для добавления одной минуты.
    public static Time operator ++(Time time)
    {
        return time + 1; // Увеличиваем на 1 минуту
    }

    // Перегрузка оператора декремента для вычитания одной минуты.
    public static Time operator --(Time time)
    {
        return time - 1; // Уменьшаем на 1 минуту
    }

    // Неявное приведение к типу byte для получения часов.
    public static implicit operator byte(Time time)
    {
        return time.hours;
    }

    // Неявное приведение к типу bool для проверки времени.
    public static implicit operator bool(Time time)
    {
        return time.hours != 0 || time.minutes != 0;
    }

    // Перегрузка метода ToString для вывода времени.
    public override string ToString()
    {
        return $"{hours:D2}:{minutes:D2}";
    }


    public static byte GetValidByteInput(string prompt, byte maxValue)
    {
        byte value;
        while (true)
        {
            Console.Write(prompt);
            if (byte.TryParse(Console.ReadLine(), out value) && value <= maxValue)
            {
                return value;
            }
            Console.WriteLine($"Пожалуйста, введите число от 0 до {maxValue}.");
        }
    }

    // Метод для получения валидного входного значения uint.
    // Параметр: prompt - строка, выводимая в консоль.
    public static uint GetValidUIntInput(string prompt)
    {
        uint value;
        while (true)
        {
            Console.Write(prompt);
            if (uint.TryParse(Console.ReadLine(), out value))
            {
                return value;
            }
            Console.WriteLine("Пожалуйста, введите неотрицательное число.");
        }
    }
}


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
        Time subtractedTime = newTime - minutesToSubtract; // Используем перегруженный оператор -
        Console.WriteLine("Новое время после вычитания: " + subtractedTime);

        // Тестирование унарных операторов
        Time incrementedTime = ++subtractedTime; // Используем перегруженный оператор ++
        Console.WriteLine("Время после добавления одной минуты: " + incrementedTime);

        Time decrementedTime = --incrementedTime; // Используем перегруженный оператор --
        Console.WriteLine("Время после вычитания одной минуты: " + decrementedTime);

        // Тестирование приведения типов
        byte hoursOnly = (byte)time; // Приведение к типу byte
        Console.WriteLine($"Часы: {hoursOnly}");

        bool isNotZero = time; // Неявное приведение к типу bool
        Console.WriteLine($"Время не равно 00:00? {isNotZero}");
    }
}
