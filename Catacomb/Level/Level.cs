using Catacomb.Entity;
using Catacomb.Entity.Mob;
using Catacomb.Entity.Predicates;
using Catacomb.GUI;
using Catacomb.Level.GameMode;
using Catacomb.Level.Tile;
using Catacomb.Math;
using Catacomb.Screen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Catacomb.Level
{
    public class Level
    {
        private LinkedList<Tile.Tile>[] tiles;
        private List<Vec2> spawnPointsP1;
        private List<Vec2> spawnPointsP2;
        private IAbstractBitmap minimap;
        private bool largeMap = false, smallMap = false;
        private bool[] seen;

        static int[] neighborOffsets;

        public int TARGET_SCORE = 100;
        public int width, height;
        public List<Entity.Entity>[] entityMap;
        public List<Entity.Entity> entities = new List<Entity.Entity>();
        public List<ILevelTickItem> tickItems = new List<ILevelTickItem>();
        public int maxMonsters;

        public int[,] monsterDensity;
        public int densityTileWidth = 5;
        public int densityTileHeight = 5;

        public IVictoryConditions victoryConditions;
        public int player1Score = 0;
        public int player2Score = 0;

        public Level(int width, int height)
        {
            neighborOffsets = [-1, 1, -width, -width + 1, -width - 1, width, width + 1, width - 1];
            this.width = width;
            this.height = height;

            int denseTileArrayWidth;
            int denseTileArrayHeight;

            if (width % 3 == 0)
            {
                denseTileArrayWidth = width / densityTileWidth;
            }
            else
            {
                denseTileArrayWidth = width / densityTileWidth + 1;
            }

            if (height % 3 == 0)
            {
                denseTileArrayHeight = height / densityTileHeight;
            }
            else
            {
                denseTileArrayHeight = height / densityTileHeight + 1;
            }

            monsterDensity = new int[denseTileArrayWidth, denseTileArrayHeight];

            // TODO: minimap

            largeMap = height > 64 || width > 64;
            smallMap = height < 64 && width < 64;

            InitializeTileMap();

            spawnPointsP1 = [];
            spawnPointsP2 = [];

            entityMap = new List<Entity.Entity>[width * height];
            for (int i = 0; i < width * height; i++)
            {
                entityMap[i] = [];
            }

            SetSeen(new bool[(width + 1) * (height + 1)]);
        }

        private void InitializeTileMap()
        {
            tiles = new LinkedList<Tile.Tile>[width * height];

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    tiles[x + y * width] = new LinkedList<Tile.Tile>();
                }
            }

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    SetTile(x, y, new FloorTile());
                }
            }
        }

        public void SetTile(int x, int y, Tile.Tile tile)
        {
            tiles[x + y * width].AddLast(tile);
            tile.Init();

            UpdateTiles(x, y, tile);

            if (tile is AnimatedTile)
            {
                tickItems.Add((ILevelTickItem)tile);
            }
        }

        public Tile.Tile GetTile(int x, int y)
        {
            if (x < 0 || y < 0 || x >= width || y >= height)
            {
                return null;
            }

            return tiles[x + y * width].Last();
        }

        public Tile.Tile GetTile(Vec2 pos)
        {
            int x = (int)pos.x / Tile.Tile.WIDTH;
            int y = (int)pos.y / Tile.Tile.HEIGHT;

            return GetTile(x, y);
        }

        public void RemoveTile(int x, int y)
        {
            int index = x + y * width;

            if (tiles[index].Count > 1)
            {
                tiles[index].RemoveLast();
                UpdateTiles(x, y, tiles[index].Last());
            }
        }

        public void UpdateTiles(int x, int y, Tile.Tile tile)
        {
            foreach (int offset in neighborOffsets)
            {
                int nbIndex = x + (y * width) + offset;

                if (nbIndex > 0 && nbIndex < width * height)
                {
                    Tile.Tile neighbor = tiles[nbIndex].Last();

                    if (neighbor != null)
                    {
                        neighbor.NeighborChanged();
                    }
                }
            }
        }

        public void AddSpawnPoint(int x, int y, int team)
        {
            if (team == Team.Team1)
            {
                spawnPointsP1.Add(new Vec2(x, y));
            }
            else if (team == Team.Team2)
            {
                spawnPointsP2.Add(new Vec2(x, y));
            }
            else
            {
                return;
            }
        }

        public Vec2 GetRandomSpawnPoint(int team)
        {
            int index;

            if (team == Team.Team1)
            {
                index = new Random().Next(spawnPointsP1.Count - 1); // TODO: network synced random
                return spawnPointsP1[index];
            }
            else if (team == Team.Team2)
            {
                index = new Random().Next(spawnPointsP2.Count - 1); // TODO: network synced random
                return spawnPointsP2[index];
            }
            else
            {
                return null;
            }
        }

        public bool CanPlayerSpawn()
        {
            return spawnPointsP1.Count > 0 && spawnPointsP2.Count > 0;
        }

        public void InsertToEntityMap(Entity.Entity e)
        {
            e.xto = (int)(e.pos.x - e.radius.x) / Tile.Tile.WIDTH;
            e.yto = (int)(e.pos.y - e.radius.y) / Tile.Tile.HEIGHT;

            int x1 = e.xto + (int)(e.radius.x * 2 + 1) / Tile.Tile.WIDTH;
            int y1 = e.yto + (int)(e.radius.y * 2 + 1) / Tile.Tile.HEIGHT;

            for (int y = e.yto; y <= y1; y++)
            {
                if (y < 0 || y >= height)
                {
                    continue;
                }

                for (int x = e.xto; x <= x1; x++)
                {
                    if (x < 0 || x >= width)
                    {
                        continue;
                    }

                    entityMap[x + y * width].Add(e);
                }
            }
        }


        public void RemoveFromEntityMap(Entity.Entity e)
        {
            int x1 = e.xto + (int)(e.radius.x * 2 + 1) / Tile.Tile.WIDTH;
            int y1 = e.yto + (int)(e.radius.y * 2 + 1) / Tile.Tile.HEIGHT;

            for (int y = e.yto; y <= y1; y++)
            {
                if (y < 0 || y >= height)
                {
                    continue;
                }

                for (int x = e.xto; x <= x1; x++)
                {
                    if (x < 0 || x >= width)
                    {
                        continue;
                    }

                    entityMap[x + y * width].Remove(e);
                }
            }
        }

        public List<Entity.Entity> GetEntities(BB bb)
        {
            return GetEntities(bb.x0, bb.y0, bb.x1, bb.y1);
        }

        public List<Entity.Entity> GetEntities(double x0, double y0, double x1, double y1)
        {
            return GetEntities(x0, y0, x1, y1, EntityIntersectsBB.Instance);
        }

        public List<Entity.Entity> GetEntities(BB bb, Entity.Entity c)
        {
            return GetEntities(bb.x0, bb.y0, bb.x1, bb.y1, c);
        }

        public List<Entity.Entity> GetEntities(double x0, double y0, double x1, double y1, Entity.Entity c)
        {
            return GetEntities(x0, y0, x1, y1, new EntityIntersectsBBAndInstanceOf(c.GetType()));
        }

        public List<Entity.Entity> GetEntities(double xx0, double yy0, double xx1, double yy1, IBBPredicate<Entity.Entity> predicate)
        {
            int x0 = System.Math.Max((int)(xx0) / Tile.Tile.WIDTH, 0);
            int x1 = System.Math.Min((int)(xx1) / Tile.Tile.WIDTH, width - 1);
            int y0 = System.Math.Max((int)(yy0) / Tile.Tile.HEIGHT, 0);
            int y1 = System.Math.Min((int)(yy1) / Tile.Tile.HEIGHT, height - 1);

            List<Entity.Entity> result = [];

            for (int y = y0; y <= y1; y++)
            {
                for (int x = x0; x <= x1; x++)
                {
                    foreach (Entity.Entity e in entityMap[x + y * width])
                    {
                        if (predicate.AppliesTo(e, xx0, yy0, xx1, yy1))
                        {
                            result.Add(e);
                        }
                    }
                }
            }

            result.Sort(new EntityComparator());

            return result;
        }

        public List<Entity.Entity> GetEntitiesSlower(double xx0, double yy0, double xx1, double yy1, Entity.Entity c)
        {
            List<Entity.Entity> result = [];
            IBBPredicate<Entity.Entity> predicate = new EntityIntersectsBBAndInstanceOf(c.GetType());

            foreach (Entity.Entity e in entities)
            {
                if (predicate.AppliesTo(e, xx0, yy0, xx1, yy1))
                {
                    result.Add(e);
                }
            }

            result.Sort(new EntityComparator());

            return result;
        }

        public void AddEntity(Entity.Entity e)
        {
            e.Init(this);
            entities.Add(e);
            InsertToEntityMap(e);
        }

        public void AddMob(Mob m, int xTile, int yTile)
        {
            UpdateDensityList();

            if (monsterDensity[(int)(xTile / densityTileWidth), (int)(yTile / densityTileHeight)] < TitleMenu.difficulty.getAllowedMobDensity())
            {
                AddEntity(m);
            }
        }

        public void RemoveEntity(Entity.Entity e)
        {
            e.removed = true;
        }

        public void UpdateDensityList()
        {
            for (int x = 0; x < monsterDensity.GetLength(0); x++)
            {
                for (int y = 0; y < monsterDensity.GetLength(1); y++)
                {
                    int entityNumb = GetEntities(x * densityTileWidth * Tile.Tile.WIDTH, y * densityTileHeight * Tile.Tile.HEIGHT, x * (densityTileWidth + 1) * Tile.Tile.WIDTH, y * (densityTileHeight + 1) * Tile.Tile.HEIGHT).Size();
                    monsterDensity[x, y] = entityNumb;
                }
            }
        }

        public void Tick()
        {
            for (int i = 0; i < tickItems.Count; i++)
            {
                tickItems[i].Tick(this);
            }

            for (int i = 0; i < entities.Count; i++)
            {
                Entity.Entity e = entities[i];

                if (!e.removed)
                {
                    e.Tick();

                    int xtn = (int)(e.pos.x - e.radius.x) / Tile.Tile.WIDTH;
                    int ytn = (int)(e.pos.y - e.radius.y) / Tile.Tile.HEIGHT;

                    if (xtn != e.xto || ytn != e.yto)
                    {
                        RemoveFromEntityMap(e);
                        InsertToEntityMap(e);
                    }
                }

                if (e.removed)
                {
                    entities.RemoveAt(i--);
                    RemoveFromEntityMap(e);
                }
            }

            if (victoryConditions != null)
            {
                victoryConditions.UpdateVictoryConditions(this);
            }

            Notifications.GetInstance().Tick();
        }

        private bool HasSeen(int x, int y)
        {
            return GetSeen()[x + y * (width + 1)] || GetSeen()[(x + 1) + y * (width + 1)]
                || GetSeen()[x + (y + 1) * (width + 1)]
                || GetSeen()[(x + 1) + (y + 1) * (width + 1)];
        }

        public void Render(IAbstractScreen screen, int xScroll, int yScroll)
        {
            // TODO: draw to screen
        }

        private void RenderTiles(IAbstractScreen screen, int x0, int y0, int x1, int y1)
        {
            // TODO: render to screen
        }

        private GameCharacter GetPlayerCharacter(int playerID)
        {
            // TODO: implement lol
        }

        private void RenderTopOfWalls(IAbstractScreen screen, int x0, int y0, int x1, int y1)
        {
            // TODO: render to screen
        }

        private void RenderDarkness(IAbstractScreen screen, int x0, int y0, int x1, int y1)
        {
            // TODO: render to screen
        }

        private void UpdateMinimap()
        {
            // TODO: implement lol
        }

        private void AddIconsToMinimap()
        {
            // TODO: implement lol
        }

        private void RenderPanelAndMinimap(IAbstractScreen screen, int x0, int y0)
        {
            // TODO: render to screen
        }

        private IAbstractBitmap CalculateSmallMapDisplay()
        {
            // TODO: implement lol.
        }

        private void renderPlayerScores(IAbstractScreen screen)
        {
            // TODO: render to screen
        }

        private bool CanSee(int x, int y)
        {
            if (x < 0 || y < 1 || x >= width || y >= height)
            {
                return true;
            }
            return GetSeen()[x + (y - 1) * (width + 1)]
                    || GetSeen()[(x + 1) + (y - 1) * (width + 1)]
                    || GetSeen()[x + y * (width + 1)] || GetSeen()[(x + 1) + y * (width + 1)]
                    || GetSeen()[x + (y + 1) * (width + 1)]
                    || GetSeen()[(x + 1) + (y + 1) * (width + 1)];
        }

        public List<BB> GetClipBBs(Entity.Entity e)
        {
            List<BB> result = [];
            BB bb = e.GetBB().Grow(Tile.Tile.WIDTH);

            int x0 = (int)(bb.x0 / Tile.Tile.WIDTH);
            int x1 = (int)(bb.x1 / Tile.Tile.WIDTH);
            int y0 = (int)(bb.y0 / Tile.Tile.HEIGHT);
            int y1 = (int)(bb.y1 / Tile.Tile.HEIGHT);

            result.Add(new BB(null, 0, 0, 0, height * Tile.Tile.HEIGHT));
            result.Add(new BB(null, 0, 0, width * Tile.Tile.WIDTH, 0));
            result.Add(new BB(null, width * Tile.Tile.WIDTH, 0, width * Tile.Tile.WIDTH,
                    height * Tile.Tile.HEIGHT));
            result.Add(new BB(null, 0, height * Tile.Tile.HEIGHT, width * Tile.Tile.WIDTH,
                    height * Tile.Tile.HEIGHT));

            for (int y = y0; y <= y1; y++)
            {
                if (y < 0 || y >= height)
                {
                    continue;
                }

                for (int x = x0; x <= x1; x++)
                {
                    if (x < 0 || x >= width)
                    {
                        continue;
                    }

                    GetTile(x, y).AddClipBBs(result, e);
                }
            }

            List<Entity.Entity> visibleEntities = GetEntities(bb);
            foreach (Entity.Entity ee in visibleEntities)
            {
                if (ee != e && ee.Blocks(e))
                {
                    result.Add(ee.GetBB());
                }
            }

            return result;
        }

        public void Reveal(int x, int y, int radius)
        {
            for (int i = 0; i < radius * 2 + 1; i++)
            {
                RevealLine(x, y, x - radius + i, y - radius, radius);
                RevealLine(x, y, x - radius + i, y + radius, radius);
                RevealLine(x, y, x - radius, y - radius + i, radius);
                RevealLine(x, y, x + radius, y - radius + i, radius);
            }
        }

        private void RevealLine(int x0, int y0, int x1, int y1, int radius)
        {
            for (int i = 0; i <= radius; i++)
            {
                int xx = x0 + (x1 - x0) * i / radius;
                int yy = y0 + (y1 - y0) * i / radius;

                if (xx < 0 || yy < 0 || xx >= width || yy >= height)
                {
                    return;
                }

                int xd = xx - x0;
                int yd = yy - y0;

                if (xd * xd + yd * yd > radius * radius)
                {
                    return;
                }

                Tile.Tile tile = GetTile(xx, yy);

                if (tile is WallTile)
                {
                    return;
                }

                GetSeen()[xx + yy * (width + 1)] = true;
                GetSeen()[(xx + 1) + yy * (width + 1)] = true;
                GetSeen()[xx + (yy + 1) * (width + 1)] = true;
                GetSeen()[(xx + 1) + (yy + 1) * (width + 1)] = true;
            }
        }

        public void PlaceTile(int x, int y, Tile.Tile tile, Player player)
        {
            if (!GetTile(x, y).IsBuildable())
            {
                return;
            }
            if (player != null)
            {
                SetTile(x, y, tile);
            }
        }
        public int CountEntities<T>(T entityType)
        {
            int count = 0;

            foreach (var entity in entities)
            {
                if (entity is T)
                {
                    count++;
                }
            }

            return count;
        }

        public bool[] GetSeen()
        {
            return seen;
        }

        public void SetSeen(bool[] seen)
        {
            this.seen = seen;
        }

        public bool checkLineOfSight(Entity.Entity eSource, Entity.Entity eTarget)
        {

            return CheckLineOfSight(eSource, eTarget.pos);
        }

        public bool CheckLineOfSight(Entity.Entity eSource, Vec2 vTarget)
        {
            Vec2 tP;
            tP = GetTileFromPosition(eSource.pos);
            int x1 = (int)tP.x;
            int y1 = (int)tP.y;

            tP = GetTileFromPosition(vTarget);
            int x2 = (int)tP.x;
            int y2 = (int)tP.y;

            if (x1 >= width || x1 <= 0 || x2 >= width || x2 <= 0 || y1 >= height
                    || y1 <= 0 || y2 >= height || y2 <= 0)
            {
                return false;
            }

            int dx = System.Math.abs(x2 - x1);
            int dy = System.Math.abs(y2 - y1);
            int sx = -1;
            int sy = -1;

            if (x1 < x2)
            {
                sx = 1;
            }

            if (y1 < y2)
            {
                sy = 1;
            }

            int dff = dx - dy;
            int d2;

            if (!GetTile(x1, y1).CanPass(eSource))
            {
                return false;
            }

            do
            {
                if (x1 == x2 && y1 == y2)
                {
                    break;
                }

                d2 = 2 * dff;

                if (d2 > -dy)
                {
                    dff -= dy;
                    x1 += sx;
                    if (!GetTile(x1, y1).CanPass(eSource))
                    {
                        return false;
                    }
                }

                if (d2 < dx)
                {
                    dff += dx;
                    y1 += sy;
                    if (!GetTile(x1, y1).CanPass(eSource))
                    {
                        return false;
                    }
                }
            } while (true);

            return true;
        }

        public Vec2 GetTileFromPosition(Vec2 pos)
        {
            int x = (int)pos.x / Tile.Tile.WIDTH;
            int y = (int)pos.y / Tile.Tile.HEIGHT;
            return new Vec2(x, y);
        }

        public Vec2 GetPositionFromTile(int x, int y)
        {
            return new Vec2(x * Tile.Tile.WIDTH + (Tile.Tile.WIDTH / 2), y * Tile.Tile.HEIGHT
                    + (Tile.Tile.HEIGHT / 2));
        }
    }
}
