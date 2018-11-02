using System;

namespace GGG_OnlineShop.Data.Services
{
    using Contracts;
    using InternalApiDB.Models;
    using System.Linq;
    using Common;
    using GGG_OnlineShop.Common;
    using Base;

    public class UsersService : BaseDataService<User>, IUsersService
    {
        public UsersService(IInternalDbRepository<User> dataSet) : base(dataSet)
        {
        }

        public void CleanUserInfoFromOrders(User user)
        {
            var orders = user.OrderedItems;

            foreach (var order in orders)
            {
                order.UserInfo = string.Format(GlobalConstants.DeletedUserInfo, user.PhoneNumber);
                order.UserЕmail = user.Email;
                order.UserId = null;
            }

            this.Data.Save();
        }

        public IQueryable<User> GetAllNotActivated()
        {
            return this.GetAll().Where(u => !u.EmailConfirmed);
        }

        public User GetByEmail(string email)
        {
            return this.GetAll().FirstOrDefault(u => u.Email == email);
        }

        public User Update(User user)
        {
            var userFromDb = this.GetById(user.Id);
            userFromDb.PercentageReduction = user.PercentageReduction;
            userFromDb.OnlyHighCostVisible = user.OnlyHighCostVisible;
            userFromDb.IsDeferredPaymentAllowed = user.IsDeferredPaymentAllowed;
            userFromDb.Bulstat = user.Bulstat;
            userFromDb.Name = user.Name;
            this.Save();

            return userFromDb;
        }

        public User UpdateContactInfo(User user)
        {
            var userFromDb = this.GetById(user.Id);
            userFromDb.DeliveryCountry = user.DeliveryCountry;
            userFromDb.DeliveryTown = user.DeliveryTown;
            userFromDb.DeliveryAddress = user.DeliveryAddress;
            userFromDb.PhoneNumber = user.PhoneNumber;
            this.Save();

            return userFromDb;
        }

        // Probably to return string with the needed info
        public bool IsCompanyAndBulstatCompatibiltyValid(User user)
        {
            bool result = true;

            if (user.IsCompany)
            {
                if (string.IsNullOrEmpty(user.Bulstat))
                {
                    result = false;
                }

                if (result && Data.All().FirstOrDefault(x => x.Bulstat == user.Bulstat) != null)
                {
                    result = false;
                }
            }
            else if (!user.IsCompany && !string.IsNullOrEmpty(user.Bulstat))
            {
                result = false;
            }

            return result;
        }

        // ------------------------- bulstat validator
        private readonly int[] _firstSum9DigitWeights = { 1, 2, 3, 4, 5, 6, 7, 8 };
        private readonly int[] _secondSum9DigitWeights = { 3, 4, 5, 6, 7, 8, 9, 10 };
        private readonly int[] _firstSum13DigitWeights = { 2, 7, 3, 5 };
        private readonly int[] _secondSum13DigitWeights = { 4, 9, 5, 7 };

        public bool IsBulstatValid(string bulstat)
        {
            bool result = false;
            string eik = bulstat.Substring(2, bulstat.Length - 1); // get the EIK (BG175309086)
            if (IsAllDigits(eik))
            {
                int[] digits = eik.Select(c => c - '0').ToArray();
                if (eik.Length == 9 && IsNineDigitsEikValid(digits))
                {
                    result = true;
                }
                else if (eik.Length == 13 && IsThirteenDigitsEikValid(digits))
                {
                    result = true;
                }
            }
            
            return result;
        }

        private bool IsNineDigitsEikValid(int[] digits)
        {
            int ninthDigit = CalculateNinthDigitInEik(digits);

            return ninthDigit == digits[8];
        }

        private bool IsThirteenDigitsEikValid(int[] digits)
        {
            int thirteenDigit = CalculateThirteenthDigitInEik(digits);

            return thirteenDigit == digits[12];
        }

        private int CalculateNinthDigitInEik(int[] digits)
        {
            int sum = 0;
            for (int i = 0; i < 8; i++)
            {
                sum = sum + (digits[i] * _firstSum9DigitWeights[i]);
            }

            int remainder = sum % 11;
            if (remainder != 10)
            {
                return remainder;
            }

            // remainder= 10
            int secondSum = 0;
            for (int i = 0; i < 8; i++)
            {
                secondSum = secondSum + (digits[i] * _secondSum9DigitWeights[i]);
            }

            int secondRem = secondSum % 11;
            if (secondRem != 10)
            {
                return secondRem;
            }

            // secondRemainder= 10
            return 0;
        }

        private int CalculateThirteenthDigitInEik(int[] digits)
        {
            int ninthDigit = CalculateNinthDigitInEik(digits);
            if (ninthDigit != digits[8])
            {
                throw new Exception("Incorrect 9th digit in EIK-13.");
            }

            // 9th Digit is a correct checkSum. Continue with 13thDigit
            int sum = 0;
            for (int i = 8, j = 0; j < 4; i++, j++)
            {
                sum = sum + (digits[i] * _firstSum13DigitWeights[j]);
            }

            int remainder = sum % 11;
            if (remainder != 10)
            {
                return remainder;
            }

            // remainder= 10
            int secondSum = 0;
            for (int i = 8, j = 0; j < 4; i++, j++)
            {
                secondSum = secondSum + (digits[i] * _secondSum13DigitWeights[j]);
            }

            int secondRem = secondSum % 11;
            if (secondRem != 10)
            {
                return secondRem;
            }

            // secondRemainder= 10
            return 0;
        }

        private bool IsAllDigits(string eikInput)
        {
            return eikInput.All(char.IsDigit);
        }
    }
}
