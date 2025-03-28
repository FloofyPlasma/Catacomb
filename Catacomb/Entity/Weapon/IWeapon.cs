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
        abstract void SetOwner(Mob.Mob mob);
        abstract void WeaponTick();
        abstract void UpgradeWeapon();
        abstract void PrimaryFire(double xDir, double yDir);
        abstract Bullet GetAmmo(double xDir, double yDir);
        abstract public IAbstractBitmap GetSprite();
        abstract void SetWeaponMode();
    }
}
