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
using System.Text;

namespace GreenPOS.Service
{

    public class RoleService : IRoleService
    {
        private readonly GreenPOSDBContext _ctx;
        private readonly IRepository<Role> _repository;
        private readonly IRepository<ScreenPermission> _screenPermissionRepository;
        private readonly IMapper _mapper;

        public RoleService(IRepository<Role> repository, IRepository<ScreenPermission> screenPermissionRepository, IMapper mapper, GreenPOSDBContext ctx)
        {
            _repository = repository;
            _screenPermissionRepository = screenPermissionRepository;
            _mapper = mapper;
            _ctx = ctx;
        }
        public async Task<long> SaveRoleAsync(RoleViewModel vm, long loggedinUserId)
        {
            if (vm.PermissionRoleIds.Length > 1)
            {
                StringBuilder builder = new StringBuilder();
                foreach (string value in vm.PermissionRoleIds)
                {
                    builder.Append(value);
                    builder.Append(',');
                }
                string values = builder.ToString();
                vm.PermissionIds = values.Substring(0, values.Length - 1);
            }
            else
            {
                string values = vm.PermissionRoleIds[0].ToString();
                vm.PermissionIds = values;
            }
            var m = _mapper.Map<Role>(vm);
            if (m.Id > 0)
            {
                var current = await _repository.FirstOrDefaultAsync(a => a.Id == m.Id);
                if (current != null)
                {
                    current.Name = m.Name;
                    current.PermissionIds = m.PermissionIds;
                    current.ModifiedBy = loggedinUserId;
                    current.ModifiedOn = DateTime.UtcNow;
                    await _repository.UpdateAsync(current, current.Id);
                }
            }
            else
            {
                m.CreatedBy = loggedinUserId;
                m.CreatedOn = CommonHelper.CurrentDateTime;
                m.IsActive = true;
                await _repository.AddAsync(m);
            }

            return m.Id;
        }

        public async Task<RoleViewModel> GetRoleByIdAsync(long id, long loggedinUserId)
        {
            var m = await _repository.FirstOrDefaultAsync(a => a.Id == id);

            if (m != null)
            {
                var vm = _mapper.Map<RoleViewModel>(m);
                vm.PermissionRoleIds = vm.PermissionIds.Split(",");
                return vm;
            }

            return Enumerable.Empty<RoleViewModel>().FirstOrDefault();
        }

        public async Task<long> DeleteRoleByIdAsync(long id, long loggedinUserId)
        {
            var current = await _repository.GetAsync(id);
            if (current != null)
            {
                current.IsActive = false;
                current.ModifiedBy = loggedinUserId;
                current.ModifiedOn = DateTime.UtcNow;
                await _repository.UpdateAsync(current, current.Id);
                return id;
            }

            return 0;
        }

        public async Task<IEnumerable<RoleViewModel>> GetRolesAsync(long loggedinUserId)
        {
            var list = await _repository.FindByAsync(a => a.IsActive);
            if (list.Any())
            {
                var vmList = list.Select(a => _mapper.Map<RoleViewModel>(a)).ToList();
                foreach (var vm in vmList)
                {
                    if (!string.IsNullOrEmpty(vm.PermissionIds))
                    {
                        var values = vm.PermissionIds.Split(',').Where(a => !string.IsNullOrEmpty(a)).Select(s => int.Parse(s));
                        foreach (var id in values)
                        {
                            var pModel = await _screenPermissionRepository.FirstOrDefaultAsync(a => a.Id == id);
                            if (pModel != null)
                                vm.PermissionNames = string.IsNullOrEmpty(vm.PermissionNames) ? pModel.Name : $"{vm.PermissionNames}, {pModel.Name}";
                        }
                    }
                }
                return vmList;
            }

            return Enumerable.Empty<RoleViewModel>();
        }

        public async Task<IEnumerable<SelectListItem>> GetRolePermissionsAsync(long loggedinUserId)
        {
            var list = await _screenPermissionRepository.FindByAsync(a => a.IsActive);

            if (list.Any())
                return list.Select(a => new SelectListItem { Text = a.Name, Value = a.Id }).OrderBy(n => n.Text).ToList();

            return Enumerable.Empty<SelectListItem>();
        }
    }
}
