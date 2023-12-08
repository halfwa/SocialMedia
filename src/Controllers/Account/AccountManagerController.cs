using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApp.Data;
using WebApp.Data.Repository;
using WebApp.Data.UoW;
using WebApp.Models.Entities.Users;
using WebApp.Models.Users;
using WebApp.Models.ViewModels.Account;

namespace WebApp.Controllers.Account
{
    public class AccountManagerController : Controller
    {
        private readonly IMapper _mapper;

        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        private readonly IUnitOfWork _unitOfWork;

        public AccountManagerController(
            IMapper mapper,
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IUnitOfWork unitOfWork)
        {
            _mapper = mapper;

            _userManager = userManager;
            _signInManager = signInManager;

            _unitOfWork = unitOfWork;
        }

        #region[Authorization]

        [Route("Login")]
        [HttpGet]
        public IActionResult Login()
        {
            return RedirectToAction("Index", "Home"); ;
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        [Route("Login")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _mapper.Map<User>(model);

                var result = await _signInManager.PasswordSignInAsync(user.UserName, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("MyPage", "AccountManager");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Неправильный логин и (или) пароль");
                }
            }
            return RedirectToAction("Index", "Home");
        }

        [Route("Logout")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        #endregion

        #region[UserPage]  

        [Route("MyPage")]
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> MyPage()
        {
            var user = User;

            var result = await _userManager.GetUserAsync(user);

            var model = new UserViewModel(result);

            model.Friends = GetAllFriend(result);

            return View("User", model);
        }

        #endregion

        #region[UserEdit]

        [Route("Edit")]
        [HttpGet]
        [Authorize]
        public IActionResult Edit()
        {
            var user = User;

            var result = _userManager.GetUserAsync(user);

            var model = new UserEditViewModel()
            {
                UserId = result.Result.Id,
                BirthDate = result.Result.BirthDate
            };

            return View("Edit", model);
        }

        [Route("Update")]
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Update(UserEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(model.UserId);

                user.Convert(model);

                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("MyPage");
                }
                else
                {
                    return RedirectToAction("Edit");
                }
            }
            else
            {
                ModelState.AddModelError("", "Некорректные данные");
                return View("Edit", model);
            }
        }

        #endregion

        #region[SearchUsers]

        [HttpPost, Route("UserList")]
        public async Task<IActionResult> UserList(string search)
        {
            var model = await CreateSearch(search);
            return View("UserList", model);
        }

        private async Task<SearchViewModel> CreateSearch(string search)
        {
            var currentUser = User;
            var result = await _userManager.GetUserAsync(currentUser);

            var list = _userManager.Users.AsEnumerable().ToList();
            if (!string.IsNullOrEmpty(search))
            {
                list = list.Where(x => x.GetFullName().ToLower().Contains(search.ToLower())).ToList();
            }
            var withFriend = await GetAllFriend();

            var data = new List<UserWithFriendExt>();
            list.ForEach(x =>
            {
                var t = _mapper.Map<UserWithFriendExt>(x);
                t.IsFriendWithCurrent = withFriend.Where(y => y.Id == x.Id || x.Id == result.Id).Count() != 0;
                data.Add(t);
            });

            var model = new SearchViewModel()
            {
                UserList = data
            };

            return model;
        }

        #endregion

        #region[Friends]
        [Route("AddFriend")]
        [HttpPost]
        public async Task<IActionResult> AddFriend(string id)
        {
            var currentUser = User;

            var result = await _userManager.GetUserAsync(currentUser);

            var friend = await _userManager.FindByIdAsync(id);

            var repository = _unitOfWork.GetRepository<Friend>(true) as FriendsRepository;

            await repository.AddFriendAsync(result, friend);

            return RedirectToAction("MyPage", "AccountManager");
        }

        [Authorize]
        [Route("DeleteFriend")]
        [HttpPost]
        public async Task<IActionResult> DeleteFriend(string id)
        {
            var currentUser = User;

            var result = await _userManager.GetUserAsync(currentUser);

            var friend = await _userManager.FindByIdAsync(id);

            var repository = _unitOfWork.GetRepository<Friend>() as FriendsRepository;

            await repository.DeleteFriend(result, friend);

            return RedirectToAction("MyPage", "AccountManager");
        }

        private async Task<List<User>> GetAllFriend()
        {
            var user = User;

            var result = await _userManager.GetUserAsync(user);

            var repository = _unitOfWork.GetRepository<Friend>(true) as FriendsRepository;

            return repository.GetFriendsByUser(result);
        }
        
        private List<User> GetAllFriend(User user)
        {

            var repository = _unitOfWork.GetRepository<Friend>(true) as FriendsRepository;

            return repository.GetFriendsByUser(user);
        }
        #endregion

        #region[Chat]

        [Route("Chat")]
        [HttpPost]
        public async Task<IActionResult> Chat(string id)
        {
            var model = await GenerateChat(id);

            return View("Chat", model);
        }

        [Route("NewMessage")]
        [HttpPost]
        public async Task<IActionResult> NewMessage(string id, ChatViewModel chat)
        {
            var currentUser = User;

            var result = await _userManager.GetUserAsync(currentUser);
            var recipient = await _userManager.FindByIdAsync(id);

            var repository = _unitOfWork.GetRepository<Message>() as MessageRepository;

            var item = new Message()
            {
                Sender = result,
                Recipient = recipient,
                Text = chat.NewMessage.Text,
            };
            await  repository.CreateAsync(item);

            var model = await GenerateChat(id);

            return View("Chat", model);
        }

        [Route("Chat")]
        [HttpGet]
        public async Task<IActionResult> Chat()
        {

            var id = Request.Query["id"];

            var model = await GenerateChat(id);
            return View("Chat", model);
        }

        private async Task<ChatViewModel> GenerateChat(string id)
        {
            var currentUser = User;

            var result = await _userManager.GetUserAsync(currentUser);
            var friend = await _userManager.FindByIdAsync(id);

            var repository = _unitOfWork.GetRepository<Message>() as MessageRepository;

            var mess = repository.GetMessages(result, friend);

            var model = new ChatViewModel()
            {
                You = result,
                ToWhom = friend,
                History = mess.OrderBy(x => x.Id).ToList(),
            };

            return model;
        }

        #endregion

        #region[GenerateUsers]

        [Route("Generate")]
        [HttpGet]
        public async Task<IActionResult> Generate()
        {
            var userGen = new GenerateUsers();
            var userList = userGen.Populate(35);

            foreach (var user in userList)
            {
                var result = await _userManager.CreateAsync(user, "qwerty");

                if (!result.Succeeded)
                    continue;
            }

            return RedirectToAction("Index", "Home");
        }

        #endregion
    }
}
