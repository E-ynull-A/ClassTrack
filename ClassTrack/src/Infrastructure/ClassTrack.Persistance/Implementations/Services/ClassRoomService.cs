using AutoMapper;
using ClassTrack.Application.DTOs;
using ClassTrack.Application.Interfaces.Repositories;
using ClassTrack.Application.Interfaces.Services;
using ClassTrack.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
namespace ClassTrack.Persistance.Implementations.Services
{
    internal class ClassRoomService : IClassRoomService
    {
        private readonly IClassRoomRepository _roomRepository;
        private readonly IMapper _mapper;

        public ClassRoomService(IClassRoomRepository roomRepository,
                                IMapper mapper)
        {
            _roomRepository = roomRepository;
            _mapper = mapper;
        }

        public async Task<ICollection<GetClassRoomItemDTO>> GetAllAsync(int page, int take)
        {
            return _mapper.Map<ICollection<GetClassRoomItemDTO>>
                                                    (await _roomRepository.GetAll(page: page,
                                                                                  take: take).ToListAsync());
        }

        public async Task<GetClassRoomDTO> GetByIdAsync(long id)
        {
            ClassRoom classRoom = await _roomRepository.GetByIdAsync(id);

            if (classRoom is null)
                throw new Exception("The Class Room isn't Found!");

            return _mapper.Map<GetClassRoomDTO>(classRoom);
        }
        public async Task CreateClassRoomAsync(PostClassRoomDTO postClass)
        {
            if (await _roomRepository.AnyAsync(r => r.Name == postClass.Name))
                throw new Exception("The Name has already used");

            _roomRepository.Add(new ClassRoom
            {
                Name = postClass.Name,
                PrivateKey = await _generateClassRoomKeyAsync()
            });

            await _roomRepository.SaveChangeAsync();
        }
        public async Task UpdateClassRoomAsync(long id,PutClassRoomDTO putClass)
        {
            if (await _roomRepository.AnyAsync(r => r.Name == putClass.Name))
                throw new Exception("The Name has already used");

            ClassRoom edited = await _roomRepository.GetByIdAsync(id);

            if (edited is null)
                throw new Exception("The ClassRoom isn't Found!");

            edited.Name = putClass.Name;

            _roomRepository.Update(edited);
            await _roomRepository.SaveChangeAsync();
        }
        public async Task DeleteClassRoomAsync(long id)
        {
            ClassRoom deleted = await _roomRepository.GetByIdAsync(id);

            if (deleted is null)
                throw new Exception("The ClassRoom isn't Found!");

            _roomRepository.Delete(deleted);
            await _roomRepository.SaveChangeAsync();
        }

        private async Task<string> _generateClassRoomKeyAsync()
        {
            char[] pool = "1234567890ABCDEFGHJKLMNPQRSTUVWXYZ".ToCharArray();
            string code;

            do
            {
                code = string.Empty;
                for (int i = 0; i < 8; i++)
                {
                    code += pool[Random.Shared.Next(pool.Length)];
                }
            }
            while (await _roomRepository.AnyAsync(r => r.PrivateKey == code));

            return code;
        }
    }
}
