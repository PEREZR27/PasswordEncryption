using System;
using System.Security.Cryptography;
using System.Text;
using System.Linq;
using System.Collections.Generic;

namespace PasswordHashingAuthentication
{
    class Program
    {
        static void Main(string[] args)
        {
            Select();
        }
        static void Select()
        {
            Console.Clear();
            Console.WriteLine(Menu());
            try
            {
                int selection;
                while (Int32.TryParse(Console.ReadLine(), out selection) == false || selection < 1 || selection > 3)
                {
                    Console.WriteLine(Menu());
                }
                switch (selection)
                {
                    case (1):
                        Account account = new Account();
                        UsernameCreator(account);
                        PasswordCreator(account);
                        //hashed password and username to the list
                        AccountRepository.Accounts.Add(account);
                        Console.WriteLine("Press [Enter]");
                        Console.ReadKey();
                        Console.Clear();
                        Select();
                        break;
                    case (2):
                        Account accntcheck = new Account();
                        Console.WriteLine("enter your username");
                        accntcheck.username = Console.ReadLine();
                        Console.WriteLine("enter your password");
                        accntcheck.password = Console.ReadLine();
                        EncryptPassword(accntcheck);
                        var check = AccountRepository.Accounts.FirstOrDefault(a => a.username == accntcheck.username && a.password == accntcheck.password);
                        if (check != null)
                        {
                            Console.WriteLine("Authorized\n" +
                                "Press [Enter]");
                            Console.ReadKey();
                            Select();
                            break;
                        }
                        Console.WriteLine("Not authorized" +
                            "Press [Enter]");
                        Console.ReadKey();
                        break;

                    case (3):
                        if (AccountRepository.Accounts != null)
                        {
                            foreach (Account a in AccountRepository.Accounts)
                                Console.WriteLine($"username:{a.username} password:{a.password}");
                        }
                        Environment.Exit(0);
                        break;
                }
            }
            catch
            {
                Console.WriteLine("Error404");
                Console.WriteLine(Menu());
            }

            string Menu() => "PASSWORD AUTHENTICATION SYSTEM!!!\n\n" +
                        "Please select one option:\n" +
                        "1. Create account\n" +
                        "2. Authenticate user\n" +
                        "3. Exit \n\n";
        }
        static void UsernameCreator(Account account)
        {

            Console.WriteLine("Enter your username\n" +
                              "(Max Char is 20)\n");
            string username = Console.ReadLine();
            if (username.Length <= 20)
            {
                account.username = username;
                foreach (Account a in AccountRepository.Accounts)
                {
                    if (a.username == account.username)
                    {
                        Console.WriteLine("That username already exists\n" +
                                          "press [Enter]");
                        Console.ReadKey();
                        Select();
                    }
                }

                Console.WriteLine($"Your username input is: {username}");
            }
            else
            {
                Console.WriteLine("Your username is too long. please try agan.\n");
                Select();
            }


        }
        static void PasswordCreator(Account account)
        {
            Console.WriteLine("Enter your password?\n" +
                              "==============================\n");
            string password = Console.ReadLine();

            account.password = password;
            EncryptPassword(account);

            Console.WriteLine("Your plain text password is: " + password + " \n" +
                              "Your hashed password is: " + account.password + ".");

        }
        static void EncryptPassword(Account account)
        {
            using (MD5 mdFiveHash = MD5.Create())
            {
                string hashedPass = getMdFiveHash(mdFiveHash, account.password);

                account.password = hashedPass;
            }
        }
        static string getMdFiveHash(MD5 md5Hash, string password)
        {
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
            StringBuilder stringBuild = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                stringBuild.Append(data[i].ToString("x1"));
            }
            return stringBuild.ToString();
        }
        class Account
        {
            public string username { get; set; }
            public string password { get; set; }

        }
        class AccountRepository
        {
            public static List<Account> Accounts = new List<Account>();
        }
    }

}
