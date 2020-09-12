using AutoMapper;
using GreenPOS.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GreenPOS.Models;
using GreenPOS.Entity;
using GreenPOS.Context;
using GreenPOS.Common;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Options;

namespace GreenPOS.Service
{
    public class UserService : IUserService
    {
        private readonly GreenPOSDBContext _ctx;
        private readonly IRepository<User> _repository;
        private readonly IRepository<UserRole> _userRoleRepository;
        private readonly IRepository<Role> _rolerepository;
        private readonly IMapper _mapper;
        private readonly AppSettings _appSettings;

        public UserService(GreenPOSDBContext ctx, IRepository<User> repository, IRepository<UserRole> userRoleRepository, IRepository<Role> rolerepository, IMapper mapper, IOptions<AppSettings> appSettings)
        {
            _ctx = ctx;
            _repository = repository;
            _userRoleRepository = userRoleRepository;
            _rolerepository = rolerepository;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }

        public async Task<long> SaveUserAsync(UserViewModel vm, long loggedinUserId)
        {
            UserRole mUserRole = null;
            var m = _mapper.Map<User>(vm);

            //---User Add / Update Section ----------------
            if (m.Id > 0)
            {
                var current = await _repository.FirstOrDefaultAsync(a => a.Id == m.Id);
                current.ModifiedBy = loggedinUserId;
                current.ModifiedOn = DateTime.UtcNow;
                current.CompanyName = vm.CompanyName;
                current.Email = vm.Email;
                current.UserName = vm.UserName;
                await _repository.UpdateAsync(current, current.Id);
            }
            else
            {
                m.CreatedBy = loggedinUserId;
                m.CreatedOn = DateTime.UtcNow;
                m.IsActive = true;
                await _repository.AddAsync(m);
            }
            //---User Add / Update Section ----------------


            //---User Role Add / Update Section ----------------
            var exists = await _userRoleRepository.AnyAsync(a => a.UserId == m.Id && a.IsActive);
            if (!exists)
            {
                mUserRole = new UserRole
                {
                    UserId = m.Id,
                    RoleId = vm.RoleId,
                    IsActive = true,
                    CreatedOn = CommonHelper.CurrentDateTime,
                    CreatedBy = loggedinUserId
                };
                await _userRoleRepository.AddAsync(mUserRole);
            }
            else
            {
                mUserRole = await _userRoleRepository.FindAsync(a => a.UserId == m.Id && a.IsActive);
                mUserRole.RoleId = vm.RoleId;
                mUserRole.ModifiedBy = loggedinUserId;
                mUserRole.ModifiedOn = CommonHelper.CurrentDateTime;
                await _userRoleRepository.UpdateAsync(mUserRole, mUserRole.Id);
            }
            //---User Role Add / Update Section ----------------


            return m.Id;
        }

        public async Task<UserViewModel> GetUserAsync(long id, long loggedinUserId)
        {
            var m = await _repository.FirstOrDefaultAsync(a => a.Id == id);

            if (m != null)
            {
                var vm = _mapper.Map<UserViewModel>(m);
                var mUserRole = await _userRoleRepository.FirstOrDefaultAsync(a => a.UserId == id && a.IsActive);
                if (mUserRole != null)
                {
                    vm.RoleId = mUserRole.RoleId;
                    var selectedRole = _ctx.Role.FirstOrDefault(a => a.Id == mUserRole.RoleId);
                    if (selectedRole != null)
                        vm.RoleName =selectedRole.Name;
                }

                return vm;
            }

            return null;
        }

        public async Task<long> DeleteUserAsync(long id, long loggedinUserId)
        {
            var result = -1;
            var m = await _repository.FirstOrDefaultAsync(a => a.Id == id);
            if (m != null)
            {
                var mUserRole = await _userRoleRepository.FindAsync(a => a.UserId == id && a.IsActive);
                if (mUserRole != null)
                {
                    mUserRole.IsActive = false;
                    mUserRole.ModifiedBy = loggedinUserId;
                    mUserRole.ModifiedOn = CommonHelper.CurrentDateTime;
                    result = await _userRoleRepository.UpdateAsync(mUserRole, mUserRole.Id);
                }


                m.ModifiedBy = loggedinUserId;
                m.ModifiedOn = CommonHelper.CurrentDateTime;
                m.IsActive = false;
                result = await _repository.UpdateAsync(m, m.Id);

            }

            return result >= 0 ? m.Id : result;
        }


        public async Task<IEnumerable<UserViewModel>> GetUsersAsync(long loggedinUserId)
        {
            var mList = await _repository.FindByAsync(a => a.IsActive);

            if (mList.Any())
            {
                var list = mList.Select(a => _mapper.Map<UserViewModel>(a));
                List<UserViewModel> uvm = new List<UserViewModel>();
                foreach (var item in list)
                {
                    var mUserRole = await _userRoleRepository.FirstOrDefaultAsync(a => a.UserId == item.Id && a.IsActive);
                    if (mUserRole != null)
                    {
                        item.RoleId = mUserRole.RoleId;
                        var selectedRole = _ctx.Role.FirstOrDefault(a => a.Id == mUserRole.RoleId);
                        if (selectedRole != null)
                            item.RoleName = selectedRole.Name;                        
                    }
                    uvm.Add(item);
                }
                return uvm;
            }

            return Enumerable.Empty<UserViewModel>();
        }

        public async Task<IEnumerable<SelectListItem>> GetRolesAsync(long loggedinUserId)
        {
            var list = await _rolerepository.FindByAsync(a => a.IsActive);

            if (list.Any())
                return list.Select(a => new SelectListItem { Text = a.Name, Value = a.Id }).OrderBy(n => n.Text).ToList();

            return Enumerable.Empty<SelectListItem>();
        }

        public UserViewModel Authenticate(LoginViewModel login)
        {
            if (string.IsNullOrEmpty(login.Email) || string.IsNullOrEmpty(login.Password))
                return null;

            var uName = login.Email.ToLower().Trim();
            var m = _repository.FirstOrDefault(a => a.UserName.ToLower().Trim().Equals(uName) && a.IsActive);

            if (m != null && m.Password.Equals(login.Password))
            {
                var vm = _mapper.Map<UserViewModel>(m);

                // authentication successful so generate jwt token
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[] { new Claim(ClaimTypes.Name, vm.Id.ToString()) }),
                    Expires = DateTime.UtcNow.AddDays(7),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                vm.Token = tokenHandler.WriteToken(token);

                return vm;
            }
            return null;
        }
    }
}
