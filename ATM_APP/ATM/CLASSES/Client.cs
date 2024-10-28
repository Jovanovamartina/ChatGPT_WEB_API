using System;
using System.Collections.Generic;
using System.Text;

namespace ATM.CLASSES
{
    internal class Client
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public long CardNumber { get; set; }
        private int _pin { get; set; }
        private decimal _balance { get; set; }
        public bool Locked { get; set; }

        public Client() { }
        public Client(string firstName, string lastName, long cardNumber, int pin, decimal balance)
        {
            FirstName = firstName;
            LastName = lastName;
            CardNumber = cardNumber;
            _pin = pin;
            _balance = balance;
        }

        public void CheckAccountBalance()
        {
            Console.WriteLine($"Your current bank balance is : {_balance}$");
        }
        public bool CheckPin(int pin)
        {
            return _pin == pin;
        }
        public void Deposit(decimal ammount)
        {
            _balance += ammount;
        }

        public void Withdraw(decimal ammount)
        {
            if (ammount > _balance)
            {
                Console.WriteLine("Insufficient funds, try again with another ammount");
                return;
            }

            _balance -= ammount;
            Console.WriteLine($"You have successfully withdrawn {ammount}$");
        }
        public void ChangePin(int newPin)
        {
            _pin = newPin;
        }

        public void LockAccount()
        {
            Locked = true;
        }

    }
}
