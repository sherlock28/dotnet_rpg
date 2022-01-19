using System;
using System.Linq;
using System.Threading.Tasks;
using dotnet_rpg.Data;
using dotnet_rpg.Dtos.Fight;
using dotnet_rpg.Models;
using Microsoft.EntityFrameworkCore;

namespace dotnet_rpg.Services.FightService
{
    public class FightService : IFightService
    {
        private readonly DataContext _context;

        public FightService(DataContext context)
        {
            _context = context;
        }

        public async Task<ServiceResponse<AttackResultDto>> WeaponAttack(WeaponAttackDto request)
        {
            var serviceResponse = new ServiceResponse<AttackResultDto>();
            try
            {
                var attacker = await _context.Characters
                .Include(c => c.Weapon)
                .FirstOrDefaultAsync(c => c.Id == request.AttackerId);
                if (attacker == null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "Character not found.";
                    return serviceResponse;
                }
                var opponent = _context.Characters.FirstOrDefault(c => c.Id == request.OppenentId);
                if (opponent == null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "Character not found.";
                    return serviceResponse;
                }

                int damage = attacker.Weapon.Damage + (new Random().Next(attacker.Strength) / 2);
                damage -= (new Random().Next(opponent.Defence) / 2);

                if (damage > 0)
                {
                    opponent.HitPoints -= damage;
                }

                if(opponent.HitPoints <= 0)
                {
                    opponent.HitPoints = 0;
                    attacker.Victories++;
                    opponent.Defeats++;
                    serviceResponse.Success = false;
                    serviceResponse.Message = $"{opponent.Name} has been defeated!";
                }

                await _context.SaveChangesAsync();

                serviceResponse.Data = new AttackResultDto
                {
                    Attacker = attacker.Name,
                    AttackerHP = attacker.HitPoints,
                    Opponent = opponent.Name,
                    OpponentHP = opponent.HitPoints,
                    Damage = damage
                };      
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }
    
        public async Task<ServiceResponse<AttackResultDto>> SkillAttack(SkillAttackDto request)
        {
            var serviceResponse = new ServiceResponse<AttackResultDto>();
            try
            {
                var attacker = await _context.Characters
                .Include(c => c.Skills)
                .FirstOrDefaultAsync(c => c.Id == request.AttackerId);
                if (attacker == null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "Character not found.";
                    return serviceResponse;
                }
                var opponent = _context.Characters.FirstOrDefault(c => c.Id == request.OpponentId);
                if (opponent == null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "Character not found.";
                    return serviceResponse;
                }

                var skill = attacker.Skills.FirstOrDefault(s => s.Id == request.SkillId);
                if(skill == null) 
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = $"{attacker.Name} does not have this skill.";
                    return serviceResponse;
                } 

                int damage = skill.Damage + (new Random().Next(attacker.Intelligence) / 2);
                damage -= (new Random().Next(opponent.Defence) / 2);

                if (damage > 0)
                {
                    opponent.HitPoints -= damage;
                }

                if(opponent.HitPoints <= 0)
                {
                    opponent.HitPoints = 0;
                    attacker.Victories++;
                    opponent.Defeats++;
                    serviceResponse.Success = false;
                    serviceResponse.Message = $"{opponent.Name} has been defeated!";
                }

                await _context.SaveChangesAsync();

                serviceResponse.Data = new AttackResultDto
                {
                    Attacker = attacker.Name,
                    AttackerHP = attacker.HitPoints,
                    Opponent = opponent.Name,
                    OpponentHP = opponent.HitPoints,
                    Damage = damage
                };      
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }
    }
}