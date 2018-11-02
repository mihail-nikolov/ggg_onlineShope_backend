using System.Collections.Generic;
using System.Dynamic;

namespace GGG_OnlineShop.Common
{
    public class GlobalConstants
    {
        public const string AdministratorRoleName = "admin";
        public const string ExternalDBRestrictionMessage = "Do not have rights on external DB";
        public const string DBContextRequiredErrorMessage = "An instance of DbContext is required to use this repository.";
        public const string DbFilledInFinishedMessage = "Db filled/updated finished. Check log files for info/errors.";
        public const string AppDomainPath = "http://shop.glassgoldgroup.eu"; 
        // TODO build on release!!!

        public const char InterchangeablePartStringSeparator = ';';
        public const int InterchangeableEurocodeMaxLength = 400;

        public const string MaxLengthErrorMessage = "{0}: max length: {1}";
        public const string MinAndMaxLengthErrorMessage = "{0}: min length:{2}, max length: {1}";

        public const double MinPercentPaidPrice = 0.45;
        public const double MinPrice = 0.1;
        public const double MaxPrice = double.MaxValue;

        public const int FullAddressMaxLength = 400;
        public const int ManufacturerMaxLength = 30;
        public const int OtherCodesMaxLength = 200;

        public const int UserЕmailMaxLength = 40;
        public const int UserInfoMaxLength = 200;
        public const int BulstatMaxLength = 20;
        public const int BulstatMinLength = 2;
        public const int NameMaxLength = 60;
        public const int NameMinLength = 2;
        public const string DeletedUserInfo = "Deleted user; {0}";
        public const int DeliveryNotesMaxLength = 600;
        public const int DeliveryCountryMaxLength = 50;
        public const int DeliveryCountryMinLength = 2;
        public const int DeliveryTownMaxLength = 50;
        public const int DeliveryTownMinLength = 2;
        public const int DeliveryAddressMaxLength = 200;
        public const int DeliveryAddressMinLength = 2;

        public const int LogPlaceMaxLength = 70;
        public const int LogPlaceMinLength = 5;
        public const int LogTypeMaxLength = 15;
        public const int LogTypeMinLength = 2;
        public const int LogInfoMaxLength = 700;
        
        public const double MinPercentageReduction = 0;
        public const double MaxPercentageReduction = 100;
        public const int PasswordMaxLength = 100;
        public const int PasswordMinLength = 4;
        public const int PhoneNumberMaxLength = 30;
        public const int PhoneNumberMinLength = 3;

        public const string InvalidBulstat = "Невалиден Булстат";
        public const string InvalidCompanyBulstatCombination = "Невалидна комбинацияя за булстат и компания";
        public const string NoSuchAUserErroMessage = "No such a user";
        public const string SMTPServer = "smtp.gmail.com";
        public const string FindingUserError = "Error while finding user";
        public const string EmalToSendFrom = "testggg9@gmail.com";
        public const string EmalToSendFromPassword = "ggg12345";
        public const string ResetPasswordSubject = "Забравена парола";
        public const string ResetPasswordBody = "Моля, обновете паролата си, използвайки следния код: {0}";
        public const string ConfirmEmailSubject = "Потвърждаване на акаунт";
        public const string ConfirmEmailBody = "Моля, потвърдете акаунта си, кликвайки на следния линк: {0}";
        public const string OrderMade = "направена поръчка, ID-та на отделните поръчани елементи: {0}";
        public const string OrderUpdated = "нов статус на поръчка, ID: {0}";
        public const string WrongCodeErrorMessage = "wrong code for {0}";
        public const string EmailConfirmationFailedErrorMessage = "email confirmation failed";
        public const string CannotRemoveAdminErrorMessage = "Cannot remove Admin";

        public const int BodyTypeCodeMinLength = 1;
        public const int BodyTypeCodeMaxLength = 10;
        public const int BodyTypeDescriptionMaxLength = 100;
        public const int MakeNameMaxLength = 100;
        public const int MakeNameMinLength = 1;
        public const int ModelNameMaxLength = 100;
        public const int ModelNameMinLength = 1;
        public const string NeededCodesErrorMessage = "No argument passed";

        public const string CodeMinLengthErrorMessage = "Enter at least 4 symbols";
        public const int CodeMinLength = 4;
        public const int EurocodeMinLength = 5;
        public const int EurocodeMaxLength = 30;
        public const int DescriptionMaxLength = 400;
        public const int OesCodeMaxLength = 30;
        public const int LocalCodeMaxLength = 30;
        public const int IndustryCodeMaxLength = 30;
        public const int MaterialNumberMaxLength = 30;
        public const int ModelDateMaxLength = 30;
        public const int PartDateMaxLength = 30;
        public const int ProductTypeMaxLength = 30;
        public const int ModificationMaxLength = 100;
        public const int TintMaxLength = 50;
        public const int FittingTypeMaxLength = 50;
        public const double MinProductFittingTimeHours = 0;
        public const double MaxProductFittingTimeHours = double.MaxValue;
        public const double MinProductHeight = 0.1;
        public const double MaxProductHeight = double.MaxValue;
        public const double MinProductWidth = 0.1;
        public const double MaxProductWidth = double.MaxValue;
        public const double MinAccessoryReplacementRate = 0;
        public const double MaxAccessoryReplacementRate = double.MaxValue;

        public const int MinAccessoryRecommendedQuantity = 0;
        public const int MaxAccessoryRecommendedQuantity = int.MaxValue;

        public const int ProductCharacteristicMinLength = 1;
        public const int ProductCharacteristicMaxLength = 200;

        public const int ProductImageCaptionMinLength = 1;
        public const int ProductImageCaptionMaxLength = 40;

        public const int SuperceedChangeDateMaxLength = 30;
        public const int NagsCodeMaxLength = 30;
        public const int ScanCodeMaxLength = 30;

        public const string AGCGroup = "AGC";
        public const string FuyaoGroup = "Fuyao";
        public const string LamexGroup = "Lamex";
        public const string NordglassGroup = "Nordglass";
        public const string PilkingtonGroup = "Pilkington";
        public const string SaintGobainGroup = "Saint-Gobain";
        public const string SharedGroup = "Общи";
        public const string YesglassGroup = "Yesglass";

        public const string NoGlassPosition = "NA";
        public static HashSet<string> GlassPositions = new HashSet<string>
        {
            "LFQ", "LFV", "LFD", "LMQ", "LRD", "LRV", "LRQ",
            "L FQ", "L FV", "L FD", "L MQ", "L RD", "L RV", "L RQ",
            "WS", "ROOF", "BL",
            "RFQ", "RFV", "RFD", "RMQ", "RRD", "RRV", "RRQ",
            "R FQ", "R FV", "R FD", "R MQ", "R RD", "R RV", "R RQ",
        };

        public static HashSet<string> HighCostGroups = new HashSet<string>
        {
            "Pilkington", "Saint-Gobain"
        };
    }
}
