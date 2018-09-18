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
            return this.GetAll().Where(u => u.Email == email).FirstOrDefault();
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
        public bool IsValidUser(User user)
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
        private static int[] FIRST_SUM_9DIGIT_WEIGHTS = { 1, 2, 3, 4, 5, 6, 7, 8 };
        private static int[] SECOND_SUM_9DIGIT_WEIGHTS = { 3, 4, 5, 6, 7, 8, 9, 10 };
        private static int[] FIRST_SUM_13DIGIT_WEIGHTS = { 2, 7, 3, 5 };
        private static int[] SECOND_SUM_13DIGIT_WEIGHTS = { 4, 9, 5, 7 };

        public static bool calculateChecksumForNineDigitsEIK(string eik)
        {
            int[] digits = checkInput(eik, 9);
            int ninthDigit = calculateNinthDigitInEIK(digits);
            return ninthDigit == digits[8];
        }

        public static bool calculateChecksumForThirteenDigitsEIK(string eik)
        {
            int[] digits = checkInput(eik, 13);
            int thirteenDigit = calculateThirteenthDigitInEIK(digits);
            return thirteenDigit == digits[12];
        }

        private static int calculateNinthDigitInEIK(int[] digits)
        {
            int sum = 0;
            for (int i = 0; i < 8; i++)
            {
                sum = sum + (digits[i] * FIRST_SUM_9DIGIT_WEIGHTS[i]);
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
                secondSum = secondSum + (digits[i] * SECOND_SUM_9DIGIT_WEIGHTS[i]);
            }

            int secondRem = secondSum % 11;
            if (secondRem != 10)
            {
                return secondRem;
            }
            // secondRemainder= 10
            return 0;
        }

        private static int calculateThirteenthDigitInEIK(int[] digits)
        {
            int ninthDigit = calculateNinthDigitInEIK(digits);
            if (ninthDigit != digits[8])
            {
                throw new Exception("Incorrect 9th digit in EIK-13.");
            }
            // 9thDigit is a correct checkSum. Continue with 13thDigit
            int sum = 0;
            for (int i = 8, j = 0; j < 4; i++, j++)
            {
                sum = sum + (digits[i] * FIRST_SUM_13DIGIT_WEIGHTS[j]);
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
                secondSum = secondSum + (digits[i] * SECOND_SUM_13DIGIT_WEIGHTS[j]);
            }
            int secondRem = secondSum % 11;
            if (secondRem != 10)
            {
                return secondRem;
            }
            // secondRemainder= 10
            return 0;
        }

        private static int[] checkInput(string eik, int eikLength)
        {
            if (eik != null && eik.ToString().Length != eikLength)
            {
                throw new Exception("Incorrect count of digits in EIK: "
                  + eik.Length + "!= 9 or 13");
            }
            // eik.length= eikLength
            char[] charDigits = eik.ToCharArray();
            int[] digits = new int[charDigits.Length];
            for (int i = 0; i < digits.Length; i++)
            {
                if (Char.IsDigit(charDigits[i]))
                {
                    // TODO fix!!!
                    //digits[i] = Char.digit(charDigits[i], 10);
                }
                else
                {
                    throw new Exception(
                      "Incorrect input character. Only digits are allowed.");
                }
            }
            return digits;
        }
    }
}
