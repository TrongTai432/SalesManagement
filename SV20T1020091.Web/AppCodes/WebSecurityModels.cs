using Microsoft.AspNetCore.Authentication.Cookies;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Security.Claims;
using System.Data;
namespace SV20T1020091.Web.AppCodes
{
    public class WebSecurityModels
    {
        /// &lt;summary&gt;
        /// Thông tin tài khoản người dùng được lưu trong phiên đăng nhập
        /// &lt;/summary&gt;
        public class WebUserData
        {
            public string? UserId { get; set; }
            public string? UserName { get; set; }
            public string? DisplayName { get; set; }
            public string? Email { get; set; }
            public string? Photo { get; set; }
            public string? ClientIP { get; set; }
            public string? SessionId { get; set; }
            public string? AdditionalData { get; set; }
            public List<string> Roles { get; set; }
            /// &lt;summary&gt;
            /// Thông tin người dùng dưới dạng danh sách các Claim
            /// &lt;/summary&gt;
            /// &lt;returns&gt;&lt;/returns&gt;
            private List<Claim> Claims
            {
                get
                {
                    List<Claim> claims = new List<Claim>()
                {
                    new Claim(nameof(UserId), UserId ?? ""),
                    new Claim(nameof(UserName), UserName ?? ""),
                    new Claim(nameof(DisplayName), DisplayName ?? ""),
                    new Claim(nameof(Email), Email ?? ""),
                    new Claim(nameof(Photo), Photo ?? ""),
                    new Claim(nameof(ClientIP), ClientIP ?? ""),
                    new Claim(nameof(SessionId), SessionId ?? ""),
                    new Claim(nameof(AdditionalData), AdditionalData ?? "")

                };
                    if (Roles != null)
                        foreach (var role in Roles)
                            claims.Add(new Claim(ClaimTypes.Role, role));
                    return claims;
                }
            }
            /// &lt;summary&gt;
            /// Tạo Pricipal dựa trên thông tin của người dùng
            /// &lt;/summary&gt;
            /// &lt;returns&gt;&lt;/returns&gt;
            public ClaimsPrincipal CreatePrincipal()
            {
                var claimIdentity = new ClaimsIdentity(Claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var claimPrincipal = new ClaimsPrincipal(claimIdentity);
                return claimPrincipal;
            }
        }
        /// &lt;summary&gt;
        /// Thông tin về nhóm/quyền
        /// &lt;/summary&gt;
        public class WebUserRole
        {
            /// &lt;summary&gt;
            /// Ctor
            /// &lt;/summary&gt;
            /// &lt;param name=&quot;name&quot;&gt;Tên/ký hiệu nhóm/quyền&lt;/param&gt;
            /// &lt;param name=&quot;description&quot;&gt;Mô tả&lt;/param&gt;
            public WebUserRole(string name, string description)
            {
                Name = name;
                Description = description;
            }
            /// &lt;summary&gt;
            /// Tên/Ký hiệu quyền
            /// &lt;/summary&gt;
            public string Name { get; set; }
            /// &lt;summary&gt;
            /// Mô tả
            /// &lt;/summary&gt;
            public string Description { get; set; }

        }
        /// &lt;summary&gt;
        /// Danh sách các nhóm quyền sử dụng trong ứng dụng
        /// &lt;/summary&gt;
        public class WebUserRoles
        {
            /// &lt;summary&gt;
            /// Lấy danh sách thông tin các Role dựa vào các hằng được định nghĩa trong lớp này
            /// &lt;/summary&gt;
            public static List<WebUserRole> ListOfRoles
            {
                get
                {
                    List<WebUserRole> listOfRoles = new List<WebUserRole>();
                    Type type = typeof(WebUserRoles);
                    var listFields = type.GetFields(BindingFlags.Public
                    | BindingFlags.Static
                    | BindingFlags.FlattenHierarchy)
                    .Where(fi => fi.IsLiteral && !fi.IsInitOnly && fi.FieldType == typeof(string));
                    foreach (var role in listFields)
                    {
                        string? roleName = role.GetRawConstantValue() as string;
                        if (roleName != null)
                        {
                            DisplayAttribute? attribute = role.GetCustomAttribute<DisplayAttribute>();
                            if (attribute != null)
                                listOfRoles.Add(new WebUserRole(roleName, attribute.Name ?? roleName));
                            else
                                listOfRoles.Add(new WebUserRole(roleName, roleName));
                        }
                    }
                    return listOfRoles;
                }
            }
            //TODO: Định nghĩa các role được sử dụng trong hệ thống tại đây
            [Display(Name = "Quản trị hệ thống")]
            public const string Administrator = "admin";
            [Display(Name = "Nhân viên")]
            public const string Employee = "employee";
            [Display(Name = "Khách hàng")]

            public const string Customer = " customer ";
        }
        /// &lt;summary&gt;
        /// Extension các phương thức cho các đối tượng liên quan đến xác thực tài khoản người dùng
        /// &lt;/summary&gt;
        public static class WebUserExtensions
        {
            /// &lt;summary&gt;
            ///
            /// &lt;/summary&gt;
            /// &lt;param name=&quot;principal&quot;&gt;&lt;/param&gt;
            /// &lt;returns&gt;&lt;/returns&gt;
            /*public static WebUserData? GetUserData(this ClaimsPrincipal principal)
            {
                try
                {
                    if (principal == null || principal.Identity == null || !principal.Identity.IsAuthenticated)
                        return null;
                    var userData = new WebUserData();
                    userData.UserId = principal.FindFirstValue(nameof(userData.UserId));
                    userData.UserName = principal.FindFirstValue(nameof(userData.UserName));
                    userData.DisplayName = principal.FindFirstValue(nameof(userData.DisplayName));
                    userData.Email = principal.FindFirstValue(nameof(userData.Email));
                    userData.Photo = principal.FindFirstValue(nameof(userData.Photo));
                    userData.ClientIP = principal.FindFirstValue(nameof(userData.ClientIP));
                    userData.SessionId = principal.FindFirstValue(nameof(userData.SessionId));
                    userData.AdditionalData = principal.FindFirstValue(nameof(userData.AdditionalData));
                    userData.Roles = new List<string>();
                    foreach (var claim in principal.FindAll(ClaimTypes.Role))
                    {
                        userData.Roles.Add(claim.Value);
                    }
                    return userData;
                }
                catch
                {

                    return null;
                }
            }*/
        }
    }
}
