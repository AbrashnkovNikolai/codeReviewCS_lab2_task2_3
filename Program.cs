using System;

public class StringHolder
{

    protected string _value;


    public StringHolder(string value)
    {
        _value = value;
    }

    public StringHolder(StringHolder other)
    {
        _value = other._value;
    }


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
    private int _length;

    public PasswordHolder(string password, int length) : base(password)
    {
        _length = length;
    }

    public PasswordHolder(PasswordHolder other) : base(other)
    {
        _length = other._length;
    }

    public bool IsPasswordLongEnough()
    {
        return _value.Length >= _length;
    }

    
    public string MaskPassword()
    {
        return new string('*', _value.Length);
    }

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


public class Time
{
    
    private byte hours;

    
    private byte minutes;

    
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
        hours %= 24; 
    }

    
    public static Time operator +(Time time, uint minutesToAdd)
    {
        uint totalMinutes = (uint)(time.minutes + minutesToAdd);
        byte newHours = (byte)((time.hours + totalMinutes / 60) % 24);
        byte newMinutes = (byte)(totalMinutes % 60);
        return new Time(newHours, newMinutes);
    }


    public static Time operator -(Time time, uint minutesToSubtract)
    {
        int totalMinutes = time.minutes - (int)minutesToSubtract;
        
        byte newHours = time.hours;
        byte newMinutes;

        
        if (totalMinutes < 0)
        {
            int hoursToSubtract = (int)Math.Ceiling((double)(-totalMinutes) / 60);
            newHours = (byte)((newHours - hoursToSubtract + 24) % 24); 
            newMinutes = (byte)((totalMinutes + 60 * hoursToSubtract) % 60); 
        }
        else
        {
            newMinutes = (byte)totalMinutes; 
        }

        return new Time(newHours, newMinutes);
    }

    
    public static Time operator ++(Time time)
    {
        return time + 1; 
    }

    
    public static Time operator --(Time time)
    {
        return time - 1; 
    }

    // Неявное приведение к типу byte для получения часов.
    public static implicit operator byte(Time time)
    {
        return time.hours;
    }

    
    public static implicit operator bool(Time time)
    {
        return time.hours != 0 || time.minutes != 0;
    }

    
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
        Time newTime = time + minutesToAdd; 
        Console.WriteLine("Новое время после добавления: " + newTime);

        uint minutesToSubtract = Time.GetValidUIntInput("Введите количество минут для вычитания: ");
        Time subtractedTime = newTime - minutesToSubtract; 
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
