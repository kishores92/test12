using GreenPOS.Common;
using GreenPOS.Interfaces;
using GreenPOS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GreenPOS.Entity;
using AutoMapper;
using GreenPOS.Context;
using Microsoft.Extensions.Options;

namespace GreenPOS.Service
{
    public class ContactService : IContactService
    {

        private readonly GreenPOSDBContext _ctx;
        private readonly IRepository<Contact> _repository;
        private readonly IRepository<Notes> _notesRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IMapper _mapper;

        public ContactService(GreenPOSDBContext ctx, IRepository<Contact> repository, IRepository<Notes> notesRepository
            , IRepository<User> userRepository, IMapper mapper)
        {
            _ctx = ctx;
            _repository = repository;
            _notesRepository = notesRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserViewModel>> GetUsersAsync(long loggedinUserId)
        {
            var mList = await _userRepository.FindByAsync(a => a.IsActive);

            if (mList.Any())
            {
                var list = mList.Select(a => _mapper.Map<UserViewModel>(a));
                return list;
            }

            return Enumerable.Empty<UserViewModel>();
        }
        public async Task<IEnumerable<ContactViewModel>> GetContactsAsync(long loggedinUserId)
        {
            var mList = await _repository.FindByAsync(a => a.IsActive);

            if (mList.Any())
                return mList.Select(a => _mapper.Map<ContactViewModel>(a));

            return Enumerable.Empty<ContactViewModel>();
        }

        public async Task<ContactViewModel> GetContactAsync(long id, long loggedinUserId)
        {
            var m = await _repository.FirstOrDefaultAsync(a => a.Id == id);

            if (m != null)
            {
                var vm = _mapper.Map<ContactViewModel>(m);
                var mNotes = await _notesRepository.FindAllAsync(a => a.ContactId == id && a.IsActive);

                if (mNotes != null && mNotes.Count > 0)
                    vm.Notes = mNotes.Select(a => _mapper.Map<NotesViewModel>(a)).ToList();

                return vm;
            }
            return null;
        }

        public async Task<long> SaveContactAsync(ContactViewModel vm, long loggedinUserId)
        {
            var m = _mapper.Map<Contact>(vm);
            m.IsActive = true;

            //---User Add / Update Section ----------------
            if (m.Id > 0)
            {
                var current = await _repository.FirstOrDefaultAsync(a => a.Id == m.Id);
                m.CreatedBy = current.CreatedBy;
                m.CreatedOn = current.CreatedOn;
                m.ModifiedBy = loggedinUserId;
                m.ModifiedOn = CommonHelper.CurrentDateTime;
                await _repository.UpdateAsync(m, m.Id);
            }
            else
            {
                m.CreatedBy = loggedinUserId;
                m.CreatedOn = CommonHelper.CurrentDateTime;
                await _repository.AddAsync(m);
            }
            //---User Add / Update Section ----------------

            if (vm.Notes != null && vm.Notes.Any())
            {
                var notes = vm.Notes.Select(a => _mapper.Map<Notes>(a)).ToList();
                foreach (var n in notes)
                {
                    if (n.Id == 0)
                    {
                        n.CreatedBy = loggedinUserId;
                        n.CreatedOn = CommonHelper.CurrentDateTime;
                        await _notesRepository.AddAsync(n);
                    }
                    else
                    {
                        n.ModifiedBy = loggedinUserId;
                        n.ModifiedOn = CommonHelper.CurrentDateTime;
                        await _notesRepository.UpdateAsync(n, n.Id);
                    }
                }
            }
            return m.Id;
        }

        public async Task<long> DeleteContactAsync(long id, long loggedinUserId)
        {
            var result = -1;
            var m = await _repository.FirstOrDefaultAsync(a => a.Id == id);
            if (m != null)
            {
                var mNotes = await _notesRepository.FindAllAsync(a => a.ContactId == id && a.IsActive);
                foreach (var item in mNotes)
                {
                    item.IsActive = false;
                    item.ModifiedBy = loggedinUserId;
                    item.ModifiedOn = CommonHelper.CurrentDateTime;
                    result = await _notesRepository.UpdateAsync(item, item.Id);
                }

                m.ModifiedBy = loggedinUserId;
                m.ModifiedOn = CommonHelper.CurrentDateTime;
                m.IsActive = false;
                result = await _repository.UpdateAsync(m, m.Id);

            }
            return result >= 0 ? m.Id : result;
        }
    }
}
