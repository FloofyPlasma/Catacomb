using Catacomb.Entity.Animation;
using Catacomb.Math;
using Catacomb.Screen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catacomb.Entity
{
    public abstract class Entity : IBBOwner
    {
        public Level.Level level;
        public bool removed;
        public Vec2 pos = new Vec2(0, 0);
        public Vec2 radius = new Vec2(10, 10);

        public bool isBlocking = true;
        public bool physicsSlide = false;

        public int xto;
        public int yto;
        public double xd, yd;
        public int minimapIcon = -1;
        public int minimapColor = -1;
        public int team;

        private Entity spawnSource;

        public void SetPos(double x, double y)
        {
            pos.Set(x, y);
        }

        public void SetPos(Vec2 pos)
        {
            pos.Set(pos.x, pos.y);
        }

        public void SetSize(int xr, int yr)
        {
            radius.Set(xr, yr);
        }

        public void Init(Level.Level level)
        {
            this.level = level;
            Init();
        }

        public void Init()
        {

        }

        public virtual void Tick()
        {

        }

        public bool Intersects(double xx0, double yy0, double xx1, double yy1)
        {
            return GetBB().Intersects(xx0, yy0, xx1, yy1);
        }

        public BB GetBB()
        {
            return new BB(this, pos.x - radius.x, pos.y - radius.y, pos.x + radius.x, pos.y + radius.y);
        }

        public virtual void Render(IAbstractScreen screen)
        {
            // TODO: render to screen
        }

        public void RenderTop(IAbstractScreen screen)
        {
            // TODO: render to screen
        }

        protected bool Move(double xa, double ya)
        {
            List<BB> bbs = level.GetClipBBs(this);

            if (physicsSlide || (xa == 0 || ya == 0))
            {
                bool moved = false;

                if (!removed)
                {
                    moved |= PartMove(bbs, xa, 0);
                }

                if (!removed)
                {
                    moved |= PartMove(bbs, 0, ya);
                }

                return moved;
            }
            else
            {
                bool moved = true;

                if (!removed)
                {
                    moved &= PartMove(bbs, xa, 0);
                }

                if (!removed)
                {
                    moved &= PartMove(bbs, 0, ya);
                }

                return moved;
            }
        }

        private bool PartMove(List<BB> bbs, double xa, double ya)
        {
            double oxa = xa;
            double oya = ya;
            BB from = GetBB();

            BB closest = null;
            double epsilon = 0.01;

            for (int i = 0; i < bbs.Count; i++)
            {
                BB to = bbs[i];

                if (from.Intersects(to))
                {
                    continue;
                }

                if (ya == 0)
                {
                    if (to.y0 >= from.y1 || to.y1 <= from.y0)
                    {
                        continue;
                    }

                    if (xa > 0)
                    {
                        double xrd = to.x0 - from.x1;

                        if (xrd >= 0 && xa > xrd)
                        {
                            closest = to;
                            xa = xrd - epsilon;

                            if (xa < 0)
                            {
                                xa = 0;
                            }
                        }
                    }
                    else if (xa < 0)
                    {
                        double xld = to.x1 - from.x0;

                        if (xld <= 0 && xa < xld)
                        {
                            closest = to;
                            xa = xld + epsilon;

                            if (xa > 0)
                            {
                                xa = 0;
                            }
                        }
                    }
                }

                if (xa == 0)
                {
                    if (to.x0 >= from.x1 || to.x1 <= from.x0)
                    {
                        continue;
                    }

                    if (ya > 0)
                    {
                        double yrd = to.y0 - from.y1;

                        if (yrd >= 0 && ya > yrd)
                        {
                            closest = to;
                            ya = yrd - epsilon;

                            if (ya < 0)
                            {
                                ya = 0;
                            }
                        }
                    }
                    else if (ya < 0)
                    {
                        double yld = to.y1 - from.y0;

                        if (yld <= 0 && ya < yld)
                        {
                            closest = to;
                            ya = yld + epsilon;

                            if (ya > 0)
                            {
                                ya = 0;
                            }
                        }
                    }
                }
            }

            if (closest != null && closest.owner != null)
            {
                closest.owner.HandleCollision(this, oxa, oya);
            }

            if (xa != 0 || ya != 0)
            {
                pos.AddSelf(xa, ya);

                return true;
            }

            return false;
        }

        public bool Blocks(Entity e)
        {
            return isBlocking && e.isBlocking && ShouldBlock(e) && e.ShouldBlock(this);
        }

        protected virtual bool ShouldBlock(Entity e)
        {
            return true;
        }

        public virtual void Remove()
        {
            removed = true;

            if (spawnSource != null && spawnSource is IRemoveEntityNotify)
            {
                ((IRemoveEntityNotify)spawnSource).RemoveEntityNotice(this);
            }
        }

        public virtual void HandleCollision(Entity entity, double xa, double ya)
        {
            if (this.Blocks(entity))
            {
                this.Collide(entity, xa, ya);
                entity.Collide(this, -xa, -ya);
            }
        }

        public virtual void Collide(Entity entity, double xa, double ya)
        {

        }

        public void Hurt(Bullet bullet)
        {

        }

        public void Bomb(LargeBombExplodeAnimation largeBombExplodeAnimation)
        {

        }

        public Entity GetSpawnSource()
        {
            return spawnSource;
        }

        public void SetSpawnSource(Entity spawnSource)
        {
            this.spawnSource = spawnSource;
        }
    }
}
