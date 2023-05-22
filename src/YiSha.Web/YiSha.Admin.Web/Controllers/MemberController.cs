using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using YiSha.Business.OrganizationManage;
using YiSha.Business.SystemManage;
using YiSha.Entity.SystemManage;
using YiSha.Enum;
using YiSha.IdGenerator;
using YiSha.Model.Result;
using YiSha.Util.Extension;
using YiSha.Web.Code;
using YiSha.Util.Model;
using YiSha.Util;
using YiSha.Entity.OrganizationManage;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using YiSha.Service.SystemManage;
using YiSha.Service.Cache;
using YiSha.Service.OrganizationManage;

namespace YiSha.Admin.Web.Controllers
{
    public class MemberController : BaseController
    {
        private MenuBLL menuBLL = new MenuBLL();
        private UserBLL userBLL = new UserBLL();
        private LogLoginBLL logLoginBLL = new LogLoginBLL();
        private MenuAuthorizeBLL menuAuthorizeBLL = new MenuAuthorizeBLL();

        #region 首页
        [HttpGet]
        [AuthorizeFilter]
        public async Task<IActionResult> Index()
        {
            OperatorInfo operatorInfo = await Operator.Instance.Current();

            TData<List<MenuEntity>> objMenu = null;
            objMenu = await menuBLL.GetMenuListForEdit();

            List<MenuEntity> menuList = objMenu.Result;
            menuList = menuList.Where(p => p.MenuStatus == StatusEnum.Yes.ParseToInt()).ToList();

            if (operatorInfo.IsSystem != 1)
            {
                TData<List<MenuAuthorizeInfo>> objMenuAuthorize = await menuAuthorizeBLL.GetAuthorizeList(operatorInfo);
                List<long?> authorizeMenuIdList = objMenuAuthorize.Result.Select(p => p.MenuId).ToList();
                menuList = menuList.Where(p => authorizeMenuIdList.Contains(p.Id)).ToList();
            }

            var configModel = await (new ConfigCache()).GetConfigModel();
            ViewBag.Title = configModel.CorporateName;

            ViewBag.MenuList = menuList;
            ViewBag.OperatorInfo = operatorInfo;
            return View();
        }
        #endregion

        #region Welcome NoPermission Error Skin
        [HttpGet]
        public IActionResult Welcome()
        {
            return View();
        }

        [HttpGet]
        public IActionResult NoPermission()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Error(string message)
        {
            ViewBag.Message = message;
            return View();
        }

        [HttpGet]
        public IActionResult Skin()
        {
            return View();
        }
        #endregion

