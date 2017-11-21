namespace GGG_OnlineShop.Web.Api.Areas.Administration.ViewModels.Users
{
    using System.ComponentModel.DataAnnotations;
    using Infrastructure;
    using InternalApiDB.Models;

    public class UserModel : IMapFrom<User>
    {
        public string Id { get; set; }

        [Required]
        [StringLength(20, ErrorMessage = "max len:{1}, min len: {2}.", MinimumLength = 3)]
        public string Email { get; set; }

        [Required]
        [StringLength(20, ErrorMessage = "max len:{1}, min len: {2}.", MinimumLength = 3)]
        public string Bulstat { get; set; }

        [Required]
        [StringLength(20, ErrorMessage = "max len:{1}, min len: {2}.", MinimumLength = 3)]
        public string CompanyName { get; set; }
    }
}