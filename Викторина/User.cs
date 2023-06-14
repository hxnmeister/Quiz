using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Викторина
{
    sealed class User
    {
        int score;
        string login;
        string password;
        DateTime birthDate;
        readonly string dataPath = @"C:\Users\Chaklun\source\repos\Викторина\bin\Debug\UserData.txt";
        readonly string scorePath = @"C:\Users\Chaklun\source\repos\Викторина\bin\Debug\UsersScore.txt";

        public User() : this("login", "password", new DateTime(year: 1111, month: 11, day: 11)) { }
        public User(string login, string password, DateTime birthDate)
        {
            score = 0;
            this.login = login;
            this.password = password;
            this.birthDate = birthDate;
        }

        private string Password
        {
            get { return password; }
            set { password = value; }
        }
        public string Login
        {
            get { return login; }
            private set { login = value; }
        }
        public int Score
        {
            get { return score; }
        }
        public DateTime BirthDate
        {
            get { return birthDate; }
            set { birthDate = value; }
        }

        void SetPassword()
        {
            Password = "";
            ConsoleKeyInfo passLetters;

            Console.Write(" Введите пароль: ");
            while ((passLetters = Console.ReadKey(true)).Key != ConsoleKey.Enter)
            {
                if (passLetters.Key == ConsoleKey.Backspace)
                {
                    if (Password != null && Password.Length > 0)
                    {
                        Password = Password.Remove(Password.Length - 1);
                        Console.Write("\b \b");
                        continue;
                    }
                    continue;
                }
                Password += passLetters.KeyChar;
                Console.Write("*");
            }
        }
        bool CheckUsersPassword()
        {
            try
            {
                using (StreamReader sr = new StreamReader(dataPath))
                {
                    while (!sr.EndOfStream)
                    {
                        if (sr.ReadLine().Contains(Login))
                        {
                            SetPassword();
                            Console.Clear();

                            return Password == sr.ReadLine();
                        }

                        sr.ReadLine();
                        sr.ReadLine();
                    }
                }
            }
            catch (FileNotFoundException e)
            {
                Console.Clear();
                Console.WriteLine(e.Message);
            }
            return false;
        }
        bool Registration()
        {
            try
            {
                using(StreamReader sr = new StreamReader(dataPath))
                {
                    while (!sr.EndOfStream)
                    {
                        if (sr.ReadLine().Contains(Login))
                        {
                            return false;
                        }

                        sr.ReadLine();
                        sr.ReadLine();
                    }
                }
            }
            catch (FileNotFoundException e)
            {
                Console.Clear();
                Console.WriteLine(e.Message);
            }

            SetPassword();
            while (true)
            {
                Console.Write("\n Введите дату рождения (ДД.ММ.ГГГГ): ");
                if(DateTime.TryParse(Console.ReadLine(), out birthDate))
                {
                    break;
                }

                Console.Clear();
                Console.WriteLine("\n Неверный формат даты!");
            }

            using (FileStream fs = new FileStream(dataPath, FileMode.Append))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.WriteLine(Login);
                    sw.WriteLine(Password);
                    sw.WriteLine(BirthDate.ToShortDateString());
                }
            }

            return true;
        }

        public bool Authentication()
        {
            while (true)
            {
                Console.WriteLine("\n Выберите действие:\n");
                Console.WriteLine("  1. Регистрация;");
                Console.WriteLine("  2. Авторизация;");

                Console.Write("\n Поле для ввода: ");
                ConsoleKeyInfo choice = Console.ReadKey();

                Console.Clear();
                switch (choice.KeyChar)
                {
                    case '1':
                        Console.Write("\n Введите логин: ");
                        Login = Console.ReadLine();

                        if (Registration())
                        {
                            Console.Clear();
                            Console.WriteLine("\n Регистрация успешна!");
                            return true;
                        }
                        Console.WriteLine($"\n Пользователь с логином \"{Login}\" уже существует!");
                        return false;
                    case '2':
                        Console.Write("\n Введите логин: ");
                        Login = Console.ReadLine();

                        if (CheckUsersPassword())
                        {
                            Console.Clear();
                            Console.WriteLine("\n Авторизация успешна!");
                            return true;
                        }
                        Console.WriteLine($"\n Логин или пароль введено неверно!");
                        return false;
                    default:
                        Console.WriteLine($"\n Введенное значение \"{choice.KeyChar}\" некорректно, повторите ввод!");
                        break;
                }
            }
        }
        public bool StartQuiz(char choice)
        {
            score = 0;

            void ShowQuestion(string category, int questionNumber)
            {
                string[] file = File.ReadAllLines($@"C:\Users\Chaklun\source\repos\Викторина\bin\Debug\{category}\{questionNumber}.txt");
                string correctAnswer = null;
                string answer = null;

                for (int i = 0; i < file.Length; i++)
                {
                    if (file[i].Contains('@'))
                    {
                        correctAnswer += file[i][1];
                        file[i] = file[i].Replace("@", "");
                    }

                    Console.WriteLine(" " + file[i]);
                }


                while (true)
                {
                    Console.Write("\n Введите правильный ответ: ");
                    try
                    {
                        foreach (string line in Console.ReadLine().Split(new char[] { ',', '.', ' ', '@', '-' }, StringSplitOptions.RemoveEmptyEntries))
                        {
                            answer += line;
                        }

                        if (answer == null)
                        {
                            throw new ArgumentNullException("Ответ");
                        }

                        break;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("\n" + e.Message);
                    }
                }

                if (answer.Count() == correctAnswer.Count())
                {
                    for (int i = 0; i < correctAnswer.Length; i++)
                    {
                        for (int j = 0; j < answer.Length; j++)
                        {
                            if (correctAnswer[i] == answer[j])
                            {
                                Console.Clear();
                                score++;
                                Console.WriteLine("\n Верный ответ!\n");
                                return;
                            }
                        }
                    }
                }

                Console.Clear();
                Console.WriteLine("\n Неверный ответ!\n");
            }
            void SaveScore(string category, int userScore)
            {
                using (FileStream fs = new FileStream(scorePath, FileMode.Append))
                {
                    using (StreamWriter sw = new StreamWriter(fs))
                    {
                        sw.WriteLine(login);
                        sw.WriteLine(category);
                        sw.WriteLine(userScore);
                    }
                }

                using (FileStream fs = new FileStream($@"C:\Users\Chaklun\source\repos\Викторина\bin\Debug\{category}\LeaderBoard.txt", FileMode.Append))
                {
                    using (StreamWriter sw = new StreamWriter(fs))
                    {
                        sw.WriteLine(login);
                        sw.WriteLine(userScore);
                    }
                }
            }

            Console.Clear();
            switch (choice)
            {
                case '1':
                    for (int i = 0; i < 20; i++)
                    {
                        ShowQuestion("Физика", i + 1);
                    }
                    Console.Clear();
                    Console.WriteLine($" В викторине по Физике Вы набрали: {Score} из 20 баллов");
                    SaveScore("Физика", Score);
                    return true;
                case '2':
                    for (int i = 0; i < 20; i++)
                    {
                        ShowQuestion("Биология", i + 1);
                    }
                    Console.Clear();
                    Console.WriteLine($" В викторине по Биологии Вы набрали: {Score} из 20 баллов");
                    SaveScore("Биология", Score);
                    return true;
                case '3':
                    for (int i = 0; i < 20; i++)
                    {
                        ShowQuestion("Математика", i + 1);
                    }
                    Console.Clear();
                    Console.WriteLine($" В викторине по Математике Вы набрали: {Score} из 20 баллов");
                    SaveScore("Математика", Score);
                    return true;
                case '4':
                    Random random = new Random();
                    for (int i = 0; i < 20; i++)
                    {
                        switch (random.Next(10, 39) / 10)
                        {
                            case 1:
                                ShowQuestion("Физика", i + 1);
                                break;
                            case 2:
                                ShowQuestion("Биология", i + 1);
                                break;
                            case 3:
                                ShowQuestion("Математика", i + 1);
                                break;
                        }
                    }
                    Console.Clear();
                    Console.WriteLine($"\n В смешанной викторине Вы набрали: {Score} из 20 баллов");
                    SaveScore("Смешанная", Score);
                    return true;
                case '0':
                    Console.Clear();
                    Console.WriteLine("\n Возвращение в главное меню...");
                    return true;
                default:
                    Console.Clear();
                    Console.WriteLine($"\n Введенное значение \"{choice}\" некорректно, повторите ввод!");
                    return false;
            }
        }
        public bool ChangePassword()
        {
            if(CheckUsersPassword())
            {
                Console.WriteLine("\n В поле ниже введите новый пароль!");
                SetPassword();
                try
                {
                    string[] fileData = File.ReadAllLines(dataPath);

                    for (int i = 0; i < fileData.Length; i++)
                    {
                        if (fileData[i].Contains(Login))
                        {
                            fileData[i + 1] = Password;
                            break;
                        }

                        i += 2;
                    }

                    using (FileStream fs = new FileStream(dataPath, FileMode.Truncate))
                    {
                        using (StreamWriter sw = new StreamWriter(fs))
                        {
                            foreach (string line in fileData) sw.WriteLine(line);
                        }
                    }
                }
                catch(FileNotFoundException e)
                {
                    Console.WriteLine(e.Message);
                    return false;
                }

                Console.Clear();
                return true;
            }

            return false;
        }
        public bool ChangeBirthDate()
        {
            if(CheckUsersPassword())
            {
                Console.WriteLine("\n В поле ниже введите новую дату рождения!");

                while(true)
                {
                    Console.Write("\n Введите дату рождения(ДД.ММ.ГГГГ): ");
                    if(DateTime.TryParse(Console.ReadLine(),out DateTime birthDate))
                    {
                        try
                        {
                            string[] fileData = File.ReadAllLines(dataPath);

                            for (int i = 0; i < fileData.Length; i++)
                            {
                                if (fileData[i].Contains(Login))
                                {
                                    fileData[i + 2] = birthDate.ToShortDateString();
                                    break;
                                }

                                i += 2;
                            }

                            using (FileStream fs = new FileStream(dataPath, FileMode.Truncate))
                            {
                                using (StreamWriter sw = new StreamWriter(fs))
                                {
                                    foreach (string line in fileData) sw.WriteLine(line);
                                }
                            }
                        }
                        catch (FileNotFoundException e)
                        {
                            Console.WriteLine(e.Message);
                            return false;
                        }

                        Console.Clear();
                        return true;
                    }
                    Console.Clear();
                    Console.WriteLine("\n Некорректный форматы даты!");
                }
            }
            return false;
        }
        public void DisplayUsersScore()
        {
            if (File.ReadAllLines(scorePath).Contains(Login))
            {
                try
                {
                    Console.WriteLine("\n Счет ранее пройденых викторин:\n");
                    using (StreamReader sr = new StreamReader(scorePath))
                    {
                        while (!sr.EndOfStream)
                        {
                            if (sr.ReadLine().Contains(Login))
                            {
                                Console.Write($" {sr.ReadLine()}: ");
                                Console.WriteLine(sr.ReadLine());
                            }
                        }
                    }
                }
                catch (FileNotFoundException)
                {
                    Console.WriteLine("\n Ни одна викторина еще не была пройдена!");
                    return;
                }

                return;
            }

            Console.WriteLine($"\n Пользователь {Login} еще не прошел ни одну викторину!");
        }
        public void DisplayTOP_20(string category)
        {
            string filePath = $@"C:\Users\Chaklun\source\repos\Викторина\bin\Debug\{category}\LeaderBoard.txt";
            Dictionary<string, int> leaderBoard = new Dictionary<string, int>();

            try
            {
                using (FileStream fs = new FileStream(filePath, FileMode.Open))
                {
                    if (fs.Length > 0)
                    {
                        using (StreamReader sr = new StreamReader(fs))
                        {
                            while (!sr.EndOfStream)
                            {
                                string userName = sr.ReadLine();
                                int userScore = Int32.Parse(sr.ReadLine());

                                if (leaderBoard.Count > 0)
                                {
                                    if (leaderBoard.ContainsKey(userName))
                                    {
                                        leaderBoard[userName] += userScore;
                                        continue;
                                    }
                                }

                                leaderBoard.Add(userName, userScore);
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("\n Ни один участник не прошел эту викторину!");
                        return;
                    }
                }

                Console.Clear();
                Console.WriteLine($"\n ТОП-20 участников которые прошли викторину по {category}:\n");
                foreach (KeyValuePair<string, int> pair in leaderBoard.OrderBy(pair => pair.Value).Reverse().Take(20))
                {
                    Console.WriteLine($" {pair.Key} - {pair.Value}");
                }
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