        #region 登录
        [HttpGet]
        public async Task<IActionResult> Login()
        {
            OperatorInfo operatorInfo = await Operator.Instance.Current();
            if (operatorInfo != null && operatorInfo.UserId.HasValue)
            {
                return this.Redirect("Index");
            }

            if (GlobalContext.SystemConfig.Demo)
            {
                ViewBag.UserName = "demo";
                ViewBag.Password = "123456";
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LoginJson(string userName, string password, string captchaCode)
        {
            TData obj = new TData();
            if (string.IsNullOrEmpty(captchaCode))
            {
                obj.Message = "验证码不能为空";
                return Json(obj);
            }
            if (captchaCode != new SessionHelper().GetSession("CaptchaCode").ParseToString())
            {
                obj.Message = "验证码错误，请重新输入";
                return Json(obj);
            }

            TData<UserEntity> userObj = await userBLL.CheckLogin(userName, password, (int)PlatformEnum.Web, AppTypeEnumType.Web);
            if (userObj.Status)
            {
                var userEntity = userObj.Result;

                var userTokenService = new UserTokenService();
                await userTokenService.AddOrUpdate(userEntity.Id, userEntity.UserToken, AppTypeEnumType.Web, "", "");

                await Operator.Instance.AddCurrent(userEntity.UserToken,false);
            }
            
            await AddLoginLog(userObj);

            obj.Status = userObj.Status;
            obj.Message = userObj.Message;
            return Json(obj);
        }

        private async Task AddLoginLog(TData<UserEntity> userObj)
        {
            string ip = NetHelper.Ip;
            string browser = NetHelper.Browser;
            string os = NetHelper.GetOSVersion();
            string userAgent = NetHelper.UserAgent;

            LogLoginEntity logLoginEntity = new LogLoginEntity
            {
                LogStatus = userObj.Status ? OperateStatusEnum.Success.ParseToInt() : OperateStatusEnum.Fail.ParseToInt(),
                Remark = userObj.Message,
                IpAddress = ip,
                IpLocation = IpLocationHelper.GetIpLocation(ip),
                Browser = browser,
                OS = os,
                ExtraRemark = userAgent,
                BaseCreatorId = userObj.Result?.Id
            };

            // 让底层不用获取HttpContext
            logLoginEntity.BaseCreatorId = logLoginEntity.BaseCreatorId ?? 0;
        

            await logLoginBLL.SaveForm(logLoginEntity);

            //AsyncTaskHelper.StartTask(userObj, async (x) =>
            //{
            //    string ip = NetHelper.Ip;
            //    string browser = NetHelper.Browser;
            //    string os = NetHelper.GetOSVersion();
            //    string userAgent = NetHelper.UserAgent;

            //    LogLoginEntity logLoginEntity = new LogLoginEntity
            //    {
            //        LogStatus = x.Status ? OperateStatusEnum.Success.ParseToInt() : OperateStatusEnum.Fail.ParseToInt(),
            //        Remark = x.Message,
            //        IpAddress = ip,
            //        IpLocation = IpLocationHelper.GetIpLocation(ip),
            //        Browser = browser,
            //        OS = os,
            //        ExtraRemark = userAgent,
            //        BaseCreatorId = x.Result?.Id
            //    };

            //    // 让底层不用获取HttpContext
            //    logLoginEntity.BaseCreatorId = logLoginEntity.BaseCreatorId ?? 0;
            //    logLoginEntity.TenantId = logLoginEntity.TenantId ?? 0;
            //    logLoginEntity.VerifyTenantId = false;

            //    await logLoginBLL.SaveForm(logLoginEntity);
            //});

        }
        #endregion

        #region 退出
        [HttpPost]
        public async Task<IActionResult> LoginOffJson()
        {
            OperatorInfo user = await Operator.Instance.Current();
            if (user != null)
            {
                #region 退出系统
                // 如果不允许同一个用户多次登录，当用户登出的时候，就不在线了
                if (!GlobalContext.SystemConfig.LoginMultiple)
                {
                    await (new UserService()).Logoff(user.UserId);
                }

                // 登出日志
                await logLoginBLL.SaveForm(new LogLoginEntity
                {
                    LogStatus = OperateStatusEnum.Success.ParseToInt(),
                    Remark = "退出系统",
                    IpAddress = NetHelper.Ip,
                    IpLocation = string.Empty,
                    Browser = NetHelper.Browser,
                    OS = NetHelper.GetOSVersion(),
                    ExtraRemark = NetHelper.UserAgent,
                    BaseCreatorId = user.UserId
                });

                Operator.Instance.RemoveCurrent();
                new CookieHelper().RemoveCookie("RememberMe");

                return Json(new TData { Status = true });
                #endregion
            }
            else
            {
                throw new Exception("非法请求");
            }
        }
        #endregion

        

        #region 获取验证码
        public IActionResult GetCaptchaImage()
        {
            string sessionId = GlobalContext.ServiceProvider?.GetService<IHttpContextAccessor>().HttpContext.Session.Id;

            Tuple<string, int> captchaCode = CaptchaHelper.GetCaptchaCode();
            byte[] bytes = CaptchaHelper.CreateCaptchaImage(captchaCode.Item1);
            new SessionHelper().WriteSession("CaptchaCode", captchaCode.Item2);
            return File(bytes, @"image/jpeg");
        }
        #endregion


        #region 注册
        [HttpGet]
        public IActionResult Register()
        {
           
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RegisterJson(string userName, string password,
            string email,string mobile, string captchaCode)
        {
            TData obj = new TData();
            if (string.IsNullOrWhiteSpace(password))
            {
                obj.Message = "密码不能为空";
                return Json(obj);
            }

            if (string.IsNullOrEmpty(captchaCode))
            {
                obj.Message = "验证码不能为空";
                return Json(obj);
            }
            if (captchaCode != new SessionHelper().GetSession("CaptchaCode").ParseToString())
            {
                obj.Message = "验证码错误，请重新输入";
                return Json(obj);
            }

            try
            {
                var entity = new UserEntity();
                entity.UserName = userName;
                entity.Password = password;
                entity.Email = email;
                entity.Mobile = mobile;

                var ret = await userBLL.Register(entity);
                obj.Message = "注册成功";
                obj.Status = true;

                //注册成功之后，自动登录
                return await LoginJson(userName, password, captchaCode);
            }
            catch (Exception ex)
            {
                obj.Message = ex.Message;
                obj.Status = false;
            }
            return Json(obj);
        }
        #endregion

    
    }
}
