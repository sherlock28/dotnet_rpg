using System.Threading.Tasks;
using dotnet_rpg.Dtos.Character;
using dotnet_rpg.Dtos.Weapon;

namespace dotnet_rpg.Services.WeaponService
{
    public interface IWeaponService
    {
        Task<Models.ServiceResponse<GetCharacterDto>> AddWeapon(AddWeaponDto newWeapon);
    }
}