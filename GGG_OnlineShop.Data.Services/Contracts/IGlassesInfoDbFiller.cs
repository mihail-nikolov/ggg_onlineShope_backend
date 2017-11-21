namespace GGG_OnlineShop.Data.Services.Contracts
{
    using JsonParseModels;
    using System.Collections.Generic;

    public interface IGlassesInfoDbFiller
    {
        void FillInfo(IList<GlassJsonInfoModel> glasses, string passedFile = "");
    }
}
