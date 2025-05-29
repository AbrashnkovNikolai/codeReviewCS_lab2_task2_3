using System;



class Program
{
    static void Main(string[] args)
    {
        byte hours = Time.StaticGetValidByteInput("Введите часы (0-23): ", 23);
        byte minutes = Time.StaticGetValidByteInput("Введите минуты (0-59): ", 59);

        Time time = new Time(hours, minutes);
        Console.WriteLine("Текущее время: " + time);

        //тестирование операторов
        uint minutesToAdd = Time.StaticGetValidUIntInput("Введите количество минут для добавления: ");
        Time newTime = time + minutesToAdd; 
        Console.WriteLine("Новое время после добавления: " + newTime);

        uint minutesToSubtract = Time.StaticGetValidUIntInput("Введите количество минут для вычитания: ");
        Time subtractedTime = newTime - minutesToSubtract; 
        Console.WriteLine("Новое время после вычитания: " + subtractedTime);


        // Тестирование инкремента и декремента 
        Time incrementedTime = ++subtractedTime; // Используем перегруженный оператор ++
        Console.WriteLine("Время после добавления одной минуты: " + incrementedTime);

        Time decrementedTime = --incrementedTime; // Используем перегруженный оператор --
        Console.WriteLine("Время после вычитания одной минуты: " + decrementedTime);


        // Тестирование приведения типов
        byte hoursOnly = (byte)time; 
        Console.WriteLine($"байт часов: {hoursOnly}");

        bool isNotZero = time; 
        Console.WriteLine($"Время не равно 00:00? {isNotZero}");
    }
}
