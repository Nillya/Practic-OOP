using System;

namespace Prcat_1
{
    // Клас "Раціональне число"
    class Fraction
    {
        public int sign;        // Знак дробу (+1 або -1)
        public int intPart;     // Ціла частина дробу
        public int numerator;   // Чисельник дробу
        public int denominator; // Знаменник дробу

        // Конструктор без параметрів
        public Fraction()
        {
            sign = 1;
            intPart = 0;
            numerator = 0;
            denominator = 1;
        }

        // Конструктор з параметрами
        public Fraction(int n, int d, int i = 0, int s = 1)
        {
            sign = s;
            intPart = i;
            numerator = n;
            denominator = d;
            GetMixedView();
        }

        // Конструктор для перетворення double у Fraction
        public Fraction(double value)
        {
            sign = value < 0 ? -1 : 1;
            value = Math.Abs(value);
            intPart = (int)value;
            double fractionPart = value - intPart;
            denominator = (int)Math.Pow(10, 10); // Точність до 10 знаків
            numerator = (int)(fractionPart * denominator);
            GetMixedView();
        }

        // Метод перетворення дробу до змішаного виду
        private void GetMixedView()
        {
            numerator += intPart * denominator;
            intPart = numerator / denominator;
            numerator %= denominator;
            Cancellation();
        }

        // Метод скорочення дробу
        private void Cancellation()
        {
            if (numerator == 0) return;
            int a = Math.Abs(numerator), b = denominator;
            while (b != 0)
            {
                int temp = b;
                b = a % b;
                a = temp;
            }
            int gcd = a;
            numerator /= gcd;
            denominator /= gcd;
        }

        // Перевантаження арифметичних операцій
        public static Fraction operator +(Fraction f1, Fraction f2)
        {
            int num = f1.sign * (f1.intPart * f1.denominator + f1.numerator) * f2.denominator +
                      f2.sign * (f2.intPart * f2.denominator + f2.numerator) * f1.denominator;
            int den = f1.denominator * f2.denominator;
            int signResult = num < 0 ? -1 : 1;
            return new Fraction(Math.Abs(num), den, 0, signResult);
        }

        public static Fraction operator +(Fraction f, double d) => f + new Fraction(d);
        public static Fraction operator +(double d, Fraction f) => new Fraction(d) + f;

        public static Fraction operator -(Fraction f1, Fraction f2) => f1 + new Fraction(-f2.sign * (f2.intPart * f2.denominator + f2.numerator), f2.denominator);
        public static Fraction operator -(Fraction f, double d) => f - new Fraction(d);
        public static Fraction operator -(double d, Fraction f) => new Fraction(d) - f;

        public static Fraction operator *(Fraction f1, Fraction f2)
        {
            int num = f1.sign * (f1.intPart * f1.denominator + f1.numerator) * f2.sign * (f2.intPart * f2.denominator + f2.numerator);
            int den = f1.denominator * f2.denominator;
            int signResult = num < 0 ? -1 : 1;
            return new Fraction(Math.Abs(num), den, 0, signResult);
        }

        public static Fraction operator *(Fraction f, double d) => f * new Fraction(d);
        public static Fraction operator *(double d, Fraction f) => new Fraction(d) * f;

        public static Fraction operator /(Fraction f1, Fraction f2)
        {
            int num = f1.sign * (f1.intPart * f1.denominator + f1.numerator) * f2.denominator;
            int den = f1.denominator * f2.sign * (f2.intPart * f2.denominator + f2.numerator);
            int signResult = num * den < 0 ? -1 : 1;
            return new Fraction(Math.Abs(num), Math.Abs(den), 0, signResult);
        }

        public static Fraction operator /(Fraction f, double d) => f / new Fraction(d);
        public static Fraction operator /(double d, Fraction f) => new Fraction(d) / f;

        // Перевантаження операцій порівняння
        public static bool operator >(Fraction f1, Fraction f2) => (double)f1 > (double)f2;
        public static bool operator <(Fraction f1, Fraction f2) => (double)f1 < (double)f2;
        public static bool operator >=(Fraction f1, Fraction f2) => (double)f1 >= (double)f2;
        public static bool operator <=(Fraction f1, Fraction f2) => (double)f1 <= (double)f2;
        public static bool operator ==(Fraction f1, Fraction f2) => (double)f1 == (double)f2;
        public static bool operator !=(Fraction f1, Fraction f2) => (double)f1 != (double)f2;

