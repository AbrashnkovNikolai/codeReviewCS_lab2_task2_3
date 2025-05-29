public class Time
{

    private byte _hours;
    public byte Hours
    {
        get => _hours;
        set => _hours = value;
    }


    private byte _minutes;
    public byte Minutes
    {
        get => _minutes;
        set => _minutes = value;
    }


    public Time(byte hours, byte minutes)
    {
        this._hours = hours;
        this._minutes = minutes;
        NormalizeTime();
    }

    //приведение врмени к 24-х часовому формату
    private void NormalizeTime()
    {
        if (Minutes >= 60)
        {
            _hours += (byte)(_minutes / 60);
            _minutes %= 60;
        }
        Hours %= 24;
    }

    public static Time operator +(Time time, uint minutesToAdd)
    {
        uint totalMinutes = (uint)(time.Minutes + minutesToAdd);
        byte newHours = (byte)((time.Hours + totalMinutes / 60) % 24);
        byte newMinutes = (byte)(totalMinutes % 60);
        return new Time(newHours, newMinutes);
    }


    public static Time operator -(Time time, uint minutesToSubtract)
    {
        int totalMinutes = time.Minutes - (int)minutesToSubtract;

        byte newHours = time.Hours;
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
        return time.Hours;
    }


    public static implicit operator bool(Time time)
    {
        return time.Hours != 0 || time.Minutes != 0;
    }


    public override string ToString()
    {
        return $"{_hours:D2}:{_minutes:D2}";
    }


    public static byte StaticGetValidByteInput(string prompt, byte maxValue)
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
    public static uint StaticGetValidUIntInput(string prompt)
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

