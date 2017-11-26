namespace GGG_OnlineShop.Common
{
    public class GlobalConstants
    {
        public const string AdministratorRoleName = "admin";

        public const char InterchangeablePartStringSeparator = ';';
        public const int InterchangeableEurocodeMaxLength = 400;

        public const string MaxLengthErrorMessage = "{0}: max length: {1}";
        public const string MinAndMaxLengthErrorMessage = "{0}: min length:{2}, max length: {1}";

        public const double MinPercentPaidPrice = 0.45;
        public const double MinPrice = 0;
        public const double MaxPrice = double.MaxValue;

        public const int FullAddressMaxLength = 400;
        public const int ManufacturerMaxLength = 30;
        public const int OtherCodesMaxLength = 200;
       
        public const int AnonymousUserЕmailMaxLength = 40;
        public const int AnonymousUserInfoMaxLength = 200;
        public const int BulstatMaxLength = 50;
        public const int BulstatMinLength = 2;
        public const int CompanyNameMaxLength = 50;
        public const int CompanyNameMinLength = 2;
        public const int DeliveryNotesMaxLength = 600;
        public const int DeliveryCountryMaxLength = 50;
        public const int DeliveryCountryMinLength = 2;
        public const int DeliveryTownMaxLength = 50;
        public const int DeliveryTownMinLength = 2;
        public const int DeliveryAddressMaxLength = 200;
        public const int DeliveryAddressMinLength = 2;
        public const double MinPercentageReduction = 0;
        public const double MaxPercentageReduction = 100;
        public const int PasswordMaxLength = 100;
        public const int PasswordMinLength = 4;
        public const int PhoneNumberMaxLength = 30;
        public const int PhoneNumberMinLength = 3;

        public const int BodyTypeCodeMinLength = 1;
        public const int BodyTypeCodeMaxLength = 10;
        public const int BodyTypeDescriptionMaxLength = 100;
        public const int MakeNameMaxLength = 100;
        public const int MakeNameMinLength = 1;
        public const int ModelNameMaxLength = 100;
        public const int ModelNameMinLength = 1;

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
    }
}
