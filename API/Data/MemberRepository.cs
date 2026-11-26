using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class MemberRepository(AppDbContext context) : IMemberRepository
    {
        public async Task<Member?> GetMemberByIdAsync(string id)
        {
          return  await context.Memebers
          .FindAsync(id);
        }

        public async Task<IReadOnlyList<Member>> GetMembersAsync()
        {
            return  await context.Memebers.ToListAsync();
        }

        public async Task<IReadOnlyList<Photo>> GetPotosForMemberAsync(string MemberId)
        {
           return  await context.Memebers
           .Where(x=> x.Id==MemberId)
           .SelectMany(x=>x.Photos)
           .ToListAsync();
        }

        public async Task<bool> SaveAllAsync()
        {
          return  await context.SaveChangesAsync() > 0;
        }

        public  void Update(Member member)
        {
            context.Entry(member).State=EntityState.Modified;
        }
    }
}