        public static bool operator >(Fraction f, double d) => (double)f > d;
        public static bool operator <(Fraction f, double d) => (double)f < d;
        public static bool operator >=(Fraction f, double d) => (double)f >= d;
        public static bool operator <=(Fraction f, double d) => (double)f <= d;
        public static bool operator ==(Fraction f, double d) => (double)f == d;
        public static bool operator !=(Fraction f, double d) => (double)f != d;

        // Перетворення в double
        public static explicit operator double(Fraction f) => f.sign * (f.intPart + (double)f.numerator / f.denominator);

        // Перетворення в рядок
        public override string ToString()
        {
            string result = sign < 0 ? "-" : "";
            if (intPart != 0) result += intPart;
            if (numerator != 0) result += (intPart != 0 ? " " : "") + numerator + "/" + denominator;
            return result.Length > 0 ? result : "0";
        }

        // Статичний метод Parse
        public static Fraction Parse(string str)
        {
            string[] parts = str.Split(' ');
            if (parts.Length == 1)
            {
                if (str.Contains("/"))
                {
                    string[] frac = str.Split('/');
                    int num = int.Parse(frac[0]);
                    int den = int.Parse(frac[1]);
                    int s = num < 0 ? -1 : 1;
                    return new Fraction(Math.Abs(num), den, 0, s);
                }
                int whole = int.Parse(str);
                return new Fraction(0, 1, whole, whole < 0 ? -1 : 1);
            }
            int i = int.Parse(parts[0]);
            string[] fracParts = parts[1].Split('/');
            int n = int.Parse(fracParts[0]);
            int d = int.Parse(fracParts[1]);
            int signResult = i < 0 ? -1 : 1; // Перейменовано s на signResult
            return new Fraction(n, d, Math.Abs(i), signResult);
        }
    }

    // Клас "Комплексне число"
    class Complex_num
    {
        public double real;  // Дійсна частина
        public double imag;  // Уявна частина

        public Complex_num(double r, double i)
        {
            real = r;
            imag = i;
        }

        public static Complex_num operator +(Complex_num c1, Complex_num c2) => new Complex_num(c1.real + c2.real, c1.imag + c2.imag);
        public static Complex_num operator -(Complex_num c1, Complex_num c2) => new Complex_num(c1.real - c2.real, c1.imag - c2.imag);
        public static Complex_num operator *(Complex_num c1, Complex_num c2) =>
            new Complex_num(c1.real * c2.real - c1.imag * c2.imag, c1.real * c2.imag + c1.imag * c2.real);
        public static Complex_num operator /(Complex_num c1, Complex_num c2)
        {
            double denominator = c2.real * c2.real + c2.imag * c2.imag;
            return new Complex_num((c1.real * c2.real + c1.imag * c2.imag) / denominator, (c1.imag * c2.real - c1.real * c2.imag) / denominator);
        }

        public static bool operator ==(Complex_num c1, Complex_num c2) => c1.real == c2.real && c1.imag == c2.imag;
        public static bool operator !=(Complex_num c1, Complex_num c2) => !(c1 == c2);

        public override string ToString() => $"{real} {(imag >= 0 ? "+" : "-")} {Math.Abs(imag)}i";

        public static Complex_num Parse(string str)
        {
            str = str.Replace(" ", "");
            int signIndex = str.Contains("+") ? str.IndexOf("+") : str.IndexOf("-");
            double r = double.Parse(str.Substring(0, signIndex));
            double i = double.Parse(str.Substring(signIndex, str.Length - signIndex - 1));
            return new Complex_num(r, i);
        }
    }

    // Клас "Дата"
    class MyDate
    {
        public int day, month, year;

        public MyDate(int d, int m, int y)
        {
            day = d;
            month = m;
            year = y;
        }

        ~MyDate() { Console.WriteLine($"Дата {this} знищена."); }

        public static MyDate operator +(MyDate date, int days)
        {
            DateTime dt = new DateTime(date.year, date.month, date.day).AddDays(days);
            return new MyDate(dt.Day, dt.Month, dt.Year);
        }

        public static int operator -(MyDate d1, MyDate d2)
        {
            DateTime dt1 = new DateTime(d1.year, d1.month, d1.day);
            DateTime dt2 = new DateTime(d2.year, d2.month, d2.day);
            return (dt1 - dt2).Days;
        }

