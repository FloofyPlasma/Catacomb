using Catacomb.Entity.Building;
using Catacomb.Entity.Mob;
using Catacomb.Screen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catacomb.Entity
{
    public class Bullet : Entity
    {
        public double xa, ya;
        public Mob.Mob owner;
        bool hit = false;

        public int duration;
        protected int facing;
        protected float damage;
        public int freezeTime = 5;
        
        public Bullet(Mob.Mob e, double xa, double ya, float damage)
        {
            owner = e;
            pos.Set(e.pos.x + xa * 4, e.pos.y + ya * 4);
            this.xa = xa * 6;
            this.ya = ya * 6;
            SetSize(4, 4);
            physicsSlide = false;
            duration = 40;
            double angle = (System.Math.Atan2(ya, xa) + System.Math.PI * 1.625);
            facing = (8 + (int) (angle / System.Math.PI * 4)) & 7;
            this.damage = damage;
        }

        override public void Tick()
        {
            if (--duration <= 0)
            {
                Remove();
                
                return;
            }

            if (!Move(xa, ya))
            {
                hit = true;
            }

            if (hit && !removed)
            {
                Remove();
            }
        }

        override protected bool ShouldBlock(Entity e)
        {
            if (e is Bullet)
            {
                return false;
            } 
            
            if ((e is Mob.Mob) && !(e is RailDroid) && !((Mob.Mob) e).IsNotFriendOf(owner))
            {
                return false;
            }

            return e != owner;
        }

        override public void Render(IAbstractScreen screen)
        {
            // TODO: Render to screen
        }

        public override void Collide(Entity entity, double xa, double ya)
        {
            if (entity is Mob.Mob)
            {
                Mob.Mob mobEnt = (Mob.Mob) entity;

                if (entity is Bomb)
                {
                    ((Bomb)entity).Hit();
                }
                else if (mobEnt.IsNotFriendOf(owner) || (entity is RailDroid))
                {
                    mobEnt.Hurt(this, damage);
                    hit = true;
                }
            }
            else
            {
                entity.Hurt(this);
                hit = true;
            }

            if (hit)
            {
                // TODO: Play sound
            }
        }
    }
}
