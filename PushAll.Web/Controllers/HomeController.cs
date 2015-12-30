using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using PushAll.Web.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.Mvc.Filters;

namespace PushAll.Web.Controllers
{
	[RequireHttps]
	public class HomeController : Controller
	{
		private static bool _storeTagImages;
		private static object _lock = new object();
		private static string _pushallKey;

		static HomeController()
		{
			AppSettingsReader appSettingsReader = new AppSettingsReader();

			_pushallKey = (string)appSettingsReader.GetValue("PushAllKey", typeof(string));


			bool parse;
			_storeTagImages = bool.TryParse((string)appSettingsReader.GetValue("StoreTagImages", typeof(string)), out parse) ? parse : false;
		}

		protected override void OnAuthentication(AuthenticationContext filterContext)
		{
			base.OnAuthentication(filterContext);
		}

		protected override void OnAuthorization(AuthorizationContext filterContext)
		{
			base.OnAuthorization(filterContext);
		}

		protected override void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
		{
			base.OnAuthenticationChallenge(filterContext);
		}

		protected override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			base.OnActionExecuting(filterContext);

			string cookieName = "pushalluser";

			string resultPushallUserId = null;

			var cookie = Request.Cookies[cookieName];

			if (cookie != null)
			{
				string encTicket = cookie.Value;
				FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(encTicket);
				resultPushallUserId = ticket.UserData;
			}

			string pushallUserId = Request.Params["pushalluserid"];

			if (!string.IsNullOrWhiteSpace(pushallUserId))
			{
				string remoteAddr = Request.Params["REMOTE_ADDR"];
				string time = Request.Params["time"];
				string sign = Request.Params["sign"];

#if !DEBUG
				if (string.Equals(sign, CalculateMD5Hash(_pushallKey + pushallUserId + time + remoteAddr), StringComparison.InvariantCultureIgnoreCase))
#endif
				{
					resultPushallUserId = pushallUserId;
				}
			}

			if (!string.IsNullOrWhiteSpace(resultPushallUserId))
			{
				if (User.Identity.IsAuthenticated)
				{
					var userId = User.Identity.GetUserId();
					using (PushAllEntities en = new PushAllEntities())
					{
						var pushUser = en.PushAllUsers.SingleOrDefault(x => x.UserId == userId);
						if (pushUser == null)
						{
							en.PushAllUsers.Add(new PushAllUser { UserId = userId, PushAllUserId = resultPushallUserId });
						}
						else
						{
							pushUser.PushAllUserId = resultPushallUserId;
						}
						en.SaveChanges();
					}

					if (Response.Cookies[cookieName] != null)
					{
						Response.Cookies[cookieName].Expires = DateTime.Now.AddYears(-100);
					}
				}
				else
				{
					FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1,
						cookieName,
						DateTime.Now,
						DateTime.Now.AddMinutes(90),
						true,
						resultPushallUserId,
						FormsAuthentication.FormsCookiePath);

					string encTicket1 = FormsAuthentication.Encrypt(ticket);

					Response.Cookies.Add(new HttpCookie(cookieName, encTicket1) { HttpOnly = true });
				}
			}
		}

		private string CalculateMD5Hash(string input)
		{
			// step 1, calculate MD5 hash from input
			MD5 md5 = System.Security.Cryptography.MD5.Create();
			byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
			byte[] hash = md5.ComputeHash(inputBytes);

			// step 2, convert byte array to hex string
			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < hash.Length; i++)
			{
				sb.Append(hash[i].ToString("X2"));
			}
			return sb.ToString();
		}

		public ActionResult Index()
		{
			ViewBag.RemoteAddr = Request.Params["REMOTE_ADDR"];
			var userId = User.Identity.GetUserId();

			List<GetTags_Result> tags = null;

			if (_storeTagImages)
			{
				lock (_lock)
				{
					using (PushAllEntities en = new PushAllEntities())
					{
						tags = en.GetTags(userId, _storeTagImages).OrderBy(x => x.Name).ToList();
					}

					if (_storeTagImages)
					{
						if (!System.IO.Directory.Exists(System.Web.HttpContext.Current.Request.MapPath("~\\Content\\ico\\")))
						{
							System.IO.Directory.CreateDirectory(System.Web.HttpContext.Current.Request.MapPath("~\\Content\\ico\\"));
						}

						foreach (var tag in tags.Where(x => x.Image != null))
						{
							System.IO.File.WriteAllBytes(System.Web.HttpContext.Current.Request.MapPath("~\\Content\\ico\\tag" + tag.TagId + ".jpeg"), tag.Image);
						}
					}
					_storeTagImages = false;
				}
			}
			else
			{
				using (PushAllEntities en = new PushAllEntities())
				{
					tags = en.GetTags(userId, false).OrderBy(x => x.Name).ToList();
				}
			}


			var top = Split(tags.OrderByDescending(x => x.IsSubscribed).ThenByDescending(x => x.Subscribers).Take(20).ToList(), 4);
			var another = Split(tags.OrderByDescending(x => x.IsSubscribed).ThenByDescending(x => x.Subscribers).Skip(20).OrderBy(x => x.Name).ToList(), 4);

			top.AddRange(another);

			foreach (var tag in tags.Where(x => x.Image != null))
			{
				System.IO.File.WriteAllBytes(System.Web.HttpContext.Current.Request.MapPath("~\\Content\\ico\\tag" + tag.TagId + ".jpeg"), tag.Image);
			}

			return View(top);
		}

		public static List<List<GetTags_Result>> Split(List<GetTags_Result> source, int size)
		{
			return source
					.Select((x, i) => new { Index = i, Value = x })
					.GroupBy(x => x.Index / size)
					.Select(x => x.Select(v => v.Value).ToList())
					.ToList();
		}

		public ActionResult Subscribe(int id, bool selected)
		{
			var userId = User.Identity.GetUserId();

			using (PushAllEntities en = new PushAllEntities())
			{
				var userTag = en.UserTags.SingleOrDefault(x => x.TagId == id && x.UserId == userId);
				if (selected)
				{
					if (userTag == null)
					{
						en.UserTags.Add(new UserTag { TagId = id, UserId = userId });
						en.SaveChanges();
					}
				}
				else
				{
					if (userTag != null)
					{
						en.UserTags.Remove(userTag);
						en.SaveChanges();
					}

				}
			}

			return Json(new { });
		}

	}
}