        public static bool operator >(MyDate d1, MyDate d2) => new DateTime(d1.year, d1.month, d1.day) > new DateTime(d2.year, d2.month, d2.day);
        public static bool operator <(MyDate d1, MyDate d2) => new DateTime(d1.year, d1.month, d1.day) < new DateTime(d2.year, d2.month, d2.day);
        public static bool operator >=(MyDate d1, MyDate d2) => new DateTime(d1.year, d1.month, d1.day) >= new DateTime(d2.year, d2.month, d2.day);
        public static bool operator <=(MyDate d1, MyDate d2) => new DateTime(d1.year, d1.month, d1.day) <= new DateTime(d2.year, d2.month, d2.day);
        public static bool operator ==(MyDate d1, MyDate d2) => new DateTime(d1.year, d1.month, d1.day) == new DateTime(d2.year, d2.month, d2.day);
        public static bool operator !=(MyDate d1, MyDate d2) => new DateTime(d1.year, d1.month, d1.day) != new DateTime(d2.year, d2.month, d2.day);

        public override string ToString() => $"{day:D2}.{month:D2}.{year}";

        public static MyDate Parse(string str)
        {
            string[] parts = str.Split('.');
            return new MyDate(int.Parse(parts[0]), int.Parse(parts[1]), int.Parse(parts[2]));
        }
    }

    // Клас "Час"
    class MyTime
    {
        public int hours, minutes, seconds;

        public MyTime(int h, int m, int s)
        {
            hours = h;
            minutes = m;
            seconds = s;
        }

        ~MyTime() { Console.WriteLine($"Час {this} знищений."); }

        public static MyTime operator +(MyTime time, int minutes)
        {
            DateTime dt = new DateTime(1, 1, 1, time.hours, time.minutes, time.seconds).AddMinutes(minutes);
            return new MyTime(dt.Hour, dt.Minute, dt.Second);
        }

        public static int operator -(MyTime t1, MyTime t2)
        {
            DateTime dt1 = new DateTime(1, 1, 1, t1.hours, t1.minutes, t1.seconds);
            DateTime dt2 = new DateTime(1, 1, 1, t2.hours, t2.minutes, t2.seconds);
            return (int)(dt1 - dt2).TotalSeconds;
        }

        public override string ToString() => $"{hours:D2}:{minutes:D2}:{seconds:D2}";

        public static MyTime Parse(string str)
        {
            string[] parts = str.Split(':');
            return new MyTime(int.Parse(parts[0]), int.Parse(parts[1]), int.Parse(parts[2]));
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Тестування класу Fraction:");
            Fraction f1 = new Fraction(2, 3);
            Fraction f2 = new Fraction(5, 7);
            Console.WriteLine($"f1 = {f1}");
            Console.WriteLine($"f2 = {f2}");
            Console.WriteLine($"f1 + f2 = {f1 + f2}");
            Console.WriteLine($"f1 - 0.5 = {f1 - 0.5}");
            Console.WriteLine($"f1 > f2: {f1 > f2}");
            Console.WriteLine($"Fraction.Parse(\"2 1/3\") = {Fraction.Parse("2 1/3")}");

            Console.WriteLine("\nТестування класу Complex_num:");
            Complex_num c1 = new Complex_num(3, 7);
            Complex_num c2 = new Complex_num(2, -4);
            Console.WriteLine($"c1 = {c1}");
            Console.WriteLine($"c2 = {c2}");
            Console.WriteLine($"c1 + c2 = {c1 + c2}");
            Console.WriteLine($"c1 * c2 = {c1 * c2}");
            Console.WriteLine($"Complex_num.Parse(\"3+7i\") = {Complex_num.Parse("3+7i")}");

            Console.WriteLine("\nТестування класу MyDate:");
            MyDate d1 = new MyDate(10, 10, 2023);
            MyDate d2 = new MyDate(2, 9, 2010);
            Console.WriteLine($"d1 = {d1}");
            Console.WriteLine($"d2 = {d2}");
            Console.WriteLine($"d1 + 5 днів = {d1 + 5}");
            Console.WriteLine($"d1 - d2 = {d1 - d2} днів");
            Console.WriteLine($"d1 > d2: {d1 > d2}");
            Console.WriteLine($"MyDate.Parse(\"15.04.2011\") = {MyDate.Parse("15.04.2011")}");

            Console.WriteLine("\nТестування класу MyTime:");
            MyTime t1 = new MyTime(14, 25, 15);
            MyTime t2 = new MyTime(0, 59, 58);
            Console.WriteLine($"t1 = {t1}");
            Console.WriteLine($"t2 = {t2}");
            Console.WriteLine($"t1 + 45 хвилин = {t1 + 45}");
            Console.WriteLine($"t1 - t2 = {t1 - t2} секунд");
            Console.WriteLine($"MyTime.Parse(\"00:59:58\") = {MyTime.Parse("00:59:58")}");
        }
    }
}