using Catacomb.Screen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catacomb.Entity.Weapon
{
    public class ElephantGun : IWeapon
    {
        protected Mob.Mob owner;
        protected static float BULLET_DAMAGE;

        private double accuracy;
        private int shootDelay = 120;

        private bool readyToShoot = true;
        private int currentShootDelay = 0;

        public ElephantGun(Mob.Mob mob)
        {
            SetOwner(mob);
            SetWeaponMode();

            if (mob.isSprint)
            {
                shootDelay *= 3;
            }
        }

        public void SetWeaponMode()
        {
            shootDelay = Constants.GetInt("shootDelay", this);

            if (Options.GetAsBoolean(Options.CREATIVE))
            {
                BULLET_DAMAGE = 100f;
                accuracy = 0;
            }
            else
            {
                BULLET_DAMAGE = Constants.GetFloat("bulletDamage", this);
                accuracy = Constants.GetFloat("accuracy", this);
            }
        }

        public void UpgradeWeapon()
        {

        }

        public void PrimaryFire(double xDir, double yDir)
        {
            if (readyToShoot)
            {
                double dir;

                if (owner.isSprint)
                {
                    dir = GetBulletDirection(accuracy * 2);
                }
                else
                {
                    dir = GetBulletDirection(accuracy);
                }

                Entity bullet = null;

                xDir = System.Math.Cos(dir);
                yDir = System.Math.Sin(dir);
                ApplyImpuls(xDir, yDir, 10);

                bullet = GetAmmo(xDir, yDir);

                owner.level.AddEntity(bullet);

                if (owner is Player)
                {
                    Player player = (Player)owner;
                    player.muzzleTicks = 3;
                    player.muzzleX = bullet.pos.x + 7 * xDir - 8;
                    player.muzzleY = bullet.pos.y + 5 * yDir - 8 + 1;
                }

                currentShootDelay = shootDelay;
                readyToShoot = false;
                // TODO: play sound
            }

        }

        public Bullet GetAmmo(double xDir, double yDir)
        {
            return new BulletCannonball(owner, xDir, yDir, BULLET_DAMAGE); ;
        }

        public void WeaponTick()
        {
            if (!readyToShoot)
            {
                if (currentShootDelay > 0)
                {
                    currentShootDelay--;
                }
                else
                {
                    readyToShoot = true;
                }
            }
        }

        private double GetBulletDirection(double accuracy)
        {
            return System.Math.Atan2(owner.aimVector.y, owner.aimVector.x) + (new Random().NextDouble() - new Random().NextDouble()) * accuracy;
        }

        private void ApplyImpuls(double xDir, double yDir, double strength)
        {
            owner.xd -= xDir * strength;
            owner.yd -= yDir * strength;
        }

        public void SetOwner(Mob.Mob mob)
        {
            owner = mob;
        }

        public IAbstractBitmap GetSprite()
        {
            return null; // TODO: add...
        }
    }
}
