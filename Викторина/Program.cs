using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.PerformanceData;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Викторина
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ConsoleKeyInfo choice;
            ConsoleKeyInfo subChoice;
            User player = new User();

            while (true)
            {
                if (player.Authentication())
                {
                    while (true)
                    {
                        Console.WriteLine($"\n Добрый день, {player.Login}!");
                        Console.WriteLine(" Выберите доступное действие:\n");
                        Console.WriteLine("  1. Начать новую викторину;");
                        Console.WriteLine("  2. Посмотреть результаты своих прошлых викторин;");
                        Console.WriteLine("  3. Посмотреть ТОП-20 по конкректной викторине;");
                        Console.WriteLine("  4. Изменить данные об аккаунте;");
                        Console.WriteLine("  0. Завершить работу;");

                        Console.Write("\n Поле для ввода: ");
                        choice = Console.ReadKey();

                        Console.Clear();
                        switch (choice.KeyChar)
                        {
                            case '1':
                                do
                                {
                                    Console.WriteLine("\n Выберите категорию викторины:\n");
                                    Console.WriteLine("  1. Физика;");
                                    Console.WriteLine("  2. Биология;");
                                    Console.WriteLine("  3. Математика;");
                                    Console.WriteLine("  4. Смешанные вопросы;");
                                    Console.WriteLine("  0. Возвращение в главное меню;");

                                    Console.Write("\n Поле для ввода: ");
                                } while (!player.StartQuiz(Console.ReadKey().KeyChar));
                                break;
                            case '2':
                                player.DisplayUsersScore();
                                break;
                            case '3':
                                do
                                {
                                    Console.WriteLine("\n Выберите категорию:\n");
                                    Console.WriteLine("  1. Физика;");
                                    Console.WriteLine("  2. Биология;");
                                    Console.WriteLine("  3. Математика;");
                                    Console.WriteLine("  4. Смешанная;");
                                    Console.WriteLine("  0. Вернуться в главное меню;");

                                    Console.Write("\n Поле для ввода: ");
                                    subChoice = Console.ReadKey(false);
                                    Console.Clear();

                                    switch (subChoice.KeyChar)
                                    {
                                        case '1':
                                            player.DisplayTOP_20("Физика");
                                            break;
                                        case '2':
                                            player.DisplayTOP_20("Биология");
                                            break;
                                        case '3':
                                            player.DisplayTOP_20("Математика");
                                            break;
                                        case '4':
                                            player.DisplayTOP_20("Смешанная");
                                            break;
                                        case '0':
                                            Console.WriteLine("\n Возвращение в главное меню...");
                                            break;
                                        default:
                                            Console.WriteLine($"\n Введенное значение \"{subChoice}\" некорректно, повторите ввод!");
                                            break;
                                    }
                                } while (subChoice.KeyChar != '0');
                                break;
                            case '4':
                                do
                                {
                                    Console.WriteLine("\n Выберите что желаете изменить:\n");
                                    Console.WriteLine("  1. Пароль;");
                                    Console.WriteLine("  2. Дату рождения;");
                                    Console.WriteLine("  0. Вернуться в главное меню;");

                                    Console.Write("\n Поле для ввода: ");
                                    subChoice = Console.ReadKey();

                                    Console.Clear();
                                    switch (subChoice.KeyChar)
                                    {
                                        case '1':
                                            if(player.ChangePassword())
                                            {
                                                Console.WriteLine("\n Пароль успешно изменен!");
                                                break;
                                            }
                                            Console.WriteLine("\n Неверный пароль!");
                                            break;
                                        case '2':
                                            if(player.ChangeBirthDate())
                                            {
                                                Console.WriteLine("\n Дата рождения заменена!");
                                                break;
                                            }
                                            Console.WriteLine("\n Неверный пароль!");
                                            break;
                                        case '0':
                                            Console.WriteLine("\n Возвращение в главное меню...");
                                            break;
                                        default:
                                            Console.WriteLine($"\n Введенное значение \"{subChoice.KeyChar}\" некорректно повторите ввод!");
                                            break;
                                    }
                                } while (subChoice.KeyChar != '0');
                                break;
                            case '0':
                                Console.WriteLine("\n Завершение работы...");
                                Console.WriteLine("\n\n Нажмите любую клавишу для продолжения...");
                                Console.ReadKey();
                                return;
                            default:
                                Console.WriteLine($"\n Введенное значение \"{choice.KeyChar}\" некорректно, повторите ввод!");
                                break;
                        }
                    }
                }
            }
        }
    }
}
