using ATM.CLASSES;
using System;

namespace ATM
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Client[] clients = new Client[3] {
                new Client("John" , "Doe" , 1111111111111111 , 1111 , 500) ,
                new Client("Jane" , "Doe" , 2222222222222222 , 2222 , 600) ,
                new Client("Bob" , "Bobsky", 3333333333333333 , 3333 , 750 )
            };
            Interface(clients);
        }

        public static void SendErrorMessage(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        public static long CardNumberInput()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Enter your card number in the following format ****-****-****-****: ");
                string input = Console.ReadLine();
                if (string.IsNullOrEmpty(input))
                {
                    SendErrorMessage("You must enter a card number");
                    Console.ReadLine();
                    continue;
                }
                string clearedInput = String.Join("", input.Split("-"));
                if (!long.TryParse(clearedInput, out long cardNumber))
                {
                    SendErrorMessage("Input is not a valid card number");
                    Console.ReadLine();
                    continue;
                }
                else if (clearedInput.Length != 16)
                {
                    SendErrorMessage("Card number must be exactly 16 digits");
                    Console.ReadLine();
                    continue;
                }
                return cardNumber;
            }
        }
        public static int PinInput()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Enter your pin : ");
                string input = Console.ReadLine();
                if (string.IsNullOrEmpty(input))
                {
                    SendErrorMessage("You must enter a pin");
                    Console.ReadLine();
                    continue;
                }
                if (!int.TryParse(input, out int pin))
                {
                    SendErrorMessage("Input is not a valid number");
                    Console.ReadLine();
                    continue;
                }
                else if (input.Length != 4)
                {
                    SendErrorMessage("Pin must be 4 digits long");
                    Console.ReadLine();
                    continue;
                }
                return pin;
            }
        }

        public static string NameInput(string name)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine($"Enter your {name} name : ");
                string input = Console.ReadLine();
                if (string.IsNullOrEmpty(input))
                {
                    SendErrorMessage("Name field cannot be empty.");
                    Console.ReadLine();
                    continue;
                }
                return input;
            }
        }

        public static Client FindClient(Client[] clients, long cardNumber)
        {

            foreach (Client c in clients)
            {
                if (c.CardNumber == cardNumber)
                {
                    return c;
                }
            }

            return null;
        }

        public static void HandleDeposit(Client client)
        {
            Console.Clear();
            Console.WriteLine("Enter ammount you would like to deposit");
            if (!decimal.TryParse(Console.ReadLine(), out decimal ammount))
            {
                SendErrorMessage("Invalid input");
                Console.ReadLine();
                return;
            }
            client.Deposit(ammount);
            Console.WriteLine($"You have successfully deposited {ammount}$");
            client.CheckAccountBalance();
            Console.ReadLine();
        }

        public static void HandleWithdraw(Client client)
        {
            Console.Clear();
            Console.WriteLine("Enter ammount you would like to withdraw : ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal ammount))
            {
                SendErrorMessage("Invalid input");
                Console.ReadLine();
                return;
            }
            client.Withdraw(ammount);
            client.CheckAccountBalance();
            Console.ReadLine();
        }

        public static bool Login(Client client)
        {
            int triesRemaining = 3;
            while (triesRemaining > 0)
            {
                int pin = PinInput();
                if (!client.CheckPin(pin))
                {
                    triesRemaining--;
                    SendErrorMessage($"Wrong pin , you have {triesRemaining} tries left.");
                    if (triesRemaining > 0)
                    {
                        Console.WriteLine("If you want to try again press y ");
                        string tryAgain = Console.ReadLine();
                        if (tryAgain.ToString() != "y")
                        {
                            Console.WriteLine("Please take your card!");
                            return false;
                        }
                    }
                    continue;
                }
                break;
            }
            if (triesRemaining == 0)
            {
                client.LockAccount();
                SendErrorMessage("Your bank account has been blocked. Contact out custommer support if you need help!");
                return false;
            }
            return true;
        }

        public static void Register(ref Client[] clients, string firstName, string lastName, long cardNumber, int pin)
        {
            Array.Resize(ref clients, clients.Length + 1);
            clients[clients.Length - 1] = new Client(firstName, lastName, cardNumber, pin, 0);
        }

        public static void ClientServices(Client client)
        {

            while (true)
            {
                Console.Clear();
                Console.WriteLine($"Welcome {client.FirstName}");
                Console.WriteLine("Enter : \n1.)Check bank balance\n2.)Withdraw Money\n3.)Deposit Money\n4.)Change pin\n5.)Exit");
                string input = Console.ReadLine();
                if (input == "1")
                {
                    Console.Clear();
                    client.CheckAccountBalance();
                    Console.ReadLine();
                    continue;
                }
                else if (input == "2")
                {
                    HandleWithdraw(client);
                    continue;
                }
                else if (input == "3")
                {
                    HandleDeposit(client);
                    continue;
                }else if (input == "4")
                {
                    Console.WriteLine("Please Enter your old pin.");
                    Console.ReadLine();
                    int oldPin = PinInput();
                    if (!client.CheckPin(oldPin))
                    {
                        SendErrorMessage("Incorrect pin");
                        Console.ReadLine();
                        continue;
                    }
                    Console.Clear();
                    Console.WriteLine("Enter your new pin.");
                    Console.ReadLine();
                    int newPin = PinInput();
                    client.ChangePin(newPin);
                    Console.WriteLine("Pin successfully changed.");
                    Console.ReadLine();
                    continue;
                }
                else if (input == "5")
                {
                    Console.WriteLine("Thank you for using our services , please take your card!");
                    break;
                }else
                {
                    Console.WriteLine("Invalid option.");
                    Console.ReadLine();
                    continue;
                }
            }
        }

        public static void Interface(Client[] clients)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("1.)LogIn\n2.)Register\n3.)Exit");

                string input = Console.ReadLine();
                if (input == "1")
                {
                    Console.Clear();
                    long cardNumber = CardNumberInput();
                    Client client = FindClient(clients, cardNumber);
                    if (client == null)
                    {
                        Console.Clear();
                        Console.WriteLine("User not found");
                        Console.ReadLine();
                        continue;
                    }
                    else if (client.Locked)
                    {
                        SendErrorMessage("Your account is locked , contact our customer support to unlock your bank account");
                        Console.ReadLine();
                        continue;
                    }
                    bool isLoggedIn = Login(client);
                    if (!isLoggedIn)
                    {
                        Console.ReadLine();
                        continue;
                    }
                    ClientServices(client);
                    continue;
                }
                else if (input == "2")
                {
                    Console.Clear();
                    long newCardNumber = CardNumberInput();

                    if (FindClient(clients, newCardNumber) != null)
                    {
                        SendErrorMessage("An account with that number already exists");
                        Console.ReadLine();
                        continue;
                    }

                    string newFirstName = NameInput("first");
                    string newLastName = NameInput("last");
                    int newPin = PinInput();
                    Register(ref clients, newFirstName, newLastName, newCardNumber, newPin);
                    Console.WriteLine($"Successfully registered , welcome {newFirstName},thank you for using our services");
                    Console.ReadLine();
                    continue;
                }
                else if (input == "3")
                {
                    Console.Clear();
                    Console.WriteLine("Bye!");
                    Console.ReadLine();
                    break;
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Invalid option");
                    Console.ReadLine();
                    continue;
                }
            }
        }
    }
        }
    
