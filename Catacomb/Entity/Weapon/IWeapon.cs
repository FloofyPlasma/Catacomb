using Catacomb.Screen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catacomb.Entity.Weapon
{
    public interface IWeapon
    {
        void SetOwner(Mob.Mob mob);
        void WeaponTick();
        void UpgradeWeapon();
        void PrimaryFire(double xDir, double yDir);
        Bullet GetAmmo(double xDir, double yDir);
        public IAbstractBitmap GetSprite();
        void SetWeaponMode();
    }
}
