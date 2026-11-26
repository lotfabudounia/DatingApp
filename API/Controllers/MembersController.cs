using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class MembersController(IMemberRepository memberRepository) : BaseApiController
    {
                 
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Member>>> GetMembers()
        { 
            return Ok(await memberRepository.GetMembersAsync()) ;             
        }

      
        [HttpGet("{Id}")]
        public async Task<ActionResult<Member>> GetMember(string id)
        {
            var member = await memberRepository.GetMemberByIdAsync(id);

            if (member==null) return NotFound();

            return member;
            
        }

        [HttpGet("{Id}/Photos")]
        public async Task<ActionResult<Member>> GetMemberPhotos(string id)
        {
             return Ok(await memberRepository.GetPotosForMemberAsync(id)); 
        }
         
        
    }
}